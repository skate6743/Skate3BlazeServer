using Blaze.Components.Util.Commands;
using Servers;

using Servers.Blaze.Models;

namespace Blaze.Components.Util.Handlers
{
    public class GetTickerServerHandler
    {
        public static string tickerServerIP = ServerGlobals.ServerIP;
        public static uint tickerServerPort = 8999;

        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            BlazeMessage response = BlazeMessage.CreateResponseFromModel(packetBytes, Get());

            await user.Stream.WriteAsync(response.Serialize());
            
        }

        public static GetTickerServerResponse Get()
        {
            return new GetTickerServerResponse()
            {
                Address = tickerServerIP,
                Port = tickerServerPort,
                Key = $"0,{tickerServerIP}:{tickerServerPort.ToString()},skate-2010-ps3,10,50,50,50,50,0,0"
            };
        }
    }
}
