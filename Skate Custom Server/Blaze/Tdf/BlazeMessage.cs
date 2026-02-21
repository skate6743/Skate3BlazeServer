using Blaze.Tdf.Types;
using Blaze.MessageLists;
using System.Text;

namespace Blaze
{
    public enum TdfType : byte
    {
        Struct = 0x0,
        String = 0x1,
        Int8 = 0x2,
        UInt8 = 0x3,
        Int16 = 0x4,
        UInt16 = 0x5,
        Int32 = 0x6,
        UInt32 = 0x7,
        Int64 = 0x8,
        UInt64 = 0x9,
        Array = 0xA,
        Blob = 0xB,
        Map = 0xC,
        Union = 0xD,
        Unknown = 0xFF
    }

    public enum BlazeMessageType : byte
    {
        Request = 0x00,
        Response = 0x10,
        Notification = 0x20,
        Error = 0x30
    }

    public class TdfField
    {
        public string Tag { get; set; }
        public TdfType Type { get; set; }
        public object Value { get; set; }

        public TdfField(string tag, TdfType type, object value)
        {
            Tag = tag;
            Type = type;
            Value = value;
        }

        public byte[] Serialize()
        {
            var result = new List<byte>(64);
            result.AddRange(TdfUtils.EncodeTdfTag(Tag));

            byte[] encoded = TdfCodec.SerializeValue(Type, Value);

            byte[] header = Type == TdfType.Map
                ? TdfCodec.BuildMapHeader((TdfDictionary)Value)
                : TdfUtils.BuildTdfHeader(Type, encoded.Length);

            result.AddRange(header);
            result.AddRange(encoded);
            return result.ToArray();
        }

        public static object DeserializePrimitiveByType(TdfType type, byte[] bytes)
        {
            return TdfCodec.DeserializePrimitive(type, bytes);
        }

        public static byte[] SerializePrimitiveByType(TdfType type, object value)
        {
            return TdfCodec.SerializePrimitive(type, value);
        }
    }

    public static class TdfCodec
    {
        // Primitive size lookup
        private static int GetPrimitiveSize(TdfType type) => type switch
        {
            TdfType.Int8 or TdfType.UInt8 => 1,
            TdfType.Int16 or TdfType.UInt16 => 2,
            TdfType.Int32 or TdfType.UInt32 => 4,
            TdfType.Int64 or TdfType.UInt64 => 8,
            _ => 0,
        };

        public static byte[] SerializePrimitive(TdfType type, object value)
        {
            return type switch
            {
                TdfType.Struct => ((TdfStruct)value).Serialize(),
                TdfType.Int8 or TdfType.UInt8 => new[] { (byte)value },
                TdfType.Int16 => TdfUtils.SwapEndianess(BitConverter.GetBytes(Convert.ToInt16(value))),
                TdfType.UInt16 => TdfUtils.SwapEndianess(BitConverter.GetBytes(Convert.ToUInt16(value))),
                TdfType.Int32 => TdfUtils.SwapEndianess(BitConverter.GetBytes(Convert.ToInt32(value))),
                TdfType.UInt32 => TdfUtils.SwapEndianess(BitConverter.GetBytes(Convert.ToUInt32(value))),
                TdfType.Int64 => TdfUtils.SwapEndianess(BitConverter.GetBytes(Convert.ToInt64(value))),
                TdfType.UInt64 => TdfUtils.SwapEndianess(BitConverter.GetBytes(Convert.ToUInt64(value))),
                TdfType.String => SerializeString((string)value),
                TdfType.Blob => (byte[])value ?? Array.Empty<byte>(),
                _ => throw new NotSupportedException($"Primitive serialize not supported for type: {type}"),
            };
        }

