using Blaze.Components.Authentication.Models;
using Blaze.Tdf.Attributes;

namespace Blaze.Components.AssociationLists.Models
{
    public struct AssociationListCollectionInfo
    {
        [TdfMember("ALMP")]
        public Dictionary<string, AssociationListInfo> AssociationListInfoByNameMap;
    }
}
