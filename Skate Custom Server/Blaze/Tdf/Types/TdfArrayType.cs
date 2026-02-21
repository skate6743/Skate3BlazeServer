using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaze.Tdf.Types
{
    public class TdfArray
    {
        public List<object> Items = new List<object>();
        public TdfType ArrayType = 0;

        public TdfArray() { }
        public TdfArray(TdfType arrayType) => this.ArrayType = arrayType;
    }
}
