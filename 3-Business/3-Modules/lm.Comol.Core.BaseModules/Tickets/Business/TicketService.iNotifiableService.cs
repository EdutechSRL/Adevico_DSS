using lm.Comol.Core.Business;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.MailCommons.Domain.Messages;
using lm.Comol.Core.Notification.Domain;
using Org.BouncyCastle.Crypto.Engines;

namespace lm.Comol.Core.BaseModules.Tickets
{
    public partial class TicketService : CoreServices, lm.Comol.Core.Notification.Domain.iNotifiableService
    {
        /// <summary>
        /// Non interessa al servizio Ticket!
        /// </summary>
        /// <param name="action"></param>
        /// <param name="channel"></param>
        /// <param name="mode"></param>
        /// <param name="idSenderUser"></param>
        /// <param name="webSiteSettings"></param>
        /// <returns></returns>
        public List<Notification.Domain.dtoModuleNotificationMessage> GetNotificationMessages(
          Notification.Domain.NotificationAction action,
          Notification.Domain.NotificationChannel channel,
          Notification.Domain.NotificationMode mode,
          int idSenderUser,
          Notification.Domain.WebSiteSettings webSiteSettings)
        {
            return new List<dtoModuleNotificationMessage>();
        }



        public List<lm.Comol.Core.Notification.Domain.GroupMessages> GetDefaultNotificationMessages(
            Notification.Domain.NotificationAction action, 
            int idSenderUser, 
            Notification.Domain.WebSiteSettings webSiteSettings)
        {
            if(action == null)
                return new List<GroupMessages>();

            ModuleTicket.NotificationActionType actionType = ModuleTicket.NotificationActionType.none;

            try
            {
                actionType = (ModuleTicket.NotificationActionType)action.IdModuleAction;
            }
            catch (Exception)
            {
                actionType = ModuleTicket.NotificationActionType.none;
            }

            IList<lm.Comol.Core.Notification.Domain.GroupMessages> outMessages = new List<lm.Comol.Core.Notification.Domain.GroupMessages>();

            if (actionType == ModuleTicket.NotificationActionType.none)
                return outMessages.ToList();
            else if (
                actionType == ModuleTicket.NotificationActionType.CategoriesNotification
                || actionType == ModuleTicket.NotificationActionType.CategoriesReordered
                )
            {
                //CATEGORY Action - TODO: implementazione
                outMessages = GetNotificationAction_Categories(action, webSiteSettings, actionType);
            }
            else if (false)
            {
                //TODO: External Action    
            } 
            else
            {
                outMessages = GetNotificationAction_Ticket(action, webSiteSettings, actionType);
            }

            

            //switch (actionType)
            //{
            //    case ModuleTicket.NotificationActionType.TicketSend:
                    
            //        break;
            //    case ModuleTicket.NotificationActionType.MassageSend:
            //        outMessages = GetNotificationAction_NewMessage(action, webSiteSettings);
            //        break;
            //}

            return outMessages.ToList();
        }


        #region "Get Notification by Action"

