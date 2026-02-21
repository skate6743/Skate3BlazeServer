using Blaze.Components.Gamemanager.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyPlayerJoining
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("PDAT")]
        public ReplicatedGamePlayer PlayerData;
    }
}
