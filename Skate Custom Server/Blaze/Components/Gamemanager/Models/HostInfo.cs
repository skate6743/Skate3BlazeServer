using Blaze.Components.UserSessions.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.Components.Gamemanager.Models
{
    public struct HostInfo
    {
        [TdfMember("HPID")]
        public uint PlayerId;

        [TdfMember("HSLT")]
        public byte SlotId;
    }
}
