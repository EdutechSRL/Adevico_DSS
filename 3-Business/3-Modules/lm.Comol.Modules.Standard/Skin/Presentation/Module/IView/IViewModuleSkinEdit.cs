using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewModuleSkinEdit : IViewModuleSkinBase
    {
        long PreloadedIdSkin { get; }
        Boolean FullSkinManagement { get; }
        Boolean isInitialized { get; set; }
        Boolean AllowEdit { set; }
        void LoadAvailableViews(List<SkinView> views);
        void LoadSkinInfo(long idSkin,String name,Boolean allowEdit, bool OverrideVoidFooterLogos);
        void DisplayUnknownItem();

        
    }
}