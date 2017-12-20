using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Mail.Messages;
using lm.Comol.Core.BaseModules.MailSender.Domain;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public class MessageTemplatePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private MailMessagesService service;
            private lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService tService;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewMessageTemplate View
            {
                get { return (IViewMessageTemplate)base.View; }
            }
            private MailMessagesService Service
            {
                get
                {
                    if (service == null)
                        service = new MailMessagesService(AppContext);
                    return service;
                }
            }
            private lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService TemplatesService
            {
                get
                {
                    if (tService == null)
                        tService = new lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService(AppContext);
                    return tService;
                }
            }
            public MessageTemplatePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public MessageTemplatePresenter(iApplicationContext oContext, IViewMessageTemplate view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            dtoTemplateMessageContext context = GetContext();
            View.ContainerContext = context;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (HasPermission(context))
                    InitializeView(context);
                else
                    View.DisplayNoPermission(context.IdCommunity, context.IdModule, context.ModuleCode);
            }
        }
        private dtoTemplateMessageContext GetContext()
        {
            dtoTemplateMessageContext item = new dtoTemplateMessageContext();
            item.ModuleObject = View.PreloadModuleObject;
            item.IdCommunity = View.PreloadIdCommunity;
            item.IdModule = View.PreloadIdModule;
            item.IdMessageTemplate = View.PreloadIdMessageTemplate;
            item.ModuleCode = View.PreloadModuleCode;

            if (item.IdCommunity == -1 && item.ModuleObject != null)
                item.IdCommunity = item.ModuleObject.CommunityID;
            if (item.IdModule > 0 && String.IsNullOrEmpty(item.ModuleCode))
                item.ModuleCode = CurrentManager.GetModuleCode(item.IdModule);
            else if (item.IdModule == 0 && !String.IsNullOrEmpty(item.ModuleCode))
                item.IdModule = CurrentManager.GetModuleID(item.ModuleCode);
            return item;
        }
        private Boolean HasPermission(dtoTemplateMessageContext context)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            return View.HasModulePermissions(context.ModuleCode, GetModulePermissions(context.IdModule, context.IdCommunity), context.IdCommunity, (p == null) ? (int)UserTypeStandard.Guest : p.TypeID, context.ModuleObject);
        }
        private long GetModulePermissions(Int32 idModule, Int32 idCommunity)
        {
            return CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule);
        }
        private void InitializeView(dtoTemplateMessageContext context) {
            MailMessage message = Service.GetMessage(context.IdMessageTemplate);
            if (message == null)
                View.DisplayNoTemplateFound();
            else {
                Boolean isAdministrative =  Service.IsAdministrativeUser(UserContext.CurrentUserID);
                Boolean senderEdit = isAdministrative;
                View.DisplayMessageInfo(message.CreatedBy, message.CreatedOn);
                View.InitializeMailSettings(message.MailSettings, senderEdit, isAdministrative, isAdministrative);


                List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = new List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation>();
                if (message.Template != null) {
                    Boolean onlyShortText = false;
                    Boolean hasShortText = false;
                    if (message.Template.IsTemplateCompliant)
                    {
                        lm.Comol.Core.TemplateMessages.Domain.TemplateDefinitionVersion version = null;
                        if (message.Template.IdVersion > 0)
                            version = TemplatesService.GetVersion(message.Template.IdVersion);
                        if (version == null)
                            version = TemplatesService.GetLastActiveVersion(message.Template.IdTemplate);
                        if (version != null)
                        {
                            translations = (from t in version.Translations
                                            where t.Deleted == BaseStatusDeleted.None
                                            select new lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation()
                                            {
                                                Id = t.Id,
                                                IdLanguage = t.IdLanguage,
                                                LanguageCode = t.LanguageCode,
                                                LanguageName = t.LanguageName,
                                                Translation = t.Translation
                                            }).ToList();
                            if (!translations.Where(t => t.LanguageCode == "multi" && t.IdLanguage == 0).Any())
                            {
                                translations.Add(new lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation()
                                                {
                                                    Id = 0,
                                                    IdLanguage = 0,
                                                    LanguageCode = "multi",
                                                    Translation = version.DefaultTranslation
                                                });
                            }
                            View.ContentModules = version.GetModuleContentCodes();
                            onlyShortText = version.OnlyShortText;
                            hasShortText = version.HasShortText;
                        }
                    }
                    else {
                        translations = (from t in message.Template.Translations
                                        where t.Deleted == BaseStatusDeleted.None
                                        select new lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation()
                                        {
                                            Id=t.Id,
                                            IdLanguage=t.IdLanguage ,
                                            LanguageCode = t.LanguageCode,
                                            LanguageName = t.LanguageName,
                                            Translation= t.Translation 
                                        }).ToList();
                        View.ContentModules = new List<String>() { context.ModuleCode };
                    }
                    List<Language> languages = CurrentManager.GetAllLanguages().ToList();
                    List<lm.Comol.Core.DomainModel.Languages.LanguageItem> inUseLanguages = translations.Select(t => new lm.Comol.Core.DomainModel.Languages.LanguageItem() { Id = t.IdLanguage, Code = t.LanguageCode, Name = t.LanguageName, IsMultiLanguage = (t.LanguageCode == "multi" && t.IdLanguage == 0), Status = (t.IsEmpty) ? DomainModel.Languages.ItemStatus.wrong : (t.Translation.IsValid(!onlyShortText, hasShortText, true)) ? DomainModel.Languages.ItemStatus.valid : DomainModel.Languages.ItemStatus.warning }).ToList();

                    inUseLanguages.Where(l=> languages.Where(ll=>ll.isDefault && l.Id== ll.Id).Any()).ToList().ForEach(l=>l.IsDefault=true);
                    inUseLanguages.Where(l=> l.Id>0).ToList().ForEach(l=> l.Name= languages.Where(ll=>ll.Id== l.Id).Select(ll=>ll.Name).FirstOrDefault());

                    inUseLanguages = inUseLanguages.OrderByDescending(l=>l.IsMultiLanguage).ThenBy(l=>l.Name).ToList();
                    lm.Comol.Core.DomainModel.Languages.LanguageItem current = inUseLanguages.Where(l=>l.Id== UserContext.Language.Id).FirstOrDefault();
                    if (current==null && languages.Any()){
                        current = inUseLanguages.Where(l=>l.IsDefault).FirstOrDefault();
                        if (current==null)
                            current = inUseLanguages.Where(l=>l.IsMultiLanguage).FirstOrDefault();
                    }
                    if (current == null && inUseLanguages.Any())
                        current =inUseLanguages[0];
                    View.InitializeControls(translations,inUseLanguages,current);
                    String tagTranslation = View.TagTranslation;
                    if (translations.Any() && !String.IsNullOrEmpty(tagTranslation)){
                        List<lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder> placeHolders = View.GetContentPlaceHolders(View.ContentModules);
                        foreach(lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder pHolder in placeHolders.Where(t=> translations.Where(ct=> !ct.Translation.IsContentEmpty() && ((!String.IsNullOrEmpty(ct.Translation.Body) && ct.Translation.Body.Contains(t.Tag)) || (!String.IsNullOrEmpty(ct.Translation.Subject) && ct.Translation.Subject.Contains(t.Tag)))).Any()) ){
                            translations.Where(ct => !ct.Translation.IsContentEmpty() && !String.IsNullOrEmpty(ct.Translation.Body) && ct.Translation.Body.Contains(pHolder.Tag)).ToList().ForEach(t => t.Translation.Body = t.Translation.Body.Replace(pHolder.Tag, String.Format(tagTranslation, pHolder.Tag, pHolder.Name)));
                            translations.Where(ct => !ct.Translation.IsContentEmpty() && !String.IsNullOrEmpty(ct.Translation.Subject) && ct.Translation.Subject.Contains(pHolder.Tag)).ToList().ForEach(t => t.Translation.Subject = t.Translation.Subject.Replace(pHolder.Tag, String.Format(tagTranslation, pHolder.Tag, pHolder.Name)));
                        }
                    }
                    View.LoadMessage((current!=null) ? translations.Where(t=>t.IdLanguage==current.Id && t.LanguageCode== current.Code).FirstOrDefault() : (translations.Any() ? translations[0] : null));
                }
              

                //List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> availableItems = Service.GetAvalableLanguage(version);
                //List<lm.Comol.Core.DomainModel.Languages.LanguageItem> items = Service.GetInUseLanguageItems(version);

                //if (version != null)
                //{
                //    View.IdSelectedVersion = version.Id;
                //    View.ContentModules = Service.GetVersionModuleContentCodes(version.Id);

                //    translations.Add(new dtoTemplateTranslation() { IdLanguage = 0, LanguageCode = "multi", Translation = version.DefaultTranslation.Copy() });
                //    translations.AddRange(version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => new dtoTemplateTranslation() { IdLanguage = t.IdLanguage, LanguageCode = t.LanguageCode, Translation = t.Translation.Copy() }).ToList());
                //    if (version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == NotificationChannel.Mail).Any())
                //        settings = version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == NotificationChannel.Mail).FirstOrDefault().MailSettings;

                //    senderEdit = senderEdit && version.Template.Type == TemplateType.System;
                //    View.LoadEditor(translations, "multi", version.HasShortText, version.OnlyShortText, availableItems, items, items.Where(i => i.IsMultiLanguage).FirstOrDefault());
                //}
                //else
                //{
                //    View.ContentModules = new List<String>() { currentCode };
                //    translations.Add(new dtoTemplateTranslation() { IdLanguage = 0, LanguageCode = "multi", Translation = new ItemObjectTranslation() { IsHtml = (settings == null) ? true : settings.isHtml, Body = "", Name = "" } });
                //    View.LoadEditor(translations, "multi", false, false, availableItems, items, items.Where(i => i.IsMultiLanguage).FirstOrDefault());
                //}

            }
        }
    }
}