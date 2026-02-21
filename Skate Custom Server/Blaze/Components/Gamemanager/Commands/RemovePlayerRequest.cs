using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct RemovePlayerRequest
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("PID")]
        public uint PlayerId;

        [TdfMember("REAS")]
        public int RemovalReason;
    }
}
