using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using NHibernate.Linq;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.MailCommons.Domain.Configurations;
using lm.Comol.Core.MailCommons.Domain.Messages;
using lm.Comol.Core.MailCommons.Domain;
namespace lm.Comol.Core.Mail.Messages
{
    public class MailMessagesService : CoreServices 
    {
        protected const Int32 maxItemsForQuery = 500;
        protected iApplicationContext _Context;

        #region initClass
            public MailMessagesService() :base() { }
            public MailMessagesService(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
            }
            public MailMessagesService(iDataContext oDC)
                : base(oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext() { DataContext = oDC };
            }
        #endregion

        #region "CRUD actions"
            //public MailTemplate GetTemplate()
            //{
            //    MailTemplate item = null;
            //    try
            //    {
            //         item = (from t in Manager.GetIQ<MailTemplate>()
            //                where t.IdTemplate == template.IdTemplate && t.IdVersion == template.IdVersion && template.IsTemplateCompliant && t.IsTemplateCompliant
            //                    && t.Deleted == BaseStatusDeleted.None
            //                select t).Skip(0).Take(1).ToList().FirstOrDefault();

            //    }
            //    catch (Exception ex)
            //    {
            //        item = null;
            //    }
            //    return item;
            //}

            public MailMessage SaveMessage(List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> tMessages, SmtpServiceConfig  smtpConfig, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mailSettings, lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = null, ModuleObject obj = null, String moduleCode = "", Int32 idModule = 0, Int32 idCommunity = -1, Boolean isPortal = false)
            {
                if (idCommunity==-1 && obj!=null)
                    idCommunity = obj.CommunityID;

                Ownership ownership = new Ownership();
                ownership.ModuleObject = obj;
                ownership.ModuleCode = (!String.IsNullOrEmpty(moduleCode)) ? moduleCode : (idModule > 0) ? Manager.GetModuleCode(idModule) : "";
                ownership.IdModule = (idModule > 0) ? idModule : (!String.IsNullOrEmpty(moduleCode) ? Manager.GetModuleID(moduleCode) : 0);
                ownership.IsPortal = isPortal || (idCommunity == 0);
                if (idCommunity > 0)
                    ownership.Community = Manager.GetLiteCommunity(idCommunity);
                return SaveMessage(ownership, tMessages,smtpConfig, mailSettings, template);
            }
            public MailMessage SaveMessage(Ownership ownership, List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> tMessages, SmtpServiceConfig smtpConfig, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mailSettings, lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = null)
            {
                MailMessage message = null;
                try
                {
                    Manager.BeginTransaction();
                    DateTime createdOn = DateTime.Now;
                    litePerson sender = Manager.GetLitePerson(UC.CurrentUserID);
                    if (sender != null && sender.TypeID != (int)UserTypeStandard.Guest && sender.TypeID != (int)UserTypeStandard.PublicUser && tMessages!= null && tMessages.Any() && tMessages.Where(m=> m.Recipients.Any() || m.RemovedRecipients.Any()).Any()){
                        message = AddMessage(sender, createdOn, ownership, tMessages, smtpConfig, mailSettings, AddTemplate(sender, createdOn, mailSettings, template));
                    }
                    
                    Manager.Commit();
                }
                catch(Exception ex){
                    message = null;
                    Manager.RollBack();
                }
                return message;
            }
            private MailTemplate AddTemplate(litePerson sender, DateTime createdOn, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mailSettings, dtoBaseMailTemplate template)
            {
                Boolean inTransaction = Manager.IsInTransaction();
                MailTemplate item = null;
                try
                {
                    if (!inTransaction)
                        Manager.BeginTransaction();
                    if (sender != null && sender.TypeID != (int)UserTypeStandard.Guest && sender.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        item = (from t in Manager.GetIQ<MailTemplate>()
                                where t.IdTemplate == template.IdTemplate && t.IdVersion == template.IdVersion && template.IsTemplateCompliant && t.IsTemplateCompliant 
                                    && t.Deleted == BaseStatusDeleted.None
                                select t).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (item == null)
                        {
                            item = new MailTemplate();
                            item.CreatedBy = sender;
                            item.CreatedOn = createdOn;
                            if (template !=null){
                                item.IdTemplate = template.IdTemplate;
                                item.IdVersion = template.IdVersion;
                                if (template.DefaultTranslation != null && !template.DefaultTranslation.IsEmpty())
                                    item.DefaultTranslation = template.DefaultTranslation.Copy();
                                item.IsTemplateCompliant = template.IsTemplateCompliant;
                                item.MailSettings = template.MailSettings;
                            }
                            else
                                item.MailSettings = mailSettings;
                            if (template.DefaultTranslation.IsEmpty())
                                template.DefaultTranslation.IsHtml = item.MailSettings.IsBodyHtml;
                            Manager.SaveOrUpdate(item);
                            if (template!=null && template.Translations != null && template.Translations.Any())
                            {
                                List<MailTemplateContent> contents = (from r in template.Translations
                                                                      where !r.IsEmpty
                                                                      select
                                                                          new MailTemplateContent()
                                                                          {
                                                                              IdLanguage = r.IdLanguage,
                                                                              LanguageCode = r.LanguageCode,
                                                                              LanguageName = r.LanguageName,
                                                                              Template = item,
                                                                              Translation = r.Translation.Copy(),
                                                                          }).ToList();
                                Manager.SaveOrUpdateList(contents);
                                item.Translations = contents;
                                Manager.SaveOrUpdate(item);
                            }
                        }
                    }
                    if (!inTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    item = null;
                    if (!inTransaction)
                        Manager.RollBack();
                }
                return item;
            }
            private MailMessage AddMessage(litePerson sender, DateTime createdOn, Ownership ownership, List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> tMessages, SmtpServiceConfig smtpConfig, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mailSettings, MailTemplate template = null)
            {
                MailMessage message = new MailMessage();
                message.CreatedOn = createdOn;
                message.CreatedBy = sender;
                message.MailSettings = mailSettings;
                message.Ownership = ownership;
                message.Template = template;
                message.SentBySystem = (mailSettings.SenderType == MailCommons.Domain.SenderUserType.System);
                Manager.SaveOrUpdate(message);
                Language language = Manager.GetDefaultLanguage();
                if (tMessages.Any()) {
                    List<MessageTranslation> translations = tMessages.Where(t => t.Recipients.Any() || t.RemovedRecipients.Any()).Select(t => AddMessageTranslation((language!= null) ? language.Id : 0, sender, createdOn, ownership,smtpConfig,mailSettings, t,message, template)).ToList();
                    if (translations.Any())
                    {
                        Manager.SaveOrUpdateList(translations);
                        message.Translations = translations;
                        Manager.SaveOrUpdate(message);
                    }
                }
                return message;
            }
            private MessageTranslation AddMessageTranslation(Int32 idLanguage, litePerson sender, DateTime createdOn, Ownership ownership, SmtpServiceConfig smtpConfig, MessageSettings mailSettings, lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage dtoMessage, MailMessage message, MailTemplate template)
            {
                MessageTranslation translation = new MessageTranslation();
                translation.Body = dtoMessage.Body;
                translation.CreatedOn = createdOn;
                translation.CreatedBy = sender;
                translation.IdLanguage = dtoMessage.IdLanguage;
                translation.LanguageCode = dtoMessage.CodeLanguage;
                translation.SentBySystem = message.SentBySystem;
                if (mailSettings.PrefixType ==  MailCommons.Domain.SubjectPrefixType.SystemConfiguration)
                    translation.Subject = smtpConfig.GetSubjectPrefix(dtoMessage.IdLanguage, idLanguage) + dtoMessage.Subject;
                else
                    translation.Subject = dtoMessage.Subject;
                translation.Ownership= ownership;
                translation.Message= message;
                switch (mailSettings.Signature)
                {
                    case Signature.FromNoReplySettings:
                        translation.Body += ((mailSettings.IsBodyHtml) ? "<br/><br/>" : "\r\n\r\n\r\n") + smtpConfig.GetNoReplySignature(dtoMessage.IdLanguage, idLanguage);
                        break;
                    case Signature.FromConfigurationSettings:
                        translation.Body += ((mailSettings.IsBodyHtml) ? "<br/><br/>" : "\r\n\r\n\r\n") + smtpConfig.GetSignature(dtoMessage.IdLanguage, idLanguage);
                        break;
                }
                Manager.SaveOrUpdate(translation);
                List<MailRecipient> recipients = new List<MailRecipient>();
                if (dtoMessage.Recipients != null && dtoMessage.Recipients.Any())
                {
                    recipients = (from r in dtoMessage.Recipients
                                                        select
                                                            new MailRecipient()
                                                            {
                                                                DisplayName = r.DisplayName,
                                                                IdPerson = r.IdPerson,
                                                                IdUserModule = r.IdUserModule,
                                                                IsMailSent = dtoMessage.Sent,
                                                                MailAddress = r.MailAddress,
                                                                Message = message,
                                                                Item= translation,
                                                                Ownership = message.Ownership,
                                                                IdLanguage= translation.IdLanguage,
                                                                LanguageCode= translation.LanguageCode,
                                                                Type = r.Type,
                                                                IdModuleObject = r.IdModuleObject,
                                                                IdModuleType = r.IdModuleType
                                                            }).ToList();
                }
                if (dtoMessage.RemovedRecipients != null && dtoMessage.RemovedRecipients.Any())
                {
                    recipients.AddRange((from r in dtoMessage.RemovedRecipients
                                                        select
                                                            new MailRecipient()
                                                            {
                                                                DisplayName = r.DisplayName,
                                                                IdPerson = r.IdPerson,
                                                                IdUserModule = r.IdUserModule,
                                                                IsMailSent = false,
                                                                MailAddress = r.MailAddress,
                                                                Message = message,
                                                                Item = translation,
                                                                Ownership = message.Ownership,
                                                                IdLanguage = translation.IdLanguage,
                                                                LanguageCode = translation.LanguageCode,
                                                                Type = r.Type,
                                                                IdModuleObject = r.IdModuleObject,
                                                                IdModuleType = r.IdModuleType
                                                            }).ToList());
                }
                if (recipients.Count > 0)
                {
                    Manager.SaveOrUpdateList(recipients);
                    translation.Recipients = recipients;
                    Manager.SaveOrUpdate(translation);
                }
                return translation;
            }
        #endregion

        #region "List"
            public List<dtoDisplayMessage> GetDisplayMessages(dtoOwnership owner, Int32 idUser)
            {
                List<dtoDisplayMessage> items = new List<dtoDisplayMessage>();
                try
                {
                    Boolean validQuery = false;
                    litePerson p = Manager.GetLitePerson(idUser);
                    if (p!=null){
                        var query = (from m in Manager.GetIQ<MailMessage>() where m.Deleted== BaseStatusDeleted.None && m.Ownership !=null select m);
                        if (owner.ModuleObject != null)
                        {
                            query = query.Where(m => m.Ownership.ModuleObject != null && m.Ownership.ModuleObject.ObjectLongID == owner.ModuleObject.ObjectLongID && m.Ownership.ModuleObject.ObjectTypeID == owner.ModuleObject.ObjectTypeID && ((!String.IsNullOrEmpty(owner.ModuleObject.ServiceCode) && m.Ownership.ModuleObject.ServiceCode == owner.ModuleObject.ServiceCode) || (m.Ownership.ModuleObject.ServiceID == owner.ModuleObject.ServiceID && owner.ModuleObject.ServiceID > 0)));
                            validQuery = true;
                        }
                        if (owner.IdModule > 0 || !String.IsNullOrEmpty(owner.ModuleCode))
                        {
                            query = query.Where(m => m.Ownership.ModuleCode == owner.ModuleCode && m.Ownership.IdModule == owner.IdModule);
                            validQuery = true;
                        }
                        if (owner.IdCommunity> 0)
                        {
                            query = query.Where(m => !m.Ownership.IsPortal && m.Ownership.Community != null && m.Ownership.Community.Id == owner.IdCommunity);
                            validQuery = true;
                        }
                        else if (owner.IdCommunity == 0)
                        {
                            query = query.Where(m=>m.Ownership.IsPortal);
                            validQuery = true;
                        }
                        if (validQuery){
                            List<MailMessage> messages = query.ToList();
                            items = (from m in messages select new dtoDisplayMessage()
                                    {
                                        CreatedOn = m.CreatedOn,
                                        CreatedBy= m.CreatedBy,
                                        SentBySystem= m.SentBySystem,
                                        Id= m.Id,
                                        IdTemplate= (m.Template != null) ? m.Template.Id : 0,
                                        IdExternalTemplate = (m.Template != null  && m.Template.IdTemplate>0) ? m.Template.IdTemplate : 0,
                                        IdExternalVersion = (m.Template != null && m.Template.IdVersion>0) ? m.Template.IdVersion:0,
                                        ExternalTemplateCompliant = (m.Template !=null && m.Template.IsTemplateCompliant)
                                    }
                                    ).ToList();
                            Language dLanguage = Manager.GetDefaultLanguage();

                            if (items.Where(i => !i.ExternalTemplateCompliant).Any())
                                UpdateDisplayNameByTemplate(p, dLanguage, items.Where(i => !i.ExternalTemplateCompliant).ToList());

                            if (items.Where(i => i.ExternalTemplateCompliant && i.IdExternalVersion > 0).Any())
                                UpdateNameByExternalTemplateVersion(p, dLanguage, items.Where(i => i.ExternalTemplateCompliant && i.IdExternalVersion > 0).ToList());
                            if (items.Where(i => i.ExternalTemplateCompliant && i.IdExternalTemplate>0 && i.IdExternalVersion == 0).Any())
                                UpdateNameByExternalTemplate(p, dLanguage, items.Where(i => i.ExternalTemplateCompliant && i.IdExternalTemplate > 0 && i.IdExternalVersion == 0).ToList());
                            items = items.OrderBy(i => i.DisplayName).ThenBy(i => i.TemplateName).ToList();
                        }
                    }
                }
                catch (Exception ex) { 
                
                }
                return items;
            }

            private void UpdateNameByExternalTemplateVersion(litePerson p, Language language, List<dtoDisplayMessage> items)
            {
                Dictionary<long, ItemObjectTranslation> tNames = GetExternalVersionsContent(items.Select(i => i.IdExternalVersion).Distinct().ToList(), p.LanguageID, language.Id);
                items.Where(i => tNames.ContainsKey(i.IdExternalVersion)).ToList().ForEach(i => UpdateItem(i, tNames[i.IdExternalVersion]));
            }
            private Dictionary<long, ItemObjectTranslation> GetExternalVersionsContent(List<long> idVersions, Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti = true)
            {
                if (idVersions.Count < maxItemsForQuery)
                    return (from t in Manager.GetIQ<lm.Comol.Core.TemplateMessages.Domain.TemplateDefinitionVersion>() where idVersions.Contains(t.Id) select t).ToList().ToDictionary(t => t.Id, t => t.GetTranslation(idUserLanguage, idDefaultLanguage, firstIsMulti, true));
                else
                {
                    Dictionary<long, ItemObjectTranslation> result = new Dictionary<long, ItemObjectTranslation>();
                    Int32 pageIndex = 0;
                    List<long> idItems = idVersions.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idItems.Any())
                    {
                        (from t in Manager.GetIQ<lm.Comol.Core.TemplateMessages.Domain.TemplateDefinitionVersion>() where idItems.Contains(t.Id) select t).ToList().ForEach(t => result.Add(t.Id, t.GetTranslation(idUserLanguage, idDefaultLanguage, firstIsMulti, true)));
                        pageIndex++;
                        idItems = idVersions.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return result;
                }
            }
            private void UpdateNameByExternalTemplate(litePerson p, Language language, List<dtoDisplayMessage> items)
            {
                
                Dictionary<long, ItemObjectTranslation> tNames = GetExternalTemplatesContent(items.Select(i => i.IdExternalTemplate).Distinct().ToList(), p.LanguageID, language.Id);
                items.Where(i => tNames.ContainsKey(i.IdExternalTemplate)).ToList().ForEach(i => UpdateItem(i, tNames[i.IdExternalTemplate]));
            }
            private Dictionary<long, ItemObjectTranslation> GetExternalTemplatesContent(List<long> idTemplates, Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti = true)
            {
                if (idTemplates.Count < maxItemsForQuery)
                    return (from t in Manager.GetIQ<lm.Comol.Core.TemplateMessages.Domain.TemplateDefinition>() where idTemplates.Contains(t.Id) select t).ToList().ToDictionary(t => t.Id, t => t.LastVersion.GetTranslation(idUserLanguage, idDefaultLanguage, firstIsMulti,true));
                else
                {
                    Dictionary<long, ItemObjectTranslation> result = new Dictionary<long, ItemObjectTranslation>();
                    Int32 pageIndex = 0;
                    List<long> idItems = idTemplates.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idItems.Any())
                    {
                        (from t in Manager.GetIQ<lm.Comol.Core.TemplateMessages.Domain.TemplateDefinition>() where idItems.Contains(t.Id) select t).ToList().ForEach(t => result.Add(t.Id, t.LastVersion.GetTranslation(idUserLanguage, idDefaultLanguage, firstIsMulti, true)));
                        pageIndex++;
                        idItems = idTemplates.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return result;
                }
            }
            private void UpdateDisplayNameByTemplate(litePerson p, Language language, List<dtoDisplayMessage> items)
            {
                Dictionary<long, ItemObjectTranslation> tNames = GetTemplatesContent(items.Select(i => i.IdTemplate).Distinct().ToList(), p.LanguageID, language.Id);
                items.Where(i => tNames.ContainsKey(i.IdTemplate)).ToList().ForEach(i => UpdateItem(i, tNames[i.IdTemplate]));
            }
            private Dictionary<long, ItemObjectTranslation> GetTemplatesContent(List<long> idTemplates, Int32 userLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti = true) {
                if (idTemplates.Count < maxItemsForQuery)
                    return (from t in Manager.GetIQ<MailTemplate>() where idTemplates.Contains(t.Id) select t).ToList().ToDictionary(t => t.Id, t => t.GetTranslation(userLanguage, idDefaultLanguage,firstIsMulti, true));
                else {
                    Dictionary<long, ItemObjectTranslation> result = new Dictionary<long, ItemObjectTranslation>();
                    Int32 pageIndex = 0;
                    List<long> idItems = idTemplates.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idItems.Any())
                    {
                        (from t in Manager.GetIQ<MailTemplate>() where idItems.Contains(t.Id) select t).ToList().ForEach(t => result.Add(t.Id, t.GetTranslation(userLanguage, idDefaultLanguage, firstIsMulti, true)));
                        pageIndex++;
                        idItems = idTemplates.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return result;
                }
            }
            private void UpdateItem(dtoDisplayMessage m, ItemObjectTranslation t) {
                m.Name = t.Name;
                m.Subject = t.Subject;
            }
        #endregion

        #region "Messages"
            public Boolean IsAdministrativeUser(Int32 idPerson)
            {
                return IsAdministrativeUser(Manager.GetLitePerson(idPerson));
            }
            public Boolean IsAdministrativeUser(litePerson litePerson)
            {
                Boolean result = (litePerson != null && (litePerson.TypeID == (int)UserTypeStandard.SysAdmin || litePerson.TypeID == (int)UserTypeStandard.Administrator));
                if (litePerson != null && litePerson.TypeID == (int)UserTypeStandard.Administrative)
                {
                    result = true;
                }
                return result;
            }
            public Boolean IsMessageOf(long idMessage,ModuleObject obj)
            {
                Boolean result = false;
                try
                {
                    if (obj != null)
                    {
                        result = (from m in Manager.GetIQ<MailMessage>()
                                  where m.Deleted == BaseStatusDeleted.None && m.Ownership.ModuleObject != null && m.Ownership.ModuleObject.ObjectLongID == obj.ObjectLongID && m.Ownership.ModuleObject.ObjectTypeID == obj.ObjectTypeID && ((!String.IsNullOrEmpty(obj.ServiceCode) && m.Ownership.ModuleObject.ServiceCode == obj.ServiceCode) || (m.Ownership.ModuleObject.ServiceID == obj.ServiceID && obj.ServiceID > 0))
                                  && m.Id == idMessage
                                  select m.Id).Any();
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public Boolean HasMessages(ModuleObject obj)
            {
                Boolean result = false;
                try
                {
                    if (obj != null)
                    {
                        result = (from m in Manager.GetIQ<MailMessage>()
                                                 where m.Deleted == BaseStatusDeleted.None && m.Ownership.ModuleObject != null && m.Ownership.ModuleObject.ObjectLongID == obj.ObjectLongID && m.Ownership.ModuleObject.ObjectTypeID == obj.ObjectTypeID && ((!String.IsNullOrEmpty(obj.ServiceCode) && m.Ownership.ModuleObject.ServiceCode == obj.ServiceCode) || (m.Ownership.ModuleObject.ServiceID == obj.ServiceID && obj.ServiceID > 0))
                                                 select m.Id).Any();
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public Boolean HasUsersForMessage(ModuleObject obj, MessageRecipientType type = MessageRecipientType.All )
            {
                Boolean result = false;
                try
                {
                    if (obj != null)
                    {
                        List<long> idMessages = (from m in Manager.GetIQ<MailMessage>()
                                                 where m.Deleted == BaseStatusDeleted.None && m.Ownership.ModuleObject != null && m.Ownership.ModuleObject.ObjectLongID == obj.ObjectLongID && m.Ownership.ModuleObject.ObjectTypeID == obj.ObjectTypeID && ((!String.IsNullOrEmpty(obj.ServiceCode) && m.Ownership.ModuleObject.ServiceCode == obj.ServiceCode) || (m.Ownership.ModuleObject.ServiceID == obj.ServiceID && obj.ServiceID > 0))
                                                 select m.Id).ToList();
                        result = HasUsersForMessage(idMessages, type);
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public Boolean HasUsersForMessage(List<long> idMessages, MessageRecipientType type = MessageRecipientType.All)
            {
                Boolean result = false;
                try
                {
                    if (idMessages.Any())
                    {
                        List<MailRecipient> recipients = new List<MailRecipient>();
                        if (idMessages.Count <= maxItemsForQuery)
                            result = (from r in Manager.GetIQ<MailRecipient>()
                                          where r.Deleted == BaseStatusDeleted.None
                                          && idMessages.Contains(r.Message.Id) && (type == MessageRecipientType.All || (type == MessageRecipientType.External && r.IdPerson < 1) || (type == MessageRecipientType.Internal && r.IdPerson>0))
                                          select r).Any();
                        else
                        {
                            Int32 pageIndex = 0;
                            var userQuery = idMessages.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (userQuery.Any() && !result )
                            {
                                result = 
                                    (from r in Manager.GetIQ<MailRecipient>()
                                     where r.Deleted == BaseStatusDeleted.None
                                     && idMessages.Contains(r.Message.Id) && (type == MessageRecipientType.All || (type == MessageRecipientType.External && r.IdPerson < 1) || (type == MessageRecipientType.Internal && r.IdPerson > 0))
                                     select r).Any();
                                pageIndex++;
                                userQuery = idMessages.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public List<dtoModuleRecipientMessages> GetUsersForMessage(ModuleObject obj, List<long> idMessages, MessageRecipientType type = MessageRecipientType.All)
            {
                List<dtoModuleRecipientMessages> results = new List<dtoModuleRecipientMessages>();
                try
                {
                    if (obj != null && idMessages.Any())
                    {
                        List<MailRecipient> recipients = new List<MailRecipient>();
                        if (idMessages.Count<= maxItemsForQuery)
                            recipients = (from r in Manager.GetIQ<MailRecipient>()
                                     where r.Deleted== BaseStatusDeleted.None
                                     && idMessages.Contains(r.Message.Id) && (type == MessageRecipientType.All || (type == MessageRecipientType.External && !r.IsInternal) || (type == MessageRecipientType.Internal && r.IsInternal))
                                       select r).ToList();
                        else{
                            Int32 pageIndex = 0;
                            var userQuery = idMessages.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (userQuery.Any())
                            {
                                recipients.AddRange(
                                    (from r in Manager.GetIQ<MailRecipient>()
                                     where r.Deleted == BaseStatusDeleted.None
                                     && idMessages.Contains(r.Message.Id) && (type == MessageRecipientType.All || (type == MessageRecipientType.External && !r.IsInternal) || (type == MessageRecipientType.Internal && r.IsInternal))
                                     select r).ToList());
                                pageIndex++;
                                userQuery = idMessages.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                        }

                        //foreach (var item in recipients.GroupBy(r => r.IdPerson)) {
                        //    if (item.Key > 0)
                        //    {
                        //        results.Add(new dtoModuleMessageRecipient()
                        //                        {
                        //                            IdPerson = item.Key,
                        //                            IdUserModule = item.FirstOrDefault().IdUserModule,
                        //                            ModuleCode = obj.ServiceCode,
                        //                            MessageNumber = item.Count(),
                        //                            Messages = item.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent = m.IsMailSent, IdModuleObject = m.IdModuleObject, IdModuleType = m.IdModuleType }).ToList()
                        //                        });
                        //    }
                        //    else if (item.Where(i => i.IdUserModule > 0).Any())
                        //    {
                        //        results.AddRange(recipients.Where(r => r.IsFromModule && r.IdUserModule>0 && r.IdPerson<1).ToList().GroupBy(r => r.IdUserModule).ToList().Select(i=> new dtoModuleMessageRecipient()
                        //                        {
                        //                            IdUserModule = i.Key,
                        //                            ModuleCode = obj.ServiceCode,
                        //                            MessageNumber = i.Count(),
                        //                            MailAddress = i.OrderByDescending(it=>it.Id).ToList().Select(it=> it.MailAddress).FirstOrDefault(),
                        //                            Messages = i.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent= m.IsMailSent, IdModuleObject=m.IdModuleObject, IdModuleType= m.IdModuleType }).ToList()
                        //                        }).ToList());
                        //    }
                        //    else {
                        //        results.AddRange(recipients.Where(r => r.IsFromModule && r.IdPerson==0 && r.IdUserModule == 0 && !String.IsNullOrEmpty(r.MailAddress)).GroupBy(r => r.MailAddress.ToLower()).ToList().Select(i=>new dtoModuleMessageRecipient()
                        //        {
                        //            IdUserModule = 0,
                        //            ModuleCode = obj.ServiceCode,
                        //            MailAddress =i.Key,
                        //            MessageNumber = i.Count(),
                        //            Messages = i.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent = m.IsMailSent, IdModuleObject = m.IdModuleObject, IdModuleType = m.IdModuleType }).ToList()
                        //        }).ToList());
                        //    }
                        //}
                        results = GetParsedRecipients(obj,recipients);
                    }
                }
                catch (Exception ex)
                {

                }
                return results;
            }
            public List<dtoModuleRecipientMessages> GetParsedRecipients(ModuleObject obj,List<MailRecipient> recipients, Boolean alsoSubject = false){
                List<dtoModuleRecipientMessages> results = new List<dtoModuleRecipientMessages>();
                foreach (var item in recipients.GroupBy(r => r.IdPerson)) {
                    if (item.Key > 0)
                    {
                        results.Add(new dtoModuleRecipientMessages()
                                        {
                                            IdPerson = item.Key,
                                            IdUserModule = item.FirstOrDefault().IdUserModule,
                                            ModuleCode = obj.ServiceCode,
                                            MessageNumber = item.Count(),
                                            Messages = item.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent = m.IsMailSent, IdModuleObject = m.IdModuleObject, IdModuleType = m.IdModuleType, Subject= (alsoSubject) ? m.Item.Subject : "" }).ToList()
                                        });
                    }
                    else if (item.Where(i => i.IdUserModule > 0).Any())
                    {
                        results.AddRange(recipients.Where(r => r.IsFromModule && r.IdUserModule>0 && r.IdPerson<1).ToList().GroupBy(r => r.IdUserModule).ToList().Select(i=> new dtoModuleRecipientMessages()
                                        {
                                            IdUserModule = i.Key,
                                            ModuleCode = obj.ServiceCode,
                                            MessageNumber = i.Count(),
                                            MailAddress = i.OrderByDescending(it=>it.Id).ToList().Select(it=> it.MailAddress).FirstOrDefault(),
                                            Messages = i.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent= m.IsMailSent, IdModuleObject=m.IdModuleObject, IdModuleType= m.IdModuleType , Subject= (alsoSubject) ? m.Item.Subject : ""}).ToList()
                                        }).ToList());
                    }
                    else {
                        results.AddRange(recipients.Where(r => r.IsFromModule && r.IdPerson==0 && r.IdUserModule == 0 && !String.IsNullOrEmpty(r.MailAddress)).GroupBy(r => r.MailAddress.ToLower()).ToList().Select(i=>new dtoModuleRecipientMessages()
                        {
                            IdUserModule = 0,
                            ModuleCode = obj.ServiceCode,
                            MailAddress =i.Key,
                            MessageNumber = i.Count(),
                            Messages = i.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent = m.IsMailSent, IdModuleObject = m.IdModuleObject, IdModuleType = m.IdModuleType, Subject= (alsoSubject) ? m.Item.Subject : "" }).ToList()
                        }).ToList());
                    }
                }
                return results;
            }
            public Int32 GetUserMessagesCount(dtoModuleMessagesContext context, Boolean alsoDeleted = false )
            {
                Int32 count = 0;
                try
                {
                    if (context.ModuleObject != null)
                        count = (from r in Manager.GetIQ<MailRecipient>()
                                 where (r.Deleted == BaseStatusDeleted.None || (alsoDeleted && r.Deleted == BaseStatusDeleted.Manual))
                                  && r.Ownership.ModuleCode == context.ModuleCode &&r.Ownership.ModuleObject != null && r.Ownership.ModuleObject.ObjectLongID == context.ModuleObject.ObjectLongID && r.Ownership.ModuleObject.ObjectTypeID == context.ModuleObject.ObjectTypeID
                                  && (
                                  (r.MailAddress == context.MailAddress && r.IdUserModule == 0 && r.IdPerson == 0)
                                  ||
                                  (r.IdUserModule== context.IdUserModule)
                                  ||
                                  (r.IdPerson == context.IdPerson && context.IdUserModule==0)
                                  )
                                 select r.Id).Count();
                }
                catch (Exception ex) { 
                
                }
                return count;
            }
            public List<dtoDisplayUserMessage> GetUserMessages(dtoModuleMessagesContext context,String removedTranslation, String unknownUserTranslation, Int32 pageIndex, Int32 pageSize, Boolean alsoDeleted = false)
            {
                List<dtoDisplayUserMessage> items = new List<dtoDisplayUserMessage>();
                try
                {
                    if (context.ModuleObject != null)
                        items = (from r in Manager.GetIQ<MailRecipient>()
                                 where (r.Deleted == BaseStatusDeleted.None || (alsoDeleted && r.Deleted == BaseStatusDeleted.Manual))
                                  && r.Ownership.ModuleCode == context.ModuleCode && r.Ownership.ModuleObject != null && r.Ownership.ModuleObject.ObjectLongID == context.ModuleObject.ObjectLongID && r.Ownership.ModuleObject.ObjectTypeID == context.ModuleObject.ObjectTypeID
                                  && (
                                  (r.MailAddress == context.MailAddress && r.IdUserModule == 0 && r.IdPerson == 0)
                                  ||
                                  (r.IdUserModule == context.IdUserModule)
                                  ||
                                  (r.IdPerson == context.IdPerson && context.IdUserModule == 0)
                                  )
                                 select r).ToList().OrderByDescending(r => r.Item.CreatedOn).Skip(pageIndex * pageSize).Take(pageSize).Select(r=> new dtoDisplayUserMessage()
                                 {
                                     CreatedBy= (r.Item.CreatedBy==null) ? removedTranslation : r.Item.CreatedBy.SurnameAndName,
                                     CreatedOn= r.Item.CreatedOn,
                                     Id=  r.Item.Id,
                                     SentBySystem= r.Item.SentBySystem,
                                     Subject= r.Item.Subject 
                                 }).ToList();
                }
                catch (Exception ex)
                {

                }
                return items;
            }
            public MailRecipient GetRecipient( String mailAddress, dtoModuleMessagesContext context) {
                MailRecipient recipient = null;
                if (context.ModuleObject != null)
                    recipient = (from r in Manager.GetIQ<MailRecipient>()
                                 where r.MailAddress == mailAddress && !String.IsNullOrEmpty(mailAddress) && r.IdPerson == 0 && r.IdUserModule == 0
                                     && r.Ownership.ModuleCode == context.ModuleCode && r.Ownership.ModuleObject != null && r.Ownership.ModuleObject.ObjectLongID == context.ModuleObject.ObjectLongID && r.Ownership.ModuleObject.ObjectTypeID == context.ModuleObject.ObjectTypeID
                                 select r).Skip(0).Take(1).ToList().FirstOrDefault();
                    

                return recipient;
            }
            public MessageTranslation GetMessageTranslation(long idMessage) {
                return Manager.Get<MessageTranslation>(idMessage);
            }
            public MailMessage GetMessage(long idMessage)
            {
                MailMessage m = null;
                try
                {
                    m = Manager.Get<MailMessage>(idMessage);
                }
                catch (Exception ex) {
                    m = null;
                }
                return m;
            }

            public Boolean HasUsersWithProfileType(ModuleObject obj, int idProfileType, long idMessage=0)
            {
                Boolean found = false;
                try
                {

                    List<Int32> idUsers = GetRecipientsQuery(obj.CommunityID, false, obj,"",0, idMessage).Where(r => r.IdPerson > 0).Select(r => r.IdPerson).ToList().Distinct().ToList();
                    if (idUsers.Any())
                    {
                        if (idUsers.Count <= maxItemsForQuery)
                        {
                            found = (from p in Manager.GetIQ<litePerson>() where p.TypeID == idProfileType where idUsers.Contains(p.Id) select p.Id).Any();
                        }
                        else
                        {
                            Int32 pageIndex = 0;
                            List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (idPagedUsers.Any())
                            {
                                found = (from p in Manager.GetIQ<litePerson>() where p.TypeID == idProfileType where idPagedUsers.Contains(p.Id) select p.Id).Any();
                                if (found)
                                    break;
                                pageIndex++;
                                idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return found;
            }

            public Dictionary<long, String> GetAgenciesForUsers(ModuleObject obj, long idMessage=0)
            {
                Dictionary<long, String> list = new Dictionary<long, String>();
                try
                {
                    List<Int32> idUsers = GetRecipientsQuery(obj.CommunityID, false, obj, "", 0, idMessage).Where(r => r.IdPerson > 0).Select(r => r.IdPerson).ToList().Distinct().ToList();
                    if (idUsers.Any())
                    {
                        if (idUsers.Count <= maxItemsForQuery)
                            list = GetAgenciesForUsers((from p in Manager.GetIQ<litePerson>() where p.TypeID == (Int32)UserTypeStandard.Employee where idUsers.Contains(p.Id) select p.Id).ToList());
                        else
                        {
                            Int32 pageIndex = 0;
                            List<Int32> idEmployee = new List<Int32>();
                            List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (idPagedUsers.Any())
                            {
                                idEmployee.AddRange((from p in Manager.GetIQ<litePerson>() where p.TypeID == (Int32)UserTypeStandard.Employee where idPagedUsers.Contains(p.Id) select p.Id).ToList());
                                
                                pageIndex++;
                                idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                            list = GetAgenciesForUsers(idEmployee);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return list;
            }

            /// <summary>
            /// Recupera la lista delle agenzie data una lista di utenti
            /// </summary>
            /// <param name="idUsers"></param>
            /// <returns></returns>
            private Dictionary<long, String> GetAgenciesForUsers(List<Int32> idUsers)
            {
                Dictionary<long, String> list = new Dictionary<long, String>();
                try
                {
                    List<Agency> agencies = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None select a).ToList();
                    List<long> idUserAgencies = new List<long>();
                    if (idUsers.Count() <= maxItemsForQuery)
                        idUserAgencies = (from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && idUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList();
                    else
                    {
                        Int32 index = 0;
                        List<Int32> tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        while (tUsers.Any())
                        {
                            idUserAgencies.AddRange((from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && tUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList());
                            index++;
                            tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                        }
                        idUserAgencies = idUserAgencies.Distinct().ToList();
                    }
                    list = agencies.Where(a => idUserAgencies.Contains(a.Id)).OrderBy(a => a.Name).ToDictionary(a => a.Id, a => a.Name);
                }
                catch (Exception ex)
                {

                }
                return list;
            }

            public List<Int32> GetAvailableProfileTypes(ModuleObject obj,long idMessage=0) {
                List<Int32> types = new List<Int32>();
                try
                {

                    List<Int32> idUsers = GetRecipientsQuery(obj.CommunityID, false, obj, "", 0, idMessage).Where(r => r.IdPerson > 0).Select(r => r.IdPerson).ToList().Distinct().ToList();
                    if (idUsers.Any())
                    {
                        if (idUsers.Count <= maxItemsForQuery)
                            types = (from p in Manager.GetIQ<litePerson>() where idUsers.Contains(p.Id) select p.TypeID).Distinct().ToList();
                        else
                        {
                            Int32 pageIndex = 0;
                            List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (idPagedUsers.Any())
                            {
                                types.AddRange((from p in Manager.GetIQ<litePerson>() where !types.Contains(p.TypeID) && idPagedUsers.Contains(p.Id) select p.Id).ToList().Distinct().ToList());
                                pageIndex++;
                                idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                            types = types.Distinct().ToList();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return types;
            }

            public List<Int32> GetAvailableSubscriptionsIdRoles(ModuleObject obj, Int32 idCommunity, Boolean alsoHidden=false ,long idMessage = 0)
            {
                List<Int32> roles = new List<Int32>();
                try
                {
                    List<Int32> idUsers = GetRecipientsQuery(idCommunity, alsoHidden, obj, "", 0, idMessage).Where(r => r.IdPerson > 0).Select(r => r.IdPerson).ToList().Distinct().ToList();
                    if (idUsers.Count <= maxItemsForQuery)
                    {
                        roles = (from t in Manager.GetIQ<LazySubscription>()
                                 where t.IdCommunity == idCommunity && ((alsoHidden && t.IdRole < 0) || (!alsoHidden && t.IdRole > 0)) && idUsers.Contains(t.IdPerson)
                                 select t.IdRole).Distinct().ToList();
                    }
                    else
                    {
                        roles = (from t in Manager.GetIQ<LazySubscription>()
                                 where t.IdCommunity == idCommunity && ((alsoHidden && t.IdRole < 0) || (!alsoHidden && t.IdRole > 0))
                                 select new { IdRole = t.IdRole, IdPerson = t.IdPerson }).ToList().Where(t => idUsers.Contains(t.IdPerson)).Select(t => t.IdRole).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    roles = new List<Int32>();
                }

                return roles;
            }

            public IEnumerable<MailRecipient> GetRecipientsQuery(Int32 idCommunity, Boolean alsoDeleted, ModuleObject obj = null, String moduleCode = "", Int32 idModule = 0, long idMessage = 0)
            {
                var query = (from r in Manager.GetIQ<MailRecipient>() where (r.Deleted == BaseStatusDeleted.None || (alsoDeleted && r.Deleted == BaseStatusDeleted.Manual)) && r.Ownership != null && (idMessage== 0 || r.Message.Id==idMessage) select r);
                if (obj != null)
                {
                    query = query.Where(m => m.Ownership.ModuleObject != null && m.Ownership.ModuleObject.ObjectLongID == obj.ObjectLongID && m.Ownership.ModuleObject.ObjectTypeID == obj.ObjectTypeID && ((!String.IsNullOrEmpty(obj.ServiceCode) && m.Ownership.ModuleObject.ServiceCode == obj.ServiceCode) || (m.Ownership.ModuleObject.ServiceID == obj.ServiceID && obj.ServiceID > 0)));
                }
                if (idModule > 0 || !String.IsNullOrEmpty(moduleCode))
                {
                    query = query.Where(m => m.Ownership.ModuleCode == moduleCode && m.Ownership.IdModule == idModule);
                }
                if (idCommunity > 0)
                {
                    query = query.Where(m => !m.Ownership.IsPortal && m.Ownership.Community != null && m.Ownership.Community.Id == idCommunity);
                }
                else if (idCommunity == 0)
                {
                    query = query.Where(m => m.Ownership.IsPortal);
                }
                return query;
            }
        #endregion
    }
}