using Blaze.Components.Gamemanager.Commands;
using Blaze.Components.Gamemanager.Models;
using Servers;
using Blaze.GamemanagerComponent;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class CancelMatchmakingHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            await ServerUtils.SendEmptyResponse(user, packetBytes);

            user.IsMatchmaking = false;
        }
    }
}