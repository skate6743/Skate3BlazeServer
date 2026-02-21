using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct StartMatchmakingRequest
    {
        [TdfMember("ATTR")]
        public Dictionary<string, string> MatchmakingAttributes;
    }
}
