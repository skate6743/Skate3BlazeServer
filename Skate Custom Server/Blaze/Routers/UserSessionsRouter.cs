using Blaze.Components.UserSessions.Handlers;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze
{
    public static class UserSessionsComponentRouter
    {
        public static async Task Handle(User user, byte[] receivedPacket)
        {
            UserSessionMessage userSessionMessage = (UserSessionMessage)TdfUtils.GetCommandFromPacket(receivedPacket);

            switch (userSessionMessage)
            {
                case UserSessionMessage.updateHardwareFlags:
                    await UpdateHardwareFlagsHandler.HandleRequest(user, receivedPacket);
                    break;
                case UserSessionMessage.updateNetworkInfo:
                    await UpdateNetworkInfoHandler.HandleRequest(user, receivedPacket);
                    break;
                case UserSessionMessage.lookupUsers:
                    await LookupUsersHandler.HandleRequest(user, receivedPacket);
                    break;
                default:
                    Console.WriteLine($"{userSessionMessage} command is not implemented!");
                    await ServerUtils.SendError(user, receivedPacket, ServerUtils.ErrorCode.ERR_COMMAND_NOT_FOUND);
                    break;
            }
        }
    }
}