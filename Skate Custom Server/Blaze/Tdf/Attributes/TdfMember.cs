using Blaze;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaze.Tdf.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TdfMember : Attribute
    {
        public string Tag { get; }
        public TdfMember(string tag)
        {
            if (tag == null || tag.Length > 4)
                throw new ArgumentException("TDF Tag must be max 4 characters...");

            this.Tag = tag;
        }
    }
}
