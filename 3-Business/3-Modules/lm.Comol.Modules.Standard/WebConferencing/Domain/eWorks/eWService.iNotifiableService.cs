using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks
{
    public partial class eWService : lm.Comol.Core.Notification.Domain.iNotifiableService
    {
        public List<lm.Comol.Core.Notification.Domain.GroupMessages> GetDefaultNotificationMessages(Core.Notification.Domain.NotificationAction action, int idSenderUser, lm.Comol.Core.Notification.Domain.WebSiteSettings webSiteSettings)
        {
            List<lm.Comol.Core.Notification.Domain.GroupMessages> messages = null;
            Domain.WbRoom room = RoomGet(action.IdObject);
            if (room != null)
            {
                List<Domain.WbUser> users = action.IdModuleUsers.Select(i => UserGet(i)).ToList().Where(u => u != null && u.Id > 0).ToList();

                // INDIVIDUO TUTTE LE POSSIBILI LINGUE DEGLI UTENTI DESTINATARI
                List<String> languageCodes = (users != null) ? users.Select(u => u.LanguageCode).Distinct().ToList() : null;

                Dictionary<String, lm.Comol.Core.Notification.Domain.dtoNotificationMessage> templates = GetTemplates(action.IdModuleAction, Core.Notification.Domain.NotificationChannel.Mail, Core.Notification.Domain.NotificationMode.Automatic, languageCodes);
                if (languageCodes.Any())
                    webSiteSettings.GenerateDateTimeFormat(languageCodes);
                if (templates.Any())
                {
                    lm.Comol.Core.DomainModel.ModuleObject owner = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(room.Id, room, (int)WebConferencing.Domain.ModuleWebConferencing.ObjectType.Room, room.CommunityId, WebConferencing.Domain.ModuleWebConferencing.UniqueCode, Manager.GetModuleID(WebConferencing.Domain.ModuleWebConferencing.UniqueCode));
                    switch (action.IdModuleAction)
                    {
                        case (long)ModuleWebConferencing.MailSenderActionType.Credential:
                        case (long)ModuleWebConferencing.MailSenderActionType.LockUser:
                        case (long)ModuleWebConferencing.MailSenderActionType.UnLockUser:
                        case (long)ModuleWebConferencing.MailSenderActionType.GenericInvitation:
                            messages = new List<lm.Comol.Core.Notification.Domain.GroupMessages>();

                            // Recupero tutti i possibili messaggi con relative traduzioni
                            List<lm.Comol.Core.Notification.Domain.dtoModuleNotificationMessage> tMessages = GetMessages(action.IdModuleAction, action.IdCommunity, room, users, templates, webSiteSettings);
                            // VERIFICO che sia tutto conforme sia in termini di template, che di id versione NEL caso in cui mi serva
                            // in un servizio che mi da diversi template per diversi canali.
                            var groups = (from m in tMessages group m by new {m.IdTemplate, m.IdVersion, m.Channel} into grp select grp.First());

                            foreach (var item in groups)
                            {
                                lm.Comol.Core.Notification.Domain.GroupMessages mGroup = new Core.Notification.Domain.GroupMessages();
                                
                                mGroup.IdCommunity = action.IdCommunity;
                                mGroup.ObjectOwner = owner;
                                mGroup.Settings.Save = false;
                                mGroup.Settings.UniqueIdentifier = Guid.NewGuid();
                                mGroup.Channel = item.Channel;
                                mGroup.Settings.Mail = item.MailSettings;
                                mGroup.Settings.Template.IdTemplate = item.IdTemplate;
                                mGroup.Settings.Template.IdVersion = item.IdVersion;
                                // IN QUESTO CASO SI, perchè recupero direttamente da DB i ltemplate, NON sto inviando un messaggio
                                // da interfaccia web, partendo da un template, ma poi ho modificato il testo predisposto nella UI prima di inviare il messaggio
                                mGroup.Settings.Template.IsCompliant = true;
                                mGroup.Messages = tMessages.Where(t => t.IdTemplate == item.IdTemplate && t.IdVersion == item.IdVersion && t.Channel == item.Channel).Select(t =>
                                                 new lm.Comol.Core.Notification.Domain.dtoModuleTranslatedMessage()
                                                 {
                                                    Recipients=t.Recipients,
                                                    Translation= t.Translation
                                                 }).ToList();
                                messages.Add(mGroup);
                            }
                            break;
                    }
                }
            }
            return messages;
        }

        public List<Core.Notification.Domain.dtoModuleNotificationMessage> GetNotificationMessages(Core.Notification.Domain.NotificationAction action, Core.Notification.Domain.NotificationChannel channel, Core.Notification.Domain.NotificationMode mode, int idSenderUser, lm.Comol.Core.Notification.Domain.WebSiteSettings webSiteSettings)
        {
            List<Core.Notification.Domain.dtoModuleNotificationMessage> messages = null;
            Domain.WbRoom room = RoomGet(action.IdObject);
            if (room != null)
            {
                List<Domain.WbUser> users = action.IdModuleUsers.Select(i => UserGet(i)).ToList().Where(u => u != null && u.Id > 0).ToList();

                // INDIVIDUO TUTTE LE POSSIBILI LINGUE DEGLI UTENTI DESTINATARI
                List<String> languageCodes = (users != null) ? users.Select(u => u.LanguageCode).Distinct().ToList() : null;

                Dictionary<String, lm.Comol.Core.Notification.Domain.dtoNotificationMessage> templates = GetTemplates(action.IdModuleAction, channel, mode, languageCodes);
                if (languageCodes.Any())
                    webSiteSettings.GenerateDateTimeFormat(languageCodes);
                if (templates.Any())
                    messages = GetMessages(action.IdModuleAction, action.IdCommunity, room, users, templates, webSiteSettings);
            }
            return messages;
        }

        private Dictionary<String, lm.Comol.Core.Notification.Domain.dtoNotificationMessage> GetTemplates(long idAction, Core.Notification.Domain.NotificationChannel channel,Core.Notification.Domain.NotificationMode mode, List<String> languageCodes)
        {
            Dictionary<String, lm.Comol.Core.Notification.Domain.dtoNotificationMessage> templates = new Dictionary<string, Core.Notification.Domain.dtoNotificationMessage>();
            if (languageCodes.Any())
                /// RECUPERO I TEMPLATE PER CIASCUNA LINGUA
                templates = languageCodes.ToDictionary(l => l, l => ServiceTemplate.GetNotificationMessage(l, ModuleWebConferencing.UniqueCode, idAction, mode, channel));
            return templates;
        }
        private List<Core.Notification.Domain.dtoModuleNotificationMessage> GetMessages(long idAction,Int32 idCommunity,Domain.WbRoom room,List<Domain.WbUser> users,Dictionary<String,lm.Comol.Core.Notification.Domain.dtoNotificationMessage> templates, lm.Comol.Core.Notification.Domain.WebSiteSettings webSiteSettings)
        {
            List<Core.Notification.Domain.dtoModuleNotificationMessage> messages = null;
            if (templates.Any())
            {
                messages = new List<Core.Notification.Domain.dtoModuleNotificationMessage>();
                lm.Comol.Core.DomainModel.ModuleObject owner = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(room.Id, room, (int)WebConferencing.Domain.ModuleWebConferencing.ObjectType.Room, room.CommunityId, WebConferencing.Domain.ModuleWebConferencing.UniqueCode, Manager.GetModuleID(WebConferencing.Domain.ModuleWebConferencing.UniqueCode));
                switch (idAction)
                {
                    case (long)ModuleWebConferencing.MailSenderActionType.Credential:
                    case (long)ModuleWebConferencing.MailSenderActionType.LockUser:
                    case (long)ModuleWebConferencing.MailSenderActionType.UnLockUser:
                    case (long)ModuleWebConferencing.MailSenderActionType.GenericInvitation:
                        messages = new List<Core.Notification.Domain.dtoModuleNotificationMessage>();
                        foreach (Domain.WbUser user in users)
                        {
                            lm.Comol.Core.Notification.Domain.dtoNotificationMessage template = templates[user.LanguageCode];
                            if (template != null)
                            {
                                lm.Comol.Core.Notification.Domain.dtoModuleNotificationMessage userMessage = new Core.Notification.Domain.dtoModuleNotificationMessage();
                                userMessage.Recipients.Add(new Core.MailCommons.Domain.Messages.Recipient() { Address = user.Mail, DisplayName = user.DisplayName, IdUserModule = user.Id, IdPerson = user.PersonID, IdModuleObject = room.Id, IdCommunity = room.CommunityId, IdModuleType = (int)WebConferencing.Domain.ModuleWebConferencing.ObjectType.Room, Status = Core.MailCommons.Domain.RecipientStatus.Available, LanguageCode = user.LanguageCode });
                                userMessage.Channel = template.Channel;
                                userMessage.MailSettings = template.MailSettings;
                                userMessage.ObjectOwner = owner;
                                userMessage.Save = false;
                                userMessage.IdTemplate = template.IdTemplate;
                                userMessage.IdVersion = template.IdVersion;
                                userMessage.IdCommunity = idCommunity;

                                userMessage.LanguageCode = user.LanguageCode;
                                userMessage.Translation = GetTemplateContentPreview(
                                        true, room.Id, user.Id, webSiteSettings.Baseurl, webSiteSettings.WebSiteUrlNoSsl, template.Translation,
                                        webSiteSettings.GetDateTimeFormat(user.LanguageCode), webSiteSettings.GetVoidDateTime(user.LanguageCode), webSiteSettings.GetPortalName(user.LanguageCode));
                                messages.Add(userMessage);
                            }
                        }
                        break;
                }
            }
            return messages;
        }

    }
}