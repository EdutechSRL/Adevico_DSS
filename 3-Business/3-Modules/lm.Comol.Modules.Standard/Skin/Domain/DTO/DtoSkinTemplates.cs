using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoSkinTemplates
    {
        public IList<DtoSkinTemplate> HeaderTemplates { get; set; }
        public IList<DtoSkinTemplate> FooterTemplates { get; set; }

        public Int64 CurrentFooterTemplareID { get; set; }
        public Int64 CurrentHeaderTemplareID { get; set; }
    }
}
