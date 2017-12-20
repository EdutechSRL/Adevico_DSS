using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    [Serializable]
    public class DTO_ListTemplateVersion
    {
        public virtual Int64 Id { get; set; }
        public virtual DTO_ListVersionPermission Permissions { get; set; }
        public virtual int Version { get; set; }

        public virtual Boolean IsActive { get; set; }
        public virtual Boolean IsDraft { get; set; }
        public virtual DateTime? LastModify { get; set; }
        public virtual DateTime? Creation { get; set; }

        public virtual DTO_ListTemplate Template { get; set; }
        //public virtual Boolean IsSystem { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }

        public virtual String LastModifiedBy { get; set; }
    }
}
