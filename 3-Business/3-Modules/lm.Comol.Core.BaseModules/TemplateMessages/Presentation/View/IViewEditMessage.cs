using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewEditMessage : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        String PortalName { get; }
        Boolean AllowTemplateSelection { get; set; }
        Boolean AllowDelete { get; set; }
        Boolean AllowPreview { get; set; }
        Boolean MantainRecipients { get; set; }
        String ModuleCode { get; set; }
        String ObjectTranslatedName { get; set; }
        EditMessageMode CurrentMode { get; set; }
        List<dtoTemplateTranslation> CurrentTranslations { get; set; }
        lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings CurrentSettings { get; }
        lm.Comol.Core.DomainModel.ModuleObject CurrentObject { get; set; }
        long IdSelectedTemplate { get; set; }
        long IdSelectedVersion { get; set; }
        long IdSelectedAction { get; set; }
        UserSelection SelectionMode { get; set; }
        Int32 ContainerIdCommunity { get; set; }
        List<String> ContentModules { get; set; }

        List<lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder> GetContentPlaceHolders(List<string> modulesCodes);

        //List<OwnerType> availableSaveAs,
        void InitializeControl( UserSelection mode, Boolean allowSelectTemplate, Int32 idCommunity, String currentCode = "", lm.Comol.Core.DomainModel.ModuleObject obj = null,String objectName="", long idTemplate = 0, long idVersion = 0, long idAction = 0);
        void InitializeMailSettings(lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, Boolean editSender, Boolean editSubject, Boolean editSignature);
        void LoadEditor(List<dtoTemplateTranslation> translations, String currentCode, Boolean displayShortText, Boolean onlyShortText, List<BaseLanguageItem> availableLanguages, List<LanguageItem> inUse, LanguageItem current);
        void UpdateTranslationSelector(List<BaseLanguageItem> availableLanguages, List<LanguageItem> inUse, LanguageItem language, dtoTemplateTranslation translation);
        void UpdateTranslation(LanguageItem language, dtoTemplateTranslation translation);
        void DisplayMessagePreview(Boolean allowSendMail,String languageCode,ItemObjectTranslation translation, List<String> modules, List<ChannelSettings> cSettings,Int32 idCommunity, ModuleObject obj=null);
        void DisplayMessagePreview(Boolean allowSendMail, String languageCode, ItemObjectTranslation translation, List<String> modules, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mSettings, Int32 idCommunity, ModuleObject obj = null);
        LanguageItem RemoveCurrentTranslation();

        void DisplayTemplateSaved(OwnerType savedAs);
        void DisplayTemplateUnSaved(OwnerType savedAs);

        List<dtoTextualRecipient> GetTextualRecipients();
        List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> SelectedRecipients();
        void DisplayModuleSelector();
        void InitializeModuleRecipientsSelector(Boolean isPortal, Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject obj, long idTemplate, long idVersion, Boolean isTemplateCompliant, List<dtoTemplateTranslation> translation);

        void DisplayEmptyMessage();
        void DisplayEmptyTranslations(List<String> languages);
        void DisplayNoRecipients();
        void LoadAvailableAddresses(List<BaseLanguageItem> items);
        void LoadAvailableAddresses(List<BaseLanguageItem> items,List<dtoTextualRecipient> recipients);
        void SendMessage(List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> recipients, List<dtoTemplateTranslation> translations, String currentCode);
        void DisplayUnableToSendMessage();
        void DisplayMessageSentTo(Int32 sentTo, Int32 skipped);
    }
}
