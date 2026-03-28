using Blaze.Components.Authentication.Commands;
using Blaze.Components.Authentication.Models;
using Blaze.Components.UserSessions.Models;
using Microsoft.EntityFrameworkCore;
using NPTicket;
using NPTicket.Verification;
using NPTicket.Verification.Keys;
using Servers;
using Servers.Blaze.Models;
using Servers.Database;
using Servers.Models;

namespace Blaze.Components.Authentication
{
    public class Ps3LoginHandler
    {
        public static bool UserBanned(string username)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bannedusers.txt");
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                return lines.Any(x => x == username);
            }
            return false;
        }

        private class SkateSigningKey : PsnSigningKey
        {
            public override string PublicKeyX => "a93f2d73da8fe51c59872fad192b832f8b9dabde8587233";
            public override string PublicKeyY => "93131936a54a0ea51117f74518e56aae95f6baff4b29f999";
        }

        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            var loginRequest = BlazeMessage.CreateModelFromRequest<Ps3LoginRequest>(packetBytes);

            Ticket ps3Ticket = Ticket.ReadFromBytes(loginRequest.ps3Ticket);

            if (UserBanned(ps3Ticket.Username))
                return;

            // RPCN Ticket Verifying
            var verifier = new TicketVerifier(loginRequest.ps3Ticket, ps3Ticket, RpcnSigningKey.Instance);
            if (verifier.IsTicketValid())
            {
                user.Platform = UserPlatform.RPCS3;
            }
            else
            {
                // PS3 Ticket Verifying if RPCN ticket came back invalid
                verifier = new TicketVerifier(loginRequest.ps3Ticket, ps3Ticket, new SkateSigningKey());
                if (verifier.IsTicketValid())
                {
                    user.Platform = UserPlatform.PS3;
                }
                else
                {
                    await ServerUtils.SendError(user, packetBytes, ServerUtils.ErrorCode.AUTH_ERR_INVALID_PS3_TICKET);
                    return;
                }
            }

            ServerLogger.LogSignIn(ps3Ticket.Username);

            await using var db = new AppDbContext();
            db.Database.Migrate();

            var userData = db.Users.FirstOrDefault(u => u.PsnId == ps3Ticket.UserId && u.Platform == user.Platform);

            if (userData == null)
            {
                userData = new UserDbData
                {
                    Platform = user.Platform,
                    PsnId = ps3Ticket.UserId,
                    DisplayName = ps3Ticket.Username
                };

                db.Users.Add(userData);
                await db.SaveChangesAsync();
            }

			SessionDetails sessionDetails = AuthUtils.CreateNewSessionDetails(ps3Ticket, userData.BlazeId);

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new Ps3LoginResponse
                {
                    IsUnderage = false,
                    IsSpammable = false,
                    SessionDetails = sessionDetails
                });

            await user.Stream.WriteAsync(response.Serialize());
            

            // Store user session details for later use
            user.Session = sessionDetails;
            user.ExtendedData = new UserSessionExtendedData
            {
                DataMap = new Dictionary<uint, uint> { { 458823, 0 } } // enUS locale
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
            ServerGlobals.Users[sessionDetails.BlazeId] = user;
        }
    }
}