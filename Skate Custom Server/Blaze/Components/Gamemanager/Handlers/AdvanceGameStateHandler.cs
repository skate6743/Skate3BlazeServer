using Blaze.Components.Gamemanager.Commands;
using Blaze.Components.Gamemanager.Notifications;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class AdvanceGameStateHandler
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

            var request = BlazeMessage.CreateModelFromRequest<AdvanceGameStateRequest>(packetBytes);
            game.GameData.GameState = request.GameState;

            await ServerUtils.SendNotificationToPlayers(
                game,
                new NotifyGameStateChanged
                {
                    GameState = request.GameState,
                    GameId = request.GameId
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyGameStateChange);
        }
    }
}