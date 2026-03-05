using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct XboxClientAddress
    {
        [TdfMember("XDDR")]
        public byte[] XnAddr;

        [TdfMember("XUID")]
        public ulong Xuid;
    }
}
