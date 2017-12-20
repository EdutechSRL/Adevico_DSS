using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel.DocTemplateVers;
using DTO = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public interface IVIewDocTemplateEditSkin : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Int64 TemplateId { get; set; } //SET: SE carico tramite Version, imposto anche relativo Id Tempalte
        Int64 VersionId { get; set; } ////SET: SE carico tramite Tempalte, imposto anche relativo Id Version (draft!)
        ViewEditTemplateSkinElement PreloadedView { get; }

        ViewEditTemplateSkinElement CurrentView { get; set; }

        Boolean SaveOnChangeView { get; }
        Boolean IsAdvancedEdit { get; }

        void ShowError(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.VersionEditError EditError);

        void SetCurrentVersion(DTO.Management.DTO_EditTemplateVersion Version);
        DTO.Management.DTO_EditTemplateVersion GetCurrentVersion();

        //Placeholder Modules
        //void LoadAvailableModules(List<ModuleContent> usedItems);
        void LoadAvailableServices(IList<DTO.Management.DTO_EditItem<ServiceContent>> usedItems);
        //void BindOrganizations(IList<lm.Comol.Modules.Standard. Domain.DTO.DtoSkinOrganization) Organization)
        //List<ModuleContent> GetCurrentModules();
        IList<DTO.Management.DTO_EditItem<ServiceContent>> GetCurrentModules();

        //Base
        void DisplaySessionTimeout();
        void SendUserAction(int idCommunity, int idModule, long idTemplate, ModuleDocTemplate.ActionType action);
        void DisplayNoPermission(int idCommunity, int idModule);

        //Navigation
        String PreloadBackUrl { get; }

        void SetBackUrl(string url);
        void SetPreviewUrl(string url);
        void SetBackToListUrl(string url);

        String TemplateBaseUrl { get; }
        String TemplateBaseTempUrl { get; }

        String TempalteBasePath { get; }
        String TempalteTempPath { get; }

        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetCurrentHeader { get; }
        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetCurrentFooter { get; }

        //Version - Recover
        void RecoverSettings(DTO.Management.DTO_EditItem<Settings> Settings);
        void RecoverPageElement(DTO.Management.DTO_EditItem<PageElement> Element);
        void RecoverSignature(DTO.Management.DTO_EditItem<Signature> Element);
    }



    


}
