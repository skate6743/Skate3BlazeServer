namespace Blaze.MessageLists
{
    public enum AssociationListMessage : ushort
    {
        addUsersToList = 1,
        removeUsersFromList = 2,
        clearList = 3,
        setUsersToList = 4,
        getListForUser = 5,
        getLists = 6,
        subscribeToLists = 7,
        unsubscribeFromLists = 8
    }
}
