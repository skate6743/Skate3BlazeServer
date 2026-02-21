using Blaze.Components.Gamemanager.Commands;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class JoinGameHandler
    {
        public static async Task HandleRequest(User matchmaker, byte[] packetBytes)
        {
            var request = BlazeMessage.CreateModelFromRequest<JoinGameRequest>(packetBytes);

            // Get lobby hosts blaze id
            User inviterUser = ServerGlobals.Users[request.User.BlazeId];
            if (inviterUser.CurrentGame == null)
            {
                await ServerUtils.SendError(matchmaker, packetBytes, ServerUtils.ErrorCode.GAMEMANAGER_ERR_PERMISSION_DENIED);
                return;
            }
            uint gameId = inviterUser.CurrentGame.GameData.GameId;

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new JoinGameResponse {
                    GameId = gameId,
                    JGS = 0 // Not sure what JGS is for, but it's in joinGame responses
                });

            await matchmaker.Stream.WriteAsync(response.Serialize());

            await GameManagerUtils.UserJoinGame(matchmaker, inviterUser.CurrentGame, false);
        }
    }
}