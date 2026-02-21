using Blaze.Tdf.Attributes;

namespace Blaze.Components.Messaging.Notifications
{
    public struct NotifyMessage
    {
        [TdfMember("PYLD")]
        public ClientMessage Payload;

        [TdfMember("SRCE")]
        public ulong Source;

        [TdfMember("TIME")]
        public uint Time;
    }
}
