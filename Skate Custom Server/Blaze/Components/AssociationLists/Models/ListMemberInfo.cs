using Blaze.Tdf.Attributes;
using Blaze.Components.UserSessions.Models;

namespace Blaze.Components.Authentication.Models
{
    public struct ListMemberInfo
    {
        [TdfMember("MATS")]
        public long TimeAdded;

        [TdfMember("MUIF")]
        public UserData UserData;
    }
}
