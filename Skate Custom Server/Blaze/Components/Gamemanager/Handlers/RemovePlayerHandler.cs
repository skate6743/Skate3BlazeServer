using Blaze.Components.Gamemanager.Commands;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class RemovePlayerHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            var request = BlazeMessage.CreateModelFromRequest<RemovePlayerRequest>(packetBytes);
            Player playerToKick = user.CurrentGame.Players.Where(x=>x.PlayerData.PlayerId == request.PlayerId).First();

            // Only allow kicking self when not host
            if (playerToKick.UserData != user && user.CurrentGame.HostId != user.UserIdentification.BlazeId)
            {
                await ServerUtils.SendError(user, packetBytes, ServerUtils.ErrorCode.GAMEMANAGER_ERR_PERMISSION_DENIED);
                return;
            }

            await ServerUtils.SendEmptyResponse(user, packetBytes);
            await GameManagerUtils.RemoveUserFromGame(playerToKick, user.CurrentGame, request.RemovalReason);
        }
    }
}