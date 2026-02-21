using Blaze.Components.UserSessions.Commands;
using Blaze.Components.UserSessions.Models;
using Servers.Blaze.Models;

namespace Blaze.Components.UserSessions.Handlers
{
    public class LookupUsersHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            BlazeMessage response = BlazeMessage.CreateResponseFromModel(packetBytes,
                new LookupUsersResponse
                {
                    UserIdentificationList = new List<UserIdentification>() // Not handling properly yet so return a empty list
                });

            await user.Stream.WriteAsync(response.Serialize());
        }
    }
}