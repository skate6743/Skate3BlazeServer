using Blaze.Components.Util.Commands;
using Servers.Blaze.Models;

namespace Blaze.Components.Util.Handlers
{
    public class PingHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new PingResponse
                {
                    Timestamp = Convert.ToUInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                });

            await user.Stream.WriteAsync(response.Serialize());
        }
    } 
}
