using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    public class DTO_AddServices
    {
        public virtual Int32 ServicesId { get; set; }
        public virtual String ServiceName { get; set; }
        public virtual String ServiceCode { get; set; }
        public virtual Boolean Selected { get; set; }
    }
}
