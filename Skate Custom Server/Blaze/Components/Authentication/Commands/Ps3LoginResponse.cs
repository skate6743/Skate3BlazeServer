using Blaze.Tdf.Attributes;
using Blaze.Components.Authentication.Models;

namespace Blaze.Components.Authentication.Commands
{
    public struct Ps3LoginResponse
    {
        [TdfMember("AGUP")]
        public bool IsUnderage;

        [TdfMember("PRIV")]
        public string PrivacyPolicyUrl;

        [TdfMember("SESS")]
        public SessionDetails SessionDetails;

        [TdfMember("SPAM")]
        public bool IsSpammable;

        [TdfMember("THST")]
        public string TosHost;

        [TdfMember("TURI")]
        public string TosHostUrl;
    }
}
