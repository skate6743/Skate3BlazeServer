using Blaze.Components.Gamemanager.Commands;
using Servers;
using Blaze.Components.Gamemanager.Models;
using Blaze.GamemanagerComponent;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class StartMatchmakingHandler
    {
        public static string[] FilteredAttributes = { "is_private", "is_ranked", "challenge_key", "challenge_type" };

        public static async Task HandleRequest(User matchmaker, byte[] packetBytes)
        {
            // Make sure player doesn't have a lobby running already
            if (matchmaker.CurrentGame != null)
            {
                await ServerUtils.SendError(matchmaker, packetBytes, ServerUtils.ErrorCode.GAMEMANAGER_ERR_PERMISSION_DENIED);
                return;
            }

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new StartMatchmakingResponse
                {
                    MatchmakingId = 123
                });

            await matchmaker.Stream.WriteAsync(response.Serialize());

            var request = BlazeMessage.CreateModelFromRequest<StartMatchmakingRequest>(packetBytes);

            var wantedAttributes = request.MatchmakingAttributes;
            
            foreach (var kv in ServerGlobals.Games)
            {
                Game game = kv.Value;

                if (game.GameData.GameState != (int)GameState.PRE_GAME)
                    continue;

                bool hasSpace;
                lock (game.Lock)
                {
                    hasSpace = game.Players.Count < 6;
                }

                if (hasSpace)
                {
                    var gameAttributes = game.GameData.GameAttributes;

                    bool foundGame = true;
                    foreach (string attribute in FilteredAttributes)
                    {
                        if (wantedAttributes.ContainsKey(attribute) && gameAttributes.ContainsKey(attribute))
                        {
                            if (wantedAttributes[attribute] != gameAttributes[attribute])
                                foundGame = false;
                        }
                    }

                    if (!foundGame)
                        continue;

                    await GameManagerUtils.UserJoinGame(matchmaker, game, true);
                    return;
                }
            }

            // In cases where matchmaking via Quick Match, a challenge_key won't be provided and we can't make a new lobby due to that
            bool fromQuickplay = !wantedAttributes.ContainsKey("challenge_key");
            
            await ServerUtils.SendNotificationToUser(
                matchmaker,
                new NotifyMatchmakingFinished
                {
                    Fit = 100,
                    MaxFit = 100,
                    GameId = 0, // Sending 0 makes the game redirect to createGame command for making lobby
                    MatchmakingResult = fromQuickplay ? (int)MatchmakingResult.SESSION_ERROR_GAME_SETUP_FAILED : (int)MatchmakingResult.SUCCESS_JOINED_EXISTING_GAME,
                    MatchmakingSessionId = 123
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyMatchmakingFinished);
        }
    }
}