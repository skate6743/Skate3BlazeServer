using System.Collections.Concurrent;
using System.Net;
using Servers.Blaze.Models;

namespace Servers
{
    public class ServerGlobals
    {
        public static string ServerIP = IPAddress.Any.ToString();
        public static ushort ServerPort = 42100;
        public static ushort HttpServerPort = 80;
        public static uint PingPeriodSecs = 20;

        public static ConcurrentDictionary<uint, User> Users = new();
        public static ConcurrentDictionary<uint, Game> Games = new();
        public static ConcurrentDictionary<Game, RelayServer> LobbyRelayServers = new();

        public static List<byte[]> CachedRequests = new List<byte[]>();

        private static int _gameIdCounter = 0;
        public static uint GetNextGameId()
        {
            while (true)
            {
                int current = _gameIdCounter;
                int next = current >= 500 ? 1 : current + 1;

                if (Interlocked.CompareExchange(ref _gameIdCounter, next, current) == current)
                    return (uint)next;
            }
        }
    }
}
