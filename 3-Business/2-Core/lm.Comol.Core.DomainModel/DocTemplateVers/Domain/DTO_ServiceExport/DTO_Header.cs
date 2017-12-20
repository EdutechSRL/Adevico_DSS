using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    public class DTO_HeaderFooter
    {
        public virtual DTO_Element Left { get; set; }
        public virtual DTO_Element Center { get; set; }
        public virtual DTO_Element Right { get; set; }
    }
}
