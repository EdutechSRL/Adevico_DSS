using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Tickets;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using lm.Comol.Core.Notification.Domain;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class TicketEditExtPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
          
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketEditExt View
        {
            get { return (View.iViewTicketEditExt)base.View; }
        }

        public TicketEditExtPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public TicketEditExtPresenter(iApplicationContext oContext, View.iViewTicketEditExt view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }


    #endregion

        public void InitView()
        {
            if (!CheckUser())
                return;

            //if (View.ViewCommunityId != UserContext.CurrentCommunityID)
            View.ViewCommunityId = -1;

            Domain.DTO.DTO_UserModify Data = service.TicketGetUserExt(View.TicketId, View.CurrentUser);
            

            if (Data.Errors == Domain.Enums.TicketEditUserErrors.none)
            {
                View.InitView(Data);

                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketLoadEditUser, -1, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action
                View.TicketId = Data.TicketId;

                View.DraftMsgId = (Data.DraftMessage != null) ? Data.DraftMessage.Id : -1;

            }
            else if (Data.Errors == Domain.Enums.TicketEditUserErrors.IsDraft)
            {
                View.ShowDraft(Data.TicketId);
            }
            else if (Data.Errors == Domain.Enums.TicketEditUserErrors.NoPermission)
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.AppContext.UserContext.CurrentCommunityID, ModuleTicket.InteractionType.None);

        }

        public void SendTimerAction()
        {
            if (CheckUser())
            {
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketLoadEditUser, -1, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            }
        }

        public void SendMessage(
            string Text, string Preview,
            Boolean CloseMessage, Boolean IsSolved)
        {
            if (!CheckUser())
                return;

            Domain.Enums.TicketStatus status = (IsSolved) ? Domain.Enums.TicketStatus.closeSolved : Domain.Enums.TicketStatus.closeUnsolved;

            Int64 NewMessageId = 0;
            Domain.Enums.TicketMessageSendError Errors =
                service.MessageSendUserExt(View.TicketId, Text, Preview, View.CurrentUser, ref NewMessageId);
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

            if (Errors != Domain.Enums.TicketMessageSendError.none && Errors != TicketMessageSendError.TicketClosed)
            {
                //Begin Action

                if (Changed)
                {
                    List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                    Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                    Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketStatusChanged, -1, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                }

                List<KeyValuePair<int, String>> ObjectsMsg = new List<KeyValuePair<int, string>>();
                ObjectsMsg.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                ObjectsMsg.Add(ModuleTicket.KVPgetMessage(NewMessageId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.MessageSend, -1, ModuleTicket.InteractionType.UserWithLearningObject, ObjectsMsg);
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
                    SendNotification(NewMessageId, ModuleTicket.NotificationActionType.MassageSend);

                this.InitView();
                View.ShowSendError(Errors);
            }
        }


        //public void ReopenTicket()
        //{
        //    if (!CheckUser())
        //        return;

        //    //String ReopenText = "ToDo : Get reopen text from View!";
        //    //ReopenText = "";
        //    //View.ShowReopenError();

        //    Domain.Enums.TicketMessageSendError SendError = Domain.Enums.TicketMessageSendError.none;
        //    //Boolean ChangeError = false;

        //    //if(String.IsNullOrEmpty(ReopenText))
        //    //    ChangeError = service.TicketChangeStatus(View.TicketId, Domain.Enums.TicketStatus.open);
        //    //else
        //    //SendError = service.MessageSendUser(
        //    //    View.TicketId, 
        //    //    , 
        //    //    "", 
        //    //    true, 
        //    //    Domain.Enums.TicketStatus.open);

        //    Boolean Changed = service.TicketStatusModify(View.TicketId, Domain.Enums.TicketStatus.open, View.GetChangeStatusMessage(Domain.Enums.TicketStatus.open), true, Domain.Enums.MessageUserType.Partecipant);

        //    //if (SendError != Domain.Enums.TicketMessageSendError.none)
        //    //    View.ShowSendError(SendError); 
        //    //else 
        //    if (!Changed)
        //    {
        //        this.View.ShowChangeStatusError(true);
        //        //Begin Action
        //        List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
        //        Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
        //        Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

        //        View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketStatusChanged, View.ViewCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
        //        //End Action
        //    }
        //    else
        //        this.InitView();
        //    //init view!!!!!! SE TUTTOP OK!!!!
        //}

        public bool CheckUser()
        {
            if (View.CurrentUser == null || View.CurrentUser.UserId <= 0)
            {
                View.DisplaySessionTimeout(0);
                return false;
            }

            Domain.DTO.DTO_Access Access = service.SettingsAccessGet(false);
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

        public void UploadFile(String HtmlText, String PreviewText)
        {
            if (!CheckUser() || View.CurrentUser.PersonId <= 0)
                return;

            Domain.DTO.DTO_UserModify Data = service.TicketGetUserExt(View.TicketId, View.CurrentUser.UserId);

            if (Data.Errors != Domain.Enums.TicketEditUserErrors.none)
                return;

            if (Data.DraftMessage.Id != View.DraftMsgId)
                return;

            int PersonId = 0;

            try
            {
                PersonId = System.Convert.ToInt32(View.CurrentUser.PersonId);
            }
            catch { }


            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(Data.CurrentUserType, 0, PersonId, null);
            if (!actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem))
                return;
            service.MessageDraftUpdate(HtmlText, PreviewText, View.TicketId, View.DraftMsgId, View.CurrentUser.UserId);

            service.AttachmentsAddFiles(View.DraftMsgId, View.CurrentUser.UserId, View.GetUploadedItems(Data.DraftMessage));

            this.InitView();

        }

        public void DeleteFile(Int64 idAttachment, String baseFilePath, String baseThumbnailPath) //, String BasePath
        {
            Int64 DraftMsgId = View.DraftMsgId;
            if (idAttachment < 0 || DraftMsgId < 0)
                return;

            if (!CheckUser() || View.CurrentUser.PersonId <= 0)
                return;

            service.AttachmentDelete(idAttachment, View.CurrentUser.UserId, baseFilePath, baseThumbnailPath);

            this.InitView();
        }

        public void SetnotificationSettings(Domain.Enums.MailSettings creatorSettings)
        {
            Domain.SettingsPortal settingsPortal = service.PortalSettingsGet();
            if (!(settingsPortal.IsNotificationUserActive && settingsPortal.IsNotificationManActive))
                return;

            service.NotificationSetTicketCreatorExternal(View.TicketId, creatorSettings, View.CurrentUser.UserId);

            this.InitView();
        }

        private void SendNotification(Int64 messageId, ModuleTicket.NotificationActionType actionType)
        {

            //test ID community
            //int currentCommunityId = UserContext.CurrentCommunityID;

            NotificationAction action = new NotificationAction();
            action.IdCommunity = -2;
            action.IdObject = messageId;
            action.IdObjectType = (long)ModuleTicket.ObjectType.Message;
            action.ModuleCode = ModuleTicket.UniqueCode;

            action.IdModuleUsers = new List<long>();
            action.IdModuleUsers.Add(View.CurrentUser.UserId);

            action.IdModuleAction = (long)actionType;

            //IList<NotificationAction> actions = new List<NotificationAction>();
            //actions.Add(action);

            //action.IdModuleAction = (long) ModuleTicket.MailSenderActionType.TicketSendMessageMan;

            View.SendNotification(action, UserContext.CurrentUserID);

        }
    }
}
