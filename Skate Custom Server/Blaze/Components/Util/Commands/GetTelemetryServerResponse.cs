using Blaze.Tdf.Attributes;
using Blaze.Tdf.Types;

namespace Blaze.Components.Util.Commands
{
    public struct GetTelemetryServerResponse
    {
        [TdfMember("ADRS")]
        public string Address;

        [TdfMember("ANON")]
        public bool IsAnonymous;

        [TdfMember("DISA")]
        public string Disable;

        [TdfMember("FILT")]
        public string Filter;

        [TdfMember("LOC")]
        public uint Locale;

        [TdfMember("NOOK")]
        public string NoToggleOk;

        [TdfMember("PORT")]
        public uint Port;

        [TdfMember("SDLY")]
        public uint SendDelay;

        [TdfMember("SKEY")]
        public string Key;

        [TdfMember("SPCT")]
        public uint SendPercentage;
    }
}
