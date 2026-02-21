using Blaze.Components.UserSessions.Notifications;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.UserSessions.Handlers
{
    public class UpdateHardwareFlagsHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            await ServerUtils.SendEmptyResponse(user, packetBytes);

            await ServerUtils.SendNotificationToUser(
                user,
                new NotifyExtendedDataUpdated
                {
                    ExtendedData = user.ExtendedData,
                    BlazeId = user.Session.BlazeId
                },
                BlazeComponent.UserSessions,
                (ushort)UserSessionNotification.UserSessionExtendedDataUpdate);
        }
    }
}