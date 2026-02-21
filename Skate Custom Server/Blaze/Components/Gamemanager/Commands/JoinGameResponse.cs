using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct JoinGameResponse
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("JGS")] // Always included and 0 but not sure what used for
        public int JGS;
    }
}
