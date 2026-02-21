using Blaze.Components.UserSessions.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Models
{
    public struct ReplicatedGameData
    {
        [TdfMember("ADMN")]
        public List<uint> AdminPlayerList;

        [TdfMember("ATTR")]
        public Dictionary<string, string> GameAttributes;

        [TdfMember("CAP")]
        public List<ushort> SlotCapacities;

        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("GNAM")]
        public string GameName;

        [TdfMember("GPVH")]
        public ulong GameProtocolVersionHash;

        [TdfMember("GSET")]
        public uint GameSettings;

        [TdfMember("GSID")]
        public uint GameReportingId;

        [TdfMember("GSTA")]
        public int GameState;

        [TdfMember("GVER")]
        public int GameProtocolVersion;

        [TdfMember("HNET")]
        public List<NetworkAddress> HostConnections;

        [TdfMember("HSES")]
        public uint TopologyHostSessionId;

        [TdfMember("IGNO")]
        public bool IgnoreEntryCriteriaWithInvite;

        [TdfMember("MCAP")]
        public ushort MaxPlayerCapacities;

        [TdfMember("NQOS")]
        public NetworkQosData NetworkQosData;

        [TdfMember("NTOP")]
        public int NetworkTopology;

        [TdfMember("PGID")]
        public string PersistedGameId;

        [TdfMember("PGSR")]
        public byte[] PersistedGameIdSecret;

        [TdfMember("PHST")]
        public HostInfo PlatformHost;

        [TdfMember("PSAS")]
        public string PingSiteAlias;

        [TdfMember("QCAP")]
        public ushort QueueCapacity;

        [TdfMember("SEED")]
        public uint SharedSeed;

        [TdfMember("THST")]
        public HostInfo TopologyHost;

        [TdfMember("UUID")]
        public string UUID;

        [TdfMember("VOIP")]
        public int VoipTopology;

        [TdfMember("VSTR")]
        public string GameProtocolVersionString;

        [TdfMember("XNNC")]
        public byte[] XnetNonce;

        [TdfMember("XSES")]
        public byte[] XnetSession;
    }
}
