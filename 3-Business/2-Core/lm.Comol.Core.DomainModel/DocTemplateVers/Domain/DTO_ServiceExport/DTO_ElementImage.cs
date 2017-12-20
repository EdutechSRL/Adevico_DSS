using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    [Serializable]
    public class DTO_ElementImage : DTO_Element
    {
        public virtual string Path { get; set; }
        public virtual Int16 Width { get; set; }
        public virtual Int16 Height { get; set; }
    }
}
