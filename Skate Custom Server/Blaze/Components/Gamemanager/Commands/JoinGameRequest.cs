using Blaze.Tdf.Attributes;
using Blaze.Components.Authentication.Models;
using Blaze.Components.UserSessions.Models;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct JoinGameRequest
    {
        [TdfMember("USER")]
        public UserIdentification User;
    }
}
