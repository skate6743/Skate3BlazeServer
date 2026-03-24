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

        public uint PlayersInQueue;
        public void AddToQueue() => Interlocked.Increment(ref PlayersInQueue);
        public void RemoveFromQueue()
        {
            uint current;
            do
            {
                current = PlayersInQueue;
                if (current == 0) return;
            }
            while (Interlocked.CompareExchange(ref PlayersInQueue, current - 1, current) != current);
        }

        public readonly object Lock = new object();
    }
}