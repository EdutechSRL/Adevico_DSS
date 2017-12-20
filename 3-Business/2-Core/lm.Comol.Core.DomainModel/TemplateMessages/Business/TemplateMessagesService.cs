using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using NHibernate.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.Notification.Domain;

namespace lm.Comol.Core.TemplateMessages.Business
{
    public class TemplateMessagesService : CoreServices 
    {
        protected const Int32 maxItemsForQuery = 500;
        protected iApplicationContext _Context;

        #region initClass
            public TemplateMessagesService() :base() { }
            public TemplateMessagesService(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
            }
            public TemplateMessagesService(iDataContext oDC)
                : base(oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext() { DataContext = oDC };
            }
        #endregion

        #region "Permission"
            public int ServiceModuleID()
            {
                return this.Manager.GetModuleID(ModuleTemplateMessages.UniqueCode);
            }
            public ModuleTemplateMessages GetPermission(Int32 idCommunity,OwnerType type) {
                Person person = Manager.GetPerson(UC.CurrentUserID);
                if (idCommunity <= 0)
                {
                    if (person == null)
                        return ModuleTemplateMessages.CreatePortalmodule((int)UserTypeStandard.Guest, type);
                    else
                        return ModuleTemplateMessages.CreatePortalmodule(person.TypeID,type);
                }
                else {
                    return new ModuleTemplateMessages(this.Manager.GetModulePermission(UC.CurrentUserID, idCommunity, ServiceModuleID()));
                }
            }
            public Boolean AllowAdd(dtoBaseTemplateOwner info)
            {
                Boolean result = false;
                switch (info.Type) {
                    case OwnerType.Community:
                    case OwnerType.System:
                        ModuleTemplateMessages m = GetPermission(info.IdCommunity, info.Type);
                        return (m.Add || m.Administration);
                    case OwnerType.Module:
                        return Manager.HasModulePermission(UC.CurrentUserID, info.IdCommunity, info.IdModule, info.ModulePermission);
                    case OwnerType.Person:
                        Person person = Manager.GetPerson(UC.CurrentUserID);
                        return (person!=null && person.TypeID !=(int)UserTypeStandard.Guest && person.TypeID !=(int)UserTypeStandard.PublicUser);
                }
                return result;
            }
            public dtoTemplatePermission GetItemPermission(long idVersion) {
                return GetItemPermission(Manager.Get<TemplateDefinitionVersion>(idVersion));
            }
            public dtoTemplatePermission GetItemPermission(TemplateDefinitionVersion version)
            {
                dtoTemplatePermission p = new dtoTemplatePermission();
                try
                {
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (version != null && version.Template !=null && person!=null )
                    {
                        Boolean isOwner = (version.Owner.Id == person.Id) || (version.Template.OwnerInfo.Person != null && version.Template.OwnerInfo.Person.Id==person.Id);
                        ModuleTemplateMessages m = null;
                        if (version.Template.OwnerInfo.Type== OwnerType.System)
                            m = GetPermission(0, version.Template.OwnerInfo.Type);
                        else if (version.Template.OwnerInfo.Type== OwnerType.Community)
                            m = GetPermission((version.Template.OwnerInfo.Community == null) ? -1 : version.Template.OwnerInfo.Community.Id, version.Template.OwnerInfo.Type);

                        p.AllowChangePermission = isOwner || ((m!= null && m.Administration));
                        p.AllowClone = isOwner || ((m != null && (m.Clone || m.Administration)));
                        p.AllowEdit = isOwner || ((m != null && (m.Administration || m.Edit)));
                        p.AllowUse = (version.Deleted == BaseStatusDeleted.None) && (isOwner || (m != null && (m.List || m.Administration || m.Edit)));
                        p.AllowDelete = (version.Deleted != BaseStatusDeleted.None) && ((m == null && isOwner) || (m != null && (m.Administration || m.Edit || (m.DeleteMyTemplates && isOwner) || m.DeleteOtherTemplates)));
                        p.AllowUnDelete = (version.Deleted == BaseStatusDeleted.Manual) && ((m == null && isOwner) || (m != null && (m.Administration || m.Edit || (m.DeleteMyTemplates && isOwner) || m.DeleteOtherTemplates)));
                        p.AllowVirtualDelete = (version.Deleted == BaseStatusDeleted.None) && ((m == null && isOwner) || (m != null && (m.Administration || m.Edit || (m.DeleteMyTemplates && isOwner) || m.DeleteOtherTemplates)));

                        if (version.AvailablePermission().Any() && !isOwner)
                        {
                            VersionPersonPermission pPermission = version.AvailableUsersPermission().Where(pm => pm.AssignedTo.Id == person.Id).FirstOrDefault();
                            if (pPermission != null)
                                UpdatePermission(p, pPermission);
                            else {
                                VersionProfileTypePermission ptPermission = version.AvailableProfilesPermission().Where(pm => pm.AssignedTo== person.TypeID).FirstOrDefault();
                                if (ptPermission != null)
                                    UpdatePermission(p, ptPermission);
                                else
                                    UpdatePermission(p,person, version.AvailableRolesPermission(), version.AvailableCommunitiesPermission());
                            }
                        }
                    }
                }
                catch (Exception ex) {
                
                }
              
                return p;
            }
            public dtoTemplatePermission GetManagementTemplatePermission(TemplateDefinitionVersion version,ModuleGenericTemplateMessages module)
            {
                dtoTemplatePermission p = new dtoTemplatePermission();
                try
                {
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (version != null && version.Template != null && person != null)
                    {
                        Boolean isOwner = (version.Owner.Id == person.Id);

                        p.AllowAdd = isOwner || ((module != null && (module.Administration && module.Add)));
                        p.AllowChangePermission = isOwner || ((module != null && module.Administration));
                        p.AllowClone = isOwner || ((module != null && (module.Clone || module.Administration)));
                        p.AllowEdit = isOwner || ((module != null && (module.Administration || module.Edit)));
                        p.AllowUse = (isOwner || (module != null && (module.List || module.Administration || module.Edit)));
                        p.AllowDelete = ((module == null && isOwner) || (module != null && (module.Administration || module.Edit || (module.DeleteMyTemplates && isOwner) || module.DeleteOtherTemplates)));
                        p.AllowUnDelete = ((module == null && isOwner) || (module != null && (module.Administration || module.Edit || (module.DeleteMyTemplates && isOwner) || module.DeleteOtherTemplates)));
                        p.AllowVirtualDelete = ((module == null && isOwner) || (module != null && (module.Administration || module.Edit || (module.DeleteMyTemplates && isOwner) || module.DeleteOtherTemplates)));

                        if (version.AvailablePermission().Any() && !isOwner)
                        {
                            VersionPersonPermission pPermission = version.AvailableUsersPermission().Where(pm => pm.AssignedTo.Id == person.Id).FirstOrDefault();
                            if (pPermission != null)
                                UpdatePermission(p, pPermission);
                            else
                            {
                                VersionProfileTypePermission ptPermission = version.AvailableProfilesPermission().Where(pm => pm.AssignedTo == person.TypeID).FirstOrDefault();
                                if (ptPermission != null)
                                    UpdatePermission(p, ptPermission);
                                else
                                    UpdatePermission(p, person, version.AvailableRolesPermission(), version.AvailableCommunitiesPermission());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return p;
            }
            private void UpdatePermission(dtoTemplatePermission dto, VersionPermission p)
            {
                dto.AllowClone = p.Clone;
                dto.AllowChangePermission = p.ChangePermission;
                dto.AllowEdit = p.Edit;
                dto.AllowDelete &= p.Edit;
                dto.AllowUnDelete &= p.Edit;
                dto.AllowVirtualDelete &= p.Edit;
                dto.AllowUse = p.See;
            }
            private void UpdatePermission(dtoTemplatePermission dto, Person person , List<VersionRolePermission> roles, List<VersionCommunityPermission> communities)
            {
                if (person != null)
                {
                    List<Int32> idCommunities = null;
                   // if (UC.CurrentCommunityID >0)
                    idCommunities= (communities == null) ? new List<Int32>() : communities.Where(c => c.AssignedTo != null).Select(c => c.AssignedTo.Id).ToList();
                    idCommunities.AddRange((roles==null) ? new List<Int32>() : roles.Where(c => c.Community != null).Select(c => c.Community.Id).ToList());
                   
                    if (idCommunities.Any())
                    {

                        List<LazySubscription> subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);
                        //if (roles != null)
                        //{
                        //    VersionRolePermission rolePermission = roles.Where(
                        //}
                        
                       // permission = subscriptions.Where(s => idCommunities.Contains(s.IdCommunity)).Any();
                    }
                }
            }
            //private Boolean IsTemplateAvailableByRole(long idCall, Person person)
            //{
            //    Boolean permission = false;
            //    if (person != null)
            //    {
            //        var query = (from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
            //                     where pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.Community != null
            //                     select pa);
            //        List<Int32> idCommunities = query.Select(a => a.Community.Id).Distinct().ToList();
            //        if (idCommunities.Any())
            //        {
            //            List<LazySubscription> subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);
            //            var result = query.Select(a => new { IdRole = a.AssignedTo.Id, IdCommunity = a.Community.Id }).ToList();
            //            permission = result.Where(a => subscriptions.Where(s => s.IdCommunity == a.IdCommunity && s.IdRole == a.IdRole).Any()).Any();
            //        }
            //    }
            //    return permission;
            //}
            //public dtoTemplatePermission GetPermission(dtoBaseTemplateOwner info, long idTemplate = 0, long idVersion = 0)
            //{
            //    ModuleTemplateMessages m = null;
            //    dtoTemplatePermission p = new dtoTemplatePermission();
            //    switch (info.Type) { 
            //        case OwnerType.Community:
            //            m = GetPermission(info.IdCommunity);
            //            break;
            //        case OwnerType.System:

            //        case OwnerType.Module:

            //        case OwnerType.Person:
                        

            //    }
                
            //    return p;
            //}


        
        #endregion

        #region "Wizard"
            public List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> GetAvailableSteps(long idVersion, WizardTemplateStep current, OwnerType type)
            {
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>>();
                WizardTemplateStep startStep = WizardTemplateStep.Settings;
                TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);

                if (current == WizardTemplateStep.None)
                    current = startStep;

                dtoSettingsStep step = GetSettingsStepInfo(version, WizardTemplateStep.Settings);
                items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>()
                {
                    DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.first,
                    Id = step,
                    Status = step.Status,
                    AutoPostBack = current != startStep,
                    Active = (current == startStep)
                });
                items.AddRange(GetStepsForVersion(version, current, type));
                return items;
            }
            private List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> GetStepsForVersion(TemplateDefinitionVersion version, WizardTemplateStep current, OwnerType type)
            {
                List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>> items = new List<lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>>();

                dtoTranslationsStep tStep = GetTranslationsStepInfo(version);
                if (tStep != null)
                {
                    items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>()
                    {
                        Id = tStep,
                        Status = tStep.Status,
                        AutoPostBack = true,
                        Active = (current == tStep.Type)
                    });

                }
                if (type != OwnerType.None && type != OwnerType.Object)
                {
                    dtoPermissionStep pStep = GetPermissionStepInfo(version);
                    if (pStep != null)
                    {
                        items.Add(new lm.Comol.Core.Wizard.NavigableWizardItem<dtoTemplateStep>()
                        {
                            Id = pStep,
                            Status = pStep.Status,
                            AutoPostBack = true,
                            Active = (current == pStep.Type)
                        });

                    }
                }
                if (items.Count > 0)
                    items.Last().DisplayOrderDetail = Wizard.DisplayOrderEnum.last;
                return items;
            }
            protected dtoSettingsStep GetSettingsStepInfo(TemplateDefinitionVersion version, WizardTemplateStep step)
            {
                dtoSettingsStep item = new dtoSettingsStep(step);
                if (version != null)
                {
                    List<ChannelSettings> nSettings = (from s in Manager.GetIQ<ChannelSettings>()
                                                            where s.Version == version && s.Deleted == BaseStatusDeleted.None
                                                            select s).ToList();

                    item.VersionStatus = version.Status;
                    item.NotificationTypes = nSettings.Count;
                }
                item.Status = (version == null) ? Core.Wizard.WizardItemStatus.none : (item.VersionStatus == TemplateStatus.Draft) ? Core.Wizard.WizardItemStatus.warning : Core.Wizard.WizardItemStatus.valid;
                return item;
            }
            protected dtoTranslationsStep GetTranslationsStepInfo(TemplateDefinitionVersion version)
            {
                dtoTranslationsStep item = new dtoTranslationsStep(WizardTemplateStep.Translations);
                if (version != null)
                {
                    item.HasMultilingua = version.DefaultTranslation.IsValid(!version.OnlyShortText, version.HasShortText, true);
                    item.Count = (version.Translations != null) ? version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Count() : 0;
                    if (item.Count > 0)
                    {
                        item.Empty = version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.Translation.IsContentEmpty()).Select(t => t.Id).Count();
                        item.Completed = version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.Translation.IsValid(!version.OnlyShortText, version.HasShortText, true)).Select(t => t.Id).Count();
                    }
                    item.Status = (item.HasMultilingua && item.Empty == 0 && item.InvalidItems == 0) ? Wizard.WizardItemStatus.valid : Wizard.WizardItemStatus.warning;
                    if (!item.HasMultilingua)
                        item.Errors.Add(EditingErrors.EmptyMessage);
                    if (item.Empty > 0)
                        item.Errors.Add(EditingErrors.EmptyTranslations);
                    if (item.InvalidItems > 0)
                    {
                        item.Errors.Add(EditingErrors.InvalidTranslations);
                        item.Status = Wizard.WizardItemStatus.error;
                    }
                }
                else
                    item.Status = Wizard.WizardItemStatus.disabled;
                return item;
            }
            protected dtoPermissionStep GetPermissionStepInfo(TemplateDefinitionVersion version)
            {
                dtoPermissionStep item = new dtoPermissionStep(WizardTemplateStep.Permission);
                if (version != null)
                {
                    item.Count = (version.Permissions == null) ? 0 : version.Permissions.Where(p => p.Deleted == BaseStatusDeleted.None).Count();
                    if (item.Count == 0)
                        item.Status = Wizard.WizardItemStatus.none;
                    else
                    {
                        if (version.Permissions.Where(p => p.Deleted == BaseStatusDeleted.None && !p.Edit && !p.Clone && !p.ChangePermission && !p.See).Any())
                        {
                            item.NoPermissionsCount = version.Permissions.Where(p => p.Deleted == BaseStatusDeleted.None && !p.Edit && !p.Clone && !p.ChangePermission && !p.See).Count();
                            item.Errors.Add(EditingErrors.NoPermission);
                            item.Status = Wizard.WizardItemStatus.warning;
                        }
                        else
                            item.Status = Wizard.WizardItemStatus.valid;
                    }
                }
                else
                    item.Status = Wizard.WizardItemStatus.disabled;
                return item;
            }
        #endregion

         #region "Edit"
          
            public TemplateDefinition AddTemplate(String name, TemplateType type, dtoBaseTemplateOwner ownership, List<String> contentModules, List<dtoChannelConfigurator> items)
            {
                TemplateDefinition template = null;
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person !=null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID !=(int)UserTypeStandard.PublicUser){
                        template = new TemplateDefinition();
                        template.CreateMetaInfo(person, UC.IpAddress,UC.ProxyIpAddress);
                        template.Type=type;
                        template.IsEnabled=true;
                        template.OwnerInfo.Community= (ownership.IdCommunity>0) ? Manager.GetLiteCommunity(ownership.IdCommunity) : null;
                        template.OwnerInfo.IdModule = ownership.IdModule;
                        template.OwnerInfo.IsPortal = ownership.IsPortal;
                        template.OwnerInfo.ModuleCode = ownership.ModuleCode;
                        template.OwnerInfo.Type = ownership.Type;
                        template.OwnerInfo.Person = person;
                        switch(ownership.Type){
                            case OwnerType.Object:
                                template.OwnerInfo.ModuleObject = ModuleObject.CreateLongObject(ownership.IdObject, ownership.IdObjectType, ownership.IdObjectCommunity, Manager.GetModuleCode(ownership.IdObjectModule), ownership.IdObjectModule);
                                break;
                            case OwnerType.Module:
                                if (ownership.IdModule > 0 && string.IsNullOrEmpty(ownership.ModuleCode))
                                    ownership.ModuleCode = Manager.GetModuleCode(ownership.IdModule);
                                else if (ownership.IdModule < 1 && !string.IsNullOrEmpty(ownership.ModuleCode))
                                    ownership.IdModule = Manager.GetModuleID(ownership.ModuleCode);

                                break;
                        }
                        if (!String.IsNullOrEmpty(name))
                            template.Name= name;
                        else
                            template.Name= "template - " + (from t in Manager.GetIQ<TemplateDefinition>() select t.Id).Max()+1;
                        template.CurrentModulesContent = "";
                        Manager.SaveOrUpdate(template);
                        TemplateDefinitionVersion version = new TemplateDefinitionVersion();
                        version.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        version.DefaultTranslation.Name = template.Name;
                        version.DefaultTranslation.IsHtml = true;
                        version.HasShortText = (items != null && items.Where(i => i.Channel == NotificationChannel.Memo).Any());
                        version.OnlyShortText = (items != null && items.Count > 0 && !items.Where(i => i.Channel != NotificationChannel.Memo).Any());
                        version.Owner = person;
                        version.Status = TemplateStatus.Draft;
                        version.Template = template;
                        version.Number = 1;
                        Manager.SaveOrUpdate(version);
                        foreach(String code in contentModules){
                            TemplateModuleContent content = new TemplateModuleContent();
                            content.IsActive=true;
                            content.IdModule= Manager.GetModuleID(code);
                            content.ModuleCode=code;
                            content.Version=version;
                            Manager.SaveOrUpdate(content);
                            version.ModulesForContent.Add(content);
                            template.CurrentModulesContent = "#" + String.Join("#",contentModules.ToArray()) + "#";
                        }
                        foreach (dtoChannelConfigurator item in items)
                        {
                            ChannelSettings settings = new ChannelSettings();
                            settings.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            settings.IsEnabled = true;
                            settings.MailSettings = item.Settings.MailSettings;
                            settings.Channel = item.Settings.Channel;
                            settings.Version = version;
                            Manager.SaveOrUpdate(settings);
                            version.ChannelSettings.Add(settings);
                            if (settings.Channel ==  NotificationChannel.Mail)
                                version.DefaultTranslation.IsHtml = item.Settings.MailSettings.IsBodyHtml;
                        }
                        Manager.SaveOrUpdate(version);
                        template.LastVersion = version;
                        template.Versions.Add(version);
                        Manager.SaveOrUpdate(template);
                    }
                    Manager.Commit();
                }
                catch (Exception ex) {
                    template = null;
                    Manager.RollBack();
                }
                return template;
            }
            public TemplateDefinition AddTemplate(String name, TemplateType type, dtoBaseTemplateOwner ownership, List<String> contentModules, List<dtoTemplateTranslation> translations, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings)
            {
                TemplateDefinition template = null;
                try
                {
                    Manager.BeginTransaction();
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        template = new TemplateDefinition();
                        template.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        template.Type = type;
                        template.IsEnabled = true;
                        template.OwnerInfo.Community = (ownership.IdCommunity > 0) ? Manager.GetLiteCommunity(ownership.IdCommunity) : null;
                        template.OwnerInfo.IdModule = ownership.IdModule;
                        template.OwnerInfo.IsPortal = ownership.IsPortal;
                        template.OwnerInfo.ModuleCode = ownership.ModuleCode;
                        template.OwnerInfo.Type = ownership.Type;
                        template.OwnerInfo.Person = person;
                        switch (ownership.Type)
                        {
                            case OwnerType.Object:
                                template.OwnerInfo.ModuleObject = ModuleObject.CreateLongObject(ownership.IdObject, ownership.IdObjectType, ownership.IdObjectCommunity, Manager.GetModuleCode(ownership.IdObjectModule), ownership.IdObjectModule);
                                break;
                            case OwnerType.Module:
                                if (ownership.IdModule > 0 && string.IsNullOrEmpty(ownership.ModuleCode))
                                    ownership.ModuleCode = Manager.GetModuleCode(ownership.IdModule);
                                else if (ownership.IdModule < 1 && !string.IsNullOrEmpty(ownership.ModuleCode))
                                    ownership.IdModule = Manager.GetModuleID(ownership.ModuleCode);

                                break;
                        }
                        template.Name ="-";
                        template.CurrentModulesContent = "";
                        Manager.SaveOrUpdate(template);
                        if (!String.IsNullOrEmpty(name) && name.Contains("{0}"))
                            template.Name = String.Format(name, template.Id);
                        else
                            template.Name = "template - " + template.Id.ToString();

                        TemplateDefinitionVersion version = new TemplateDefinitionVersion();
                        version.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        version.DefaultTranslation.Name = template.Name;
                        version.DefaultTranslation.IsHtml = (settings != null) ? settings.IsBodyHtml : true;
                        version.HasShortText = false;
                        version.OnlyShortText = false;
                        version.Owner = person;
                        version.Status = TemplateStatus.Draft;
                        version.Template = template;
                        version.Number = 1;
                        if (translations.Where(tr=>tr.LanguageCode=="multi").Any()){
                            version.DefaultTranslation =  translations.Where(tr=>tr.LanguageCode=="multi").Select(tr=>tr.Translation).FirstOrDefault();
                        }
                        Manager.SaveOrUpdate(version);
                        foreach (dtoTemplateTranslation translation in translations.Where(tr=>tr.LanguageCode!="multi")) {
                            TemplateTranslation t = new TemplateTranslation();
                            t.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            t.IdLanguage = translation.IdLanguage;
                            t.LanguageCode = translation.LanguageCode;
                            t.LanguageName = translation.LanguageName;
                            t.Version = version;
                            t.Translation = translation.Translation;
                            Manager.SaveOrUpdate(t);
                            version.Translations.Add(t);
                        } 
                        foreach (String code in contentModules)
                        {
                            TemplateModuleContent content = new TemplateModuleContent();
                            content.IsActive = true;
                            content.IdModule = Manager.GetModuleID(code);
                            content.ModuleCode = code;
                            content.Version = version;
                            Manager.SaveOrUpdate(content);
                            version.ModulesForContent.Add(content);
                            template.CurrentModulesContent = "#" + String.Join("#", contentModules.ToArray()) + "#";
                        }
                        if (settings != null) {
                            ChannelSettings cSettings = new ChannelSettings();
                            cSettings.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            cSettings.IsEnabled = true;
                            cSettings.MailSettings = settings;
                            cSettings.Channel = NotificationChannel.Mail;
                            cSettings.Version = version;
                            Manager.SaveOrUpdate(cSettings);
                            version.ChannelSettings.Add(cSettings);
                        }
                        Manager.SaveOrUpdate(version);
                        template.LastVersion = version;
                        template.Versions.Add(version);
                        Manager.SaveOrUpdate(template);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    template = null;
                    Manager.RollBack();
                }
                return template;
            }
            public dtoChannelConfigurator GetDraftChannelConfigurator(NotificationChannel channel, TemplateType templateType, long idVersion = 0)
            {
                dtoChannelConfigurator config = new dtoChannelConfigurator();
                config.TemporaryId = System.Guid.NewGuid();
                //config.AvailableModes = GetAvailableNotificationModes(templateType, idVersion);
                config.IsEnabled = true;
                config.Settings = GetDraftChannelSetting(channel, templateType, idVersion);
                return config;
            }
            private dtoNotificationSettings GetDraftChannelSetting(NotificationChannel channel, TemplateType templateType, long idVersion = 0)
            {
                dtoNotificationSettings settings = null;
                List<NotificationChannel> channels = GetAvailableChannels(templateType, idVersion);
                if (channels.Contains(channel))
                {
                    settings = new dtoNotificationSettings() { IsEnabled = true, Channel = channel, IdVersion = idVersion };
                    switch (channel)
                    {
                        case NotificationChannel.Mail:

                            break;
                    }
                }
                return settings;
            }

            #region "Translations"
                public List<lm.Comol.Core.DomainModel.Languages.LanguageItem> GetInUseLanguageItems(long idVersion) {
                    return GetInUseLanguageItems(Manager.Get<TemplateDefinitionVersion>(idVersion));
                }
                public List<lm.Comol.Core.DomainModel.Languages.LanguageItem> GetInUseLanguageItems(TemplateDefinitionVersion version){
                    List<lm.Comol.Core.DomainModel.Languages.LanguageItem> items = new List<lm.Comol.Core.DomainModel.Languages.LanguageItem>();

                    try
                    {
                        List<Language> languages = Manager.GetAllLanguages().ToList();
                        if (version!=null && version.Translations != null)
                        {
                            items.Add(new lm.Comol.Core.DomainModel.Languages.LanguageItem() { Id = 0, Code = "multi", IsEnabled = true, IsMultiLanguage = true, Status = (version.DefaultTranslation.IsValid(!version.OnlyShortText, version.HasShortText, true)) ? lm.Comol.Core.DomainModel.Languages.ItemStatus.valid : lm.Comol.Core.DomainModel.Languages.ItemStatus.warning });
                            foreach (TemplateTranslation tr in version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None).ToList()) {
                                Language language = null;
                                lm.Comol.Core.DomainModel.Languages.LanguageItem item = new lm.Comol.Core.DomainModel.Languages.LanguageItem();
                                language= languages.Where(l=> (tr.IdLanguage>0 && tr.IdLanguage== l.Id) || (!(tr.IdLanguage>0) && tr.LanguageCode== l.Code)).FirstOrDefault();
                                if (language != null)
                                {
                                    item.Id = language.Id;
                                    item.Code = language.Code;
                                    item.Name = language.Name;
                                }
                                else {
                                    item.Id = tr.IdLanguage;
                                    item.Code = tr.LanguageCode;
                                    item.Name = tr.LanguageName;
                                }
                                item.ToolTip = item.Name;
                                item.Status = (tr.IsEmpty) ? DomainModel.Languages.ItemStatus.wrong : (tr.Translation.IsValid(!version.OnlyShortText, version.HasShortText, true)) ? DomainModel.Languages.ItemStatus.valid : DomainModel.Languages.ItemStatus.warning;
                                items.Add(item);
                            }
                        }
                        else
                            items.Add(new lm.Comol.Core.DomainModel.Languages.LanguageItem() { Id = 0, Code = "multi", IsEnabled = true, IsMultiLanguage = true, Status = DomainModel.Languages.ItemStatus.none });
                    }
                    catch(Exception ex){
                    
                    }

                    return items;
                }
                public List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> GetAvalableLanguages()
                {
                    List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> items = new List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem>();

                    try
                    {
                        List<Language> languages = Manager.GetAllLanguages().ToList();
                        items.AddRange(languages.Select(l => new lm.Comol.Core.DomainModel.Languages.BaseLanguageItem(l)).ToList());
                    }
                    catch (Exception ex)
                    {

                    }

                    return items;
                }
                public List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem > GetAvalableLanguage(TemplateDefinitionVersion version)
                {
                    List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> items = new List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem>();

                    try
                    {
                        List<Language> languages = Manager.GetAllLanguages().ToList();
                        items.AddRange(languages.Select(l => new lm.Comol.Core.DomainModel.Languages.BaseLanguageItem(l)).ToList());
                    }
                    catch(Exception ex){
                    
                    }

                    return items;
                }
                public dtoTemplateTranslation GetTranslation(long idVersion, Int32 idLanguage, String codeLanguage) {
                    dtoTemplateTranslation translation = new dtoTemplateTranslation();
                    try
                    {
                        translation = (from t in Manager.GetIQ<TemplateTranslation>()
                                       where t.Deleted == BaseStatusDeleted.None && t.Version.Id == idVersion && ((idLanguage>0 && t.IdLanguage==idLanguage) || (t.LanguageCode==codeLanguage))
                                       select t).Skip(0).Take(1).ToList().Select(t=>new dtoTemplateTranslation(t)).FirstOrDefault();
                        if (translation == null) {
                            TemplateDefinitionVersion v = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            translation = new dtoTemplateTranslation();
                            //translation.IdLanguage = idLanguage;
                            //translation.LanguageCode = codeLanguage;
                            translation.IdLanguage = 0;
                            translation.LanguageCode = "multi";
                            translation.IdVersion = idVersion;
                            if (v != null)
                                translation.Translation = v.DefaultTranslation.Copy();
                        }
                    }
                    catch (Exception ex) { 
                    
                    }
                    return translation;
                }
                public Dictionary<String, List<String>> GetInUserPlaceHolders(TemplateDefinitionVersion version, Dictionary<String, List<String>> placeHolders)
                {
                    Dictionary<String, List<String>> results = new Dictionary<String, List<String>>();
                    if (!version.DefaultTranslation.IsContentEmpty() || (version.Translations != null && version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && !t.IsEmpty).Any())) {
                        List<String> searchIn = new List<String>();
                        searchIn.Add(version.DefaultTranslation.Subject);
                        searchIn.Add(version.DefaultTranslation.Body);
                        if (version.Translations != null)
                        {
                            searchIn.AddRange(version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && !String.IsNullOrEmpty(t.Translation.Subject)).Select(t => t.Translation.Subject).ToList());
                            searchIn.AddRange(version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None && !String.IsNullOrEmpty(t.Translation.Body)).Select(t => t.Translation.Body).ToList());
                        }
                        foreach (var item in placeHolders) {
                            List<String> items = item.Value.Where(p => searchIn.Where(s => !String.IsNullOrEmpty(s) && s.ToLower().Contains(p.ToLower())).Any()).ToList();
                            if (items.Any())
                                results.Add(item.Key, items);
                        }
                    }
                    return results;
                }
                private void RemoveTagsFromTranslations(TemplateDefinitionVersion version, Dictionary<String, List<String>> tagsToRemove = null)
                {
                    if (tagsToRemove != null && tagsToRemove.Count > 0 && version != null )
                    {
                        version.DefaultTranslation.RemoveTags(tagsToRemove);
                        version.Translations.Where(t => t.Deleted == BaseStatusDeleted.None).ToList().ForEach(t => t.Translation.RemoveTags(tagsToRemove));
                    }
                }

                public TemplateTranslation SaveTranslation(long idVersion, dtoTemplateTranslation dto)
                {
                    TemplateTranslation translation = null;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                        if (p != null && p.TypeID != (int)UserTypeStandard.Guest && version!=null)
                        {
                            translation = Manager.Get<TemplateTranslation>(dto.Id);
                            if (translation == null)
                                translation = (from t in Manager.GetIQ<TemplateTranslation>() where t.Version!=null && t.Version== version && t.IdLanguage == dto.IdLanguage select t).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (translation == null)
                            {
                                translation = new TemplateTranslation();
                                translation.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                translation.Version = version;
                                translation.IdLanguage = dto.IdLanguage;
                                translation.LanguageCode = dto.LanguageCode;
                                translation.LanguageName = dto.LanguageName;
                            }
                            else
                            {
                                translation.Deleted = BaseStatusDeleted.None;
                                translation.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            }
                            translation.Translation = dto.Translation;
                            Manager.SaveOrUpdate(translation);
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                        translation = null;
                    }
                    return translation;
                }
                public Boolean SaveDefaultTranslation(long idVersion, dtoTemplateTranslation dto)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                        if (p != null && p.TypeID != (int)UserTypeStandard.Guest && version != null)
                        {
                            version.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            version.DefaultTranslation = dto.Translation;
                            Manager.SaveOrUpdate(version);
                            result = true;
                        }
                        Manager.Commit();

                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                public Boolean VirtualDeleteTranslation(long idVersion, long idTranslation)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        if (p != null && p.TypeID != (int)UserTypeStandard.Guest) {
                            TemplateTranslation translation = Manager.Get<TemplateTranslation>(idTranslation);
                            if (translation != null && (translation.Version==null || translation.Version.Id==idVersion))
                            {
                                translation.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(translation);
                            }
                            result = true;
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                    }
                    return result;
                }
            #endregion
        
            #region "StandardSettings"
                public TemplateDefinitionVersion SaveSettings(long idVersion,String name, List<String> contentmodules, List<dtoChannelConfigurator> items, TemplateStatus status, Dictionary<String, List<String>> tagsToRemove = null)
                {
                    TemplateDefinitionVersion version = null;
                    try
                    {
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        if (p != null && p.TypeID != (int)UserTypeStandard.Guest ) {
                            Manager.BeginTransaction();
                            version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            if (version != null) {
                                version.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                if (!String.IsNullOrEmpty(name)) {
                                    version.DefaultTranslation.Name = name;
                                    version.Template.Name = name;
                                }
                                SaveModuleContentSettings(version, contentmodules, tagsToRemove);
                                SaveChannelSettings(p, version, items);
                                version.Status = status;
                                Manager.SaveOrUpdate(version);
                                if (status == TemplateStatus.Active) {
                                    if (version.Template.LastVersion != null && version.Template.LastVersion.Id != version.Id) {
                                        if (version.Template.LastVersion.Permissions != null)
                                        {
                                            version.Template.LastVersion.Permissions.ToList().ForEach(pm => pm.ToApply = false);
                                        }
                                        if (version.Template.LastVersion.Status == TemplateStatus.Active) {
                                            version.Template.LastVersion.Status = TemplateStatus.Replaced;
                                            Manager.SaveOrUpdate(version.Template.LastVersion);
                                        }
                                    }
                                    if (contentmodules.Any())
                                        version.Template.CurrentModulesContent = "#" + String.Join("#", contentmodules.ToArray()) + "#";
                                    else 
                                        version.Template.CurrentModulesContent="";
                                    version.Template.LastVersion = version;
                                    if (version.Permissions != null) {
                                        version.Permissions.Where(pm => pm.Deleted == BaseStatusDeleted.None).ToList().ForEach(pm => pm.ToApply = true);
                                    }
                                    Manager.SaveOrUpdate(version.Template);
                                }

                                Manager.SaveOrUpdate(version);
                            }
                            Manager.Commit();
                        }
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                        version = null;
                    }
                    return version;
                }
            #endregion

            #region "ContentSettings"
                /// <summary>
                /// 
                /// </summary>
                /// <param name="version"></param>
                /// <param name="contentmodules"></param>
                /// <param name="removeTags"></param>
                /// <returns></returns>
                public Boolean SaveModuleContentSettings(TemplateDefinitionVersion version, List<String> contentmodules,Dictionary<String, List<String>> tagsToRemove = null)
                {
                    Boolean isInTransacion = Manager.IsInTransaction();
                    Boolean result = false;
                    try
                    {
                        
                        if (!isInTransacion)
                            Manager.BeginTransaction();
                        Person p = Manager.GetPerson(UC.CurrentUserID);
                        if (p != null && p.TypeID != (int)UserTypeStandard.Guest && version != null)
                        {
                            List<TemplateModuleContent> contents = version.ModulesForContent.ToList();
                            contents.Where(c => c.Deleted == BaseStatusDeleted.None && !contentmodules.Contains(c.ModuleCode)).ToList().ForEach(c => c.Deleted = BaseStatusDeleted.Manual);
                            contents.Where(c => c.Deleted != BaseStatusDeleted.None && contentmodules.Contains(c.ModuleCode)).ToList().ForEach(c => c.Deleted = BaseStatusDeleted.None);
                            Manager.SaveOrUpdateList(contents);

                            foreach (String code in contentmodules.Where(m=> !contents.Where(c=>c.ModuleCode==m).Any()).ToList()) {
                                TemplateModuleContent content = new TemplateModuleContent();
                                content.Version = version;
                                content.IsActive = true;
                                content.ModuleCode = code;
                                content.IdModule = Manager.GetModuleID(code);
                                Manager.SaveOrUpdate(content);
                            }

                            RemoveTagsFromTranslations(version, tagsToRemove);

                            result = true;
                        }
                        if (!isInTransacion)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransacion)
                            Manager.RollBack();
                        result=false;
                    }
                    return result;
                }

                
                public List<String> GetVersionModuleContentCodes(long idVersion) {
                    List<String> codes = new List<String>();
                    try
                    {
                        codes = (from c in Manager.GetIQ<TemplateModuleContent>() where c.Version.Id == idVersion && c.Deleted == BaseStatusDeleted.None select c.ModuleCode).ToList();
                    }
                    catch (Exception ex) { 
                    
                    }
                    return codes;
                }
            #endregion

            #region "Channel Settings"
                public List<ChannelSettings> SaveChannelSettings(long idVersion, List<dtoChannelConfigurator> dtoSettings)
                {
                    Boolean isInTransacion = Manager.IsInTransaction();
                    List<ChannelSettings> items = new List<ChannelSettings>();
                    try
                    {
                        if (!isInTransacion)
                            Manager.BeginTransaction();
                            litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            items = SaveChannelSettings(p, version, dtoSettings);
                        if (!isInTransacion)
                            Manager.Commit();
                    }
                    catch (Exception ex) {
                        if (!isInTransacion)
                            Manager.RollBack();
                        items = null;
                    }
                    return items;
                }
                public List<ChannelSettings> SaveChannelSettings(litePerson p, TemplateDefinitionVersion version, List<dtoChannelConfigurator> dtoSettings)
                {
                    Boolean isInTransacion = Manager.IsInTransaction();
                    List<ChannelSettings> items = new List<ChannelSettings>();
                    try
                    {
                        if (p != null && p.TypeID != (int)UserTypeStandard.Guest && version !=null)
                        {
                            if (!isInTransacion)
                                Manager.BeginTransaction();
                            List<ChannelSettings> settings = version.ChannelSettings.ToList();
                            List<long> idSettings = dtoSettings.Where(s=>s.Deleted!= BaseStatusDeleted.None && s.IdSettings>0).Select(s=>s.IdSettings).ToList();
                            settings.Where(c => c.Deleted == BaseStatusDeleted.None && idSettings.Contains(c.Id)).ToList().ForEach(c => c.SetDeleteMetaInfo(p,UC.IpAddress,UC.ProxyIpAddress));

                            foreach (dtoChannelConfigurator item in dtoSettings.Where(s => s.Deleted == BaseStatusDeleted.None).ToList())
                            {
                                ChannelSettings notification = Manager.Get<ChannelSettings>(item.IdSettings);
                                if (notification == null)
                                {
                                    notification.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                    notification.IsEnabled = true;
                                    notification.MailSettings = item.Settings.MailSettings;
                                    notification.Channel = item.Channel;
                                    notification.Version = version;
                                    Manager.SaveOrUpdate(notification);
                                    version.ChannelSettings.Add(notification);
                                    items.Add(notification);
                                }
                                else {
                                    notification.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                    notification.Deleted = BaseStatusDeleted.None;
                                    notification.IsEnabled = true;
                                    notification.MailSettings = item.Settings.MailSettings;
                                    notification.Channel = item.Channel;
                                    Manager.SaveOrUpdate(notification);
                                    items.Add(notification);
                                }
                            }
                           // Manager.SaveOrUpdateList(settings);
                            Manager.SaveOrUpdate(version);
                            if (!isInTransacion)
                                Manager.Commit();
                        }
                    }
                    catch (Exception ex) {
                        if (!isInTransacion)
                            Manager.RollBack();
                        items = null;
                    }
                    return items;
                }

                public Boolean VirtualDeleteChannelSetting(long idSettings)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        if (p != null && p.TypeID != (int)UserTypeStandard.Guest) {
                            ChannelSettings settings = Manager.Get<ChannelSettings>(idSettings);
                            if (settings != null) {
                                settings.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(settings);
                            }
                            result = true;
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                    }
                    return result;
                }
                public List<dtoChannelConfigurator> GetChannelSettings(long idVersion)
                {
                    return GetChannelSettings(Manager.Get<TemplateDefinitionVersion>(idVersion));
                }
                public List<dtoChannelConfigurator> GetChannelSettings(TemplateDefinitionVersion version)
                {
                    if (version == null || (version != null && version.ChannelSettings == null))
                        return new List<dtoChannelConfigurator>();
                    else {
                        List<dtoNotificationSettings> settings = (from n in version.ChannelSettings.Where(s => s.Deleted == BaseStatusDeleted.None).ToList()
                                                                  select new dtoNotificationSettings(n)).ToList();

                        return settings.Select(s => new dtoChannelConfigurator
                        {
                            Settings = s,
                            //AvailableModes = GetAvailableNotificationModes(version.Template.Type, version)
                            AllowDelete= !(from n in Manager.GetIQ<CommonNotificationSettings>()
                                          where n.Deleted== BaseStatusDeleted.None && n.Template != null && n.Template.Id == version.Template.Id 
                                          && ((n.Version !=null && n.Version.Id == s.IdVersion) || (n.Version == null && version.Status== TemplateStatus.Active))
                                          select n).Any()
                        }).ToList();
                    }
                }
                public List<NotificationChannel> GetAvailableChannels(TemplateType templateType, long idVersion = 0)
                {
                    return GetAvailableChannels(templateType, (idVersion <= 0) ? null : Manager.Get<TemplateDefinitionVersion>(idVersion));
                }
                public List<NotificationChannel> GetAvailableChannels(TemplateType templateType, TemplateDefinitionVersion version)
                {
                    List<NotificationChannel> types = new List<NotificationChannel>();

                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    switch (templateType)
                    {
                        case TemplateType.System:
                            if (IsAdministrativeUser(person))
                                types.Add(NotificationChannel.Mail);
                            break;
                        case TemplateType.Module:
                            if (IsAdministrativeUser(person))
                                types.Add(NotificationChannel.Mail);
                            break;
                        case TemplateType.User:
                            types.Add(NotificationChannel.Mail);
                            break;
                    }
                    if (version != null && version.ChannelSettings != null)
                    {
                        types = types.Where(t => !version.ChannelSettings.Where(n => n.Deleted == BaseStatusDeleted.None && n.Channel == t).Any()).ToList();
                    }
                    return types;
                }
                //public List<NotificationMode> GetAvailableNotificationModes(TemplateType type, long idVersion = 0) {
                //    return GetAvailableNotificationModes(type, (idVersion <= 0) ? null : Manager.Get<TemplateDefinitionVersion>(idVersion));
                //}
                //public List<NotificationMode> GetAvailableNotificationModes(TemplateType type, TemplateDefinitionVersion version)
                //{
                //    List<NotificationMode> modes = new List<NotificationMode>();


                //    Person person = Manager.GetPerson(UC.CurrentUserID);
                //    //if (version!=null )
                //    //{
                //    //    TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                //    //}
                //    //else
                //    //{
                //        switch (type)
                //        {
                //            case TemplateType.System:
                //                if (IsAdministrativeUser(person))
                //                {
                //                    modes.Add(NotificationMode.Manual);
                //                    modes.Add(NotificationMode.Automatic);
                //                    // modes.Add(NotificationMode.Scheduling);
                //                }
                //                break;
                //            case TemplateType.Module:
                //                if (IsAdministrativeUser(person))
                //                {
                //                    modes.Add(NotificationMode.Manual);
                //                    modes.Add(NotificationMode.Automatic);
                //                    // modes.Add(NotificationMode.Scheduling);
                //                }
                //                break;
                //            case TemplateType.User:
                //                modes.Add(NotificationMode.Manual);
                //                break;
                //        }
                //    //}

                //    return modes;
                //}
            #endregion

            #region "Common edit"

                public TemplateDefinitionVersion GetLastActiveVersion(long idTemplate)
                {
                    TemplateDefinitionVersion version = null;
                    try
                    {
                        version = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                   where v.Deleted == BaseStatusDeleted.None && v.Status == TemplateStatus.Active && v.Template != null && v.Template.Id == idTemplate
                                   orderby v.Id  descending
                                   select v).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                    }
                    return version;
                }
                public TemplateDefinitionVersion GetVersion(long idVersion){
                    TemplateDefinitionVersion version = null;
                    try
                    {
                        version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                    }
                    catch (Exception ex)
                    {
                    }
                    return version;
                }
                public TemplateDefinitionVersion GetVersion(long idTemplate,long idVersion)
                {
                    TemplateDefinitionVersion version = null;
                    try
                    {
                        if (idVersion>0)
                            version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                        else
                            version = (from v in Manager.GetIQ<TemplateDefinitionVersion>() where v.Deleted== BaseStatusDeleted.None && v.Status== TemplateStatus.Active && v.Template !=null && v.Template.Id == idTemplate select v).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                    }
                    return version;
                }
            #endregion

            #region "Assignments"
                public List<dtoTemplateAssignment> GetPermissionAssignments(TemplateDefinitionVersion version, Dictionary<String, String> translatedModules,  Dictionary<Int32, String> translatedProfileTypes, Dictionary<Int32, String> translatedRoles)
                {
                    List<dtoTemplateAssignment> assignments = new List<dtoTemplateAssignment>();
                    try
                    {
                        if (version != null && version.Template !=null )
                        {
                            #region "Portal Assignment"
                            if (version.IsForPortal())
                            {
                                dtoPortalAssignment assignment = new dtoPortalAssignment() { IsDefault = true, Id = 0 };
                                assignment.ProfileTypes = (from pa in Manager.GetIQ<VersionProfileTypePermission>() where pa.Deleted== BaseStatusDeleted.None && pa.Version.Id== version.Id 
                                                           select pa).ToList().Select(
                                                           pa => new dtoProfileTypeAssignment(pa, translatedProfileTypes)).ToList();
                                assignments.Add(assignment);
                            }
                            #endregion

                            #region "Community/Role Assignments"
                            // GET role assignments
                            List<dtoRoleAssignment> rAssignments = (from rp in Manager.GetIQ<VersionRolePermission>()
                                                                    where rp.Version.Id == version.Id && rp.Deleted== BaseStatusDeleted.None  && rp.Community != null && rp.AssignedTo != null
                                                                    select rp).ToList().Select(rp =>
                                                                        new dtoRoleAssignment(rp)
                                                                        {
                                                                            IdCommunity = rp.Community.Id,
                                                                            IdRole = rp.AssignedTo.Id,
                                                                            Deleted = BaseStatusDeleted.None,
                                                                            Id = rp.Id,
                                                                            DisplayName = (translatedRoles.ContainsKey(rp.AssignedTo.Id) ? translatedRoles[rp.AssignedTo.Id] : rp.AssignedTo.Name)
                                                                        }).ToList();

                            // Add communities
                            List<dtoCommunityAssignment> cAssignments = (from ca in Manager.GetIQ<VersionCommunityPermission>()
                                                                             where ca.Version.Id == version.Id && ca.Deleted== BaseStatusDeleted.None && ca.AssignedTo != null
                                                                             select ca).ToList().Select(ca =>
                                                                        new dtoCommunityAssignment(ca)
                                                                        {
                                                                            IdCommunity = (ca.AssignedTo == null) ? -1 : ca.AssignedTo.Id,
                                                                            IsUnknown = (ca.AssignedTo == null),
                                                                            DisplayName = (ca.AssignedTo == null) ? "" : ca.AssignedTo.Name,
                                                                            Deleted = BaseStatusDeleted.None,
                                                                            Id = ca.Id
                                                                        }).ToList();

                            if (rAssignments.Any())
                            {
                                foreach (var r in rAssignments.GroupBy(ra => ra.IdCommunity).ToList())
                                {
                                    dtoCommunityAssignment communityAssignment = cAssignments.Where(c => c.IdCommunity == r.Key).FirstOrDefault();
                                    if (communityAssignment != null)
                                        communityAssignment.Roles = r.OrderBy(ra => ra.DisplayName).ThenBy(ra => ra.IdRole).ToList();
                                    else if (Manager.GetCommunity(r.Key) != null)
                                    {
                                        assignments.Add(new dtoCommunityAssignment()
                                        {
                                            Id = -1,
                                            IdCommunity = r.Key,
                                            Roles = r.OrderBy(ra => ra.DisplayName).ThenBy(ra => ra.IdRole).ToList(),
                                            IsDefault = (version.Template.OwnerInfo.Community != null && r.Key == version.Template.OwnerInfo.Community.Id),
                                            DisplayName = Manager.GetCommunity(r.Key).Name
                                        });
                                    }
                                }
                            }
                            foreach (dtoCommunityAssignment cAssignment in cAssignments.OrderBy(c => c.DisplayName).ThenBy(c => c.IdCommunity))
                            {
                                cAssignment.IsDefault = (version.Template.OwnerInfo.Community != null && cAssignment.IdCommunity == version.Template.OwnerInfo.Community.Id);
                                cAssignment.ForAllUsers = (cAssignment.Roles == null || !cAssignment.Roles.Any());
                                assignments.Add(cAssignment);
                            }
                            #endregion
                           
                            if (!version.IsForPortal() && version.Template.OwnerInfo.Community != null && !assignments.Where(a => a.Type == PermissionType.Community).Select(a => (dtoCommunityAssignment)a).Where(a => !a.IsEmpty && a.IsDefault).Any())
                            {
                                assignments.Add(new dtoCommunityAssignment()
                                {
                                    IdCommunity = version.Template.OwnerInfo.Community.Id,
                                    IsUnknown = false,
                                    DisplayName = version.Template.OwnerInfo.Community.Name,
                                    Deleted = BaseStatusDeleted.None,
                                    Id = -1,
                                    ForAllUsers = false,
                                    IsDefault = true
                                });
                            }

                            // GET modules assignments
                            //List<dtoModuleAssignment> mAssignments = (from mp in version.AvailableModulesPermission() select mp).ToList().Select(mp =>
                            //                                            new dtoModuleAssignment(mp)
                            //                                            {
                            //                                                IdCommunity = (mp.Community != null) ? mp.Community.Id : -1,
                            //                                                Deleted = BaseStatusDeleted.None,
                            //                                                Id = mp.Id,
                            //                                                DisplayName = (translatedModules.ContainsKey(mp.ModuleCode) ? translatedModules[mp.ModuleCode] : mp.ModuleCode)
                            //                                            }).ToList();

                            //foreach (var m in mAssignments.GroupBy(mp => mp.IdCommunity).ToList())
                            //{
                            //    dtoCommunityAssignment communityAssignment = cAssignments.Where(c => c.IdCommunity == m.Key).FirstOrDefault();
                            //    if (communityAssignment != null)
                            //        communityAssignment.Modules = m.OrderBy(ra => ra.DisplayName).ThenBy(ra => ra.IdModule).ToList();
                            //    else if (Manager.GetCommunity(m.Key) != null)
                            //    {
                            //        assignments.Add(new dtoCommunityAssignment()
                            //        {
                            //            Id = -1,
                            //            IdCommunity = m.Key,
                            //            Modules = m.OrderBy(ra => ra.DisplayName).ThenBy(ra => ra.IdModule).ToList(),
                            //            IsDefault = (version.Template.OwnerInfo.Community != null && m.Key == version.Template.OwnerInfo.Community.Id),
                            //            DisplayName = Manager.GetCommunity(m.Key).Name
                            //        });
                            //    }
                            //}

                            (from pa in Manager.GetIQ<VersionPersonPermission>()
                                where pa.Version.Id == version.Id && pa.Deleted== BaseStatusDeleted.None && pa.AssignedTo != null select pa).ToList().Select(pa=> new dtoPersonAssignment(pa)).ToList().ForEach(pa => assignments.Add(pa));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return assignments;
                }
                #region "Default Assignments"
                    public dtoTemplateAssignment GetDefaultAssignmentSettings(long idVersion)
                    {
                        dtoTemplateAssignment settings = null;
                        try
                        {
                            Manager.BeginTransaction();
                            litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            if (p != null && version != null)
                            {
                                RemoveVersionPermissions(p, version.Id);
                                settings = new dtoPortalAssignment() { IsDefault = true };
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                            settings = null;
                        }
                        return settings;
                    }
                    //public dtoTemplateAssignment GetDefaultAssignmentSettings(long idVersion, Dictionary<String, String> translatedModules, Dictionary<Int32, String> translatedRoles, Int32 idModule, String moduleCode, long permissions)
                    //{
                    //    dtoTemplateAssignment settings = null;
                    //    try
                    //    {
                    //        Manager.BeginTransaction();
                    //        Person p = Manager.GetPerson(UC.CurrentUserID);
                    //        TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                    //        if (p != null && version != null)
                    //        {
                    //            RemoveVersionPermissions(p, version.Id );
                    //            settings = AddDefaultCallAssignment(p, version,translatedModules,translatedRoles, idModule, moduleCode, permissions);
                    //        }
                    //        Manager.Commit();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        if (Manager.IsInTransaction())
                    //            Manager.RollBack();
                    //        settings = null;
                    //    }
                    //    return settings;
                    //}
                    //private dtoTemplateAssignment AddDefaultCallAssignment(Person p, TemplateDefinitionVersion version, Dictionary<String, String> translatedModules, Dictionary<Int32 , String> translatedRoles, Int32 idModule, String moduleCode, long permissions)
                    //{
                    //    if (version.IsForPortal())
                    //        return new dtoPortalAssignment() { IsDefault = true };
                    //    else if (version.Template != null && version.Template.OwnerInfo.Community != null)
                    //    {
                    //        VersionRolePermission assignment = (from a in Manager.GetIQ<VersionRolePermission>()
                    //                                                      where a.Version.Id == version.Id && a.AssignedTo != null && a.Community!=null  && a.Community.Id == version.Template.OwnerInfo.Community.Id
                    //                                                      && a.AssignedTo.Id== 1
                    //                                                      select a).Skip(0).Take(1).ToList().FirstOrDefault();
                    //        if (assignment == null)
                    //        {
                    //            assignment = new VersionRolePermission();
                    //            assignment.Deleted = BaseStatusDeleted.None;
                    //            assignment.AssignedTo = Manager.Get<Role>(1);
                    //            assignment.Community= version.Template.OwnerInfo.Community;
                    //            assignment.Version = version;
                    //            assignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    //            assignment.See = true;
                    //            assignment.Clone = true;
                    //            Manager.SaveOrUpdate(assignment);
                    //        }
                    //        else if (assignment.Deleted != BaseStatusDeleted.None)
                    //        {
                    //            assignment.RecoverMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    //            assignment.See = true;
                    //            assignment.Clone = true;
                    //            assignment.ChangePermission = false;
                    //            assignment.Edit = false;
                    //            Manager.SaveOrUpdate(assignment);
                    //        }
                    //        if (assignment != null)
                    //        {
                               
                    //            dtoCommunityAssignment item = new dtoCommunityAssignment() { Id = assignment.Id, IdCommunity = (assignment.Community == null) ? -1 : assignment.Community.Id, IdVersion = version.Id, IsDefault = true, DisplayName = (assignment.Community == null) ? "" : assignment.Community.Name };
                    //            item.Roles.Add(new dtoRoleAssignment(assignment, translatedRoles));
                    //            return item;
                    //        }
                    //        //VersionModulePermission assignment = (from a in Manager.GetIQ<VersionModulePermission>()
                    //        //                                      where a.Version.Id == version.Id && a.ModuleCode == moduleCode
                    //        //                                              select a).Skip(0).Take(1).ToList().FirstOrDefault();
                    //        //if (assignment == null)
                    //        //{
                    //        //    assignment = new VersionModulePermission();
                    //        //    assignment.Deleted = BaseStatusDeleted.None;
                    //        //    assignment.IdModule = idModule;
                    //        //    assignment.ModuleCode = moduleCode;
                    //        //    assignment.ModulePermission = permissions;
                    //        //    assignment.Version = version;
                    //        //    assignment.ForPortal = version.IsForPortal();
                    //        //    if (!assignment.ForPortal)
                    //        //        assignment.Community = version.Template.OwnerInfo.Community;
                    //        //    assignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    //        //    Manager.SaveOrUpdate(assignment);
                    //        //}
                    //        //else if (assignment.Deleted != BaseStatusDeleted.None)
                    //        //{
                    //        //    assignment.RecoverMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    //        //    assignment.ModulePermission = permissions;
                    //        //    Manager.SaveOrUpdate(assignment);
                    //        //}
                    //        //if (assignment != null)
                    //        //{
                    //        //    dtoCommunityAssignment item =  new dtoCommunityAssignment() { Id = assignment.Id, IdCommunity = (assignment.Community == null) ? -1 : assignment.Community.Id, IdVersion = version.Id, IsDefault = true, DisplayName = (assignment.Community == null) ? "" : assignment.Community.Name };
                    //        //    //item.Modules.Add(new dtoModuleAssignment(assignment) { DisplayName = (translatedModules.ContainsKey(assignment.ModuleCode) ? translatedModules[assignment.ModuleCode] : assignment.ModuleCode) });
                    //        //    return item;
                    //        //}
                    //    }
                    //    return null;
                    //}
                #endregion
                #region "Remove Assignments"
                    private void RemoveVersionPermissions(litePerson p, long idVersion)
                    {
                        List<VersionPermission> assignments = (from a in Manager.GetIQ<VersionPermission>()
                                                               where a.Version.Id == idVersion && a.Deleted == BaseStatusDeleted.None
                                                                    select a).ToList();
                        foreach (VersionPermission assignment in assignments)
                        {
                            assignment.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        if (assignments.Any())
                            Manager.SaveOrUpdateList(assignments);

                    }
                    private void RemoveVersionProfileTypePermissions(litePerson p, long idVersion)
                    {
                        List<VersionProfileTypePermission> assignments = (from a in Manager.GetIQ<VersionProfileTypePermission>()
                                                                              where a.Version.Id == idVersion && a.Deleted == BaseStatusDeleted.None
                                                                              select a).ToList();
                        foreach (VersionProfileTypePermission assignment in assignments)
                        {
                            assignment.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        if (assignments.Any())
                            Manager.SaveOrUpdateList(assignments);

                    }
                    private void RemoveVersionRolePermissions(litePerson p, long idVersion)
                    {
                        List<VersionRolePermission> assignments = (from a in Manager.GetIQ<VersionRolePermission>()
                                                                   where a.Version.Id == idVersion && a.Deleted == BaseStatusDeleted.None
                                                                        select a).ToList();
                        foreach (VersionRolePermission assignment in assignments)
                        {
                            assignment.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        if (assignments.Any())
                            Manager.SaveOrUpdateList(assignments);

                    }
                #endregion
                #region "Portal Assignments"
                    public dtoPortalAssignment GetPortalAssignment(long idVersion, Dictionary<Int32, String> translatedProfileTypes)
                    {
                        dtoPortalAssignment assignment = null;
                        try
                        {
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            if (version != null)
                            {
                                assignment = new dtoPortalAssignment() { IsDefault = true, Id = 0 };
                                assignment.ProfileTypes = (from pa in Manager.GetIQ<VersionProfileTypePermission>()
                                                           where pa.Version.Id == idVersion && pa.Deleted == BaseStatusDeleted.None
                                                           select pa).ToList().Select(pa => new dtoProfileTypeAssignment(pa, translatedProfileTypes)).ToList();
                                assignment.ProfileTypes = assignment.ProfileTypes.OrderBy(p => p.DisplayName).ThenBy(p => p.Id).ToList();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return assignment;
                    }
                    public Boolean SavePortalPermissions(long idVersion, List<dtoProfileTypeAssignment> items)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (version != null && person != null)
                            {
                                SaveProfileTypePermissions(person, version, items);
                            }
                            Manager.Commit();
                            result = (version != null);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    private void SaveProfileTypePermissions(litePerson person, TemplateDefinitionVersion version, List<dtoProfileTypeAssignment> assignments)
                    {
                        List<Int32> idProfiles = assignments.Select(i => i.IdPersonType).ToList();
                        List<VersionProfileTypePermission> pAssignments = (from a in Manager.GetIQ<VersionProfileTypePermission>()
                                                                           where a.Version.Id == version.Id 
                                                                           select a).ToList();
                        // Delete removed profile types
                        if (pAssignments.Where(p => !idProfiles.Contains(p.AssignedTo) && p.Deleted == BaseStatusDeleted.None).Any())
                        {
                            foreach (VersionProfileTypePermission assignment in pAssignments.Where(p => !idProfiles.Contains(p.AssignedTo) && p.Deleted == BaseStatusDeleted.None))
                            {
                                assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                        }
                        foreach (dtoProfileTypeAssignment item in assignments)
                        {
                            VersionProfileTypePermission pa = pAssignments.Where(p => p.AssignedTo == item.IdPersonType).FirstOrDefault();
                            if (pa == null)
                            {
                                pa = new VersionProfileTypePermission();
                                pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                pa.Version = version;
                                pa.Template = version.Template;
                                pa.AssignedTo = item.IdPersonType;

                            }
                            else if (pa.Deleted != BaseStatusDeleted.None)
                            {
                                pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                            pa.ChangePermission = item.ChangePermission;
                            pa.Clone = item.Clone;
                            pa.Edit = item.Edit;
                            pa.See = item.Use;
                            pa.ToApply = (version.Id == version.Template.LastVersion.Id);
                            Manager.SaveOrUpdate(pa);
                            item.Id = pa.Id;
                        }
                    }
                #endregion

                #region "Community Assignments"
                    public dtoCommunityAssignment GetCommunityAssignments(long idVersion, Int32 idCommunity, long idAssignment, Dictionary<String, String> translatedModules, Dictionary<Int32, String> translatedRoles)
                    {
                        dtoCommunityAssignment assignment = null;
                        try
                        {
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            if (version != null && version.Template !=null)
                            {
                                assignment = new dtoCommunityAssignment();
                                VersionCommunityPermission cAssignment = Manager.Get<VersionCommunityPermission>(idAssignment);
                                if (cAssignment != null)
                                {
                                    assignment.ForAllUsers = true;
                                    assignment.IsDefault = (version.Template.OwnerInfo.Community != null && cAssignment.AssignedTo != null && version.Template.OwnerInfo.Community.Id == cAssignment.AssignedTo.Id);
                                }
                                else
                                {
                                    assignment.IsDefault = (version.Template.OwnerInfo.Community != null && version.Template.OwnerInfo.Community.Id == idCommunity);
                                    assignment.Roles = (from pa in Manager.GetIQ<VersionRolePermission>()
                                                        where pa.Version.Id == idVersion && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo != null && pa.Community !=null && pa.Community.Id == idCommunity  
                                                        select pa).ToList().Select(
                                                          pa => new dtoRoleAssignment(pa, translatedRoles)).ToList();
                                }
                                //assignment.Modules = (from pa in Manager.GetIQ<VersionModulePermission>()
                                //                      where pa.Version.Id == idVersion && pa.Deleted == BaseStatusDeleted.None && pa.Community.Id == idCommunity 
                                //                    select pa).ToList().Select(
                                //                          pa => new dtoModuleAssignment(pa, translatedModules)).ToList();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return assignment;
                    }
                    public Boolean SaveCommunityPermissions(long idVersion, Int32 idCommunity, List<dtoRoleAssignment> assignments)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                            if (version != null && person != null && community != null)
                            {
                                List<VersionCommunityPermission> cAssignments = (from a in Manager.GetIQ<VersionCommunityPermission>()
                                                                                 where a.Version.Id == idVersion && a.AssignedTo != null && a.AssignedTo.Id == idCommunity
                                                                                      select a).ToList();
                                foreach (VersionCommunityPermission assignment in cAssignments)
                                {
                                    assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                                SaveCommunityRolePermissions(version, person, community, assignments);
                            }
                            Manager.Commit();
                            result = (version != null);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    private void SaveCommunityRolePermissions(TemplateDefinitionVersion version, litePerson person, liteCommunity community, List<dtoRoleAssignment> assignments)
                    {
                        List<Int32> roles = assignments.Select(a => a.IdRole).ToList();
                        List<VersionRolePermission> rAssignments = (from a in Manager.GetIQ<VersionRolePermission>()
                                                                    where a.Version.Id == version.Id && a.AssignedTo != null && a.Community != null && a.Community.Id == community.Id
                                                                         select a).ToList();
                        // Delete removed profile types
                        if (rAssignments.Where(p => !roles.Contains(p.AssignedTo.Id) && p.Deleted== BaseStatusDeleted.None).Any())
                        {
                            foreach (VersionRolePermission assignment in rAssignments.Where(p => !roles.Contains(p.AssignedTo.Id) && p.Deleted == BaseStatusDeleted.None))
                            {
                                assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            }
                        }
                        foreach (dtoRoleAssignment assignment in assignments)
                        {
                            VersionRolePermission pa = rAssignments.Where(p => p.AssignedTo.Id == assignment.IdRole).FirstOrDefault();
                            if (pa == null)
                            {
                                pa = new VersionRolePermission();
                                pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                pa.Version = version;
                                pa.Template = version.Template;
                                pa.AssignedTo = Manager.Get<Role>(assignment.IdRole);
                                pa.Community = community;
                            }
                            else if (pa.Deleted != BaseStatusDeleted.None)
                                pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            pa.See = assignment.Use;
                            pa.Edit = assignment.Edit;
                            pa.ChangePermission = assignment.ChangePermission;
                            pa.Clone = assignment.Clone;
                            pa.ToApply = (version.Id == version.Template.LastVersion.Id);
                            Manager.SaveOrUpdate(pa);
                            assignment.Id = pa.Id;
                        }
                    }
                    public List<Int32> GetIdCommunityAssignments(long idVersion)
                    {
                        List<Int32> items = new List<Int32>();
                        try
                        {
                            items = (from a in Manager.GetIQ<VersionCommunityPermission>() where a.Deleted == BaseStatusDeleted.None && a.Version.Id == idVersion && a.AssignedTo != null select a.AssignedTo.Id).ToList();
                            items.AddRange((from ra in Manager.GetIQ<VersionRolePermission>()
                                            where ra.Version.Id == idVersion && ra.Deleted == BaseStatusDeleted.None && ra.Community != null && ra.AssignedTo != null
                                            select ra.Community.Id).ToList());
                            items = items.Distinct().ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        return items;
                    }
                    public Boolean AddCommunityAssignment(long idVersion, List<Int32> items)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (version != null && version.Template != null && person != null)
                            {
                                //List<BaseForPaperCommunityAssignment> assignments = (from a in Manager.GetIQ<BaseForPaperCommunityAssignment>()
                                //                                                     where a.BaseForPaper.Id == call.Id && a.Deleted != BaseStatusDeleted.None
                                //                                                     select a).ToList();
                                //foreach (BaseForPaperCommunityAssignment assignment in assignments.Where(a => a.AssignedTo != null && items.Contains(a.AssignedTo.Id)))
                                //{
                                //    assignment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                //    items.Remove(assignment.AssignedTo.Id);
                                //}
                                foreach (Int32 idCommunity in items)
                                {
                                    List<VersionRolePermission> rAssignments = (from a in Manager.GetIQ<VersionRolePermission>()
                                                                                where a.Version.Id == idVersion && a.Deleted == BaseStatusDeleted.None && a.Community != null
                                                                                     && a.Community.Id == idCommunity && a.AssignedTo!=null 
                                                                                     select a).ToList();
                                    foreach (VersionRolePermission rAssignment in rAssignments.Where(a=> a.AssignedTo.Id != 1))
                                    {
                                        rAssignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    if (rAssignments.Any())
                                        Manager.SaveOrUpdateList(rAssignments);

                                    VersionRolePermission assignment = rAssignments.Where(a => a.AssignedTo.Id == 1).FirstOrDefault();
                                    if (assignment == null)
                                    {
                                        assignment = new VersionRolePermission();
                                        assignment.Deleted = BaseStatusDeleted.None;
                                        assignment.AssignedTo = Manager.Get<Role>(1);
                                        assignment.Community = Manager.GetLiteCommunity(idCommunity);
                                        assignment.Version = version;
                                        assignment.Template = version.Template;
                                        assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        assignment.See = true;
                                        assignment.Clone = true;
                                        assignment.ToApply = (version.Id == version.Template.LastVersion.Id);
                                        Manager.SaveOrUpdate(assignment);
                                    }
                                    else if (assignment.Deleted != BaseStatusDeleted.None)
                                    {
                                        assignment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        assignment.See = true;
                                        assignment.Clone = true;
                                        assignment.ChangePermission = false;
                                        assignment.Edit = false;
                                        assignment.ToApply = (version.Id == version.Template.LastVersion.Id);
                                        Manager.SaveOrUpdate(assignment);
                                    }
                                }
                            }
                            Manager.Commit();
                            result = (version != null);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    public Boolean DeleteCommunityAssignments(long idVersion, Int32 idCommunity, long idAssignment)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (version != null && person != null)
                            {
                                if (idAssignment > 0)
                                {
                                    VersionCommunityPermission assignment = Manager.Get<VersionCommunityPermission>(idAssignment);
                                    if (assignment != null)
                                        assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                                else
                                {
                                    List<VersionCommunityPermission> assignments = (from a in Manager.GetIQ<VersionCommunityPermission>()
                                                                                    where a.Version.Id == idVersion && a.Deleted != BaseStatusDeleted.None && a.AssignedTo != null && a.AssignedTo.Id == idCommunity
                                                                                   select a).ToList();
                                    foreach (VersionCommunityPermission assignment in assignments)
                                    {
                                        assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                }

                                List<VersionRolePermission> rAssignments = (from a in Manager.GetIQ<VersionRolePermission>()
                                                                               where a.Version.Id == idVersion && a.Deleted == BaseStatusDeleted.None && a.Community != null
                                                                                 && a.Community.Id == idCommunity
                                                                                 select a).ToList();
                                foreach (VersionRolePermission rAssignment in rAssignments)
                                {
                                    rAssignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                            }
                            //List<VersionModulePermission> mAssignments = (from a in Manager.GetIQ<VersionModulePermission>()
                            //                                            where a.Version.Id == idVersion && a.Deleted == BaseStatusDeleted.None && a.Community != null
                            //                                              && a.Community.Id == idCommunity
                            //                                            select a).ToList();
                            //foreach (VersionModulePermission mAssignment in mAssignments)
                            //{
                            //    mAssignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            //}

                            Manager.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    private void SaveRolePermissions(litePerson person, TemplateDefinitionVersion version, List<dtoRoleAssignment> roleAssignments)
                    {
                        foreach (var item in roleAssignments.GroupBy(a => a.IdCommunity))
                        { 
                            liteCommunity community = Manager.GetLiteCommunity(item.Key);
                            if (community !=null){
                                List<Int32> roles = item.Select(a => a.IdRole).ToList();
                                List<VersionRolePermission> rAssignments = (from a in Manager.GetIQ<VersionRolePermission>()
                                                                            where a.Version.Id == version.Id && a.AssignedTo != null && a.Community != null && a.Community.Id == community.Id
                                                                            select a).ToList();
                                // Delete removed profile types
                                if (rAssignments.Where(p => !roles.Contains(p.AssignedTo.Id) && p.Deleted == BaseStatusDeleted.None).Any())
                                {
                                    foreach (VersionRolePermission assignment in rAssignments.Where(p => !roles.Contains(p.AssignedTo.Id) && p.Deleted == BaseStatusDeleted.None))
                                    {
                                        assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                }
                                foreach (dtoRoleAssignment assignment in item)
                                {
                                    Boolean added = false;
                                    VersionRolePermission pa = rAssignments.Where(p => p.AssignedTo.Id == assignment.IdRole).FirstOrDefault();
                                    if (pa == null)
                                    {
                                        pa = new VersionRolePermission();
                                        pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        pa.Version = version;
                                        pa.Template = version.Template;
                                        pa.AssignedTo = Manager.Get<Role>(assignment.IdRole);
                                        pa.Community = community;
                                        added = true;
                                    }
                                    else if (pa.Deleted != BaseStatusDeleted.None)
                                        pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                    
                                    pa.See = assignment.Use;
                                    pa.Edit = assignment.Edit;
                                    pa.ChangePermission = assignment.ChangePermission;
                                    pa.Clone = assignment.Clone;
                                    pa.ToApply = (version.Id == version.Template.LastVersion.Id);
                                    Manager.SaveOrUpdate(pa);

                                    if (added)
                                        assignment.Id = pa.Id;
                                }
                            }
                        }
                    }
                #endregion

                #region "User Assignments"
                    public List<Int32> GetIdUserAssignments(long idVersion)
                    {
                        List<Int32> items = new List<Int32>();
                        try
                        {
                            items = (from a in Manager.GetIQ<VersionPersonPermission>()
                                     where a.Deleted == BaseStatusDeleted.None && a.Version.Id == idVersion && a.AssignedTo != null
                                     select a.AssignedTo.Id).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        return items.Distinct().ToList();
                    }
                    public List<VersionPersonPermission> AddPersonAssignments(long idVersion, List<Int32> users)
                    {
                        List<VersionPersonPermission> assignments = (users.Any() ? new List<VersionPersonPermission>() : null);
                        try
                        {
                            Manager.BeginTransaction();
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (version != null && person != null)
                            {
                                List<VersionPersonPermission> pAssignments = (from a in Manager.GetIQ<VersionPersonPermission>()
                                                                                   where a.Version == version
                                                                                   select a).ToList();
                                foreach (Int32 idUser in users)
                                {
                                    VersionPersonPermission pa = pAssignments.Where(p => p.AssignedTo.Id == idUser).FirstOrDefault();
                                    if (pa == null)
                                    {
                                        pa = new VersionPersonPermission();
                                        pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        pa.Version = version;
                                        pa.Template = version.Template;
                                        pa.AssignedTo = Manager.GetLitePerson(idUser);
                                        if (pa.AssignedTo != null)
                                        {
                                            pa.See = true;
                                            pa.Clone = true;
                                            pa.ToApply = (version.Id == version.Template.LastVersion.Id);
                                            Manager.SaveOrUpdate(pa);
                                            assignments.Add(pa);
                                        }
                                    }
                                    else if (pa.Deleted != BaseStatusDeleted.None)
                                    {
                                        pa.See = true;
                                        pa.Edit = false;
                                        pa.ChangePermission = false;
                                        pa.Clone = true;
                                        pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                        pa.ToApply = (version.Id == version.Template.LastVersion.Id);
                                        Manager.SaveOrUpdate(pa);
                                        assignments.Add(pa);
                                    }
                                }
                            }
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            assignments = new List<VersionPersonPermission>();
                        }
                        return assignments;
                    }
                    public Boolean SavePersonPermissions(long idVersion, List<dtoPersonAssignment> assignments)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                            if (version != null && person != null )
                            {
                                SavePersonPermissions(person, version, assignments);
                            }
                            Manager.Commit();
                            result = (version != null);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }

                    private void SavePersonPermissions(litePerson person, TemplateDefinitionVersion version, List<dtoPersonAssignment> assignments)
                    {
                     
                        List<Int32> users = assignments.Select(a => a.IdPerson).ToList();
                        List<VersionPersonPermission> pAssignments = (from a in Manager.GetIQ<VersionPersonPermission>()
                                                                        where a.Version == version
                                                                        select a).ToList();
                        // Delete removed profile types
                        if (pAssignments.Where(p => !users.Contains(p.AssignedTo.Id) && p.Deleted == BaseStatusDeleted.None).Any())
                        {
                            pAssignments.Where(p => !users.Contains(p.AssignedTo.Id) && p.Deleted == BaseStatusDeleted.None).ToList().ForEach(a => a.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                            Manager.SaveOrUpdateList(pAssignments.Where(p => !users.Contains(p.AssignedTo.Id)).ToList());
                        }
                        foreach (dtoPersonAssignment assignment in assignments)
                        {
                            VersionPersonPermission pa = pAssignments.Where(p => p.AssignedTo.Id == assignment.IdPerson).FirstOrDefault();
                            if (pa == null)
                            {
                                pa = new VersionPersonPermission();
                                pa.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                pa.Version = version;
                                pa.Template = version.Template;
                                pa.AssignedTo = Manager.GetLitePerson(assignment.IdPerson);
                            }
                            else if (pa.Deleted != BaseStatusDeleted.None)
                                pa.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            pa.See = assignment.Use;
                            pa.Edit = assignment.Edit;
                            pa.ChangePermission = assignment.ChangePermission;
                            pa.Clone = assignment.Clone;
                            pa.ToApply = (version.Id == version.Template.LastVersion.Id);
                            Manager.SaveOrUpdate(pa);
                            assignment.Id = pa.Id;
                        }
                    }
                #endregion

                public Boolean SavePermissions(long idVersion, List<dtoTemplateAssignment> assignments)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                        litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                        if (version != null && person != null )
                        {
                            if (assignments.Where(a=>a.Type== PermissionType.Person).Any())
                                SavePersonPermissions(person, version, assignments.Where(a=>a.Type== PermissionType.Person).Select(a=> (dtoPersonAssignment)a).ToList());
                            if (assignments.Where(a => a.Type == PermissionType.ProfileType).Any())
                                SaveProfileTypePermissions(person, version, assignments.Where(a => a.Type == PermissionType.ProfileType).Select(a => (dtoProfileTypeAssignment)a).ToList());
                            if (assignments.Where(a => a.Type == PermissionType.Role).Any())
                                SaveRolePermissions(person, version, assignments.Where(a => a.Type == PermissionType.Role).Select(a => (dtoRoleAssignment)a).ToList());
                        }
                        Manager.Commit();
                        result = (version != null);
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return result;
                }
            #endregion
        #endregion

        #region  "List"
            public List<TemplateDisplay> GetAvailableTemplateDisplay(TemplateType type, Int32 idCommunity, Int32 idUser,String moduleCode="", Boolean alsoByUser=true)
                {
                List<TemplateDisplay> items = new List<TemplateDisplay>();
                var query = (from t in Manager.Linq<TemplateDefinition>() select t);
                switch (type)
                {
                    case TemplateType.System:
                        query = query.Where(t => t.Type == TemplateType.System && t.OwnerInfo.Type == OwnerType.System);
                        break;
                    case TemplateType.Module:
                        query = query.Where(t => t.Type == TemplateType.Module && t.OwnerInfo.ModuleCode == moduleCode && (idCommunity > 0 && (t.OwnerInfo.Community.Id == idCommunity)));
                        break;
                    case TemplateType.User:
                        query = query.Where(t => t.Type == TemplateType.User && t.OwnerInfo.Type == OwnerType.Person && t.OwnerInfo.Person != null && (!alsoByUser || (alsoByUser && t.OwnerInfo.Person.Id == idUser)));
                        break;
                }
                items.Add(TemplateDisplay.OnlyVisible);
                if (query.Where(t => t.Deleted != BaseStatusDeleted.None).Any())
                {
                    items.Add(TemplateDisplay.All);
                    items.Add(TemplateDisplay.Deleted);
                }
                return items;
            }
            public List<dtoDisplayTemplateDefinition> GetTemplates(dtoModuleContext context, dtoBaseFilters filters, String unknownUser, Int32 idUser, Int32 idUserLanguage = 0) //, int pageIndex, int pageSize)
            {
                List<dtoDisplayTemplateDefinition> templates = null;
                try
                {
                    litePerson p = Manager.GetLitePerson(idUser);
                    Language l = Manager.GetDefaultLanguage();
                    if (idUserLanguage==0)
                        idUserLanguage = UC.Language.Id;
                    if (p != null && p.Id>0)
                    {
                        templates = GetParsedList(context, filters, p,unknownUser, idUserLanguage, l.Id);
                    }
                }
                catch (Exception ex)
                {

                }
                return templates;
            }


            private List<dtoDisplayTemplateDefinition> GetParsedList(dtoModuleContext context, dtoBaseFilters filters, litePerson person, String unknownUser, Int32 idUserLanguage, Int32 idDefaultLanguage)//, int pageIndex, int pageSize)
            {
                List<dtoDisplayTemplateDefinition> items = null;
                try
                {
                    List<TemplateDefinition> templates = GetTemplatesList(context, filters, person, idUserLanguage, idDefaultLanguage);
                    if (context.IdAction > 0) {
                        var query = (from ns in Manager.GetIQ<CommonNotificationSettings>()
                                     where ns.Deleted == BaseStatusDeleted.None  && ((ns.Settings.ModuleCode == context.ModuleCode && !String.IsNullOrEmpty(context.ModuleCode)) || (ns.Settings.IdModule == context.IdModule && context.IdModule > 0))
                                     select ns);
                        List<long> tActions = query.Where(ns=>ns.Settings.IdModuleAction== context.IdAction).Select(ns=>ns.Template.Id).ToList();
                        List<long> eActions = new List<long>();
                        if (context.AlsoEmptyActions)
                            eActions = query.Where(ns=>ns.Settings.IdModuleAction!= context.IdAction).Select(ns=>ns.Template.Id).ToList();
                        templates = templates.Where(t => tActions.Contains(t.Id) || (context.AlsoEmptyActions && !eActions.Contains(t.Id))).ToList();
                    }
                    items = new List<dtoDisplayTemplateDefinition>();
                    String searchName = (!String.IsNullOrEmpty(filters.SearchForName) ? filters.SearchForName.Trim() : "");
                    if (!String.IsNullOrEmpty(searchName))
                        searchName = searchName.ToLower();

                    foreach (TemplateDefinition template in templates.Where(t=> String.IsNullOrEmpty(searchName) || t.LastVersion.GetLowerName(idUserLanguage, idDefaultLanguage).Contains(searchName)))
                    { 
                        dtoDisplayTemplateDefinition item = new dtoDisplayTemplateDefinition();
                        item.Id = template.Id;
                        item.Deleted= template.Deleted;
                        item.ModifiedByName = (template.LastVersion.ModifiedBy == null) ? unknownUser : template.LastVersion.ModifiedBy.SurnameAndName;
                        item.CreatorName = (template.CreatedBy == null) ? unknownUser : template.CreatedBy.SurnameAndName;
                        item.UserDisplayName = item.CreatorName;
                        item.ModifiedOn = template.LastVersion.ModifiedOn;
                        item.OwnerInfo = new dtoBaseTemplateOwner(template.OwnerInfo); 
                        item.Type= template.Type;
                        item.IsEnabled= template.IsEnabled;
                        item.Name = template.LastVersion.GetName(idUserLanguage, idDefaultLanguage);
                        item.IdLastVersion = template.LastVersion.Id;

                        //dtoTemplatePermission permission = 
                        dtoTemplatePermission permission = null;
                        if (template.Type == filters.TemplateType && ((template.OwnerInfo.Community== null && template.OwnerInfo.IsPortal && context.IdCommunity<1) || (template.OwnerInfo.Community != null && template.OwnerInfo.Community.Id == context.IdCommunity)))
                        {
                            permission = GetManagementTemplatePermission(template.LastVersion, context.Permissions);
                            item.FromOther=false;
                        }
                        else{
                            permission= GetItemPermission(template.LastVersion);
                            item.FromOther=true;
                        }
                        item.Permission = permission.Copy();
                        item.Permission.AllowNewVersion = (item.Permission.AllowEdit && (item.Deleted == BaseStatusDeleted.None) && !template.Versions.Where(v => v.Deleted == BaseStatusDeleted.None && v.Status == TemplateStatus.Draft).Any());
                        item.Permission.AllowEdit = (template.LastVersion.Status != TemplateStatus.Active) && (item.Deleted== BaseStatusDeleted.None) && permission.AllowEdit;
                        item.Permission.AllowDelete = (item.Deleted != BaseStatusDeleted.None) && item.Permission.AllowDelete;
                        item.Permission.AllowUnDelete = (item.Deleted != BaseStatusDeleted.None) && item.Permission.AllowUnDelete;
                        item.Permission.AllowVirtualDelete = (item.Deleted == BaseStatusDeleted.None) && item.Permission.AllowVirtualDelete;
                        item.Permission.AllowClone = item.Permission.AllowClone && (item.Deleted == BaseStatusDeleted.None);
                        foreach (TemplateDefinitionVersion version in template.Versions.Where(v => v.Deleted == BaseStatusDeleted.None && (permission.AllowEdit || (!item.Permission.AllowEdit && v.Status != TemplateStatus.Draft))).OrderByDescending(v => v.Id))
                        {
                            dtoDisplayTemplateVersion dtoVersion = new dtoDisplayTemplateVersion(){ 
                                         Id=version.Id,
                                         IdTemplate= version.Template.Id,
                                         Number= version.Number,
                                         Owner= version.Owner,
                                         Status= version.Status,
                                         Name = version.GetName(idUserLanguage, idDefaultLanguage)
                                        };
                            dtoVersion.Template = item;
                            dtoVersion.IdTemplate = item.Id;
                            dtoVersion.ModifiedByName = (version.ModifiedBy == null) ? unknownUser : version.ModifiedBy.SurnameAndName;
                            dtoVersion.CreatorName = (version.CreatedBy == null) ? unknownUser : version.CreatedBy.SurnameAndName;
                            dtoVersion.CreatedOn = version.CreatedOn;
                            dtoVersion.UserDisplayName = item.CreatorName;
                            dtoVersion.Permission = (version.Id == template.LastVersion.Id) ? permission.Copy() : GetManagementTemplatePermission(version, context.Permissions);

                            dtoVersion.ModifiedOn = version.ModifiedOn;
                            if (version.Id== item.IdLastVersion){
                                dtoVersion.Permission.AllowDelete = false;
                                dtoVersion.Permission.AllowVirtualDelete = false;
                                dtoVersion.Permission.AllowUnDelete = false;
                                dtoVersion.Permission.AllowClone = false;
                            }
                            else{
                                dtoVersion.Permission.AllowDelete = (item.Deleted == BaseStatusDeleted.None) && item.Permission.AllowDelete && (version.Deleted != BaseStatusDeleted.None) && version.Status != TemplateStatus.Active;
                                dtoVersion.Permission.AllowVirtualDelete = (item.Deleted == BaseStatusDeleted.None) && item.Permission.AllowVirtualDelete && (version.Deleted == BaseStatusDeleted.None) && version.Status != TemplateStatus.Active;
                                dtoVersion.Permission.AllowUnDelete = (item.Deleted == BaseStatusDeleted.None) && item.Permission.AllowUnDelete && (version.Deleted != BaseStatusDeleted.None) && version.Status != TemplateStatus.Active;
                                dtoVersion.Permission.AllowClone = false;
                                dtoVersion.Permission.AllowAdd = false;
                            }
                            dtoVersion.Permission.AllowEdit = (item.Deleted == BaseStatusDeleted.None) && dtoVersion.Permission.AllowEdit && (version.Deleted == BaseStatusDeleted.None) && (version.Status == TemplateStatus.Draft);
                            dtoVersion.Permission.AllowChangePermission = (item.Deleted == BaseStatusDeleted.None) && dtoVersion.Permission.AllowChangePermission && (version.Deleted == BaseStatusDeleted.None) && (version.Status == TemplateStatus.Draft || version.Status == TemplateStatus.Active);
                            item.Versions.Add(dtoVersion);
                        }
                        if (item.Versions.Count == 1)
                        {
                            item.Versions[0].DisplayAs = ItemDisplayAs.first | ItemDisplayAs.last;
                            item.Versions[0].Permission.AllowAdd = (item.Versions[0].Permission.AllowAdd && item.Versions[0].Status == TemplateStatus.Active);
                        }
                        else if (item.Versions.Count > 1)
                        {
                            item.Versions.ForEach(v => v.Permission.AllowAdd = false);
                            item.Versions.First().DisplayAs = ItemDisplayAs.first;
                            item.Versions.Last().DisplayAs = ItemDisplayAs.last;
                            if (item.Versions.Last().Status == TemplateStatus.Active)
                                item.Versions.Last().Permission.AllowAdd = item.Permission.AllowAdd;
                        }
                        if (item.Versions.Count > 0)
                            item.CreatedOn = item.Versions.OrderBy(v => v.Id).FirstOrDefault().CreatedOn;
                        else
                            item.CreatedOn = template.CreatedOn;
                        items.Add(item);
                    }
                    switch (filters.OrderBy) { 
                        case TemplateOrder.ByDate:
                            items = (filters.Ascending) ? items.OrderBy(t => t.ModifiedOn).ToList() : items.OrderByDescending(t => t.ModifiedOn).ToList();
                            break;
                        case TemplateOrder.ByName:
                            items = (filters.Ascending) ? items.OrderBy(t => t.Name).ToList() : items.OrderByDescending(t => t.Name).ToList();
                            break;
                        case TemplateOrder.ByUser:
                            items = (filters.Ascending) ? items.OrderBy(t => t.UserDisplayName).ToList() : items.OrderByDescending(t => t.UserDisplayName).ToList();
                            break;
                        case TemplateOrder.ByType:
                            items = (filters.Ascending) ? items.OrderBy(t => filters.TranslationsType[t.Type]).ToList() : items.OrderByDescending(t => filters.TranslationsType[t.Type]).ToList();
                            break;
                    }
                }
                catch (Exception ex) { 
                
                }
                return items.Where(t=> t.Permission.AllowUse).ToList();
            }
            private List<TemplateDefinition> GetTemplatesList(dtoModuleContext context, dtoBaseFilters filters, litePerson person, Int32 idUserLanguage, Int32 idDefaultLanguage)//, int pageIndex, int pageSize)
            {
                List<TemplateDefinition> templates = new List<TemplateDefinition>();

                //  recupero i template di base prescelti (ossia modulo, di sistema, personali)
                var query = GetTemplatesQuery(context, filters, person.Id, idUserLanguage, idDefaultLanguage);
                templates.AddRange(query.ToList());

                // Devo aggiungere i template associati a me come persona/ come profilo / come ruolo
                templates.AddRange(GetAssignedByProfile(templates.Select(t => t.Id).ToList(), context, filters, person));
                switch (context.LoaderType) { 
                    case TemplateLoaderType.User:
                        break;
                    case TemplateLoaderType.None:
                        break;
                    default:
                        templates.AddRange(GetAssignedByRoles(templates.Select(t => t.Id).ToList(), context, filters, (context.IdCommunity == 0), Manager.GetLiteCommunity(context.IdCommunity), person));
                        break;

                }
                return templates;
            }
            protected List<TemplateDefinition> GetAssignedByProfile(List<long> idTemplates, dtoModuleContext context, dtoBaseFilters filters, litePerson person)
            {
                List<TemplateDefinition> results = new List<TemplateDefinition>();
                var query = GetBaseTemplatesQuery(context, filters, person.Id, false);

                results.AddRange(query.ToList().Where(t =>
                                               !idTemplates.Contains(t.Id) && t.LastVersion != null && t.LastVersion.Permissions != null
                                               &&
                                               (
                                                   t.LastVersion.Permissions.Where(p => p.Type == PermissionType.Person && ((VersionPersonPermission)p).AssignedTo.Id == person.Id).Any()
                                                   ||
                                                   t.LastVersion.Permissions.Where(p => p.Type == PermissionType.ProfileType && ((VersionProfileTypePermission)p).AssignedTo == person.TypeID).Any()
                                               )
                                               ).ToList());
                return results;
            }
            protected List<TemplateDefinition> GetAssignedByRoles(List<long> idTemplates, dtoModuleContext context, dtoBaseFilters filters, Boolean forPortal, liteCommunity community, litePerson person)
            {
                List<TemplateDefinition> results = new List<TemplateDefinition>();

                List<VersionRolePermission> permissions = GetVersionRolePermissionQuery(context, filters, person.Id, false).Where(p => !idTemplates.Contains(p.Template.Id)).ToList();
                List<int> idCommunities = permissions.Where(p=> p.Community !=null && p.AssignedTo !=null).Select(i => i.Community.Id).ToList();
                List<LazySubscription> subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);

                List<long> idQueryTemplates = (from p in permissions
                                          where subscriptions.Where(s => s.IdCommunity == p.Community.Id && s.IdRole == p.AssignedTo.Id).Any()
                                          select p.Version.Template.Id).ToList();
                
                Int32 pageIndex = 0;
                List<long> idPagedTemplates = idQueryTemplates.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                while (idPagedTemplates.Any()) {
                    pageIndex++;
                    results.AddRange((from t in Manager.GetIQ<TemplateDefinition> () where idPagedTemplates.Contains(t.Id) select t).ToList());
                    idPagedTemplates = idQueryTemplates.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                }
                return results;
            }

            private IQueryable<TemplateDefinition> GetTemplatesQuery(dtoModuleContext context, dtoBaseFilters filters, Int32 idUser, Int32 idUserLanguage , Int32 idDefaultLanguage)
            {
                var query = GetBaseTemplatesQuery(context, filters, idUser);
                String searchName = (!String.IsNullOrEmpty(filters.SearchForName) ? filters.SearchForName.Trim() : "");
                if (!String.IsNullOrEmpty(searchName))
                {
                    searchName = searchName.ToLower();
                    List<long> idTemplates = GetTemplatesByName(GetBaseTemplatesQuery(context, filters, idUser), searchName, idUserLanguage, idDefaultLanguage);
                    query = query.ToList().Where(t => idTemplates.Contains(t.Id)).AsQueryable();
                }
                return query;
            }
            private IQueryable<TemplateDefinition> GetBaseTemplatesQuery(dtoModuleContext context, dtoBaseFilters filters, Int32 idUser, Boolean alsoByUser = true)
            {
                var query = (from t in Manager.Linq<TemplateDefinition>()
                             where (filters.TemplateDisplay== TemplateDisplay.All || (filters.TemplateDisplay== TemplateDisplay.Deleted && t.Deleted!=  BaseStatusDeleted.None) || (filters.TemplateDisplay== TemplateDisplay.OnlyVisible && t.Deleted== BaseStatusDeleted.None))
                             select t);
                
                //else
                //{
                    switch (filters.TemplateType)
                    {
                        case TemplateType.System:
                            query = query.Where(t => t.Type == TemplateType.System && t.OwnerInfo.Type == OwnerType.System);
                            break;
                        case TemplateType.Module:
                            if (context.ModuleObject == null)
                            {
                                query = query.Where(t =>
                                   (t.Type == TemplateType.Module && t.OwnerInfo.ModuleObject == null && t.OwnerInfo.ModuleCode == filters.ModuleCode && (context.IdCommunity > 0 && (t.OwnerInfo.Community.Id == context.IdCommunity)))
                                   ||
                                       (
                                       context.LoaderType == TemplateLoaderType.OtherModule && t.Type != TemplateType.Module && t.CurrentModulesContent.Contains("#" + filters.ModuleCode + "#")
                                       )
                                   );
                            }
                            else {
                                query = query.Where(t =>
                                   (t.Type == TemplateType.Module && (t.OwnerInfo.ModuleObject == null || (context.ModuleObject != null && t.OwnerInfo.ModuleObject != null && t.OwnerInfo.ModuleObject.ObjectLongID == context.ModuleObject.ObjectLongID && t.OwnerInfo.ModuleObject.ObjectTypeID == context.ModuleObject.ObjectTypeID && t.OwnerInfo.ModuleObject.ServiceID == context.ModuleObject.ServiceID) || (context.ModuleObject == null && t.OwnerInfo.ModuleObject == null)) && t.OwnerInfo.ModuleCode == filters.ModuleCode && (context.IdCommunity > 0 && (t.OwnerInfo.Community.Id == context.IdCommunity)))
                                   ||
                                       (
                                       context.LoaderType == TemplateLoaderType.OtherModule && t.Type != TemplateType.Module && t.CurrentModulesContent.Contains("#" + filters.ModuleCode + "#")
                                       )
                                   );
                            }
                            break;
                        case TemplateType.User:
                            query = query.Where(t => t.Type == TemplateType.User && t.OwnerInfo.Type == OwnerType.Person && t.OwnerInfo.Person != null && (!alsoByUser || (alsoByUser && t.OwnerInfo.Person.Id == idUser)));
                            break;
                        default:
                            query = query.Where(t => t.Type == filters.TemplateType);
                            break;
                    }
                //}
                return query;
            }
            private IQueryable<VersionRolePermission> GetVersionRolePermissionQuery(dtoModuleContext context, dtoBaseFilters filters, Int32 idUser, Boolean alsoByUser = true)
            {
                var query = (from t in Manager.Linq<VersionRolePermission>()
                             where t.ToApply == true && t.Deleted == BaseStatusDeleted.None && (filters.TemplateDisplay == TemplateDisplay.All || (filters.TemplateDisplay == TemplateDisplay.Deleted && t.Template.Deleted != BaseStatusDeleted.None) || (filters.TemplateDisplay == TemplateDisplay.OnlyVisible && t.Template.Deleted == BaseStatusDeleted.None))
                             select t).ToList().AsQueryable();
                switch (filters.TemplateType)
                {
                    case TemplateType.System:
                        query = query.Where(t => t.Template.Type == TemplateType.System && t.Template.OwnerInfo.Type == OwnerType.System).AsQueryable();
                        break;
                    case TemplateType.Module:
                        query = query.Where(t => t.Template.Type == TemplateType.Module && t.Template.OwnerInfo.ModuleCode == filters.ModuleCode && (context.IdCommunity > 0 && (t.Template.OwnerInfo.Community.Id == context.IdCommunity))).AsQueryable();
                        break;
                    case TemplateType.User:
                        query = query.Where(t => t.Template.Type == TemplateType.User && t.Template.OwnerInfo.Type == OwnerType.Person && t.Template.OwnerInfo.Person != null && (!alsoByUser || (alsoByUser && t.Template.OwnerInfo.Person.Id == idUser))).AsQueryable();
                        break;
                    default:
                        query = query.Where(t => t.Template.Type == filters.TemplateType).AsQueryable();
                        break;
                }
                return query;
            }
            private List<long> GetTemplatesByName(IQueryable<TemplateDefinition> query, String searchName, Int32 idUserLanguage , Int32 idDefaultLanguage)
            {
                List<long> results = new List<long>();
                List<long> idTemplates = query.Select(t => t.Id).ToList();

                Int32 pageSize = 50;
                Int32 pageIndex = 0;
                var tQuery = idTemplates.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                while (tQuery.Any())
                {
                    List<TemplateDefinitionVersion> versions = (from t in Manager.GetIQ<TemplateDefinition>() where tQuery.Contains(t.Id) select t).ToList().Select(t=> t.LastVersion).ToList();
                    results.AddRange(
                        (from t in versions where t.GetLowerName(idUserLanguage,idDefaultLanguage).Contains(searchName) select t.Template.Id).ToList());
                    pageIndex++;
                    tQuery = idTemplates.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                return results.Distinct().ToList();
            }
            //protected List<long> GetIdCallsBySubmissionQuery(List<long> idCalls, Boolean fromAllcommunities, Boolean forPortal, Community community, Person person, CallForPaperType type, CallStatusForSubmitters status, FilterCallVisibility filter)
            //{
            //    List<long> results = new List<long>();
            //    Int32 pageIndex = 0;

            //    List<long> idPagedCalls = idCalls.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
            //    while (idPagedCalls.Any())
            //    {
            //        results = (from pa in Manager.GetIQ<BaseForPaperPersonAssignment>()
            //                   where idPagedCalls.Contains(pa.BaseForPaper.Id) && pa.AssignedTo == person && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
            //                   select pa.BaseForPaper.Id).ToList();
            //        idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();
            //        if (forPortal && idPagedCalls.Any())
            //        {
            //            results.AddRange((from pa in Manager.GetIQ<BaseForPaperPersonTypeAssignment>()
            //                              where idPagedCalls.Contains(pa.BaseForPaper.Id) && pa.AssignedTo == person.TypeID && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
            //                              select pa.BaseForPaper.Id).ToList());
            //            idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();
            //        }
            //        if (!fromAllcommunities && idPagedCalls.Any())
            //        {
            //            results.AddRange((from pa in Manager.GetIQ<BaseForPaperCommunityAssignment>()
            //                              where idPagedCalls.Contains(pa.BaseForPaper.Id) && community != null && pa.AssignedTo == community && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
            //                              select pa.BaseForPaper.Id).ToList());
            //            idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();

            //            if (idPagedCalls.Any())
            //            {
            //                Role role = Manager.GetRole(Manager.GetActiveSubscriptionRoleId(person, community));

            //                results.AddRange((from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
            //                                  where idPagedCalls.Contains(pa.BaseForPaper.Id) && community != null && pa.Community == community && pa.AssignedTo == role && role != null && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
            //                                  select pa.BaseForPaper.Id).ToList());
            //            }
            //        }
            //        else
            //        {
            //            var callCommunities = (from pa in Manager.GetIQ<BaseForPaperCommunityAssignment>()
            //                                   where idPagedCalls.Contains(pa.BaseForPaper.Id) && pa.AssignedTo != null && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
            //                                   select new { idCall = pa.BaseForPaper.Id, idCallCommunity = (pa.BaseForPaper.Community == null) ? 0 : pa.BaseForPaper.Community.Id, idCommunity = pa.AssignedTo.Id }).ToList();
            //            List<int> idCommunities = callCommunities.Select(i => i.idCommunity).ToList();
            //            List<LazySubscription> subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);
            //            // add other assigned communities
            //            results.AddRange((from c in callCommunities where subscriptions.Where(s => s.IdCommunity == c.idCommunity && c.idCallCommunity != c.idCommunity).Any() select c.idCall).ToList());
            //            idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();

            //            if (idPagedCalls.Any())
            //            {
            //                //add by permissions
            //                List<LazySubscription> tSubscriptions = subscriptions.Where(s => callCommunities.Where(c => c.idCommunity == c.idCallCommunity && c.idCommunity == s.IdCommunity).Any()).ToList();
            //                idCommunities = tSubscriptions.Select(s => s.IdCommunity).Distinct().ToList();
            //                Int32 idModule = (type == CallForPaperType.CallForBids) ? ServiceModuleID(ModuleCallForPaper.UniqueCode) : ServiceModuleID(ModuleRequestForMembership.UniqueCode);
            //                Int32 idRole = 0;
            //                long requiredPermission = (type == CallForPaperType.CallForBids) ? (long)ModuleCallForPaper.Base2Permission.ListCalls : (long)ModuleRequestForMembership.Base2Permission.ListRequests;

            //                List<CommunityModuleAssociation> modules = (from cModule in Manager.GetIQ<CommunityModuleAssociation>()
            //                                                            where cModule.Enabled && cModule.Service.Available && idCommunities.Contains(cModule.Community.Id) && cModule.Service.Id == idModule
            //                                                            select cModule).ToList();

            //                foreach (CommunityModuleAssociation m in modules)
            //                {
            //                    idRole = tSubscriptions.Where(s => s.IdCommunity == m.Community.Id).Select(s => s.IdRole).FirstOrDefault();
            //                    var qPermission = (from crmp in Manager.GetIQ<LazyCommunityModulePermission>()
            //                                       where crmp.CommunityID == m.Community.Id && crmp.ModuleID == idModule && crmp.RoleID == idRole
            //                                       select crmp).Skip(0).Take(1).FirstOrDefault();
            //                    if (qPermission != null && qPermission.PermissionLong > 0 && PermissionHelper.CheckPermissionSoft(requiredPermission, qPermission.PermissionLong))
            //                    {
            //                        results.AddRange(callCommunities.Where(c => c.idCallCommunity == c.idCommunity && c.idCallCommunity == m.Community.Id).Select(c => c.idCall).ToList());
            //                    }
            //                }
            //                idPagedCalls = idPagedCalls.Where(i => !results.Contains(i)).ToList();
            //            }

            //            if (idPagedCalls.Any())
            //            {
            //                var callRoles = (from pa in Manager.GetIQ<BaseForPaperRoleAssignment>()
            //                                 where idPagedCalls.Contains(pa.BaseForPaper.Id) && pa.AssignedTo != null && pa.Community != null && !pa.Deny && pa.Deleted == BaseStatusDeleted.None
            //                                 select new { idCall = pa.BaseForPaper.Id, idRole = pa.AssignedTo.Id, idCommunity = pa.Community.Id }).ToList();
            //                idCommunities = callRoles.Select(i => i.idCommunity).ToList();
            //                subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);
            //                results.AddRange((from c in callRoles where subscriptions.Where(s => s.IdCommunity == c.idCommunity && s.IdRole == c.idRole).Any() select c.idCall).ToList());
            //            }
            //        }
            //        pageIndex++;
            //        idPagedCalls = idCalls.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
            //    }

            //    return results.Distinct().ToList();
            //}
        #endregion
        #region "CRUD Actions"
            public Boolean VirtualDeleteTemplate(long idTemplate, Boolean delete)
            {
                Boolean actionDone = false;
                try
                {
                    Manager.BeginTransaction();
                    TemplateDefinition template = Manager.Get<TemplateDefinition>(idTemplate);
                    Int32 idUser = UC.CurrentUserID;
                    litePerson person = Manager.GetLitePerson(idUser);
                    if (template != null && person != null && person.TypeID != (int)UserTypeStandard.Guest)
                    {
                        template.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                        template.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(template);
                    }
                    Manager.Commit();
                    actionDone = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    actionDone = false;
                }
                return actionDone;
            }
            public Boolean PhisicalDeleteTemplate(long idTemplate)
            {
                Boolean deleted = false;
                try
                {
                    Manager.BeginTransaction();
                    TemplateDefinition template = Manager.Get<TemplateDefinition>(idTemplate);
                    Int32 idUser = UC.CurrentUserID;
                    litePerson person = Manager.GetLitePerson(idUser);
                    if (template != null && person != null && person.TypeID != (int)UserTypeStandard.Guest)
                    {
                        Manager.DeletePhysical(template);
                    }

                    Manager.Commit();
                    deleted = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    deleted = false;
                }
                return deleted;
            }
            public TemplateDefinition CloneTemplate(long idTemplate,String clonePrefix, dtoModuleContext context)
            {
                TemplateDefinition clone = null;
                try
                {
                    Manager.BeginTransaction();
                    TemplateDefinition template = Manager.Get<TemplateDefinition>(idTemplate);
                    Int32 idUser = UC.CurrentUserID;
                    litePerson person = Manager.GetLitePerson(idUser);
                    if (template != null && person != null && person.TypeID != (int)UserTypeStandard.Guest)
                    {
                        clone = template.Copy(clonePrefix, person, UC.IpAddress, UC.ProxyIpAddress);
                        if (!clone.Versions.Where(v => v.Status == TemplateStatus.Draft).Any() && clone.Versions.Count>0)
                            clone.Versions.OrderByDescending(v => v.Id).FirstOrDefault().Status = TemplateStatus.Draft;
                        clone.Versions.ToList().ForEach(v => v.Permissions = v.Permissions.Where(p => !(!p.Edit && !p.Clone && !p.ChangePermission && !p.See)).ToList());
                        switch (context.LoaderType) { 
                            case  TemplateLoaderType.User:
                                if (clone.OwnerInfo.Person != person) {
                                    clone.Versions.ToList().ForEach(v => v.Permissions = new List<VersionPermission>());
                                }
                                if (clone.OwnerInfo.Type == OwnerType.Person)
                                    clone.OwnerInfo.Person = person;
                                else {
                                    clone.OwnerInfo.Person = person;
                                    clone.OwnerInfo.Type = OwnerType.Person;
                                    clone.OwnerInfo.ModuleObject = null;
                                    clone.OwnerInfo.ModuleCode = "";
                                    clone.OwnerInfo.IdModule = 0;
                                    clone.OwnerInfo.IsPortal = true;
                                    clone.OwnerInfo.Community = null;
                                }
                                break;
                            case  TemplateLoaderType.System:
                                clone.OwnerInfo.Person = person;
                                clone.OwnerInfo.ModuleObject = null;
                                clone.OwnerInfo.ModuleCode = "";
                                clone.OwnerInfo.IdModule = 0;
                                clone.OwnerInfo.IsPortal = true;
                                clone.OwnerInfo.Community = null;
                                if (clone.OwnerInfo.Type != OwnerType.System) {
                                    clone.OwnerInfo.Type = OwnerType.System;
                                }
                                break;

                            case TemplateLoaderType.OtherModule:
                            case TemplateLoaderType.Module:
                                if (clone.OwnerInfo.Person != person)
                                {
                                    clone.Versions.ToList().ForEach(v => v.Permissions = new List<VersionPermission>());
                                }
                                clone.OwnerInfo.Person = person;
                                clone.OwnerInfo.Type = OwnerType.Module;
                                clone.OwnerInfo.Community = (context.IdCommunity > 1) ? Manager.GetLiteCommunity(context.IdCommunity) : null;
                                clone.OwnerInfo.IsPortal = (clone.OwnerInfo.Community == null);
                                clone.OwnerInfo.ModuleCode = context.ModuleCode;
                                clone.OwnerInfo.IdModule = Manager.GetModuleID(context.ModuleCode);
                                clone.OwnerInfo.ModuleObject = null;
                                break;
                            default:
                                clone.OwnerInfo.Person = person;
                                break;
                        }
                    }
                    Manager.SaveOrUpdate(clone);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    clone = null;
                }
                return clone;
            }
            public Boolean VirtualDeleteVersion(long idVersion, Boolean delete)
            {
                Boolean actionDone = false;
                try
                {
                    Manager.BeginTransaction();
                    TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                    Int32 idUser = UC.CurrentUserID;
                    litePerson person = Manager.GetLitePerson(idUser);
                    if (version != null && version.Status != TemplateStatus.Active && person != null && person.TypeID != (int)UserTypeStandard.Guest)
                    {
                        version.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                        version.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(version);
                        
                    }
                    Manager.Commit();
                    actionDone = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    actionDone = false;
                }
                return actionDone;
            }
            public Boolean PhisicalDeleteVersion(long idVersion)
            {
                Boolean deleted = false;
                try
                {
                    Manager.BeginTransaction();
                    TemplateDefinitionVersion version = Manager.Get<TemplateDefinitionVersion>(idVersion);
                    Int32 idUser = UC.CurrentUserID;
                    Person person = Manager.GetPerson(idUser);
                    if (version != null && person != null && person.TypeID != (int)UserTypeStandard.Guest)
                    {
                        Manager.DeletePhysical(version);
                    }

                    Manager.Commit();
                    deleted = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    deleted = false;
                }
                return deleted;
            }
            public TemplateDefinitionVersion AddTemplateVersion(long idTemplate)
            {
                TemplateDefinitionVersion version = null;
                try
                {
                    Manager.BeginTransaction();
                    TemplateDefinition template = Manager.Get<TemplateDefinition>(idTemplate);
                    Int32 idUser = UC.CurrentUserID;
                    litePerson person = Manager.GetLitePerson(idUser);
                    if (template != null && person != null && person.TypeID != (int)UserTypeStandard.Guest)
                    {
                        var query = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                     where  v.Template != null && v.Template.Id == idTemplate
                                     select v);
                        Boolean hasDraft = query.Where(v => v.Deleted == BaseStatusDeleted.None && v.Status == TemplateStatus.Draft).Any();
                        if (!hasDraft)
                        {
                            Int32 index = 1;
                            query.ToList().ForEach(v => v.Number = index++);
                            TemplateDefinitionVersion activeVersion = query.Where(v => v.Deleted == BaseStatusDeleted.None && v.Status == TemplateStatus.Active).OrderByDescending(v => v.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (activeVersion != null) {
                                version = activeVersion.Copy(template, person, UC.IpAddress, UC.ProxyIpAddress);
                                version.Status = TemplateStatus.Draft;
                                version.Number = index;
                                Manager.SaveOrUpdate(version);
                                template.Versions.Add(version);
                                Manager.SaveOrUpdate(template);
                            }
                        }
                    }

                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    version = null;
                }
                return version;
            }
        #endregion
        #region "Common"
            public long GetTemplateNumber(dtoBaseTemplateOwner info)
            {
                var query = (from t in Manager.GetIQ<TemplateDefinition>() where t.Deleted== BaseStatusDeleted.None && t.OwnerInfo.Type== info.Type  select t);
                long result = 1;
                switch (info.Type)
                {
                    case OwnerType.Community:
                        result = query.Where(t => t.OwnerInfo.Community != null && t.OwnerInfo.Community.Id == info.IdCommunity).Count() + 1;
                        break;
                    case OwnerType.System:
                        result = query.Count() + 1;
                        break;
                    case OwnerType.Module:
                        result = query.Where(t =>  t.OwnerInfo.IdModule == info.IdModule).Count() + 1;
                        break;
                    case OwnerType.Object:
                        result = query.Where(t => t.OwnerInfo.ModuleObject != null && t.OwnerInfo.ModuleObject.ObjectLongID == info.IdObject && t.OwnerInfo.ModuleObject.ObjectTypeID == info.IdObjectType && t.OwnerInfo.ModuleObject.ServiceID == info.IdObjectModule && t.OwnerInfo.ModuleObject.CommunityID == info.IdObjectCommunity).Count() + 1;
                        break;
                    case OwnerType.Person:
                        result = query.Where(t => t.OwnerInfo.Person != null && t.OwnerInfo.Person.Id == info.IdPerson).Count() + 1;
                        break;
                }
                return result;
            }

            public Boolean IsAdministrativeUser(Int32 idPerson)
            {
                return IsAdministrativeUser(Manager.GetPerson(idPerson));
            }
            public Boolean IsAdministrativeUser(Person person)
            {
                Boolean result = (person!= null && (person.TypeID==(int)UserTypeStandard.SysAdmin || person.TypeID==(int)UserTypeStandard.Administrator));
                if (person != null && person.TypeID == (int)UserTypeStandard.Administrative) {
                    result = true;
                }
                return result; 
            }
        #endregion
    }
}