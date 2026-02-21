using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct MigrateHostRequest
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("HOST")]
        public uint NewHostPlayer;
    }
}
