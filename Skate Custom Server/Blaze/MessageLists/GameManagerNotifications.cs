namespace Blaze.MessageLists
{
    public enum GameManagerNotifications : ushort
    {
        NotifyMatchmakingFinished = 10,
        NotifyMatchmakingAsyncStatus = 12,
        NotifyGameCreated = 15,
        NotifyGameRemoved = 16,
        NotifyJoinGame = 20,
        NotifyPlayerJoining = 21,
        NotifyPlayerJoinCompleted = 30,
        NotifyGroupPreJoinedGame = 35,
        NotifyPlayerRemoved= 40,
        NotifyQueueChanged = 41,
        NotifyHostMigrationFinished = 60,
        NotifyHostMigrationStart = 70,
        NotifyPlatformHostInitialized = 71,
        NotifyGameAttribChange = 80,
        NotifyPlayerAttribChange = 90,
        NotifyPlayerCustomDataChange = 95,
        NotifyGameStateChange = 100,
        NotifyGameSettingsChange = 110,
        NotifyGameCapacityChange = 111,
        NotifyGameReset = 112,
        NotifyGameReportingIdChange = 113,
        NotifyGameSessionUpdated = 115,
        NotifyPlayerStateChange = 116,
        NotifyPlayerTeamChange = 117,
        NotifyGameTeamIdChange = 118,
        NotifyGameListUpdate = 201,
        NotifyAdminListChange = 202,
        NotifyCreateDynamicDedicatedServerGame = 220
    }

}
