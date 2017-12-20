using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Tickets;
using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using lm.Comol.Core.Notification.Domain;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class TicketAddExtPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
          
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketAddExt View
        {
            get { return (View.iViewTicketAddExt)base.View; }
        }

        public TicketAddExtPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public TicketAddExtPresenter(iApplicationContext oContext, View.iViewTicketAddExt view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        public void InitView()
        {
            if (!CheckUser())
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, -1, ModuleTicket.InteractionType.None);

                View.ShowCantCreate(false, false);
                
                return;
            }

            IList<Domain.DTO.DTO_CategoryTree> Categories = service.CategoriesGetTreeDLL(0, CategoryTREEgetType.System);

            if(Categories.Count <= 0)
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, -1, ModuleTicket.InteractionType.None);

                View.ShowCantCreate(true, false);
                return;
            }


            Domain.Enums.TicketAddCondition Cond = service.PermissionTicketUsercanCreateExternal();

            if(Cond == Domain.Enums.TicketAddCondition.NoPermission)
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, -1, ModuleTicket.InteractionType.None);

                View.ShowCantCreate(false, false);
                return;
            }

            Domain.DTO.DTO_AddInit values = new Domain.DTO.DTO_AddInit();

            values.CurrentUser = View.CurrentUser;

            //DDL Lingue
            values.availableLanguages = service.LanguagesGetAvailableSys();

            int CommunityId = CurrentCommunityId; // View.ViewCommunityId;//this.UserContext.CurrentCommunityID;

            //Ticket (Se draft, altrimenti nuovo!)
            Domain.DTO.DTO_Ticket Tk = new Domain.DTO.DTO_Ticket();

            if (View.CurrentTicketId > 0)
            {
                //Carico Ticket precedente
                Tk = service.TicketGetDraft(View.CurrentTicketId, View.CurrentUser.UserId);

                if (Tk != null && Tk.TicketId > 0)
                {
                    values.TicketData = Tk;
                    CommunityId = Tk.CommunityId;
                }
            }
            else
            {
                if(Cond == Domain.Enums.TicketAddCondition.CheckCount
                   && (service.TicketGetNumDraft(values.CurrentUser.UserId) > Access.MaxDraft))
                {
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, -1, ModuleTicket.InteractionType.None);
                    
                    View.ShowCantCreate(false, true);
                    return;
                } else if (Cond == Domain.Enums.TicketAddCondition.NoPermission || Cond == Domain.Enums.TicketAddCondition.NoUser)
                {
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, -1, ModuleTicket.InteractionType.None);
                    
                    View.ShowCantCreate(false, false);
                    return;
                }

                //Creo un nuovo Ticket in BOZZA
                Tk = service.TicketCreateNewDraft(View.CurrentUser.UserId, CommunityId, View.GetDraftTitle(), View.GetDraftPreview());

                values.IsNew = true;
            }

            //SE Ticket == null o cercano di accedere ad un Ticket non valido
            //o non è possibile creare un nuovo Ticket.
            if (Tk == null || Tk.TicketId <= 0)
            {
                View.ShowCantCreate(false, false);
                return;
            }
            
            View.ViewCommunityId = CommunityId;
            
            View.CurrentTicketId = Tk.TicketId;
            View.DraftMsgId = Tk.DraftMsgId;

            //Categorie disponibili - TO DO -
            //values.Categories = service.CategoriesGetTreeDLLSystem(CommunityId);
            values.CurrentCommunityId = CommunityId;
            //String ComName = service.GetAltCommunityName(this.UserContext.CurrentCommunityID);
            //if(ComName == TicketService.ComErrName)
            //    ComName = 
            
            if (values.TicketData != null && values.TicketData.CategoryId > 0)
            {
                //Domain.DTO.DTO_CategoryTree SelectedCate = service.CategoryGetDTOCatTree(values.TicketData.CategoryId);
                View.refreshCategory(Categories, values.TicketData.CategoryId);
            }
            else
            {
                View.refreshCategory(Categories, service.CategoryDefaultGetID());
            }

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.ExternalCreate, -1, ModuleTicket.InteractionType.None);


            //Impostazioni notifica
            //notification

            bool isNotifDef = false;
            values.CreatorMailSettings = Domain.Enums.MailSettings.none;

            values.OwnerMailSettings =  service.MailSettingsGet(View.CurrentUser.UserId,
                Tk.TicketId, false, ref isNotifDef);

            values.IsDefaultNotOwner = isNotifDef;

            //values.IsBehalf = Tk.IsBehalf;
            
            //values.OwnerMailSettings = service.MailSettingsGet(Tk.OwnerId, Tk.TicketId, false, ref isdefaultOwner);



            View.InitView(values);
        }

        public void SendTimerAction()
        {
            if (CheckUser())
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.ExternalCreate, -1, ModuleTicket.InteractionType.None);
            }
        }

        public bool SaveTicket(Domain.DTO.DTO_Ticket TkData, Boolean ForUpload = false)
        {
            if (!CheckUser())
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, -1, ModuleTicket.InteractionType.None);

                View.ShowCantCreate(false, false);
                return false;
            }

            Domain.Enums.TicketCreateError Error = Domain.Enums.TicketCreateError.none;
            Domain.Enums.TicketAddCondition Cond = service.PermissionTicketUsercanCreateExternal();

            if (Cond == Domain.Enums.TicketAddCondition.CheckCount)
            {
                if (TkData.IsDraft)
                {
                    if (!(service.TicketGetNumDraft(TkData.CreatorId) <= Access.MaxDraft))
                        Error = Domain.Enums.TicketCreateError.ToMuchDraft;
                }
                else
                {
                    if (!(service.TicketGetNumOpen(TkData.CreatorId) <= Access.MaxSended))
                        Error = Domain.Enums.TicketCreateError.ToMuchTicket;
                }
            }
            else if (Cond != Domain.Enums.TicketAddCondition.CanCreate)
            {
                Error = Domain.Enums.TicketCreateError.NoPermission;
            }

            if (Error != Domain.Enums.TicketCreateError.none)
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                View.ShowError(Error);
                return false;
            }

            //Boolean CanCreate = false;
            //if (Cond == Domain.Enums.TicketAddCondition.CanCreate)
            //    CanCreate = true;
            //else if (Cond == Domain.Enums.TicketAddCondition.CheckCount)
            //{
            //    if (TkData.IsDraft)
            //    {
            //        if (service.TicketGetNumDraft(TkData.UserId) <= Access.MaxDraft)
            //            CanCreate = true;

            //    }
            //    else
            //    {
            //        if (service.TicketGetNumOpen(TkData.UserId) <= Access.MaxSended)
            //            CanCreate = true;
            //    }
            //}

            //if (!CanCreate)
            //{

            //    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, -1, ModuleTicket.InteractionType.None);

            //    //View.SendAction(ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId);
            //    //View.SendAction(ModuleTicket.ActionType.NoPermission);
            //    View.ShowCantCreate(false);
            //    return;
            //}


            Error = service.TicketCreateExternal(View.CurrentUser, ref TkData);

            if (Error == Domain.Enums.TicketCreateError.none ||
                TkData.IsDraft && Error != Domain.Enums.TicketCreateError.NoCategory)
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(TkData.CreatorId));
                Objects.Add(ModuleTicket.KVPgetTicket(TkData.TicketId));

                if (TkData.IsDraft)
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketCreateDraft, -1, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                else
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketCreate, -1, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action



                service.NotificationSetTicketCreatorExternal(TkData.TicketId, TkData.MailSettings, View.CurrentUser.UserId);


                if(!TkData.IsDraft)
                    SendNotification(TkData.DraftMsgId, TkData.CreatorId, ModuleTicket.NotificationActionType.TicketSend);

                


                View.TicketCreated(TkData.TicketId, TkData.IsDraft);
            }
            else
            {
                View.ShowError(Error);
                return false;
            }

            return true;
        }

        //public void UpdateDDLCategory()
        //{
        //    if (!CheckUser())
        //        return;

        //    View.refreshCategory(service.CategoriesGetTreeDLL(0));
        //}

        //public void test()
        //{
        //    service.test();
        //}
        /// <summary>
        /// ID comunità corrente (tiene conto dell'URL o della sessione utente)
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
                    return 0;
                }
            }
        }

        public bool CheckUser()
        {
            if (View.CurrentUser == null || View.CurrentUser.UserId <= 0)
            {
                View.DisplaySessionTimeout(0);
                return false;
            }
            
            Domain.DTO.DTO_Access Access = service.SettingsAccessGet(false);
            if (!(Access.IsActive && Access.CanEditTicket))
            {
                View.ShowNoAccess();
                return false;
            }

            return true;
        }

        public Domain.DTO.DTO_Access Access
        {
            get
            {
                if (_access == null)
                    _access = service.SettingsAccessGet(true);

                return _access;
            }
        }

        private Domain.DTO.DTO_Access _access = null;

        public Domain.Enums.TicketDraftDeleteError DeleteDraft(String baseFilePath, String baseThumbnailPath)
        {
            if (!CheckUser())
                return Domain.Enums.TicketDraftDeleteError.hide;

            return service.TicketDeleteDraft(View.CurrentTicketId, View.CurrentUser.UserId, baseFilePath,baseThumbnailPath);

        }

        public void AttachmentsAddInternal(Domain.DTO.DTO_Ticket TkData, Boolean alwaysLastVersion)
        {
            if (!SaveTicket(TkData, true) || View.CurrentUser.PersonId <= 0)
                return;

            Domain.Message Msg = service.MessageGetFromTicketDraft(TkData.TicketId, TkData.CreatorId);

            if (Msg == null)
                return;

            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(Domain.Enums.MessageUserType.Partecipant, 0, 0, null);

            if (!actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem))
                return;

            service.AttachmentsAddFiles(View.DraftMsgId, View.CurrentUser.UserId, View.GetUploadedItems(Msg));

            this.InitView();
            
        }

        public void DeleteFile(Int64 idAttachment,String baseFilePath, String baseThumbnailPath) //, String BasePath
        {
            Int64 DraftMsgId = View.DraftMsgId;
            if (idAttachment < 0 || DraftMsgId < 0)
                return;

            if (!CheckUser() || View.CurrentUser.PersonId <= 0)
                return;

            service.AttachmentDelete(idAttachment, View.CurrentUser.UserId, baseFilePath, baseThumbnailPath);

            InitView();
        }

        private void SendNotification(Int64 messageId, Int64 tkUserId, ModuleTicket.NotificationActionType actionType)
        {
            SettingsPortal settingsPortal = service.PortalSettingsGet();
            if (!(settingsPortal.IsNotificationUserActive && settingsPortal.IsNotificationManActive))
                return;

            //test ID community
            //int currentCommunityId = UserContext.CurrentCommunityID;

            NotificationAction action = new NotificationAction();
            action.IdCommunity = -2;
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
    }
}
