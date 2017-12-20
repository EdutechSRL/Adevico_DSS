using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    [Serializable]
    public class DTO_Modules
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 IdModule { get; set; }
        public virtual Boolean IsActive { get; set; }

        public virtual String ModuleCode { get; set; }
        public virtual String ModuleName { get; set; }
    }
}
