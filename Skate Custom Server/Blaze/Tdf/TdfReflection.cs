using Blaze.Tdf.Attributes;
using Blaze.Tdf.Types;
using System.Collections;
using System.Reflection;

namespace Blaze
{
    // Class used for converting BlazeMessage objects to structs
    public static class TdfReflection
    {
        static bool IsUnionType(Type type) =>
        type.GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Any(f => f.GetCustomAttribute<TdfUnionCase>() != null);

        public static TdfType CLRTypeToTdfType(Type type)
        {
            if (type == typeof(byte[])) return TdfType.Blob;
            if (type == typeof(string)) return TdfType.String;
            if (type == typeof(bool)) return TdfType.Int8;
            if (type == typeof(byte)) return TdfType.UInt8;
            if (type == typeof(sbyte)) return TdfType.Int8;
            if (type == typeof(short)) return TdfType.Int16;
            if (type == typeof(ushort)) return TdfType.UInt16;
            if (type == typeof(int)) return TdfType.Int32;
            if (type == typeof(uint)) return TdfType.UInt32;
            if (type == typeof(long)) return TdfType.Int64;
            if (type == typeof(ulong)) return TdfType.UInt64;

            if (typeof(IDictionary).IsAssignableFrom(type))
                return TdfType.Map;

            if (typeof(IList).IsAssignableFrom(type))
                return TdfType.Array;

            // Check if struct is a union
            if (IsUnionType(type)) return TdfType.Union;    

            return TdfType.Struct;
        }

        public static TdfUnion ToTdfUnion(object val)
        {
            TdfUnion res = new TdfUnion();
            res.SelectedUnionValue = 0x7F;

            foreach (var field in val.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = field.GetValue(val);
                if (value != null)
                {
                    res.SelectedUnionValue = field.GetCustomAttribute<TdfUnionCase>().Selector;
                    res.Field = new TdfField("", TdfType.Struct, ToTdfStruct(value));
                    return res;
                }
            }

            return res;
        }

        public static TdfDictionary ToTdfDictionary(object val)
        {
            // declaredType is always Dictionary<TKey, TValue>
            Type declaredType = val.GetType();
            var args = declaredType.GetGenericArguments();
            var keyClrType = args[0];
            var valClrType = args[1];

            var keyTdfType = CLRTypeToTdfType(keyClrType);
            var valueTdfType = CLRTypeToTdfType(valClrType);

            var dict = (IDictionary)val; // Dictionary<,> supports this
            var tdfDict = new TdfDictionary();
            tdfDict.KeyType = keyTdfType;
            tdfDict.ValueType = valueTdfType;

            foreach (DictionaryEntry kv in dict)
            {
                tdfDict.Map.Add(
                    ToTdfValue(keyTdfType, kv.Key),
                    ToTdfValue(valueTdfType, kv.Value)
                );
            }

            return tdfDict;
        }

        public static TdfArray ToTdfArray(object val)
        {
            TdfArray tdfArr = new TdfArray();

            var list = (IList)val;
            if (list.Count > 0)
            {
                TdfType elemType = TdfType.Struct;

                var first = list[0];
                if (first != null)
                    elemType = CLRTypeToTdfType(first.GetType());
                tdfArr.ArrayType = elemType;    

                foreach (var item in list)
                {
                    var tdfVal = ToTdfValue(elemType, item);
                    tdfArr.Items.Add(tdfVal);
                }
            }

            return tdfArr;
        }

        public static byte BoolToByte(bool val)
        {
            return val ? (byte)1 : (byte)0;
        }

        public static object ToTdfValue(TdfType type, object val) => type switch
        {
            TdfType.Struct => ToTdfStruct(val),
            TdfType.Union => ToTdfUnion(val),
            TdfType.Map => ToTdfDictionary(val),
            TdfType.Array => ToTdfArray(val),
            TdfType.Int8 => BoolToByte((bool)val),
            _=> val
        };

        public static TdfStruct ToTdfStruct(object val)
        {
            TdfStruct result = new TdfStruct();
            foreach (var field in val.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                TdfType tdfType = CLRTypeToTdfType(field.FieldType);

                var attribute = field.GetCustomAttribute<TdfMember>();

                if (attribute == null)
                    continue;

                object value = field.GetValue(val);
                result.Add(attribute.Tag, tdfType, ToTdfValue(tdfType, value));
            }

            return result;
        }

        public static object TdfValueToCLRValue(TdfType tdfType, object tdfValue, Type fieldType)
        {
            if (tdfType == TdfType.Struct)
            {
                object value = TdfStructToModel((TdfStruct)tdfValue, fieldType);

                return value;
            }
            else if (tdfType == TdfType.Map)
            {
                // Create dictionary somehow here based on the FieldInfo Dictionary<TKey, TValue>
                IDictionary newDict = (IDictionary)Activator.CreateInstance(fieldType);
                TdfDictionary tdfDict = (TdfDictionary)tdfValue;

                Type[] types = fieldType.GetGenericArguments();

                foreach (var kv in tdfDict.Map)
                {
                    object key = TdfValueToCLRValue(tdfDict.KeyType, kv.Key, types[0]);
                    object val = TdfValueToCLRValue(tdfDict.ValueType, kv.Value, types[1]);

                    newDict.Add(key, val);
                }

                return newDict;
            }
            else if (tdfType == TdfType.Array)
            {
                // Create dictionary somehow here based on the FieldInfo Dictionary<TKey, TValue>
                IList newArr = (IList)Activator.CreateInstance(fieldType);
                TdfArray tdfArr = (TdfArray)tdfValue;

                Type[] types = fieldType.GetGenericArguments();

                foreach (var item in tdfArr.Items)
                {
                    object newVal = TdfValueToCLRValue(tdfArr.ArrayType, item, types[0]);

                    newArr.Add(newVal);
                }

                return newArr;
            }
            else if (tdfType == TdfType.Union)
            {
                TdfUnion tdfUnion = (TdfUnion)tdfValue;
                byte selectedUnion = tdfUnion.SelectedUnionValue;

                object value = Activator.CreateInstance(fieldType);
                if (selectedUnion == 0x7f)
                    return value;

                foreach (var field in fieldType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    TdfUnionCase unionCase = field.GetCustomAttribute<TdfUnionCase>();
                    if (unionCase.Selector == selectedUnion)
                    {
                        object structValue = TdfStructToModel((TdfStruct)tdfUnion.Field.Value, field.FieldType);
                        field.SetValue(value, structValue);
                    }
                }
                return value;
            }
            else if (tdfType == TdfType.Int8) // Int8 is used as bool in blaze servers
            {
                return ((byte)tdfValue) == 1;
            }
            else
            {
                return tdfValue;
            }

            throw new NotSupportedException($"TdfType {tdfValue} is not supported yet.");
        }

        public static object TdfStructToModel(TdfStruct tdfStruct, Type type)
        {
            Type actualType = Nullable.GetUnderlyingType(type) ?? type;
            object model = Activator.CreateInstance(actualType)!;

            var fieldDict = tdfStruct.Fields.ToDictionary(f => f.Tag);

            foreach (var field in model.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var attribute = field.GetCustomAttribute<TdfMember>();
                if (attribute == null)
                    continue;

                if (fieldDict.TryGetValue(attribute.Tag, out var fieldEquivalent))
                {
                    field.SetValue(model, TdfValueToCLRValue(
                        fieldEquivalent.Type, fieldEquivalent.Value, field.FieldType));
                }
            }

            return model;
        }

    }
}
