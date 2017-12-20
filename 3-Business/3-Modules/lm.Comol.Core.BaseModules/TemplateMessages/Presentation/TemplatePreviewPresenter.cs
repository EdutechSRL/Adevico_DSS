using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.TemplateMessages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class TemplatePreviewPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService tService;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewTemplatePreview View
            {
                get { return (IViewTemplatePreview)base.View; }
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
            private lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService internalService;
            private lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService InternalService
            {
                get
                {
                    if (internalService == null)
                        internalService = new lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService(AppContext);
                    return internalService;
                }
            }
            public TemplatePreviewPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TemplatePreviewPresenter(iApplicationContext oContext, IViewTemplatePreview view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idTemplate, long idVersion)
        {
            dtoSelectorContext context = GetContext();
            View.ContainerContext = context;
            View.ContentIdTemplate = idTemplate;
            View.ContentIdVersion = idVersion;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (HasPermission(context))
                    InitializeView(idTemplate, idVersion,context);
                else if (context.ObjectOwner != null)
                    View.DisplayNoPermission(context.IdCommunity, context.IdModule, context.ModuleCode);
                else
                    View.DisplayNoPermission(context.IdCommunity, TemplatesService.ServiceModuleID(), context.ModuleCode);
            }
        }
        private dtoSelectorContext GetContext()
        {
            dtoSelectorContext item = new dtoSelectorContext();
            item.ObjectOwner = View.PreloadModuleObject;
            item.IdCommunity = View.PreloadIdCommunity;
            item.IdModule = View.PreloadIdModule;
            item.ModuleCode = View.PreloadModuleCode;
            item.IsForPortal = View.PreloadForPortal;
            if (item.IdCommunity == -1 && item.ObjectOwner != null)
                item.IdCommunity = item.ObjectOwner.CommunityID;
            if (item.IdModule > 0 && String.IsNullOrEmpty(item.ModuleCode))
                item.ModuleCode = CurrentManager.GetModuleCode(item.IdModule);
            else if (item.IdModule == 0 && !String.IsNullOrEmpty(item.ModuleCode))
                item.IdModule = CurrentManager.GetModuleID(item.ModuleCode);

            Community c = CurrentManager.GetCommunity(item.IdCommunity);
            if (item.IdCommunity == 0 && item.IsForPortal == false)
            {
                item.IdCommunity = UserContext.CurrentCommunityID;
                item.IsForPortal = (item.IdCommunity == 0);
            }
            else if (item.IdCommunity > 0 && item.IsForPortal)
                item.IsForPortal = false;
            item.IdOrganization = (item.IdOrganization == 0) ? ((c == null) ? 0 : c.IdOrganization) : item.IdOrganization;
            if (item.IdOrganization > 0)
                item.IdOrganizationCommunity = CurrentManager.GetIdCommunityFromOrganization(item.IdOrganization);
            return item;
        }
        private Boolean HasPermission(dtoSelectorContext context)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);

            ModuleTemplateMessages permission = (context.IsForPortal) ? ModuleTemplateMessages.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID, OwnerType.System) : new ModuleTemplateMessages(CurrentManager.GetModulePermission(UserContext.CurrentUserID, context.IdCommunity, TemplatesService.ServiceModuleID()));
            if (permission.Administration)
            {
                //List<lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages> permissions = InitializePermissions(forPortal, idCommunity, p);
                return true;
            }
            else
                return View.HasModulePermissions(context.ModuleCode, GetModulePermissions(context.IdModule, context.IdCommunity), context.IdCommunity, (p == null) ? (int)UserTypeStandard.Guest : p.TypeID, context.ObjectOwner);
        }
        private long GetModulePermissions(Int32 idModule, Int32 idCommunity)
        {
            return CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule);
        }
        private void InitializeView(long idTemplate, long idVersion,dtoSelectorContext context)
        {
            TemplateDefinitionVersion version = InternalService.GetVersion(idTemplate, idVersion);
            if (version == null)
            {
                if (idVersion > 0)
                    View.DisplayNoTemplateVersionFound();
                else
                    View.DisplayNoTemplateFound();
            }
            else {
                if (idVersion > 0)
                    View.DisplayVersionInfo(version.Number);
                else
                    View.DisplayLastVersionInfo();
                Boolean isAdministrative =  InternalService.IsAdministrativeUser(UserContext.CurrentUserID);
                if (version.ChannelSettings != null && version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail).Any())
                    View.InitializeMailSettings(version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail).FirstOrDefault().MailSettings, isAdministrative, isAdministrative, isAdministrative);
                List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = new List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation>();
                translations = (from t in version.Translations where t.Deleted == BaseStatusDeleted.None
                                select new lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation()
                                {
                                    Id = t.Id,
                                    IdLanguage = t.IdLanguage,
                                    LanguageCode = t.LanguageCode,
                                    LanguageName = t.LanguageName,
                                    Translation = t.Translation
                                }).ToList();
                translations.Add(new dtoTemplateTranslation() { Id = 0, IdLanguage = 0, LanguageCode = "multi", LanguageName = "multi", IdVersion = version.Id, Translation = version.DefaultTranslation });
                View.ContentModules = version.GetModuleContentCodes();
                List<Language> languages = CurrentManager.GetAllLanguages().ToList();
                List<lm.Comol.Core.DomainModel.Languages.LanguageItem> inUseLanguages = translations.Select(t => new lm.Comol.Core.DomainModel.Languages.LanguageItem() { Id = t.IdLanguage, Code = t.LanguageCode, Name = t.LanguageName, IsMultiLanguage = (t.LanguageCode == "multi" && t.IdLanguage == 0), Status = (t.IsEmpty) ? DomainModel.Languages.ItemStatus.wrong : (t.Translation.IsValid(!version.OnlyShortText, version.HasShortText, true)) ? DomainModel.Languages.ItemStatus.valid : DomainModel.Languages.ItemStatus.warning }).ToList();
                inUseLanguages.Where(l => languages.Where(ll => ll.isDefault && l.Id == ll.Id).Any()).ToList().ForEach(l => l.IsDefault = true);
                inUseLanguages.Where(l => l.Id > 0).ToList().ForEach(l => l.Name = languages.Where(ll => ll.Id == l.Id).Select(ll => ll.Name).FirstOrDefault());
                inUseLanguages.Add(new lm.Comol.Core.DomainModel.Languages.LanguageItem() { Id = 0, Code = "multi", IsEnabled = true, IsMultiLanguage = true, Status = (version.DefaultTranslation.IsValid(!version.OnlyShortText, version.HasShortText, true)) ? lm.Comol.Core.DomainModel.Languages.ItemStatus.valid : lm.Comol.Core.DomainModel.Languages.ItemStatus.warning });

                inUseLanguages = inUseLanguages.OrderByDescending(l => l.IsMultiLanguage).ThenBy(l => l.Name).ToList();
                //if (!inUseLanguages.Any(l => l.IsMultiLanguage))
                //{
                //    inUseLanguages.Add(new DomainModel.Languages.LanguageItem(){ IsMultiLanguage=true, })
                //}
                lm.Comol.Core.DomainModel.Languages.LanguageItem current = inUseLanguages.Where(l => l.Id == UserContext.Language.Id).FirstOrDefault();
                if (current == null && inUseLanguages.Any())
                {
                    current = inUseLanguages.Where(l => l.IsDefault).FirstOrDefault();
                    if (current == null)
                        current = inUseLanguages.Where(l => l.IsMultiLanguage).FirstOrDefault();
                }
                if (current == null && inUseLanguages.Any())
                    current = inUseLanguages.FirstOrDefault();
                View.InitializeControls(translations, inUseLanguages, current);
                View.LoadTemplate((current != null) ? translations.Where(t => t.IdLanguage == current.Id && t.LanguageCode == current.Code).FirstOrDefault() : (translations.Any() ? translations[0] : null));
            }
        //        Boolean isAdministrative =  Service.IsAdministrativeUser(UserContext.CurrentUserID);
        //        Boolean senderEdit = isAdministrative;

        //        List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations = new List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation>();
        //        if (message.Template != null) {
        //            Boolean onlyShortText = false;
        //            Boolean hasShortText = false;

       
        //            String tagTranslation = View.TagTranslation;
        //            if (translations.Any() && !String.IsNullOrEmpty(tagTranslation)){
        //                List<lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder> placeHolders = View.GetContentPlaceHolders(View.ContentModules);
        //                foreach(lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder pHolder in placeHolders.Where(t=> translations.Where(ct=> !ct.Translation.IsContentEmpty && ((!String.IsNullOrEmpty(ct.Translation.Body) && ct.Translation.Body.Contains(t.Tag)) || (!String.IsNullOrEmpty(ct.Translation.Subject) && ct.Translation.Subject.Contains(t.Tag)))).Any()) ){
        //                    translations.Where(ct => !ct.Translation.IsContentEmpty && !String.IsNullOrEmpty(ct.Translation.Body) && ct.Translation.Body.Contains(pHolder.Tag)).ToList().ForEach(t => t.Translation.Body = t.Translation.Body.Replace(pHolder.Tag, String.Format(tagTranslation, pHolder.Tag, pHolder.Name)));
        //                    translations.Where(ct => !ct.Translation.IsContentEmpty && !String.IsNullOrEmpty(ct.Translation.Subject) && ct.Translation.Subject.Contains(pHolder.Tag)).ToList().ForEach(t => t.Translation.Subject = t.Translation.Subject.Replace(pHolder.Tag, String.Format(tagTranslation, pHolder.Tag, pHolder.Name)));
        //                }
        //            }
        
        //        }
        //    }
        }
    }
}