        private IList<lm.Comol.Core.Notification.Domain.GroupMessages> GetNotificationAction_Ticket(
            Notification.Domain.NotificationAction action,
            Notification.Domain.WebSiteSettings webSiteSettings,
            ModuleTicket.NotificationActionType actionType)
        {
            if (action.IdObjectType != (long)ModuleTicket.ObjectType.Message)
            {
                return null;
            }

            Domain.SettingsPortal portalSettings = this.PortalSettingsGet();

            if (!portalSettings.IsNotificationUserActive && !portalSettings.IsNotificationManActive)
                return null;

            //TODO : ACTUNG!!!!!!!!!!!!!!!!!
            Int64 senderId = -1;

            if (action.IdModuleUsers != null && action.IdModuleUsers.Any())
                senderId = action.IdModuleUsers.FirstOrDefault();




            //Carico gli oggetti che mi servono
            Int64 messageId = action.IdObject;

            Domain.Message tkMessage = this.MessageGet(messageId);

            if (tkMessage == null || tkMessage.Ticket == null)
                return null;
            
            Domain.Ticket ticket = tkMessage.Ticket;

            IList<Domain.Message> tkMessagesList = ticket.Messages.Where(m => m.SendDate <= tkMessage.SendDate).OrderBy(m => m.SendDate).ToList();

            
            //Definizione Owner/Creator
            TicketUser creatorUser = this.UserGetfromPerson(ticket.CreatedBy.Id);
            TicketUser ownerUser = ticket.Owner;

            if (creatorUser == null && ownerUser == null)
                return null;

            if (ticket.IsBehalf)
            {
                if (ticket.IsHideToOwner)
                {
                    ownerUser = null;
                }
            }
            else if(creatorUser.Id == ownerUser.Id)
            {
                creatorUser = null;
            }

            IList<Domain.DTO.Notification.DTO_UserNotificationData> NotificationUsers = new List<Domain.DTO.Notification.DTO_UserNotificationData>();
            
            List<lm.Comol.Core.Notification.Domain.GroupMessages> messages = new List<lm.Comol.Core.Notification.Domain.GroupMessages>();
            
            ModuleTicket.MailSenderActionType templateActionType_owner = ModuleTicket.MailSenderActionType.none;
            MailSettings ownerSettings = MailSettings.none;
            MailSettings creatorSettings = MailSettings.none;

            ModuleTicket.MailSenderActionType templateActionType_Assigner = ModuleTicket.MailSenderActionType.none;
            List<Domain.TicketUser> assList = null;
            MailSettings assSettings = MailSettings.none;

            ModuleTicket.MailSenderActionType templateActionType_Follower = ModuleTicket.MailSenderActionType.none;
            List<Domain.TicketUser> folList = new List<TicketUser>();
            MailSettings folSettings = MailSettings.none;
            IList<Int64> excludedUserId = null;

            bool allManRes = false;

            //"CORE": parte che in base alle action imposta le varibili in modo corretto.

            SettingsPortal settingsPortal = this.PortalSettingsGet();
            

            switch (actionType)
            {
                case ModuleTicket.NotificationActionType.TicketSend:

                    if (settingsPortal.IsNotificationUserActive)
                    {
                        //Onwer
                        templateActionType_owner = ModuleTicket.MailSenderActionType.TicketNewUser;
                        ownerSettings = MailSettings.NewTicketUsr;
                        creatorSettings = MailSettings.NewTicketUsr;    
                    }

                    if (settingsPortal.IsNotificationManActive)
                    {
                        //Assigner
                        templateActionType_Assigner = ModuleTicket.MailSenderActionType.TicketNewMan;
                        templateActionType_Follower = ModuleTicket.MailSenderActionType.none;
                        assSettings = MailSettings.NewTicketManager;

                        if (ticket.CreationCategory != null)
                        {
                            assList =
                                (from Domain.LK_UserCategory asgn in
                                     this.CategoryGetLkucList(ticket.CreationCategory.Id)
                                 where asgn.User != null
                                 select asgn.User).ToList();
                        }

                        //Followers (none)    
                    }
                    
                    break;

                case ModuleTicket.NotificationActionType.MassageSend:
                    if (settingsPortal.IsNotificationUserActive)
                    {
                        //Onwer
                        templateActionType_owner = ModuleTicket.MailSenderActionType.TicketSendMessageUser;
                        ownerSettings = MailSettings.NewMessageUsr;
                        creatorSettings = MailSettings.NewMessageUsr;
                    }

                    if (settingsPortal.IsNotificationManActive)
                    {
                        //Assigner
                        templateActionType_Assigner = ModuleTicket.MailSenderActionType.TicketSendMessageMan;
                        assSettings = MailSettings.NewMessageManager;
                        assList = AssignerGetStandard(ticket, senderId);

                        //Followers

                        if (assList.Any())
                        {
                            excludedUserId = (from TicketUser usr in assList select usr.Id).Distinct().ToList();
                        }

                        if (excludedUserId == null) excludedUserId = new List<long>();
                        if (ownerUser != null) excludedUserId.Add(ownerUser.Id);
                        if (creatorUser != null) excludedUserId.Add(creatorUser.Id);
                        if (senderId > 0) excludedUserId.Add(senderId);


                        templateActionType_Follower = ModuleTicket.MailSenderActionType.TicketSendMessageMan;
                        //assSettings = MailSettings.NewMessageFollower;
                        folSettings = MailSettings.NewMessageManager;
                    }
                    break;

                case ModuleTicket.NotificationActionType.StatusChanged:
                    if (settingsPortal.IsNotificationUserActive)
                    {
                        //Onwer
                        templateActionType_owner = ModuleTicket.MailSenderActionType.TicketStatusChangedUser;
                        ownerSettings = MailSettings.StatusChangedUsr;
                        creatorSettings = MailSettings.StatusChangedUsr;
                    }

                    if (settingsPortal.IsNotificationManActive)
                    {
                        //Assigner
                        templateActionType_Assigner = ModuleTicket.MailSenderActionType.TicketStatusChangedMan;

                        assSettings = MailSettings.StatusChangedManager;
                        assList = AssignerGetStandard(ticket, senderId);

                        //Followers

                        if (assList.Any())
                        {
                            excludedUserId = (from TicketUser usr in assList select usr.Id).Distinct().ToList();
                        }

                        if (excludedUserId == null) excludedUserId = new List<long>();
                        if (ownerUser != null) excludedUserId.Add(ownerUser.Id);
                        if (creatorUser != null) excludedUserId.Add(creatorUser.Id);
                        if (senderId > 0) excludedUserId.Add(senderId);


                        templateActionType_Follower = ModuleTicket.MailSenderActionType.TicketStatusChangedMan;
                        //assSettings = MailSettings.StatusChangedFollower;
                        folSettings = MailSettings.StatusChangedManager;
                    }
                    break;

                case ModuleTicket.NotificationActionType.ModerationChanged:
                    if (settingsPortal.IsNotificationUserActive)
                    {
                        //Onwer
                        if (tkMessage.ToCondition != TicketCondition.blocked ||
                            tkMessage.ToCondition != TicketCondition.cancelled)
                        {
                            ownerUser = null;
                        }
                        else
                        {
                            templateActionType_owner = ModuleTicket.MailSenderActionType.TicketModeratedUser;
                            ownerSettings = MailSettings.StatusChangedUsr;
                            creatorSettings = MailSettings.StatusChangedUsr;
                        }
                    }

                    if (settingsPortal.IsNotificationManActive)
                    {
                        //Assigner
                        templateActionType_Assigner = ModuleTicket.MailSenderActionType.TicketModeratedMan;

                        assSettings = MailSettings.ModerationChangedMan;

                        assList = AssignerGetStandard(ticket, senderId);

                        //Followers

                        if (assList.Any())
                            excludedUserId = (from TicketUser usr in assList select usr.Id).Distinct().ToList();

                        if (excludedUserId == null) excludedUserId = new List<long>();
                        if (ownerUser != null) excludedUserId.Add(ownerUser.Id);
                        if (creatorUser != null) excludedUserId.Add(creatorUser.Id);
                        if (senderId > 0) excludedUserId.Add(senderId);

                        templateActionType_Follower = ModuleTicket.MailSenderActionType.TicketStatusChangedMan;
                        //assSettings = MailSettings.ModerationChangedFol;
                        folSettings = MailSettings.ModerationChangedMan;
                        allManRes = (tkMessage.ToCondition != TicketCondition.active);
                    }
                    break;

                case ModuleTicket.NotificationActionType.OwnerChanged:
                    if (settingsPortal.IsNotificationUserActive)
                    {
                        //Onwer
                        templateActionType_owner = ModuleTicket.MailSenderActionType.TicketOwnerChanged;
                        creatorSettings = MailSettings.OwnerChanged;
                        ownerUser = null;
                    }
                    //Assigner
                    assList = new List<TicketUser>();

                    //follower
                    break;

                case ModuleTicket.NotificationActionType.AssignmentCategory:
                    if (settingsPortal.IsNotificationUserActive)
                    {
                        //Onwer
                        templateActionType_owner = ModuleTicket.MailSenderActionType.none;
                        ownerSettings = MailSettings.none;
                        creatorSettings = MailSettings.none;
                    }

                    if (settingsPortal.IsNotificationManActive)
                    {
                        //Assigner
                        templateActionType_Assigner = ModuleTicket.MailSenderActionType.TicketCategoryAdd;

                        assSettings = MailSettings.TicketAssCategoryMan;
                        assList = AssignerGetStandard(ticket, senderId);

                        //Followers
                        if (assList.Any())
                            excludedUserId = (from TicketUser usr in assList select usr.Id).Distinct().ToList();
                        if (excludedUserId == null) excludedUserId = new List<long>();
                        if (ownerUser != null) excludedUserId.Add(ownerUser.Id);
                        if (creatorUser != null) excludedUserId.Add(creatorUser.Id);
                        if (senderId > 0) excludedUserId.Add(senderId);

                        templateActionType_Follower = ModuleTicket.MailSenderActionType.TicketCategoryAdd;
                        //assSettings = MailSettings.TicketAssCategoryFol;
                        folSettings = MailSettings.TicketAssCategoryMan;
                    }
                    break;

                case ModuleTicket.NotificationActionType.AssignmentUser:
                    //Onwer
                    templateActionType_owner = ModuleTicket.MailSenderActionType.none;
                    ownerSettings = MailSettings.none;
                    creatorSettings = MailSettings.none;
                    
                    if (settingsPortal.IsNotificationManActive)
                    {
                        //Assigner
                        templateActionType_Assigner = ModuleTicket.MailSenderActionType.TicketAssignmentAddAssigner;
                        assSettings = MailSettings.TicketNewAssignmentAss;

                        assList = new List<TicketUser>();
                        if (tkMessage.ToUser != null) assList.Add(tkMessage.ToUser);

                        //Followers
                        if (assList.Any())
                            excludedUserId = (from TicketUser usr in assList select usr.Id).Distinct().ToList();
                        if (excludedUserId == null) excludedUserId = new List<long>();
                        if (ownerUser != null) excludedUserId.Add(ownerUser.Id);
                        if (creatorUser != null) excludedUserId.Add(creatorUser.Id);
                        if (senderId > 0) excludedUserId.Add(senderId);

                        templateActionType_Follower = ModuleTicket.MailSenderActionType.TicketCategoryAdd;
                        //assSettings = MailSettings.TicketAssCategoryFol;
                        folSettings = MailSettings.TicketAssCategoryMan;
                    }
                    break;

                case ModuleTicket.NotificationActionType.AssignmentReset:
                    if (settingsPortal.IsNotificationUserActive)
                    {
                        //Onwer
                        templateActionType_owner = ModuleTicket.MailSenderActionType.TicketCategoryResetUser;
                        ownerSettings = MailSettings.TicketResetAssUsr;
                        creatorSettings = MailSettings.TicketResetAssUsr;
                    }

                    if (settingsPortal.IsNotificationManActive)
                    {
                        //Assigner
                        templateActionType_Assigner = ModuleTicket.MailSenderActionType.TicketCategoryResetMan;
                        assSettings = MailSettings.TicketResetAssMan;
                        assList = AssignerGetStandard(ticket, senderId);

                        //Followers
                        if (assList.Any())
                            excludedUserId = (from TicketUser usr in assList select usr.Id).Distinct().ToList();
                        if (excludedUserId == null) excludedUserId = new List<long>();
                        if (ownerUser != null) excludedUserId.Add(ownerUser.Id);
                        if (creatorUser != null) excludedUserId.Add(creatorUser.Id);
                        if (senderId > 0) excludedUserId.Add(senderId);

                        templateActionType_Follower = ModuleTicket.MailSenderActionType.TicketCategoryResetMan;
                        //assSettings = MailSettings.TicketResetAssFol;
                        folSettings = MailSettings.TicketResetAssMan;
                    }
                    break;
            }

            
            
            
            //Messagi Owner/Creator
            Domain.DTO.Notification.DTO_UserNotificationData owner = null;
            Domain.DTO.Notification.DTO_UserNotificationData creator = null;

            if (creatorUser != null && creatorUser.IsNotificationActiveUser
                && creatorSettings != MailSettings.none && portalSettings.IsNotificationUserActive)
            {
                MailNotification creatorSets = NotificationGetUser(creatorUser.Id, ticket.Id);

                if ((creatorSets.Settings & creatorSettings) == creatorSettings)
                {
                    creator = UserGetNotificationData(creatorUser);
                }
            }

            if (ownerUser != null && ownerUser.IsNotificationActiveUser
                && ownerSettings != MailSettings.none && portalSettings.IsNotificationUserActive)
            {
                MailNotification ownerSets = NotificationGetUser(ownerUser.Id, ticket.Id);

                if ((ownerSets.Settings & ownerSettings) == ownerSettings)
                {
                    owner = UserGetNotificationData(ownerUser);    
                }

            }
            
            lm.Comol.Core.Notification.Domain.GroupMessages msgGroup_owner = getMessageGroupByUser(
                owner,
                ticket, tkMessage, tkMessagesList,
                null, null,
                webSiteSettings,
                templateActionType_owner
                );

            if (msgGroup_owner != null)
                messages.Add(msgGroup_owner);

            msgGroup_owner = getMessageGroupByUser(
                creator,
                ticket, tkMessage, tkMessagesList,
                null, null,
                webSiteSettings,
                templateActionType_owner
                );
            if (msgGroup_owner != null)
                messages.Add(msgGroup_owner);



            //Altri destinatari
            if (ticket.CreationCategory != null && portalSettings.IsNotificationManActive)
            {
                //// TO//DO : DA RIVEDERE!!!!
                List<long> assIdList = null;   

                //Assigner
                if (assList != null && assList.Any() 
                    && templateActionType_Assigner != ModuleTicket.MailSenderActionType.none 
                    && assSettings != MailSettings.none)
                {
                    lm.Comol.Core.Notification.Domain.GroupMessages msgGroup_ass;

                    assIdList = (
                        from Domain.TicketUser usr 
                            in assList
                            where usr.IsNotificationActiveManager
                        select usr.Id).ToList();


                    IList<Domain.MailNotification> assNotifications =
                        Manager.GetAll<Domain.MailNotification>(
                            mn => mn.Deleted == BaseStatusDeleted.None &&
                                (
                                (mn.IsDefaultManager == false &&
                                assIdList.Contains(mn.User.Id)
                                && (mn.Ticket.Id == ticket.Id || mn.Ticket == null)
                                )
                                || mn.IsPortal == true)
                                );
                    //mn.Settings > MailSettings.Default &&

                    Domain.MailNotification assNotDefault = assNotifications
                    .FirstOrDefault(mn => mn.IsPortal == true) ??
                    new MailNotification();


                    foreach (Domain.TicketUser manRes in assList)
                    {
                        if (manRes.IsNotificationActiveManager)
                        {
                            Domain.MailNotification usrNot =
                                assNotifications.FirstOrDefault
                                (mn => mn.User != null && mn.User.Id == manRes.Id && mn.Ticket != null && mn.Ticket.Id == ticket.Id)
                                ?? assNotifications.FirstOrDefault
                                (mn => mn.User != null && mn.User.Id == manRes.Id && mn.Ticket == null)
                                ?? assNotDefault;

                            if ((usrNot.Settings & assSettings) == assSettings)
                            {

                                msgGroup_ass = getMessageGroupByUser(
                                    UserGetNotificationData(manRes),
                                    ticket, tkMessage, tkMessagesList,
                                    null, null,
                                    webSiteSettings,
                                    templateActionType_Assigner
                                    );

                                if (msgGroup_ass != null)
                                {
                                    messages.Add(msgGroup_ass);
                                }
                            }    
                        }
                    }

                }
                

                // TODO : RIVEDERE PARTE FOLLOWERS!!!
                
                //Followers
                //      =>  excludedUserId
                //allManRes     BOOLEAN     <= Indica se caricare TUTTI i manager/resolver della history...

                if (folList != null && folList.Any() && portalSettings.IsNotificationManActive)
                {
                    lm.Comol.Core.Notification.Domain.GroupMessages msgGroup_fol;

                    IList<Domain.MailNotification> folNotifications =
                        Manager.GetAll<Domain.MailNotification>(
                            mn => mn.Deleted == BaseStatusDeleted.None &&
                                mn.Settings > MailSettings.Default &&
                                mn.Settings != MailSettings.UserDefault && mn.Settings != MailSettings.ManResDefault &&
                                !excludedUserId.Contains(mn.User.Id)
                                );

                    Domain.MailNotification assNotDefault = folNotifications
                    .FirstOrDefault(mn => mn.User.Id == -1) ??
                    new MailNotification();


                    foreach (Domain.TicketUser manRes in assList)
                    {
                        Domain.MailNotification usrNot =
                            folNotifications.FirstOrDefault
                            (mn => mn.User != null && mn.User.Id == manRes.Id)
                            ?? assNotDefault;


                        if ((usrNot.Settings & MailSettings.NewTicketManager) == MailSettings.NewTicketManager)
                        {

                            msgGroup_fol = getMessageGroupByUser(
                                UserGetNotificationData(manRes),
                                ticket, tkMessage, tkMessagesList,
                                null, null,
                                webSiteSettings,
                                ModuleTicket.MailSenderActionType.TicketNewMan
                                );

                            if (msgGroup_fol != null)
                            {
                                messages.Add(msgGroup_fol);
                            }
                        }
                    }
                }

            }

            return messages;
        }

