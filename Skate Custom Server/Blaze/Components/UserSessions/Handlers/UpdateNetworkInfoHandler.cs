using Blaze.Components.UserSessions.Notifications;
using Servers;
using Blaze.Components.UserSessions.Commands;
using Servers.Blaze.Models;
using Blaze.MessageLists;
using Blaze.Components.UserSessions.Models;

namespace Blaze.Components.UserSessions.Handlers
{
    public class UpdateNetworkInfoHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            var request = BlazeMessage.CreateModelFromRequest<UpdateNetworkInfoRequest>(packetBytes);

            user.ExtendedData.NetworkAddress = request.NetworkAddress;
            user.ExtendedData.QosData = request.QosData;

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

            if (request.QosData.NatType == (int)NatType.NAT_TYPE_STRICT || request.QosData.NatType == (int)NatType.NAT_TYPE_STRICT_SEQUENTIAL)
            {
                await ServerUtils.SendFeedMessageToUser(
                    user,
                    "ID_COMMUNITY_SKATER_NAME",
                    "Warning! Strict NAT Type has been detected. Please enable UPnP from router settings or disable VPN to be able to matchmake.  ");
            }
        }
    }
}