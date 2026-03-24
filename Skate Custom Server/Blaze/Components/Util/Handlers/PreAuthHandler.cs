using Blaze.Components.Util.Commands;
using Blaze.Components.Util.Models;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Util.Handlers
{
    public static class PreAuthHandler
    {
        // preAuth response is pretty much same so cache it instead of rebuilding always
        private static byte[] _cachedBytes;

        static PreAuthHandler()
        {
            Dictionary<string, string> config = new Dictionary<string, string>
            {
                { "pingPeriodInMs", (ServerGlobals.PingPeriodSecs * 1000).ToString() },
                { "voipHeadsetUpdateRate", "1000" }
            };

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                new byte[] { 0, 0, 0, 9, 0, 7, 0, 0, 0, 0, 0, 0 },
                new PreAuthResponse
                {
                    ComponentIds = new List<ushort> { 1, 4, 7, 8, 9, 11, 12, 15, 25, 30720, 30722, 30723 },
                    Config =
                    {
                        ConfigFields = config
                    },
                    QosSettings =
                    {
                        NumLatencyProbes = 0,
                        PingSiteInfoByAliasMap = new Dictionary<string, QosPingSiteInfo> {  }
                    },
                    ServerVersion = "Custom Blaze 2.11.3.1 Server"
                });

            _cachedBytes = response.Serialize();
        }

        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            byte[] response = (byte[])_cachedBytes.Clone();
            for (int i = 0; i < 2; i++)
                response[10 + i] = packetBytes[10+i];

			await user.Stream.WriteAsync(response);
        }
    }
}
