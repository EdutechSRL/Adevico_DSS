using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Business;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class EditSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEditSettings View
            {
                get { return (IViewEditSettings)base.View; }
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
            public EditSettingsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditSettingsPresenter(iApplicationContext oContext, IViewEditSettings view)
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
                Logout(t,idTemplate,idVersion);
            }
            else
            {
                Boolean allowDraft = false;
                Boolean allowActivate = false;
                TemplateDefinitionVersion version = Service.GetVersion(idVersion);
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> steps = Service.GetAvailableSteps(idVersion, WizardTemplateStep.Settings, ownerInfo.Type);
                if (version == null || (version.Deleted != BaseStatusDeleted.None && !View.PreloadPreview) || (version.Template==null) )
                {
                    View.DisplayUnknownTemplate();
                    steps = steps.Where(s => s.Id.Type == WizardTemplateStep.Settings).ToList();
                    steps[0].Status = Wizard.WizardItemStatus.disabled;
                    steps[0].DisplayOrderDetail = Wizard.DisplayOrderEnum.first | Wizard.DisplayOrderEnum.last;
                    View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.UnknownTemplateVersion);
                }
                else
                {
                    dtoTemplatePermission permission = null;
                    Boolean allowSee = false;
                    Boolean isPreview = View.PreloadPreview;
                    View.IdTemplate = version.Template.Id;
                    View.IdVersion = version.Id;
                    t = version.Template.Type;
                    if (View.IsTemplateAdded)
                        View.DisplayTemplateAdded();

                    switch (ownerInfo.Type)
                    {
                        case OwnerType.None:
                            View.UnableToReadUrlSettings();
                            View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.MalformedUrl);
                            break;
                        case OwnerType.Object:
                            //allowDraft = View.HasPermissionForObject(ModuleObject.CreateLongObject(ownerInfo.IdObject, ownerInfo.IdObjectType, ownerInfo.IdObjectCommunity, CurrentManager.GetModuleCode(ownerInfo.IdObjectModule)));
                            //break;
                        case OwnerType.Module:
                            View.IdTemplateModule = ownerInfo.IdModule;
                            if (ownerInfo.IdModule == Service.ServiceModuleID())
                                permission = Service.GetItemPermission(idVersion);
                            else
                                permission = Service.GetManagementTemplatePermission(version, GetPermissions(ownerInfo.ModuleCode, ownerInfo.ModulePermission, ownerInfo.IdCommunity, t));
                            allowDraft = permission.AllowEdit;
                            allowSee = permission.AllowUse;
                            break;
                        default:
                            permission = Service.GetItemPermission(idVersion);
                            allowDraft = permission.AllowEdit;
                            allowSee = permission.AllowUse;
                            break;
                    }
                    allowDraft = allowDraft && (version.Status == TemplateStatus.Draft) && !isPreview;
                    allowActivate = allowDraft && (version.DefaultTranslation.IsValid() || (version!=null && version.Translations.Where(tn => tn.Deleted == BaseStatusDeleted.None && tn.IsValid).Any()));

                    Boolean isAdministrative = Service.IsAdministrativeUser(UserContext.CurrentUserID);
                    View.AllowSenderEdit = isAdministrative && t == TemplateType.System;
                    View.AllowSubjectEdit = isAdministrative;
                    View.AllowSignatureEdit = isAdministrative;
                    View.InputReadOnly = isPreview || (!allowActivate && !allowDraft && allowSee);
                    View.AllowSaveDraft = allowDraft;
                    View.AllowSave = allowActivate;
                    if (allowDraft || allowActivate || allowSee)
                    {
                        LoadSettings(version);
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
        public void AddChannelSetting(lm.Comol.Core.Notification.Domain.NotificationChannel channel, List<dtoChannelConfigurator> items)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType,View.IdTemplate, View.IdVersion);
            else
            {
                dtoChannelConfigurator config = Service.GetDraftChannelConfigurator(channel, View.CurrentType, 0);
                if (config == null)
                    View.DisplayTemplateUnableToAddNotificationChannel();
                else
                {
                    List<lm.Comol.Core.Notification.Domain.NotificationChannel> channels = Service.GetAvailableChannels(View.CurrentType, View.IdVersion);
                    channels.Remove(channel);
                    channels = channels.Where(t => !items.Where(i => i.Channel == t).Any()).ToList();

                    //config.AvailableModuleCodes = View.GetModuleCodesForNotification().Select(m => m.Key).ToList();
                    items.Add(config);
                    //UpdateInUseSettings(items);
                    View.ChannelSettings = items;
                    View.LoadChannelSettings(items);
                    View.LoadChannels(channels);
                    View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.AddNotificationSetting);
                }
            }
        }
        public void SaveModuleContentSettings(List<String> contentmodules) {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType,View.IdTemplate, View.IdVersion);
            else
            {
                View.SavingSettings = false;
                TemplateDefinitionVersion version = Service.GetVersion(View.IdVersion);
                if (version != null)
                {
                    Boolean askConfirm = false;
                    Dictionary<String, List<String>> placeHolders = View.GetOldContentPlaceHolders((version.ModulesForContent == null) ? new List<String>() : version.ModulesForContent.Where(m => m.Deleted == BaseStatusDeleted.None && !contentmodules.Contains(m.ModuleCode)).Select(m => m.ModuleCode).ToList());
                    if (placeHolders.Keys.Count > 0) {
                        Dictionary<String, List<String>> inUsePlaceHolders = Service.GetInUserPlaceHolders(version, placeHolders);
                        if (inUsePlaceHolders.Keys.Count > 0)
                        {
                            View.InUsePlaceHolders = inUsePlaceHolders;
                            View.DisplayConfirmModules(inUsePlaceHolders.Keys.ToList(), inUsePlaceHolders);
                            askConfirm = true;
                        }
                    }
                    if (!askConfirm)
                    {
                        if (Service.SaveModuleContentSettings(version, contentmodules))
                        {
                            View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.SaveModuleContent);
                            View.DisplayContentModulesSaved();
                        }
                        else
                        {
                            View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.ErrorSavingModuleContent);
                            View.DisplayContentModulesErrorSaving();
                        }
                    }
                }
                else
                    View.DisplayUnknownTemplate();
            }
        }
        public void SaveSettings(String name,List<String> contentmodules, List<dtoChannelConfigurator> items, TemplateStatus status)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
            {
                View.SavingSettings = true;
                View.SavingStatus = status;
                TemplateDefinitionVersion version = Service.GetVersion(View.IdVersion);
                if (version != null)
                {
                    Boolean askConfirm = false;
                    Dictionary<String, List<String>> placeHolders = View.GetOldContentPlaceHolders((version.ModulesForContent == null) ? new List<String>() : version.ModulesForContent.Where(m => m.Deleted == BaseStatusDeleted.None && !contentmodules.Contains(m.ModuleCode)).Select(m => m.ModuleCode).ToList());
                    if (placeHolders.Keys.Count > 0) {
                        Dictionary<String, List<String>> inUsePlaceHolders = Service.GetInUserPlaceHolders(version, placeHolders);
                        if (inUsePlaceHolders.Keys.Count > 0){
                            View.InUsePlaceHolders = inUsePlaceHolders;
                            View.DisplayConfirmModules(inUsePlaceHolders.Keys.ToList(), inUsePlaceHolders);
                            askConfirm = true;
                        }
                    }
                    if (!askConfirm)
                        SaveSettings(version.Id, name, contentmodules, items, status);
                }
                else
                    View.DisplayUnknownTemplate();
            }
        }
        /// <summary>
        ///  EFFETTUA AL MOMENTO LA CANCELLAZIONE IMMEDIATA
        /// </summary>
        /// <param name="items"></param>
        /// <param name="temporaryId"></param>
        public void VirtualDeleteChannelSetting(List<dtoChannelConfigurator> items, System.Guid temporaryId)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
            {
                dtoChannelConfigurator configurator = items.Where(i => i.TemporaryId == temporaryId).FirstOrDefault();
                if (configurator != null && configurator.IdSettings ==0)
                    items = items.Where(i => i.TemporaryId != temporaryId).ToList();
                else if (configurator != null && configurator.IdSettings > 0) {
                    if (Service.VirtualDeleteChannelSetting(configurator.IdSettings))
                    {
                        View.DisplayTemplateSettingDeleted();
                        items = items.Where(i => i.TemporaryId != temporaryId).ToList();
                    }
                    else
                        View.DisplayTemplateSettingErrorDeleting();
                }

                List<lm.Comol.Core.Notification.Domain.NotificationChannel> channels = Service.GetAvailableChannels(View.CurrentType, View.IdVersion);
                channels = channels.Where(t => !items.Where(i => i.Channel == t).Any()).ToList();

                View.ChannelSettings = items;
                View.LoadChannelSettings(items.Where(i => i.Deleted == BaseStatusDeleted.None).ToList());
                View.LoadChannels(channels);
            }
        }

        public void ReloadModules(List<String> codes)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else{
                codes.AddRange(Service.GetVersionModuleContentCodes(View.IdVersion));
                View.InUsePlaceHolders = new Dictionary<String, List<String>>();
                View.LoadContentModules(codes.Distinct().ToList());
            }
        }
        public void RemovePlaceHolders(List<String> contentmodules)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
            {
                View.SavingSettings = false;
                TemplateDefinitionVersion version = Service.GetVersion(View.IdVersion);
                if (version != null)
                {
                    if (Service.SaveModuleContentSettings(version, contentmodules, View.InUsePlaceHolders))
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.SaveModuleContent);
                        View.DisplayContentModulesSaved();
                    }
                    else
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.ErrorSavingModuleContent);
                        View.DisplayContentModulesErrorSaving();
                    }
                }
                else
                    View.DisplayUnknownTemplate();
            }
        }
        public void RemovePlaceHolders(String name, List<String> contentmodules, List<dtoChannelConfigurator> items, TemplateStatus status) {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
                SaveSettings(View.IdVersion, name, contentmodules, items, status,View.InUsePlaceHolders);
        }
        public void LeavePlaceHolders(List<String> contentmodules)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
            {
                View.SavingSettings = false;
                TemplateDefinitionVersion version = Service.GetVersion(View.IdVersion);
                if (version != null)
                {
                    if (Service.SaveModuleContentSettings(version, contentmodules))
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.SaveModuleContent);
                        View.DisplayContentModulesSaved();
                    }
                    else
                    {
                        View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.ErrorSavingModuleContent);
                        View.DisplayContentModulesErrorSaving();
                    }
                }
                else
                    View.DisplayUnknownTemplate();
            }
        }
        public void LeavePlaceHolders(String name, List<String> contentmodules, List<dtoChannelConfigurator> items, TemplateStatus status)
        {
            View.HideUserMessage();
            if (UserContext.isAnonymous)
                Logout(View.CurrentType, View.IdTemplate, View.IdVersion);
            else
                SaveSettings(View.IdVersion, name, contentmodules, items, status);
        }

        private void SaveSettings(long idVersion, String name, List<String> contentmodules, List<dtoChannelConfigurator> items, TemplateStatus status, Dictionary<String, List<String>> placeHolders=null)
        {
            TemplateDefinitionVersion version = Service.SaveSettings(idVersion, name, contentmodules, items, status, placeHolders);
            if (version != null)
            {
                View.AllowSaveDraft = (version.Status == TemplateStatus.Draft);
                View.AllowSave = (version.Status == TemplateStatus.Draft);
                //UpdateInUseSettings(items);
                View.LoadWizardSteps(View.IdTemplateCommunity, Service.GetAvailableSteps(idVersion, WizardTemplateStep.Settings, View.Ownership.Type));
                View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.SaveVersionSettings);
                View.DisplayTemplateSettingsSaved();
                View.DisplayInput(version.DefaultTranslation.Name, version.Number, version.Status);
            }
            else
            {
                View.SendUserAction(View.IdTemplateCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.ErrorSavingSettings);
                View.DisplayTemplateSettingsErrors();
            }
        }
        private void LoadSettings(TemplateDefinitionVersion version)
        {
            LoadSettings(version, Service.GetChannelSettings(version));
        }
        private void LoadSettings(TemplateDefinitionVersion version, List<dtoChannelConfigurator> items)
        {
            List<string> modules = View.GetAvailableModules();
            View.AllowEditModuleContent = (version.Template != null && version.Template.OwnerInfo.Type != OwnerType.Module && version.Template.OwnerInfo.IdModule<=0);
            View.DisplayInput(version.DefaultTranslation.Name, version.Number,version.Status,modules,version.GetModuleContentCodes(), Service.GetAvailableChannels(version.Template.Type, version));
            //UpdateInUseSettings(items);
            View.ChannelSettings = items;
            View.LoadChannelSettings(items);
        }

        //private void UpdateInUseSettings( List<dtoNotificationConfigurator> items) {
        //    if (items == null || !items.Any())
        //        View.InUseNotificationAction = new Dictionary<String, List<long>>();
        //    else
        //        View.InUseNotificationAction = items.Where(i => !String.IsNullOrEmpty(i.Settings.ModuleCode)).Select(i => i.Settings.ModuleCode).ToDictionary(i => i, c => items.Where(item => item.Settings.ModuleCode == c).Select(i => i.Settings.IdModuleAction).ToList());
        //}

        private void Logout(TemplateType t, long idTemplate, long idVersion)
        {
            View.DisplaySessionTimeout(RootObject.EditByStep(t, View.PreloadOwnership, WizardTemplateStep.Settings, View.PreloadFromIdCommunity, View.PreloadFromModuleCode,View.PreloadFromModulePermissions, View.GetEncodedBackUrl(), idTemplate, idVersion, View.PreloadPreview));
        }
    }
}