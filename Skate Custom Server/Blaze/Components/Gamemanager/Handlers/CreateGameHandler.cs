using Blaze.Components.Gamemanager.Commands;
using Blaze.Components.Gamemanager.Models;
using Servers;
using Blaze.GamemanagerComponent;
using Servers.Blaze.Models;
using Blaze.MessageLists;
using Blaze.Components.UserSessions.Models;

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

        public static async Task HandleRequest(User creator, byte[] packetBytes)
        {
            // Make sure player doesn't have a lobby running already
            if (creator.CurrentGame != null)
            {
                await ServerUtils.SendError(creator, packetBytes, ServerUtils.ErrorCode.GAMEMANAGER_ERR_PERMISSION_DENIED);
                return;
            }

            uint gameId = (uint)ServerGlobals.GetNextGameId();

            var request = BlazeMessage.CreateModelFromRequest<CreateGameRequest>(packetBytes);

            var response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new JoinGameResponse
                {
                    GameId = gameId
                });

            await creator.Stream.WriteAsync(response.Serialize());

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

            var platformHostInfo = new HostInfo
            {
                PlayerId = creator.Session.BlazeId,
                SlotId = 1
            };

            var topologyHostInfo = new HostInfo
            {
                PlayerId = 123,
                SlotId = 0
            };

            var game = new Game();

            // Reserve and start Dirtycast server for lobby
            ushort relayServerPort = Convert.ToUInt16(17000 + gameId);
            int attempts = 0;
            while (ServerGlobals.LobbyRelayServers.Any(x => x.Value.Port == relayServerPort))
            {
                gameId = ServerGlobals.GetNextGameId();
                relayServerPort = Convert.ToUInt16(17000 + gameId);
                if (++attempts > 500)
                {
                    await ServerUtils.SendError(creator, packetBytes, ServerUtils.ErrorCode.GAMEMANAGER_ERR_PERMISSION_DENIED);
                    return;
                }
            }
            NetworkAddress relayServer = ServerUtils.ReserveLobbyRelayServer(game, relayServerPort);
            
            var replicatedGameData = new ReplicatedGameData
            {
                AdminPlayerList = new List<uint> { 123, creator.Session.BlazeId },
                GameAttributes = filteredAttributes,
                SlotCapacities = new List<ushort> { 6, 0 },
                GameId = gameId,
                GameName = creator.UserIdentification.Name,
                GameSettings = request.GameSettings,
                GameState = (int)GameState.INITIALIZING,
                HostConnections = new List<NetworkAddress> { relayServer },
                TopologyHostSessionId = Convert.ToUInt32(creator.Session.UserId),
                MaxPlayerCapacities = 6,
                NetworkQosData = creator.ExtendedData.QosData,
                NetworkTopology = (int)GameNetworkTopology.PEER_TO_PEER_DIRTYCAST_FAILOVER,
                PersistedGameId = request.PersistedGameId,
                PersistedGameIdSecret = request.PersistedGameIdSecret,
                TopologyHost = topologyHostInfo,
                PlatformHost = platformHostInfo,
                QueueCapacity = request.QueueCapacity,
                VoipTopology = (int)VoipTopology.VOIP_PEER_TO_PEER,
                GameProtocolVersionString = request.GameProtocolVersionString
            };

            game.GameData = replicatedGameData;

            ReplicatedGamePlayer playerData = GameManagerUtils.CreateReplicatedGamePlayer(creator, game, true);
            var player = new Player
            {
                PlayerData = playerData,
                UserData = creator
            };

            game.Players.Add(player);
            game.HostId = creator.UserIdentification.BlazeId;
            ServerGlobals.Games[gameId] = game;

            await ServerUtils.SendNotificationToUser(
                creator,
                new NotifyJoinGame
                {
                    Error = 0,
                    GameData = replicatedGameData,
                    Players = new List<ReplicatedGamePlayer> { playerData },
                    MatchmakingId = 123
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyJoinGame);

            creator.CurrentGame = game;
            creator.GamePlayer = player;
        }
    }
}