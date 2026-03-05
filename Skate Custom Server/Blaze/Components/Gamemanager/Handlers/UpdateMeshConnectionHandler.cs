using Blaze.Components.Gamemanager.Commands;
using Blaze.GamemanagerComponent;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;
using Blaze.Components.Gamemanager.Models;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class UpdateMeshConnectionHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            if (user.CurrentGame != null)
            {
                List<Player> snapshot;
                lock (user.CurrentGame.Lock)
                {
                    snapshot = user.CurrentGame.Players.ToList();
                }

                Player? plr = snapshot.Where(x => x.PlayerData.PlayerId == user.UserIdentification.BlazeId).FirstOrDefault();

                if (plr != null && plr.PlayerData.PlayerState != (int)PlayerState.ACTIVE_CONNECTED)
                {
                    await ServerUtils.SendNotificationToPlayers(
                        user.CurrentGame,
                        new NotifyPlayerJoinCompleted
                        {
                            GameId = user.CurrentGame.GameData.GameId,
                            PlayerId = user.UserIdentification.BlazeId
                        },
                        BlazeComponent.Gamemanager,
                        (ushort)GameManagerNotifications.NotifyPlayerJoinCompleted);

                    plr.PlayerData.PlayerState = (int)PlayerState.ACTIVE_CONNECTED;
                    user.CurrentGame.AcceptingRelayConnections = false;
                }
            }
        }
    }
}