using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;
namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class NotificationSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region initClass
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService service;
            private Int32 idModule;


            private lm.Comol.Core.BaseModules.TemplateMessages.Business.ModuleTemplateMessageService moduleService;
            private lm.Comol.Core.BaseModules.TemplateMessages.Business.ModuleTemplateMessageService ModuleService
            {
                get
                {
                    if (moduleService == null)
                        moduleService = new lm.Comol.Core.BaseModules.TemplateMessages.Business.ModuleTemplateMessageService(AppContext);
                    return moduleService;
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
            private lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService Service
            {
                get
                {
                    if (service == null)
                        service = new lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (idModule == 0)
                        idModule = CurrentManager.GetModuleID(ModuleTemplateMessages.UniqueCode);
                    return idModule;
                }
            }
       
            protected virtual IViewNotificationSettings View
            {
                get { return (IViewNotificationSettings)base.View; }
            }

            public NotificationSettingsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                service = new lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            public NotificationSettingsPresenter(iApplicationContext oContext, IViewNotificationSettings view)
                : base(oContext, view)
            {
                service = new lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean forPortal = false, Int32 idCommunity = 0, Int32 idOrganization = 0, ModuleObject obj=null)
        {
            liteCommunity c = CurrentManager.Get< liteCommunity>(idCommunity);
            if (idCommunity == 0 && forPortal == false)
            {
                idCommunity = UserContext.CurrentCommunityID;
                forPortal = (idCommunity == 0);
            }
            else if (idCommunity > 0 && forPortal)
                forPortal = false;
            idOrganization = (idOrganization == 0) ? ((c ==null) ? 0: c.IdOrganization) : idOrganization;

            InitializeView(forPortal, idCommunity,c, idOrganization, obj);
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                View.SettingsForObject = (obj !=null);
                if (obj == null)
                {

                    ModuleTemplateMessages permission = (forPortal) ? ModuleTemplateMessages.CreatePortalmodule((p == null) ? (Int32)UserTypeStandard.Guest : p.TypeID, OwnerType.System) : new ModuleTemplateMessages(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, CurrentIdModule));
                    if (permission.Administration)
                    {
                        View.SendUserAction(idCommunity, CurrentIdModule, ModuleTemplateMessages.ActionType.NotificationSettingsView);
                        List<lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages> permissions = InitializePermissions(forPortal, idCommunity, p);

                        LoadItems(GetItems(permissions, Service.GetNotificationSettings(forPortal, idCommunity, idOrganization), forPortal, idCommunity, idOrganization));
                    }
                    else
                        View.DisplayNoPermission(idCommunity, CurrentIdModule);
                }
                else{
                }
            }
        }
        private void InitializeView(Boolean forPortal = false, Int32 idCommunity = 0, liteCommunity community = null, Int32 idOrganization = 0, ModuleObject obj = null)
        {
            if (forPortal)
            {
                View.DisplayPortalInfo();
                View.SettingsLevel = TemplateLevel.Portal;
            }
            else if (obj != null)
            {
                View.DisplayObjectInfo(obj.ServiceCode, obj.ObjectTypeID);
                View.SettingsLevel = TemplateLevel.Object;
            }
            else if (community != null && community.IdOrganization == idOrganization && community.IdFather == 0)
            {
                View.DisplayOrganizationInfo(community.Name);
                View.SettingsLevel = TemplateLevel.Organization;
            }
            else if (community == null && idOrganization > 0)
            {
                View.DisplayOrganizationInfo(CurrentManager.GetOrganizationName(idOrganization));
                View.SettingsLevel = TemplateLevel.Organization;
            }
            else if (community != null)
            {
                View.DisplayCommunityInfo(community.Name);
                View.SettingsLevel = TemplateLevel.Community;
            }
            View.SettingsIdOrganization = idOrganization;
            View.SettingsForPortal = forPortal;
            View.SettingsIdCommunity = idCommunity;
            View.SettingsObj = obj;
        }
        private List<lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages> InitializePermissions(Boolean forPortal, Int32 idCommunity, Person person)
        {
            List<String> codes = View.GetAvailableModules(forPortal);
            if (idCommunity > 0)
                codes = (from m in CurrentManager.GetIQ<CommunityModule>() where m.Enabled && m.Community != null && m.Community.Id == idCommunity select m).ToList().Where(m => m.Service.Available && codes.Contains(m.Service.Code)).Select(m => m.Service.Code).ToList();
            else
                codes = (from m in CurrentManager.GetIQ<ModuleDefinition>() select m.Code).ToList().Where(c => codes.Contains(c)).Select(c => c).ToList();
            List<lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages> permissions = new List<lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages>();
            codes.ForEach(c => permissions.Add(View.GetModulePermissions(c, (idCommunity > 0) ? CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, CurrentManager.GetModuleID(c)) : 0, idCommunity, person.TypeID)));
            View.CurrentPermissions = permissions.ToDictionary(p => p.UniqueCode, p => p);
            return permissions;
        }
        public void SaveSettings(List<dtoModuleEvent> userSettings)
        { 
            Int32 idCommunity = View.SettingsIdCommunity;
            Int32 idOrganization = View.SettingsIdOrganization;
            ModuleObject obj = View.SettingsObj;
            Boolean forPortal = View.SettingsForPortal;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.Settings(forPortal, idCommunity, idOrganization, obj));
            else
            {
                List<CommonNotificationSettings> inUseSettings = Service.GetNotificationSettings(forPortal, idCommunity, idOrganization);
                List<dtoModuleEvents> items = GetItems(View.CurrentPermissions.Values.Select(v => v).ToList(), inUseSettings, forPortal, idCommunity, idOrganization,obj, userSettings);
                LoadItems(items);

                List<dtoModuleEvent> events = items.SelectMany(m => m.Events).ToList();
                Int32 mandatoryMissing = events.Where(ev=> ev.IsMandatory && (ev.Error== EventError.NoActionSelected || ev.Error== EventError.TemplateUnselected)).Count();
                if (mandatoryMissing > 0 && !events.Where(ev => ev.Error == EventError.None && ev.IsEnabled && ev.EditEnabled ).Any())
                    View.DisplayMandatoryMissing(mandatoryMissing);
                else
                {
                    TemplateLevel level = View.SettingsLevel;
                    Int32 inheritedItems = events.Where(e => e.ItemLevel != level && level != TemplateLevel.None && e.ItemLevel != TemplateLevel.None && e.IdTemplate > 0 && e.Error == EventError.None).Count();
                    List<CommonNotificationSettings> savedSettings = ModuleService.SaveNotificationSettings(level, userSettings.Where(ev => ev.Error != EventError.TemplateRemoved && ev.Error != EventError.VersionRemoved).ToList(), forPortal, idCommunity, idOrganization, obj);

                    Int32 unselectedItems = events.Where(ev => !ev.IsMandatory && (ev.Error == EventError.NoActionSelected || ev.Error == EventError.TemplateUnselected || (ev.Error == EventError.None && ev.IdTemplate <= 0))).Where(ev =>
                        (savedSettings == null || !savedSettings.Where(s => s.Settings.IdModuleAction == ev.IdEvent && s.Settings.ModuleCode == ev.ModuleCode && ((ev.IdTemplate <= 0 && s.Deleted != BaseStatusDeleted.None) || (ev.IdTemplate > 0 && s.Template != null && ev.IdTemplate == s.Template.Id))).Any())).Count();
                    if (savedSettings != null)
                    {
                        Int32 unsavedItems = events.Where(e => !e.IsMandatory || (e.IsMandatory && !(e.Error == EventError.NoActionSelected || e.Error == EventError.TemplateUnselected))).Count() - savedSettings.Count - unselectedItems - inheritedItems;
                        if (unsavedItems < 0)
                            unsavedItems = 0;
                        View.DisplaySavedSettings(savedSettings.Count, unselectedItems, unsavedItems, inheritedItems, mandatoryMissing);
                    }
                    else
                        View.DisplaySavingErrors();
                }
                View.SendUserAction(idCommunity, CurrentIdModule, ModuleTemplateMessages.ActionType.NotificationSettingsSave);
            }
        }

        public void ReloadItems(List<dtoModuleEvent> userSettings) {
            Int32 idCommunity = View.SettingsIdCommunity;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.Settings(View.SettingsForPortal, idCommunity, View.SettingsIdOrganization, View.SettingsObj));
            else
            {
                View.SendUserAction(idCommunity, CurrentIdModule, ModuleTemplateMessages.ActionType.NotificationSettingsEditing);
                if (View.SettingsForObject) {
                
                }
                else
                    LoadItems(GetItems(View.CurrentPermissions.Values.Select(v => v).ToList(), Service.GetNotificationSettings(View.SettingsForPortal, idCommunity, View.SettingsIdOrganization), View.SettingsForPortal, View.SettingsIdCommunity, View.SettingsIdOrganization,View.SettingsObj,userSettings));
            }
        }
        private void LoadItems(List<dtoModuleEvents> items)
        {
            if (items!=null && items.Any())
                View.LoadItems(items.OrderBy(m => m.ModuleName).ToList());
            else
                View.LoadEmptyItems();
        }

        private List<dtoModuleEvents> GetItems(List<lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages> permissions, List<CommonNotificationSettings> settings, Boolean forPortal, Int32 idCommunity, Int32 idOrganization,ModuleObject obj = null, List<dtoModuleEvent> userSettings = null)
        { 
            List<dtoModuleEvents> items = new List<dtoModuleEvents>();
            Dictionary<String, String> tModules = View.GetTranslatedModules(permissions.Select(p=>p.UniqueCode).ToList());
            Dictionary<String, Int32> modules = CurrentManager.GetIdModules(permissions.Select(p => p.UniqueCode).ToList());

            liteCommunity community = CurrentManager.Get<liteCommunity>(idCommunity);
            Int32 idOrganizationCommunity = 0;
            if (idOrganization > 0)
                idOrganizationCommunity = CurrentManager.GetIdCommunityFromOrganization(idOrganization);
            else if (community !=null && idOrganization <= 0)
                idOrganizationCommunity = (community.IdFather == 0) ? community.Id : CurrentManager.GetIdCommunityFromOrganization(community.IdOrganization);

            items = permissions.Select(p => new dtoModuleEvents()
            {
                Id = (modules.ContainsKey(p.UniqueCode) ? modules[p.UniqueCode] : 0),
                ModuleCode = p.UniqueCode,
                ModuleName = (tModules.ContainsKey(p.UniqueCode) ? tModules[p.UniqueCode] : "--"),
                Permissions = p,
                Events = View.GetTranslatedModuleActions(p.UniqueCode).Select(a => new dtoModuleEvent()
                {
                    IdEvent = a.IdEvent,
                    ModuleCode= p.UniqueCode,
                    IdModule = (modules.ContainsKey(p.UniqueCode) ? modules[p.UniqueCode] : 0),
                    Name = a.Name,
                    IsMandatory = a.IsMandatory,
                    Context = new lm.Comol.Core.TemplateMessages.Domain.dtoSelectorContext()
                    {
                        IdAction = a.IdEvent,
                        IdCommunity = idCommunity,
                        IsForPortal=forPortal,
                        IdModule = (modules.ContainsKey(p.UniqueCode) ? modules[p.UniqueCode] : 0),
                        ModuleCode= p.UniqueCode,
                        IdOrganization = idOrganization,
                        IdOrganizationCommunity = idOrganizationCommunity,
                        ObjectOwner = obj
                    },
                    EditEnabled= p.Administration,
                    Permissions = p
                }).ToList().OrderBy(a=> a.Name).ToList()
            }).Where(i=> i.Events.Any()).ToList();

            if (items.Any())
            {
                Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
                Language dLanguage = CurrentManager.GetDefaultLanguage();
                //foreach (dtoModuleEvents m in items) {
                //    m.Events = ParseItems(person, dLanguage, m.ModuleCode, m.Events, settings, forPortal, idCommunity, community, idOrganizationCommunity, idOrganization,null, userSettings);
                //}
                ParseModuleEvents(items, person, dLanguage, settings,forPortal,idCommunity, community,idOrganization,idOrganizationCommunity,obj, userSettings);
            }
            return items;
        }

        private void ParseModuleEvents(List<dtoModuleEvents> events, Person person, Language dLanguage, List<CommonNotificationSettings> settings, Boolean forPortal, Int32 idCommunity, liteCommunity community, Int32 idOrganization, Int32 idOrganizationCommunity, ModuleObject obj = null, List<dtoModuleEvent> userSettings = null)
        {
            foreach (dtoModuleEvents m in events)
            {
                foreach (dtoModuleEvent e in m.Events)
                {
                    CommonNotificationSettings notification = GetDefaultSettings(settings.Where(s => s.Settings.ModuleCode == e.ModuleCode && s.Settings.IdModuleAction == e.IdEvent).ToList(), e.Context );
                    if (userSettings != null && userSettings.Where(s => s.IdEvent == e.IdEvent && s.ModuleCode == e.ModuleCode).Any())
                    {
                        if (userSettings.Where(s => s.IdEvent == e.IdEvent && s.ModuleCode == e.ModuleCode && s.IdTemplate <= 0).Any())
                        {
                            e.IsEnabled = false;
                            e.Error = (!e.IsMandatory) ? EventError.None : ((notification == null) ? EventError.NoActionSelected : EventError.TemplateUnselected);
                        }
                        else
                        {
                            e.IdTemplate = userSettings.Where(s => s.IdEvent == e.IdEvent && s.ModuleCode == e.ModuleCode).Select(s => s.IdTemplate).FirstOrDefault();
                            e.IdVersion = userSettings.Where(s => s.IdEvent == e.IdEvent && s.ModuleCode == e.ModuleCode).Select(s => s.IdVersion).FirstOrDefault();
                            e.IsEnabled = true;
                            e.Error = EventError.None;
                        }
                    }
                    else if (notification != null)
                    {

                        e.IdTemplate = (notification.Template != null) ? notification.Template.Id : 0;
                        e.IdVersion = (notification.Template != null && notification.Version != null) ? notification.Version.Id : 0;
                        e.IsEnabled = (e.IdTemplate > 0);
                        e.Id = notification.Id;
                        e.Error = EventError.None;
                    }
                    else
                    {
                        e.Error = (e.IsMandatory) ? EventError.NoActionSelected : EventError.None;
                    }
                }
                List<long> inUseIdTemplates = (userSettings == null) ? settings.Where(s => s.Settings.ModuleCode != m.ModuleCode && s.Template != null).Select(s => s.Template.Id).Distinct().ToList() : userSettings.Where(us => us.ModuleCode != m.ModuleCode && us.IdTemplate>0).Select(us => us.IdTemplate).Distinct().ToList();
                List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateItem> mTemplates = GetModuleTemplates(person, new dtoSelectorContext() { IdCommunity = idCommunity, IdOrganization = idOrganization, IdOrganizationCommunity = idOrganizationCommunity, ObjectOwner = obj, IsForPortal = forPortal, ModuleCode = m.ModuleCode }, m.Permissions, inUseIdTemplates);
                if (mTemplates.Any())
                    mTemplates.ForEach(t => t.Name = Service.GetTemplateName(t.Id, person.LanguageID, dLanguage.Id));

                foreach (dtoModuleEvent e in m.Events)
                {
                    inUseIdTemplates = (userSettings == null) ? settings.Where(s => s.Settings.ModuleCode != m.ModuleCode && s.Template != null).Select(s => s.Template.Id).Distinct().ToList() : userSettings.Where(us => us.ModuleCode != m.ModuleCode && us.IdTemplate > 0).Select(us => us.IdTemplate).Distinct().ToList();
                    // remove other action in use
                    inUseIdTemplates.AddRange((userSettings == null) ? settings.Where(s => s.Settings.ModuleCode == m.ModuleCode && s.Settings.IdModuleAction != e.Context.IdAction && s.Template != null).Select(s => s.Template.Id).Distinct().ToList() : userSettings.Where(us => us.ModuleCode == m.ModuleCode && us.IdEvent != e.Context.IdAction && us.IdTemplate > 0).Select(us => us.IdTemplate).Distinct().ToList()); //&& us.IdEvent== e.Context.IdAction

                    e.Templates = GetAvailableTemplates(e.IdEvent, e.ModuleCode,mTemplates.Where(t => !inUseIdTemplates.Contains(t.Id)).ToList(), settings, userSettings, e.IdTemplate, e.IdVersion, true);
                    if (e.Templates.Any() && e.Templates.Where(t=> t.Id>0 && String.IsNullOrEmpty(t.Name)).Any())
                        e.Templates.Where(t => t.Id > 0 && String.IsNullOrEmpty(t.Name)).ToList().ForEach(t => t.Name = Service.GetTemplateName(t.Id, person.LanguageID, dLanguage.Id));
                    e.Templates = e.Templates.OrderBy(t => t.Level).ThenBy(t => t.Name).ToList();
                    if (e.Error == EventError.None && e.Templates.Where(t => t.Versions.Where(v => v.IsSelected && v.Status != TemplateStatus.Active).Any()).Any())
                    {
                        e.Error = (e.Templates.Where(t => t.Versions.Where(v => v.IsSelected && v.Status != TemplateStatus.Active && v.Number > 0).Any()).Any()) ? EventError.VersionRemoved : EventError.TemplateRemoved;
                    }
                }
            }
        }
        private CommonNotificationSettings GetDefaultSettings(List<CommonNotificationSettings> settings,dtoSelectorContext context)
        {
            CommonNotificationSettings notification = null;
            try
            {
                if (context.ObjectOwner != null)
                    settings = settings.Where(n =>
                                        (n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == context.ObjectOwner.ObjectLongID && n.ObjectOwner.ObjectTypeID == context.ObjectOwner.ObjectTypeID && n.ObjectOwner.ServiceCode == context.ObjectOwner.ServiceCode)
                                        ||
                                        (context.IdCommunity != -1 && (context.IdCommunity == n.IdCommunity))
                                        ||
                                        (context.IdOrganization != -1 && (context.IdOrganization == n.IdOrganization))
                                        ||
                                        n.IsForPortal
                                        ).ToList();
                else
                    settings = settings.Where(n =>
                                       n.ObjectOwner == null &&
                                       (
                                           (context.IdCommunity != -1 && (context.IdCommunity == n.IdCommunity))
                                           ||
                                           (context.IdOrganization != -1 && (context.IdOrganization == n.IdOrganization))
                                           ||
                                           n.IsForPortal
                                       )).ToList();
               
                if (settings.Any() && settings.Where(n => n.IsValid() && n.IsEnabled).Any())
                {
                    settings = settings.Where(n => n.IsValid() && n.IsEnabled).ToList();
                    notification = settings.Where(n => n.ObjectOwner != null && n.Settings.ActionType == lm.Comol.Core.Notification.Domain.NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                    if (notification == null)
                        notification = settings.Where(n => n.IdCommunity > 0 && n.Settings.ActionType == lm.Comol.Core.Notification.Domain.NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                    if (notification == null)
                        notification = settings.Where(n => n.IdOrganization > 0 && n.Settings.ActionType == lm.Comol.Core.Notification.Domain.NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                    if (notification == null)
                        notification = settings.Where(n => n.IsForPortal).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }

            return notification;
        }

        private List<dtoTemplateItem> GetModuleTemplates(Person person, dtoSelectorContext context, ModuleGenericTemplateMessages permissions, List<long> inUseIdTemplates)
        {
            List<dtoTemplateItem> items = new List<dtoTemplateItem>();
            List<TemplateDefinition> templates = GetTemplatesForSelection(context, permissions, person,  inUseIdTemplates);

            items.AddRange(templates.Where(t => t.Type == TemplateType.System).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Portal }).ToList());
            items.AddRange(templates.Where(t => t.Type != TemplateType.System && t.OwnerInfo.Community != null && context.IdOrganizationCommunity > 0 && t.OwnerInfo.Community.Id == context.IdOrganizationCommunity).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Organization }).ToList());
            items.AddRange(templates.Where(t => t.Type != TemplateType.System && t.OwnerInfo.Community != null && t.OwnerInfo.Community.Id == context.IdCommunity && context.IdCommunity != context.IdOrganizationCommunity).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Community }).ToList());

            return GetTemplateItems(items);
        }
        private List<TemplateDefinition> GetTemplatesForSelection(dtoSelectorContext context, ModuleGenericTemplateMessages permissions, Person person, List<long> inUseIdTemplates)
        {
            List<TemplateDefinition> items = new List<TemplateDefinition>();
            try
            {
                List<TemplateDefinition> templates = Service.GetTemplatesForSelection(context, person).Where(t => !inUseIdTemplates.Contains(t.Id)).ToList();

                foreach (TemplateDefinition template in templates)
                {
                    dtoTemplatePermission permission = null;
                    if (template.OwnerInfo.ModuleCode != context.ModuleCode && ((template.OwnerInfo.Community == null && template.OwnerInfo.IsPortal && context.IdCommunity < 1) || (template.OwnerInfo.Community != null && template.OwnerInfo.Community.Id == context.IdCommunity)))
                        permission = InternalService.GetManagementTemplatePermission(template.LastVersion, permissions);
                    else
                        permission = InternalService.GetItemPermission(template.LastVersion);
                    if (permission.AllowUse)
                        items.Add(template);
                }
            }
            catch (Exception ex)
            {

            }
            return items;
        }
        private List<dtoTemplateItem> GetTemplateItems(List<dtoTemplateItem> items)
        {
            try
            {
                if (items != null && items.Any())
                {
                    items.ForEach(t => t.Versions = (from v in CurrentManager.GetIQ<TemplateDefinitionVersion>()
                                                     where (v.Deleted == BaseStatusDeleted.None && v.Status != TemplateStatus.Draft) && v.Template.Id == t.Id
                                                     select new dtoVersionItem(t.Level)
                                                     {
                                                         Id = v.Id,
                                                         Status = v.Status,
                                                         Number = v.Number,
                                                         IdTemplate = v.Template.Id,
                                                         Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn
                                                     }
                                    ).ToList().OrderByDescending(v => v.Number).ToList());

                    DateTime lastVersion = DateTime.Now;
                    items.Where(t => t.Versions.Count > 0).ToList().ForEach(t => t.Versions.Insert(0,
                        new dtoVersionItem(t.Level)
                        {
                            Id = 0,
                            Status = TemplateStatus.Active,
                            Lastmodify = lastVersion,
                            Number = 0,
                            IdTemplate = t.Id
                        }));
                }
            }
            catch (Exception ex)
            {
            }
            return items;
        }
        private List<dtoTemplateItem> GetAvailableTemplates(long idAction,String moduleCode,List<dtoTemplateItem> items,List<CommonNotificationSettings> settings, List<dtoModuleEvent> userSettings = null,  long idTemplate = 0, long idVersion = 0, Boolean removeReplaced=false)
        {
            List<dtoTemplateItem> results = new List<dtoTemplateItem>();
            try
            {
                dtoTemplateItem current = items.Where(i=> idTemplate>0 && i.Id==idTemplate).FirstOrDefault();
                if (current != null)
                {
                    dtoVersionItem vItem = current.Versions.Where(v=> v.Id== idVersion).FirstOrDefault();
                    if (vItem != null)
                        vItem.IsSelected = true;
                    else {
                        vItem = (from v in CurrentManager.GetIQ<TemplateDefinitionVersion>()
                                where v.Id == idVersion && v.Template.Id == idTemplate
                                 select new dtoVersionItem(current.Level)
                                {
                                    Id = v.Id,
                                    Status = v.Status,
                                    Number = v.Number,
                                    IdTemplate = current.Id,
                                    Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn,
                                    IsSelected = true
                                }
                                 ).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (vItem != null) { 
                            dtoVersionItem pVersion = current.Versions.Where(v=> v.Number < vItem.Number).OrderByDescending(v=>v.Number).FirstOrDefault();
                            if (pVersion == null || (pVersion != null && pVersion.Number == 0))
                                current.Versions.Add(vItem);
                            else
                                current.Versions.Insert(current.Versions.Select(v => v.Id).ToList().IndexOf(pVersion.Id), vItem);
                        }
                    }
                }

                // Recupero tutte le impostazioni base presenti su DB
                List<CommonNotificationSettings> mSettings = settings.Where(s => s.Settings.ModuleCode == moduleCode && s.Settings.IdModuleAction== idAction).ToList();
                // SE ci sono selezioni lato utente TOLGO quelli presenti
                if (userSettings != null)
                    mSettings = mSettings.Where(ms => !(userSettings.Where(us => us.ModuleCode == moduleCode && us.IdEvent == idAction && us.IdTemplate > 0 && ms.Template != null && (ms.Template.Id != us.IdTemplate)).Any())).ToList();
                // Effettuo le verifiche del caso su
                // PORTALE
                results.AddRange(CreateNotificationItems(mSettings.Where(s => s.IsForPortal && s.ObjectOwner == null).ToList(), items, TemplateLevel.Portal));
                // Organizzazione
                results.AddRange(CreateNotificationItems(mSettings.Where(s => !s.IsForPortal && s.ObjectOwner == null && s.IdOrganization >0).ToList(), items, TemplateLevel.Organization));
                // Comunità
                results.AddRange(CreateNotificationItems(mSettings.Where(s => !s.IsForPortal && s.ObjectOwner == null && s.IdOrganization <= 0 && s.IdCommunity > 0).ToList(), items, TemplateLevel.Community));
                // oggetto
                results.AddRange(CreateNotificationItems(mSettings.Where(s => s.ObjectOwner != null).ToList(), items, TemplateLevel.Object));

                PopulateNotificationItems(results, idTemplate, idVersion, (current!=null && current.Versions.Where(v=>v.IsSelected).Any()));
                List<long> idInUse = results.Select(r=> r.Id).ToList();
                results.AddRange(items.Where(i=> !idInUse.Contains(i.Id)).ToList());
                
                //Add Selected: Solo se il TEMPLATE indicato NON è stato trovato!
                if (!results.Where(t => t.Versions.Where(v => v.IsSelected == true).Any()).Any() && (idTemplate > 0 || idVersion > 0))
                {
                    dtoTemplateItem dtoTemplate = null;
                    dtoVersionItem dtoVersion = null;

                    if (idVersion <= 0) //LAST VERSION! Se non ci sono versioni "Definitive", recupero l'ultima "Deprecata"...
                    {
                        dtoTemplate = (from t in CurrentManager.GetIQ<TemplateDefinition>() where t.Id == idTemplate
                                       select new dtoTemplateItem() {Id = t.Id,Level = TemplateLevel.Removed}).Skip(0).Take(1).ToList().FirstOrDefault();

                        if (dtoTemplate != null)
                        {
                            dtoVersion = (from v in CurrentManager.GetIQ<TemplateDefinitionVersion>()
                                          where v.Deleted == BaseStatusDeleted.None && v.Status != TemplateStatus.Draft && v.Template.Id == dtoTemplate.Id
                                          select new dtoVersionItem()
                                          {
                                              Id = v.Id,Status = v.Status,Number = v.Number,
                                              IdTemplate = v.Template.Id,Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn
                                          }
                                        ).ToList().OrderByDescending(v => v.Number).Skip(0).Take(1).ToList().FirstOrDefault();
                        }
                    }
                    else
                    {
                        dtoVersion = (from v in CurrentManager.GetIQ<TemplateDefinitionVersion>()
                                      where v.Id == idVersion
                                      select new dtoVersionItem(TemplateLevel.Removed)
                                      {
                                          Id = v.Id,
                                          Status = v.Status,
                                          Number = v.Number,
                                          IdTemplate = v.Template.Id,
                                          Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn,
                                          IsSelected = true
                                      }
                                    ).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (dtoVersion != null)
                        {
                            dtoTemplate = (from t in CurrentManager.GetIQ<TemplateDefinition>()
                                           where t.Id == idTemplate
                                           select new dtoTemplateItem()
                                           {
                                               Id = t.Id,
                                               Level = TemplateLevel.Removed
                                           }).Skip(0).Take(1).ToList().FirstOrDefault();
                            //Controllo la versione recuperata da dB:
                            // Se NON soddisfa i requisiti, viene impostata a null
                            if (!(dtoTemplate != null && dtoTemplate.Id == dtoVersion.IdTemplate))
                                dtoVersion = null;
                        }
                    }
                    DateTime lastVersion = DateTime.Now;
                    if (dtoTemplate != null && dtoVersion != null)
                    {
                        dtoTemplate.Versions.Add(dtoVersion);
                        dtoTemplate.Versions.Insert(0,
                            new dtoVersionItem(dtoTemplate.Level)
                            {
                                Id = 0,
                                Status = TemplateStatus.Active,
                                Lastmodify = lastVersion,
                                Number = 0,
                                IdTemplate = dtoTemplate.Id,
                                IsSelected = (idVersion == 0 && dtoTemplate.Id == idTemplate)
                            });
                        results.Add(dtoTemplate);
                    }
                }
                results = results.Where(i => i.Versions.Any()).ToList();
                if (removeReplaced)
                    results.ForEach(i => i.Versions = i.Versions.Where(v => !(v.IsSelected == false && v.Status == TemplateStatus.Replaced)).ToList());
            }
            catch (Exception ex)
            {
            }

            return results;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notifications">Lista impostazioni</param>
        /// <param name="cItems">Elementi correnti</param>
        /// <returns></returns>
        private List<dtoTemplateItem> CreateNotificationItems(List<CommonNotificationSettings> notifications,List<dtoTemplateItem> cItems ,TemplateLevel level) {
            List<dtoTemplateItem> items = new List<dtoTemplateItem>();
            foreach (CommonNotificationSettings notification in notifications)
            {
                dtoTemplateItem pItem = new dtoTemplateItem() { Id = notification.Template.Id, Level = level };
                dtoTemplateItem lItem = items.Where(i => i.Id == pItem.Id).FirstOrDefault();
                if (lItem == null)
                    items.Add(pItem);
                else if (lItem.Level != level)
                {
                    lItem.Level = level;
                    lItem.Versions.ForEach(v => v.Level = level);
                }
            }
            return items;
        }
        private void PopulateNotificationItems(List<dtoTemplateItem> items, long idTemplate = 0, long idVersion = 0, Boolean hasSelectedItem = false )
        {
            try
            {
                if (items != null && items.Any())
                {
                    items.ForEach(t => t.Versions = (from v in CurrentManager.GetIQ<TemplateDefinitionVersion>()
                                                     where ((idVersion== 0 || (idVersion>0 && idVersion==v.Id))&& (v.Deleted == BaseStatusDeleted.None && v.Status != TemplateStatus.Draft)) && v.Template.Id == t.Id
                                                     select new dtoVersionItem(t.Level)
                                                     {
                                                         Id = v.Id,
                                                         Status = v.Status,
                                                         Number = v.Number,
                                                         IdTemplate = v.Template.Id,
                                                         Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn,
                                                         IsSelected = (v.Id == idVersion && t.Id == idTemplate) && !hasSelectedItem
                                                     }
                                    ).ToList().OrderByDescending(v => v.Number).ToList());

                    dtoTemplateItem template = items.Where(t => t.Versions.Count == 0 && t.Id == idTemplate).FirstOrDefault();
                    if (template != null)
                    {
                        dtoVersionItem ver = (from v in CurrentManager.GetIQ<TemplateDefinitionVersion>()
                                              where (v.Id == idVersion || (v.Deleted == BaseStatusDeleted.None && v.Status != TemplateStatus.Draft)) && v.Template.Id == template.Id
                                              select new dtoVersionItem(template.Level)
                                              {
                                                  Id = v.Id,
                                                  Status = v.Status,
                                                  Number = v.Number,
                                                  IdTemplate = template.Id,
                                                  Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn,
                                                  IsSelected = (idVersion == 0 && template.Id == idTemplate) && !hasSelectedItem
                                              }
                                                ).ToList().OrderByDescending(v => v.Number).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (ver != null && !template.Versions.Where(v=>v.Id==ver.Id).Any())
                            template.Versions.Add(ver);
                    }

                    DateTime lastVersion = DateTime.Now;
                    items.Where(t => t.Versions.Count > 0).ToList().ForEach(t => t.Versions.Insert(0,
                        new dtoVersionItem(t.Level)
                        {
                            Id = 0,
                            Status = TemplateStatus.Active,
                            Lastmodify = lastVersion,
                            Number = 0,
                            IdTemplate = t.Id,
                            IsSelected = (idVersion == 0 && t.Id == idTemplate)
                        }));
                }
            }
            catch (Exception ex)
            {
            }
        }

        //private List<dtoModuleEvent> ParseItems(Person person, Language dLanguage, String moduleCode, List<dtoModuleEvent> events, List<CommonNotificationSettings> settings, Boolean forPortal, Int32 idCommunity, Community community, Int32 idOrganizationCommunity, Int32 idOrganization, ModuleObject obj = null, List<dtoModuleEvent> userSettings = null)
        //{
        //    foreach (dtoModuleEvent e in events)
        //    {
        //        CommonNotificationSettings notification = GetDefaultSettings(settings.Where(s => s.Settings.ModuleCode == moduleCode && s.Settings.IdModuleAction == e.IdEvent).ToList(), forPortal, idCommunity, community, idOrganization, obj);
        //        if (userSettings != null && userSettings.Where(s => s.IdEvent == e.IdEvent && s.ModuleCode == e.ModuleCode).Any())
        //        {
        //            if (userSettings.Where(s => s.IdEvent == e.IdEvent && s.ModuleCode == e.ModuleCode && s.IdTemplate <= 0).Any())
        //            {
        //                e.IsEnabled = false;
        //                e.Error = (!e.IsMandatory) ? EventError.None : ((notification == null) ? EventError.NoActionSelected : EventError.TemplateUnselected);
        //            }
        //            else
        //            {
        //                e.IdTemplate = userSettings.Where(s => s.IdEvent == e.IdEvent && s.ModuleCode == e.ModuleCode).Select(s => s.IdTemplate).FirstOrDefault();
        //                e.IdVersion = userSettings.Where(s => s.IdEvent == e.IdEvent && s.ModuleCode == e.ModuleCode).Select(s => s.IdVersion).FirstOrDefault();
        //                e.IsEnabled = true;
        //                e.Error = EventError.None;
        //            }
        //        }
        //        else if (notification != null)
        //        {

        //            e.IdTemplate = (notification.Template != null) ? notification.Template.Id : 0;
        //            e.IdVersion = (notification.Template != null && notification.Version != null) ? notification.Version.Id : 0;
        //            e.IsEnabled = (e.IdTemplate > 0);
        //            e.Id = notification.Id;
        //            e.Error = EventError.None;
        //        }
        //        else
        //        {
        //            e.Error = (e.IsMandatory) ? EventError.NoActionSelected : EventError.None;
        //        }
        //        e.Templates = GetAvailableTemplates(person, dLanguage, settings, e.Context, e.Permissions, idOrganizationCommunity, e.IdTemplate, e.IdVersion, userSettings);
        //        if (e.Error == EventError.None && e.Templates.Where(t => t.Versions.Where(v => v.IsSelected && v.Status != TemplateStatus.Active).Any()).Any())
        //        {
        //            e.Error = (e.Templates.Where(t => t.Versions.Where(v => v.IsSelected && v.Status != TemplateStatus.Active && v.Version > 0).Any()).Any()) ? EventError.VersionRemoved : EventError.TemplateRemoved;
        //        }

        //    }

        //    return events;
        //}

        //private List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateItem> GetAvailableTemplates( Person person,Language dLanguage, List<CommonNotificationSettings> nSettings, dtoSelectorContext context,ModuleGenericTemplateMessages permissions, Int32 idOrganizationCommunity, long idTemplate = 0, long idVersion = 0,List<dtoModuleEvent> userSettings = null)
        //{
        //    List<long> idTemplates = (userSettings!= null) ? userSettings.Where(us=>us.IdTemplate>0).Select(us=> us.IdTemplate).Distinct().ToList() : nSettings.Where(s=> s.Template != null).Select(s=> s.Template.Id).Distinct().ToList();
        //    List<CommonNotificationSettings> eventSettings = nSettings.Where(n => n.Settings.ModuleCode == context.ModuleCode && n.Settings.IdModuleAction == context.IdAction
        //                                                               && (userSettings == null ||
        //                                                                   (userSettings != null && userSettings.Where(u => u.IdEvent == n.Settings.IdModuleAction && u.ModuleCode == n.Settings.ModuleCode && (n.Template != null && u.IdTemplate == n.Template.Id) && ((n.Version == null && u.IdVersion <= 0) || (n.Version != null && n.Version.Id == u.IdVersion))).Any()))
        //                                                                   ).ToList();
        //    if (idTemplates.Any() && userSettings != null)
        //    {
        //        dtoModuleEvent current = userSettings.Where(us => us.IdEvent == context.IdAction && us.ModuleCode == context.ModuleCode).FirstOrDefault();
        //        idTemplates = (current != null && !eventSettings.Where(ev => ev.Template != null && ev.Template.Id == current.IdTemplate && ((ev.AlwaysLastVersion && current.IdVersion <= 0) || (!ev.AlwaysLastVersion && current.IdVersion > 0 && ev.Version != null && ev.Version.Id == current.IdVersion))).Any()) ? idTemplates.Where(i=> i !=current.IdTemplate).ToList() : idTemplates;
        //    }

        //    if (eventSettings != null)
        //        eventSettings = eventSettings.Where(e => (e.Template != null && e.Template.Deleted != BaseStatusDeleted.None)
        //                                            || (e.Template != null && e.Version != null && e.Version.Deleted != BaseStatusDeleted.None)).ToList();
            
        //    List<TemplateDefinition> templates = GetTemplatesForSelection(context, permissions, person,nSettings, idTemplates);
        //    List<dtoTemplateItem> items = new List<dtoTemplateItem>();

        //    items.AddRange(eventSettings.Where(s => s.IsForPortal).Select(s => new dtoTemplateItem() { Id = s.Template.Id, Level = TemplateLevel.Portal }).ToList());
        //    items.AddRange(templates.Where(t => t.Type == TemplateType.System ).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Portal }).ToList());
        //    items.AddRange(templates.Where(t => t.Type != TemplateType.System && t.OwnerInfo.Community != null && idOrganizationCommunity > 0 && t.OwnerInfo.Community.Id == idOrganizationCommunity).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Organization }).ToList());

        //    items.AddRange(eventSettings.Where(s => !s.IsForPortal && s.IdCommunity == context.IdCommunity).Select(s => new dtoTemplateItem() { Id = s.Template.Id, Level = TemplateLevel.Community }).ToList());
        //    items.AddRange(templates.Where(t => t.Type != TemplateType.System  && t.OwnerInfo.Community != null && t.OwnerInfo.Community.Id == context.IdCommunity && context.IdCommunity != idOrganizationCommunity).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Community }).ToList());
        //    items.AddRange(eventSettings.Where(s => s.ObjectOwner != null).Select(s => new dtoTemplateItem() { Id = s.Template.Id, Level = TemplateLevel.Object }).ToList());


        //    return Service.ParseAvailableTemplates(items, dLanguage, idTemplate, idVersion,true);
        //}
        //private List<TemplateDefinition> GetTemplatesForSelection(dtoSelectorContext context, ModuleGenericTemplateMessages permissions, Person person, List<CommonNotificationSettings> settings, List<long> removeIdTemplates)
        //{
        //    List<TemplateDefinition> items = new List<TemplateDefinition>();
        //    try
        //    {
        //        List<TemplateDefinition> templates = Service.GetTemplatesForSelection(context, person).Where(t => !removeIdTemplates.Contains(t.Id)).ToList();

        //       //// List<long> tActions = settings.Where(ns =>s.Settings.ModuleCode == moduleCode && ns.Settings.IdModuleAction == context.IdAction).Select(ns => ns.Template.Id).ToList();
        //       // List<long> eActions = new List<long>();
        //       //     ////if (context.AlsoEmptyActions)
        //       // eActions = settings.Where(ns => ns.Settings.IdModuleAction != context.IdAction).Select(ns => ns.Template.Id).ToList();
        //       //     ////templates = templates.Where(t => tActions.Contains(t.Id) || (context.AlsoEmptyActions && !eActions.Contains(t.Id))).ToList();
        //       // templates = templates.Where(t => !eActions.Contains(t.Id)).ToList();
                

        //        foreach (TemplateDefinition template in templates)
        //        {
        //            dtoTemplatePermission permission = null;
        //            if (template.OwnerInfo.ModuleCode != context.ModuleCode && ((template.OwnerInfo.Community == null && template.OwnerInfo.IsPortal && context.IdCommunity < 1) || (template.OwnerInfo.Community != null && template.OwnerInfo.Community.Id == context.IdCommunity)))
        //                permission = InternalService.GetManagementTemplatePermission(template.LastVersion, permissions);
        //            else
        //                permission = InternalService.GetItemPermission(template.LastVersion);
        //            if (permission.AllowUse)
        //                items.Add(template);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return items;
        //}
    }
}