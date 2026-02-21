using Blaze.Components.Gamemanager.Models;
using Blaze.Components.Gamemanager.Notifications;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class ReplayGameHandler
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

            uint gameId = game.GameData.GameId;

            await ServerUtils.SendNotificationToPlayers(
                game,
                new NotifyGameStateChanged
                {
                    GameState = (int)GameState.PRE_GAME,
                    GameId = gameId
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyGameStateChange);
        }
    }
}