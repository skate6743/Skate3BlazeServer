using Blaze.Tdf.Attributes;
using Blaze.Components.UserSessions.Models;

namespace Blaze.Components.UserSessions.Notifications
{
    public struct NotifyExtendedDataUpdated
    {
        [TdfMember("DATA")]
        public UserSessionExtendedData ExtendedData;

        [TdfMember("USID")]
        public uint BlazeId;
    }
}