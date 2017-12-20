using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using NHibernate.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.Notification.Domain;
using lm.Comol.Core.MailCommons.Domain.Configurations;
using lm.Comol.Core.MailCommons.Domain;

namespace lm.Comol.Core.TemplateMessages.Business
{
    public class TemplatesForOtherService : CoreServices 
    {
        protected iApplicationContext _Context;
        protected const Int32 maxItemsForQuery = 500;
        #region initClass
            private lm.Comol.Core.Mail.Messages.MailMessagesService messageService;
            private TemplateMessagesService internalService;
            private TemplateMessagesService InternalService
            {
                get
                {
                    if (internalService == null)
                        internalService = new TemplateMessagesService(_Context);
                    return internalService;
                }
            }
            private lm.Comol.Core.Mail.Messages.MailMessagesService MessageService
            {
                get
                {
                    if (messageService == null)
                        messageService = new lm.Comol.Core.Mail.Messages.MailMessagesService(_Context);
                    return messageService;
                }
            }
            public TemplatesForOtherService() :base() { }
            public TemplatesForOtherService(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
            }
            public TemplatesForOtherService(iDataContext oDC)
                : base(oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext() { DataContext = oDC };
            }
        #endregion


        public Int32 GetIdLanguage(String languageCode)
        {
            Int32 idLanguage = 0;
            try{
                Language l = Manager.GetLanguageByCodeOrDefault(languageCode,false);
                if (l!=null)
                    idLanguage = l.Id;
            }
            catch(Exception ex){
            
            }
            return idLanguage;
        }

