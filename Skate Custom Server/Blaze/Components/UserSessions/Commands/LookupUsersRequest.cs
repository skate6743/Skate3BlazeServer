using Blaze.Tdf.Attributes;
using Blaze.Components.UserSessions.Models;

namespace Blaze.Components.UserSessions.Commands
{
    public struct LookupUsersRequest
    {
        [TdfMember("LTYP")]
        public int LookupType;

        [TdfMember("ULST")]
        public List<UserIdentification> UserIdentificationList;
    }
}
