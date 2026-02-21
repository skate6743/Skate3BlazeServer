using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct SetGameAttributesRequest
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("ATTR")]
        public Dictionary<string, string> GameAttributes;
    }
}
