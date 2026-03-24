using Blaze.Components.Util.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.Components.Util.Commands
{
    public struct FilterForProfanityResponse
    {
        [TdfMember("TLST")]
        public List<FilteredUserText> FilteredTextList;
    }
}
