using Blaze.Components.GameReporting.Commands;
using Blaze.Components.GameReporting.Notifications;
using Blaze.MessageLists;
using Servers;
using Servers.Blaze.Models;
using Blaze.Components.GameReporting.Models;

namespace Blaze.Components.GameReporting.Handlers
{
    public class SubmitOfflineGameReportHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            await ServerUtils.SendEmptyResponse(user, packetBytes);

            await ServerUtils.SendNotificationToUser(
                user,
                new ResultNotification
                {
                    BlazeError = 0,
                    FinalResult = true,
                    GameReportingId = 0,
                    Report = new Report
                    {
                        Finished = true,
                        GameReportingId = 0,
                        GameTypeId = 0,
                        Process = true
                    }
                },
                BlazeComponent.GameReporting,
                (ushort)GameReportingNotification.ResultNotification);
        }
    }
}