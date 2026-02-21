using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct NetworkAddress
    {
        [TdfUnionCase(2)]
        public IpPairAddress? IpPairAddress;

        [TdfUnionCase(3)]
        public IpAddress? IpAddress;
    }
}
