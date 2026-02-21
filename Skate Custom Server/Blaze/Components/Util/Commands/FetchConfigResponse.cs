using Blaze.Tdf.Attributes;

namespace Blaze.Components.Util.Commands
{
    public struct FetchConfigResponse
    {
        [TdfMember("CONF")]
        public Dictionary<string, string> ConfigFields;
    }
}
