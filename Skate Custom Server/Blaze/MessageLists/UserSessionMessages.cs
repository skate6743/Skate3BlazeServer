namespace Blaze.MessageLists
{
    public enum UserSessionMessage : ushort
    {
        lookupUserInformation = 1,
        lookupUsersInformation = 2,
        fetchExtendedData = 3,
        updatePingSiteLatency = 4,
        updateExtendedDataAttribute = 5,
        assignUserToGroup = 6,
        removeUserFromGroup = 7,
        updateHardwareFlags = 8,
        getPermissions = 9,
        getAccessGroup = 10,
        checkOnlineStatus = 11,
        lookupUser = 12,
        lookupUsers = 13,
        updateNetworkInfo = 20,
        listDefaultAccessGroup = 21,
        listAuthorization = 22,
        lookupUserGeoIPData = 23,
        overrideUserGeoIPData = 24,
        setUserInfoAttribute = 25
    }
}
