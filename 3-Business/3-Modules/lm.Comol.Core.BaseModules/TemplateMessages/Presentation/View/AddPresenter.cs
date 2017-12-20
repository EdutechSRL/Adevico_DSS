using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Business;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.Notification.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class AddPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewAdd View
            {
                get { return (IViewAdd)base.View; }
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
            public AddPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddPresenter(iApplicationContext oContext, IViewAdd view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            TemplateType t = View.PreloadTemplateType;
            dtoBaseTemplateOwner ownerInfo = View.PreloadOwnership;
            Int32 idCommunity = ownerInfo.IdCommunity;
            if (idCommunity == -1)
            {
                idCommunity = UserContext.CurrentCommunityID;
                //if (t== TemplateType.System)
                    ownerInfo.IdCommunity = idCommunity;
            }
            if (ownerInfo.IdModule == 0 && !String.IsNullOrEmpty(ownerInfo.ModuleCode))
                ownerInfo.IdModule = CurrentManager.GetModuleID(ownerInfo.ModuleCode);
            View.Ownership=ownerInfo;
            View.IdTemplateCommunity= idCommunity;
            View.CurrentType=t;
            if(UserContext.isAnonymous){
                View.DisplaySessionTimeout(RootObject.Add(t, View.PreloadOwnership, View.PreloadFromIdCommunity, View.PreloadFromModuleCode, View.PreloadFromModulePermissions));
            }
            else{
                Boolean allowAdd = false;
                switch(ownerInfo.Type){
                    case OwnerType.None:
                        View.UnableToReadUrlSettings();
                        View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.MalformedUrl);
                        break;
                    //case OwnerType.Object:
                    //    allowAdd = View.HasPermissionForObject(ModuleObject.CreateLongObject(ownerInfo.IdObject, ownerInfo.IdObjectType, ownerInfo.IdObjectCommunity,CurrentManager.GetModuleCode(ownerInfo.IdObjectModule)));
                    //    break;
                    default:
                        if (t == TemplateType.Module)
                        {
                            ModuleGenericTemplateMessages p = GetPermissions(View.PreloadFromModuleCode, View.PreloadFromModulePermissions,View.PreloadFromIdCommunity, t);
                            allowAdd = p.Administration || p.Add;
                        }
                        else
                            allowAdd = Service.AllowAdd(ownerInfo);
                        break;
                }
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> steps = Service.GetAvailableSteps(0, WizardTemplateStep.Settings, ownerInfo.Type);
                View.AllowAdd = allowAdd;
                if (allowAdd)
                {
                    Boolean isAdministrative = Service.IsAdministrativeUser(UserContext.CurrentUserID);
                    View.AllowSenderEdit = isAdministrative && t == TemplateType.System;
                    View.AllowSubjectEdit = isAdministrative;
                    View.AllowSignatureEdit = isAdministrative;
                    List<NotificationChannel> channels = Service.GetAvailableChannels(t, 0);
                    if (t == TemplateType.Module || ownerInfo.IdModule>0)
                    {
                        View.IdTemplateModule = ownerInfo.IdModule;
                        View.DisplayInput(CurrentManager.GetLanguage(UserContext.Language.Id), Service.GetTemplateNumber(ownerInfo), channels);
                        if (channels.Any() && channels.Count==1 && channels.Contains(NotificationChannel.Mail))
                            AddNotificationSetting(NotificationChannel.Mail, new List<dtoChannelConfigurator>());
                    }
                    else
                    {
                        View.DisplayInput(CurrentManager.GetLanguage(UserContext.Language.Id), Service.GetTemplateNumber(ownerInfo), View.GetAvailableModules(), channels);
                        if (t == TemplateType.User && channels.Contains(NotificationChannel.Mail))
                            AddNotificationSetting(NotificationChannel.Mail, new List<dtoChannelConfigurator>());
                    }
                    View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.StartAddNewTemplate);
                }
                else
                {
                    steps = steps.Where(s => s.Id.Type == WizardTemplateStep.Settings).ToList();
                    steps[0].Status = Wizard.WizardItemStatus.disabled;
                    steps[0].DisplayOrderDetail = Wizard.DisplayOrderEnum.first | Wizard.DisplayOrderEnum.last;
                    View.DisplayAddUnavailable();
                    View.SendUserAction(idCommunity, Service.ServiceModuleID(), ModuleTemplateMessages.ActionType.TryToAddNewTemplate);
                }
                View.LoadWizardSteps(idCommunity, steps);
                if (String.IsNullOrEmpty(View.PreloadBackUrl))
                    View.SetBackUrl(RootObject.List(View.PreloadFromIdCommunity, t, ownerInfo, true, View.PreloadFromModuleCode, View.PreloadFromModulePermissions ));
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
            if (permission==null)
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
        public void AddNotificationSetting(lm.Comol.Core.Notification.Domain.NotificationChannel channel, List<dtoChannelConfigurator> items)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.Add(View.CurrentType, View.PreloadOwnership,View.PreloadFromIdCommunity, View.PreloadFromModuleCode,View.PreloadFromModulePermissions ));
            else
            {
                dtoChannelConfigurator config = Service.GetDraftChannelConfigurator(channel, View.CurrentType, 0);
                if (config == null)
                    View.DisplayTemplateUnableToAddNotificationChannel();
                else
                {
                    List<NotificationChannel> channels = Service.GetAvailableChannels(View.CurrentType, 0);
                    channels.Remove(channel);
                    channels = channels.Where(t => !items.Where(i => i.Channel == t).Any()).ToList();

                    //config.AvailableModuleCodes = View.GetModuleCodesForNotification().Select(m => m.Key).ToList();
                    items.Add(config);

                    //UpdateInUseSettings(items);
                    View.ChannelSettings = items;
                    View.LoadChannelSettings(items);
                    View.LoadChannels(channels);
                }
            }
        }
        public void VirtualDeleteNotificationSetting(List<dtoChannelConfigurator> items, System.Guid temporaryId)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.Add(View.CurrentType, View.PreloadOwnership, View.PreloadFromIdCommunity, View.PreloadFromModuleCode, View.PreloadFromModulePermissions));
            else
            {
                items = items.Where(i => i.TemporaryId != temporaryId).ToList();

                List<NotificationChannel> channels = Service.GetAvailableChannels(View.CurrentType, 0);
                channels = channels.Where(t => !items.Where(i => i.Channel == t).Any()).ToList();

                //UpdateInUseSettings(items);
                View.ChannelSettings = items;
                View.LoadChannelSettings(items);
                View.LoadChannels(channels);
            }
        }
        public void AddTemplate(String name,List<String> contentModules, List<dtoChannelConfigurator> items)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.Add(View.CurrentType, View.PreloadOwnership, View.PreloadFromIdCommunity, View.PreloadFromModuleCode, View.PreloadFromModulePermissions));
            else
            {
                dtoBaseTemplateOwner owner = View.Ownership;
                if (!contentModules.Any() && (String.IsNullOrEmpty(owner.ModuleCode) || (View.CurrentType == TemplateType.Module)))
                    contentModules.Add(owner.ModuleCode);
                TemplateDefinition template = Service.AddTemplate(name, View.CurrentType, owner, contentModules, items);
                if (template == null)
                    View.DisplayTemplateAddError();
                else if (View.PreloadCurrentSessionId == Guid.Empty)
                    View.GoToUrl(RootObject.EditByStep(View.CurrentType, View.Ownership, WizardTemplateStep.Settings,View.PreloadFromIdCommunity, View.PreloadFromModuleCode,View.PreloadFromModulePermissions, View.GetEncodedBackUrl(),template.Id, template.Versions[0].Id,false ,true ));
                else
                    View.GoToUrl(RootObject.EditByStep(View.CurrentType, View.Ownership, WizardTemplateStep.Settings, View.PreloadFromIdCommunity, View.PreloadFromModuleCode, View.PreloadFromModulePermissions, "", template.Id, template.Versions[0].Id, false, true));
            }
        }

        //private void UpdateInUseSettings(List<dtoNotificationConfigurator> items)
        //{
        //    if (items == null || !items.Any())
        //        View.InUseNotificationAction = new Dictionary<String, List<long>>();
        //    else
        //        View.InUseNotificationAction = items.Where(i=>!String.IsNullOrEmpty(i.Settings.ModuleCode)).Select(i => i.Settings.ModuleCode).ToDictionary(i => i, c => items.Where(item => item.Settings.ModuleCode == c).Select(i => i.Settings.IdModuleAction).ToList());
        //}
    }
}