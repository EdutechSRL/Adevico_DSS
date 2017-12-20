using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    public class DTO_EditElementPermission
    {
        public virtual Boolean Preview { get; set; }
        public virtual Boolean Recover { get; set; }
        public virtual Boolean Delete { get; set; }
    }
}
