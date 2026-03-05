using Blaze.Components.Authentication.Commands;
using Blaze.Components.Authentication.Models;
using Blaze.Components.UserSessions.Models;
using NPTicket;
using NPTicket.Verification;
using NPTicket.Verification.Keys;
using Servers;
using Servers.Blaze.Models;
using System.Net.Http.Headers;

namespace Blaze.Components.Authentication
{
    public class XboxLoginHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            var req = BlazeMessage.CreateModelFromRequest<XboxLoginRequest>(packetBytes);

			Console.WriteLine($"Xbox user {req.Gamertag} has signed in!");

            uint id = ServerGlobals.GetNextUserId();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string usernamesPath = Path.Combine(basePath, "spoofed_usernames.json");

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
                    DisplayName = req.Gamertag,
                    LastLoginTime = 0,
                    PersonaId = id
                }
            };

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new Ps3LoginResponse
                {
                    IsUnderage = false,
                    IsSpammable = false,
                    SessionDetails = session
                });

            await user.Stream.WriteAsync(response.Serialize());
            

            // Store user session details for later use
            user.Session = session;
            user.ExtendedData = new UserSessionExtendedData
            {
                BestPingSite = "sjc",
                DataMap = new Dictionary<uint, uint> { { 458823, 0 } },
                NetworkAddress = new NetworkAddress
                {
                    IpPairAddress = new IpPairAddress
                    {
                         ExternalIp = new IpAddress
                         {
                             IP =0,
                             Port = 10000
                         },
                         InternalIp = new IpAddress
                         {
                             IP = 0,
                             Port = 10000
                         }
                    }
                }
            };

            user.UserIdentification = new UserIdentification
            {
                AccountId = user.Session.UserId,
                AccountLocale = 1701729619, // enUS
                BlazeId = user.Session.BlazeId,
                IsOnline = true,
                Name = user.Session.PersonaDetails.DisplayName,
                PersonaId = user.Session.PersonaDetails.PersonaId
            };

            user.IsAuthenticated = true;
            ServerGlobals.Users[session.BlazeId] = user;
        }
    }
}