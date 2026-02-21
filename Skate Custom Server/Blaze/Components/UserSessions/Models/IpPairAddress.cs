using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct IpPairAddress
    {
        [TdfMember("EXIP")]
        public IpAddress ExternalIp;

        [TdfMember("INIP")]
        public IpAddress InternalIp;
    }
}
