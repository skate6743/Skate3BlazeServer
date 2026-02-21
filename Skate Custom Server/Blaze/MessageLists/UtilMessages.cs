namespace Blaze.MessageLists
{
    public enum UtilMessage : ushort
    {
        fetchClientConfig = 1,
        ping = 2,
        setClientData = 3,
        localizeStrings = 4,
        getTelemetryServer = 5,
        getTickerServer = 6,
        preAuth = 7,
        postAuth = 8,
        userSettingsLoad = 10,
        userSettingsSave = 11,
        userSettingsLoadAll = 12,
        userSettingsLoadAllForUserId = 13,
        filterForProfanity = 20,
        fetchQosConfig = 21,
        setClientMetrics = 22,
        setConnectionState = 23
    }
}
