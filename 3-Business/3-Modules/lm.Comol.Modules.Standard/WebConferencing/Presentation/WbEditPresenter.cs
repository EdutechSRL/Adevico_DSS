using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using WCMod = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing;

namespace lm.Comol.Modules.Standard.WebConferencing.Presentation
{
    public class WbEditPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region costruttori/view/service/module

        private lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService ServiceTemplate;
        

        public WbEditPresenter(iApplicationContext oContext)
            : base(oContext)
        { }

        public WbEditPresenter(iApplicationContext oContext, View.iViewWbEdit view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
            this.ServiceTemplate = new Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
        }

        protected virtual View.iViewWbEdit View
        {
            get { return (View.iViewWbEdit)base.View; }
        }

        private Domain.ModuleWebConferencing _module;
        private Domain.ModuleWebConferencing Module
        {
            get
            {
                if ((_module == null))
                {
                    Int32 idUser = UserContext.CurrentUserID;
                    _module = Service.GetServicePermission(idUser, this.CurrentCommunityId);
                }
                return _module;
            }
        }

        Domain.WbService _Service;
        private Domain.WbService Service
        {
            get
            {
                if (_Service == null)
                {
                    if (View.SysParameter.CurrentSystem == Domain.wBImplementedSystem.eWorks)
                    {
                        Domain.eWorks.eWSystemParameter param = (Domain.eWorks.eWSystemParameter)View.SysParameter;
                        _Service = new Domain.eWorks.eWService(param, this.AppContext);
                    }
                    else if (View.SysParameter.CurrentSystem == Domain.wBImplementedSystem.OpenMeetings)
                    {
                        Domain.OpenMeetings.oMSystemParameter param = (Domain.OpenMeetings.oMSystemParameter)View.SysParameter;
                        _Service = new Domain.OpenMeetings.oMService(param, this.AppContext);
                    }
                }

                return _Service;
            }
        }

        #endregion

