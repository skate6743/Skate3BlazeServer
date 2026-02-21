using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaze.Tdf.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TdfUnionCase : Attribute
    {
        public byte Selector { get; }

        public TdfUnionCase(byte selector)
        {
            Selector = selector;
        }
    }
}
