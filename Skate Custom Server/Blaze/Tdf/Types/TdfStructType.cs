using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaze.Tdf.Types
{
    public class TdfStruct
    {
        public List<TdfField> Fields = new List<TdfField>();

        public TdfStruct() { }

        public void Add(string tag, TdfType type, object value)
        {
            Fields.Add(new TdfField(tag, type, value));
        }

        public byte[] Serialize()
        {
            List<byte> bytes = new List<byte>();
            foreach (TdfField field in Fields)
            {
                bytes.AddRange(field.Serialize());
            }
            bytes.Add(0);
            return bytes.ToArray();
        }
    }
}
