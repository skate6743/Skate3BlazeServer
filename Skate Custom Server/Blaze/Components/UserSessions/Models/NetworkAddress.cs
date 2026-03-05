using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct NetworkAddress
    {
        [TdfUnionCase(2)]
        public IpPairAddress? IpPairAddress; // Used by actual players

        [TdfUnionCase(3)]
        public IpAddress? IpAddress; // Used for dirtycast servers
    }
}