        public static object DeserializePrimitive(TdfType type, byte[] bytes)
        {
            return type switch
            {
                TdfType.Int8 or TdfType.UInt8 => bytes[0],
                TdfType.Int16 => BitConverter.ToInt16(TdfUtils.SwapEndianess(bytes), 0),
                TdfType.UInt16 => BitConverter.ToUInt16(TdfUtils.SwapEndianess(bytes), 0),
                TdfType.Int32 => BitConverter.ToInt32(TdfUtils.SwapEndianess(bytes), 0),
                TdfType.UInt32 => BitConverter.ToUInt32(TdfUtils.SwapEndianess(bytes), 0),
                TdfType.Int64 => BitConverter.ToInt64(TdfUtils.SwapEndianess(bytes), 0),
                TdfType.UInt64 => BitConverter.ToUInt64(TdfUtils.SwapEndianess(bytes), 0),
                TdfType.String => Encoding.ASCII.GetString(bytes).Replace("\0", ""),
                TdfType.Blob => bytes,
                _ => throw new NotSupportedException($"Primitive deserialize not supported for type: {type}"),
            };
        }

        // Top level value serialization
        public static byte[] SerializeValue(TdfType type, object value)
        {
            return type switch
            {
                TdfType.Struct => ((TdfStruct)value).Serialize(),
                TdfType.Array => SerializeArray((TdfArray)value),
                TdfType.Map => SerializeDictionary((TdfDictionary)value),
                TdfType.Union => SerializeUnion((TdfUnion)value),
                _ => SerializePrimitive(type, value),
            };
        }

        private static byte[] SerializeString(string s)
        {
            s ??= string.Empty;
            int len = Encoding.ASCII.GetByteCount(s);
            var bytes = new byte[len + 1]; // null-terminated
            Encoding.ASCII.GetBytes(s, 0, s.Length, bytes, 0);
            return bytes;
        }

        // Dictionary serialization methods
        public static byte[] BuildMapHeader(TdfDictionary dict)
        {
            int count = dict?.Map?.Count ?? 0;

            if (count <= 0x0E)
                return new[] { (byte)(((byte)TdfType.Map << 4) | (count & 0x0F)) };

            byte[] vlq = TdfUtils.EncodeVlq(count).ToArray();
            var header = new byte[1 + vlq.Length];
            header[0] = (byte)(((byte)TdfType.Map << 4) | 0x0F);
            Buffer.BlockCopy(vlq, 0, header, 1, vlq.Length);
            return header;
        }

        private static byte BuildMapTypeDescriptor(TdfType type)
        {
            if (type == TdfType.String)
                return 0x1F;

            int size = GetPrimitiveSize(type);
            return (byte)(((byte)type << 4) | size);
        }

        private static void WriteMapValue(List<byte> bytes, TdfType type, object value)
        {
            switch (type)
            {
                case TdfType.Struct:
                    {
                        var st = (TdfStruct)value;
                        byte[] structBytes = st.Serialize();
                        bytes.AddRange(structBytes);
                        break;
                    }

                case TdfType.String:
                    {
                        byte[] data = SerializePrimitive(type, value);
                        if (data.Length <= 0x0E)
                            bytes.Add((byte)data.Length);
                        else
                            bytes.AddRange(TdfUtils.EncodeVlq(data.Length).ToArray());
                        bytes.AddRange(data);
                        break;
                    }

                case TdfType.Blob:
                    {
                        byte[] data = SerializePrimitive(type, value);
                        if (data.Length <= 0x0E)
                            bytes.Add((byte)data.Length);
                        else
                            bytes.AddRange(TdfUtils.EncodeVlq(data.Length).ToArray());
                        bytes.AddRange(data);
                        break;
                    }

                default:
                    {
                        byte[] data = SerializePrimitive(type, value);
                        bytes.AddRange(data);
                        break;
                    }
            }
        }

        private static byte[] SerializeDictionary(TdfDictionary dict)
        {
            if (dict?.Map == null || dict.Map.Count == 0)
                return Array.Empty<byte>();

            var bytes = new List<byte>(64);
            bytes.Add(BuildMapTypeDescriptor(dict.KeyType));

            bool first = true;
            foreach (var kv in dict.Map)
            {
                WriteMapValue(bytes, dict.KeyType, kv.Key);

                if (first)
                {
                    bytes.Add(BuildMapTypeDescriptor(dict.ValueType));
                    first = false;
                }

                WriteMapValue(bytes, dict.ValueType, kv.Value);
            }

            return bytes.ToArray();
        }

