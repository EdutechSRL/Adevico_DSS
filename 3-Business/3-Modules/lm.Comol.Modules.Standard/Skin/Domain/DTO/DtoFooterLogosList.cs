using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Domain.DTO
{
    public class DtoFooterLogosList
    {
        public IList<Domain.FooterLogo> Logos { get; set; }
        public IList<Domain.DTO.DtoSkinLanguage> Languages { get; set; }
    }
}
