using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers;
//using lm.Comol.Core.DomainModel.DocTemplate;        //SOLO PER ACTION!!!  POI TOGLIERE!

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public interface IViewDocTemplatePreview : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Boolean PreloadFromList { get; }
        //String PreloadBackUrl { get; }
        Int64 PreloadIdTemplate { get; }
        Int64 PreloadIdVersion { get; }


        Int64 CurrentIdTemplate { get; set; }
        Int64 CurrentIdVersion { get; set; }

        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
        void DisplayUnknownTemplate();
        void LoadTemplate(TemplateVers.Domain.DTO.ServiceExport.DTO_Template template);
        //void SetBackUrl(string url);
        //void SetEditUrl(string url);
        //void SetBackToListUrl(string url);
        void SendUserAction(int idCommunity, int idModule, long idTemplate, TemplateVers.ModuleDocTemplate.ActionType action);

        //lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter GetSysHeader();
        //lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter 
        TemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter 
GetSysFooter();
        TemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
GetSysHeader();
        //lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter GetConfigHeaderTemplate();
        //lm.Comol.Core.DomainModel.DocTemplate.TemplateHeaderFooter GetConfigFooterTemplate();

        String TemplateBaseUrl { get; }
    }
}
