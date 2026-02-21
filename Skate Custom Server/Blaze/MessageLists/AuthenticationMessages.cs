namespace Blaze.MessageLists
{
    public enum AuthenticationMessage : ushort
    {
        createAccount = 10,
        updateAccount = 20,
        updateParentalEmail = 28,
        getAccount = 30,
        grantEntitlement = 31,
        getEntitlements = 32,
        hasEntitlement = 33,
        getUseCount = 34,
        decrementUseCount = 35,
        getAuthToken = 36,
        getHandoffToken = 37,
        listEntitlement2 = 38,
        acceptTos = 41,
        getTosInfo = 42,
        consumecode = 44,
        checkAgeReq = 51,
        logout = 70,
        listPersonas = 100,
        loginPersona = 110,
        xboxLogin = 170,
        ps3CreateAccount = 180,
        ps3AssociateAccount = 190,
        ps3Login = 200,
        createWalUserSession = 230
    }
}
