using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct UserSessionExtendedData
    {
        [TdfMember("ADDR")]
        public NetworkAddress NetworkAddress;

        [TdfMember("BPS")]
        public string BestPingSite;

        [TdfMember("CTY")]
        public string Country;

        [TdfMember("DMAP")]
        public Dictionary<uint, uint> DataMap;

        [TdfMember("HWFG")]
        public uint HardwareFlags;

        [TdfMember("QDAT")]
        public NetworkQosData QosData;

        [TdfMember("UATT")]
        public ulong UserInfoAttribute;
    }
}
