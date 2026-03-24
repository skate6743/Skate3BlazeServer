using Blaze.Tdf.Attributes;

namespace Blaze.Components.Util.Models
{
    public struct FilteredUserText
    {
        [TdfMember("DIRT")]
        public int Result;

        [TdfMember("UTXT")]
        public string FilteredText;
    }
}