        private List<TicketUser> AssignerGetStandard(Ticket ticket, Int64 senderId)
        {
            List<TicketUser> assList = new List<TicketUser>();
            TicketUser assigner = (from Domain.Assignment ass in ticket.Assignemts
                        where ass.IsCurrent == true && ass.AssignedTo != null
                        select ass.AssignedTo).FirstOrDefault();

            if (assigner != null)
            {
                if (assigner.Id != senderId)
                    assList.Add(assigner);
            }
            else if (ticket.CreationCategory != null)
            {
                assList =
                    (from Domain.LK_UserCategory asgn in
                         this.CategoryGetLkucList(ticket.CreationCategory.Id)
                     where asgn.User != null
                     select asgn.User).ToList();
            }

            return assList;
        }



        private IList<lm.Comol.Core.Notification.Domain.GroupMessages> GetNotificationAction_NewMessage(
            Notification.Domain.NotificationAction action,
            Notification.Domain.WebSiteSettings webSiteSettings)
        {
            if (action.IdObjectType != (long)ModuleTicket.ObjectType.Message)
            {
                return null;
            }



            Int64 notSendNotificationUserId = -1;
            if (action.IdModuleUsers != null && action.IdModuleUsers.Any())
            {
                notSendNotificationUserId = action.IdModuleUsers.FirstOrDefault();
            }



            return null;
        }

        private lm.Comol.Core.Notification.Domain.GroupMessages getMessageGroupByUser(
            Domain.DTO.Notification.DTO_UserNotificationData userData,
            Domain.Ticket ticket,
            Domain.Message tkMessage,
            IList<Domain.Message> tkMessagesList,
            Domain.TicketUser user,
            Domain.Category category,
            Notification.Domain.WebSiteSettings webSiteSettings,
            ModuleTicket.MailSenderActionType ActionType
            )
        {
            if (userData == null || ActionType == ModuleTicket.MailSenderActionType.none)
                return null;

            int comId = (ticket.Community == null) ? 0 : ticket.Community.Id;

            lm.Comol.Core.Notification.Domain.GroupMessages msgGroup = new GroupMessages();
            
            lm.Comol.Core.Notification.Domain.dtoNotificationMessage userTemplate =
                    ServiceTemplate.GetNotificationMessage(userData.LanguageCode, ModuleTicket.UniqueCode,
                        (long)ActionType, NotificationMode.Automatic,
                        userData.Channel, comId);

            if (userTemplate != null)
            {
                dtoModuleTranslatedMessage userMessage = new dtoModuleTranslatedMessage();

                userMessage.Recipients = new List<Recipient>();

                userMessage.Recipients.Add(
                    new Core.MailCommons.Domain.Messages.Recipient()
                    {
                        Address = userData.ChannelAddress,
                        DisplayName = userData.FullUserName,
                        IdUserModule = userData.UserId,
                        IdPerson = userData.PersonId,
                        IdModuleObject = tkMessage.Id,
                        IdCommunity = comId,
                        IdModuleType = (int)ModuleTicket.ObjectType.Message,
                        Status = Core.MailCommons.Domain.RecipientStatus.Available,
                        LanguageCode = userData.LanguageCode
                    });

                TicketUser usr = Manager.Get<TicketUser>(userData.UserId);

                userMessage.Translation = GetTemplateContentPreview(
                    true,
                    webSiteSettings,
                    userTemplate.Translation,
                    ticket, tkMessage, tkMessagesList,
                    null, null, 0,
                    usr,
                    null,
                    userData.LanguageCode);

                //lm.Comol.Core.Notification.Domain.GroupMessages msgGroup = new GroupMessages();

                msgGroup.IdCommunity = comId;
                msgGroup.ObjectOwner =
                    lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(
                        tkMessage.Id,
                        tkMessage,
                        (int)ModuleTicket.ObjectType.Message,
                        comId,
                        ModuleTicket.UniqueCode,
                        Manager.GetModuleID(ModuleTicket.UniqueCode)
                        );
                msgGroup.Settings.Save = false;
                msgGroup.Settings.UniqueIdentifier = Guid.NewGuid();
                msgGroup.Channel = userData.Channel;
                msgGroup.Settings.Mail = userTemplate.MailSettings;
                msgGroup.Settings.Template.IdTemplate = userTemplate.IdTemplate;
                msgGroup.Settings.Template.IdVersion = userTemplate.IdVersion;

                msgGroup.Messages = new List<dtoModuleTranslatedMessage>();
                msgGroup.Messages.Add(userMessage);

                //messages.Add(msgGroup);
            }

            return msgGroup;
        }

       


        #endregion

