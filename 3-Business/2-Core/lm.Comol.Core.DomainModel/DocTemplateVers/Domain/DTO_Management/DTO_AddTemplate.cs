using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    public class DTO_AddTemplate
    {
        public TemplateType Type { get; set; }
        public IList<DTO_AddServices> Services { get; set; }

        public Boolean IsActiveDefault { get; set; }
        public Boolean IsActiveSystem { get; set; }
        public Boolean IsActiveSkin { get; set; }
        public Boolean IsActiveExternal { get; set; }

        public String Name { get; set; }
    }
}
