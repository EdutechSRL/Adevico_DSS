using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using lm.Comol.Core.DomainModel.DocTemplate;
using lm.Comol.Core.DomainModel.DocTemplateVers;
using DTO = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public interface IViewDocTemplateList : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        //ModuleDocTemplate CurrentPermission { get; set; }
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);

        DTO.Management.TemplateFilter Filter { get; }
        DTO.Management.TemplateOrderCol OrderBy { get; }
        Boolean OrderAscending { get; }

        String TemplateBaseUrl { get; }

        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetHeader { get; }
        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetFooter { get; }


        void SendUserAction(int idCommunity, int idModule, ModuleDocTemplate.ActionType action);
        void SendUserAction(int idCommunity, int idModule,long idTemplate, ModuleDocTemplate.ActionType action);

        void LoadTemplates(IList<DTO.Management.DTO_ListTemplate> templates);

        String TemplateBasePath { get; }
        String TemplateTempPath { get; }
    }
}
