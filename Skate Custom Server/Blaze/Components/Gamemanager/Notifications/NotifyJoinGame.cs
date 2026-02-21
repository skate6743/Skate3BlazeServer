using Blaze.Components.Gamemanager.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyJoinGame
    {
        [TdfMember("ERR")]
        public uint Error;

        [TdfMember("GAME")]
        public ReplicatedGameData GameData;

        [TdfMember("MMID")]
        public uint MatchmakingId;

        [TdfMember("PROS")]
        public List<ReplicatedGamePlayer> Players;
    }
}