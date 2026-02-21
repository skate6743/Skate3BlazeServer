using Blaze.Components.Gamemanager.Commands;
using Blaze.GamemanagerComponent;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class SetGameAttributesHandler
    {
        private static string[] _validKeys = {
            "gameCodeVersion",
            "challenge_type",
            "challenge_key",
            "ping_site",
            "is_private",
            "max_players",
            "is_ranked",
            "overall_skill",
            "challenge_skill",
            "is_team_challenge",
            "team_id",
            "previous_game_id",
            "world_key",
            "skatepark_crc",
            "difficulty_mode",
            "allow_proposals",
            "is_coop_challenge",
            "is_free_skate",
            "dlc_mask",
            "skatepark_owner_id"
        };

        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            Game game = user.CurrentGame;
            if (game.HostId != user.UserIdentification.BlazeId)
            {
                await ServerUtils.SendError(user, packetBytes, ServerUtils.ErrorCode.GAMEMANAGER_ERR_PERMISSION_DENIED);
                return;
            }

            await ServerUtils.SendEmptyResponse(user, packetBytes);

            var request = BlazeMessage.CreateModelFromRequest<SetGameAttributesRequest>(packetBytes);

            if (request.GameAttributes.Count <= _validKeys.Length)
            {
                foreach (var kvp in request.GameAttributes)
                {
                    if (_validKeys.Contains(kvp.Key) && kvp.Value.Length <= 30)
                    {
                        game.GameData.GameAttributes[kvp.Key] = kvp.Value;
                    }
                }

                await ServerUtils.SendNotificationToPlayers(
                game,
                new NotifyGameAttributesChanged
                {
                    GameAttributes = request.GameAttributes,
                    GameId = request.GameId
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyGameAttribChange);
            }
        }
    }
}