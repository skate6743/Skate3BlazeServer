using Blaze.Tdf.Attributes;

namespace Blaze.Components.Authentication.Models
{
    public struct PersonaDetails
    {
        [TdfMember("DSNM")]
        public string DisplayName;

        [TdfMember("LAST")]
        public uint LastLoginTime;

        [TdfMember("PID")]
        public long PersonaId;

        [TdfMember("XREF")]
        public ulong ExternalRef;

        [TdfMember("XTYP")]
        public int ExternalRefType;
    }
}