        //TODO: IMPLEMENTAZIONE!
        private IList<lm.Comol.Core.Notification.Domain.GroupMessages> GetNotificationAction_Categories(
            Notification.Domain.NotificationAction action,
            Notification.Domain.WebSiteSettings webSiteSettings,
            ModuleTicket.NotificationActionType actionType)
        {
            IList<lm.Comol.Core.Notification.Domain.GroupMessages> outMessageses = new List<GroupMessages>();

            int idCommunity = action.IdCommunity;
            if (idCommunity <= 0)
                return outMessageses;


            Int64 CategoryId = 0;


            if (action.IdObjectType == (long)ModuleTicket.ObjectType.Category)
            {
                CategoryId = (int) action.IdObject;
            }

            switch (actionType)
            {
                case ModuleTicket.NotificationActionType.CategoriesNotification:
                  
                    //Utenti interessati alla notifica
                    IList<TicketUser> Users = new List<TicketUser>();

                    //Link utenti-categorie a seconda delle impostaizoni.
                    IList<LK_UserCategory> allLK_ucs = new List<LK_UserCategory>();



                    bool allUsers = (action.IdModuleUsers == null 
                        || action.IdModuleUsers.Contains((long)ModuleTicket.NotificationActionCategoryUserReceiver.All));

                    bool allManagers = !allUsers 
                        && (action.IdModuleUsers != null 
                        && action.IdModuleUsers.Contains((long)ModuleTicket.NotificationActionCategoryUserReceiver.Managers));
                    bool allResolvers = !allManagers
                        && (action.IdModuleUsers != null
                        && action.IdModuleUsers.Contains((long)ModuleTicket.NotificationActionCategoryUserReceiver.Managers));

                    
                    if (!allResolvers && !allManagers && !allUsers) //(action.IdModuleUsers != null && action.IdModuleUsers.Any())
                    {
                        //SOLO utenti INDICATI!
                        Users =
                            Manager.GetAll<TicketUser>(
                                u => action.IdModuleUsers.Contains(u.Id) && u.Deleted == BaseStatusDeleted.None).Distinct().ToList();

                        if (CategoryId > 0)
                        {
                            allLK_ucs =
                                Manager.GetAll<LK_UserCategory>(
                                    lk =>
                                        lk.Category != null && lk.Category.Id == CategoryId &&
                                        lk.Deleted == BaseStatusDeleted.None);
                        } 
                        else 
                        {
                            allLK_ucs =
                                Manager.GetAll<LK_UserCategory>(
                                    lk =>
                                        lk.Category != null && lk.Category.IdCommunity == idCommunity &&
                                        lk.Deleted == BaseStatusDeleted.None);
                        }

                    }
                    else 
                    {
                        if (allUsers)
                        { 
                            if (CategoryId > 0)
                            {
                                allLK_ucs =
                                    Manager.GetAll<LK_UserCategory>
                                        (lu =>
                                            lu.Category != null && lu.Category.Id == CategoryId &&
                                            lu.Deleted == BaseStatusDeleted.None && lu.User != null);

                                //TUTTI gli utenti associati a Categorie della comunità
                                Users = (from LK_UserCategory lkuc in allLK_ucs
                                    select lkuc.User
                                    ).Distinct().ToList();
                            }
                            else
                            {
                                allLK_ucs = Manager.GetAll<LK_UserCategory>
                                    (lu =>
                                        lu.Category != null && lu.Category.IdCommunity == idCommunity &&
                                        lu.Deleted == BaseStatusDeleted.None && lu.User != null);

                                Users = (from LK_UserCategory lkuc
                                         in allLK_ucs
                                         select lkuc.User
                                        ).Distinct().ToList();
                            }
                        }
                        else
                        {
                            if (CategoryId > 0)
                            {
                                allLK_ucs =
                                    Manager.GetAll<LK_UserCategory>
                                        (lu =>
                                            lu.Category != null && lu.Category.Id == CategoryId &&
                                            lu.Deleted == BaseStatusDeleted.None && lu.User != null
                                            && lu.IsManager == allManagers);

                                //TUTTI gli utenti associati a Categorie della comunità
                                Users = (from LK_UserCategory lkuc in allLK_ucs
                                         select lkuc.User
                                    ).Distinct().ToList();
                            }
                            else
                            {
                                allLK_ucs = Manager.GetAll<LK_UserCategory>
                                    (lu =>
                                        lu.Category != null && lu.Category.IdCommunity == idCommunity &&
                                        lu.Deleted == BaseStatusDeleted.None && lu.User != null
                                        && lu.IsManager == allManagers);

                                Users = (from LK_UserCategory lkuc
                                         in allLK_ucs
                                         select lkuc.User
                                        ).Distinct().ToList();
                            }
                        }
                    }
                    //Ora ho Utenti e categorie che mi servono (lista piana!)

                    foreach (TicketUser usr in Users)
                    {
                        GroupMessages msgUser = new GroupMessages();

                        DTO_UserNotificationData userData = UserGetNotificationData(usr);

                        
                        lm.Comol.Core.Notification.Domain.dtoNotificationMessage userTemplate =
                            ServiceTemplate.GetNotificationMessage(
                                userData.LanguageCode, ModuleTicket.UniqueCode,
                                (long)ModuleTicket.MailSenderActionType.CategoryReorder, 
                                NotificationMode.Automatic,
                                userData.Channel, 
                                idCommunity);

                        dtoModuleTranslatedMessage userMessage = new dtoModuleTranslatedMessage();

                        userMessage.Recipients = new List<Recipient>();

                        userMessage.Recipients.Add(
                            new Core.MailCommons.Domain.Messages.Recipient()
                            {
                                Address = userData.ChannelAddress,
                                DisplayName = userData.FullUserName,
                                IdUserModule = userData.UserId,
                                IdPerson = userData.PersonId,
                                IdModuleObject = usr.Id,
                                IdCommunity = idCommunity,
                                IdModuleType = (int)ModuleTicket.ObjectType.User,
                                Status = Core.MailCommons.Domain.RecipientStatus.Available,
                                LanguageCode = userData.LanguageCode
                            });

                        IList<Category> manCategories = new List<Category>();
                        IList<Category> resCategories = new List<Category>();

                        if (CategoryId > 0)
                        {
                            manCategories = (from LK_UserCategory lk in allLK_ucs
                                             where lk.Category.Id == CategoryId
                                                && lk.IsManager == true
                                             select lk.Category).Distinct().ToList();

                            resCategories = (from LK_UserCategory lk in allLK_ucs
                                             where lk.Category.Id == CategoryId
                                                && lk.IsManager == false
                                             select lk.Category).Distinct().ToList();
                        }
                        else
                        {
                            manCategories = (from LK_UserCategory lk in allLK_ucs
                                                         where lk.Category != null 
                                                            && lk.Category.Father == null
                                                            && lk.User != null
                                                            && lk.User.Id == usr.Id
                                                            && lk.IsManager == true
                                                        select lk.Category).Distinct().ToList();

                            resCategories = (from LK_UserCategory lk in allLK_ucs
                                                             where lk.Category != null
                                                                && lk.User != null
                                                                && lk.User.Id == usr.Id
                                                                && lk.IsManager == false
                                                             select lk.Category).Distinct().ToList();
                        }
                        
                        //Attenzione. Lo userData.FullUserName viene passato come nome, ed il cognome passato come vuoto, 
                        userMessage.Translation = GetTemplateContentPreview(
                            true,
                            webSiteSettings,
                            userTemplate.Translation,
                            null, null, null,
                            manCategories, resCategories, idCommunity,
                            usr, null,
                            userData.LanguageCode);

                        lm.Comol.Core.Notification.Domain.GroupMessages msgGroup = new GroupMessages();

                        msgGroup.IdCommunity = idCommunity;
                        msgGroup.ObjectOwner =
                            lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(
                                usr.Id,
                                usr,
                                (int)ModuleTicket.ObjectType.User,
                                idCommunity,
                                ModuleTicket.UniqueCode,
                                Manager.GetModuleID(ModuleTicket.UniqueCode)
                                );
                        msgGroup.Settings.Save = false;
                        msgGroup.Settings.UniqueIdentifier = Guid.NewGuid();
                        msgGroup.Channel = userData.Channel;
                        msgGroup.Settings.Mail = userTemplate.MailSettings;
                        msgGroup.Settings.Template.IdTemplate = userTemplate.IdTemplate;
                        msgGroup.Settings.Template.IdVersion = userTemplate.IdVersion;

                        msgGroup.Messages = new List<dtoModuleTranslatedMessage>();
                        msgGroup.Messages.Add(userMessage);

                        outMessageses.Add(msgGroup);
                    }

                    break;

                case ModuleTicket.NotificationActionType.CategoriesReordered:
                    //TODO: al momento lasciamo perdere...
                    break;
            }

            return outMessageses;
        }

        #region Analyze Content
        /// <summary>
        /// Conversione dei TAG - TODO: Categories List (al momento passo NULL!!!) - sempre che serva...
        /// </summary>
        /// <param name="isHtml">SE l'output è HTML o no</param>
        /// <param name="webSiteSettings">Impostazioni (contiene le traduzioni)</param>
        /// <param name="content">Contenuto</param>
        /// <param name="ticket">Ticket: se "null" i Tag vengono rimossi</param>
        /// <param name="tkMessage">Messaggio di riferimento: se "null" viene usato l'ultimo messaggio presente nei ticket</param>
        /// <param name="recipientName">Stringa con nome destinatario</param>
        /// <param name="recipientSurname">Stringa con cognome destinatario</param>
        /// <param name="user">Utente: se "null" i Tag vengono rimossi (x Iscrizione)</param>
        /// <param name="category">Categoria: se "null" i Tag vengono rimossi (Management category)</param>
        /// <param name="langCode">Codice lingua di riferimento</param>
        /// <remarks>
        /// SE viene passato uno USER VALIDO, vengono usati i suoi dati per impostare i parametri [UserName] ed [UserSurname],
        /// altrimenti vengono usate le stringhe "recipientName" e "recipientSurname". ToDo: rivedere!
        /// </remarks>
        /// <returns></returns>
        private lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTemplateContentPreview(
            Boolean isHtml,
            Notification.Domain.WebSiteSettings webSiteSettings,
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content,
            Domain.Ticket ticket,
            Domain.Message tkMessage,
            IList<Domain.Message> tkMessagesList,
            IList<Category> manCategories,
            IList<Category> resCategories,
            int CommunityId,
            Domain.TicketUser user,
            Domain.Category category,
            String langCode)
        {



            //if (ticket != null && tkMessage != null)
            //{
                //Alterazione content
                if (!String.IsNullOrEmpty(content.Body) && content.Body.Contains(TemplatePlaceHolders.OpenTag) && content.Body.Contains(TemplatePlaceHolders.CloseTag))
                {
                    content.Body = AnalyzeContent(
                        isHtml,
                        content.Body,
                        langCode,
                        ticket, tkMessage, tkMessagesList,
                        user,
                        category, manCategories, resCategories, CommunityId,
                        webSiteSettings.GetDateTimeFormat(langCode), 
                        webSiteSettings.GetVoidDateTime(langCode),
                        webSiteSettings.Baseurl,
                        webSiteSettings.WebSiteUrlNoSsl,
                        webSiteSettings.GetModuleTranslationKeys(ModuleTicket.UniqueCode, langCode)
                        );
                }

            //Boolean isHtml,
            //String content,
            //String langCode,
            //Domain.Ticket ticket,
            //Domain.Message tkMessage,
            //string dateTimeFormat, string voidDateTime,
            //string baseUrl, String webSiteUrlNoSsl)

                //if (!String.IsNullOrEmpty(content.Name) && content.Name.Contains(openTag) && content.Name.Contains(closeTag))
                //{

                //}

                if (!String.IsNullOrEmpty(content.ShortText) &&
                    content.ShortText.Contains(TemplatePlaceHolders.OpenTag) &&
                    content.ShortText.Contains(TemplatePlaceHolders.CloseTag))
                {
                    content.ShortText = AnalyzeContent(
                        isHtml,
                        content.ShortText,
                        langCode,
                        ticket, tkMessage, tkMessagesList,
                        user,
                        category, manCategories, resCategories, CommunityId,
                        webSiteSettings.GetDateTimeFormat(langCode),
                        webSiteSettings.GetVoidDateTime(langCode),
                        webSiteSettings.Baseurl,
                        webSiteSettings.WebSiteUrlNoSsl,
                        webSiteSettings.GetModuleTranslationKeys(ModuleTicket.UniqueCode, langCode)
                        );
                }

                if (!String.IsNullOrEmpty(content.Subject) &&
                    content.Subject.Contains(TemplatePlaceHolders.OpenTag) &&
                    content.Subject.Contains(TemplatePlaceHolders.CloseTag))
                {
                    content.Subject = AnalyzeContent(
                        isHtml,
                        content.Subject,
                        langCode,
                        ticket, tkMessage, tkMessagesList,
                        user,
                        category, manCategories, resCategories, CommunityId,
                        webSiteSettings.GetDateTimeFormat(langCode),
                        webSiteSettings.GetVoidDateTime(langCode),
                        webSiteSettings.Baseurl,
                        webSiteSettings.WebSiteUrlNoSsl,
                        webSiteSettings.GetModuleTranslationKeys(ModuleTicket.UniqueCode, langCode)
                        );
                    //recipientName, recipientSurname, 
                }
            //}

            return content;
        }


