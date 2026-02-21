using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct UserIdentification
    {
        [TdfMember("AID")]
        public long AccountId;

        [TdfMember("ALOC")]
        public uint AccountLocale;

        [TdfMember("EXBB")]
        public byte[] ExternalBlob;

        [TdfMember("EXID")]
        public ulong ExternalId;

        [TdfMember("ID")]
        public uint BlazeId;

        [TdfMember("NAME")]
        public string Name;

        [TdfMember("PID")]
        public long PersonaId;

        [TdfMember("ONLN")]
        public bool IsOnline;
    }
}
