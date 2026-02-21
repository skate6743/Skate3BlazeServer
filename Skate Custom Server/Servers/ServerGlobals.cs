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
        public static uint QoSProbes = 5;
        public static uint QoSProbeSize = 1000;

        public static ConcurrentDictionary<uint, User> Users = new ConcurrentDictionary<uint, User>();
        public static ConcurrentDictionary<uint, Game> Games = new ConcurrentDictionary<uint, Game>();

        private static int _gameIdCounter = 1;
        public static uint GetNextGameId()
        {
            return (uint)Interlocked.Increment(ref _gameIdCounter);
        }

        private static int _userIdCounter = 1;
        public static uint GetNextUserId()
        {
            return (uint)Interlocked.Increment(ref _userIdCounter);
        }
    }
}
