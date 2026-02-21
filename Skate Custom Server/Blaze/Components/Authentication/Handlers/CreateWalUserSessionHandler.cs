using Blaze.Components.Authentication.Commands;
using Blaze.Components.Authentication.Models;
using Servers.Blaze.Models;

namespace Blaze.Components.Authentication
{
    public class CreateWalUserSessionHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            SessionDetails sessionDetails = user.Session;

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new CreateWalUserSessionResponse
                {
                    BlazeId = sessionDetails.BlazeId,
                    IsFirstLogin = false,
                    BlazeToken = sessionDetails.BlazeToken,
                    LastLoginTime = 0,
                    Email = sessionDetails.Email,
                    PersonaDetails = sessionDetails.PersonaDetails,
                    UserId = sessionDetails.UserId
                });

            await user.Stream.WriteAsync(response.Serialize());
            
        }
    }
}