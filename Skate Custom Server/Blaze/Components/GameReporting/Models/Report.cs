using Blaze.Tdf.Attributes;

namespace Blaze.Components.GameReporting.Models
{
    public struct Report
    {
        [TdfMember("FNSH")]
        public bool Finished;

        [TdfMember("GRID")]
        public uint GameReportingId;

        [TdfMember("GTYP")]
        public uint GameTypeId;

        [TdfMember("PRCS")]
        public bool Process;
    }
}
