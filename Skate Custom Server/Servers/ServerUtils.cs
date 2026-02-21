using Blaze;
using Blaze.Tdf.Types;
using Servers.Blaze.Models;
using Blaze.MessageLists;
using Blaze.Components.Messaging.Notifications;

namespace Servers
{
    public class ServerUtils
    {
        public enum ErrorCode : ushort
        {
            ERR_COMPONENT_NOT_FOUND = 0x4002,
            ERR_COMMAND_NOT_FOUND = 0x4003,
            ERR_AUTHENTICATION_REQUIRED = 0x4004,
            GAMEMANAGER_ERR_PERMISSION_DENIED = 0x1E00,
            AUTH_ERR_INVALID_PS3_TICKET = 0x6800
        };

        public static async Task SendNotificationToUser(User receiver, object notification, BlazeComponent component, ushort command)
        {
            TdfStruct tdfNotification = TdfReflection.ToTdfStruct(notification);
            BlazeMessage msg = new BlazeMessage();
            msg.Component = (ushort)component;
            msg.Command = command;
            msg.MessageType = (byte)BlazeMessageType.Notification;

            msg.Fields.AddRange(tdfNotification.Fields);

            byte[] packet = msg.Serialize();

            await receiver.Stream.WriteAsync(packet);
        }

        public static async Task SendNotificationToPlayers<T>(Game game, T notification, BlazeComponent component, ushort command)
        {
            TdfStruct tdfNotification = TdfReflection.ToTdfStruct(notification);
            BlazeMessage msg = new BlazeMessage();
            msg.Component = (ushort)component;
            msg.Command = command;
            msg.MessageType = (byte)BlazeMessageType.Notification;

            msg.Fields.AddRange(tdfNotification.Fields);

            byte[] packet = msg.Serialize();

            List<Player> snapshot;
            lock (game.Lock)
            {
                snapshot = game.Players.ToList();
            }

            foreach (Player player in snapshot)
            {
                await player.UserData.Stream.WriteAsync(packet);
            }
        }

        public static async Task SendEmptyResponse(User receiver, byte[] packetBytes)
        {
            BlazeMessage response = BlazeMessage.CreateResponseHeaderFrom(packetBytes);
            await receiver.Stream.WriteAsync(response.Serialize());
        }

        public static async Task SendError(User receiver, byte[] packetBytes, ErrorCode errorCode)
        {
            BlazeMessage response = BlazeMessage.CreateResponseHeaderFrom(packetBytes);
            response.MessageType = (byte)BlazeMessageType.Error;
            response.ErrorCode = (ushort)errorCode;

            await receiver.Stream.WriteAsync(response.Serialize());
        }

        public static async Task SendFeedMessageToUser(User receiver, string halId, string attribute = "")
        {
            string xml = $"<FeedMessage><halId>{halId}</halId><attrib><value>{attribute}</value></attrib></FeedMessage>";
            List<byte> targetBytes = new List<byte> { 0x78, 0x02, 0x00, 0x01 };
            targetBytes.AddRange(BitConverter.GetBytes(receiver.UserIdentification.BlazeId));
            targetBytes.Reverse();

            await SendNotificationToUser(
                receiver,
                new NotifyMessage
                {
                    Payload = new ClientMessage
                    {
                        Attributes = new Dictionary<uint, string> { { 65282, xml } }, // Magic number haven't figured out yet why this number is used in all skate feed messages
                        Target = BitConverter.ToUInt64(targetBytes.ToArray())
                    },
                    Time = 0,
                    Source = 0
                },
                BlazeComponent.Messaging,
                (ushort)MessagingNotifications.NotifyMessage);
        }
    }
}