        // Array serialization methods
        private static byte GetElementLengthNibble(TdfType elemType)
        {
            int size = GetPrimitiveSize(elemType);
            if (size > 0) return (byte)size;

            return elemType switch
            {
                TdfType.String or TdfType.Struct or TdfType.Union
                    or TdfType.Blob or TdfType.Array or TdfType.Map => 0x0F,
                _ => 0x00,
            };
        }

        private static byte[] SerializeArray(TdfArray array)
        {
            var bytes = new List<byte>(64);

            if (array.Items.Count > 255)
                throw new NotSupportedException("Arrays with more than 255 elements are not supported.");

            bytes.Add((byte)array.Items.Count);

            TdfType elemType = array.ArrayType;
            byte elemLenNib = GetElementLengthNibble(elemType);
            byte typeAndLen = (byte)(((byte)elemType << 4) | (elemLenNib & 0x0F));

            if (elemType == TdfType.Union)
                bytes.Add(0xD0);
            else if (typeAndLen != 0x0F)
                bytes.Add(typeAndLen);
            else
                bytes.Add(0x00);

            for (int i = 0; i < array.Items.Count; i++)
            {
                SerializeArrayElement(bytes, elemType, array.Items[i]);
            }

            return bytes.ToArray();
        }

        private static void SerializeArrayElement(List<byte> bytes, TdfType elemType, object element)
        {
            switch (elemType)
            {
                case TdfType.Struct:
                    {
                        var st = element as TdfStruct
                            ?? throw new InvalidOperationException("Array struct element must be TdfStruct.");
                        bytes.AddRange(st.Serialize());
                        break;
                    }

                case TdfType.String:
                    {
                        string s = element as string
                            ?? throw new InvalidOperationException("Array string element must be a string.");

                        byte[] strBytes = Encoding.ASCII.GetBytes(s);
                        if (strBytes.Length > 255)
                            throw new NotSupportedException("Array string element too long (>255).");

                        bytes.Add((byte)strBytes.Length);

                        if (strBytes.Length > 0 && strBytes[^1] != 0)
                            bytes.Add(0);

                        bytes.AddRange(strBytes);
                        break;
                    }

                case TdfType.Union:
                    {
                        var union = element as TdfUnion
                            ?? throw new InvalidOperationException("Array union element must be TdfUnion.");
                        SerializeUnionBody(bytes, union);
                        break;
                    }

                case TdfType.Blob:
                    {
                        byte[] blobData = (byte[])element;
                        bytes.AddRange(TdfUtils.EncodeVlq(blobData.Length).ToArray());
                        bytes.AddRange(blobData);
                        break;
                    }

                case TdfType.Array:
                    {
                        var nestedArray = element as TdfArray
                            ?? throw new InvalidOperationException("Nested array element must be TdfArray.");
                        byte[] nestedBytes = SerializeArray(nestedArray);
                        bytes.AddRange(nestedBytes);
                        break;
                    }

                case TdfType.Map:
                    {
                        var nestedDict = element as TdfDictionary
                            ?? throw new InvalidOperationException("Nested map element must be TdfDictionary.");
                        byte[] mapHeader = BuildMapHeader(nestedDict);
                        byte[] mapBytes = SerializeDictionary(nestedDict);
                        bytes.AddRange(mapHeader);
                        bytes.AddRange(mapBytes);
                        break;
                    }

                default:
                    {
                        // Numeric primitives
                        bytes.AddRange(SerializePrimitive(elemType, element));
                        break;
                    }
            }
        }

        // Union serialization methods
        private static byte[] SerializeUnion(TdfUnion union)
        {
            var bytes = new List<byte>(32);
            SerializeUnionBody(bytes, union);
            return bytes.ToArray();
        }