        private String AnalyzeContent(
            Boolean isHtml,
            String content,
            String langCode,
            Domain.Ticket ticket,
            Domain.Message tkMessage,
            IList<Domain.Message> tkMessagesList,
            Domain.TicketUser user,
             Domain.Category category,
            IList<Domain.Category> managerCategories,
            IList<Domain.Category> userCategories,
            int idCommunity,
            string dateTimeFormat, string voidDateTime,
            string baseUrl, String webSiteUrlNoSsl,
            IDictionary<string, string> translation
            )
        {
            //TOLTO: String recipientName, String recipientSurname,

            if (idCommunity <= 0)
            {
                if (ticket != null && ticket.Community != null)
                    idCommunity = ticket.Community.Id;
                else if (category != null)
                    idCommunity = category.IdCommunity;
            }


            //Dati Ticket
            content = AnalyseContentTicket(
                isHtml,
                content,
                langCode,
                ticket,
                tkMessage, tkMessagesList,
                dateTimeFormat, voidDateTime,
                baseUrl,
                translation
                );

            //if (user == null)
            //{
            //    //recipient
            //    content = AnalyseContentRecipient(
            //        isHtml,
            //        content,
            //        langCode,
            //        recipientName,
            //        recipientSurname
            //        );
            //}
            
            //User -> SOLO per ISCRIZIONE (user!=null), altrimenti usa recipient. Lasciato per togliere eventuali tag inutili.
            //DA FINIRE x questione TOKEN/URL! (lo lascerei diretto, però...)
            content = AnalyseContentUser(
                isHtml,
                content,
                langCode,
                user,
                dateTimeFormat, voidDateTime,
                baseUrl,
                translation
                );    
            
            
            //Category (x Management) - TODO: VERIFICARE!
            content = AnalyseContentCategory(
                isHtml,
                content,
                langCode,
                category,
                dateTimeFormat, voidDateTime,
                baseUrl,
                translation
                );

            //Categories (x Management)!
            content = AnalyseContentCategories(
                isHtml,
                content,
                langCode,
                managerCategories,
                userCategories,
                idCommunity,
                dateTimeFormat, voidDateTime,
                baseUrl,
                translation
                );

            return content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHtml"></param>
        /// <param name="content"></param>
        /// <param name="langCode"></param>
        /// <param name="ticket"></param>
        /// <param name="tkMessage"></param>
        /// <param name="messages"></param>
        /// <param name="dateTimeFormat"></param>
        /// <param name="voidDateTimeFormat"></param>
        /// <param name="baseUrl"></param>
        /// <param name="translation"></param>
        /// <param name="changeLoginUrl"></param>
        /// <returns></returns>
        /// <remarks>
        /// SE il ticket è vuoto, tolgo TUTTI i tag.
        /// SE la lista messaggi è vuota
        ///        e SE ho un messaggio, recupero tutti i messaggi da Ticket fino a quello
        ///        altrimenti recupero TUTTI i messaggi da Ticket.
        /// SE il messaggio di riferimento è vuoto, recupero dalla lista di messaggi
        /// 
        /// ToDo: verificare e testare!
        /// </remarks>
        private String AnalyseContentTicket(
            Boolean isHtml,
            String content,
            String langCode,
            Domain.Ticket ticket,
            Domain.Message tkMessage, IList<Domain.Message> messages,
            string dateTimeFormat,
            string voidDateTimeFormat,
            string baseUrl,
            IDictionary<string, string> translation,
            bool changeLoginUrl = true)
        {
            bool cleanText = false;


            if (ticket == null)
                cleanText = true;
            else if (tkMessage == null)// && messages != null && messages.Any())
            {
                if (messages == null || !messages.Any())
                {
                    messages = ticket.Messages.OrderByDescending(m => m.SendDate).ToList();
                }

                if (messages.Any())
                {
                    tkMessage = messages.FirstOrDefault();
                }
            } else if (messages == null || ! messages.Any())
            {
                messages = ticket.Messages.Where(m => m.SendDate <= tkMessage.SendDate).OrderByDescending(m => m.SendDate).ToList();
            }

            if (!cleanText && tkMessage == null)
                cleanText = true;

            int comId = 0;
            
            if(ticket != null && ticket.Community != null)
                comId =  ticket.Community.Id;







            if(!cleanText)
            {   
                //NOTA: tutto si basa sui messaggi inviati FINO al messaggio corrente!
                //Ogni azione su un ticket, invia comunque un messaggio, anche di sistema.
                //Questo evita che modifiche avvenute tra l'azione e l'invio causino disallineamenti nei dati proposti nella mail.
               // IList<Domain.Message> messages = ticket.Messages;
                //.Where(m => m.SendDate <= tkMessage.SendDate).ToList();


                //Assegnatario corrente
                Domain.TicketUser tkAssUsr = (from Domain.Message msg in
                                                  (messages.Where(m => m.ToUser != null).OrderByDescending(m => m.SendDate))
                                              select msg.ToUser).FirstOrDefault();

                if (tkAssUsr != null)
                {
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.TicketAssigner), tkAssUsr.Person == null ? string.Format("{0} {1}", tkAssUsr.Sname, tkAssUsr.Name) : tkAssUsr.Person.SurnameAndName);
                }
                else
                {
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.TicketAssigner),
                        translation[string.Format("Assignment.none")]);
                }

