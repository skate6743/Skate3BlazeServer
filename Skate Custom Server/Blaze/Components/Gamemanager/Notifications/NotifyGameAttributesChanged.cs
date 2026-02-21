using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyGameAttributesChanged
    {
        [TdfMember("ATTR")]
        public Dictionary<string, string> GameAttributes;

        [TdfMember("GID")]
        public uint GameId;
    }
}