        private static void SerializeUnionBody(List<byte> bytes, TdfUnion union)
        {
            bytes.Add(union.SelectedUnionValue);

            if (union.SelectedUnionValue == 0x7F)
                return;

            TdfField innerField = union.Field;
            bytes.AddRange(TdfUtils.EncodeTdfTag("VALU"));

            switch (innerField.Type)
            {
                case TdfType.Struct:
                    {
                        bytes.AddRange(TdfUtils.BuildTdfHeader(TdfType.Struct, 0));
                        var st = innerField.Value as TdfStruct
                            ?? throw new InvalidOperationException("Union struct value must be TdfStruct.");
                        bytes.AddRange(st.Serialize());
                        break;
                    }

                default:
                    {
                        // Serialize any non-struct union value (string, int, blob, etc.)
                        byte[] encoded = SerializeValue(innerField.Type, innerField.Value);
                        byte[] header = TdfUtils.BuildTdfHeader(innerField.Type, encoded.Length);
                        bytes.AddRange(header);
                        bytes.AddRange(encoded);
                        break;
                    }
            }
        }
    }

    // Main BlazeMessage class
    public class BlazeMessage
    {
        public List<TdfField> Fields { get; set; } = new();

        public ushort Component { get; set; }
        public ushort Command { get; set; }
        public ushort ErrorCode { get; set; }
        public byte MessageType { get; set; }
        public ushort MessageId { get; set; }

        public BlazeMessage() { }

        public BlazeMessage(byte[] packetBytes)
        {
            ushort bodyLength = ReadU16BE(packetBytes, 0x00);
            Component = ReadU16BE(packetBytes, 0x02);
            Command = ReadU16BE(packetBytes, 0x04);
            ErrorCode = ReadU16BE(packetBytes, 0x06);
            MessageType = packetBytes[0x08];
            MessageId = ReadU16BE(packetBytes, 0x0A);

            int i = 0x0C;
            int end = bodyLength + 0x0C;

            while (i < end)
            {
                var result = DeserializeTdfValue(packetBytes, i, inArray: false, expectedTypeAndLength: 0xFF);
                i += result.Length;
                Fields.Add(result.Field);
            }
        }

        public void Add(string tag, TdfType type, object value)
        {
            Fields.Add(new TdfField(tag, type, value));
        }

        public static BlazeMessage CreateResponseFromModel(byte[] packetBytes, object model)
        {
            var response = CreateResponseHeaderFrom(packetBytes);
            response.Fields.AddRange(TdfReflection.ToTdfStruct(model).Fields);
            return response;
        }

        public static T CreateModelFromRequest<T>(byte[] packetBytes) where T : new()
        {
            var message = new BlazeMessage(packetBytes);
            var temp = new TdfStruct();
            temp.Fields.AddRange(message.Fields);
            return (T)TdfReflection.TdfStructToModel(temp, typeof(T));
        }

        public static BlazeMessage CreateNotificationMessageBase(BlazeComponent component, ushort command)
        {
            return new BlazeMessage
            {
                Component = (ushort)component,
                Command = command,
                MessageType = (byte)BlazeMessageType.Notification,
                MessageId = 0,
                ErrorCode = 0,
            };
        }

        public static BlazeMessage CreateResponseHeaderFrom(byte[] requestPacket)
        {
            return new BlazeMessage
            {
                Component = TdfUtils.GetComponentFromPacket(requestPacket),
                Command = TdfUtils.GetCommandFromPacket(requestPacket),
                MessageType = (byte)BlazeMessageType.Response,
                MessageId = TdfUtils.GetMessageIDFromPacket(requestPacket),
                ErrorCode = 0,
            };
        }

        public byte[] Serialize()
        {
            var payload = new List<byte>(256);
            foreach (var field in Fields)
                payload.AddRange(field.Serialize());

            var packet = new List<byte>(0x0C + payload.Count);
            packet.AddRange(TdfUtils.SwapEndianess((ushort)payload.Count));
            packet.AddRange(TdfUtils.SwapEndianess(Component));
            packet.AddRange(TdfUtils.SwapEndianess(Command));
            packet.AddRange(TdfUtils.SwapEndianess(ErrorCode));
            packet.Add(MessageType);
            packet.Add(0x00);
            packet.AddRange(TdfUtils.SwapEndianess(MessageId));
            packet.AddRange(payload);

            return packet.ToArray();
        }

        private struct DeserializeResult
        {
            public int Length;
            public TdfField Field;
        }

