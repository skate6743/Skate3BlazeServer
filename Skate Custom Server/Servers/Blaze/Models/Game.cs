using Blaze.Components.Gamemanager.Models;

namespace Servers.Blaze.Models
{
    public class Game()
    {
        public uint HostId;
        public ReplicatedGameData GameData = new ReplicatedGameData();
        public List<Player> Players = new List<Player>();
        public readonly object Lock = new object();
    }
}