using Blaze.Components.GameReporting.Handlers;
using Servers;
using Servers.Blaze.Models;
using Blaze.MessageLists;

namespace Blaze
{
    public static class GameReportingComponentRouter
    {
        public static async Task Handle(User user, byte[] receivedPacket)
        {
            GameReportingMessage gameReportingMessage = (GameReportingMessage)TdfUtils.GetCommandFromPacket(receivedPacket);

            switch (gameReportingMessage)
            {
                case GameReportingMessage.submitOfflineGameReport:
                    await SubmitOfflineGameReportHandler.HandleRequest(user, receivedPacket);
                    break;
                default:
                    Console.WriteLine($"{gameReportingMessage} command is not implemented!");
                    await ServerUtils.SendError(user, receivedPacket, ServerUtils.ErrorCode.ERR_COMMAND_NOT_FOUND);
                    break;
            }
        }
    }
}