using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Business;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class EditTranslationsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditTranslations View
            {
                get { return (IViewEditTranslations)base.View; }
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
            public EditTranslationsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditTranslationsPresenter(iApplicationContext oContext, IViewEditTranslations view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            long idTemplate = View.PreloadIdTemplate;
            long idVersion = View.PreloadIdVersion;
            TemplateType t = View.PreloadTemplateType;
            dtoBaseTemplateOwner ownerInfo = View.PreloadOwnership;
            Int32 idCommunity = ownerInfo.IdCommunity;
            if (idCommunity == -1)
                idCommunity = UserContext.CurrentCommunityID;
            if (ownerInfo.IdModule == 0 && !String.IsNullOrEmpty(ownerInfo.ModuleCode))
                ownerInfo.IdModule = CurrentManager.GetModuleID(ownerInfo.ModuleCode);
            View.Ownership = ownerInfo;
            View.IdTemplateCommunity = idCommunity;

            if (UserContext.isAnonymous)
            {
                Logout(t, idTemplate, idVersion);
            }
            else
            {
                Boolean allowSave = false;
                TemplateDefinitionVersion version = Service.GetVersion(idVersion);
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> steps = Service.GetAvailableSteps(idVersion, WizardTemplateStep.Translations, ownerInfo.Type);;
                if (version == null || (version.Deleted != BaseStatusDeleted.None && !View.PreloadPreview) || (version.Template==null) )
                {
                    View.DisplayUnknownTemplate();
                    steps.ForEach(s => s.Status = Wizard.WizardItemStatus.disabled);
                    View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.UnknownTemplateVersion);
                }
                else
                {
                    dtoTemplatePermission permission = null;
                    Boolean isPreview = View.PreloadPreview;
                    Boolean allowSee = false;
                    View.IdTemplate = version.Template.Id;
                    View.IdVersion = version.Id;
                    t = version.Template.Type;
                    switch (ownerInfo.Type)
                    {
                        case OwnerType.None:
                            View.UnableToReadUrlSettings();
                            View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.MalformedUrl);
                            break;
                        case OwnerType.Object:
                            //allowSave = View.HasPermissionForObject(ModuleObject.CreateLongObject(ownerInfo.IdObject, ownerInfo.IdObjectType, ownerInfo.IdObjectCommunity, CurrentManager.GetModuleCode(ownerInfo.IdObjectModule)));
                            //break;
                        case OwnerType.Module:
                            View.IdTemplateModule = ownerInfo.IdModule;
                            if (ownerInfo.IdModule == Service.ServiceModuleID())
                                permission = Service.GetItemPermission(idVersion);
                            else
                                permission = Service.GetManagementTemplatePermission(version, GetPermissions(ownerInfo.ModuleCode, ownerInfo.ModulePermission, ownerInfo.IdCommunity, t));
                            allowSave = permission.AllowEdit;
                            allowSee = permission.AllowUse;
                            break;
                        default:
                            permission = Service.GetItemPermission(idVersion);
                            allowSave = permission.AllowEdit;
                            allowSee = permission.AllowUse;
                            break;
                    }
                    allowSave = allowSave && (version.Status == TemplateStatus.Draft) && !isPreview;
                    //allowActivate = allowDraft && (version.DefaultTranslation.IsValid() || (version!=null && version.Translations.Where(tn => tn.Deleted == BaseStatusDeleted.None && tn.IsValid).Any()));

                    Boolean isAdministrative = Service.IsAdministrativeUser(UserContext.CurrentUserID);
                    View.InputReadOnly = isPreview || (!allowSave && allowSee);
                    View.AllowSave = allowSave;
                    if (allowSave || allowSee)
                    {
                        LoadDefaultTranslation(version);
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.StartEditingSettings);
                    }
                    else
                    {
                        View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.TryToEditingSettings);
                    }
                }
                View.CurrentType = t;
              
                View.LoadWizardSteps(idCommunity, steps);
                if (String.IsNullOrEmpty(View.PreloadBackUrl))
                    View.SetBackUrl(RootObject.List(View.PreloadFromIdCommunity, t, ownerInfo, true, View.PreloadFromModuleCode, View.PreloadFromModulePermissions, idTemplate, idVersion));
                else
                    View.SetBackUrl(View.PreloadBackUrl);
            }
        }
        private ModuleGenericTemplateMessages GetPermissions(String moduleCode, long permissions, Int32 idCommunity, TemplateType type)
        {
            ModuleGenericTemplateMessages permission = null;
            Int32 idUser = UserContext.CurrentUserID;
            switch (type)
            {
                case TemplateType.Module:
                    if (moduleCode == ModuleTemplateMessages.UniqueCode)
                        permission = new ModuleGenericTemplateMessages(Service.GetPermission(idCommunity, OwnerType.Module));
                    else
                    {
                        Int32 idModule = CurrentManager.GetModuleID(moduleCode);
                        dtoBaseTemplateOwner ownerInfo = View.PreloadOwnership;
                        ModuleObject obj = (ownerInfo.Type == OwnerType.Object) ? ModuleObject.CreateLongObject(ownerInfo.IdObject, ownerInfo.IdObjectType, ownerInfo.IdObjectCommunity, CurrentManager.GetModuleCode(ownerInfo.IdObjectModule), ownerInfo.IdObjectModule) : null;
                        if (obj != null && obj.ServiceID == 0 && !String.IsNullOrEmpty(obj.ServiceCode))
                            obj.ServiceID = CurrentManager.GetModuleID(obj.ServiceCode);
                        else if (obj != null && obj.ServiceID > 0 && String.IsNullOrEmpty(obj.ServiceCode))
                            obj.ServiceCode = CurrentManager.GetModuleCode(obj.ServiceID);
                        if (permissions > 0)
                            permission = View.GetModulePermissions(moduleCode, idModule, CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule), idCommunity, UserContext.UserTypeID, obj);
                        else
                            permission = View.GetModulePermissions(moduleCode, idModule, GetModulePermission(idCommunity, idModule), idCommunity, UserContext.UserTypeID, obj);
                    }
                    break;
                case TemplateType.User:
                    Person p = GetCurrentUser(ref idUser);
                    Boolean allowView = (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser);

                    permission = new ModuleGenericTemplateMessages("personal");
                    permission.Add = allowView;
                    permission.Administration = allowView;
                    permission.Clone = allowView;
                    permission.DeleteMyTemplates = allowView;
                    permission.Edit = allowView;
                    permission.List = allowView;
                    break;
            }
            if (permission == null)
                permission = new ModuleGenericTemplateMessages(moduleCode);
            return permission;
        }
        public long GetModulePermission(Int32 idCommunity, Int32 idModule)
        {
            return CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule);
        }
        private Person GetCurrentUser(ref Int32 idUser)
        {
            Person person = null;
            if (UserContext.isAnonymous)
            {
                person = (from p in CurrentManager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();//CurrentManager.GetPerson(UserContext.CurrentUserID);
                idUser = (person != null) ? person.Id : UserContext.CurrentUserID; //if(Person!=null) { IdUser = PersonId} else {IdUser = UserContext...}
            }
            else
                person = CurrentManager.GetPerson(idUser);
            return person;
        }
        private void LoadDefaultTranslation(TemplateDefinitionVersion version)
        {
            dtoTemplateTranslation current = new dtoTemplateTranslation() { Id=0, IdLanguage=0, LanguageCode= "multi", IdVersion=version.Id, Translation= version.DefaultTranslation};
            List<lm.Comol.Core.DomainModel.Languages.LanguageItem> items =  Service.GetInUseLanguageItems(version);

            View.AllowDelete = false;
            View.ContentModules = Service.GetVersionModuleContentCodes(version.Id);
            View.AllowPreview = (!String.IsNullOrEmpty(current.Translation.Subject) || !String.IsNullOrEmpty(current.Translation.ShortText) || !String.IsNullOrEmpty(current.Translation.Body));
            View.LoadTranslation(current,version.HasShortText, version.OnlyShortText,  Service.GetAvalableLanguage(version), items, items.Where(i=>i.IsMultiLanguage).FirstOrDefault());
        }

        public void LoadTranslation(LanguageItem current, Boolean forAdd = false)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
            {
                TemplateDefinitionVersion version = Service.GetVersion(View.IdVersion);
                if (version == null || (version.Deleted != BaseStatusDeleted.None && !View.PreloadPreview) || (version.Template == null))
                {
                    View.DisplayUnknownTemplate();
                    List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> steps = Service.GetAvailableSteps(View.IdVersion, WizardTemplateStep.Translations, View.Ownership.Type);
                    steps.ForEach(s => s.Status = Wizard.WizardItemStatus.disabled);
                    View.LoadWizardSteps(View.IdTemplateCommunity, steps);
                    View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.UnknownTemplateVersion);
                }
                else {
                    dtoTemplateTranslation t = Service.GetTranslation(View.IdVersion, current.Id, current.Code);
                    if (t.Id != current.Id)
                    {
                        t.IdLanguage = current.Id;
                        t.LanguageCode = current.Code;
                    }
                    if (forAdd && t != null)
                    {
                        TemplateTranslation sTranslation = Service.SaveTranslation(View.IdVersion, t);
                        if (sTranslation != null)
                        {
                            View.UpdateTranslationSelector(Service.GetAvalableLanguages(), Service.GetInUseLanguageItems(View.IdVersion), View.CurrentTranslation);
                            View.LoadWizardSteps(View.IdTemplateCommunity, Service.GetAvailableSteps(View.IdVersion, WizardTemplateStep.Translations, View.Ownership.Type));
                            View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.TranslationSaved);
                        }
                    }

                    View.AllowDelete = !current.IsMultiLanguage && (View.InputReadOnly && (View.AllowSave ));
                    View.AllowPreview= t != null && (!String.IsNullOrEmpty(t.Translation.Subject) || !String.IsNullOrEmpty(t.Translation.ShortText) || !String.IsNullOrEmpty(t.Translation.Body));
                    View.LoadTranslation(t, version.HasShortText, version.OnlyShortText, current);
                }
            }
        }
        public void SaveTranslation(dtoTemplateTranslation translation)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
            {
                if (translation != null && translation.LanguageCode == "multi")
                {
                    if (Service.SaveDefaultTranslation(View.IdVersion, translation))
                    {
                        View.AllowPreview = (!String.IsNullOrEmpty(translation.Translation.Subject) || !String.IsNullOrEmpty(translation.Translation.ShortText) || !String.IsNullOrEmpty(translation.Translation.Body));
                        View.UpdateTranslationSelector(Service.GetAvalableLanguages(), Service.GetInUseLanguageItems(View.IdVersion), View.CurrentTranslation);
                        View.LoadWizardSteps(View.IdTemplateCommunity, Service.GetAvailableSteps(View.IdVersion, WizardTemplateStep.Translations, View.Ownership.Type));
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.TranslationSaved);
                        View.DisplayTemplateTranslationSaved(0);
                    }
                    else
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.ErrorSavingTranslation);
                        View.DisplayTemplateTranslationErrorSaving();
                    }
                }
                else
                {
                    TemplateTranslation t = Service.SaveTranslation(View.IdVersion, translation);
                    if (t != null)
                    {
                        View.UpdateTranslationSelector(Service.GetAvalableLanguages(), Service.GetInUseLanguageItems(View.IdVersion), View.CurrentTranslation);
                        View.LoadWizardSteps(View.IdTemplateCommunity, Service.GetAvailableSteps(View.IdVersion, WizardTemplateStep.Translations, View.Ownership.Type));
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.TranslationSaved);
                        View.DisplayTemplateTranslationSaved(t.Id);
                    }
                    else
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.ErrorSavingTranslation);
                        View.DisplayTemplateTranslationErrorSaving();
                    }
                }
            }
        }
        public void VirtualDeleteTranslation(long idTranslation, Int32 idLanguage, String code)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
            {
                if (idTranslation > 0)
                {
                    if (Service.VirtualDeleteTranslation(View.IdVersion, idTranslation))
                    {
                        View.LoadWizardSteps(View.IdTemplateCommunity, Service.GetAvailableSteps(View.IdVersion, WizardTemplateStep.Translations, View.Ownership.Type));
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.VirtualDeleteTranslation);
                        View.DisplayTemplateTranslationDeleted();
                        LoadTranslation(View.RemoveCurrentTranslation());
                    }
                    else
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.ErrorDeletingTranslation);
                        View.DisplayTemplateTranslationErrorDeleting();
                    }
                }
                else {
                    View.RemoveCurrentTranslation();
                    View.DisplayTemplateTranslationDeleted();
                }
            }
        }

        public void PreviewMessage(dtoTemplateTranslation content)
        {
            long idVersion = View.IdVersion;
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, idVersion);
            else
            {
                TemplateDefinitionVersion v = Service.GetVersion(idVersion);
                if (v!=null && v.Template!=null)
                {
                    Int32 idCommunity = View.IdTemplateCommunity;
                    if (View.Ownership.IdObjectCommunity > 0)
                        idCommunity = View.Ownership.IdObjectCommunity;
                    List<ChannelSettings> channels = (v.ChannelSettings == null) ? new List<ChannelSettings>() : v.ChannelSettings.Where(s => s.Deleted == BaseStatusDeleted.None).ToList();
                    Boolean sendMail = (channels != null && channels.Where(n => n.Channel == lm.Comol.Core.Notification.Domain.NotificationChannel.Mail).Any()) && v.Template.OwnerInfo.Type == OwnerType.Object;

                    View.DisplayMessagePreview(sendMail, content.LanguageCode, content.Translation, View.ContentModules, channels, idCommunity, (v.Template.OwnerInfo.Type == OwnerType.Object) ? v.Template.OwnerInfo.ModuleObject : null);
                }
            }
        }

        private void Logout(TemplateType t, long idTemplate, long idVersion)
        {
            View.DisplaySessionTimeout(RootObject.EditByStep(t, View.PreloadOwnership, WizardTemplateStep.Translations, View.PreloadFromIdCommunity, View.PreloadFromModuleCode,View.PreloadFromModulePermissions, View.GetEncodedBackUrl(), idTemplate, idVersion, View.PreloadPreview));
        }
    }
}