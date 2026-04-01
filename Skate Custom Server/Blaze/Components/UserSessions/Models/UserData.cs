using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct UserData
    {
        [TdfMember("EDAT")]
        public UserSessionExtendedData ExtendedData;

        [TdfMember("FLGS")]
        public int StatusFlags;

        [TdfMember("USER")]
        public UserIdentification UserInfo;
    }
}
