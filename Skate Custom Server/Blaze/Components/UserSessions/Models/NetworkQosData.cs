using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct NetworkQosData
    {
        [TdfMember("DBPS")]
        public uint DownStreamBitsPerSecond;

        [TdfMember("NATT")]
        public int NatType;

        [TdfMember("UBPS")]
        public uint UpstreamBitsPerSecond;  
    }
}
