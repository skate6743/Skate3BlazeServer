using Blaze.Components.Gamemanager.Models;
using Blaze.Components.UserSessions.Models;
using Blaze.GamemanagerComponent;
using Blaze.MessageLists;
using Servers;
using Servers.Blaze.Models;
using System.Net;

namespace Blaze.Components.Gamemanager
{
    public class GameManagerUtils
    {
        public static async Task UserJoinGame(User matchmaker, Game game, bool joiningFromMatchmaking)
        {
            // Not cleanest solution but wait for all joining players to be connected first
            while (true)
            {
                lock (game.Lock)
                {
                    if (!matchmaker.isMatchmaking)
                        return;

                    if (game.Players.All(plr => plr.PlayerData.PlayerState == (int)PlayerState.ACTIVE_CONNECTED))
                        break;
                }
                await Task.Delay(100);
            }

            uint gameId = game.GameData.GameId;

            ReplicatedGamePlayer replicatedPlayer = CreateReplicatedGamePlayer(matchmaker, game);

            var newPlayer = new Player
            {
                PlayerData = replicatedPlayer,
                UserData = matchmaker
            };

            List<Player> snapshot;
            lock (game.Lock)
            {
                if (game.Players.Count >= 6)
                {
                    _ = ServerUtils.SendNotificationToUser(
                        matchmaker,
                        new NotifyMatchmakingFinished
                        {
                            Fit = 100,
                            MaxFit = 100,
                            GameId = 0, // Sending 0 makes the game redirect to createGame command for making lobby
                            MatchmakingResult = (int)MatchmakingResult.SESSION_ERROR_GAME_SETUP_FAILED,
                            MatchmakingSessionId = 123
                        },
                        BlazeComponent.Gamemanager,
                        (ushort)GameManagerNotifications.NotifyMatchmakingFinished);

                    return;
                }

                game.Players.Add(newPlayer);
                snapshot = game.Players.ToList();
                game.AcceptingRelayConnections = true;
                _ = ConfirmPlayerConnectivity(matchmaker);
            }
            
            // Notify matchmaker to join new game
            List<ReplicatedGamePlayer> playerList = snapshot.Select(player => player.PlayerData).ToList();

            await ServerUtils.SendNotificationToUser(
                matchmaker,
                new NotifyJoinGame
                {
                    Error = 0,
                    GameData = game.GameData,
                    Players = playerList,
                    MatchmakingId = 123
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyJoinGame);

            if (joiningFromMatchmaking)
            {
                await ServerUtils.SendNotificationToUser(
                    matchmaker,
                    new NotifyMatchmakingFinished
                    {
                        Fit = 100,
                        MaxFit = 100,
                        GameId = game.GameData.GameId,
                        MatchmakingResult = (int)MatchmakingResult.SUCCESS_JOINED_EXISTING_GAME,
                        MatchmakingSessionId = 123
                    },
                    BlazeComponent.Gamemanager,
                    (ushort)GameManagerNotifications.NotifyMatchmakingFinished);
            }

            // Notify players ingame about new player joining
            var playerJoiningNotification = new NotifyPlayerJoining
            {
                GameId = game.GameData.GameId,
                PlayerData = replicatedPlayer
            };

            foreach (Player player in snapshot)
            {
                if (player.UserData != matchmaker)
                {
                    await ServerUtils.SendNotificationToUser(
                        player.UserData,
                        playerJoiningNotification,
                        BlazeComponent.Gamemanager,
                        (ushort)GameManagerNotifications.NotifyPlayerJoining);
                }
            }

            matchmaker.CurrentGame = game;
            matchmaker.gamePlayer = newPlayer;
            matchmaker.isMatchmaking = false;
        }

        public static byte FindFreeSlot(Game game)
        {
            lock (game.Lock)
            {
                if (game.Players.Count > 0)
                {
                    for (byte slot = 1; slot <= 6; slot++)
                    {
                        if (!game.Players.Any(x => x.PlayerData.SlotId == slot))
                            return slot;
                    }
                }
            }

            return 1;
        }

        public static async Task ConfirmPlayerConnectivity(User user)
        {
            await Task.Delay(20000);

            if (user.gamePlayer != null && user.CurrentGame != null)
            {
                if (user.gamePlayer.PlayerData.PlayerState == (int)PlayerState.ACTIVE_CONNECTING)
                    await RemoveUserFromGame(user.gamePlayer, user.CurrentGame, (int)PlayerRemovedReason.PLAYER_CONN_LOST);
            }

            return;
        }

        public static ReplicatedGamePlayer CreateReplicatedGamePlayer(User user, Game game, bool creatingGame = false)
        {
            long epochMicroseconds =
            (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000) +
            (DateTime.UtcNow.Ticks % TimeSpan.TicksPerMillisecond) / 10;

            byte slotId = FindFreeSlot(game);

            return new ReplicatedGamePlayer
            {
                Locale = 1701729619, // enUS
                GameId = game.GameData.GameId,
                DisplayName = user.Session.PersonaDetails.DisplayName,
                NetworkQosData = user.ExtendedData.QosData,
                PlayerAttributes = new Dictionary<string, string> { { "dlc_mask", "499" } },
                PlayerId = user.Session.BlazeId,
                PlayerNetwork = new NetworkAddress { }, // Empty NetworkAddress union to force Dirtycast connection
                SlotId = slotId,
                SlotType = 0,
                PlayerState = creatingGame ? (int)PlayerState.ACTIVE_CONNECTED : (int)PlayerState.ACTIVE_CONNECTING,
                Team = 0xFFFF,
                TeamIndex = 0xFFFF,
                JoinDate = epochMicroseconds
            };
        }

        public static async Task AssignNewHost(Game game, uint oldHostId = 0, uint newHostId = 0)
        {
            if (game != null)
            {
                Player newHost;

                // If new hosts id is not specified, just find any player with different PlayerId
                lock (game.Lock)
                {
                    if (newHostId == 0)
                        newHost = game.Players.Where(x => x.PlayerData.PlayerId != oldHostId).First();
                    else
                        newHost = game.Players.Where(x => x.PlayerData.PlayerId == newHostId).First();
                }
                
                byte newHostSlot = newHost.PlayerData.SlotId;

                if (newHostId == 0)
                    newHostId = newHost.PlayerData.PlayerId;

                var hostInfo = new HostInfo
                {
                    PlayerId = newHostId,
                    SlotId = newHostSlot
                };

                game.GameData.PlatformHost = hostInfo;

                game.GameData.AdminPlayerList = new List<uint> { 123, newHostId };

                game.HostId = newHost.PlayerData.PlayerId;

                // Notify all players in lobby about host migration
                await ServerUtils.SendNotificationToPlayers(
                    game,
                    new NotifyAdminListChange
                    {
                        AdminPlayerId = newHostId,
                        GameId = game.GameData.GameId,
                        Operation = (int)UpdateAdminListOperation.GM_ADMIN_MIGRATED,
                        UpdaterPlayerId = oldHostId
                    },
                    BlazeComponent.Gamemanager,
                    (ushort)GameManagerNotifications.NotifyAdminListChange);
            }
        }

        public static async Task RemoveUserFromGame(Player playerToRemove, Game game, int removeReason)
        {
            if (playerToRemove.UserData.CurrentGame == game)
            {
                User userData = playerToRemove.UserData;

                uint hostId = game.GameData.PlatformHost.PlayerId;
                uint userBlazeId = userData.UserIdentification.BlazeId;

                uint gid = game.GameData.GameId;

                var playerRemovedNotification = new NotifyPlayerRemoved
                {
                    GameId = gid,
                    PlayerId = userBlazeId,
                    RemovalReason = removeReason
                };

                List<Player> snapshot;

                lock (game.Lock)
                {
                    snapshot = game.Players.ToList();
                    game.Players.Remove(playerToRemove);

                    // If lobby has no players left Remove it
                    if (game.Players.Count == 0)
                    {
                        _= ServerUtils.SendNotificationToUser(
                            playerToRemove.UserData,
                            playerRemovedNotification,
                            BlazeComponent.Gamemanager,
                            (ushort)GameManagerNotifications.NotifyPlayerRemoved);

                        // Destroy lobby relay server
                        _= ServerGlobals.LobbyRelayServers[game].StopAsync();
                        ServerGlobals.LobbyRelayServers.TryRemove(game, out _);
                        ServerGlobals.Games.TryRemove(game.GameData.GameId, out _);

                        playerToRemove.UserData.CurrentGame = null;
                        playerToRemove.UserData.gamePlayer = null;
                        return;
                    }
                }

                // If player was host and left then migrate host
                if (hostId == userBlazeId)
                {
                    await AssignNewHost(game, playerToRemove.PlayerData.PlayerId);
                }

                foreach (Player player in snapshot)
                {
                    await ServerUtils.SendNotificationToUser(
                    player.UserData,
                    playerRemovedNotification,
                    BlazeComponent.Gamemanager,
                    (ushort)GameManagerNotifications.NotifyPlayerRemoved);
                }
            }

            playerToRemove.UserData.CurrentGame = null;
            playerToRemove.UserData.gamePlayer = null;
        }
    }
}
