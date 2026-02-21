using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Models
{
    public struct PlayerConnectionStatus
    {
        [TdfMember("PID")]
        public uint TargetPlayer;

        [TdfMember("STAT")]
        public int PlayerNetConnectionStatus;
    }
}
