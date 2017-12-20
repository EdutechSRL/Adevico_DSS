using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    [Serializable]
    public class DTO_ListTemplate
    {
        public virtual Int64 Id { get; set; }
        public virtual String Name { get; set; }
        public virtual TemplateType Type { get; set; }
        public virtual DateTime? Creation { get; set; }
        public virtual DateTime? LastModify { get; set; }
        //public virtual Int64 IdWorkingVersion { get; set; }
        public virtual IList<DTO_ListTemplateVersion> TemplateVersions { get; set; }
        public virtual IList<DTO_ListService> Services { get; set; }
        public virtual DTO_ListPermission Permissions { get; set; }

        public virtual Boolean IsSystem { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual Boolean HasDraft { get; set; }
        public virtual Boolean HasActive { get; set; }
        public virtual Boolean HasDefinitive { get; set; }

        public virtual BaseStatusDeleted Deleted { get; set; }

    }
}
