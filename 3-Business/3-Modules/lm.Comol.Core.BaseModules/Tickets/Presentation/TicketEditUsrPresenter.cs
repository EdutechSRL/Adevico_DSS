using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.Notification.Domain;


namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class TicketEditUsrPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketEditUsr View
        {
            get { return (View.iViewTicketEditUsr)base.View; }
        }

        public TicketEditUsrPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }

        public TicketEditUsrPresenter(iApplicationContext oContext, View.iViewTicketEditUsr view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        public void InitView()
        {
            //Nascondo messaggi. Saranno eventualmente visualizzati in seguito all'InitView o internamente.
            View.ShowSendError(TicketMessageSendError.none);
            if (!CheckSessionAccess())
                return;

            //int ComId = UserContext.CurrentCommunityID;
            int PersonId = UserContext.CurrentUserID;


            if (View.ViewCommunityId != CurrentCommunityId)
                View.ViewCommunityId = CurrentCommunityId;

            Domain.DTO.DTO_UserModify Data = service.TicketGetUser(View.TicketId);


            //Repository 4 upload

            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = service.GetRepositoryPermissions(CurrentCommunityId, PersonId);

            List<RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(
                Data.CurrentUserType,
                CurrentCommunityId,
                PersonId,
                cRepository);


            //List<iCoreItemFileLink<long>> alreadyLinkedFiles = new List<iCoreItemFileLink<long>>();

            //if (Data.DraftMessage != null && Data.DraftMessage.Attachments.Any())
            //{
            //    alreadyLinkedFiles = (from Domain.TicketFile fl in Data.DraftMessage.Attachments
            //                          where fl.File != null && fl.Link != null
            //                          select new dtoCoreItemFileLink<long>()
            //                          {
            //                              CreatedBy = fl.CreatedBy,
            //                              CreatedOn = fl.CreatedOn,
            //                              Deleted = fl.Deleted,
            //                              ItemFileLinkId = fl.Id,
            //                              StatusId = 0,
            //                              Link = fl.Link,
            //                              ModifiedBy = fl.ModifiedBy,
            //                              ModifiedOn = fl.ModifiedOn,
            //                              Owner = fl.CreatedBy,
            //                              isVisible = (fl.Deleted == BaseStatusDeleted.None && !fl.File.isDeleted),
            //                              File = fl.File
            //                          }).ToList<iCoreItemFileLink<long>>();
            //    // && fl.Visibility == Domain.Enums.FileVisibility.visible
            //    //NO: se non è visibile, non lo è per l'utente, manager e resolver lo possono vedere E comunque non posso linkarlo nuovamente.
            //}
            ////if (alreadyLinkedFiles == null)
            ////    alreadyLinkedFiles = new List<iCoreItemFileLink<long>>();

            RepositoryAttachmentUploadActions dAction = RepositoryAttachmentUploadActions.none;

            View.InitView(Data, actions, dAction, cRepository, CurrentCommunityId, (Data.DraftMessage == null ? 0 : Data.DraftMessage.Id));


            if (Data.Errors == Domain.Enums.TicketEditUserErrors.none)
            {

                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketLoadEditUser, CurrentCommunityId,
                    ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action
                View.TicketId = Data.TicketId;

                View.DraftMsgId = (Data.DraftMessage != null) ? Data.DraftMessage.Id : -1;

                if (Data.BehalfRevoked)
                {
                    View.ShowBehalfError(BehalfError.permissionRevoked);
                }
                else
                {
                    View.ShowBehalfError(BehalfError.none);    
                }
                
            }
            else
            {
                if (Data.Errors == Domain.Enums.TicketEditUserErrors.NoPermission)
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowBehalfError(BehalfError.NoPermission);
            }

            //View.ShowInitError(Data.Errors);
        }

        public void SendTimerAction()
        {
            if (CheckSessionAccess())
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketLoadEditUser, CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action
            }
        }

        //TODO: Notification - V - TEST
        public void SendMessage(
            string Text, string Preview,
            Boolean CloseMessage, Boolean IsSolved)
        {
            if (!CheckSessionAccess())
                return;

            Domain.Enums.TicketStatus status = (IsSolved)? Domain.Enums.TicketStatus.closeSolved : Domain.Enums.TicketStatus.closeUnsolved;

            if (service.MessageCheckDraft(View.DraftMsgId))
            {
                this.InitView();
                return;
            }

            Int64 NewMessageId = View.DraftMsgId;

            Domain.Enums.TicketMessageSendError Errors = 
                service.MessageSendUser(View.TicketId, Text, Preview, ref NewMessageId);
                //, CloseMessage, status);

            Boolean Changed = true;
            if (CloseMessage)
            {
                if (IsSolved)
                    Changed = service.TicketStatusModify(View.TicketId,
                        Domain.Enums.TicketStatus.closeSolved, 
                        View.GetChangeStatusMessage(Domain.Enums.TicketStatus.closeSolved),
                        false,
                        Domain.Enums.MessageUserType.Partecipant,
                        ref NewMessageId);
                else
                    Changed = service.TicketStatusModify(View.TicketId,
                        Domain.Enums.TicketStatus.closeUnsolved, 
                        View.GetChangeStatusMessage(Domain.Enums.TicketStatus.closeUnsolved),
                        false,
                        Domain.Enums.MessageUserType.Partecipant,
                        ref NewMessageId);
            }

            Int64 userId = service.UserGetIdfromPerson(UserContext.CurrentUserID);

            if (Errors != Domain.Enums.TicketMessageSendError.none && Errors != TicketMessageSendError.TicketClosed)
            {
                //Begin Action

                if (Changed)
                {
                    List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                    Objects.Add(ModuleTicket.KVPgetUser(userId));
                    Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketStatusChanged, CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                }

                List<KeyValuePair<int, String>> ObjectsMsg = new List<KeyValuePair<int, string>>();
                ObjectsMsg.Add(ModuleTicket.KVPgetUser(userId));
                ObjectsMsg.Add(ModuleTicket.KVPgetMessage(NewMessageId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.MessageSend, CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, ObjectsMsg);
                //End Action


              View.ShowSendError(Errors);
            }
            else if (!Changed)
            {
                View.ShowChangeStatusError(false);
            }
            else
            {
                if (Errors == TicketMessageSendError.none)
                {
                    SendNotification(NewMessageId, userId, ModuleTicket.NotificationActionType.MassageSend);
                }
                this.InitView();
                View.ShowSendError(Errors);
            }
        }

        //TODO: Notification - V - TEST
        public void ReopenTicket()
        {
            if (!CheckSessionAccess())
                return;

            //String ReopenText = "ToDo : Get reopen text from View!";
            //ReopenText = "";
            //View.ShowReopenError();

            Domain.Enums.TicketMessageSendError SendError = Domain.Enums.TicketMessageSendError.none;
            //Boolean ChangeError = false;

            //if(String.IsNullOrEmpty(ReopenText))
            //    ChangeError = service.TicketChangeStatus(View.TicketId, Domain.Enums.TicketStatus.open);
            //else
            //SendError = service.MessageSendUser(
            //    View.TicketId, 
            //    , 
            //    "", 
            //    true, 
            //    Domain.Enums.TicketStatus.open);
            Int64 NewMessageId = 0;
            Boolean Changed = service.TicketStatusModify(View.TicketId, Domain.Enums.TicketStatus.open, View.GetChangeStatusMessage(Domain.Enums.TicketStatus.open), true, Domain.Enums.MessageUserType.Partecipant, ref NewMessageId);
            
            //if (SendError != Domain.Enums.TicketMessageSendError.none)
            //    View.ShowSendError(SendError); 
            //else 
            Int64 userId = service.UserGetIdfromPerson(UserContext.CurrentUserID);

            if (!Changed)
            {
                this.View.ShowChangeStatusError(true);
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                    Objects.Add(ModuleTicket.KVPgetUser(userId));
                    Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketStatusChanged, CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action

                SendNotification(NewMessageId, userId, ModuleTicket.NotificationActionType.StatusChanged);

            } else
                this.InitView();
            //init view!!!!!! SE TUTTOP OK!!!!
        }

        public bool CheckSessionAccess()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(CurrentCommunityId);
                return false;
            }

            Domain.DTO.DTO_Access Access = service.SettingsAccessGet(true);
            if (!(Access.IsActive && Access.CanShowTicket))
            {
                View.ShowNoAccess();
                return false;
            }
            else if (!Access.CanEditTicket)
            {
                View.SetReadOnly(false);
            }

            return true;
        }

        public void AttachmentsAddInternal(String HtmlText, String PreviewText)
        {
            if (!CheckSessionAccess())
                return;

            Domain.DTO.DTO_UserModify Data = service.TicketGetUser(View.TicketId);

            if (Data.Errors != Domain.Enums.TicketEditUserErrors.none)
                return;

            if (Data.DraftMessage.Id != View.DraftMsgId)
                return;

            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(Data.CurrentUserType, CurrentCommunityId, UserContext.CurrentUserID, null);

            if (!actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem))
                return;


            service.MessageDraftUpdate(HtmlText, PreviewText, View.TicketId, View.DraftMsgId);

            service.AttachmentsAddFiles(View.DraftMsgId, View.GetUploadedItems(Data.DraftMessage, RepositoryAttachmentUploadActions.uploadtomoduleitem));

            this.InitView();

        }
        public void AttachmentsAddAlsoToCommunity(String HtmlText, String PreviewText)
        {

            Domain.DTO.DTO_UserModify Data = FileUpdateMessage(HtmlText, PreviewText);

            if (Data == null || Data.DraftMessage == null)
            {
                this.InitView();
                return;
            }

            int UserID = (this.UserContext != null) ? UserContext.CurrentUserID : 0;
            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = service.GetRepositoryPermissions(CurrentCommunityId, UserID);


            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(Data.CurrentUserType, CurrentCommunityId, UserContext.CurrentUserID, cRepository);

            if (!actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity))
                return;

            service.MessageDraftUpdate(HtmlText, PreviewText, View.TicketId, View.DraftMsgId);
            //-------------------------------------------------------------------------------------
            service.AttachmentsAddFiles(View.DraftMsgId, View.GetUploadedItems(Data.DraftMessage, RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity));
            //-------------------------------------------------------------------------------------

            this.InitView();
        }
        public void AttachmentsLinkFromCommunity(String HtmlText, String PreviewText, List<ModuleActionLink> links)
        {
            Domain.DTO.DTO_UserModify Data = FileUpdateMessage(HtmlText, PreviewText);

            if (Data == null || Data.DraftMessage == null)
            {
                this.InitView();
                return;
            }

            List<Domain.TicketFile> attachments = service.AttachmentsLinkFiles(Data.DraftMessage.Id, links);

            int addedfiles = (attachments != null ? attachments.Count : 0);
            this.InitView();
        }


        private Domain.DTO.DTO_UserModify FileUpdateMessage(String HtmlText, String PreviewText)
        {
            if (!CheckSessionAccess())
                return null;

            Domain.DTO.DTO_UserModify Data = service.TicketGetUser(View.TicketId);
            
            if (Data.Errors != Domain.Enums.TicketEditUserErrors.none)
                return null;

            if (Data.DraftMessage.Id != View.DraftMsgId)
                return null;

            service.MessageDraftUpdate(HtmlText, PreviewText, View.TicketId, View.DraftMsgId);

            return Data;
        }









        public void DeleteFile(Int64 idAttachment, String baseFilePath, String baseThumbnailPath) //, String BasePath
        {
            Int64 DraftMsgId = View.DraftMsgId;
            if (idAttachment < 0 || DraftMsgId < 0)
                return;

            if (!CheckSessionAccess())
                return;

            service.AttachmentDelete(idAttachment, baseFilePath, baseThumbnailPath);
            InitView();
        }

        //TODO: Notification - V - TEST
        public void SetBehalfPerson(Int32 PersonId, bool HideToOwner)
        {
            View.ShowBehalfError(Domain.Enums.BehalfError.none);

            if (!CheckSessionAccess())
            {
                //Session error
                return;
            }

            Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
            if (Usr.Person == null)
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.NoPermission);
                //Error: external?
                return;
            }

            if (!service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID, Domain.Enums.PermissionType.Behalf))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.NoPermission);
                //Error: no permission
                return;
            }

            Int64 messageId = 0;
            // -- SET BEHALF --
            if (!service.TicketSetBehalfPerson(View.TicketId, PersonId, HideToOwner, ref messageId))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.dBerror);
                return;
                //Error: dB
            }

            
            //TODO: Notification - TEST
            
            if (messageId > 0) //se MINORE <= 0 il Ticket è in DRAFT!
            {
                Int64 userId = service.UserGetIdfromPerson(UserContext.CurrentUserID);
                SendNotification(messageId, userId, ModuleTicket.NotificationActionType.OwnerChanged);    
            }
            

            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetPerson(PersonId));
            Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.BehalfTicketSet, CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            this.InitView();
            View.ShowBehalfError(Domain.Enums.BehalfError.success);
            //OK on page
        }

        // TODO: notification - V - test
        public void SetBehalfUser(Int64 UserId, bool HideToOwner)
        {

            View.ShowBehalfError(Domain.Enums.BehalfError.none);

            if (!CheckSessionAccess())
            {
                //Session error
                return;
            }

            Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
            if (Usr.Person == null)
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.NoPermission);
                //Error: external?
                return;
            }

            if (!service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID, Domain.Enums.PermissionType.Behalf))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.NoPermission);
                //Error: no permission
                return;
            }

            Int64 messageId = 0;

            if (!service.TicketSetBehalfUser(View.TicketId, UserId, HideToOwner, ref messageId))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.dBerror);
                return;
                //Error: dB
            }

            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
            Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.PermissionBehalfSet, CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            //TODO: Notification - TEST
            if (messageId > 0) //se MINORE <= 0 il Ticket è in DRAFT!
            {
                Int64 userId = service.UserGetIdfromPerson(UserContext.CurrentUserID);
                SendNotification(messageId, userId, ModuleTicket.NotificationActionType.OwnerChanged);
            }


            this.InitView();
            View.ShowBehalfError(Domain.Enums.BehalfError.success);
            //OK on page
        }

        // TODO: notification - V - test
        public void RemoveBehalf()
        {

            View.ShowBehalfError(Domain.Enums.BehalfError.none);

            if (!CheckSessionAccess())
            {
                return;
            }

            Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
            if (Usr.Person == null)
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.NoPermission);
                return;
            }

            if (!service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID, Domain.Enums.PermissionType.Behalf))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.NoPermission);
                return;
            }
            Int64 msgId = 0;
            if (!service.TicketSetBehalfCurrent(View.TicketId, ref msgId))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.dBerror);

            }

            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            //Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
            Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.BehalfTicketRemove, CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            //TODO: Notification - TEST
            if (msgId > 0) //se MINORE <= 0 il Ticket è in DRAFT!
            {
                Int64 userId = service.UserGetIdfromPerson(UserContext.CurrentUserID);
                SendNotification(msgId, userId, ModuleTicket.NotificationActionType.OwnerChanged);
            }

            this.InitView();
            View.ShowBehalfError(Domain.Enums.BehalfError.deleteSuccess);
            //OK on page
        }


        public void InitPersonSelector()
        {
            Int32 currentOwnerId = service.TicketGetOwnerPersonId(View.TicketId);

            if (currentOwnerId > 0)
            {
                List<int> currentSelection = new List<int>();
                currentSelection.Add(currentOwnerId);
                View.InitPersonSelector(currentSelection);
            }
            else
            {
                View.InitPersonSelector(null);
            }
        }

        
        public void SetTicketVisibility(bool HideToOwner)
        {
            View.ShowBehalfError(Domain.Enums.BehalfError.none);

            if (!CheckSessionAccess())
            {
                return;
            }

            Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
            if (Usr.Person == null)
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.NoPermission);
                //Error: external?
                return;
            }

            if (!service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID, Domain.Enums.PermissionType.Behalf))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.NoPermission);
                //Error: no permission
                return;
            }

            if (!service.TicketVisibilitySet(View.TicketId, HideToOwner))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.dBerror);
                return;
                //Error: dB
            }

            //TODO: Notification

            this.InitView();
            View.ShowBehalfError(Domain.Enums.BehalfError.visibilitySuccess);

        }

        public void SetnotificationSettings(Domain.Enums.MailSettings creatorSettings,
            Domain.Enums.MailSettings ownerSettings)
        {
            //SettingsPortal settingsPortal = service.PortalSettingsGet();
            //if (!(settingsPortal.IsNotificationUserActive && settingsPortal.IsNotificationManActive))
            //    return;


            ////Se DISABLED, SOLO per il creator corrente (no behalf)
            //if (ownerSettings != MailSettings.DISABLED)
            //{
            //    service.NotificationSetTicketOwner(View.TicketId, ownerSettings);
            //}


            //service.NotificationSetTicketCreatorCurrent(View.TicketId, creatorSettings);

            //Se DISABLED, SOLO per il creator corrente (no behalf)
            if (ownerSettings != MailSettings.DISABLED)
            {
                bool IsOwner = creatorSettings == MailSettings.DISABLED;
                service.NotificationSetTicketOwner(View.TicketId, ownerSettings, IsOwner);
            }

            if (creatorSettings != MailSettings.DISABLED)
            {
                service.NotificationSetTicketCreatorCurrent(View.TicketId, creatorSettings);
            }
            
            this.InitView();
        }

        private void SendNotification(Int64 messageId, Int64 tkUserId, ModuleTicket.NotificationActionType actionType)
        {
            ////SE non ho un ID messaggio di riferimento, NON INVIO NOTIFICHE!!!
            if (messageId <= 0)
            {
                //throw new ArgumentNullException("messageId");
                return;
            }
                

            //test ID community
            //int currentCommunityId = UserContext.CurrentCommunityID;

            NotificationAction action = new NotificationAction();
            action.IdCommunity = CurrentCommunityId;
            action.IdObject = messageId;
            action.IdObjectType = (long)ModuleTicket.ObjectType.Message;
            action.ModuleCode = ModuleTicket.UniqueCode;

            action.IdModuleUsers = new List<long>();
            action.IdModuleUsers.Add(tkUserId);

            action.IdModuleAction = (long)actionType;

            //IList<NotificationAction> actions = new List<NotificationAction>();
            //actions.Add(action);

            //action.IdModuleAction = (long) ModuleTicket.MailSenderActionType.TicketSendMessageMan;

            View.SendNotification(action, UserContext.CurrentUserID);

        }


       

        private int _comId = -1;
        /// <summary>
        /// Comunità corrente. Da view (URL/viewsatate) se presente, altrimenti dalla sessione utente
        /// </summary>
        public Int32 CurrentCommunityId
        {
            get
            {
                if (_comId <= 0)
                {
                    _comId = View.ViewCommunityId;    
                }


                if (_comId <= 0)
                {
                    _comId = UserContext.CurrentCommunityID;
                    View.ViewCommunityId = _comId;
                    
                }

                return _comId;
            }
        }
    }
}
