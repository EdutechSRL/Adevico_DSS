using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.Skin.Domain
{
    public enum SkinShareType
    {
        NotAssociate = -2,
        All = -1,
        Portal = 0,
        Community = 1,
        Organization = 2,
        Module = 3
    }

    public class Skin_Share : DomainBaseObjectMetaInfo<long>
    {
        public virtual SkinShareType Discriminator { get; protected set; }

        public virtual Skin Skin { get; set; }
    }

    public class Skin_ShareCommunity : Skin_Share
    {
        public virtual Int32 CommunityId { get; set; }
    }

    public class Skin_ShareOrganization : Skin_Share
    {
        public virtual Int32 OrganizationId { get; set; }
    }

    public class Skin_ShareModule : Skin_Share
    {
        public virtual Int32 IdOrganization { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual Int32 OwnerTypeID { get; set; }
        public virtual long OwnerLongID { get; set; }
        public virtual String OwnerFullyQualifiedName { get; set; }
    }
}