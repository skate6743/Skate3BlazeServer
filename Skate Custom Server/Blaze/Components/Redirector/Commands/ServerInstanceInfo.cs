using Blaze.Tdf.Attributes;
using Blaze.Components.Redirector.Models;

namespace Blaze.Components.Redirector.Commands
{
    public struct ServerInstanceInfo
    {
        [TdfMember("ADDR")]
        public ServerAddress Address;

        [TdfMember("SECU")]
        public bool Secure;

        [TdfMember("XDNS")]
        public uint DefaultDnsAddress;
    }
}
