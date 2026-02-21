using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct StartMatchmakingResponse
    {
        [TdfMember("MSID")]
        public uint MatchmakingId;
    }
}
