using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    [Serializable]
    public class DTO_ElementText : DTO_Element
    {
        public virtual String Text { get; set; }
        public virtual Boolean IsHTML { get; set; }
    }
}
