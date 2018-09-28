using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewSkinHeaderLogo : IViewSkinEditorLogos
    {
        long IdLogoDefaultLanguage{ get; set; }

        void LoadItems(IList<Domain.DTO.DtoHeadLogoLang> items);
    }
}
