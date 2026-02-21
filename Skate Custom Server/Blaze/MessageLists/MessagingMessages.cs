namespace Blaze.MessageLists
{
    public enum MessagingMessage : ushort
    {
        sendMessage = 1,
        fetchMessages = 2,
        purgeMessages = 3,
        touchMessages = 4,
        getMessages = 5
    }
}
