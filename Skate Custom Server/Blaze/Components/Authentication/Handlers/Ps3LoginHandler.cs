using Blaze.Components.Authentication.Commands;
using Blaze.Components.Authentication.Models;
using Blaze.Components.UserSessions.Models;
using NPTicket;
using NPTicket.Verification;
using NPTicket.Verification.Keys;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Authentication
{
    public class Ps3LoginHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            var loginRequest = BlazeMessage.CreateModelFromRequest<Ps3LoginRequest>(packetBytes);

            Ticket ps3Ticket = Ticket.ReadFromBytes(loginRequest.ps3Ticket);

            var verifier = new TicketVerifier(loginRequest.ps3Ticket, ps3Ticket, RpcnSigningKey.Instance);
            if (!verifier.IsTicketValid())
            {
                await ServerUtils.SendError(user, packetBytes, ServerUtils.ErrorCode.AUTH_ERR_INVALID_PS3_TICKET);
                return;
            }

			Console.WriteLine($"RPCN user {ps3Ticket.Username} has signed in!");

			SessionDetails sessionDetails = AuthUtils.CreateNewSessionDetails(ps3Ticket);

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
                BestPingSite = "sjc",
                DataMap = new Dictionary<uint, uint> { { 458823, 0 } }
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

            user.Authenticated = true;
            ServerGlobals.Users[sessionDetails.BlazeId] = user;
        }
    }
}