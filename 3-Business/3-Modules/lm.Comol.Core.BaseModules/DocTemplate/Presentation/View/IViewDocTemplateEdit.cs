using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel.DocTemplateVers;
using DTO = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public interface IViewDocTemplateEdit : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Int64 TemplateId { get; set; } //SET: SE carico tramite Version, imposto anche relativo Id Tempalte
        Int64 VersionId { get; set; } ////SET: SE carico tramite Tempalte, imposto anche relativo Id Version (draft!)
        ViewEditTemplateElement PreloadedView { get; }

        ViewEditTemplateElement CurrentView { get; set; }

        Boolean SaveOnChangeView { get; }
        Boolean IsAdvancedEdit { get; }

        void ShowError(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.VersionEditError EditError);

        void SetCurrentVersion(DTO.Management.DTO_EditTemplateVersion Version);
        DTO.Management.DTO_EditTemplateVersion GetCurrentVersion();

        //Placeholder Modules
        //void LoadAvailableModules(List<ModuleContent> usedItems);
        void LoadAvailableServices(IList<DTO.Management.DTO_EditItem<ServiceContent>> usedItems);
        //List<ModuleContent> GetCurrentModules();
        IList<DTO.Management.DTO_EditItem<ServiceContent>> GetCurrentModules();

        //Base
        void DisplaySessionTimeout();
        void SendUserAction(int idCommunity, int idModule, long idTemplate, ModuleDocTemplate.ActionType action);
        void DisplayNoPermission(int idCommunity, int idModule);
        void DisplayNoVersion();

        //Navigation
        String PreloadBackUrl { get; }

        void SetBackUrl(string url);
        void SetPreviewUrl(string url);
        void SetBackToListUrl(string url);
         
        String TemplateBaseUrl { get; }
        String TemplateBaseTempUrl { get; }

        String TemplateBasePath { get; }
        String TemplateTempPath { get; }

        //Version - Recover
        void RecoverSettings(DTO.Management.DTO_EditItem<Settings> Settings);
        void RecoverPageElement(DTO.Management.DTO_EditItem<PageElement> Element);
        void RecoverSignature(DTO.Management.DTO_EditItem<Signature> Element);
    }


}
