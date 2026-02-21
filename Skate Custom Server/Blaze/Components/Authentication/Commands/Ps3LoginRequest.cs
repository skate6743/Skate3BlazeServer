using Blaze.Tdf.Attributes;

namespace Blaze.Components.Authentication.Commands
{
    public struct Ps3LoginRequest
    {
        [TdfMember("MAIL")]
        public string email;

        [TdfMember("TCKT")]
        public byte[] ps3Ticket;
    }
}
