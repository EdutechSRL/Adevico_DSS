using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    [Serializable]
    public class DTO_ListService
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 IdModule { get; set; }
        public virtual String Code { get; set; }
        public virtual String Name { get; set; }

        public virtual DTO_ListTemplate Template { get; set; }
    }
}
