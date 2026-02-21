using Blaze.Components.Util.Handlers;
using Blaze.Components.UserSessions.Notifications;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze

{
    public static class UtilComponentRouter
    {
        public static async Task Handle(User user, byte[] receivedPacket)
        {
            UtilMessage utilMessage = (UtilMessage)TdfUtils.GetCommandFromPacket(receivedPacket);

            switch (utilMessage)
            {
                case UtilMessage.preAuth:
                    await PreAuthHandler.HandleRequest(user, receivedPacket);
                    break;
                case UtilMessage.ping:
                    await PingHandler.HandleRequest(user, receivedPacket);
                    break;
                case UtilMessage.postAuth:
                    await PostAuthHandler.HandleRequest(user, receivedPacket);

                    await ServerUtils.SendNotificationToUser(
                        user,
                        user.UserIdentification,
                        BlazeComponent.UserSessions,
                        (ushort)UserSessionNotification.UserAdded);

                    await ServerUtils.SendNotificationToUser(
                        user,
                        new NotifyExtendedDataUpdated
                        {
                            ExtendedData = user.ExtendedData,
                            BlazeId = user.UserIdentification.BlazeId
                        },
                        BlazeComponent.UserSessions,
                        (ushort)UserSessionNotification.UserSessionExtendedDataUpdate);

                    break;
                case UtilMessage.setClientMetrics:
                    await ServerUtils.SendEmptyResponse(user, receivedPacket);
                    break;
                case UtilMessage.getTelemetryServer:
                    await GetTelemetryServerHandler.HandleRequest(user, receivedPacket);
                    break;
                case UtilMessage.getTickerServer:
                    await GetTickerServerHandler.HandleRequest(user, receivedPacket);
                    break;
                default:
                    Console.WriteLine($"{utilMessage} command is not implemented!");
                    await ServerUtils.SendError(user, receivedPacket, ServerUtils.ErrorCode.ERR_COMMAND_NOT_FOUND);
                    break;
            }
        }
    }
}