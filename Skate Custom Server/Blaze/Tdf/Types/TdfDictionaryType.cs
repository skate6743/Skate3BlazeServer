using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaze.Tdf.Types
{
    public class TdfDictionary
    {
        public TdfType KeyType;
        public TdfType ValueType;
        public Dictionary<object, object> Map = new Dictionary<object, object>();

        public TdfDictionary() { }

        public TdfDictionary(TdfType keyType, TdfType valueType)
        {
            this.KeyType = keyType;
            this.ValueType = valueType;
        }
    }
}
