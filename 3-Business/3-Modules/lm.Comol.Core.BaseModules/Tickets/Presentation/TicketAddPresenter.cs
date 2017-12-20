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
    public class TicketAddPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketAdd View
        {
            get { return (View.iViewTicketAdd)base.View; }
        }

        public TicketAddPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }

        public TicketAddPresenter(iApplicationContext oContext, View.iViewTicketAdd view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion


        public void InitView()
        {
            if (!CheckSessionAccess())
            {
                return;
            }
            //Boolean IsNewAndOtherTicket = false;
            

            Domain.DTO.DTO_AddInit values = new Domain.DTO.DTO_AddInit();

            //Utente
            Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
            if (Usr == null)
            {
                View.DisplaySessionTimeout(View.ViewCommunityId);
                return;
            }

            Domain.Enums.TicketAddCondition Cond = service.PermissionTicketUsercanCreate();

            if (Cond == Domain.Enums.TicketAddCondition.CheckCount
                && service.TicketGetNumOpen(Usr.Id) >= Access.MaxSended)
                    View.ShowCantCreate(Domain.Enums.CantCreate.MaxSend);

            int DraftNum = service.TicketGetNumDraft(Usr.Id);

            //DDL Lingue
            values.availableLanguages = service.LanguagesGetAvailableSys();
            
            int CommunityId = CurrentCommunityId; 

            //Ticket (Se draft, altrimenti nuovo!)
            Domain.DTO.DTO_Ticket Tk = new Domain.DTO.DTO_Ticket();

            if(View.CurrentTicketId <= 0)
            {
                //Controllo se può creare nuovi Ticket (Bozze)
                if (!(Cond == Domain.Enums.TicketAddCondition.CanCreate ||
                   (Cond == Domain.Enums.TicketAddCondition.CheckCount
                && (DraftNum < Access.MaxDraft))))
                {
                    if (View.CurrentTicketId <= 0)
                    {
                        View.ShowCantCreate(Domain.Enums.CantCreate.MaxDraftNoSend);
                        View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, CommunityId, ModuleTicket.InteractionType.None);
                        return;
                    }
                    
                }

                //Creo un nuovo Ticket in BOZZA
                Tk = service.TicketCreateNewDraft(Usr.Id, CommunityId, View.GetDraftTitle(), View.GetDraftPreview());
                values.IsNew = true;

                //Una volta creato, redirezione alla pagina con il ticket appena creato, per evitare problema F5.
                View.GotoNewTicketCreated(Tk.Code);
                return;
            }
            else
            {
                //Prendo il Ticket in Bozza
                Tk = service.TicketGetDraft(View.CurrentTicketId);
            }

            //SE Ticket == null o cercano di accedere ad un Ticket non valido
            //o non è possibile creare un nuovo Ticket.
            if (Tk == null || Tk.TicketId <= 0 || !Tk.IsDraft)
            {
                View.ShowCantCreate(Domain.Enums.CantCreate.permission);
                return;
            }
            
            //Carico Ticket (Nuovo, appena creato o precedente che sia)
            CommunityId = Tk.CommunityId;

            View.CurrentTicketId = Tk.TicketId;
            View.ViewCommunityId = CommunityId;
            View.DraftMsgId = Tk.DraftMsgId;

            values.TicketData = Tk;
            values.CurrentCommunityId = CommunityId;
            values.FileCommunityId = CurrentCommunityId;

            //Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.ExternalCreate, -1, ModuleTicket.InteractionType.None, Objects);
            //End Action

            values.HasOtherDraft = (values.IsNew && DraftNum > 0);
            
            this.UpdateCommunity(CommunityId, Tk.CategoryId);


            //USER!
            values.CurrentUser.UserId = Usr.Id;

            if (Usr.Person != null)
            {
                values.CurrentUser.PersonId = Usr.Person.Id;
                values.CurrentUser.Name = Usr.Person.Name;
                values.CurrentUser.SName = Usr.Person.Surname;
                values.CurrentUser.LanguageCode = Usr.LanguageCode;
                values.CurrentUser.Mail = Usr.Person.Mail;
            }
            else
            {
                values.CurrentUser.PersonId = 0;
                values.CurrentUser.Name = Usr.Name;
                values.CurrentUser.SName = Usr.Sname;
                values.CurrentUser.LanguageCode = Usr.LanguageCode; // TicketService.LangMultiCODE; //OR SYS DEFAULT!?
                values.CurrentUser.Mail = Usr.mail;
            }


            //Behalf

            if (Usr.Person != null)
            {
                values.CanBehalf = service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID,
                    Domain.Enums.PermissionType.Behalf);
            }
            else
            {
                values.CanBehalf = false;
            }


            // Notification (sono state messe qui a differenza di "Edit" in cui sono messe nel service.
            // SPOSTARE IN SERVICE?

            if (Tk.IsBehalf)
            {
                if (Tk.HideToOwner)
                {
                    bool isdefaultCreator = false;
                    values.CreatorMailSettings = service.MailSettingsGet(Usr.Id, Tk.TicketId, false, ref isdefaultCreator);
                    values.IsDefaultNotCreator = isdefaultCreator;
                    values.OwnerMailSettings = MailSettings.none;
                    values.IsDefaultNotOwner = true;
                }
                else
                {
                    bool isdefaultOwner = false;
                    bool isdefaultCreator = false;
                    values.CreatorMailSettings = service.MailSettingsGet(Usr.Id, Tk.TicketId, false, ref isdefaultCreator);
                    values.IsDefaultNotCreator = isdefaultCreator;
                    values.OwnerMailSettings = service.MailSettingsGet(Tk.OwnerId, Tk.TicketId, false, ref isdefaultOwner);
                    values.IsDefaultNotOwner = isdefaultOwner;
                }

                values.IsCreatorNotificationEnable = Usr.IsNotificationActiveUser;
                values.IsOwnerNotificationEnable = Tk.IsOwnerNotificationActive;
            }
            else
            {
                values.OwnerMailSettings = MailSettings.none;
                bool isdefaultCreator = false;
                values.CreatorMailSettings = service.MailSettingsGet(Usr.Id, Tk.TicketId, false, ref isdefaultCreator);
                values.IsDefaultNotCreator = isdefaultCreator;
                values.IsDefaultNotOwner = true;

                values.IsCreatorNotificationEnable = Usr.IsNotificationActiveUser;
                //values.IsOwnerNotificationEnable = Usr.IsNotificationActiveUser;
            }

            values.CanListUsers = CanListUsers(UserContext.CurrentCommunityID);


            //Upload File comunità
            //Repository 4 upload

            int CurPersId = (Usr.Person != null) ? Usr.Person.Id : 0;

            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = service.GetRepositoryPermissions(values.FileCommunityId, CurPersId);
            List<RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(MessageUserType.Partecipant,values.FileCommunityId,Usr.Person.Id,cRepository);
            
            //if (values.TicketData.Attachments != null && values.TicketData.Attachments.Any())
            //{
            //    alreadyLinkedFiles = (from Domain.DTO.DTO_AttachmentItem atc in values.TicketData.Attachments
            //                          where atc.File != null && atc.Link != null
            //                          select new dtoCoreItemFileLink<long>()
            //                          {
            //                              CreatedBy = atc.CreatedBy,
            //                              CreatedOn = atc.CreatedOn,
            //                              Deleted = atc.Deleted,
            //                              ItemFileLinkId = atc.IdAttachment,//fl.Id,
            //                              StatusId = 0,
            //                              Link = atc.Link,
            //                              Owner = atc.CreatedBy,
            //                              isVisible = (atc.Deleted == BaseStatusDeleted.None && !atc.File.isDeleted),
            //                              File = atc.File
            //                          }).ToList<iCoreItemFileLink<long>>();

            //    //ModifiedBy = atc.ModifiedBy,
            //    //ModifiedOn = atc.ModifiedOn,

            //    // && fl.Visibility == Domain.Enums.FileVisibility.visible
            //    //NO: se non è visibile, non lo è per l'utente, manager e resolver lo possono vedere E comunque non posso linkarlo nuovamente.
            //}
            RepositoryAttachmentUploadActions dAction = RepositoryAttachmentUploadActions.none;
            View.InitView(values, actions, dAction, cRepository, Tk.DraftMsgId);
            initCommunitySelector(Tk.CommunityId);
        }

        public void SendTimerAction()
        {
            if (CheckSessionAccess())
            {
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));

                if (View.CurrentTicketId > 0)
                {
                    Objects.Add(ModuleTicket.KVPgetTicket(View.CurrentTicketId));
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.ExternalCreate, -1, ModuleTicket.InteractionType.None, Objects);
                }
                else
                {
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.ExternalCreate, -1, ModuleTicket.InteractionType.None, Objects);
                }
            }
        }

        //TODO: notification - V - test
        public Boolean SaveTicket(Domain.DTO.DTO_Ticket TkData, 
            Domain.Enums.MailSettings  ownerSettings,
            Domain.Enums.MailSettings creatorSettings,
            Boolean ForUpload = false)
        {
            if (!CheckSessionAccess())
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, -1, ModuleTicket.InteractionType.None);

                View.ShowError(Domain.Enums.TicketCreateError.NoPermission);
                return false;
            }

            Domain.Enums.TicketCreateError Error = Domain.Enums.TicketCreateError.none;
            Domain.Enums.TicketAddCondition Cond = service.PermissionTicketUsercanCreate();

            if(Cond == Domain.Enums.TicketAddCondition.CheckCount)
            {
                if(TkData.IsDraft)
                {
                    if (!(service.TicketGetNumDraft(TkData.CreatorId) <= Access.MaxDraft))
                        Error = Domain.Enums.TicketCreateError.ToMuchDraft;
                } else 
                {
                    if (!(service.TicketGetNumOpen(TkData.CreatorId) <= Access.MaxSended))
                        Error = Domain.Enums.TicketCreateError.ToMuchTicket;
                }
            } else if(Cond != Domain.Enums.TicketAddCondition.CanCreate)
            {
                Error = Domain.Enums.TicketCreateError.NoPermission;
            }

            if (Error != Domain.Enums.TicketCreateError.none)
            {
                View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                View.ShowError(Error);
                return false;
            }

            Error = service.TicketCreate(ref TkData);

            Boolean CanSave = false;

            if (Error == Domain.Enums.TicketCreateError.none)
                CanSave = true;
            else if (Error == Domain.Enums.TicketCreateError.NoCategory
                || Error == Domain.Enums.TicketCreateError.NoText
                || Error == Domain.Enums.TicketCreateError.NoTitle)
            {
                CanSave = ForUpload || TkData.IsDraft;
            }

            if (CanSave) 
            {
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetUser(TkData.CreatorId));
                Objects.Add(ModuleTicket.KVPgetTicket(TkData.TicketId));

                if(TkData.IsDraft)
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketCreateDraft, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                else
                    View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.TicketCreate, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action


                //NOTIFICATION

                //Se DISABLED, SOLO per il creator corrente (no behalf)
                if (ownerSettings != MailSettings.DISABLED)
                {
                    bool IsOwner = creatorSettings == MailSettings.DISABLED;
                    service.NotificationSetTicketOwner(TkData.TicketId, ownerSettings, IsOwner);
                }

                if (creatorSettings != MailSettings.DISABLED)
                {
                    service.NotificationSetTicketCreatorCurrent(TkData.TicketId, creatorSettings);
                }

                //TODO: notification - test
                if (!TkData.IsDraft)
                {
                    SendNotification(TkData.DraftMsgId, TkData.CreatorId, ModuleTicket.NotificationActionType.TicketSend); 
                    //SE inviato il DraftMsgdiventa il Primo messaggio.
                    //Il Creator è sempre quello che "fa fede" sulle logiche di invio.
                }   

                View.TicketCreated(TkData.TicketId, TkData.IsDraft);

                
                if (!ForUpload)
                {
                    this.InitView();
                }

            }
            else
            {
                View.ShowError(Error);
                return false;
            }

            return true;
        }

        public void UpdateCommunity(Int32 CommunityId, Int64 SelectedCateId)
        {
            if (!CheckSessionAccess())
                return;

            //values.CurrentCommunityId = CommunityId;

            if (CommunityId == -1)
                CommunityId = UserContext.CurrentCommunityID;
            
            String ComName = service.CommunityNameGet(CommunityId);
            if (String.IsNullOrEmpty(ComName) || ComName == TicketService.ComErrName)
            {
                ComName = TicketService.ComPortalName;
                CommunityId = 0;
                //values.TicketData.CommunityId = 0;
            }

            View.UpdateCommunity(CommunityId, ComName);
            initCommunitySelector(CommunityId);
            View.refreshCategory(service.CategoriesGetTreeDLL(CommunityId, CategoryTREEgetType.FilterUser), SelectedCateId);
        }

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
                    Int32 SysComId = UserContext.CurrentCommunityID;
                    View.ViewCommunityId = SysComId;
                    return SysComId;
                }
            }
        }


        public bool CheckSessionAccess()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(View.ViewCommunityId);
                return false;
            }

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


       

        public void DeleteFile(Int64 idAttachment, String baseFilePath, String baseThumbnailPath) //, String BasePath
        {
            Int64 DraftMsgId = View.DraftMsgId;
            if (idAttachment < 0 || DraftMsgId < 0)
                return;

            if (!CheckSessionAccess())
                return;

            service.AttachmentDelete(idAttachment, baseFilePath, baseThumbnailPath);

            this.InitView();
        }

        public Domain.Enums.TicketDraftDeleteError DeleteDraft(String baseFilePath, String baseThumbnailPath)
        {
            if (!CheckSessionAccess())
                return Domain.Enums.TicketDraftDeleteError.hide;

            return service.TicketDeleteDraft(View.CurrentTicketId,baseFilePath, baseThumbnailPath);

        }

        //TODO: Notification - V - TEST
        public void SetBehalfPerson(Int32 PersonId, bool HideToOwner)
        {
            View.ShowBehalfError(BehalfError.none);

            if (!CheckSessionAccess())
            {
                //Session error
                return;
            }

            Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
            if (Usr.Person == null)
            {
                View.ShowBehalfError(BehalfError.NoPermission);
                //Error: external?
                return;
            }

            if (!service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID, PermissionType.Behalf))
            {
                View.ShowBehalfError(BehalfError.NoPermission);
                //Error: no permission
                return;
            }

            Int64 messageId = 0;
            if (!service.TicketSetBehalfPerson(View.CurrentTicketId, PersonId, HideToOwner, ref messageId))
            {
                View.ShowBehalfError(BehalfError.dBerror);
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
            Objects.Add(ModuleTicket.KVPgetTicket(View.CurrentTicketId));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.BehalfTicketSet, View.ViewCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            this.InitView();
            View.ShowBehalfError(BehalfError.success);
            //OK on page
        }

        public void SetBehalfUser(Int64 UserId, bool HideToOwner)
        {

            View.ShowBehalfError(BehalfError.none);

            if (!CheckSessionAccess())
            {
                //Session error
                return;
            }

            Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
            if (Usr.Person == null)
            {
                View.ShowBehalfError(BehalfError.NoPermission);
                //Error: external?
                return;
            }

            if (!service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID, PermissionType.Behalf))
            {
                View.ShowBehalfError(BehalfError.NoPermission);
                //Error: no permission
                return;
            }

            
            Int64 messageId = 0;
            if (!service.TicketSetBehalfUser(View.CurrentTicketId, UserId, HideToOwner, ref messageId))
            {
                View.ShowBehalfError(BehalfError.dBerror);
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
            Objects.Add(ModuleTicket.KVPgetUser(UserId));
            Objects.Add(ModuleTicket.KVPgetTicket(View.CurrentTicketId));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.BehalfTicketSet, View.ViewCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            this.InitView();
            View.ShowBehalfError(BehalfError.success);
            //OK on page
        }

        //TODO: Notification - V - TEST
        public void RemoveBehalf()
        {

            View.ShowBehalfError(BehalfError.none);

            if (!CheckSessionAccess())
            {
                return;
            }

            Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
            if (Usr.Person == null)
            {
                View.ShowBehalfError(BehalfError.NoPermission);
                return;
            }

            //if (!service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID, PermissionType.Behalf))
            //{
            //    View.ShowBehalfError(BehalfError.NoPermission);
            //    return;
            //}

            Int64 messageId = 0;
            if (!service.TicketSetBehalfCurrent(View.CurrentTicketId, ref messageId))
            {
                View.ShowBehalfError(BehalfError.dBerror);
            }

            //TODO: Notification - TEST
            if (messageId > 0) //se MINORE <= 0 il Ticket è in DRAFT!
            {
                Int64 userId = service.UserGetIdfromPerson(UserContext.CurrentUserID);
                SendNotification(messageId, userId, ModuleTicket.NotificationActionType.OwnerChanged);
            }



            //Begin Action
            List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
            //Objects.Add(ModuleTicket.KVPgetUser(service.UserGetIdfromPerson(UserContext.CurrentUserID)));
            Objects.Add(ModuleTicket.KVPgetTicket(View.CurrentTicketId));

            View.SendUserActions(service.ModuleID, ModuleTicket.ActionType.BehalfTicketRemove, View.ViewCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            //End Action

            this.InitView();
            View.ShowBehalfError(BehalfError.deleteSuccess);
            //OK on page
        }

        //public void SetBehalf(int PersonId)
        //{
        //    if (!CheckSessionAccess())
        //        return;
            
        //    Domain.TicketUser Usr = service.UserGetfromPerson(UserContext.CurrentUserID);
        //    if (Usr.Person == null)
        //    {
        //        //Error: external?
        //        return;
        //    }

        //    if (!service.SettingPermissionGet(Usr.Id, Usr.Person.TypeID, PermissionType.Behalf))
        //    {
        //        //Error: no permission
        //        return;
        //    }

        //    if (service.TicketSetBehalfPerson(View.CurrentTicketId, PersonId))
        //    {
        //        this.InitView();
        //        //OK: message
        //    } else
        //    { 
        //        //Error: message
        //    }
                

        //}

        public void InitPersonSelector()
        {
            Int32 currentOwnerId = service.TicketGetOwnerPersonId(View.CurrentTicketId);


            int communityId = UserContext.CurrentCommunityID;
            //bool canList = (communityId <= 0 ? canListUsersPortal() : canListUsersCommunityCurrent());

            if (currentOwnerId > 0)
            {
                List<int> currentSelection = new List<int>();
                currentSelection.Add(currentOwnerId);
                View.InitPersonSelector(currentSelection, communityId);
            }
            else
            {
                View.InitPersonSelector(null, communityId);
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

            if (!service.TicketVisibilitySet(View.CurrentTicketId, HideToOwner))
            {
                View.ShowBehalfError(Domain.Enums.BehalfError.dBerror);
                return;
                //Error: dB
            }

            this.InitView();
            View.ShowBehalfError(Domain.Enums.BehalfError.visibilitySuccess);

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

        private bool CanListUsers(int CommunityId)
        {

            if (CommunityId <= 0)
            {


                //COPIA da SELETTORE UTENTI (SelectUserPresenter.cs)
                //

                ProfileManagement.ModuleProfileManagement module =
                    ProfileManagement.ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);

                ProfileManagement.Business.ProfileManagementService profileService =
                    new ProfileManagement.Business.ProfileManagementService(AppContext);


                List<Organization> organizations = profileService.GetAvailableOrganizations(UserContext.CurrentUserID,
                    (module.ViewProfiles || module.Administration)
                        ? SearchCommunityFor.SystemManagement
                        : SearchCommunityFor.CommunityManagement);

                return (organizations.Any() || module.Administration || module.ViewProfiles);
            }
            else
            {
                CommunityManagement.Business.ServiceCommunityManagement serviceCommunity = new CommunityManagement.Business.ServiceCommunityManagement(AppContext);

                DomainModel.Domain.ModuleCommunityManagement cModule = serviceCommunity.GetModulePermission(UserContext.CurrentUserID, CommunityId);

                return cModule.Administration || cModule.Manage;
            }
        }

        public void AttachmentsAddInternal(
           Domain.DTO.DTO_Ticket TkData,
           Domain.Enums.MailSettings ownerSettings,
           Domain.Enums.MailSettings creatorSettings)
        {
            if (!this.SaveTicket(TkData, ownerSettings, creatorSettings, true))
                return;

            Domain.Message message = service.MessageGetFromTicketDraft(TkData.TicketId, TkData.CreatorId);

            if (message == null || View.DraftMsgId != message.Id)
                return;

            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> FileActions = service.UploadAvailableActionsGet(Domain.Enums.MessageUserType.Partecipant, 0, 0, null);


            if (!FileActions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem))
                return;

            service.AttachmentsAddFiles(message.Id, View.GetUploadedItems(message, RepositoryAttachmentUploadActions.uploadtomoduleitem));

            this.InitView();

        }
        public void AttachmentsAddAlsoToCommunity(
            Domain.DTO.DTO_Ticket TkData,
            Domain.Enums.MailSettings ownerSettings,
            Domain.Enums.MailSettings creatorSettings,
             Boolean alwaysLastVersion)
        {
            if(!this.SaveTicket(TkData, ownerSettings, creatorSettings, true))
                return;

            Domain.Message Msg = service.MessageGetFromTicketDraft(TkData.TicketId, TkData.CreatorId);

            if (Msg == null || View.DraftMsgId != Msg.Id)
                return;

            int UserID = (this.UserContext != null) ? UserContext.CurrentUserID : 0;
            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = service.GetRepositoryPermissions(CurrentCommunityId, UserID);
            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(MessageUserType.Partecipant, CurrentCommunityId, UserContext.CurrentUserID, cRepository);

            if (!actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity))
                return;

            //service.MessageDraftUpdate(HtmlText, PreviewText, View.CurrentTicketId, View.DraftMsgId);
            //-------------------------------------------------------------------------------------
            service.AttachmentsAddFiles(View.DraftMsgId, View.GetUploadedItems(Msg, RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity));
            //-------------------------------------------------------------------------------------

            this.InitView();
        }

        public void AttachmentsLinkFromCommunity(Domain.DTO.DTO_Ticket TkData, 
            Domain.Enums.MailSettings  ownerSettings,
            Domain.Enums.MailSettings creatorSettings,
            List<ModuleActionLink> links)
        {
            if (!this.SaveTicket(TkData, ownerSettings, creatorSettings, true))
                return;

            Domain.Message Msg = service.MessageGetFromTicketDraft(TkData.TicketId, TkData.CreatorId);

            if (Msg == null || View.DraftMsgId != Msg.Id)
                return;

            int UserID = (this.UserContext != null) ? UserContext.CurrentUserID : 0;
            lm.Comol.Core.FileRepository.Domain.ModuleRepository cRepository = service.GetRepositoryPermissions(CurrentCommunityId, UserID);

            List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = service.UploadAvailableActionsGet(MessageUserType.Partecipant, CurrentCommunityId, UserContext.CurrentUserID, cRepository);
            if (!actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity))
                return;

            IList<Domain.TicketFile> attachments = service.AttachmentsLinkFiles(View.DraftMsgId, links);
            int addedfiles = 0;

            if (attachments != null)
                addedfiles = attachments.Count();

            this.InitView();
        }


        public void initCommunitySelector(int idCommunity)
        {
            Boolean forAdmin = false;
                //UserContext.UserTypeID == (Int32)UserTypeStandard.SysAdmin ||
                //UserContext.UserTypeID == (Int32)UserTypeStandard.Administrator ||
                //UserContext.UserTypeID == (Int32)UserTypeStandard.Administrative;

            CommunityManagement.CommunityAvailability availability = forAdmin ? CommunityManagement.CommunityAvailability.All : CommunityManagement.CommunityAvailability.Subscribed;

            var rPermissions = new Dictionary<Int32, Int64>();
            //rPermissions.Add(Service.GetServiceIdModule(), (long)(ModuleGlossaryNew.Base2Permission.Admin | ModuleGlossaryNew.Base2Permission.AddGlossary)); <- Solo glossari

            List<Int32> unloadIdCommunities = new List<Int32>();
            if (idCommunity > 0) unloadIdCommunities.Add(idCommunity);

            //{idComunitàDaRimuover SE PRESENTE (Comunità corrente. Comunità del ticket, ecc) }

            View.InitializeCommunitySelector(forAdmin, UserContext.CurrentUserID, unloadIdCommunities, availability, rPermissions);

        }

        //private void SendNotification(Int64 MessageId, Int64 TkUserId)
        //{

        //    NotificationAction action = new NotificationAction();
        //    action.IdCommunity = CurrentCommunityId;
        //    action.IdObject = MessageId;
        //    action.IdObjectType = (long)ModuleTicket.ObjectType.Message;
        //    action.ModuleCode = ModuleTicket.UniqueCode;

        //    action.IdModuleUsers = new List<long>();
        //    action.IdModuleUsers.Add(TkUserId);

        //    action.IdModuleAction = (long)ModuleTicket.NotificationActionType.TicketSend;

        //    //IList<NotificationAction> actions = new List<NotificationAction>();
        //    //actions.Add(action);

        //    //action.IdModuleAction = (long) ModuleTicket.MailSenderActionType.TicketSendMessageMan;
            
        //    View.SendNotification(action, UserContext.CurrentUserID);

        //}
    }


}
