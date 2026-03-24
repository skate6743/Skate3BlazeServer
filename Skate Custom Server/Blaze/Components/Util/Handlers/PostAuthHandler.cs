using Blaze.Components.Util.Commands;
using Blaze.Components.Util.Models;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Util.Handlers
{
    public static class PostAuthHandler
    {
        // postAuth response is always the same so cache it instead of rebuilding always
        private static byte[] _cachedBytes;

        static PostAuthHandler()
        {
            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                new byte[] { 0, 0, 0, 9, 0, 7, 0, 0, 0, 0, 0, 0 },
                new PostAuthResponse
                {
                    TelemetryServer = GetTelemetryServerHandler.Get(),
                    TickerServer = GetTickerServerHandler.Get()
                });

            _cachedBytes = response.Serialize();
        }

        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            byte[] response = (byte[])_cachedBytes.Clone();
            for (int i = 0; i < 2; i++)
                response[10 + i] = packetBytes[10 + i];

            await user.Stream.WriteAsync(response);
        }
    }
}
