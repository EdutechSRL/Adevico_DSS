using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    [Serializable]
    public class DTO_sTemplate
    {
        public virtual Int64 Id { get; set; }
        public virtual String Name { get; set; }

        public virtual Boolean IsActive { get; set; }
        public virtual Boolean IsSystem { get; set; }
        public virtual Boolean HasDraft { get; set; }
        public virtual Boolean HasActive { get; set; }
        public virtual Boolean HasDefinitive { get; set; }

        public virtual List<DTO_sTemplateVersion> Versions { get; set; }
        public virtual List<DTO_sServiceContent> Services { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }

    }
}