                //Categoria corrente
                Domain.Category currentCategory = (from Domain.Message msg in
                                                       (messages.Where(m => m.ToCategory != null).OrderByDescending(m => m.SendDate))
                                                   select msg.ToCategory).FirstOrDefault();
                if (currentCategory == null)
                    currentCategory = ticket.CreationCategory;

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketCategoryCurrent), currentCategory.GetTranslatedName(langCode));

                //Categoria di creazione
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketCategoryInitial), ticket.CreationCategory.GetTranslatedName(langCode));

                //Codice Ticket
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketCode), ticket.Code);

                //Creatore
                string ownerName = (ticket.Owner.Person != null)
                    ? ticket.Owner.Person.SurnameAndName
                    : string.Format("{0} {1}", ticket.Owner.Sname, ticket.Owner.Name);

                if (ticket.IsBehalf)
                {
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.TicketCreatorDisplayName), ticket.CreatedBy.SurnameAndName);
                }
                else
                {
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.TicketCreatorDisplayName), ownerName);
                }
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.TicketOwnerDisplayName),
                        ownerName
                        );
                

                //Lingua ticket
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketLanguage), Manager.GetLanguage(ticket.LanguageCode).Name);

                //Codice lingua
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketLanguageCode), ticket.LanguageCode);

                Domain.Message firstMsg = messages.OrderBy(m => m.SendDate).FirstOrDefault();

                if (firstMsg == null)
                    return content;

                //Ticket testo
                String ticketText = "";

                if (isHtml)
                {
                    ticketText = firstMsg.Text;
                }
                if (string.IsNullOrEmpty(ticketText))
                    ticketText = ticket.Preview;
                
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketLongText), ticketText);

                //Subject/Title
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketObject), ticket.Title);

                //preview
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketPreview), ticket.Preview);

                //Data invio
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketSendDate), firstMsg.SendDate.ToString(dateTimeFormat));

                //Stato
                //Domain.Message lastmsg = messages.OrderBy(m => m.SendDate).LastOrDefault();

                TicketStatus status = (tkMessage != null) ? tkMessage.ToStatus : (Domain.Enums.TicketStatus.open);

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketStatus), translation[string.Format("Status.{0}", status.ToString())]);
                //Status.open

                //URL 
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketUrlListManager), string.Format("{0}{1}", baseUrl, RootObject.TicketListResolver(comId)));

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketUrlManager), string.Format("{0}{1}", baseUrl, RootObject.TicketEditResolver(comId, ticket.Code)));

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketUrlUser), string.Format("{0}{1}", baseUrl, RootObject.TicketEditUser(comId, ticket.Code)));

                if (changeLoginUrl)
                {
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ExternalAccessUrl),
                    string.Format("{0}{1}", baseUrl, RootObject.ExternalLogin()));    
                }


                //ActionDisplayName - lastmsg 
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.ActionDisplayName),
                    (tkMessage.Creator.Person != null) ?
                    tkMessage.Creator.Person.SurnameAndName :
                    string.Format("{0} {1}", tkMessage.Creator.Sname, tkMessage.Creator.Name));
                //&& lastmsg.Creator.d
                
                //ActionRole = 36,
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.ActionRole), "");

                String ShortText = ValidatePreview(tkMessage.Preview, tkMessage.Text);
                if (isHtml)
                {
                    //AnswerShortText = 40,
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.AnswerShortText), ShortText);

                    //AnswerFullText = 41,
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.AnswerFullText), tkMessage.Text);
                }
                else
                {
                    //AnswerShortText = 40,
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.AnswerShortText), HtmlToText(ShortText));

                    //AnswerFullText = 41,
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.AnswerFullText), HtmlToText(tkMessage.Text));
                }
            }
            else
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                PlaceHoldersType.TicketAssigner), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketCategoryCurrent), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketCategoryInitial), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketCode), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketCreatorDisplayName), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketLanguage), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketLanguageCode), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketLongText), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketObject), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketPreview), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketSendDate), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketStatus), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketUrlListManager), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketUrlManager), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.TicketUrlUser), "");

                if (changeLoginUrl)
                {
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ExternalAccessUrl), "");
                }

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.ActionDisplayName), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.ActionRole), "");

                //AnswerShortText = 40,
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.AnswerShortText), "");

                //AnswerFullText = 41,
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.AnswerFullText), "");
            }

            return content;
        }

        ///// <summary>
        ///// Rivedere!
        ///// </summary>
        ///// <param name="isHtml"></param>
        ///// <param name="content"></param>
        ///// <param name="langCode"></param>
        ///// <param name="userName"></param>
        ///// <returns></returns>
        //private String AnalyseContentRecipient(
        //    Boolean isHtml,
        //    String content,
        //    String langCode,
        //    String userName,
        //    String userSurname)
        //{
        //    //[UserName]
        //    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
        //           PlaceHoldersType.UserName), userName);
        //    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
        //           PlaceHoldersType.UserSurname), userSurname);

        //    return content;
        //}

        /// <summary>
        /// Converte i tag "User" (x registrazione!) VEDI REMARK!
        /// </summary>
        /// <param name="isHtml"></param>
        /// <param name="content"></param>
        /// <param name="langCode"></param>
        /// <param name="user"></param>
        /// <param name="dateTimeFormat"></param>
        /// <param name="voidDateTimeFormat"></param>
        /// <param name="baseUrl"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        /// <remarks>GESTIONE TOKEN e PASSWORD!</remarks>
        private String AnalyseContentUser(
            Boolean isHtml,
            String content,
            String langCode,
            Domain.TicketUser user,
            string dateTimeFormat,
            string voidDateTimeFormat,
            string baseUrl,
            IDictionary<string, string> translation)
        {
            if (user != null)
            {

                if (user.Person != null)
                {
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.UserMail), user.Person.Mail);

                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.UserName), user.Person.Name);

                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.UserSurname), user.Person.Surname);
                }
                else
                {
                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.UserMail), user.mail);

                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.UserName), user.Name);

                    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.UserSurname), user.Sname); //string.Format("{0} {0}", user.Sname)'
                }

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.UserLanguageCode), user.LanguageCode);

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.UserIdCode), user.UsrCode);

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.UserNotificationSettingsUrl), RootObject.SettingsUser(0));
                

                //  TODO - Tag utenti esterni: registrazione.
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                        PlaceHoldersType.UserPassword), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.UserToken), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.UserTokenExpiration), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                    PlaceHoldersType.UserTokenUrl), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ExternalAccessUrl),
                    string.Format("{0}{1}", baseUrl, RootObject.ExternalLogin()));    //ADD TOKEN!
                
                

            }
            else
            {

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.UserLanguageCode), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.UserMail), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.UserName), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.UserPassword), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.UserSurname), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.UserToken), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.UserTokenExpiration), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.UserTokenUrl), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ExternalAccessUrl), "");

            }
            
            return content;
        }

        //TODO: TOGLIERE in favore di AnalyseContentCategories
        private String AnalyseContentCategory(
           Boolean isHtml,
           String content,
           String langCode,
           Domain.Category category,
           string dateTimeFormat,
           string voidDateTimeFormat,
           string baseUrl,
           IDictionary<string, string> translation)
        {

            if (category != null)
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                            PlaceHoldersType.CategoryName), category.GetTranslatedName(langCode));

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                            PlaceHoldersType.CategoryDescription), category.GetTranslatedDescription(langCode));


                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                            PlaceHoldersType.CategoryType), translation[string.Format("CategoryType.{0}", category.Type.ToString())]);

                //TODO:
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                            PlaceHoldersType.CategoryLinkListSimple), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                            PlaceHoldersType.CategoryLinkListFull), "");
                
                
            }
            else
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                         PlaceHoldersType.CategoryName), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                            PlaceHoldersType.CategoryDescription), "");

                //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                //            PlaceHoldersType.CategoryLONGNameAndDescriptionLIST), "");

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                            PlaceHoldersType.CategoryType), "");

                //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                //            PlaceHoldersType.CategoryLanguagesCodeList), "");

                //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                //            PlaceHoldersType.CategoryNewAssignerDisplayName), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                           PlaceHoldersType.CategoryLinkListSimple), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                            PlaceHoldersType.CategoryLinkListFull), "");
            }
            


            return content;
        }



        private String AnalyseContentCategories(
           Boolean isHtml,
           String content,
           String langCode,
           IList<Domain.Category> manCategories,
           IList<Domain.Category> usrCategories,
           int idCommunity,
           string dateTimeFormat,
           string voidDateTimeFormat,
           string baseUrl,
           IDictionary<string, string> translation)
        {

            //TODO: verifica PERFORMANCE!

            if (!content.Contains(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.CategoryLinkListFull))
                || !content.Contains(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.CategoryLinkListSimple)))
            {
                return content;
            }



            if ((manCategories == null || !manCategories.Any())
                && (usrCategories == null || !usrCategories.Any()))
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.CategoryLinkListSimple), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.CategoryLinkListFull), "");

                return content;
            }


            string formatContainer = "";

            string subItemsContainer = "";

            string simpleFormatElement = "";
            string fullFormatElement = "";
            
            if (isHtml)
            {
                formatContainer = "<span>{0}</span>:<br/><ul>{1}</ul><br/>" + System.Environment.NewLine;
                subItemsContainer = "<ul>{0}</ul>" + System.Environment.NewLine;

                simpleFormatElement = "<li><a href=\"{0}\" target=\"_blank\">{1}</a>{2}</li>" + System.Environment.NewLine;
                fullFormatElement = "<li><a href=\"{0}\" target=\"_blank\">{1}</a><br/><span>{2}</span>{3}</li>" + System.Environment.NewLine;
            }
            else
            {
                formatContainer = "{0}" + System.Environment.NewLine + "{1}";// + System.Environment.NewLine;
                
                subItemsContainer = "{0}";

                simpleFormatElement = "{1}:" + System.Environment.NewLine + "{0}" + System.Environment.NewLine + "{2}";
                fullFormatElement = "{1}:" + System.Environment.NewLine + "{2}" + System.Environment.NewLine + "{0}" + System.Environment.NewLine + "{3}";
            }

            
            //MANAGER!
            string manContenSimple = "";
            string manContenFULL = "";

            //string manager = "";
            //string resolver = "";

            if (manCategories != null && manCategories.Any())
            {
                foreach (Category cat in manCategories)
                {
                    string name = cat.GetTranslatedName(langCode);
                    string description = cat.GetTranslatedDescription(langCode);
                    string subItemsSimple = "";
                    string subItemsFull = "";
                    
                    //Creo la lista di elementi figli
                    AnalyzeRecoursiveCategory(
                        langCode,
                        cat.Children,
                        idCommunity,
                        simpleFormatElement,
                        fullFormatElement,
                        subItemsContainer,
                        "", isHtml,
                        ref subItemsSimple,
                        ref subItemsFull);

                     //AnalyzeRecoursiveCategory(
                     //langCode,
                     //cat.Children,
                     //idCommunity,
                     //simpleFormatElement,
                     //fullFormatElement,
                     //subItemsContainer,
                     //ref simpleInnerChild,
                     //ref fullInnerChild);


                    //Aggiungo il contenitore per gli elementi figli
                    if (!String.IsNullOrEmpty(subItemsSimple))
                    {
                        subItemsSimple = string.Format(subItemsContainer, subItemsSimple);
                    }
                    if (!String.IsNullOrEmpty(subItemsFull))
                    {
                        subItemsFull = string.Format(subItemsContainer, subItemsFull);
                    }

                    //Creo l'elemento completo.
                    manContenSimple += string.Format(simpleFormatElement,
                        RootObject.TicketListResolver(idCommunity, cat.Id), name, subItemsSimple);
                    manContenFULL += string.Format(fullFormatElement,
                        RootObject.TicketListResolver(idCommunity, cat.Id), name, description, subItemsFull);


                }
            }

            string manTranslation = translation["CategoryUserType.Manager"];
            if (String.IsNullOrEmpty(manTranslation))
                manTranslation = "Manager";

            if (!String.IsNullOrEmpty(manContenSimple))
            {
                manContenSimple = string.Format(formatContainer, manTranslation, manContenSimple);
            }
            if (!String.IsNullOrEmpty(manContenFULL))
            {
                manContenFULL = string.Format(formatContainer, manTranslation, manContenFULL);
            }


            //RESOLVER
            string resContenSimple = "";
            string resContenFULL = "";


            if (usrCategories != null && usrCategories.Any())
            {
                foreach (Category cat in usrCategories)
                {
                    string name = cat.GetTranslatedName(langCode);
                    string description = cat.GetTranslatedDescription(langCode);
                    
                    //Creo l'elemento completo.
                    resContenSimple += string.Format(simpleFormatElement,
                        RootObject.TicketListResolver(idCommunity, cat.Id), name, "");
                    resContenFULL += string.Format(fullFormatElement,
                        RootObject.TicketListResolver(idCommunity, cat.Id), name, description, "");


                }
            }

            string resTranslation = translation["CategoryUserType.Resolver"];
            if (String.IsNullOrEmpty(resTranslation))
                resTranslation = "Resolver";

            if (!String.IsNullOrEmpty(resContenSimple))
            {
                resContenSimple = string.Format(formatContainer, resTranslation, resContenSimple);
            }
            if (!String.IsNullOrEmpty(manContenFULL))
            {
                resContenFULL = string.Format(formatContainer, resTranslation, resContenFULL);
            }

            //Creazione finale content
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.CategoryLinkListSimple),
                       string.Format("{0}" + System.Environment.NewLine + "{1}", manContenSimple, resContenSimple));
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
                       PlaceHoldersType.CategoryLinkListFull),
                       string.Format("{0}" + System.Environment.NewLine + "{1}", manContenFULL, resContenFULL));

            return content;
        }

        private void AnalyzeRecoursiveCategory(
            String langCode,
            IList<Domain.Category> categories,
            Int32 idCommunity,
            string simpleFormatElement,
            string fullFormatElement,
            string subItemsContainer,
            string pre, bool isHtml,
            ref string simplecontent,
            ref string fullContent)
        {
            if(!isHtml)
                pre += " - ";


            if (categories == null || !categories.Any())
            {
                return;
            }

            foreach (Category cat in categories)
            {
                string simpleInnerChild = "";
                string fullInnerChild = "";

               AnalyzeRecoursiveCategory(
                    langCode,
                    cat.Children,
                    idCommunity,
                    simpleFormatElement,
                    fullFormatElement,
                    subItemsContainer,
                    pre, isHtml,
                    ref simpleInnerChild,
                    ref fullInnerChild);
                
                string name = string.Format("{0}{1}", pre, cat.GetTranslatedName(langCode));
                string description = cat.GetTranslatedDescription(langCode);

                //Aggiungo il contenitore per gli elementi figli
                if (!String.IsNullOrEmpty(simpleInnerChild))
                {
                    simpleInnerChild = string.Format(subItemsContainer, simpleInnerChild);
                }
                if (!String.IsNullOrEmpty(fullInnerChild))
                {
                    fullInnerChild = string.Format(subItemsContainer, fullInnerChild);
                }

                simplecontent += string.Format(simpleFormatElement,
                    RootObject.TicketListResolver(idCommunity, cat.Id), name, simpleInnerChild);
                fullContent += string.Format(fullFormatElement,
                    RootObject.TicketListResolver(idCommunity, cat.Id), name, description, fullInnerChild);

            }

        }



        /// <summary>
        /// SIMPLE version:
        /// converte HTML in Testo, limitandosi a:
        /// - Togliere i tag html, sostituendoli con "newline"
        /// - decodificare elementi come &nbsp;
        /// - Togliere i "newline" in eccesso
        /// </summary>
        /// <param name="Html">HTML di input</param>
        /// <returns>Testo</returns>
        /// <remarks>Da smeplici test, il risultato pare buono.</remarks>
        public static String HtmlToText(string Html)
        {
            String text = "";
            System.Text.RegularExpressions.Regex regHtmlTag =
                new System.Text.RegularExpressions.Regex("<[^>]+>",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                    System.Text.RegularExpressions.RegexOptions.Multiline);

            System.Text.RegularExpressions.Regex regLines =
                new System.Text.RegularExpressions.Regex("[\\n\\r]+|[\\r\\n]+|[\\r]+|[\\n]+",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                    System.Text.RegularExpressions.RegexOptions.Multiline);

            text = regLines.Replace(
                System.Web.HttpUtility.HtmlDecode(
                regHtmlTag.Replace(Html, Environment.NewLine)
                ),
                Environment.NewLine
                );
            
            return text;
        }

        private string ValidatePreview(string Preview, string HTML)
        {
            if (!String.IsNullOrEmpty(Preview))
                return Preview;
            else
            {
                return HtmlToText(HTML);
            }

        }

    #endregion

        #region Notification settings

        private SettingsPortal _portalSettings;

        private Domain.MailNotification NotificationGetDefault()
        {
            if (_portalSettings == null)
            {
                _portalSettings = this.PortalSettingsGet();
            }

            Domain.MailNotification defaultMailNotification = new MailNotification();

            
            defaultMailNotification.IsDefaultManager = true;
            defaultMailNotification.IsDefaultUser = true;
            defaultMailNotification.IsPortal = true;
            defaultMailNotification.Settings = _portalSettings.MailSettingsUser | _portalSettings.MailSettingsManager;
            
            return defaultMailNotification;
        }

        /// <summary>
        /// Recupera le impostazioni di notifica per uno specifico utente per uno specifico Ticket, altrimenti quelle dell'utente, altrimenti quelle di Default
        /// </summary>
        /// <param name="UserId">Id Utente. Se -1 = Default</param>
        /// <param name="TicketId">Id Ticket. Se -1 = UserDefault</param>
        /// <returns>
        /// Notifiche utente. Tiene conto della gerarchia Ticket+User => User => Default, sia che i valori non ci siano, sia che siano impostati su "prendi default".
        /// </returns>
        public Domain.MailNotification NotificationGetUser(Int64 UserId, Int64 TicketId)//, bool isForUserMainSettings)
        {
            if(UserId <= 0)
                return NotificationGetDefault();

            Domain.MailNotification notification = null;
            
            if(TicketId > 0)
            {
                notification = Manager.GetAll<Domain.MailNotification>(
                            mn => mn.Deleted == BaseStatusDeleted.None &&
                                mn.User != null && mn.User.Id == UserId
                                && mn.Ticket != null && mn.Ticket.Id == TicketId
                                ).FirstOrDefault();
            }

            if (notification == null ||
                notification.IsDefaultUser ||
                (notification.Settings & MailSettings.Default) == MailSettings.Default ||
                (notification.Settings & MailSettings.UserDefault) == MailSettings.UserDefault)
                
            {
                notification = Manager.GetAll<Domain.MailNotification>(
                    mn => mn.Deleted == BaseStatusDeleted.None &&
                          mn.User != null && mn.User.Id == UserId
                          && mn.Ticket == null && mn.IsPortal
                    ).FirstOrDefault();
            }

            if (notification == null ||
                notification.IsDefaultUser ||
                (notification.Settings & MailSettings.Default) == MailSettings.Default ||
                (notification.Settings & MailSettings.UserDefault) == MailSettings.UserDefault)
            {
                notification = NotificationGetDefault();
                //notification.IsDefaultUser &= !isForUserMainSettings;
                //notification.IsDefaultManager &= !isForUserMainSettings;
            }

            return notification;
        }

        private IDictionary<Int64, MailNotification> NotificationGetTicketForUsers(
            IList<Int64> UsersId , Int64 TicketId)
        {
            IDictionary<Int64, MailNotification> dicNotifications = new ConcurrentDictionary<long, MailNotification>();

            IList<MailNotification> notifications =
                Manager.GetAll<Domain.MailNotification>(
                            mn => mn.Deleted == BaseStatusDeleted.None
                                && mn.Settings > MailSettings.Default
                                && mn.Settings != MailSettings.UserDefault && mn.Settings != MailSettings.ManResDefault
                                && mn.Ticket != null && mn.Ticket.Id == TicketId && mn.IsPortal == false
                                && mn.User != null && UsersId.Contains(mn.User.Id)
                                )
                                .Distinct()
                                .ToList();
            if (notifications.Any())
            {
                try
                {
                    dicNotifications =
                notifications.ToDictionary(k => k.User.Id, v => v);
                }
                catch (Exception)
                {
                    
                }
            }

            return dicNotifications;

        }

        private IDictionary<Int64, MailNotification> NotificationGetPortalForUsers(
    IList<Int64> UsersId)
        {
            IDictionary<Int64, MailNotification> dicNotifications = new ConcurrentDictionary<long, MailNotification>();

            IList<MailNotification> notifications =
                Manager.GetAll<Domain.MailNotification>(
                            mn => mn.Deleted == BaseStatusDeleted.None
                                && mn.Settings > MailSettings.Default
                                && mn.Ticket == null && mn.IsPortal == true
                                && mn.User != null && UsersId.Contains(mn.User.Id)
                                )
                                .Distinct()
                                .ToList();
            if (notifications.Any())
            {
                try
                {
                    dicNotifications =
                notifications.ToDictionary(k => k.User.Id, v => v);
                }
                catch (Exception)
                {

                }
            }

            return dicNotifications;

        }
        #endregion
    }
}

