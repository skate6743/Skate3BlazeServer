using Blaze.Components.Util.Commands;
using Blaze.Components.Util.Models;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Util.Handlers
{
    public class PreAuthHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            Dictionary<string, string> config = new Dictionary<string, string>
            {
                { "pingPeriodInMs", (ServerGlobals.PingPeriodSecs * 1000).ToString() },
                { "voipHeadsetUpdateRate", "1000" }
            };

            QosPingSiteInfo pingSiteInfo = new QosPingSiteInfo
            {
                Address = ServerGlobals.ServerIP,
                Port = 80,
                SiteName = ServerGlobals.ServerIP
            };

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new PreAuthResponse
                {
                    ComponentIds = new List<ushort> { 1, 4, 7, 8, 9, 11, 12, 15, 25, 30720, 30722, 30723 },
                    Config =
                    {
                        ConfigFields = config
                    },
                    QosSettings =
                    {
                        BandwidthPingSiteInfo = pingSiteInfo,
                        NumLatencyProbes = 0,
                        PingSiteInfoByAliasMap = new Dictionary<string, QosPingSiteInfo> { { "sjc", pingSiteInfo } }
                    },
                    ServerVersion = "Custom Blaze 2.11.3.1 Server"
                });

			await user.Stream.WriteAsync(response.Serialize());
        }
    }
}
