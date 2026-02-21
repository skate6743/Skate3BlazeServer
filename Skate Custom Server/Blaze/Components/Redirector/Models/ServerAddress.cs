using Blaze.Components.UserSessions.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.Components.Redirector.Models
{
    public struct ServerAddress
    {
        [TdfUnionCase(0)]
        public IpAddress? IpAddress;
    }
}
