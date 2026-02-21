using Blaze.Tdf.Attributes;
using Blaze.Tdf.Types;

namespace Blaze.Components.Util.Commands
{
    public struct GetTickerServerResponse
    {
        [TdfMember("ADRS")]
        public string Address;

        [TdfMember("PORT")]
        public uint Port;

        [TdfMember("SKEY")]
        public string Key;
    }
}
