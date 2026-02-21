using Blaze.Tdf.Attributes;

namespace Blaze.Components.GameReporting.Commands
{
    public struct GameReport
    {
        [TdfMember("GRID")]
        public uint GameReportingId;
    }
}
