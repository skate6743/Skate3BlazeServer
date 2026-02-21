using System.Net.Sockets;
using Blaze.MessageLists;

namespace Blaze.Components.UserSessions.Notifications
{
    public class NotifyUserRemoved
    {
        public static async Task Send(NetworkStream receiver, uint removedUsersId)
        {
            BlazeMessage notification = BlazeMessage.CreateNotificationMessageBase(BlazeComponent.UserSessions, (ushort)UserSessionNotification.UserRemoved);

            notification.Add("BUID", TdfType.UInt32, removedUsersId);

            await receiver.WriteAsync(notification.Serialize());
        }
    }
}