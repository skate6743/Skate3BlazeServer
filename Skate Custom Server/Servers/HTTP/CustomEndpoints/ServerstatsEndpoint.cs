using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Servers.Blaze.Models;
using System.Net;

namespace Servers.HTTP.CustomEndpoints
{
    public class ServerstatsEndpoint
    {
        private static string[] _difficulties = { "Easy", "Normal", "Hardcore" };
        private static Dictionary<string, string> _challengeTypes = new Dictionary<string, string>
        {
            { "22", "Freeskate" },
            { "33", "Hall of meat" },
            { "53", "Spot Battle" },
            { "8", "Street Contest" },
            { "15", "Tranny Contest" },
            { "19", "Deathrace" },
            { "51", "Domination" },
            { "4", "S.K.A.T.E" },
            { "32", "1-Up" },
            { "7", "Film" },
            { "38", "Photo" },
            { "9", "Own The Spot" },
            { "52", "Own The Lot" },
            { "31", "Hall of Meat" }
        };
        private static Dictionary<string, string>? _challengeKeys = new Dictionary<string, string>();

        private static Dictionary<string, string> GetGameInfo(Game game, Dictionary<string, string> attributes)
        {
            var converted = new Dictionary<string, string>();

            foreach (var kv in attributes)
            {
                switch (kv.Key)
                {
                    case "difficulty_mode":
                        int.TryParse(kv.Value, out int difficulty);
                        if (difficulty >= 0 && difficulty < _difficulties.Length)
                            converted.Add("Difficulty", _difficulties[difficulty]);
                        break;
                    case "challenge_type":
                        string challengeInfo = "";
                        if (attributes.ContainsKey("is_team_challenge") && attributes["is_team_challenge"] == "true")
                            challengeInfo += "Team ";

                        if (attributes.ContainsKey("is_coop_challenge") && attributes["is_coop_challenge"] == "true")
                            challengeInfo += "Career ";

                        if (_challengeTypes.ContainsKey(kv.Value))
                            converted.Add("Game Mode", challengeInfo + _challengeTypes[kv.Value]);
                        break;
                    case "challenge_key":
                        if (_challengeKeys == null)
                            continue;

                        if (_challengeKeys.ContainsKey(kv.Value))
                        {
                            converted.Add("Map", _challengeKeys[kv.Value]);
                        }
                        break;
                }
            }

            converted.Add("Player Count", game.Players.Count.ToString());

            return converted;
        }

        static ServerstatsEndpoint()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "challenge_keys.json");
            if (File.Exists(path))
                _challengeKeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
        }

        public static async Task<string> GetServerStats(HttpListenerContext ctx)
        {
            uint inLobbies = 0;
            foreach (User user in ServerGlobals.Users.Values)
            {
                if (user.CurrentGame != null)
                    inLobbies++;
            }

            var games = ServerGlobals.Games.Values;

            var convertedAttributes = new List<Dictionary<string, string>>();
            uint gamesCount = 0;
            foreach (Game game in games)
            {
                var attributes = game.GameData.GameAttributes;
                if (attributes.ContainsKey("is_private") && attributes["is_private"] == "false")
                {
                    convertedAttributes.Add(GetGameInfo(game, attributes));
                    gamesCount++;
                }
            }

            return new JObject(
                new JProperty("signed-in", ServerGlobals.Users.Count),
                new JProperty("in-lobbies", inLobbies),
                new JProperty("dirtycast-instances", ServerGlobals.LobbyRelayServers.Count),
                new JProperty("overall-games", ServerGlobals.Games.Count),
                new JProperty("public-games", gamesCount),
                new JProperty("game-infos", JToken.FromObject(convertedAttributes))
            ).ToString();
        }
    }
}