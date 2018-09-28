using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoShare
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 SkinId { get; set; }
    }

    public class DtoShareCommunity : DtoShare
    {
        public virtual Int32 CommunityId { get; set; }
    }

    public class DtoShareOrganization : DtoShare
    {
        public virtual Int32 OrganizationId { get; set; }
    }
    public class DtoShareModule : DtoShare
    {
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdService { get; set; }
        public virtual Int32 OwnerTypeID { get; set; }
        public virtual long OwnerLongID { get; set; }
        public virtual String OwnerFullyQualifiedName { get; set; }
    }
}
