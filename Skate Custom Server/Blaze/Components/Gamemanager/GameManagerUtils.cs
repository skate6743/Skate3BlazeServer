using Blaze.Components.Gamemanager.Models;
using Blaze.Components.UserSessions.Models;
using Blaze.GamemanagerComponent;
using Blaze.MessageLists;
using Servers.Blaze.Models;
using Servers;

namespace Blaze.Components.Gamemanager
{
    public class GameManagerUtils
    {
        public static async Task UserJoinGame(User matchmaker, Game game, bool joiningFromMatchmaking)
        {
            uint gameId = game.GameData.GameId;

            ReplicatedGamePlayer replicatedPlayer = CreateReplicatedGamePlayer(matchmaker, game);

            List<Player> snapshot;
            lock (game.Lock)
            {
                game.Players.Add(
                    new Player
                    {
                        PlayerData = replicatedPlayer,
                        UserData = matchmaker
                    });
                snapshot = game.Players.ToList();
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

            await ServerUtils.SendNotificationToPlayers(
                game,
                new NotifyPlayerJoinCompleted
                {
                    GameId = game.GameData.GameId,
                    PlayerId = matchmaker.UserIdentification.BlazeId
                },
                BlazeComponent.Gamemanager,
                (ushort)GameManagerNotifications.NotifyPlayerJoinCompleted);

            matchmaker.CurrentGame = game;
        }

        public static byte FindFreeSlot(Game game)
        {
            lock (game.Lock)
            {
                if (game.Players.Count > 0)
                {
                    for (byte slot = 0; slot <= 5; slot++)
                    {
                        if (!game.Players.Any(x => x.PlayerData.SlotId == slot))
                            return slot;
                    }
                }
            }

            return 0;
        }

        public static ReplicatedGamePlayer CreateReplicatedGamePlayer(User user, Game game)
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
                PlayerNetwork = user.ExtendedData.NetworkAddress,
                SlotId = slotId,
                SlotType = 0,
                PlayerState = (int)PlayerState.ACTIVE_CONNECTED,
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
                game.GameData.TopologyHost = hostInfo;

                game.GameData.AdminPlayerList = new List<uint> { newHostId };

                game.HostId = newHost.PlayerData.PlayerId;

                var networkAddress = newHost.UserData.ExtendedData.NetworkAddress;
                game.GameData.HostConnections = new List<NetworkAddress> { networkAddress };

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

        public static async Task RemoveUserFromGame(Player playerToRemove, Game game, int removeReason, bool removedFromDisconnect = false)
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

                if (!removedFromDisconnect)
                {
                    await ServerUtils.SendNotificationToUser(
                        userData,
                        playerRemovedNotification,
                        BlazeComponent.Gamemanager,
                        (ushort)GameManagerNotifications.NotifyPlayerRemoved);
                }

                lock (game.Lock)
                {
                    game.Players.Remove(playerToRemove);

                    // If lobby has no players left Remove it
                    if (game.Players.Count == 0)
                    {
                        ServerGlobals.Games.TryRemove(game.GameData.GameId, out _);
                        playerToRemove.UserData.CurrentGame = null;
                        Console.WriteLine($"{game.GameData.GameId} lobby has been destroyed due to no players!");
                        return;
                    }
                }

                // If player was host and left then migrate host
                if (hostId == userBlazeId)
                {
                    await AssignNewHost(game, playerToRemove.PlayerData.PlayerId);
                }

                await ServerUtils.SendNotificationToPlayers(
                    game,
                    playerRemovedNotification,
                    BlazeComponent.Gamemanager,
                    (ushort)GameManagerNotifications.NotifyPlayerRemoved);
            }

            playerToRemove.UserData.CurrentGame = null;
        }
    }
}