        private static DeserializeResult DeserializeTdfValue(
            byte[] packetBytes, int index, bool inArray, byte expectedTypeAndLength)
        {
            int i = index;
            string tagName = "";

            if (!inArray)
            {
                tagName = TdfUtils.DecodeTdfTag(new[] { packetBytes[i], packetBytes[i + 1], packetBytes[i + 2] });
                i += 3;
            }

            int typeNib;
            int lenNib;
            int tdfValueLength;

            if (expectedTypeAndLength != 0xFF)
            {
                var split = TdfUtils.SplitByte(expectedTypeAndLength);
                typeNib = split.highPart;
                lenNib = split.lowPart;
                tdfValueLength = lenNib;

                if (typeNib == (int)TdfType.String)
                {
                    tdfValueLength = packetBytes[i];
                    i++;
                }
            }
            else
            {
                var split = TdfUtils.SplitByte(packetBytes[i]);
                typeNib = split.highPart;
                lenNib = split.lowPart;
                tdfValueLength = lenNib;
                i++;

                if (lenNib == 0x0F)
                {
                    var ms = new MemoryStream(packetBytes, i, Math.Min(10, packetBytes.Length - i), false);
                    var res = TdfUtils.ReadTdfLengthBigEndian(ms);
                    tdfValueLength = res.length;
                    i += res.bytesUsed;
                    ms.Close();
                }
            }

            var tdfType = (TdfType)typeNib;

            if (tdfType == TdfType.Union)
            {
                tdfValueLength = packetBytes[i];
                i += 4;
            }

            object val;

            switch (tdfType)
            {
                case TdfType.Struct:
                    (val, i) = DeserializeStruct(packetBytes, index, i, inArray);
                    tdfValueLength = 0;
                    break;

                case TdfType.String:
                    val = Encoding.ASCII.GetString(packetBytes, i, tdfValueLength).Replace("\0", "");
                    break;

                case TdfType.Int8:
                case TdfType.UInt8:
                    val = packetBytes[i];
                    break;

                case TdfType.Int16:
                case TdfType.UInt16:
                case TdfType.Int32:
                case TdfType.UInt32:
                case TdfType.Int64:
                case TdfType.UInt64:
                    val = TdfCodec.DeserializePrimitive(tdfType, CopyBytes(packetBytes, i, tdfValueLength));
                    break;

                case TdfType.Blob:
                    val = CopyBytes(packetBytes, i, tdfValueLength).Clone();
                    break;

                case TdfType.Array:
                    (val, i) = DeserializeArray(packetBytes, i);
                    tdfValueLength = 0;
                    break;

                case TdfType.Map:
                    (val, i) = DeserializeDictionary(packetBytes, i, tdfValueLength);
                    tdfValueLength = 0;
                    break;

                case TdfType.Union:
                    (val, i) = DeserializeUnion(packetBytes, i, tdfValueLength);
                    tdfValueLength = 0;
                    break;

                default:
                    throw new NotImplementedException($"TDF type not supported: {tdfType}");
            }

            i += tdfValueLength;

            return new DeserializeResult
            {
                Length = i - index,
                Field = new TdfField(tagName, tdfType, val),
            };
        }

        private static (TdfStruct value, int newOffset) DeserializeStruct(
            byte[] packetBytes, int originalIndex, int currentOffset, bool inArray)
        {
            var tdfStruct = new TdfStruct();

            int i = !inArray ? originalIndex + 4 : originalIndex + 1;

            int test = packetBytes[i];
            while (test != 0)
            {
                var inner = DeserializeTdfValue(packetBytes, i, inArray: false, expectedTypeAndLength: 0xFF);
                tdfStruct.Fields.Add(inner.Field);
                i += inner.Length;
                test = packetBytes[i];
            }

            i++; // skip struct terminator (0x00)
            return (tdfStruct, i);
        }

