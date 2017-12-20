using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewTemplatePreview : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long PreloadIdTemplate { get; }
        long PreloadIdVersion { get; }
        Boolean PreloadForPortal { get; }
        Int32 PreloadIdCommunity { get; }
        Int32 PreloadIdOrganization { get; }
        Int32 PreloadIdModule { get; }
        String PreloadModuleCode { get; }
        lm.Comol.Core.DomainModel.ModuleObject PreloadModuleObject { get; }

        String TagTranslation { get; }
        List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> CurrentTranslations { get; set; }
        List<lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder> GetContentPlaceHolders(List<string> modulesCodes);
        List<String> ContentModules { get; set; }
        long ContentIdTemplate { get; set; }
        long ContentIdVersion { get; set; }
        dtoSelectorContext ContainerContext { get; set; }

        Boolean HasModulePermissions(String moduleCode, long permissions, Int32 idCommunity, Int32 profileType, lm.Comol.Core.DomainModel.ModuleObject obj);

        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule, String moduleCode);
        void InitializeMailSettings(lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, Boolean editSender, Boolean editSubject, Boolean editSignature);
        void InitializeControls(List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> tTemplates, List<LanguageItem> inUse, LanguageItem current);
        void LoadTemplate(lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation content);
        void DisplayNoTemplateFound();
        void DisplayNoTemplateVersionFound();
        void DisplayLastVersionInfo();
        void DisplayVersionInfo(Int32 versionNumber);
    }
}