// --- OLD FUNCION ---

#region GetNotificationAction_NewTicket_Manager

//private IList<lm.Comol.Core.Notification.Domain.GroupMessages> GetNotificationAction_NewTicket_Manager(
//    Notification.Domain.NotificationAction action,
//    Notification.Domain.WebSiteSettings webSiteSettings)
//{
//    List<lm.Comol.Core.Notification.Domain.GroupMessages> sendMessages = new List<lm.Comol.Core.Notification.Domain.GroupMessages>();

//     if (action.IdObjectType != (long)ModuleTicket.ObjectType.Message)
//    {
//        return sendMessages;
//    }

//    Int64 messageId = action.IdObject;

//    Domain.Message tkMessage = this.MessageGet(messageId);

//    if (tkMessage == null || tkMessage.Ticket == null)
//        return sendMessages;

//    Domain.Ticket ticket = tkMessage.Ticket;

//    IList<Domain.DTO.Notification.DTO_UserNotificationData> NotificationUsers = new List<Domain.DTO.Notification.DTO_UserNotificationData>();

//    IList<Domain.TicketUser> recipienTicketUsers = new List<TicketUser>();


//    //Recupero i destinatari

//    if (tkMessage.ToUser != null)
//    {
//        //SE c'è un assegnatario, è quello
//        recipienTicketUsers.Add(tkMessage.ToUser);
//    }
//    else
//    {
//        //Altrimenti guardo la categoria
//        IList<Domain.Message> messages =
//        ticket.Messages.Where(m => m.SendDate <= tkMessage.SendDate && m.IsDraft == false).OrderBy(m => m.SendDate).ToList();

