using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyHostMigrationStart
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("HOST")]
        public uint HostPlayerId;

        [TdfMember("PMIG")]
        public int MigrationType;

        [TdfMember("SLOT")]
        public byte HostSlotId;
    }
}
