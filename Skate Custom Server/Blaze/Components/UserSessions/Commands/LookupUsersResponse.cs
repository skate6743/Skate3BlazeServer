using Blaze.Tdf.Attributes;
using Blaze.Components.UserSessions.Models;

namespace Blaze.Components.UserSessions.Commands
{
    public struct LookupUsersResponse
    {
        [TdfMember("ULST")]
        public List<UserIdentification> UserIdentificationList;
    }
}
