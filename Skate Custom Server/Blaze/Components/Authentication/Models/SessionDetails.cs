using Blaze.Tdf.Attributes;

namespace Blaze.Components.Authentication.Models
{
    public struct SessionDetails
    {
        [TdfMember("BUID")]
        public uint BlazeId;

        [TdfMember("FRST")]
        public bool IsFirstLogin;

        [TdfMember("KEY")]
        public string BlazeToken;

        [TdfMember("LLOG")]
        public long LastLoginTime;

        [TdfMember("MAIL")]
        public string Email;

        [TdfMember("UID")]
        public long UserId;

        [TdfMember("PDTL")]
        public PersonaDetails PersonaDetails;
    }
}
