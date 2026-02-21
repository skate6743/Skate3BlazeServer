using Blaze.Components.AssociationLists;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze
{
    public static class AssociationListsComponentRouter
    {
        public static async Task Handle(User user, byte[] receivedPacket)
        {
            AssociationListMessage associationListMessage = (AssociationListMessage)TdfUtils.GetCommandFromPacket(receivedPacket);

            switch (associationListMessage)
            {
                case AssociationListMessage.getLists:
                    await GetLists.HandleRequest(user, receivedPacket);
                    break;
                default:
                    Console.WriteLine($"{associationListMessage} command is not implemented!");
                    await ServerUtils.SendError(user, receivedPacket, ServerUtils.ErrorCode.ERR_COMMAND_NOT_FOUND);
                    break;
            }
        }
    }
}