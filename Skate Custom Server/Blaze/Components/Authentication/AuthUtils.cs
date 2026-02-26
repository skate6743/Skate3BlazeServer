using Blaze.Components.Authentication.Models;
using Newtonsoft.Json;
using NPTicket;
using Servers;
using System.Net;

namespace Blaze.Components.Authentication
{
    public class AuthUtils
    {
        public static SessionDetails CreateNewSessionDetails(Ticket ps3Ticket)
        {
            uint id = ServerGlobals.GetNextUserId();

            string displayName = ps3Ticket.Username;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string usernamesPath = Path.Combine(basePath, "spoofed_usernames.json");

            if (File.Exists(usernamesPath))
            {
                string usernamesJson = File.ReadAllText(usernamesPath);
                var nameSpoofs = JsonConvert.DeserializeObject<Dictionary<string, string>>(usernamesJson);

                if (nameSpoofs != null)
                {
                    if (nameSpoofs.ContainsKey(displayName))
                    {
                        displayName = nameSpoofs[displayName];
                    }
                }
            }

            var session = new SessionDetails
            {
                BlazeId = id,
                IsFirstLogin = false,
                BlazeToken = "1282100f_1e21c860f435814f8f25495ef53eea2d",
                LastLoginTime = 0,
                Email = "nobody@nobody.com",
                UserId = id,
                PersonaDetails =
                {
                    DisplayName = displayName,
                    LastLoginTime = 0,
                    PersonaId = id
                }
            };

            return session;
        }
    }
}
