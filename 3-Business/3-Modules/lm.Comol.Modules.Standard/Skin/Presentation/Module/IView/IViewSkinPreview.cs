using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.Skin.Domain;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewSkinPreview: iDomainView
    {
        Int32 PreloadedIdModule { get; }
        Int32 PreloadedIdCommunity { get; }
        long PreloadedIdItem { get; }
        SkinDisplayType PreloadedIdItemType { get; }
        String CurrentModule { get; set; }
        void InitializeSkin(lm.Comol.Core.DomainModel.Helpers.ExternalPageContext content);
        void DisplayContentByService(String moduleCode);
        void DisplaySessionTimeout();
    }
}