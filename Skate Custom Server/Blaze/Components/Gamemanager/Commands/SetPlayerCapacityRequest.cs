using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Commands
{
    public struct SetPlayerCapacityRequest
    {
        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("PCAP")]
        public List<ushort> SlotCapacities;
    }
}
