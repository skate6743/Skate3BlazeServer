using Blaze.Components.Gamemanager.Models;
using System.Collections.Concurrent;
using System.Net;

namespace Servers.Blaze.Models
{
    public class Game()
    {
        public uint HostId;
        public ReplicatedGameData GameData = new ReplicatedGameData();
        public List<Player> Players = new List<Player>();
        public bool AcceptingRelayConnections = false;
        public readonly object Lock = new object();
    }
}