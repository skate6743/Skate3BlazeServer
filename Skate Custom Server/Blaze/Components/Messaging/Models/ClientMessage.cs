using Blaze.Tdf.Attributes;

namespace Blaze.Components.Messaging.Notifications
{
    public struct ClientMessage
    {
        [TdfMember("ATTR")]
        public Dictionary<uint, string> Attributes;

        [TdfMember("TARG")]
        public ulong Target;
    }
}
