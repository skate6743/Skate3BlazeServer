using Blaze.Components.Gamemanager.Commands;
using Blaze.Components.Gamemanager.Models;
using Blaze.Components.Gamemanager.Notifications;
using Blaze.GamemanagerComponent;
using Blaze.MessageLists;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class FinalizeGameCreationHandler
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

            game.GameData.GameState = (int)GameState.PRE_GAME;

            await ServerUtils.SendNotificationToPlayers(
                game,
                new NotifyGameStateChanged
                {
                    GameState = (int)GameState.PRE_GAME,
                    GameId = game.GameData.GameId
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyGameStateChange);
        }
    }
}