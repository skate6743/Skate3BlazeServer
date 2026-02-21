using Blaze.Tdf.Attributes;

namespace Blaze.GamemanagerComponent
{
    public struct NotifyMatchmakingFinished
    {
        [TdfMember("FIT")]
        public uint Fit;

        [TdfMember("GID")]
        public uint GameId;

        [TdfMember("MAXF")]
        public uint MaxFit;

        [TdfMember("MSID")]
        public uint MatchmakingSessionId;

        [TdfMember("RSLT")]
        public int MatchmakingResult;

        [TdfMember("USID")]
        public uint UserId;
    }
}
