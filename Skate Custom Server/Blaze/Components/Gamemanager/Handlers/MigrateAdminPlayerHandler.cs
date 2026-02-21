using Blaze.Components.Gamemanager.Commands;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class MigrateAdminPlayerHandler
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

            var request = BlazeMessage.CreateModelFromRequest<MigrateHostRequest>(packetBytes);

            await GameManagerUtils.AssignNewHost(game, game.HostId, request.NewHostPlayer);
        }
    }
}