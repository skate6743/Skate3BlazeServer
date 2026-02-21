namespace NPTicket.Types;

public readonly struct TicketVersion
{
    public byte Major { get; }
    public byte Minor { get; }

    internal TicketVersion(byte major, byte minor)
    {
        Major = major;
        Minor = minor;
    }
}