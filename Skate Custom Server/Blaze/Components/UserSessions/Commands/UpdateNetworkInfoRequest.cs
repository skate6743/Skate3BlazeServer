using Blaze.Tdf.Attributes;
using Blaze.Components.UserSessions.Models;

namespace Blaze.Components.UserSessions.Commands
{
    public struct UpdateNetworkInfoRequest
    {
        [TdfMember("ADDR")]
        public NetworkAddress NetworkAddress;

        [TdfMember("QDAT")]
        public NetworkQosData QosData;
    }
}