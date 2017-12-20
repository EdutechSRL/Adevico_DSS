using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Business;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;
using lm.Comol.Core.Notification.Domain;
using lm.Comol.Core.MailCommons.Domain.Configurations;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class EditMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            private TemplatesForOtherService otherModuleService;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditMessage View
            {
                get { return (IViewEditMessage)base.View; }
            }
            private TemplateMessagesService Service
            {
                get
                {
                    if (service == null)
                        service = new TemplateMessagesService(AppContext);
                    return service;
                }
            }
            private TemplatesForOtherService OtherModuleService
            {
                get
                {
                    if (otherModuleService == null)
                        otherModuleService = new TemplatesForOtherService(AppContext);
                    return otherModuleService;
                }
            }
            public EditMessagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditMessagePresenter(iApplicationContext oContext, IViewEditMessage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean allowSelectTemplate, Int32 idCommunity, String currentCode = "",lm.Comol.Core.DomainModel.ModuleObject obj=null, long idTemplate = 0, long idVersion = 0, long idAction = 0)
        {
            if (UserContext.isAnonymous)
                View.CurrentMode = Domain.EditMessageMode.None;
            else
            {
                View.ModuleCode = currentCode;
                View.CurrentObject = obj;
                View.ContainerIdCommunity = idCommunity;
                View.AllowTemplateSelection=allowSelectTemplate;
                View.MantainRecipients = false;
                InitializeEditor(currentCode, idTemplate, idVersion, idAction);
            }
        }

        private void InitializeEditor(String currentCode = "", long idTemplate = 0, long idVersion = 0, long idAction = 0)
        {
            Boolean isAdministrative = Service.IsAdministrativeUser(UserContext.CurrentUserID);
            Boolean senderEdit = isAdministrative;

            View.CurrentMode = Domain.EditMessageMode.Edit;
            View.IdSelectedTemplate = idTemplate;
            View.IdSelectedVersion = idVersion;
            View.IdSelectedAction = idAction;

            lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings = null;
            List<dtoTemplateTranslation> translations = new List<dtoTemplateTranslation>();
            TemplateDefinitionVersion version = null;
            if (idVersion > 0)
                version = Service.GetVersion(idVersion);
            if (version == null)
                version = Service.GetLastActiveVersion(idTemplate);

            List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> availableItems = Service.GetAvalableLanguage(version);
            List<lm.Comol.Core.DomainModel.Languages.LanguageItem> items = Service.GetInUseLanguageItems(version);

            if (version != null)
            {
                View.IdSelectedVersion = version.Id;
                View.ContentModules = Service.GetVersionModuleContentCodes(version.Id);   
                
                translations.Add(new dtoTemplateTranslation() { IdLanguage = 0, LanguageCode = "multi", Translation = version.DefaultTranslation.Copy() });
                translations.AddRange(version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => new dtoTemplateTranslation() { IdLanguage = t.IdLanguage, LanguageCode = t.LanguageCode, Translation = t.Translation.Copy() }).ToList());
                if (version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == NotificationChannel.Mail).Any())
                    settings = version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == NotificationChannel.Mail).FirstOrDefault().MailSettings;

                senderEdit = senderEdit && version.Template.Type == TemplateType.System;
                View.LoadEditor(translations, "multi", version.HasShortText, version.OnlyShortText, availableItems, items, items.Where(i => i.IsMultiLanguage).FirstOrDefault());
            }
            else
            {
                View.ContentModules = new List<String>() { currentCode };
                translations.Add(new dtoTemplateTranslation() { IdLanguage = 0, LanguageCode = "multi", Translation = new ItemObjectTranslation() { IsHtml = (settings == null) ? true : settings.IsBodyHtml, Body="", Name= "" } });
                View.LoadEditor(translations, "multi", false, false, availableItems, items, items.Where(i => i.IsMultiLanguage).FirstOrDefault());
            }
            if (settings == null) {
                settings = new lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings();
                settings.CopyToSender = false;
                settings.IsBodyHtml = true;
                settings.NotifyToSender = false;
                settings.SenderType = MailCommons.Domain.SenderUserType.LoggedUser;
                settings.PrefixType = MailCommons.Domain.SubjectPrefixType.SystemConfiguration;
            }
            View.CurrentTranslations = translations;
            View.InitializeMailSettings(settings, senderEdit, isAdministrative, isAdministrative);
        }
        public Boolean SaveAsTemplate(OwnerType t, String prefixName, List<dtoTemplateTranslation> translations, List<String> contentModules, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings, lm.Comol.Core.DomainModel.ModuleObject obj = null)
        {
            Boolean saved = false;
            if (UserContext.isAnonymous)
                View.CurrentMode = Domain.EditMessageMode.None;
            else
            {
                TemplateType type = TemplateType.None;
                dtoBaseTemplateOwner ownership = new dtoBaseTemplateOwner() { IdPerson = UserContext.CurrentUserID, ModuleCode = View.ModuleCode };
                switch (t) { 
                    case OwnerType.Object:
                        ownership.Type = OwnerType.Object;
                        ownership.IdObject = obj.ObjectLongID;
                        ownership.IdObjectCommunity = obj.CommunityID;
                        ownership.IdObjectModule = obj.ServiceID;
                        ownership.IdObjectType = obj.ObjectTypeID;
                        ownership.IsPortal = false;
                        break;
                    case OwnerType.Person:
                        ownership.IsPortal = true;
                        ownership.Type = OwnerType.Person;
                        break;
                    case OwnerType.Module:
                        ownership.IsPortal = false;
                        ownership.IdCommunity = View.ContainerIdCommunity;
                        ownership.IdModule = CurrentManager.GetModuleID(View.ModuleCode);
                        ownership.ModuleCode = View.ModuleCode;
                        ownership.Type = OwnerType.Module;
                        break;
                    default:
                        ownership = null;
                        break;
                }
                if (ownership != null)
                {
                    TemplateDefinition template = Service.AddTemplate(prefixName, type, ownership, contentModules, translations, settings);
                    if (template == null)
                        View.DisplayTemplateUnSaved(t);
                    else {
                        View.DisplayTemplateSaved(t);
                    }
                }
                else
                    View.DisplayTemplateUnSaved(t);
            }
            return saved;
        }

        public void LoadTranslation(LanguageItem language,dtoTemplateTranslation current, Boolean forAdd)
        {
            if (UserContext.isAnonymous)
                View.CurrentMode = Domain.EditMessageMode.None;
            else
            {
                List<dtoTemplateTranslation> translations = View.CurrentTranslations;
                if (translations.Any() && translations.Where(t=> t.IdLanguage== current.IdLanguage).Any()){
                    dtoTemplateTranslation fItem = translations.Where(t=> t.IdLanguage== current.IdLanguage).FirstOrDefault();
                    fItem.Translation=  current.Translation;
                }
                else
                    translations.Add(current);
                
                dtoTemplateTranslation content = translations.Where(t=> t.IdLanguage== language.Id).FirstOrDefault();
                if (content == null)
                {
                    content = new dtoTemplateTranslation { IdLanguage = language.Id, LanguageCode = language.Code, LanguageName = language.Name };
                    if (translations.Where(t => t.LanguageCode == "multi").Any())
                        content.Translation = translations.Where(t => t.LanguageCode == "multi").FirstOrDefault().Translation.Copy();
                    else
                        content.Translation = current.Translation.Copy();
                    translations.Add(content);
                }

                View.CurrentTranslations = translations;
                //if (forAdd)
                //    View.UpdateTranslation(language, content);
                //else
                    View.UpdateTranslationSelector(Service.GetAvalableLanguages(), GetInUseLanguageItems(language, translations,content), language, content);
            }
        }
        public List<lm.Comol.Core.DomainModel.Languages.LanguageItem> GetInUseLanguageItems(LanguageItem language,List<dtoTemplateTranslation> translations, dtoTemplateTranslation content)
        {
            List<lm.Comol.Core.DomainModel.Languages.LanguageItem> items = new List<lm.Comol.Core.DomainModel.Languages.LanguageItem>();

            try
            {
                List<Language> languages = CurrentManager.GetAllLanguages().ToList();
                items = translations.Select(t => new lm.Comol.Core.DomainModel.Languages.LanguageItem() { 
                    Id= t.IdLanguage,
                    Code= t.LanguageCode,
                    IsDefault = languages.Where(l=>l.isDefault && l.Id== t.IdLanguage).Any(),
                    IsEnabled= true,
                    IsMultiLanguage= (t.LanguageCode=="multi"),
                    Name = (String.IsNullOrEmpty(t.LanguageName) && languages.Where(l => l.Id == t.IdLanguage).Any()) ? languages.Where(l => l.Id == t.IdLanguage).Select(l => l.Name).FirstOrDefault() : t.LanguageName,
                    ToolTip = (String.IsNullOrEmpty(t.LanguageName) && languages.Where(l => l.Id == t.IdLanguage).Any()) ? languages.Where(l => l.Id == t.IdLanguage).Select(l => l.Name).FirstOrDefault() : t.LanguageName,
                    Status = (t.IsValid || (content.IdLanguage==t.IdLanguage && content.IsValid)) ? lm.Comol.Core.DomainModel.Languages.ItemStatus.valid : ((content.IsEmpty) ? lm.Comol.Core.DomainModel.Languages.ItemStatus.wrong :lm.Comol.Core.DomainModel.Languages.ItemStatus.warning)
                }).ToList();
            }
            catch (Exception ex)
            {

            }

            return items;
        }
        public void PreviewMessage(dtoTemplateTranslation content)
        {
            long idVersion = View.IdSelectedVersion;
            if (UserContext.isAnonymous)
                View.CurrentMode = Domain.EditMessageMode.None;
            else
            {
                Int32 idCommunity = View.ContainerIdCommunity;
                TemplateDefinitionVersion v = Service.GetVersion(idVersion);
                if (v != null && v.Template != null)
                {
                    List<ChannelSettings> channels = (v.ChannelSettings == null) ? new List<ChannelSettings>() : v.ChannelSettings.Where(s => s.Deleted == BaseStatusDeleted.None).ToList();
                    Boolean sendMail = (channels != null && channels.Where(n => n.Channel == NotificationChannel.Mail).Any()) && v.Template.OwnerInfo.Type == OwnerType.Object;
                    if (sendMail)
                        View.DisplayMessagePreview(sendMail, content.LanguageCode, content.Translation, View.ContentModules, channels, idCommunity, View.CurrentObject);
                    else
                        View.DisplayMessagePreview(true, content.LanguageCode, content.Translation, View.ContentModules,  View.CurrentSettings, idCommunity, View.CurrentObject);
                }
                else
                    View.DisplayMessagePreview(true, content.LanguageCode, content.Translation, View.ContentModules, View.CurrentSettings, idCommunity, View.CurrentObject);
            }
        }
        public void SelectRecipients(LanguageItem language, dtoTemplateTranslation current)
        { 
            if (UserContext.isAnonymous)
                View.CurrentMode = Domain.EditMessageMode.None;
            else
            {
                List<dtoTemplateTranslation> translations = View.CurrentTranslations;
                if (translations.Where(t => t.IdLanguage == language.Id && t.LanguageCode == language.Code).Any())
                    translations.Where(t => t.IdLanguage == language.Id && t.LanguageCode == language.Code).FirstOrDefault().Translation = current.Translation;
                else
                    translations.Add(current);
                View.CurrentTranslations = translations;

                if (translations == null || !translations.Any() || !translations.Where(t => t.IsValid).Any())
                    View.DisplayEmptyMessage();
                else {
                    View.CurrentMode = Domain.EditMessageMode.SelectUsers;
                    List<String> languages = translations.Where(t => !t.IsValid).Select(t => t.LanguageName).ToList();
                    if (languages.Any())
                        View.DisplayEmptyTranslations(languages);

                    List<BaseLanguageItem> items = translations.Where(t => t.IsValid).Select(t => new BaseLanguageItem() { Code = t.LanguageCode, Name = t.LanguageName, ToolTip = t.LanguageName, Id = t.IdLanguage, IsMultiLanguage= (t.Id==0 && t.LanguageCode=="multi") }).OrderByDescending(t=>t.IsMultiLanguage).ThenBy(t=>t.Name).ToList();
                    if (View.MantainRecipients)
                    {
                        if ((View.SelectionMode & UserSelection.FromInputText) > 0 && items.Count > 0)
                            View.LoadAvailableAddresses(items, View.GetTextualRecipients());
                        if ((View.SelectionMode & UserSelection.FromModule) > 0 && items.Count > 0)
                            View.DisplayModuleSelector();
                    }
                    else {
                        View.MantainRecipients = true;
                        if ((View.SelectionMode & UserSelection.FromInputText) > 0 && items.Count > 0)
                            View.LoadAvailableAddresses(items);
                        if ((View.SelectionMode & UserSelection.FromModule) > 0 && items.Count > 0)
                            InitializeModuleSelector(translations);
                    }
                }
            }
        }

        public void AnalyzeMessageToSend(List<dtoTextualRecipient> tRecipients, List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> sRecipients)
        {
            if (UserContext.isAnonymous)
                View.CurrentMode = Domain.EditMessageMode.None;
            else
            {
                if ((tRecipients == null || !tRecipients.Any() || !tRecipients.Where(r => !String.IsNullOrEmpty(r.Addresses)).Any()) && (sRecipients == null || !sRecipients.Any()))
                    View.DisplayNoRecipients();
                else {
                    String moduleCode = View.ModuleCode;
                    Int32 idModule = CurrentManager.GetModuleID(moduleCode);
                    if (sRecipients == null)
                        sRecipients = new List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient>();
                    if (tRecipients.Any()){
                        List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> tmp = new List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient>();
                        foreach(dtoTextualRecipient t in tRecipients.Where(i=> !String.IsNullOrEmpty(i.Addresses))){
                            tmp.AddRange(t.Addresses.Split(',').Where(s => !String.IsNullOrEmpty(s)).Select(s => new lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient()
                            {
                                CodeLanguage = t.LanguageCode,
                                IdLanguage = t.IdLanguage,
                                IdPerson = 0,
                                ModuleCode = moduleCode,
                                IdUserModule = 0,
                                DisplayName = s,
                                MailAddress = s,
                                Type = lm.Comol.Core.MailCommons.Domain.RecipientType.BCC
                            }).ToList());
                        }
                        if (tmp != null) {
                            List<String> internalAddresses = sRecipients.Where(r => r.IsInternal).Select(r => r.MailAddress).ToList();
                            tmp = tmp.Where(a => !internalAddresses.Contains(a.MailAddress)).ToList();
                            if (tmp.Any())
                                sRecipients.AddRange(tmp);
                        }
                    }
                    View.SendMessage(sRecipients , View.CurrentTranslations.Where(t => t.IsValid).ToList(), moduleCode);
                }
            }
        }
        public void SendMessage(SmtpServiceConfig smtpConfig, List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> messages)
        {
            if (UserContext.isAnonymous)
                View.CurrentMode = Domain.EditMessageMode.None;
            else            
            {
                if (messages == null)
                    View.DisplayUnableToSendMessage();
                else {
                    List<dtoTemplateTranslation> translations = View.CurrentTranslations;
                    List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> rSkipped = new List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient>();
                    lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = new lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate(View.CurrentSettings);
                    template.IdTemplate= View.IdSelectedTemplate;
                    TemplateDefinitionVersion version = null;
                    if (View.IdSelectedVersion > 0 || template.IdTemplate > 0)
                        version = Service.GetVersion(template.IdTemplate, View.IdSelectedVersion);
                    if (version != null)
                    {
                        template.IdVersion= version.Id;
                        template.IsTemplateCompliant = IsTemplateCompliant(translations,version);
                    }
                    else
                        template.IsTemplateCompliant=false;
                    if (!template.IsTemplateCompliant) {
                        template.DefaultTranslation = (translations.Where(t => t.LanguageCode == "multi" && t.IdLanguage == 0).Any() ? translations.Where(t => t.LanguageCode == "multi" && t.IdLanguage == 0).FirstOrDefault().Translation.Copy() : null);
                        template.Translations = translations.Where(t => !(t.LanguageCode == "multi" && t.IdLanguage == 0) && t.IsValid).Select(t => new lm.Comol.Core.Mail.Messages.dtoBaseMailTemplateContent()
                        {
                            IdLanguage = t.IdLanguage,
                            LanguageCode = t.LanguageCode,
                            LanguageName = t.LanguageName,
                            Translation = t.Translation
                        }).ToList();
                    }
                    Boolean sent = OtherModuleService.SendMail(CurrentManager.GetPerson(UserContext.CurrentUserID), smtpConfig, template.MailSettings, messages, ref rSkipped, template, View.CurrentObject, View.ModuleCode);
                    Int32 removed = rSkipped.Count + messages.SelectMany(m => m.RemovedRecipients).Count();
                    View.DisplayMessageSentTo(messages.SelectMany(m => m.Recipients).Count() - rSkipped.Count, removed);
                }
            }
        }

        private Boolean IsTemplateCompliant(List<dtoTemplateTranslation> translations, TemplateDefinitionVersion version) {
            Boolean isCompliant = true;
            foreach (dtoTemplateTranslation t in translations) {
                if (t.IdLanguage == 0 && t.LanguageCode == "multi")
                {
                    isCompliant = t.IsCompliant(version.DefaultTranslation);
                }
                else
                {
                    ItemObjectTranslation content = version.GetTranslation(t.LanguageCode, t.IdLanguage);
                    isCompliant = (content != null && t.IsCompliant(content));
                }
                if (!isCompliant)
                    break;
            }

            return isCompliant;
        }

        private void InitializeModuleSelector(List<dtoTemplateTranslation> translations)
        {
            long idTemplate = View.IdSelectedTemplate;
            long idVersion = View.IdSelectedVersion;
            Boolean isTemplateCompliant = false;
            TemplateDefinitionVersion version = null;
            if (idVersion > 0 || idTemplate > 0)
                version = Service.GetVersion(idTemplate, idVersion);
            if (version != null)
            {
                isTemplateCompliant = IsTemplateCompliant(translations, version);
            }
            View.InitializeModuleRecipientsSelector((View.ContainerIdCommunity == 0 && View.CurrentObject.CommunityID == 0), View.ContainerIdCommunity, View.CurrentObject, idTemplate, idVersion, isTemplateCompliant,translations);
        }
    }
}