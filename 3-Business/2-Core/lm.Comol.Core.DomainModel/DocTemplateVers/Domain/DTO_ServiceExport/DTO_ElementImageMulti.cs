using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport
{
    public class DTO_ElementImageMulti : DTO_Element
    {
        public IList<DTO_ElementImage> ImgElements { get; set; }
    }
}
