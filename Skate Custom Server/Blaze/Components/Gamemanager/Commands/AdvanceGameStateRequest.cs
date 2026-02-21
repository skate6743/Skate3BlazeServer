using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct AdvanceGameStateRequest
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("GSTA")]
        public int GameState;
    }
}