//        Domain.Category currentCategory;

//        if (tkMessage.ToCategory != null)
//        {
//            currentCategory = tkMessage.ToCategory;
//        }
//        else
//        {
//            currentCategory = (from Domain.Message msg in messages
//                               select msg.ToCategory).FirstOrDefault();
//        }

//        if (currentCategory == null)
//            currentCategory = ticket.CreationCategory;

//        if (currentCategory == null)
//            return null;

//        recipienTicketUsers = (from LK_UserCategory lkuc in
//            Manager.GetIQ<LK_UserCategory>()
//            where lkuc.Category != null && lkuc.Category.Id == currentCategory.Id
//                  && lkuc.User != null
//            select lkuc.User).ToList();

//    }

//    //Elenco destinatari "diretti" (ID)
//    IList<Int64> userIdList = (from TicketUser usr in recipienTicketUsers
//        select usr.Id).ToList();

//    //Setting di portale 
//    //  + destinatari diretti 
//    // NO: ACTION A PARTE!!! + chi vuole comunque una notifica
//    // || (mn.Ticket != null && mn.Ticket.Id == ticket.Id))

//    IList<Domain.MailNotification> manresNotifications =
//        Manager.GetAll<Domain.MailNotification>(
//            mn => mn.Deleted == BaseStatusDeleted.None &&
//                mn.Settings > MailSettings.Default &&
//                (userIdList.Contains(mn.User.Id) 
//                || mn.IsPortal)
//                );

//    Domain.MailNotification manresNotDefault = manresNotifications
//        .FirstOrDefault(mn => mn.IsPortal) ??
//        new MailNotification();

//    foreach (TicketUser usr in recipienTicketUsers)
//    {
//        Domain.MailNotification usrNot =
//            manresNotifications.FirstOrDefault(
//                mn => mn.User != null && mn.User.Id == usr.Id) ??
//            manresNotDefault;

//        if ((usrNot.Settings & MailSettings.ManResNewTicket) == MailSettings.ManResNewTicket)
//        {
//            DTO_UserNotificationData usrND = UserGetNotificationData(usr);
//            if(usrND != null)
//                NotificationUsers.Add(usrND);
//        }
//    }

//    if (!NotificationUsers.Any())
//        return sendMessages;



//    foreach (DTO_UserNotificationData notUser in NotificationUsers)
//    {
//        //Get Template
//        lm.Comol.Core.Notification.Domain.dtoNotificationMessage usrTemplate =
//            ServiceTemplate.GetNotificationMessage(notUser.LanguageCode, ModuleTicket.UniqueCode,
//                (long)ModuleTicket.MailSenderActionType.TicketNewMan, NotificationMode.Automatic,
//                notUser.Channel, ticket.Community.Id);

//        if (usrTemplate != null)
//        {
//            dtoModuleTranslatedMessage ownerMessage = new dtoModuleTranslatedMessage();
//            ownerMessage.Recipients = new List<Recipient>();

//            ownerMessage.Recipients.Add(
//                new Core.MailCommons.Domain.Messages.Recipient()
//                {
//                    Address = notUser.ChannelAddress,
//                    DisplayName = notUser.FullUserName,
//                    IdUserModule = notUser.UserId,
//                    IdPerson = notUser.PersonId,
//                    IdModuleObject = tkMessage.Id,
//                    IdCommunity = ticket.Community.Id,
//                    IdModuleType = (int)ModuleTicket.ObjectType.Message,
//                    Status = Core.MailCommons.Domain.RecipientStatus.Available,
//                    LanguageCode = notUser.LanguageCode
//                });

//            ownerMessage.Translation = GetTemplateContentPreview(
//                true,
//                webSiteSettings,
//                usrTemplate.Translation,
//                ticket, tkMessage, messages,
//                notUser.FullUserName, null, null,
//                notUser.LanguageCode);

//            lm.Comol.Core.Notification.Domain.GroupMessages msgGroup = new GroupMessages();

//            msgGroup.Messages = new List<dtoModuleTranslatedMessage>();
//            msgGroup.Messages.Add(ownerMessage);

//            msgGroup.IdCommunity = ticket.Community.Id;
//            msgGroup.ObjectOwner =
//                lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(
//                    tkMessage.Id,
//                    tkMessage,
//                    (int)ModuleTicket.ObjectType.Message,
//                    ticket.Community.Id,
//                    ModuleTicket.UniqueCode,
//                    Manager.GetModuleID(ModuleTicket.UniqueCode)
//                    );
//            msgGroup.Settings.Save = false;
//            msgGroup.Settings.UniqueIdentifier = Guid.NewGuid();
//            msgGroup.Channel = notUser.Channel;
//            msgGroup.Settings.Mail = usrTemplate.MailSettings;
//            msgGroup.Settings.Template.IdTemplate = usrTemplate.IdTemplate;
//            msgGroup.Settings.Template.IdVersion = usrTemplate.IdVersion;

//            sendMessages.Add(msgGroup);

//        }
//    }

//    return sendMessages;
//}
#endregion


#region Category TranslationList convertion
//string categoryTranslationsList = "";
//string categoryLanguagesList = "";
//string stringFormat = "";
//string stringFormatCatList = "";

//if (isHtml)
//{
//    stringFormat = "{0} <li>{1} - {2}<br/>{3}</li>";
//    stringFormatCatList = "{0} <li>{1} - {2}</li>";
//}
//else
//{
//    stringFormat = "{0} /r/n {1} - {2}/r/n{3}/r/n";
//    stringFormatCatList = "{0} /r/n {1} - {2}";
//}

//foreach (CategoryTranslation catTran in category.Translations)
//{
//    categoryTranslationsList =
//        string.Format(stringFormat,
//            categoryTranslationsList,
//            catTran.LanguageCode,
//            catTran.Name,
//            catTran.Description);
//    categoryLanguagesList = string.Format(stringFormatCatList,
//            categoryLanguagesList,
//            catTran.LanguageCode,
//            catTran.LanguageName);
//}

//if (isHtml)
//{
//    categoryTranslationsList =
//        string.Format("<ul>{0}</ul>", categoryTranslationsList);
//    categoryLanguagesList =
//        string.Format("<ul>{0}</ul>", categoryLanguagesList);
//}

//content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
//            PlaceHoldersType.CategoryLONGNameAndDescriptionLIST), categoryTranslationsList);

//content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
//            PlaceHoldersType.CategoryLanguagesCodeList), categoryLanguagesList);



//IList<LK_UserCategory> lkUC = category.UserRoles;
//if (lkUC == null)
//{
//    lkUC = this.CategoryGetLkucList(category.Id);
//}

//if (lkUC != null && lkUC.Any())
//{
//    string stringUserRoles = "";
//    string userRoleStringFormat = "";


//    if (isHtml)
//    {
//        userRoleStringFormat = "{0} <li>{1} - {2}</li>";
//    }
//    else
//    {
//        userRoleStringFormat = "{0} /r/n {1} - {2}";
//    }

//    String managerString = translation["CategoryUserType.Manager"];
//    String resolverString = translation["CategoryUserType.Resolver"];

//    foreach (LK_UserCategory ur in lkUC)
//    {
//        stringUserRoles =
//            string.Format(
//                userRoleStringFormat,
//                stringUserRoles,
//                ur.IsManager? managerString : resolverString,
//                ur.User.Person != null? ur.User.Person.SurnameAndName : string.Format("{0} {1}", ur.User.Sname, ur.User.Name)
//                );
//    }

//    if (isHtml)
//    {
//        stringUserRoles =
//        string.Format("<ul>{0}</ul>", stringUserRoles);
//    }

//    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
//        PlaceHoldersType.CategoryNewAssignerDisplayName), stringUserRoles);
//}
//else
//{
//    content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(
//        PlaceHoldersType.CategoryNewAssignerDisplayName), "");
//    //    (lastmsg.ToUser.Person != null)
//    //        ? lastmsg.ToUser.Person.SurnameAndName
//    //        : string.Format("{0} {1}", lastmsg.ToUser.Sname, lastmsg.ToUser.Name)
//    //    );    
//}
#endregion