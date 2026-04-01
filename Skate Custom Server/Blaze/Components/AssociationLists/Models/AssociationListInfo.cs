using Blaze.Tdf.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Blaze.Components.Authentication.Models
{
    public struct AssociationListInfo
    {
        [TdfMember("ALML")]
        public List<ListMemberInfo> ListMemberInfoVector;

        [TdfMember("BOID")]
        public ulong BlazeObjId;

        [TdfMember("LID")]
        public uint Id;

        [TdfMember("LMS")]
        public uint MaxSize;

        [TdfMember("LNM")]
        public string Name;

        [TdfMember("RFLG")]
        public bool Rollover;

        [TdfMember("SFLG")]
        public bool Subscprition;
    }
}
