using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct IpAddress
    {
        [TdfMember("IP")]
        public uint IP;

        [TdfMember("PORT")]
        public ushort Port;
    }
}