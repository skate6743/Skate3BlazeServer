using Blaze.Components.UserSessions.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Models
{
    public struct ReplicatedGamePlayer
    {
        [TdfMember("BLOB")]
        public byte[] Blob;

        [TdfMember("EXID")]
        public ulong ExternalId;

        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("LOC")]
        public uint Locale;

        [TdfMember("NAME")]
        public string DisplayName;

        [TdfMember("NQOS")]
        public NetworkQosData NetworkQosData;

        [TdfMember("PATT")]
        public Dictionary<string, string> PlayerAttributes;

        [TdfMember("PID")]
        public uint PlayerId;

        [TdfMember("PNET")]
        public NetworkAddress PlayerNetwork;

        [TdfMember("SID")]
        public byte SlotId;

        [TdfMember("SLOT")]
        public int SlotType;

        [TdfMember("STAT")]
        public int PlayerState;

        [TdfMember("TEAM")]
        public ushort Team;

        [TdfMember("TIDX")]
        public ushort TeamIndex;

        [TdfMember("TIME")]
        public long JoinDate;

        [TdfMember("UID")]
        public uint PlayerSessionId;
    }
}
