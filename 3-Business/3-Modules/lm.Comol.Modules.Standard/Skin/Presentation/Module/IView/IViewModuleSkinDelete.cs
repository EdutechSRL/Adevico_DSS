using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewModuleSkinDelete : IViewModuleSkinBase
    {
        long PreloadedIdSkin { get; }
        Boolean FullSkinManagement { get; }
        Boolean isInitialized { get; set; }
        Boolean AllowDelete { set; }
        void LoadSkinInfo(long idSkin,String name,Boolean allowEdit);
        void DisplayUnknownItem();
    }
}