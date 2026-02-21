using Blaze.Tdf.Attributes;
using Blaze.Components.Authentication.Models;

namespace Blaze.Components.Authentication.Commands
{
    public struct CreateWalUserSessionResponse
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

        [TdfMember("PDTL")]
        public PersonaDetails PersonaDetails;

        [TdfMember("UID")]
        public long UserId;
    }
}
