using Blaze.Tdf.Attributes;
using Blaze.Components.Gamemanager.Models;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct UpdateMeshConnectionRequest
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("TARG")]
        public List<PlayerConnectionStatus> MeshConnectionStatusList;
    }
}
