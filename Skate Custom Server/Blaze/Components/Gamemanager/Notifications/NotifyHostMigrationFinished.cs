using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyHostMigrationFinished
    {
        [TdfMember("GID")]
        public uint GameId;
    }
}