        #region "Get Notification"
            /// <summary>
            ///     Recupero di un template automatico in base al servizio e all'azione richiesta
            /// </summary>
            /// <param name="idLanguage">Identificativo lingua</param>
            /// <param name="moduleCode">Codice univoco servizio </param>
            /// <param name="idAction">Azione di cui recuperare il template</param>
            /// <param name="idCommunity">identificativo della comunità corrente (opzionale)</param>
            /// <param name="mode">modalità di notifica (opzionale, di default è "Automatico")</param>
            /// <param name="channel">canale da utilizzare per la notificia (opzionale, di default è "Mail")</param>
            /// <param name="objectModuleCode">codcie identificativo dell'oggetto per cui si richiede un template (opzionale)</param>
            /// <param name="idObjectType">identificativo della tipologia di oggetto per cui si richiede un template (opzionale)</param>
            /// <param name="idObject">identificativo dell'oggetto per cui si richiede un template (opzionale)</param>
            /// <returns></returns>
            public dtoNotificationMessage GetNotificationMessage(Int32 idLanguage, String moduleCode, long idAction,NotificationMode mode = NotificationMode.Automatic ,NotificationChannel channel = NotificationChannel.Mail, Int32 idCommunity = -1, String objectModuleCode = "", Int32 idObjectType = -1, long idObject = -1)
            {
                return GetNotificationMessage(idLanguage, "", moduleCode, idAction, mode, channel,idCommunity, (!String.IsNullOrEmpty(objectModuleCode) && idObjectType != -1 && idObject > 0) ? ModuleObject.CreateLongObject(idObject, idObjectType, 0, moduleCode) : null);
            }
            ///// <summary>
            /////     Recupero di un template automatico in base al servizio e all'azione richiesta
            ///// </summary>
            ///// <param name="idLanguage">Identificativo lingua</param>
            ///// <param name="moduleCode">Codice univoco servizio </param>
            ///// <param name="idAction">Azione di cui recuperare il template</param>
            ///// <param name="mode">modalità di notifica (opzionale, di default è "Automatico")</param>
            ///// <param name="channel">canale da utilizzare per la notificia (opzionale, di default è "Mail")</param>
            ///// <param name="idCommunity">identificativo della comunità corrente (opzionale)</param>
            ///// <param name="obj">L'oggetto proprietario</param>
            ///// <returns></returns>
            //public dtoNotificationMessage GetNotificationMessage(Int32 idLanguage, String moduleCode, long idAction, NotificationMode mode = NotificationMode.Automatic, NotificationChannel channel = NotificationChannel.Mail, Int32 idCommunity = -1, ModuleObject obj = null)
            //{
            //    return GetNotificationMessage(idLanguage, "", moduleCode, idAction,mode,channel, idCommunity, obj);
            //}
            /// <summary>
            ///     Recupero di un template automatico in base al servizio e all'azione richiesta
            /// </summary>
            /// <param name="languageCode">codice lingua (es: it-IT)</param>
            /// <param name="moduleCode">Codice univoco servizio </param>
            /// <param name="idAction">Azione di cui recuperare il template</param>
            /// <param name="mode">modalità di notifica (opzionale, di default è "Automatico")</param>
            /// <param name="channel">canale da utilizzare per la notificia (opzionale, di default è "Mail")</param>    
            /// <param name="idCommunity">identificativo della comunità corrente (opzionale)</param>
            /// <param name="objectModuleCode">codcie identificativo dell'oggetto per cui si richiede un template (opzionale)</param>
            /// <param name="idObjectType">identificativo della tipologia di oggetto per cui si richiede un template (opzionale)</param>    
            /// <param name="idObject">identificativo dell'oggetto per cui si richiede un template (opzionale)</param>
            /// <returns></returns>
            public dtoNotificationMessage GetNotificationMessage(String languageCode, String moduleCode, long idAction, NotificationMode mode = NotificationMode.Automatic, NotificationChannel channel = NotificationChannel.Mail, Int32 idCommunity = -1, String objectModuleCode = "", Int32 idObjectModule = 0, Int32 idObjectType = 0, long idObject = -1)
            {
                return GetNotificationMessage(0, languageCode, moduleCode, idAction, mode, channel, idCommunity, (!String.IsNullOrEmpty(objectModuleCode) && idObjectType != -1 && idObject > 0) ? ModuleObject.CreateLongObject(idObject, idObjectType, 0, moduleCode) : null);
            }
            public dtoNotificationMessage GetNotificationMessage(Int32 idLanguage, String languageCode, String moduleCode, long idAction, NotificationMode mode = NotificationMode.Automatic, NotificationChannel channel = NotificationChannel.Mail, Int32 idCommunity = -1, ModuleObject obj = null)
            {
                dtoNotificationMessage message = null;
                try
                {
                    TemplateDefinitionVersion version = GetTemplateNotification(moduleCode, idAction, mode, channel, idCommunity, obj);
                    if (version!=null)
                    {
                        dtoTemplateTranslation item = InternalService.GetTranslation(version.Id, idLanguage, languageCode);
                        if (item != null)
                        {
                            message = new dtoNotificationMessage();
                            message.Translation = item.Translation.Copy();
                            message.Channel = channel;
                            message.IdTemplate = (version.Template != null) ? version.Template.Id : 0;
                            message.IdVersion = version.Id;
                            if (channel == NotificationChannel.Mail && version.ChannelSettings.Where(c=>c.Deleted== BaseStatusDeleted.None && c.Channel== channel).Any()){
                                message.MailSettings = version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == channel).FirstOrDefault().MailSettings.Copy();
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    message = null;
                }
                return message;
            }
            /// <summary>
            ///      Recupero delle possibili traduzioni di un template automatico in base al servizio e all'azione richiesta
            /// </summary>
            /// <param name="moduleCode">Codice univoco servizio </param>
            /// <param name="idAction">Azione di cui recuperare il template</param>
            /// <param name="mode">modalità di notifica (opzionale, di default è "Automatico")</param>
            /// <param name="channel">canale da utilizzare per la notificia (opzionale, di default è "Mail")</param>    
            /// <param name="idCommunity">identificativo della comunità corrente (opzionale)</param>
            /// <param name="obj">L'oggetto proprietario(opzionale)</param>
            public dtoNotificationMessages GetTemplateTranslations(String moduleCode, long idAction, NotificationMode mode = NotificationMode.Automatic, NotificationChannel channel = NotificationChannel.Mail, Int32 idCommunity = -1, ModuleObject obj = null)
            {
                dtoNotificationMessages messages = null;
                try
                {
                    TemplateDefinitionVersion version = GetTemplateNotification(moduleCode, idAction, mode, channel, idCommunity, obj);
                    if (version != null)
                    {
                        messages = new dtoNotificationMessages();
                        messages.Channel = channel;
                        messages.Translations.Add(new dtoTemplateTranslation() { Translation = version.DefaultTranslation });
                        messages.Translations.AddRange(version.Translations.Where(t=>t.Deleted== BaseStatusDeleted.None).Select(t=> new dtoTemplateTranslation() { LanguageCode= t.LanguageCode, IdLanguage= t.IdLanguage, Translation= t.Translation , Id= t.Id}) .ToList());

                        if (channel == NotificationChannel.Mail && version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == channel).Any())
                        {
                            messages.MailSettings = version.ChannelSettings.Where(c => c.Deleted == BaseStatusDeleted.None && c.Channel == channel).FirstOrDefault().MailSettings;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messages = null;
                }
                return messages;
            }
            private TemplateDefinitionVersion GetTemplateNotification(String moduleCode, long idAction, NotificationMode mode,NotificationChannel channel, Int32 idCommunity = -1, ModuleObject obj = null)
            {
                CommonNotificationSettings notification = null;

                List<CommonNotificationSettings> availableNotifications = new List<CommonNotificationSettings>();
                if (obj==null)
                    availableNotifications = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                                where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == moduleCode && n.Settings.IdModuleAction == idAction
                                                && n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != NotificationActionType.Ignore && n.Settings.ActionType != NotificationActionType.None
                                                &&  (n.ObjectOwner == null || (idCommunity != -1 && (idCommunity == n.IdCommunity)) || n.IsForPortal )
                                                select n).ToList();
                else
                    availableNotifications = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                              where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == moduleCode && n.Settings.IdModuleAction == idAction
                                              && n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != NotificationActionType.Ignore && n.Settings.ActionType != NotificationActionType.None
                                              && (
                                              (n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == obj.ObjectLongID && n.ObjectOwner.ObjectTypeID == obj.ObjectTypeID && n.ObjectOwner.ServiceCode == obj.ServiceCode)
                                              ||
                                              (idCommunity != -1 && (idCommunity == n.IdCommunity))
                                              ||
                                              n.IsForPortal
                                              )
                                              select n).ToList();
                if (availableNotifications.Any() && availableNotifications.Where(n=>n.IsValid() && n.IsEnabled ).Any()){
                    availableNotifications = availableNotifications.Where(n=>n.IsValid() && n.IsEnabled).ToList();
                    notification = availableNotifications.Where(n => n.ObjectOwner != null && n.Settings.ActionType == NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                    if (notification==null)
                        notification = availableNotifications.Where(n => n.IdCommunity > 0 && n.Settings.ActionType == NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                    if (notification == null)
                        notification = availableNotifications.Where(n => n.IsForPortal).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                }
                if (notification==null)
                    return null;
                else if (notification.Template != null) {
                    if (notification.Version != null)
                        return notification.Version;
                    else
                        return notification.Template.GetActiveVersion();
                }
                else
                    return null;
            }
        #endregion

        #region "Get Templates"
            public List<dtoTemplateItem> GetAvailableTemplates(long idAction, Int32 idCommunity, Int32 idModule = 0, String moduleCode = "", lm.Comol.Core.DomainModel.ModuleObject obj = null,NotificationChannel channel = NotificationChannel.Mail, long idTemplate = 0, long idVersion = 0)
            {
                List<dtoTemplateItem> items = new List<dtoTemplateItem>();

                try
                {
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (idModule > 0 && String.IsNullOrEmpty(moduleCode))
                        moduleCode = Manager.GetModuleCode(idModule);
                    else if (idModule == 0 && !String.IsNullOrEmpty(moduleCode))
                        idModule = Manager.GetModuleID(moduleCode);

                    List<CommonNotificationSettings> settings = null;
                    if (obj==null)
                        settings = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                                    where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == moduleCode && n.Settings.IdModuleAction == idAction
                                                    && n.Settings.Channel == channel && n.Settings.Mode ==  NotificationMode.Automatic && n.Settings.ActionType != NotificationActionType.Ignore && n.Settings.ActionType != NotificationActionType.None
                                                    &&  (n.ObjectOwner == null || (idCommunity != -1 && (idCommunity == n.IdCommunity)) || n.IsForPortal )
                                                    select n).ToList();
                    else
                        settings = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                                  where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == moduleCode && n.Settings.IdModuleAction == idAction
                                                  && n.Settings.Channel == channel && n.Settings.Mode == NotificationMode.Automatic && n.Settings.ActionType != NotificationActionType.Ignore && n.Settings.ActionType != NotificationActionType.None
                                                  && (
                                                  (n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == obj.ObjectLongID && n.ObjectOwner.ObjectTypeID == obj.ObjectTypeID && n.ObjectOwner.ServiceCode == obj.ServiceCode)
                                                  ||
                                                  (idCommunity != -1 && (idCommunity == n.IdCommunity))
                                                  ||
                                                  n.IsForPortal
                                                  )
                                                  select n).ToList();
                    items.AddRange(settings.Where(s => s.IsForPortal).Select(s => new dtoTemplateItem() { Id = s.Template.Id,  Level = TemplateLevel.Portal }).ToList());
                    items.AddRange(settings.Where(s => !s.IsForPortal && s.IdCommunity == idCommunity).Select(s => new dtoTemplateItem() { Id = s.Template.Id,  Level = TemplateLevel.Community }).ToList());
                    items.AddRange(settings.Where(s => s.ObjectOwner != null).Select(s => new dtoTemplateItem() { Id = s.Template.Id, Level = TemplateLevel.Object }).ToList());

                    items.ForEach(t => t.Versions = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                        where ( v.Id == idVersion || (v.Deleted == BaseStatusDeleted.None && v.Status != TemplateStatus.Draft )) && v.Template.Id==t.Id 
                                        select new dtoVersionItem(t.Level) {
                                            Id=v.Id,
                                            Status = v.Status,
                                            Number = v.Number,
                                            IdTemplate = v.Template.Id,
                                            Lastmodify= (v.ModifiedOn==null) ? v.CreatedOn: v.ModifiedOn,
                                            IsSelected = (v.Id == idVersion && t.Id == idTemplate)
                                        }
                                    ).ToList().OrderByDescending(v => v.Number).ToList());

                    dtoTemplateItem template = items.Where(t => t.Versions.Count == 0 && t.Id == idTemplate).FirstOrDefault();
                    if (template != null) {
                        dtoVersionItem ver = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                                where (v.Id == idVersion || (v.Deleted == BaseStatusDeleted.None && v.Status!= TemplateStatus.Draft)) && v.Template.Id == template.Id
                                              select new dtoVersionItem(template.Level)
                                              {
                                                        Id = v.Id,
                                                        Status = v.Status,
                                                        Number = v.Number,
                                                        IdTemplate = template.Id,
                                                        Lastmodify = (v.ModifiedOn==null) ? v.CreatedOn: v.ModifiedOn,
                                                        IsSelected = (idVersion == 0 && template.Id == idTemplate)
                                                    }
                                                ).OrderByDescending(v => v.Number).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (ver != null)
                            template.Versions.Add(ver);
                    }

                    DateTime lastVersion = DateTime.Now;
                    items.Where(t => t.Versions.Count > 0).ToList().ForEach(t=> t.Versions.Insert(0,
                        new dtoVersionItem(t.Level)
                        {
                         Id=0,
                         Status = TemplateStatus.Active,
                         Lastmodify= lastVersion,
                         Number=0,
                         IdTemplate = t.Id,
                         IsSelected = (idVersion == 0 && t.Id == idTemplate)
                        }));


                    //Add Selected: Solo se il TEMPLATE indicato NON è stato trovato!
                    if (!items.Where(t=> t.Versions.Where(v=> v.IsSelected==true).Any()).Any() && (idTemplate>0 || idVersion>0)){
                        dtoTemplateItem dtoTemplate = null;
                        dtoVersionItem dtoVersion = null;

                        if (idVersion <= 0) //LAST VERSION! Se non ci sono versioni "Definitive", recupero l'ultima "Deprecata"...
                        {
                            dtoTemplate = (from t in Manager.GetIQ<TemplateDefinition>()
                                     where t.Id== idTemplate
                                     select new dtoTemplateItem()
                                     {
                                         Id= t.Id,
                                         Level = TemplateLevel.Removed,
                                     }).Skip(0).Take(1).ToList().FirstOrDefault();

                            if (dtoTemplate != null)
                            {
                                dtoVersion = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                                     where v.Status!= TemplateStatus.Draft
                                              select new dtoVersionItem(dtoTemplate.Level)
                                              {
                                                          Id = v.Id,
                                                          Status = v.Status,
                                                          Number = v.Number,
                                                          IdTemplate = v.Template.Id,
                                                          Lastmodify = (v.ModifiedOn==null) ? v.CreatedOn: v.ModifiedOn
                                                     }
                                                   ).OrderByDescending(v => v.Number).Skip(0).Take(1).ToList().FirstOrDefault();
                            }
                        }
                        else
                        {
                            dtoVersion = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                           where v.Id== idVersion
                                          select new dtoVersionItem(TemplateLevel.Removed)
                                          {
                                                Id = v.Id,
                                                Status = v.Status,
                                                Number = v.Number,
                                                IdTemplate = v.Template.Id,
                                                Lastmodify = (v.ModifiedOn==null) ? v.CreatedOn: v.ModifiedOn,
                                            }
                                        ).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (dtoVersion != null) {
                                dtoTemplate = (from t in Manager.GetIQ<TemplateDefinition>()
                                     where t.Id== idTemplate 
                                     select new dtoTemplateItem()
                                     {
                                         Id= t.Id,
                                         Level = TemplateLevel.Removed,
                                 }).Skip(0).Take(1).ToList().FirstOrDefault();
                                //Controllo la versione recuperata da dB:
                                // Se NON soddisfa i requisiti, viene impostata a null
                                if (!(dtoTemplate != null && dtoTemplate.Id == dtoVersion.IdTemplate))
                                    dtoVersion = null;
                            }
                        }

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
                            items.Add(dtoTemplate);
                        }
                    }
                    Language l = Manager.GetDefaultLanguage();
                    items.ForEach(t => t.Name = GetTemplateName(t.Id, UC.Language.Id, l.Id));
                }
                catch (Exception ex)
                {
                }

                return items;
            }
            public dtoTemplateItem GetDefaultTemplate(long idAction, Int32 idCommunity, Int32 idModule = 0, String moduleCode = "", lm.Comol.Core.DomainModel.ModuleObject obj = null, long idTemplate = 0, long idVersion = 0, NotificationChannel channel = NotificationChannel.Mail)
            {
                dtoTemplateItem dTemplate = null;

                try
                {
                    Manager.BeginTransaction();
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (idModule > 0 && String.IsNullOrEmpty(moduleCode))
                        moduleCode = Manager.GetModuleCode(idModule);
                    else if (idModule == 0 && !String.IsNullOrEmpty(moduleCode))
                        idModule = Manager.GetModuleID(moduleCode);
                    List<CommonNotificationSettings> settings = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                                                    where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == moduleCode && n.Settings.IdModuleAction == idAction
                                                                    && n.Settings.Channel == channel && n.Settings.Mode ==  NotificationMode.Automatic && n.Settings.ActionType != NotificationActionType.Ignore
                                                                    && n.Settings.ActionType != NotificationActionType.None
                                                            && (
                                                            (obj != null && n.ObjectOwner != null && n.ObjectOwner.ObjectLongID==obj.ObjectLongID && n.ObjectOwner.ObjectTypeID==obj.ObjectTypeID && n.ObjectOwner.ServiceCode==obj.ServiceCode) 
                                                            ||
                                                            (idCommunity != -1 && (idCommunity == n.IdCommunity))
                                                            ||
                                                            n.IsForPortal
                                                            )
                                                            select n).ToList();
                    CommonNotificationSettings notification = null;
                    if (settings.Any() && settings.Where(n => n.IsValid() && n.IsEnabled).Any())
                    {
                        settings = settings.Where(n => n.IsValid() && n.IsEnabled).ToList();
                        notification = settings.Where(n => n.ObjectOwner != null && n.Settings.ActionType == NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                        if (notification == null)
                            notification = settings.Where(n => n.IdCommunity > 0 && n.Settings.ActionType == NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                        if (notification == null)
                            notification = settings.Where(n => n.IsForPortal).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                    }
                    if (notification.Template != null)
                    {
                        Language l = Manager.GetDefaultLanguage();
                        dTemplate = new dtoTemplateItem()
                            {
                                Id = notification.Template.Id,
                                Name = notification.Version.GetName(UC.Language.Id, l.Id),
                                Level = (notification.Template.Type == TemplateType.System) ? TemplateLevel.Portal : ((notification.Template.Type == TemplateType.User) ? TemplateLevel.Personal : ((notification.Template.OwnerInfo.Type == OwnerType.Object) ? TemplateLevel.Object : TemplateLevel.Community))
                            };
                        TemplateDefinitionVersion v = (notification.Version!=null) ? notification.Version : notification.Template.GetActiveVersion();
                        if (v!=null){
                            dTemplate.Versions.Add(new dtoVersionItem(dTemplate.Level ) { Status = v.Status, Id = v.Id, IdTemplate = dTemplate.Id, Number = v.Number, IsSelected = (notification.Version != null), Lastmodify = notification.Version.ModifiedOn });
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                return dTemplate;
            }

           
            public dtoTemplateItem GetDefaultAutomaticTemplate(dtoSelectorContext context, NotificationChannel channel = NotificationChannel.Mail)
            {
                return GetDefaultTemplate(context,channel, NotificationMode.Automatic);
            }
            private dtoTemplateItem GetDefaultTemplate(dtoSelectorContext context, NotificationChannel channel = NotificationChannel.Mail,NotificationMode mode = NotificationMode.Automatic)
            {
                dtoTemplateItem dTemplate = null;

                try
                {
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    var query = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                 where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == context.ModuleCode && n.Settings.IdModuleAction == context.IdAction
                                    && n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != NotificationActionType.Ignore
                                    && n.Settings.ActionType != NotificationActionType.None
                                 select n);
                    if (context.ObjectOwner != null)
                        query = query.Where(n =>
                                            (context.ObjectOwner != null && n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == context.ObjectOwner.ObjectLongID && n.ObjectOwner.ObjectTypeID == context.ObjectOwner.ObjectTypeID && n.ObjectOwner.ServiceCode == context.ObjectOwner.ServiceCode)
                                            ||
                                            (context.IdCommunity != -1 && (context.IdCommunity == n.IdCommunity))
                                            ||
                                            (context.IdOrganization != -1 && (context.IdOrganization == n.IdOrganization))
                                            ||
                                            n.IsForPortal
                                            );
                    else
                         query = query.Where(n =>
                                            n.ObjectOwner == null && 
                                            (
                                                (context.IdCommunity != -1 && (context.IdCommunity == n.IdCommunity))
                                                ||
                                                (context.IdOrganization != -1 && (context.IdOrganization == n.IdOrganization))
                                                ||
                                                n.IsForPortal
                                            ));
                    List<CommonNotificationSettings> settings = query.ToList();
                    CommonNotificationSettings notification = null;
                    if (settings.Any() && settings.Where(n => n.IsValid() && n.IsEnabled).Any())
                    {
                        settings = settings.Where(n => n.IsValid() && n.IsEnabled).ToList();
                        notification = settings.Where(n => n.ObjectOwner != null && n.Settings.ActionType == NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                        if (notification == null)
                            notification = settings.Where(n => n.IdCommunity > 0 && n.Settings.ActionType == NotificationActionType.ByTemplate).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                        if (notification == null)
                            notification = settings.Where(n => n.IsForPortal).OrderByDescending(n => n.ModifiedOn).FirstOrDefault();
                    }
                    if (notification.Template != null)
                    {
                        Language l = Manager.GetDefaultLanguage();
                        dTemplate = new dtoTemplateItem()
                        {
                            Id = notification.Template.Id,
                            Name = notification.Version.GetName(UC.Language.Id, l.Id),
                            Level = (notification.Template.Type == TemplateType.System) ? TemplateLevel.Portal : ((notification.Template.Type == TemplateType.User) ? TemplateLevel.Personal : ((notification.Template.OwnerInfo.Type == OwnerType.Object) ? TemplateLevel.Object : TemplateLevel.Community))
                        };
                        TemplateDefinitionVersion v = (notification.Version != null) ? notification.Version : notification.Template.GetActiveVersion();
                        if (v != null)
                        {
                            dTemplate.Versions.Add(new dtoVersionItem(dTemplate.Level) { Status = v.Status, Id = v.Id, IdTemplate = dTemplate.Id, Number = v.Number, IsSelected = (notification.Version != null), Lastmodify = notification.Version.ModifiedOn });
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                return dTemplate;
            }
            private IQueryable<CommonNotificationSettings> CommonNotificationQuery(dtoSelectorContext context, NotificationChannel channel = NotificationChannel.Mail, NotificationMode mode = NotificationMode.Automatic)
            {
                var query = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == context.ModuleCode && n.Settings.IdModuleAction == context.IdAction
                                && n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != NotificationActionType.Ignore
                                && n.Settings.ActionType != NotificationActionType.None
                                select n);
                if (context.ObjectOwner != null)
                    query = query.Where(n =>
                                        (context.ObjectOwner != null && n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == context.ObjectOwner.ObjectLongID && n.ObjectOwner.ObjectTypeID == context.ObjectOwner.ObjectTypeID && n.ObjectOwner.ServiceCode == context.ObjectOwner.ServiceCode)
                                        ||
                                        (context.IdCommunity != -1 && (context.IdCommunity == n.IdCommunity))
                                        ||
                                        (context.IdOrganization != -1 && (context.IdOrganization == n.IdOrganization))
                                        ||
                                        n.IsForPortal
                                        );
                else
                        query = query.Where(n =>
                                        n.ObjectOwner == null && 
                                        (
                                            (context.IdCommunity != -1 && (context.IdCommunity == n.IdCommunity))
                                            ||
                                            (context.IdOrganization != -1 && (context.IdOrganization == n.IdOrganization))
                                            ||
                                            n.IsForPortal
                                        ));
                return query;
            }
            
            public String GetTemplateName(long idTemplate, Int32 idUserLanguage, Int32 idDefaultLanguage)
            {
                String name = "";
                try
                {
                    name = Manager.Get<TemplateDefinition>(idTemplate).LastVersion.GetName(idUserLanguage, idDefaultLanguage);
                }
                catch (Exception ex) { 
                
                }
                return name;
            }

            #region "For Notification Selection"
                /// <summary>
                /// Retrieve notification settings 
                /// </summary>
                /// <param name="obj"></param>
                /// <param name="channel">Trasmission Channel</param>
                /// <param name="mode">Notification mode: autoamtic/manual/scheduling</param>
                /// <returns></returns>
                /// 
                public List<CommonNotificationSettings> GetNotificationSettings(Boolean forPortal, Int32 idCommunity=0, Int32 idOrganization = 0, ModuleObject obj=null, NotificationChannel channel = NotificationChannel.Mail, NotificationMode mode = NotificationMode.Automatic)
                {
                    List<CommonNotificationSettings> settings = null;

                    try
                    {
                        if (obj == null)
                            settings = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                        where n.Deleted == BaseStatusDeleted.None
                                        && n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != NotificationActionType.Ignore && n.Settings.ActionType != NotificationActionType.None
                                        && (n.ObjectOwner == null || (idCommunity != -1 && (idCommunity == n.IdCommunity)) || (idOrganization != -1 && idOrganization == n.IdOrganization) || n.IsForPortal)
                                        select n).ToList();
                        else
                            settings = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                        where n.Deleted == BaseStatusDeleted.None 
                                        && n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != NotificationActionType.Ignore && n.Settings.ActionType != NotificationActionType.None
                                        && (
                                        (n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == obj.ObjectLongID && n.ObjectOwner.ObjectTypeID == obj.ObjectTypeID && n.ObjectOwner.ServiceCode == obj.ServiceCode)
                                        || (idCommunity != -1 && (idCommunity == n.IdCommunity)) || (idOrganization != -1 && idOrganization == n.IdOrganization) || n.IsForPortal
                                        )
                                        select n).ToList();
                    }
                    catch (Exception ex)
                    {
                    }
                    return settings;
                }

                public List<dtoTemplateItem> GetAvailableTemplates(ModuleGenericTemplateMessages permissions, dtoSelectorContext sContext, long idTemplate = 0, long idVersion = 0, NotificationChannel channel = NotificationChannel.Mail, NotificationMode mode = NotificationMode.Automatic)
                {
                    List<dtoTemplateItem> items = new List<dtoTemplateItem>();

                    try
                    {
                        Person person = Manager.GetPerson(UC.CurrentUserID);
                        List<CommonNotificationSettings> settings = null;
                        if (sContext.ObjectOwner == null)
                            settings = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                        where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == sContext.ModuleCode && n.Settings.IdModuleAction == sContext.IdAction
                                        && n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != NotificationActionType.Ignore && n.Settings.ActionType != NotificationActionType.None
                                        && (n.ObjectOwner == null || (sContext.IdCommunity != -1 && (sContext.IdCommunity == n.IdCommunity)) || (sContext.IdOrganization != -1 && sContext.IdCommunity == n.IdCommunity) || n.IsForPortal)
                                        select n).ToList();
                        else
                            settings = (from n in Manager.GetIQ<CommonNotificationSettings>()
                                        where n.Deleted == BaseStatusDeleted.None && n.Settings.ModuleCode == sContext.ModuleCode && n.Settings.IdModuleAction == sContext.IdAction
                                        && n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != NotificationActionType.Ignore && n.Settings.ActionType != NotificationActionType.None
                                        && (
                                        (n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == sContext.ObjectOwner.ObjectLongID && n.ObjectOwner.ObjectTypeID == sContext.ObjectOwner.ObjectTypeID && n.ObjectOwner.ServiceCode == sContext.ObjectOwner.ServiceCode)
                                        || (sContext.IdCommunity != -1 && (sContext.IdCommunity == n.IdCommunity)) || (sContext.IdOrganization != -1 && sContext.IdCommunity == n.IdCommunity) || n.IsForPortal
                                        )
                                        select n).ToList();

                        List<long> idTemplates = settings.Select(s => s.Template.Id).ToList();
                        List<TemplateDefinition> templates = GetTemplatesForSelection(sContext,permissions, person, idTemplates);
                        Int32 idOrganizationCommunity = (from c in Manager.GetIQ<Community>() where c.IdOrganization == sContext.IdOrganization && c.IdFather == 0 select c.Id).Skip(0).Take(1).ToList().FirstOrDefault();


                        items.AddRange(settings.Where(s => s.IsForPortal).Select(s => new dtoTemplateItem() { Id = s.Template.Id, Level = TemplateLevel.Portal }).ToList());
                        items.AddRange(templates.Where(t=>t.Type== TemplateType.System).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Portal }).ToList());

                        items.AddRange(templates.Where(t => t.Type != TemplateType.System && t.OwnerInfo.Community != null && idOrganizationCommunity>0 && t.OwnerInfo.Community.Id== idOrganizationCommunity).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Organization }).ToList());

                        items.AddRange(settings.Where(s => !s.IsForPortal && s.IdCommunity == sContext.IdCommunity).Select(s => new dtoTemplateItem() { Id = s.Template.Id, Level = TemplateLevel.Community }).ToList());
                        items.AddRange(templates.Where(t => t.Type != TemplateType.System && t.OwnerInfo.Community != null && t.OwnerInfo.Community.Id == sContext.IdCommunity && sContext.IdCommunity != idOrganizationCommunity).Select(s => new dtoTemplateItem() { Id = s.Id, Level = TemplateLevel.Community }).ToList());
                        items.AddRange(settings.Where(s => s.ObjectOwner != null).Select(s => new dtoTemplateItem() { Id = s.Template.Id, Level = TemplateLevel.Object }).ToList());

                        items = ParseAvailableTemplates(items, Manager.GetDefaultLanguage(), idTemplate, idVersion);
                    }
                    catch (Exception ex)
                    {
                    }

                    return items;
                }

                public List<dtoTemplateItem> ParseAvailableTemplates(List<dtoTemplateItem> items, Language dLanguage, long idTemplate = 0, long idVersion = 0, Boolean removeReplaced = false)
                {
                    try
                    {
                        if (items != null && items.Any())
                        {
                            items.ForEach(t => t.Versions = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                                             where (v.Id == idVersion || (v.Deleted == BaseStatusDeleted.None && v.Status != TemplateStatus.Draft)) && v.Template.Id == t.Id
                                                             select new dtoVersionItem(t.Level)
                                                             {
                                                                 Id = v.Id,
                                                                 Status = v.Status,
                                                                 Number = v.Number,
                                                                 IdTemplate = v.Template.Id,
                                                                 Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn,
                                                                 IsSelected = (v.Id == idVersion && t.Id == idTemplate)
                                                             }
                                            ).ToList().OrderByDescending(v => v.Number).ToList());

                            dtoTemplateItem template = items.Where(t => t.Versions.Count == 0 && t.Id == idTemplate).FirstOrDefault();
                            if (template != null)
                            {
                                dtoVersionItem ver = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                                      where (v.Id == idVersion || (v.Deleted == BaseStatusDeleted.None && v.Status != TemplateStatus.Draft)) && v.Template.Id == template.Id
                                                      select new dtoVersionItem(template.Level )
                                                      {
                                                          Id = v.Id,
                                                          Status = v.Status,
                                                          Number = v.Number,
                                                          IdTemplate = template.Id,
                                                          Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn,
                                                          IsSelected = (idVersion == 0 && template.Id == idTemplate)
                                                      }
                                                        ).ToList().OrderByDescending(v => v.Number).Skip(0).Take(1).ToList().FirstOrDefault();
                                if (ver != null)
                                    template.Versions.Add(ver);
                            }

                            DateTime lastVersion = DateTime.Now;
                            items.Where(t => t.Versions.Count > 0).ToList().ForEach(t => t.Versions.Insert(0,
                                new dtoVersionItem(t.Level )
                                {
                                    Id = 0,
                                    Status = TemplateStatus.Active,
                                    Lastmodify = lastVersion,
                                    Number = 0,
                                    IdTemplate = t.Id,
                                    IsSelected = (idVersion == 0 && t.Id == idTemplate)
                                }));


                            //Add Selected: Solo se il TEMPLATE indicato NON è stato trovato!
                            if (!items.Where(t => t.Versions.Where(v => v.IsSelected == true).Any()).Any() && (idTemplate > 0 || idVersion > 0))
                            {
                                dtoTemplateItem dtoTemplate = null;
                                dtoVersionItem dtoVersion = null;

                                if (idVersion <= 0) //LAST VERSION! Se non ci sono versioni "Definitive", recupero l'ultima "Deprecata"...
                                {
                                    dtoTemplate = (from t in Manager.GetIQ<TemplateDefinition>()
                                                   where t.Id == idTemplate
                                                   select new dtoTemplateItem()
                                                   {
                                                       Id = t.Id,
                                                       Level =  TemplateLevel.Removed
                                                   }).Skip(0).Take(1).ToList().FirstOrDefault();

                                    if (dtoTemplate != null)
                                    {
                                        dtoVersion = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                                      where v.Deleted == BaseStatusDeleted.None && v.Status != TemplateStatus.Draft && v.Template.Id == dtoTemplate.Id 
                                                      select new dtoVersionItem()
                                                      {
                                                          Id = v.Id,
                                                          Status = v.Status,
                                                          Number = v.Number,
                                                          IdTemplate = v.Template.Id,
                                                          Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn
                                                      }
                                                            ).ToList().OrderByDescending(v => v.Number).Skip(0).Take(1).ToList().FirstOrDefault();
                                    }
                                }
                                else
                                {
                                    dtoVersion = (from v in Manager.GetIQ<TemplateDefinitionVersion>()
                                                  where v.Id == idVersion
                                                  select new dtoVersionItem(TemplateLevel.Removed)
                                                  {
                                                      Id = v.Id,
                                                      Status = v.Status,
                                                      Number = v.Number,
                                                      IdTemplate = v.Template.Id,
                                                      Lastmodify = (v.ModifiedOn == null) ? v.CreatedOn : v.ModifiedOn,
                                                      IsSelected=true 
                                                  }
                                                ).Skip(0).Take(1).ToList().FirstOrDefault();
                                    if (dtoVersion != null)
                                    {
                                        dtoTemplate = (from t in Manager.GetIQ<TemplateDefinition>()
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
                                    items.Add(dtoTemplate);
                                }
                            }
                            items = items.Where(i => i.Versions.Any()).ToList();
                            if (removeReplaced)
                                items.ForEach(i => i.Versions = i.Versions.Where(v => !(v.IsSelected == false && v.Status == TemplateStatus.Replaced)).ToList());
                            items.ForEach(t => t.Name = GetTemplateName(t.Id, UC.Language.Id, dLanguage.Id));
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    return items;
                }
                public List<TemplateDefinition> GetTemplatesForSelection(dtoSelectorContext context, ModuleGenericTemplateMessages permissions, Person person, List<long> removeIdTemplates)
                {
                    List<TemplateDefinition> items = new List<TemplateDefinition>();
                    try
                    {
                        List<TemplateDefinition> templates = GetTemplatesForSelection(context,  person).Where(t=>!removeIdTemplates.Contains(t.Id)).ToList();
                        if (context.IdAction > 0)
                        {
                            var query = (from ns in Manager.GetIQ<CommonNotificationSettings>()
                                         where ns.Deleted == BaseStatusDeleted.None && ((ns.Settings.ModuleCode == context.ModuleCode && !String.IsNullOrEmpty(context.ModuleCode)) || (ns.Settings.IdModule == context.IdModule && context.IdModule > 0))
                                         select ns);
                            //List<long> tActions = query.Where(ns => ns.Settings.IdModuleAction == context.IdAction).Select(ns => ns.Template.Id).ToList();
                            List<long> eActions = new List<long>();
                            //if (context.AlsoEmptyActions)
                                eActions = query.Where(ns => ns.Settings.IdModuleAction != context.IdAction).Select(ns => ns.Template.Id).ToList();
                            //templates = templates.Where(t => tActions.Contains(t.Id) || (context.AlsoEmptyActions && !eActions.Contains(t.Id))).ToList();
                            templates = templates.Where(t =>!eActions.Contains(t.Id)).ToList();
                        }

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
                //public enum OwnerType
                //{
                //    None = 0,
                //    System = 1,
                //    Module = 2,
                //    Community = 3,
                //    Person = 4,
                //    Object = 5
                //}
                public List<TemplateDefinition> GetTemplatesForSelection(dtoSelectorContext context, Person person)
                {
                    List<TemplateDefinition> templates = new List<TemplateDefinition>();
                    var query = GetBaseTemplatesQuery(context);
                    templates.AddRange(query.ToList());

                    // Devo aggiungere i template associati a me come persona/ come profilo / come ruolo
                    templates.AddRange(GetAssignedByProfile(templates.Select(t => t.Id).ToList(), context, person));
                    templates.AddRange(GetAssignedByRoles(templates.Select(t => t.Id).ToList(), context, person));

                    return templates;
                }
            #endregion

            #region "Selection"
                public List<TemplateDefinition> GetTemplatesForNotification(dtoSelectorContext context, Person person)
                {
                    List<TemplateDefinition> templates = new List<TemplateDefinition>();
                    var query = GetBaseTemplatesQuery(context);
                    templates.AddRange(query.ToList());

                    templates.AddRange(GetAssignedByProfile(templates.Select(t => t.Id).ToList(), context, person));
                    templates.AddRange(GetAssignedByRoles(templates.Select(t => t.Id).ToList(), context, person));

                    return templates;
                }

                protected List<TemplateDefinition> GetAssignedByProfile(List<long> idTemplates, dtoSelectorContext context, Person person)
                {
                    List<TemplateDefinition> results = new List<TemplateDefinition>();
                    var query = GetBaseTemplatesQuery(context);

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
                protected List<TemplateDefinition> GetAssignedByRoles(List<long> idTemplates, dtoSelectorContext context, Person person)
                {
                    List<TemplateDefinition> results = new List<TemplateDefinition>();

                    List<VersionRolePermission> permissions = GetVersionRolePermissionQuery(context).Where(p => !idTemplates.Contains(p.Template.Id)).ToList();
                    List<int> idCommunities = permissions.Where(p => p.Community != null && p.AssignedTo != null).Select(i => i.Community.Id).ToList();
                    List<LazySubscription> subscriptions = Manager.GetBaseActiveSubscriptions(person.Id, idCommunities);

                    List<long> idQueryTemplates = (from p in permissions
                                                   where subscriptions.Where(s => s.IdCommunity == p.Community.Id && s.IdRole == p.AssignedTo.Id).Any()
                                                   select p.Version.Template.Id).ToList();

                    Int32 pageIndex = 0;
                    List<long> idPagedTemplates = idQueryTemplates.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idPagedTemplates.Any())
                    {
                        pageIndex++;
                        results.AddRange((from t in Manager.GetIQ<TemplateDefinition>() where idPagedTemplates.Contains(t.Id) select t).ToList());
                        idPagedTemplates = idQueryTemplates.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return results;
                }
                private IQueryable<TemplateDefinition> GetBaseTemplatesQuery(dtoSelectorContext context)
                {
                    return (from t in Manager.Linq<TemplateDefinition>()
                            where t.Deleted == BaseStatusDeleted.None && t.CurrentModulesContent.Contains("#" + context.ModuleCode + "#")
                             &&
                             (
                             (context.IsForPortal && t.Type == TemplateType.System)
                             ||
                             (!context.IsForPortal && (t.Type == TemplateType.Module && t.OwnerInfo.Type == OwnerType.Module || t.OwnerInfo.Type == OwnerType.Community)
                                 &&
                               (
                                    (context.IdOrganizationCommunity > 0 && t.OwnerInfo.Community != null && t.OwnerInfo.Community.Id == context.IdOrganizationCommunity)
                                    ||
                                    (context.IdCommunity > 0 && t.OwnerInfo.Community != null && t.OwnerInfo.Community.Id == context.IdCommunity)
                                )
                             ))
                            select t);
                }
                private IQueryable<VersionRolePermission> GetVersionRolePermissionQuery(dtoSelectorContext context)
                {
                    var query = (from t in Manager.Linq<VersionRolePermission>()
                                 where t.ToApply == true && t.Deleted == BaseStatusDeleted.None && t.Template.Deleted == BaseStatusDeleted.None
                                 select t).ToList().AsQueryable();
                    query = query.Where(t => (t.Template.Type == TemplateType.System && t.Template.CurrentModulesContent.Contains("#" + context.ModuleCode + "#")) || (
                                        !context.IsForPortal && t.Template.Type == TemplateType.Module
                                        && t.Template.OwnerInfo.ModuleCode == context.ModuleCode
                                        &&
                                        (
                                            (context.IdOrganizationCommunity == -1 || (t.Template.OwnerInfo.Community != null && t.Template.OwnerInfo.Community.Id == context.IdOrganizationCommunity))
                                            ||
                                            (context.IdCommunity == -1 || (context.IdCommunity > 0 && t.Template.OwnerInfo.Community != null && t.Template.OwnerInfo.Community.Id == context.IdCommunity))
                                        )
                                        ));
                    return query;
                }

            #endregion

        #endregion
        
        #region "Send Message"
                /// <summary>
            /// 
            /// </summary>
            /// <param name="sender">Il mittente</param>
            /// <param name="smtpConfig">La configurazione SMTP, dal presenter con SystemSettings.</param>
            /// <param name="mailSettings">Impostazioni per l'invio recuperte</param>
            /// <param name="subject">Oggetto mail</param>
            /// <param name="body">Corpo della mail</param>
            /// <param name="addresses">Destinatari, lista di stringe separata da ";"</param>
            /// <returns></returns>
            public Boolean SendMail(Person sender, SmtpServiceConfig smtpConfig, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mailSettings, String subject, String body, String addresses)
            {
                return SendMail(sender, smtpConfig, mailSettings, subject, body, GetRecipients(addresses));
            }
            public Boolean SendMail(Person sender, SmtpServiceConfig smtpConfig, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mailSettings, String subject, String body, List<lm.Comol.Core.Mail.dtoRecipient> recipients)
            {
                Boolean sent = false;
                try
                {
                    lm.Comol.Core.Mail.MailService mailService = new lm.Comol.Core.Mail.MailService(smtpConfig, mailSettings);
                    lm.Comol.Core.Mail.dtoMailMessage message = new lm.Comol.Core.Mail.dtoMailMessage(subject, body);
                    message.FromUser = (sender == null) ? smtpConfig.GetSystemSender() : new System.Net.Mail.MailAddress(sender.Mail, sender.SurnameAndName);
                    message.AddAddresses(recipients, RecipientType.BCC);
                    //message.FromUser = smtpConfig.System.Sender;
                    lm.Comol.Core.Mail.MailException result = mailService.SendMail((sender==null) ? 0 : sender.LanguageID, Manager.GetDefaultLanguage(), message);
                    sent = (result == Mail.MailException.MailSent);
                }
                catch (Exception ex) { 
                
                }
                
                return sent;
            }

            public Boolean SendMail(Person sender, SmtpServiceConfig smtpConfig, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mailSettings, List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> messages, ref List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> skippedItems, lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = null, ModuleObject obj = null, String moduleCode = "", Int32 idModule = 0, Int32 idCommunity = -1, Boolean isPortal = false)
            {
                Int32 count = (messages == null || !messages.Any()) ? -1 : messages.Count;
                try
                {
                    lm.Comol.Core.Mail.MailService mailService = new lm.Comol.Core.Mail.MailService(smtpConfig, mailSettings);
                    Language dLanguage = Manager.GetDefaultLanguage();
                    foreach (lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage m in messages) {
                        if (m.Recipients.Any())
                        {
                            lm.Comol.Core.Mail.dtoMailMessage message = new lm.Comol.Core.Mail.dtoMailMessage(m.Subject, m.Body);
                            message.FromUser = (sender == null) ? smtpConfig.GetSystemSender() : new System.Net.Mail.MailAddress(sender.Mail, sender.SurnameAndName);
                            if (m.Recipients.Where(r => r.Type == RecipientType.To).Any())
                                message.AddAddresses(m.Recipients.Where(r => r.Type == RecipientType.To).Select(r => r.ToRecipient()).ToList(), RecipientType.To);
                            if (m.Recipients.Where(r => r.Type == RecipientType.CC).Any())
                                message.AddAddresses(m.Recipients.Where(r => r.Type == RecipientType.CC).Select(r => r.ToRecipient()).ToList(), RecipientType.CC);
                            if (m.Recipients.Where(r => r.Type == RecipientType.BCC).Any())
                                message.AddAddresses(m.Recipients.Where(r => r.Type == RecipientType.BCC).Select(r => r.ToRecipient()).ToList(), RecipientType.BCC);
                            //message.FromUser = smtpConfig.System.Sender;
                            lm.Comol.Core.Mail.MailException result = mailService.SendMail(m.IdLanguage, dLanguage,message);
                            if (result == Mail.MailException.MailSent)
                            {
                                count--;
                                m.Sent = true;
                            }
                            else
                            {
                                m.Sent = false;
                                skippedItems.AddRange(m.Recipients);
                            }
                        }
                        else{
                            count--;
                            m.Sent=false;
                        }
                    }
                    MessageService.SaveMessage(messages,smtpConfig, mailSettings, template,obj,moduleCode,idModule,idCommunity, isPortal );
                }
                catch (Exception ex)
                {

                }
                return (count == -1 || count==0);
            }
            private List<lm.Comol.Core.Mail.dtoRecipient> GetRecipients(String addresses)
            {
                List<String> recipients = new List<String>();
                Char splitChar = ' ';
                if (addresses.Contains(";"))
                    splitChar = ';';
                else if (addresses.Contains(","))
                    splitChar = ',';

                
                if (splitChar == ' ')
                    recipients.Add(addresses);
                else
                    recipients.AddRange(addresses.Split(splitChar).Where(s => !string.IsNullOrEmpty(s)).ToList());
                return recipients.Select(r => new lm.Comol.Core.Mail.dtoRecipient() { MailAddress = r }).ToList();
            }
        #endregion


        #region "Service Module Skins"
        //public Domain.Skin AddSkin(String name, String path, lm.Comol.Core.DomainModel.ModuleObject owner)
            //{
            //    Domain.Skin skin = null;
            //    try
            //    {
            //        Manager.BeginTransaction();
            //        skin = new Domain.Skin();
            //        skin.IsModule = (owner != null);
            //        skin.Name = name;
            //        Person user = Manager.Get<Person>(UC.CurrentUserID);
            //        skin.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            //        Manager.SaveOrUpdate<Domain.Skin>(skin);
            //        if (owner != null)
            //        {
            //            ModuleAssociation association = AddModuleAssociation(user, new DtoDisplaySkin() { Id = skin.Id, Type = SkinDisplayType.Module }, owner.CommunityID, owner);
            //            AddModuleShare(user, owner.CommunityID, association, owner, SkinDisplayType.Module);
            //        }

            //        Manager.Commit();
            //        SkinFileManagement.CreateDir(skin.Id, path);
            //    }
            //    catch (Exception ex)
            //    {
            //        if (Manager.IsInTransaction())
            //            Manager.RollBack();
            //        if (skin.Id > 0)
            //        {
            //            DELETE_Skin(skin.Id, path);
            //            skin = null;
            //        }
            //    }
            //    return skin;
            //}
            //public Domain.Skin SaveSkin(long idSKin, String name)
            //{
            //    Domain.Skin skin = null;
            //    try
            //    {
            //        Manager.BeginTransaction();
            //        skin = Manager.Get<Domain.Skin>(idSKin);
            //        if (skin != null)
            //        {
            //            skin.Name = name;
            //            Person user = Manager.Get<Person>(UC.CurrentUserID);
            //            skin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            //            Manager.SaveOrUpdate<Domain.Skin>(skin);
            //        }
            //        Manager.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        if (Manager.IsInTransaction())
            //            Manager.RollBack();
            //        skin = null;
            //    }
            //    return skin;
            //}
            //public lm.Comol.Core.DomainModel.Helpers.dtoItemSkin GetModuleSkin(lm.Comol.Core.DomainModel.ModuleObject source, lm.Comol.Core.DomainModel.Helpers.dtoItemSkin starter)
            //{
            //    try
            //    {
            //        List<Skin.Domain.Skin> items = (from m in Manager.GetIQ<Skin.Domain.Skin_ShareModule>()
            //                                        where m.IdModule == source.ServiceID && m.OwnerLongID == source.ObjectLongID && m.OwnerTypeID == source.ObjectTypeID
            //                                        && m.Deleted == BaseStatusDeleted.None && m.Skin != null
            //                                        select m.Skin).ToList();

            //        Skin.Domain.Skin skin = items.OrderByDescending(s => s.ModifiedOn).Where(s => s.Deleted == BaseStatusDeleted.None).FirstOrDefault();
            //        if (skin == null)
            //        {
            //            ModuleAssociation association = (from ma in Manager.GetIQ<ModuleAssociation>()
            //                                             where ma.IdModule == source.ServiceID && ma.OwnerLongID == source.ObjectLongID && ma.OwnerTypeID == source.ObjectTypeID && ma.Deleted == Core.DomainModel.BaseStatusDeleted.None
            //                                             select ma).Skip(0).Take(1).ToList().FirstOrDefault();
            //            if (association != null)
            //            {
            //                starter.IsForPortal = association.IsPortal;
            //                starter.IdOrganization = association.IdOrganization;
            //                starter.IdCommunity = association.IdCommunity;
            //            }
            //        }
            //        else
            //            starter.IdSkin = skin.Id;
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    return starter;
            //}

           
            //private List<DtoDisplaySkin> GetModuleSkins(Person person, Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, LoadItemsBy loadBy)
            //{
            //    List<DtoDisplaySkin> items = new List<DtoDisplaySkin>();
            //    try
            //    {
            //        var query = (from sm in Manager.GetIQ<Skin_ShareModule>()
            //                     where sm.Deleted == Core.DomainModel.BaseStatusDeleted.None
            //                     select sm);
            //        if ((loadBy & LoadItemsBy.Creator) > 0)
            //            query = query.Where(m => m.CreatedBy == person);
            //        if ((loadBy & LoadItemsBy.Community) > 0)
            //            query = query.Where(m => m.IdCommunity == idCommunity);
            //        if ((loadBy & LoadItemsBy.Module) > 0)
            //            query = query.Where(m => m.IdModule == idModule);
            //        if ((loadBy & LoadItemsBy.Object) > 0)
            //            query = query.Where(m => m.OwnerLongID == idModuleItem && m.OwnerTypeID == idItemType);
            //        List<long> idSKins = query.Select(m => m.Skin.Id).ToList();

            //        items.AddRange((from s in Manager.GetIQ<Domain.Skin>()
            //                        where s.Deleted == Core.DomainModel.BaseStatusDeleted.None && (idSKins.Contains(s.Id) || s.IsModule)
            //                        orderby s.Name
            //                        select new DtoDisplaySkin() { Id = s.Id, Name = s.Name, Type = SkinDisplayType.Module, IsValid = true }).ToList());

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    return items;
            //}

            //public DtoDisplaySkin GetDefaultSkinForModule(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType)
            //{
            //    DtoDisplaySkin result = null;
            //    try
            //    {
            //        ModuleAssociation assigned = (from m in Manager.GetIQ<ModuleAssociation>()
            //                                      where m.Deleted == Core.DomainModel.BaseStatusDeleted.None && m.OwnerTypeID == idItemType && m.OwnerLongID == idModuleItem
            //                                      && m.IdModule == idModule
            //                                      select m).Skip(0).Take(1).ToList().FirstOrDefault();
            //        if (assigned != null)
            //        {
            //            if (assigned.IdSkin > 0)
            //                result = new DtoDisplaySkin() { Type = SkinDisplayType.Module, Id = assigned.IdSkin };
            //            else if (assigned.IdOrganization > 0 && assigned.IdCommunity > 0)
            //                result = new DtoDisplaySkin() { Type = SkinDisplayType.Community, Id = assigned.IdCommunity };
            //            else if (assigned.IdOrganization > 0)
            //                result = new DtoDisplaySkin() { Type = SkinDisplayType.Organization, Id = assigned.IdOrganization };
            //        }
            //    }
            //    catch (Exception es)
            //    {

            //    }
            //    return result;
            //}

            //public Boolean SaveSkinAssociation(DtoDisplaySkin skin, Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source)
            //{
            //    Boolean result = false;
            //    try
            //    {
            //        Manager.BeginTransaction();
            //        Person person = Manager.GetPerson(UC.CurrentUserID);
            //        if (skin.Type == SkinDisplayType.Empty)
            //        {
            //            DeleteSkinAssociation(person, idCommunity, source, false);
            //            DeleteModuleShare(person, idCommunity, source, false, false);
            //        }
            //        else
            //        {
            //            ModuleAssociation association = AddModuleAssociation(person, skin, idCommunity, source);
            //            Domain.Skin_ShareModule share = AddModuleShare(person, idCommunity, association, source, skin.Type);
            //            if (association != null)
            //                Manager.SaveOrUpdate(association);
            //            if (share != null)
            //                Manager.SaveOrUpdate(share);
            //        }
            //        Manager.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        result = false;
            //        Manager.RollBack();
            //    }
            //    return result;
            //}
            //private ModuleAssociation AddModuleAssociation(Person person, DtoDisplaySkin skin, Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source)
            //{
            //    ModuleAssociation association = (from ma in Manager.GetIQ<ModuleAssociation>()
            //                                     where ma.IdModule == source.ServiceID && ma.OwnerLongID == source.ObjectLongID && ma.OwnerTypeID == source.ObjectTypeID
            //                                     select ma).Skip(0).Take(1).ToList().FirstOrDefault();
            //    if (association == null)
            //    {
            //        association = new ModuleAssociation();
            //        association.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            //        association.IdCommunity = idCommunity;
            //        association.IdModule = source.ServiceID;
            //        association.OwnerLongID = source.ObjectLongID;
            //        association.OwnerTypeID = source.ObjectTypeID;
            //        association.OwnerFullyQualifiedName = source.FQN;
            //    }
            //    else
            //    {
            //        association.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            //        association.Deleted = Core.DomainModel.BaseStatusDeleted.None;
            //    }
            //    switch (skin.Type)
            //    {
            //        case SkinDisplayType.Portal:
            //            association.IsPortal = true;
            //            break;
            //        case SkinDisplayType.Organization:
            //            association.IdOrganization = Convert.ToInt32(skin.Id);
            //            break;
            //        case SkinDisplayType.CurrentCommunity:
            //        case SkinDisplayType.Community:
            //            association.IdCommunity = Convert.ToInt32(skin.Id);
            //            break;
            //        case SkinDisplayType.Module:
            //        case SkinDisplayType.NotAssociated:
            //            association.IdSkin = skin.Id;
            //            break;
            //    }

            //    return association;
            //}
            //private Skin_ShareModule AddModuleShare(Person person, Int32 idCommunity, ModuleAssociation association, lm.Comol.Core.DomainModel.ModuleObject source, SkinDisplayType type)
            //{
            //    Skin_ShareModule share = null;
            //    if (type == SkinDisplayType.Module)
            //    {
            //        share = (from s in Manager.GetIQ<Skin_ShareModule>()
            //                 where s.IdModule == association.IdModule && s.OwnerLongID == association.OwnerLongID && s.OwnerTypeID == association.OwnerTypeID
            //                 && s.IdCommunity == idCommunity
            //                 select s).Skip(0).Take(1).ToList().FirstOrDefault();
            //        if (share == null)
            //        {
            //            share = new Skin_ShareModule();
            //            share.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            //            share.IdCommunity = idCommunity;
            //            share.IdModule = association.IdModule;
            //            share.OwnerLongID = association.OwnerLongID;
            //            share.OwnerTypeID = association.OwnerTypeID;
            //            share.OwnerFullyQualifiedName = association.OwnerFullyQualifiedName;
            //            share.Skin = Manager.Get<Domain.Skin>(association.IdSkin);
            //        }
            //        else
            //        {
            //            share.Deleted = Core.DomainModel.BaseStatusDeleted.None;
            //            share.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            //            share.Skin = Manager.Get<Domain.Skin>(association.IdSkin);
            //        }
            //    }
            //    else
            //    {
            //        DeleteModuleShare(person, idCommunity, source, false, false);
            //    }
            //    return share;
            //}
            //private Boolean DeleteSkinAssociation(Person person, Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source, Boolean physical)
            //{
            //    Boolean result = false;
            //    var queryAssociation = (from ma in Manager.GetIQ<ModuleAssociation>()
            //                            where ma.IdModule == source.ServiceID && ma.OwnerLongID == source.ObjectLongID && ma.OwnerTypeID == source.ObjectTypeID
            //                            select ma);

            //    if (physical)
            //        Manager.DeletePhysicalList(queryAssociation.ToList());
            //    else
            //    {
            //        List<ModuleAssociation> items = queryAssociation.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
            //        items.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
            //        items.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
            //        Manager.SaveOrUpdateList(items);

            //    }
            //    result = true;
            //    return result;
            //}

            //private Boolean DeleteModuleShare(Person person, Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source, Boolean deleteSkin, Boolean physical)
            //{
            //    Boolean result = false;
            //    var queryShare = (from s in Manager.GetIQ<Skin_ShareModule>()
            //                      where s.IdModule == source.ServiceID && s.OwnerLongID == source.ObjectLongID && s.OwnerTypeID == source.ObjectTypeID && s.IdCommunity == idCommunity
            //                      select s);
            //    List<long> idSkins = queryShare.Where(s => s.Skin != null).Select(s => s.Skin.Id).ToList();


            //    if (physical)
            //        Manager.DeletePhysicalList(queryShare.ToList());
            //    else
            //    {
            //        List<Skin_ShareModule> shares = queryShare.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
            //        shares.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
            //        shares.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
            //        Manager.SaveOrUpdateList(shares);
            //    }
            //    if (deleteSkin)
            //    {
            //        foreach (long idSkin in idSkins)
            //        {
            //            if (!queryShare.Where(s => s.Skin.Id == idSkin && s.Deleted == Core.DomainModel.BaseStatusDeleted.None).Any())
            //            {
            //                Domain.Skin skin = Manager.Get<Domain.Skin>(idSkin);
            //                if (skin != null && physical)
            //                    Manager.DeletePhysical(skin);
            //                else if (skin != null)
            //                {
            //                    skin.Deleted = Core.DomainModel.BaseStatusDeleted.Manual;
            //                    skin.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            //                    Manager.SaveOrUpdate(skin);
            //                }
            //            }
            //        }
            //    }
            //    result = true;
            //    return result;
            //}

            ///// <summary>
            ///// A seconda del tipo, recupera gli Id necessari alla master per il render delle skin
            ///// </summary>
            ///// <param name="idCommunity">Id comunità impostato</param>
            ///// <param name="idItem">Id elemento (il significato del valore dipende dal tipo)</param>
            ///// <param name="itemType">Il tipo di skin</param>
            ///// <returns>Un dto con gli elementi necessari al render</returns>
            ///// <remarks>
            /////     Significato di idItem a seconda di itemType:
            /////             Empty                   n.d.    (DEFAULT = Come Portal)
            /////             Portal                  Portale. Gli altri Id sono a zero.
            /////             Organization            Id Organizzazione
            /////             Community               Id Comunità
            /////             Module                  Id Skin di modulo
            /////             NotAssociated           n.d.    (DEFAULT = Come Portal)
            /////             CurrentCommunity        Id Comunità (Al momento come Community)
            ///// </remarks>
            //public lm.Comol.Core.DomainModel.Helpers.ExternalPageContext GetItemSkinSettings(Int32 idCommunity, long idItem, SkinDisplayType itemType)
            //{
            //    lm.Comol.Core.DomainModel.Helpers.ExternalPageContext content = new lm.Comol.Core.DomainModel.Helpers.ExternalPageContext();
            //    content.Skin = new lm.Comol.Core.DomainModel.Helpers.dtoItemSkin();
            //    Community community = Manager.GetCommunity(idCommunity);
            //    Int32 idOrganization = (from p in Manager.GetIQ<OrganizationProfiles>() where p.Profile != null && p.Profile.Id == UC.CurrentUserID && p.isDefault select p.OrganizationID).Skip(0).Take(1).ToList().FirstOrDefault();

            //    switch (itemType)
            //    {
            //        case SkinDisplayType.Portal:
            //            content.Skin.IsForPortal = true;
            //            content.Skin.IdCommunity = 0;
            //            content.Skin.IdOrganization = 0;//idOrganization; -> modificato Mirco
            //            break;

            //        case SkinDisplayType.Community:
            //            //Agginto Mirco
            //            content.Skin.IsForPortal = false;
            //            content.Skin.IdCommunity = Convert.ToInt32(idItem);
            //            community = Manager.GetCommunity(content.Skin.IdCommunity);
            //            if (community != null)
            //            {
            //                content.Title = community.Name;
            //                content.Skin.IdOrganization = community.IdOrganization;
            //            }
            //            else
            //            {
            //                content.Skin.IdOrganization = idOrganization;
            //                content.Title = GetOrganizationName(idOrganization);
            //            }
            //            break;

            //        case SkinDisplayType.CurrentCommunity:
            //            content.Skin.IsForPortal = false;
            //            content.Skin.IdCommunity = Convert.ToInt32(idItem);
            //            community = Manager.GetCommunity(content.Skin.IdCommunity);
            //            if (community != null)
            //            {
            //                content.Title = community.Name;
            //                content.Skin.IdOrganization = community.IdOrganization;
            //            }
            //            else
            //            {
            //                content.Skin.IdOrganization = idOrganization;
            //                content.Title = GetOrganizationName(idOrganization);
            //            }
            //            break;
            //        case SkinDisplayType.Organization:
            //            content.Skin.IsForPortal = false;
            //            content.Skin.IdOrganization = Convert.ToInt32(idItem);
            //            content.Title = GetOrganizationName(content.Skin.IdOrganization);
            //            break;
            //        case SkinDisplayType.Module:
            //            content.Skin.IsForPortal = false;
            //            content.Skin.IdCommunity = Convert.ToInt32(idCommunity);
            //            if (community != null)
            //                content.Skin.IdOrganization = community.IdOrganization;
            //            else
            //                content.Skin.IdOrganization = idOrganization;
            //            content.Skin.IdSkin = idItem;
            //            break;
            //        default:
            //            //Aggiunto - Mirco (Come Portal. Se rimane così toglierei Portal)
            //            content.Skin.IsForPortal = true;
            //            content.Skin.IdCommunity = 0;
            //            content.Skin.IdOrganization = 0;
            //            break;
            //    }

            //    return content;
            //}
            //private String GetOrganizationName(Int32 idOrganization)
            //{
            //    return (from o in Manager.GetIQ<Organization>() where o.Id == idOrganization select o.Name).Skip(0).Take(1).ToList().FirstOrDefault();
            //}
            //public Boolean SkinHasMultipleAssociations(long idSkin, lm.Comol.Core.DomainModel.ModuleObject source)
            //{
            //    Boolean multiple = false;
            //    try
            //    {
            //        multiple = (from ma in Manager.GetIQ<ModuleAssociation>()
            //                    where ma.Deleted == Core.DomainModel.BaseStatusDeleted.None && ma.IdSkin == idSkin
            //                    && ma.OwnerLongID != source.ObjectLongID && ma.OwnerTypeID != source.ObjectTypeID && ma.IdModule != source.ServiceID && ma.OwnerFullyQualifiedName != source.FQN
            //                    select ma).Any();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    return multiple;
            //}

            //public Boolean DeleteModuleSkin(long idSkin, Boolean physical, String basePath)
            //{
            //    Boolean result = false;
            //    try
            //    {
            //        Manager.BeginTransaction();
            //        Person person = Manager.GetPerson(UC.CurrentUserID);
            //        var queryAssociation = (from ma in Manager.GetIQ<ModuleAssociation>() where ma.IdSkin == idSkin select ma);
            //        if (physical)
            //            Manager.DeletePhysicalList(queryAssociation.ToList());
            //        else
            //        {
            //            List<ModuleAssociation> items = queryAssociation.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
            //            items.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
            //            items.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
            //            Manager.SaveOrUpdateList(items);

            //        }
            //        Domain.Skin skin = Manager.Get<Domain.Skin>(idSkin);
            //        var queryShare = (from s in Manager.GetIQ<Skin_ShareModule>()
            //                          where s.Skin == skin
            //                          select s);

            //        if (physical)
            //            Manager.DeletePhysicalList(queryShare.ToList());
            //        else
            //        {
            //            List<Skin_ShareModule> shares = queryShare.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
            //            shares.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
            //            shares.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
            //            Manager.SaveOrUpdateList(shares);
            //        }


            //        if (physical)
            //            SkinFileManagement.EraseDir(idSkin, basePath);

            //        if (skin != null && physical)
            //            Manager.DeletePhysical(skin);
            //        else if (skin != null)
            //        {
            //            skin.Deleted = Core.DomainModel.BaseStatusDeleted.Manual;
            //            skin.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            //            Manager.SaveOrUpdate(skin);
            //        }
            //        Manager.Commit();
            //        result = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        if (Manager.IsInTransaction())
            //            Manager.RollBack();
            //        result = false;
            //    }
            //    return result;
            //}

            //public Boolean ObjectHasSkinAssociation(Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source)
            //{
            //    Boolean found = false;
            //    try
            //    {
            //        found = (from ma in Manager.GetIQ<ModuleAssociation>()
            //                 where ma.Deleted == Core.DomainModel.BaseStatusDeleted.None && ma.OwnerLongID == source.ObjectLongID && ma.OwnerTypeID == source.ObjectTypeID && ma.IdModule == source.ServiceID && ma.OwnerFullyQualifiedName == source.FQN
            //                 select ma).Any();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    return found;
            //}

            //public Boolean CloneSkinAssociation(int idUser, Int32 idModule, Int32 idCommunity, long idOldModuleItem, long idNewModuleItem, Int32 idItemType, String fullyQualifiedName)
            //{
            //    Boolean cloned = false;
            //    try
            //    {
            //        Manager.BeginTransaction();
            //        Person person = Manager.GetPerson(idUser);
            //        ModuleAssociation oldModuleAssociation = (from ma in Manager.GetIQ<ModuleAssociation>()
            //                                                  where ma.Deleted == Core.DomainModel.BaseStatusDeleted.None && ma.OwnerLongID == idOldModuleItem && ma.OwnerTypeID == idItemType && ma.IdModule == idModule && ma.OwnerFullyQualifiedName == fullyQualifiedName
            //                                                  select ma).Skip(0).Take(1).ToList().FirstOrDefault();
            //        if (oldModuleAssociation != null)
            //        {
            //            ModuleAssociation association = new ModuleAssociation();
            //            association.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            //            association.IdCommunity = idCommunity;
            //            association.IdModule = idModule;
            //            association.OwnerLongID = idNewModuleItem;
            //            association.OwnerTypeID = idItemType;
            //            association.OwnerFullyQualifiedName = fullyQualifiedName;
            //            association.IdCommunity = oldModuleAssociation.IdCommunity;
            //            association.IsPortal = oldModuleAssociation.IsPortal;
            //            association.IdOrganization = oldModuleAssociation.IdOrganization;
            //            association.IdSkin = oldModuleAssociation.IdSkin;
            //            Manager.SaveOrUpdate(association);
            //            Skin_ShareModule oldShare = (from s in Manager.GetIQ<Skin_ShareModule>()
            //                                         where s.IdModule == association.IdModule && s.OwnerLongID == idOldModuleItem && s.OwnerTypeID == association.OwnerTypeID
            //                        && s.IdCommunity == idCommunity && s.OwnerFullyQualifiedName == fullyQualifiedName
            //                                         select s).Skip(0).Take(1).ToList().FirstOrDefault();
            //            if (oldShare != null)
            //            {
            //                Skin_ShareModule s = new Skin_ShareModule();
            //                s.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            //                s.IdCommunity = idCommunity;
            //                s.IdModule = association.IdModule;
            //                s.OwnerLongID = association.OwnerLongID;
            //                s.OwnerTypeID = association.OwnerTypeID;
            //                s.OwnerFullyQualifiedName = association.OwnerFullyQualifiedName;
            //                s.Skin = oldShare.Skin;
            //                Manager.SaveOrUpdate(s);
            //            }
            //        }
            //        else
            //            cloned = true;
            //        Manager.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        if (Manager.IsInTransaction())
            //            Manager.RollBack();
            //        cloned = false;
            //    }
            //    return cloned;
            //}
            //public Boolean DeleteSkinAssociation(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName)
            //{
            //    Boolean deleted = false;
            //    try
            //    {
            //        Manager.BeginTransaction();
            //        List<ModuleAssociation> assignments = (from ma in Manager.GetIQ<ModuleAssociation>()
            //                                               where ma.OwnerLongID == idModuleItem && ma.OwnerTypeID == idItemType && ma.IdModule == idModule && ma.OwnerFullyQualifiedName == fullyQualifiedName
            //                                               select ma).ToList();
            //        if (assignments.Any())
            //            Manager.DeletePhysicalList(assignments);

            //        List<Skin_ShareModule> skinShares = (from ma in Manager.GetIQ<Skin_ShareModule>()
            //                                             where ma.OwnerLongID == idModuleItem && ma.OwnerTypeID == idItemType && ma.IdModule == idModule && ma.OwnerFullyQualifiedName == fullyQualifiedName
            //                                             select ma).ToList();
            //        if (skinShares.Any())
            //            Manager.DeletePhysicalList(skinShares);

            //        Manager.Commit();
            //        deleted = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        if (Manager.IsInTransaction())
            //            Manager.RollBack();
            //        deleted = false;
            //    }
            //    return deleted;
            //}


            //public List<lm.Comol.Modules.Standard.Skin.Presentation.SkinView> GetAvailableViews(SkinType type)
            //{
            //    List<lm.Comol.Modules.Standard.Skin.Presentation.SkinView> items = new List<lm.Comol.Modules.Standard.Skin.Presentation.SkinView>();
            //    if (type != SkinType.Module)
            //    {
            //        items.Add(Presentation.SkinView.Css);
            //        items.Add(Presentation.SkinView.Images);
            //    }
            //    items.Add(Presentation.SkinView.HeaderLogo);
            //    items.Add(Presentation.SkinView.FooterLogos);
            //    items.Add(Presentation.SkinView.FooterText);
            //    if (type == SkinType.Module)
            //        items.Add(Presentation.SkinView.Templates);
            //    else
            //        items.Add(Presentation.SkinView.Shares);
            //    return items;
            //}
        #endregion
    }
}