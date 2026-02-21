using Blaze.Components.Util.Commands;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Util.Handlers
{
    public class GetTelemetryServerHandler
    {
        public static string telemetryServerIP = ServerGlobals.ServerIP;
        public static uint telemetryServerPort = 9946;

        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            BlazeMessage response = BlazeMessage.CreateResponseFromModel(packetBytes, Get());

            await user.Stream.WriteAsync(response.Serialize());
            
        }

        public static GetTelemetryServerResponse Get()
        {
            return new GetTelemetryServerResponse()
            {
                Address = telemetryServerIP,
                IsAnonymous = false,
                Locale = 1701729619,
                Port = telemetryServerPort,
                SendDelay = 15000,
                SendPercentage = 75
            };
        }
    }
}
