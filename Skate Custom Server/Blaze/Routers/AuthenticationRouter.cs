using Blaze.Components.Authentication;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze
{
    public static class AuthenticationComponentRouter
    {
        public static async Task Handle(User user, byte[] receivedPacket)
        {
            AuthenticationMessage authenticationMessage = (AuthenticationMessage)TdfUtils.GetCommandFromPacket(receivedPacket);

            switch (authenticationMessage)
            {
                case AuthenticationMessage.acceptTos:
                    await ServerUtils.SendEmptyResponse(user, receivedPacket);
                    break;
                case AuthenticationMessage.ps3Login:
                    await Ps3LoginHandler.HandleRequest(user, receivedPacket);
                    break;
                case AuthenticationMessage.createWalUserSession:
                    await CreateWalUserSessionHandler.HandleRequest(user, receivedPacket);
                    break;
                case AuthenticationMessage.getEntitlements:
                    await ServerUtils.SendEmptyResponse(user, receivedPacket);
                    break;
                default:
                    Console.WriteLine($"{authenticationMessage} command is not implemented!");
                    await ServerUtils.SendError(user, receivedPacket, ServerUtils.ErrorCode.ERR_COMMAND_NOT_FOUND);
                    break;
            }
        }
    }
}