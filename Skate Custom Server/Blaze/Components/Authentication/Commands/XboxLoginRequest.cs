using Blaze.Tdf.Attributes;

namespace Blaze.Components.Authentication.Commands
{
    public struct XboxLoginRequest
    {
        [TdfMember("GTAG")]
        public string Gamertag;

        [TdfMember("XUID")]
        public ulong XUID;
    }
}
