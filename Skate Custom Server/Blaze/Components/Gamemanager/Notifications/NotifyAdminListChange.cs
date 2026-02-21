using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyAdminListChange
    {
        [TdfMember("ALST")]
        public uint AdminPlayerId;

        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("OPER")]
        public int Operation;

        [TdfMember("UID")]
        public uint UpdaterPlayerId;
    }
}
