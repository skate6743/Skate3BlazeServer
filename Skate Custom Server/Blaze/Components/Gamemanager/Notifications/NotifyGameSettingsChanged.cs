using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyGameSettingsChanged
    {
        [TdfMember("ATTR")]
        public uint GameSettings;

        [TdfMember("GID")]
        public uint GameId;
    }
}
