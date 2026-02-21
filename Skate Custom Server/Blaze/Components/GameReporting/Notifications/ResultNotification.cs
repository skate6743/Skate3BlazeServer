using Blaze.Tdf.Attributes;
using Blaze.Components.GameReporting.Models;

namespace Blaze.Components.GameReporting.Notifications
{
    public struct ResultNotification
    {
        [TdfMember("EROR")]
        public int BlazeError;

        [TdfMember("FNL")]
        public bool FinalResult;

        [TdfMember("GRID")]
        public uint GameReportingId;

        [TdfMember("RPRT")]
        public Report Report;
    }
}
