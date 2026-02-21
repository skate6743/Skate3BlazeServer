using Blaze.Components.Gamemanager.Commands;
using Blaze.Components.Gamemanager.Notifications;
using Blaze.GamemanagerComponent;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class SetGameSettingsHandler
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

            var request = BlazeMessage.CreateModelFromRequest<SetGameSettingsRequest>(packetBytes);
            game.GameData.GameSettings = request.GameSettings;

            await ServerUtils.SendNotificationToPlayers(
                game,
                new NotifyGameSettingsChanged
                {
                    GameSettings = request.GameSettings,
                    GameId = request.GameId
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyGameSettingsChange);
        }
    }
}