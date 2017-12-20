using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    [Serializable]
    public class DTO_sTemplateVersion
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 IdTemplate { get; set; }
        public virtual int Version { get; set; }
        public virtual Boolean IsDraft { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual DateTime? Lastmodify { get; set; }
        public virtual Boolean IsSelected { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
    }
}