        /// <summary>
        /// Inizializza la View (da PageLoad)
        /// </summary>
        public void InitView()
        {
            if (UserContext.isAnonymous)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoUser);
                View.DisplaySessionTimeout();
            }
            else if (!ServerStatus())
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoServer);
                View.DisplayNoServer();
            }
            else
            {
                Domain.WbRoom oRoom = Service.RoomGet(View.RoomId);

                if (oRoom == null || oRoom.CommunityId != UserContext.CurrentCommunityID)
                {
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                    View.DisplayNoPermission();
                }
                else if (Module.ManageRoom || Module.AddChatRoom)
                {
                    
                    Boolean LockUsers = Module.AddChatRoom && !(Module.ManageRoom);

                    //SOLO per eWorks: inizializza le Drop Down List.
                    Domain.eWorks.DTO.DTOAvailableParameters eWDefParameter = null;
                    if (View.SysParameter.CurrentSystem == Domain.wBImplementedSystem.eWorks)
                    {
                        Domain.eWorks.eWSystemParameter eWSysParam = (Domain.eWorks.eWSystemParameter)View.SysParameter;
                        eWDefParameter = Domain.eWorks.eWAPIConnector.getAvailableParameters(
                            eWSysParam.BaseUrl, eWSysParam.ProxyUrl, eWSysParam.MainUserId, eWSysParam.MainUserPwd, eWSysParam.MainUserId);
                    }

                    String code = Service.RoomCodeGet(View.RoomId);

                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomEdit);
                    this.View.Init(eWDefParameter, LockUsers, code, oRoom.CommunityId, oRoom.Id);

                    resetData();
                }
                else
                {
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                    View.DisplayNoPermission();
                }
            }

        }

        /// <summary>
        /// Reimposta i dati dall'ultimo salvataggio
        /// </summary>
        private void resetData()
        {
            Domain.WbRoom Room = Service.RoomGet(View.RoomId);

            if (Room != null)
            {
                Domain.DTO.DTO_GenericRoomData Data = new Domain.DTO.DTO_GenericRoomData()
                {
                    Id = Room.Id,
                    HasIdInName = Room.hasIdInName,
                    Description = Room.Description,
                    Duration = Room.Duration,
                    EndDate = Room.EndDate,
                    StartDate = Room.StartDate,
                    Public = Room.Public,
                    Name = Room.Name,
                    MaxAllowUsers = Room.MaxAllowUsers, 
                    NotificationEnableUsr = Room.NotificationEnableUsr,
                    NotificationDisableUsr = Room.NotificationDisableUsr
                };

               
                //TemplateId = Room.TemplateId

                

                String code = Service.RoomCodeGet(this.View.RoomId);


                int PageCount = 0;

                int currentPageIndex = this.View.UserPager.PageIndex;
                int currentPageSize = this.View.UserPager.PageSize;

                IList<Domain.WbUser> Users = Service.UsersGet(this.View.RoomId, this.View.UserFilters, currentPageIndex, currentPageSize, ref PageCount);
                
                PagerBase pager = new PagerBase();
                pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
                pager.Count = PageCount;
                if (currentPageIndex > PageCount)
                    currentPageIndex = PageCount;

                pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
                View.UserPager = pager;


                Boolean LockUsers = Module.AddChatRoom && !(Module.ManageRoom);

                this.View.SetParameter(Room.Id, Room.CreatedBy.Id, Data, Room.Parameter, Room.Type, Users, LockUsers, code); //, LockUsers
                View.RoomType = Room.Type;

                List<Domain.DTO.DTO_AccessType> AccessType = new List<Domain.DTO.DTO_AccessType>();

                Domain.DTO.DTO_AccessType Community = new Domain.DTO.DTO_AccessType();
                Community.ID = -1;
                Community.IsSystem = true;
                Community.DisplayName = Domain.SysSubscriptionType.Community.ToString();
                Community.SelectedType = Room.SubCommunity;
                AccessType.Add(Community);

                Domain.DTO.DTO_AccessType System = new Domain.DTO.DTO_AccessType();
                System.ID = -2;
                System.IsSystem = true;
                System.DisplayName = Domain.SysSubscriptionType.System.ToString();
                System.SelectedType = Room.SubSystem;
                AccessType.Add(System);

                Domain.DTO.DTO_AccessType External = new Domain.DTO.DTO_AccessType();
                External.ID = -3;
                External.IsSystem = true;
                External.DisplayName = Domain.SysSubscriptionType.External.ToString();
                External.SelectedType = Room.SubExternal;
                AccessType.Add(External);

                View.RoomAccessTypes = AccessType;
            }
        }

        /// <summary>
        /// Salva TUTTI i dati presenti nella view
        /// </summary>
        /// <param name="UpdateView">
        /// Se TRUE aggiorna i dati nella view
        /// </param>
        public void Save(Boolean UpdateView)
        {
            Domain.DTO.DTO_GenericRoomData grData = View.CurrentRoomData;

            IList<Domain.DTO.DTO_AccessType> sysAccType = (from Domain.DTO.DTO_AccessType at in View.RoomAccessTypes where at.IsSystem == true select at).ToList();

            if (sysAccType != null && sysAccType.Count > 0)
            {
                foreach (Domain.DTO.DTO_AccessType at in sysAccType)
                {
                    if (at.ID == -1)
                        grData.SubCommunity = at.SelectedType;
                    else if (at.ID == -2)
                        grData.SubSystem = at.SelectedType;
                    else if (at.ID == -3)
                        grData.SubExternal = at.SelectedType;
                }
            }

            Domain.WbRoom UpdatedRoom = Service.RoomUpdateData(
                View.RoomId,
                grData, 
                View.CurrentRoomParameters,
                View.SYS_HasIdInName
                );

            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomUpdate);

            if(UpdateView)
                resetData();
        }

        /// <summary>
        /// Cancella utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        public void DeleteUser(Int32 UserId)
        {
            this.Service.UserDelete(UserId, View.RoomId);
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserRemove);
            resetData();
        }

        /// <summary>
        /// Disabilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        public void DisableUser(Int32 UserId)
        {
            bool SendMail = this.Service.UserDisable(UserId, View.RoomId);

            bool Sended = true;

            if (SendMail)
            {
                Sended = SendLockUnlockUser(UserId, true);
            }
            
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserLock);

            
            resetData();

            if (!Sended)
                View.DisplayMailNotSended();

        }

        /// <summary>
        /// Abilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        public void EnableUser(Int32 UserId)
        {
            bool SendMail = this.Service.UserEnable(UserId, View.RoomId);
            bool Sended = true;

            if (SendMail)
            {
                Sended = SendLockUnlockUser(UserId, false);
            }
                        
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserUnlock);

            resetData();

            if (!Sended)
                View.DisplayMailNotSended();


        }

        /// <summary>
        /// Invia credenziali all'utente indicato.
        /// SE non possiede già una chiave, questa viene generata.
        /// </summary>
        /// <param name="UserId"></param>
        public void SendUserKey(Int64 UserId) //, Int64 TemplateId)
        {
            Domain.WbUser usr = Service.UserGet(UserId);
            Domain.WbRoom room = Service.RoomGet(View.RoomId);

            //if (room.TemplateId != TemplateId)
            //{
            //    Service.RoomUpdateTemplate(View.RoomId, TemplateId);
            //    room.TemplateId = TemplateId;
            //}

            //Aggiungere gestione errore
            if (usr == null || room == null)
                return;

            lm.Comol.Core.Notification.Domain.dtoNotificationMessage msg = 
                ServiceTemplate.GetNotificationMessage(
                    usr.LanguageCode,
                    WCMod.UniqueCode,
                    (Int64)WCMod.MailSenderActionType.Credential);

            if (msg == null)
            {
                View.DisplayMailNotSended();
            }
            else
            {
                //String BaseUrl = "";
                //String DateTimeFormat = "";
                //String VoidDatetime = "";
                //String PortalName = ""; //  SystemSetting.InstanceName

                Domain.DTO.DTO_MailTagSettings mts = View.GetMailTagSetting(); // 2 = Language ID!

                msg.Translation = Service.GetTemplateContentPreview(
                    true,
                    View.RoomId,
                    UserId,
                    mts.Baseurl,
                    mts.WebSiteUrlNoSsl,
                    msg.Translation,
                    mts.DateTimeFormat,
                    mts.VoidDateTime,
                    mts.PortalName
                    );


                bool sentMail = ServiceTemplate.SendMail(
                    this.Service.CurrentUser,
                    mts.SmtpConfig,
                    msg.MailSettings,
                    msg.Translation.Subject,
                    msg.Translation.Body,
                    usr.Mail);


                if (sentMail == false)
                    View.DisplayMailNotSended();
            }

            //            person,				//Chi manda (admin)
            //smtp,				// da view (vedere)
            //message.MailSettings,		
            //message.Translation.Subject,
            //message.Translation.Body,
            //"francesco.conte@unitn.it")	//Mail destinatario (o mail, separate da ';'  )sentMail = service.SendMail(
            //person,				//Chi manda (admin)
            //smtp,				// da view (vedere)
            //message.MailSettings,		
            //message.Translation.Subject,
            //message.Translation.Body,
            //"francesco.conte@unitn.it")	//Mail destinatario (o mail, separate da ';'  )

            //public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTemplateContentPreview(
           
        }

        /// <summary>
        /// Invia un messaggio a TUTTI gli utenti selezionati.
        /// NOTA: foreach per ogni utente... (rivedere)
        /// Ottimizzazioni: recuperare IDictionary di Template con chiave Codice/Id lingua e valore il tmeplate.
        /// </summary>
        /// <param name="UsersId">Lista di Utenti WebConference</param>
        /// <param name="TemplateId">Id template selezionato. SE -1: default!</param>
        public void SendUsersKey() //Int64 TemplateId)
        {
            IList<Int64> UsersId = View.GetSelectedUsers();
            foreach (Int64 usrId in UsersId)
            {
                SendUserKey(usrId); //, TemplateId);
            }
        }

        /// <summary>
        /// Invia invito/credenziali
        /// </summary>
        /// <param name="UserId">ID utente stanza</param>
        /// <param name="IsLock">
        /// TRUE = Blocco utente
        /// FALSE = SBLOCCO utente
        /// </param>
        /// <returns>
        /// True = mail inviata
        /// False = errore template
        /// </returns>
        /// <remarks>
        /// La mail verrà SEMPRE inviata ad utenti esterni che attendono conferma da parte dell'amministratore.
        /// </remarks>
        public Boolean SendLockUnlockUser(Int64 UserId, Boolean IsLock)
        {
            Domain.WbUser usr = Service.UserGet(UserId);
            Domain.WbRoom room = Service.RoomGet(View.RoomId);

            //Aggiungere gestione errore
            if (usr == null || room == null)
                return true;

            //int LangId = 2; // -> 

            Int64 Action = (Int64)WCMod.MailSenderActionType.UnLockUser;

            if(IsLock)
            {
                Action = (Int64)WCMod.MailSenderActionType.LockUser;
            }

            lm.Comol.Core.Notification.Domain.dtoNotificationMessage msg =
                ServiceTemplate.GetNotificationMessage(
                    usr.LanguageCode,
                    WCMod.UniqueCode,
                    Action);

            if (msg == null)
            { 
                
                return false;
            }

            //String BaseUrl = "";
            //String DateTimeFormat = "";
            //String VoidDatetime = "";
            //String PortalName = ""; //  SystemSetting.InstanceName

            Domain.DTO.DTO_MailTagSettings mts = View.GetMailTagSetting(); // 2 = Language ID!

            msg.Translation = Service.GetTemplateContentPreview(
                true,
                View.RoomId,
                UserId,
                mts.Baseurl,
                mts.WebSiteUrlNoSsl,
                msg.Translation,
                mts.DateTimeFormat,
                mts.VoidDateTime,
                mts.PortalName
                );


            bool sentMail = ServiceTemplate.SendMail(
                this.Service.CurrentUser,
                mts.SmtpConfig,
                msg.MailSettings,
                msg.Translation.Subject,
                msg.Translation.Body,
                usr.Mail);

            if (sentMail == false)
                View.DisplayMailNotSended();

            return true;
        }

        /// <summary>
        /// Aggiunge utenti alla stanza
        /// </summary>
        /// <param name="Users">Lista di utenti "WB"</param>
        public void AddUsers(IList<Domain.WbUser> Users)
        {
            Service.UsersAdd(Users, View.RoomId);
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddInternal);
            resetData();
        }

        /// <summary>
        /// Aggiunge PERSON alla stanza
        /// </summary>
        /// <param name="PersonsIds">ID Person da aggiugnere</param>
        /// <param name="Audio">Abilitare l'audio per tutti gli utenti indicati</param>
        /// <param name="Video">Abilitare il video per tutti gli utenti indicati</param>
        /// <param name="Chat">Abilitare la chat per tutti gli utenti indicati</param>
        /// <param name="Admin">Gli utenti aggiunti saranno amministratori della stanza</param>
        public void AddPersonsByIds(IList<Int32> PersonsIds, bool Audio, bool Video, bool Chat, bool Host, bool Controller)
        {
            Service.UserPersonAddIds(PersonsIds, View.RoomId, Audio, Video, Chat, Host, Controller);
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddInternal);
            resetData();
        }

        /// <summary>
        /// Aggiorna i paramtri utenti
        /// </summary>
        /// <param name="Users">Utenti WB con i dati aggiornati</param>
        /// <param name="UpdateView">Se aggiornare la view con i dati salvati</param>
        public void UpdateUsersParameters(IList<Domain.WbUser> Users, Boolean UpdateView)
        {
            Service.UsersUpdate(Users, View.RoomId);
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserUpdate);
            if(UpdateView)
                resetData();
        }

        /// <summary>
        /// Aggiorna codice stanza
        /// </summary>
        public void UpdateRoomCode()
        {
            Service.RoomCodeGenerate(View.RoomId);
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomUpdate);
            Save(true);
        }

        /// <summary>
        /// Elimina codice stanza
        /// </summary>
        public void DeleteAccessCode()
        {
            Service.RoomCodeDelete(View.RoomId);
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomUpdate);
            Save(true);
        }

        /// <summary>
        /// Aggiorna chiave accesso utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        public void UpdateUserCode(Int32 UserId)
        {
            this.Service.GenerateUserCode(View.RoomId, UserId);
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserUpdate);
            resetData();
        }

        /// <summary>
        /// Aggiunge utenti esterni
        /// </summary>
        /// <param name="Users">Lista dati utenti</param>
        /// <returns>
        /// True: ALL user added
        /// False: some error
        /// </returns>
        public bool AddExternalUsers(IList<Domain.DTO.DTO_ExtUser> Users)
        {
            IList<Domain.DTO.DTO_ExtUser> ErrUsers = this.Service.UsersExternalsAdd(View.RoomId, Users);
            Save(true);
            
            if (ErrUsers != null && ErrUsers.Count > 0)
            {
                this.View.ShowErrUsers(ErrUsers);
                return false;
            }
            else {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddExternal);
                //resetData();
                return true;
            }
        }

        /// <summary>
        /// Controllo stato del server
        /// </summary>
        /// <returns>
        /// True: server OK
        /// False: server irraggiungibile
        /// </returns>
        public Boolean ServerStatus()
        {
            return this.Service.ServerCheck();
        }

        /// <summary>
        /// Invia azioni utente
        /// </summary>
        /// <param name="Action">Azione compiuta</param>
        private void SendUserAction(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType Action)
        {
            View.SendUserAction(this.CurrentCommunityId, Service.ServiceModuleID(), Action);
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
            }
        }

        /// <summary>
        /// Recupera lingue disponibili per il sistema (Config)
        /// </summary>
        /// <returns>
        /// Lista dingue disponibili
        /// </returns>
        public IList<Domain.DTO.DTO_Language> GetLanguages()
        {
            List<Int32> LanguagesId = new List<int>();

            foreach (KeyValuePair<int, string> LangKeyPair in View.SystemLanguages)
            {
                LanguagesId.Add(LangKeyPair.Key);
            }

            return Service.LanguagesGet(LanguagesId);
        }

        public Boolean InvitationsSend()
        {
            return true;
        }

        public int KeysGenerate(bool RegenerateAll)
        {
            int generated = Service.GenerateUsersCode(View.RoomId, View.GetSelectedUsers(), RegenerateAll);
            this.InitView();
            return generated;
        }

    }
}