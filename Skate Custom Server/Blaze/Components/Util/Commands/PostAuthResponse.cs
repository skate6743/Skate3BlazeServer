using Blaze.Tdf.Attributes;
using Blaze.Components.Util.Models;

namespace Blaze.Components.Util.Commands
{
    public struct PostAuthResponse
    {
        [TdfMember("TELE")]
        public GetTelemetryServerResponse TelemetryServer;

        [TdfMember("TICK")]
        public GetTickerServerResponse TickerServer;
    }
}
