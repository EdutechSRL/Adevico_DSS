using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class Share : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int64 IdGlossary { get; set; }
        public virtual Int64 IdTerm { get; set; }
        public virtual Boolean Visible { get; set; }
        public virtual ShareStatusEnum Status { get; set; }
        public virtual SharePermissionEnum Permissions { get; set; }
        public virtual ShareTypeEnum Type { get; set; }
    }
}