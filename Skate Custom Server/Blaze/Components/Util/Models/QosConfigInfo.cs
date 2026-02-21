using Blaze.Tdf.Attributes;

namespace Blaze.Components.Util.Models
{
    public struct QosConfigInfo
    {
        [TdfMember("BWPS")]
        public QosPingSiteInfo BandwidthPingSiteInfo;

        [TdfMember("LNP")]
        public ushort NumLatencyProbes;

        [TdfMember("LTPS")]
        public Dictionary<string, QosPingSiteInfo> PingSiteInfoByAliasMap;

        [TdfMember("SVID")]
        public uint ServiceId;
    }
}
