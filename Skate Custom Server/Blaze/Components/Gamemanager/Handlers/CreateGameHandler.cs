using Blaze.Components.Gamemanager.Commands;
using Blaze.Components.Gamemanager.Models;
using Servers;
using Blaze.GamemanagerComponent;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze.Components.Gamemanager.Handlers
{
    public class CreateGameHandler
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

        public static async Task HandleRequest(User joiningPlayer, byte[] packetBytes)
        {
            // Make sure player doesn't have a lobby running already
            if (joiningPlayer.CurrentGame != null)
            {
                await ServerUtils.SendError(joiningPlayer, packetBytes, ServerUtils.ErrorCode.GAMEMANAGER_ERR_PERMISSION_DENIED);
                return;
            }

            uint gameId = (uint)ServerGlobals.GetNextGameId();
            Console.WriteLine($"Creating game with gameId {gameId}");

            var request = BlazeMessage.CreateModelFromRequest<CreateGameRequest>(packetBytes);

            var response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new JoinGameResponse
                {
                    GameId = gameId
                });

            await joiningPlayer.Stream.WriteAsync(response.Serialize());

            var hostInfo = new HostInfo
            {
                PlayerId = joiningPlayer.Session.BlazeId,
                SlotId = 0
            };

            var filteredAttributes = new Dictionary<string, string>();
            if (request.GameAttributes.Count <= _validKeys.Length)
            {
                foreach (var kvp in request.GameAttributes)
                {
                    if (_validKeys.Contains(kvp.Key) && kvp.Value.Length <= 30)
                    {
                        filteredAttributes[kvp.Key] = kvp.Value;
                    }
                }
            }

            var replicatedGameData = new ReplicatedGameData
            {
                AdminPlayerList = new List<uint> { joiningPlayer.Session.BlazeId },
                GameAttributes = filteredAttributes,
                SlotCapacities = new List<ushort> { 6, 0 },
                GameId = gameId,
                GameName = joiningPlayer.UserIdentification.Name,
                GameSettings = request.GameSettings,
                GameState = (int)GameState.INITIALIZING,
                HostConnections = request.HostConnections,
                TopologyHostSessionId = Convert.ToUInt32(joiningPlayer.Session.UserId),
                MaxPlayerCapacities = 6,
                NetworkQosData = joiningPlayer.ExtendedData.QosData,
                NetworkTopology = (int)GameNetworkTopology.PEER_TO_PEER_FULL_MESH,
                PersistedGameId = request.PersistedGameId,
                PersistedGameIdSecret = request.PersistedGameIdSecret,
                TopologyHost = hostInfo,
                PlatformHost = hostInfo,
                QueueCapacity = request.QueueCapacity,
                VoipTopology = (int)VoipTopology.VOIP_PEER_TO_PEER,
                GameProtocolVersionString = request.GameProtocolVersionString
            };

            var game = new Game();
            game.GameData = replicatedGameData;

            ReplicatedGamePlayer playerData = GameManagerUtils.CreateReplicatedGamePlayer(joiningPlayer, game);
            var player = new Player
            {
                PlayerData = playerData,
                UserData = joiningPlayer
            };

            game.Players.Add(player);
            game.HostId = joiningPlayer.UserIdentification.BlazeId;
            ServerGlobals.Games[gameId] = game;

            await ServerUtils.SendNotificationToUser(
                joiningPlayer,
                new NotifyJoinGame
                {
                    Error = 0,
                    GameData = replicatedGameData,
                    Players = new List<ReplicatedGamePlayer> { playerData },
                    MatchmakingId = 123
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyJoinGame
            );

            joiningPlayer.CurrentGame = game;
        }
    }
}