using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyPlayerJoinCompleted
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("PID")]
        public uint PlayerId;
    }
}
