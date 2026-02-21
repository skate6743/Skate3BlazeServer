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

            if (File.Exists("spoofed_usernames.json"))
            {
                string usernamesJson = File.ReadAllText("spoofed_usernames.json");
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
