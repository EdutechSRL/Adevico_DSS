using System;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewBaseEdit : IViewBase 
    {
        System.Guid PreloadCurrentSessionId { get; }
        Int32 PreloadFromIdCommunity { get; }
        String PreloadFromModuleCode { get; }
        long PreloadFromModulePermissions { get; }

        //Int32 PreloadFromIdCommunity { get; }
        //Int32 PreloadFromIdModule { get; }
        String PreloadBackUrl { get; }
        long PreloadIdTemplate { get; }
        long PreloadIdVersion { get; }
        Boolean PreloadPreview { get; }
        //Boolean AllowSaveDraft { get; set; }
        Boolean AllowSave { get; set; }
        Boolean InputReadOnly { get; set; }

        long IdTemplate { get; set; }
        long IdVersion { get; set; }
        Int32 IdTemplateCommunity { get; set; }
        Int32 IdTemplateModule { get; set; }
        String Portalname { get; }

        void UnableToReadUrlSettings();
        void SetBackUrl(String url);
        Boolean HasPermissionForObject(ModuleObject source);
        void HideUserMessage();
        void DisplayUnknownTemplate();
        void LoadWizardSteps(int idCommunity, List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> steps);
        lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages GetModulePermissions(string moduleCode, Int32 idModule, long permissions, Int32 idCommunity, Int32 profileType, lm.Comol.Core.DomainModel.ModuleObject obj);
        void GoToUrl(string url);

        String GetEncodedBackUrl();
    }
}