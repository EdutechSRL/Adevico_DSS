using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewModuleSkinAdd : IViewModuleSkinBase
    {
        Boolean isInitialized { get; set; }
        Boolean AllowAdd { set; }
        void RedirectToEdit();
        void DisplayForm(lm.Comol.Core.DomainModel.ModuleObject item,Boolean allowAdd);
    }
}
