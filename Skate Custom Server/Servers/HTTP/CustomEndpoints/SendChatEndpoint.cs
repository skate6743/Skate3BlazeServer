using Blaze.Components.Gamemanager.Models;
using Blaze.Components.UserSessions.Models;
using Blaze.GamemanagerComponent;
using Blaze.MessageLists;
using Servers.Blaze.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace Servers.HTTP.CustomEndpoints
{
    public class SendChatEndpoint
    {
        private static Dictionary<string, List<DateTime>> _chatRate = new();
        private static Dictionary<string, DateTime> _chatTimeouts = new();

        private static Dictionary<string, string> _conversions = new Dictionary<string, string>
        {
            { "1", "i" },
            { "!", "i" },
            { "4", "a" },
            { "0", "o" },
            { "3", "e" },
            { "5", "s" },
            { "7", "t" },
            { "8", "b" },
            { "9", "g" },
            { "2", "z" },
            { "l", "i" }
        };

        private static bool CheckForVulgarity(string message)
        {
            var badWords = new List<string>();

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "badwords.txt")))
                badWords = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "badwords.txt"))
                    .Select(x => x.Trim().ToLower())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

            if (badWords.Count == 0)
                return false;

            string toCheck = message.ToLower();

            foreach (var kv in _conversions)
                toCheck = toCheck.Replace(kv.Key, kv.Value);

            toCheck = Regex.Replace(toCheck, @"(.)\1+", "$1");

            toCheck = Regex.Replace(toCheck, @"[^a-z]", "");

            foreach (var bad in badWords)
            {
                if (toCheck.Contains(bad))
                    return true;
            }

            return false;
        }

        private static string[] SplitMessage(string input, int maxLength = 40)
        {
            var result = new List<string>();

            while (input.Length > maxLength)
            {
                int splitIndex = input.LastIndexOf(' ', maxLength);

                if (splitIndex <= 0) // no space found
                    splitIndex = maxLength;

                result.Add(input.Substring(0, splitIndex).Trim());
                input = input.Substring(splitIndex).TrimStart();
            }

            if (input.Length > 0)
                result.Add(input);

            return result.ToArray();
        }

        private static bool IsRateLimited(string token)
        {
            var now = DateTime.UtcNow;

            // Check timeout
            if (_chatTimeouts.TryGetValue(token, out var timeout))
            {
                if (timeout > now)
                    return true;

                _chatTimeouts.Remove(token);
            }

            if (!_chatRate.ContainsKey(token))
                _chatRate[token] = new List<DateTime>();

            var messages = _chatRate[token];

            // remove messages older than 10 seconds
            messages.RemoveAll(t => (now - t).TotalSeconds > 10);

            messages.Add(now);

            if (messages.Count > 7)
            {
                _chatTimeouts[token] = now.AddMinutes(1);
                _chatRate.Remove(token);
                return true;
            }

            return false;
        }

        public static async Task SendChat(HttpListenerContext ctx)
        {
            var req = ctx.Request;

            try
            {
                string token = req.QueryString["token"] ?? "";

                if (IsRateLimited(token))
                    return;

                string msg = req.QueryString["msg"] ?? "";
                if (string.IsNullOrWhiteSpace(msg) || msg.Length > 80)
                    return;

                User? user = ServerGlobals.Users.Values.Where(x => x.Session.BlazeToken == token).FirstOrDefault();

                if (user != null && user.CurrentGame != null && user.Stream.Socket.RemoteEndPoint != null)
                {
                    // IP matching based on Blaze token so people can't send chats with others Blaze tokens
                    IPEndPoint userTCPEndpoint = (IPEndPoint)user.Stream.Socket.RemoteEndPoint;
                    string userTCPEndpointIP = userTCPEndpoint.Address.ToString();
                    if (userTCPEndpointIP != ctx.Request.RemoteEndPoint.Address.ToString())
                        return;

                    Game game = user.CurrentGame;

                    bool isValid = Regex.IsMatch(msg, @"^[a-zA-Z0-9 ?!/\\,._'():;-]*$");
                    bool isOffensive = CheckForVulgarity(msg);

                    if (!isValid || isOffensive)
                        return;

                    int chunkSize = 45;
                    string[] result = SplitMessage(msg, chunkSize);

                    List<uint> ids = new List<uint>();
                    bool first = true;
                    uint tempPlayerId = 1337;
                    foreach (string messageChunk in result)
                    {
                        ids.Add(tempPlayerId);
                        string message = first ? $"[{user.UserIdentification.Name}]: {messageChunk}<B" : $"{messageChunk}<B";

                        var tempPlayer = new ReplicatedGamePlayer
                        {
                            DisplayName = message,
                            PlayerId = tempPlayerId,
                            PlayerState = (int)PlayerState.QUEUED,
                            SlotId = 9,
                            NetworkQosData = new NetworkQosData { NatType = (int)NatType.NAT_TYPE_STRICT },
                            PlayerNetwork = new NetworkAddress { },
                            GameId = game.GameData.GameId,
                            PlayerAttributes = new Dictionary<string, string>() { { "dlc_mask", "499" } }
                        };

                        await ServerUtils.SendNotificationToPlayers(
                        game,
                        new NotifyPlayerJoining
                        {
                            GameId = game.GameData.GameId,
                            PlayerData = tempPlayer
                        },
                        BlazeComponent.Gamemanager,
                        (int)GameManagerNotifications.NotifyPlayerJoining);
                        first = false;
                        tempPlayerId++;
                    }

                    foreach (uint id in ids)
                    {
                        await ServerUtils.SendNotificationToPlayers(
                        game,
                        new NotifyPlayerRemoved
                        {
                            GameId = game.GameData.GameId,
                            PlayerId = id
                        },
                        BlazeComponent.Gamemanager,
                        (int)GameManagerNotifications.NotifyPlayerRemoved);
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}