using Blaze.Components.Gamemanager.Commands;
using Blaze.GamemanagerComponent;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class SetPlayerCapacityHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            Game game = user.CurrentGame;
            if (game.HostId != user.UserIdentification.BlazeId)
            {
                await ServerUtils.SendError(user, packetBytes, ServerUtils.ErrorCode.GAMEMANAGER_ERR_PERMISSION_DENIED);
                return;
            }

            await ServerUtils.SendEmptyResponse(user, packetBytes);

            var request = BlazeMessage.CreateModelFromRequest<SetPlayerCapacityRequest>(packetBytes);

            if (request.SlotCapacities.Count <= 2)
            {
                game.GameData.SlotCapacities = request.SlotCapacities;

                await ServerUtils.SendNotificationToPlayers(
                    game,
                    new NotifyGameCapacityChanged
                    {
                        SlotCapacities = request.SlotCapacities,
                        GameId = request.GameId
                    },
                    BlazeComponent.Gamemanager,
                    (ushort)GameManagerNotifications.NotifyGameCapacityChange);
            }
        }
    }
}