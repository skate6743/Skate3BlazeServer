using Blaze.Tdf.Attributes;

namespace Blaze.Components.Util.Commands
{
    public struct PingResponse
    {
        [TdfMember("STIM")]
        public uint Timestamp;
    }
}
