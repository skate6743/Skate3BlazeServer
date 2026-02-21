using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct SetGameSettingsRequest
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("GSET")]
        public uint GameSettings;
    }
}
