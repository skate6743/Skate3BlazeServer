using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Notifications
{
    public struct NotifyGameStateChanged
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("GSTA")]
        public int GameState;
    }
}
