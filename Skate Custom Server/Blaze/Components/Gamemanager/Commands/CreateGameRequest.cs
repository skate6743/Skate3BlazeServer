using Blaze.Components.UserSessions.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct CreateGameRequest
    {
        [TdfMember("ATTR")]
        public Dictionary<string, string> GameAttributes;

        [TdfMember("GSET")]
        public uint GameSettings;

        [TdfMember("HNET")]
        public List<NetworkAddress> HostConnections;

        [TdfMember("PGID")]
        public string PersistedGameId;

        [TdfMember("PGSC")]
        public byte[] PersistedGameIdSecret;

        [TdfMember("GCTR")]
        public string PingSiteAlias;

        [TdfMember("QCAP")]
        public ushort QueueCapacity;

        [TdfMember("VSTR")]
        public string GameProtocolVersionString;
    }
}
