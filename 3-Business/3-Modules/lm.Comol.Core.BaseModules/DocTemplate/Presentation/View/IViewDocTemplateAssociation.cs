using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public interface IViewDocTemplateAssociation : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        lm.Comol.Core.DomainModel.ModuleObject Source { get; set; }
        Boolean AllowPreview { set; }
        Boolean isInitialized { get; set; }
        Boolean isInAjaxPanel { get; set; }
        Boolean AllowSelect { get; set; }
        String DestinationUrl { get; }
        long CurrentIdModule { get; set; }
        DTO_sTemplateVersion SelectedItem { get; }
        List<DTO_sTemplate> Items { get; set; }

        //void InitializeControl();
        //void InitializeControl(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName, Boolean allowAdd, Boolean allowEdit);

        void InitializeControl(long idTemplate, long idVersion, long idModule);
        void InitializeControl(long idTemplate, long idVersion, long idModule,lm.Comol.Core.DomainModel.ModuleObject source);

        void LoadTemplates(List<DTO_sTemplate> templates);
        void LoadEmptyTemplate();
        void DisplaySessionTimeout();
    }
}