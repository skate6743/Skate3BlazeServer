using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyGameCapacityChanged
    {
        [TdfMember("CAP")]
        public List<ushort> SlotCapacities;

        [TdfMember("GID")]
        public uint GameId;
    }
}
