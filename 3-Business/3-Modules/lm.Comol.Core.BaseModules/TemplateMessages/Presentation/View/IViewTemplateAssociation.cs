using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewTemplateAssociation : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        lm.Comol.Core.DomainModel.ModuleObject Source { get; set; }
        Boolean AllowPreview { set; }
        Boolean isInitialized { get; set; }
        Boolean isInAjaxPanel { get; set; }
        Boolean AllowSelect { get; set; }
        Boolean RaiseSelectionEvent { get; set; }
        //String DestinationUrl { get; }
        //long CurrentIdAction { get; set; }
        //long CurrentIdModule { get; set; }
        //String CurrentModuleCode { get; set; }

        dtoVersionItem SelectedItem { get; }
        List<dtoTemplateItem> Items { get; set; }

        void InitializeControl(long idAction,Int32 idCommunty,long idTemplate, long idVersion, Int32 idModule = 0, String moduleCode = "", lm.Comol.Core.DomainModel.ModuleObject source = null);

        void LoadTemplates(List<dtoTemplateItem> templates);
        void LoadEmptyTemplate();
        void DisplaySessionTimeout();
    }
}