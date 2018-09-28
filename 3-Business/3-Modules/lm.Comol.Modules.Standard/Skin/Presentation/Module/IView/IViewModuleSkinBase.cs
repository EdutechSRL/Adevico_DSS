using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.Skin.Domain;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewModuleSkinBase : iDomainView
    {
        Int32 PreloadedIdCommunity { get; }
        SkinType PreloadedSkinType { get; }
        Int32 PreloadedIdModule { get; }
        long PreloadedIdItem { get; }
        Int32 PreloadedIdItemType { get; }
        String PreloadedBackUrl { get; }

        long IdSkin { get; set; }
        String BackUrl { get; set; }
        lm.Comol.Core.DomainModel.ModuleObject Source { get; set; }
        SkinType SkinType { get; set; }

        Boolean HasPermissionForItem(lm.Comol.Core.DomainModel.ModuleObject item);
        void DisplaySessionTimeout();
        void DisplayNoPermission();
      
    }
}