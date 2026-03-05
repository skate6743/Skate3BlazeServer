using Blaze.Tdf.Attributes;

namespace Blaze.Components.UserSessions.Models
{
    public struct XboxServerAddress
    {
        [TdfMember("PORT")]
        public ushort Port;

        [TdfMember("SITE")]
        public string SiteName;

        [TdfMember("SVID")]
        public uint ServiceId;
    }
}
