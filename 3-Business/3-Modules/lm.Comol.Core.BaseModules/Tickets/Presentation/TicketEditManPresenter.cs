using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Notification.Domain;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    /// <summary>
    /// REMARKS: SE vengono aggiunte FUNZIONALITA', aggiungere l'aggiornamento a LAST ACCESS!!!
    /// </summary>
    public class TicketEditManPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketEditMan View
        {
            get { return (View.iViewTicketEditMan)base.View; }
        }

        public TicketEditManPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }

        public TicketEditManPresenter(iApplicationContext oContext, View.iViewTicketEditMan view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        public void InitView()
        {
            if (!CheckSessionAccess())
                return;
            
            int ColUserId = this.UserContext.CurrentUserID;

            Domain.DTO.DTO_ManagerModify Data = service.TicketGetManager(View.TicketId, View.MassageFilter, View.MessagesOrder);

            if (Data.Errors == TicketEditManErrors.NoPermission)// (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }

            //Int32 CommunityId = UserContext.CurrentCommunityID;

            IList<Domain.DTO.DTO_CategoryTree> Categories = service.CategoriesGetTreeDLL(CurrentCommunityId, CategoryTREEgetType.FilterManager);
            
            
            //service.CategoryGetDDLManRes_ComCurrent();
            View.UserType = Data.CurrentUserType;

            //this.CurrentCommunityId
            //

            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = service.GetRepositoryPermissions(CurrentCommunityId, ColUserId);

            //List<iCoreItemFileLink<long>> alreadyLinkedFiles = new List<iCoreItemFileLink<long>>();
            ////= new List<iCoreItemFileLink<long>>();

            ////Link di comunità già usati
            
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

            //    //fl.Link.DestinationItem.ObjectLongID


            //    // && fl.Visibility == Domain.Enums.FileVisibility.visible
            //    //NO: se non è visibile, non lo è per l'utente, manager e resolver lo possono vedere E comunque non posso linkarlo nuovamente.
            //}

            //if (alreadyLinkedFiles == null)
            //    alreadyLinkedFiles = new List<iCoreItemFileLink<long>>();

            bool hasComManager = service.UsersCommunityHasManRes(CurrentCommunityId, Data.UserAssignedId);
            bool hasCommunity = (CurrentCommunityId > 0);


            View.InitView(
                Data, 
                Categories, 
                service.CategoryGetDTOCatTree(Data.CategoryCurrentId),
                service.UploadAvailableActionsGet(
                    Data.CurrentUserType,
                    CurrentCommunityId,
                    ColUserId, 
                    cRepository),
                cRepository, 
                CurrentCommunityId,
                (Data.DraftMessage==null ? 0 : Data.DraftMessage.Id),
                hasComManager,
                hasCommunity);

            if (Data.Errors == Domain.Enums.TicketEditManErrors.none)
            {
                View.DraftMsgId = Data.DraftMessage.Id;
                View.TicketId = Data.TicketId;
            }
                
            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetPerson(UserContext.CurrentUserID));
            Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketLoadEditManRes, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action
        }


        //TODO: Notification - V - test
        public void ChangeStatus(Domain.Enums.TicketStatus Status)
        {
            if (!CheckSessionAccess())
                return;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }

            if (Status != Domain.Enums.TicketStatus.draft)
            {
                Int64 messageId = 0;
                Boolean Changed = service.TicketStatusModify(View.TicketId, Status, View.GetChangeStatusMessage(Status), true, View.UserType, ref messageId);
                if (Changed)
                {
                    Int64 userId = this.service.UserGetIdfromPerson(UserContext.CurrentUserID);
                    //Begin Action
                    List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                    Objects.Add(ModuleTicket.KVPgetUser(userId));
                    Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketStatusChanged, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                    
                    //End Action


                    //TODO: Notification - change status
                    if(messageId > 0)
                        SendNotification(messageId, userId, ModuleTicket.NotificationActionType.StatusChanged);
                    
                    this.InitView();
                }
                else
                {
                    //Mange Errors
                }

                
            }
        }

        public void SendTimerAction()
        {
            if (service.UserHasManResTicketPermission(View.TicketId))
            {
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketLoadEditManRes, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            }
        }

        //TODO: notification - V - test
        public void SendMessage(
            String Text, String Preview, Boolean HideToUser, 
            Domain.Enums.TicketStatus NewStatus, 
            Domain.Enums.MessageType MsgType,
            Int64 DraftId)
        {
            if (!CheckSessionAccess())
                return;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }
            if (DraftId <= 0)
            {
                this.InitView();
                return;
            }

            if (this.service.MessageCheckDraft(DraftId))
            {
                this.InitView();
                return;
            }

            

            Domain.Enums.TicketMessageSendError error = Domain.Enums.TicketMessageSendError.none;



            Int64 messageId = 0;
            Int64 userId = this.service.UserGetIdfromPerson(UserContext.CurrentUserID);
            
            if (NewStatus == Domain.Enums.TicketStatus.closeSolved || NewStatus == Domain.Enums.TicketStatus.closeUnsolved)
            {  
                error = service.MessageSendMan(View.TicketId, Text, Preview, HideToUser, MsgType, View.UserType, false, ref messageId);
                service.TicketStatusModify(View.TicketId, NewStatus, View.GetChangeStatusMessage(NewStatus), true, View.UserType, ref messageId);

                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(userId));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketStatusChanged, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action
            }
            else 
            {
                service.TicketStatusModify(View.TicketId, NewStatus, View.GetChangeStatusMessage(NewStatus), false, View.UserType, ref messageId);
                error = service.MessageSendMan(View.TicketId, Text, Preview, HideToUser, MsgType, View.UserType, true, ref messageId);
            }

            if (error == Domain.Enums.TicketMessageSendError.none)
            {
                //Int64 userId = this.service.UserGetIdfromPerson(UserContext.CurrentUserID);
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(userId));
                Objects.Add(ModuleTicket.KVPgetMessage(messageId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.MessageSend, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action

                //ToDo: Send Notification
                if(messageId > 0)
                    SendNotification(messageId, userId, ModuleTicket.NotificationActionType.MassageSend);

                this.InitView();

            }
            //else
            View.ShowSendError(error);

        }

        public void ChangeMsgVisibility(Int64 MessageId,Boolean IsVisible)
        {
            if (!CheckSessionAccess())
                return;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }

            Boolean Changed = service.MessageChangeVisibility(View.TicketId, MessageId, IsVisible, View.UserType);
            if (Changed)
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(this.service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                Objects.Add(ModuleTicket.KVPgetMessage(MessageId));

                if (IsVisible)
                {
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.MessageShow, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);

                    //TODO: Notification    (If IsLastMessage...)
                }
                else
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.MessageHide, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action

                InitView();
            }
                

        }

        /// <summary>
        /// Comunità corrente. Da view (URL) se presente, altrimenti dalla sessione utente
        /// </summary>
        public Int32 CurrentCommunityId
        {
            get
            {
                Int32 VComId = View.ViewCommunityId;
                if (VComId > 0)
                {
                    return VComId;
                }
                else
                {
                    Int32 SysComId = UserContext.CurrentCommunityID;
                    View.ViewCommunityId = SysComId;
                    return SysComId;
                }

                //return UserContext.CurrentCommunityID;
            }
        }

        //TODO: notification - V - TEST
        public void CategoryReassign(Int64 NewCategoryID)
        {
            if (NewCategoryID <= 0)
            {
                this.InitView();
                this.View.ShowAssignError(CategoryReassignError.noChange);
                return;
            }
                

            if (!CheckSessionAccess())
            {
                View.ShowNoPermission();
                return; // Domain.Enums.CategoryReassignError.noPermission;
            }
                

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
                return;// Domain.Enums.CategoryReassignError.noPermission;
            }

            Int64 messageId = 0;
            Domain.Enums.CategoryReassignError Response = service.TicketAssignToCategory(View.TicketId, NewCategoryID, View.GetChangeCategoryMessage(), View.UserType, true, ref messageId);

            if (Response == Domain.Enums.CategoryReassignError.none)
            {
                Int64 userId = this.service.UserGetIdfromPerson(UserContext.CurrentUserID);
                
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));
                Objects.Add(ModuleTicket.KVPgetCategory(NewCategoryID));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketAssignCategory, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action

                //TODO: Notification - test
                if (messageId > 0)
                    SendNotification(messageId, userId, ModuleTicket.NotificationActionType.AssignmentCategory);

                this.InitView();
                this.View.ShowCategoryChanged();
            }
            else
            {
                this.InitView();
                this.View.ShowAssignError(Response);
            }
            
            
            //return Response;
        }

        //TODO: Notification - V - test
        public void AssingPerson(int PersonId, bool IsManager)
        {
            if (!CheckSessionAccess())
                return;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }
            Int64 messageId = 0;
            Boolean IsChanged = service.TicketAssignToPerson(
                View.TicketId, View.UserType, 
                PersonId, 
                View.GetChangeUserMessage(), false, 
                IsManager,
                ref messageId);


            if (IsChanged)
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));
                Objects.Add(ModuleTicket.KVPgetPerson(PersonId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketAssignPerson, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action

                //TODO: Notification - test
                if (messageId > 0)
                {
                    Int64 userId = this.service.UserGetIdfromPerson(UserContext.CurrentUserID);
                    SendNotification(messageId, userId, ModuleTicket.NotificationActionType.AssignmentUser);
                }
                    
            }
            this.InitView();

            if (!IsChanged)
            {
                View.ShowAssignUsrError(UserReassignError.Current);
            }
            else
            {
                View.ShowAssignUsrError(UserReassignError.none);
            }
        }

        //TODO: Notification - V - test
        public void AssignUser(Int64 UserId, bool IsManager)
        {
            if (!CheckSessionAccess())
                return;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }

            Int64 messageId = 0;
            Boolean IsChanged = service.TicketAssignToUser(
                View.TicketId, 
                View.UserType, 
                UserId, 
                View.GetChangeUserMessage(), 
                false, IsManager, 
                ref messageId);

            if (IsChanged)
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));
                Objects.Add(ModuleTicket.KVPgetUser(UserId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketAssignUser, this.CurrentCommunityId,
                    ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action

                //TODO: Notification - test
                if (messageId > 0)
                {
                    Int64 userId = this.service.UserGetIdfromPerson(UserContext.CurrentUserID);
                    SendNotification(messageId, userId, ModuleTicket.NotificationActionType.AssignmentUser);
                }
                
            }

            this.InitView();

            if (!IsChanged)
            {
                View.ShowAssignUsrError(UserReassignError.Current);
            }
            else
            {
                View.ShowAssignUsrError(UserReassignError.none);
            }
            
        }

        //TODO: Notification - V - test
        public void AssignMe()
        {
            if (!CheckSessionAccess())
                return;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }

            Int64 messageId = 0;

            bool IsManager = false;

            if (View.UserType == Domain.Enums.MessageUserType.Manager || View.UserType == Domain.Enums.MessageUserType.CategoryManager)
                IsManager = true;

            Boolean Assigned = service.TicketAssignToCurrent(
                View.TicketId, View.UserType, 
                View.GetChangeUserMessage(), false, 
                IsManager, ref messageId);

            if (Assigned)
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketAssignPerson, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action


                //TODO: Notification - test
                if (messageId > 0)
                {
                    Int64 userId = this.service.UserGetIdfromPerson(UserContext.CurrentUserID);
                    SendNotification(messageId, userId, ModuleTicket.NotificationActionType.AssignmentUser);
                }
            }
            
            this.InitView();

            if (!Assigned)
            {
                View.ShowAssignUsrError(UserReassignError.Current);
            }
            else
            {
                View.ShowAssignUsrError(UserReassignError.none);
            }
        }

        public IList<Domain.DTO.DTO_User> GetManRes(Int64 UnloadedUserId)
        {
            return service.UsersGetManRes(this.UserContext.CurrentCommunityID, true, true, UnloadedUserId);
        }

        public bool CheckSessionAccess()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(View.ViewCommunityId);
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
            Domain.DTO.DTO_ManagerModify Data = FileUpdateMessage(HtmlText, PreviewText);

            if(Data == null)
                return;

            int idCommunity = (this.UserContext != null) ? UserContext.CurrentCommunityID : 0;
            int idUser = (this.UserContext != null) ? UserContext.CurrentUserID : 0;

            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = service.GetRepositoryPermissions(idCommunity, idUser);

            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(
                Data.CurrentUserType,
                idCommunity,
                idUser,
                cRepository);

            if (!actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem))
                return;

            service.AttachmentsAddFiles(View.DraftMsgId, View.GetUploadedItems(Data.DraftMessage, DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem ));

            this.InitView();
        }
        public void AttachmentsAddAlsoToCommunity(String HtmlText, String PreviewText)
        {
            
            Domain.DTO.DTO_ManagerModify Data = FileUpdateMessage(HtmlText, PreviewText);

            if(Data == null)
                return;

            int CommunityID = (this.UserContext != null) ? UserContext.CurrentCommunityID : 0;
            int UserID = (this.UserContext != null) ? UserContext.CurrentUserID : 0;

            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = service.GetRepositoryPermissions(CommunityID, UserID);

            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(Data.CurrentUserType, CommunityID, UserID, cRepository);

            if (!actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity))
                return;

            //-------------------------------------------------------------------------------------
            service.AttachmentsAddFiles(View.DraftMsgId, View.GetUploadedItems(Data.DraftMessage, DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity));
            //-------------------------------------------------------------------------------------

            this.InitView();
        }
        public void AttachmentsLinkFromCommunity(String HtmlText, String PreviewText, List<ModuleActionLink> links)
        {
            Domain.DTO.DTO_ManagerModify Data = FileUpdateMessage(HtmlText, PreviewText);

            if (Data == null || Data.DraftMessage == null)
                return;

            //TO DO
            List<Domain.TicketFile> attachments = service.AttachmentsLinkFiles(Data.DraftMessage.Id, links);

            int addedfiles = (attachments != null ? attachments.Count : 0);

            this.InitView();
        }
        private Domain.DTO.DTO_ManagerModify FileUpdateMessage(String HtmlText, String PreviewText)
        {
            if (!CheckSessionAccess())
                return null;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }

            Domain.DTO.DTO_ManagerModify Data = service.TicketGetManager(View.TicketId, View.MassageFilter, View.MessagesOrder);

            if (Data.Errors != Domain.Enums.TicketEditManErrors.none)
                return null;

            if (Data.DraftMessage.Id != View.DraftMsgId)
                return null;

            service.MessageDraftUpdate(HtmlText, PreviewText, View.TicketId, View.DraftMsgId);

            return Data;
        }
        
        public void HideShowFile(Int64 idAttachment, bool hide)
        {
            if (idAttachment < 0)
                return;

            if (!CheckSessionAccess())
                return;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }
            else
                service.AttachmentEditVisibility(idAttachment, hide);

            this.InitView();
        }

        public void DeleteFile(Int64 idAttachment, String baseFilePath, String baseThumbnailPath) //, String BasePath
        {
            Int64 DraftMsgId = View.DraftMsgId;
            if (idAttachment < 0 || DraftMsgId < 0)
                return;

            if (!CheckSessionAccess())
                return;

            service.AttachmentDelete(idAttachment, baseFilePath,baseThumbnailPath);
            InitView();
        }

        //TODO: notification - V - TEST
        public void TicketChangeCondition(Domain.Enums.TicketCondition Condition, bool status)
        {
            if (!CheckSessionAccess())
                return;

            if (!service.UserHasManResTicketPermission(View.TicketId))
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                View.ShowNoPermission();
            }

            Int64 messageId = 0;

            Boolean Changed = service.TicketConditionModify(
                View.TicketId, Condition, status,
                View.GetChangeConditionMessage(Condition), true,
                View.UserType, ref messageId);

            if (Changed)
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(this.service.UserGetIdfromPerson(UserContext.CurrentUserID)));
                Objects.Add(ModuleTicket.KVPgetTicket(View.TicketId));

                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketStatusChanged, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action

                //TODO: Notification - test
                if (messageId > 0)
                {
                    Int64 userId = this.service.UserGetIdfromPerson(UserContext.CurrentUserID);
                    SendNotification(messageId, userId, ModuleTicket.NotificationActionType.ModerationChanged);
                }

                this.InitView();
            }
            else
            {
                //Mange Errors
            }
        }

        private void SendNotification(Int64 messageId, Int64 tkUserId, ModuleTicket.NotificationActionType actionType)
        {
            SettingsPortal settingsPortal = service.PortalSettingsGet();
            if (!(settingsPortal.IsNotificationUserActive && settingsPortal.IsNotificationManActive))
                return;

            //test ID community
            int currentCommunityId = UserContext.CurrentCommunityID;

            NotificationAction action = new NotificationAction();
            action.IdCommunity = currentCommunityId;
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

        public void SetnotificationSettings(Domain.Enums.MailSettings managerSettings)
        {
            service.NotificationSetTicket(View.TicketId, managerSettings, this.service.UserGetIdfromPerson(UserContext.CurrentUserID), false);

            this.InitView();
        }
    }
}
