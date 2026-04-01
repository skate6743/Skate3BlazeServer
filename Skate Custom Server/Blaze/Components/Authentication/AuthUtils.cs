using Blaze.Components.Authentication.Models;
using Newtonsoft.Json;
using NPTicket;
using Servers;
using System.Security.Cryptography;

namespace Blaze.Components.Authentication
{
    public class AuthUtils
    {
        // Random Blaze token generation
        public static string GenerateToken()
        {
            string token;
            do
            {
                Span<byte> prefix = stackalloc byte[4];
                Span<byte> suffix = stackalloc byte[16];
                RandomNumberGenerator.Fill(prefix);
                RandomNumberGenerator.Fill(suffix);
                token = $"{Convert.ToHexString(prefix).ToLower()}_{Convert.ToHexString(suffix).ToLower()}";
            }
            while (ServerGlobals.Users.Values.Any(u => u.Session.BlazeToken == token));

            return token;
        }

        public static SessionDetails CreateNewSessionDetails(Ticket ps3Ticket, uint blazeId)
        {
            string token = GenerateToken();

            string displayName = ps3Ticket.Username;

            string usernamesPath = Path.Combine(ServerGlobals.BaseDirectory, "spoofed_usernames.json");

            if (File.Exists(usernamesPath))
            {
                string usernamesJson = File.ReadAllText(usernamesPath);
                var nameSpoofs = JsonConvert.DeserializeObject<Dictionary<string, string>>(usernamesJson);

                if (nameSpoofs != null && nameSpoofs.ContainsKey(displayName))
                {
                    displayName = nameSpoofs[displayName];
                }
            }

            return new SessionDetails
            {
                BlazeId = blazeId,
                IsFirstLogin = false,
                BlazeToken = token,
                LastLoginTime = 0,
                Email = "nobody@nobody.com",
                UserId = blazeId,
                PersonaDetails =
                {
                    DisplayName = displayName,
                    LastLoginTime = 0,
                    PersonaId = blazeId,
                    ExternalRef = ps3Ticket.UserId,
                    ExternalRefType = 0x2
                }
            };
        }
    }
}
