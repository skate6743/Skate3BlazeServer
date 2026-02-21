using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyPlayerRemoved
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("PID")]
        public uint PlayerId;

        [TdfMember("REAS")]
        public int RemovalReason;
    }
}
