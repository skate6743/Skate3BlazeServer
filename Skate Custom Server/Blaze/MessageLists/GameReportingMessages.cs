namespace Blaze.MessageLists
{
    public enum GameReportingMessage : ushort
    {
        submitGameReport = 1,
        submitOfflineGameReport = 2,
        submitGameEvents = 3,
        getGameReports = 4,
        getGameReportView = 5,
        getGameReportViewInfo = 6,
        getGameReportViewInfoList = 7,
        getGameReportTypes = 8,
        submitTrustedMidGameReport = 100,
        submitTrustedEndGameReport = 101
    }
}