        private static (TdfArray value, int newOffset) DeserializeArray(
            byte[] packetBytes, int offset)
        {
            int i = offset;
            var tdfArray = new TdfArray();

            int arrayLength = packetBytes[i];
            i++;

            tdfArray.ArrayType = (TdfType)TdfUtils.SplitByte(packetBytes[i]).highPart;
            byte arrayTypeAndLen = packetBytes[i];

            if (tdfArray.ArrayType != TdfType.Struct)
                i++;

            for (int a = 0; a < arrayLength; a++)
            {
                var inner = DeserializeTdfValue(packetBytes, i, inArray: true, expectedTypeAndLength: arrayTypeAndLen);
                tdfArray.Items.Add(inner.Field.Value);
                i += inner.Length;

                if (tdfArray.ArrayType == TdfType.Struct)
                    i--;
            }

            if (tdfArray.ArrayType == TdfType.Struct)
                i++;

            return (tdfArray, i);
        }

        private static (TdfDictionary value, int newOffset) DeserializeDictionary(
            byte[] packetBytes, int offset, int entryCount)
        {
            int i = offset;
            var map = new Dictionary<object, object>();

            TdfType keyType = 0;
            TdfType valueType = 0;
            byte keyLength = 0;
            byte valueTypeLength = 0;

            for (int a = 0; a < entryCount; a++)
            {
                if (a == 0)
                {
                    var kp = TdfUtils.SplitByte(packetBytes[i]);
                    keyType = (TdfType)kp.highPart;
                    keyLength = (byte)kp.lowPart;
                    i++;
                }

                object key;
                (key, i) = DeserializeMapEntry(packetBytes, i, keyType, keyLength);

                if (a == 0)
                {
                    var vp = TdfUtils.SplitByte(packetBytes[i]);
                    valueType = (TdfType)vp.highPart;
                    valueTypeLength = (byte)vp.lowPart;
                    i++;
                }

                object value;
                (value, i) = DeserializeMapEntry(packetBytes, i, valueType, valueTypeLength);

                map.Add(key, value);
            }

            var tdfMap = new TdfDictionary(keyType, valueType) { Map = map };
            return (tdfMap, i);
        }

        private static (object value, int newOffset) DeserializeMapEntry(
            byte[] packetBytes, int offset, TdfType type, byte typeLength)
        {
            int i = offset;

            switch (type)
            {
                case TdfType.String:
                    {
                        byte strLen = packetBytes[i];
                        i++;
                        object val = TdfField.DeserializePrimitiveByType(type, CopyBytes(packetBytes, i, strLen));
                        i += strLen;
                        return (val, i);
                    }

                case TdfType.Struct:
                    {
                        var tdfStruct = new TdfStruct();
                        int test = packetBytes[i];
                        while (test != 0)
                        {
                            var inner = DeserializeTdfValue(packetBytes, i, inArray: false, expectedTypeAndLength: 0xFF);
                            tdfStruct.Fields.Add(inner.Field);
                            i += inner.Length;
                            test = packetBytes[i];
                        }
                        i++; // skip struct terminator
                        return (tdfStruct, i);
                    }

                default:
                    {
                        object val = TdfField.DeserializePrimitiveByType(type, CopyBytes(packetBytes, i, typeLength));
                        i += typeLength;
                        return (val, i);
                    }
            }
        }

        private static (TdfUnion value, int newOffset) DeserializeUnion(
            byte[] packetBytes, int offset, int selectedValue)
        {
            int i = offset;
            var union = new TdfUnion { SelectedUnionValue = (byte)selectedValue };

            if (union.SelectedUnionValue == 0x7F)
            {
                i -= 3;
                return (union, i);
            }

            var inner = DeserializeTdfValue(packetBytes, i, inArray: true, expectedTypeAndLength: 0xFF);
            union.Field = inner.Field;
            i += inner.Length;

            return (union, i);
        }

        private static ushort ReadU16BE(byte[] bytes, int offset)
        {
            var tmp = new byte[2];
            Buffer.BlockCopy(bytes, offset, tmp, 0, 2);
            return BitConverter.ToUInt16(TdfUtils.SwapEndianess(tmp), 0);
        }

        private static byte[] CopyBytes(byte[] src, int offset, int len)
        {
            var dst = new byte[len];
            Buffer.BlockCopy(src, offset, dst, 0, len);
            return dst;
        }
    }
}