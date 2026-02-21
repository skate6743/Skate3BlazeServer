using Blaze.Components.Util.Commands;
using Servers.Blaze.Models;

namespace Blaze.Components.Util.Handlers
{
    public class PostAuthHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new PostAuthResponse
                {
                    TelemetryServer = GetTelemetryServerHandler.Get(),
                    TickerServer = GetTickerServerHandler.Get()
                });

            await user.Stream.WriteAsync(response.Serialize());
            
        }
    }
}
