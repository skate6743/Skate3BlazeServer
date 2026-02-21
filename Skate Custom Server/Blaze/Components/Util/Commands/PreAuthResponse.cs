using Blaze.Tdf.Attributes;
using Blaze.Components.Util.Models;

namespace Blaze.Components.Util.Commands
{
    public struct PreAuthResponse
    {
        [TdfMember("CIDS")]
        public List<ushort> ComponentIds;

        [TdfMember("CONF")]
        public FetchConfigResponse Config;

        [TdfMember("QOSS")]
        public QosConfigInfo QosSettings;

        [TdfMember("SVER")]
        public string ServerVersion;
    }
}
