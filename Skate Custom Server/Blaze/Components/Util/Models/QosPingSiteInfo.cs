using Blaze.Tdf.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Blaze.Components.Util.Models
{
    public struct QosPingSiteInfo
    {
        [TdfMember("PSA")]
        public string Address;

        [TdfMember("PSP")]
        public ushort Port;

        [TdfMember("SNA")]
        public string SiteName;
    }
}
