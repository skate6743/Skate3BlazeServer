using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct MigrateHostRequest
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("PID")]
        public uint NewHostId;
    }
}
