using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

//using OMroomService;


namespace lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings
{
    public partial class oMService : WbService
    {

    #region Properties
        private lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService ServiceTemplate;
        private static string DateFormat = "yyyy-MM-dd";
        private static string TimeFormat = "hh:mm";
        private static long OMP_Roomtypes_id = 1;    //conference
        private static bool OMP_appointment = false;
        private static bool OMP_isDemoRoom = false;
        private static int OMP_demoTime = 120;
        private static int OMP_reminderTypeId = 1;  //none

        private oMSystemParameter oMSysParameter
        {
            get
            {
                return (oMSystemParameter)base.SysParameter;
            }
        }

        private OMroomService.RoomServicePortTypeClient _RoomService;
        private OMroomService.RoomServicePortTypeClient RoomService
        {
            get
            {
                if (_RoomService == null)
                {
                    try
                    {
                        _RoomService = new OMroomService.RoomServicePortTypeClient();
                    }
                    catch (Exception ex)
                    { }
                }
                return _RoomService;
            }
        }

        private OMuserService.UserServicePortTypeClient _UserService;
        private OMuserService.UserServicePortTypeClient UserService
        {
            get
            {
                if (_UserService == null)
                {
                    try
                    {
                        _UserService = new OMuserService.UserServicePortTypeClient();
                    }
                    catch (Exception ex)
                    {
                        string test = ex.ToString();
                    }
                }
                return _UserService;
            }
        }

    #endregion

    #region Costruttori
        

        public oMService(oMSystemParameter SysParameter, iApplicationContext oContext)
            : base(SysParameter, oContext)
        {
            if (SysParameter.GetType() != typeof(oMSystemParameter)) throw new ArgumentException("Wrong SysParameter Type. Must be oMSystemParameter!");
            base.SysParameter = SysParameter;
            this.DAL = new DAL.WbGenericDAL(oContext);
            ServiceTemplate = new Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
        }
        

        public oMService(oMSystemParameter SysParameter, iDataContext oDC)
            : base(SysParameter, oDC)
        {
            if (SysParameter.GetType() != typeof(oMSystemParameter)) throw new ArgumentException("Wrong SysParameter Type. Must be oMSystemParameter!");
            base.SysParameter = SysParameter;
            this.DAL = new DAL.WbGenericDAL(oDC);
            ServiceTemplate = new Core.TemplateMessages.Business.TemplatesForOtherService(oDC);
        }

    #endregion

    #region System/Generics

        /// <summary>
        /// Controlla lo stato del server
        /// </summary>
        /// <returns>
        /// True: server funziona
        /// False: server non raggiungibile o errore server (controllare configurazione e stato server)
        /// </returns>
        public override bool ServerCheck()
        {
            if (RoomService == null || UserService == null)
                return false;

            if (RoomService.State == System.ServiceModel.CommunicationState.Faulted || UserService.State == System.ServiceModel.CommunicationState.Faulted)
                return false;

            try
            {
                // SESSIONE DI LAVORO
                OMuserService.Sessiondata oResponse = UserService.getSession();
                String SessionID = oResponse.session_id;

                // LOGIN DI UN ADMINISTRATOR 
                //  SE < 0 errore LOGIN! =-> Usare getErrorByCode(String SID, long errorid, long language_id)) 
                if (UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd) < 0)
                    return false;
            }
            catch { return false; }

            return true;
        }

        /// <summary>
        /// Recupera i dati avanzati di una stanza in base al tipo della stessa
        /// </summary>
        /// <param name="Type">Tipo di stanza</param>
        /// <returns>Dati avanzati TIPIZZATI sul sistema in uso</returns>
        public override WbRoomParameter ParameterGetByType(RoomType Type)
        {
            oMRoomParameters Param = new oMRoomParameters();

            Param.isPasswordProtected = false;
            Param.password = "";

            switch (Type)
            {
                case RoomType.VideoChat:
                    Param.numberOfPartizipants = 2;
                    Param.isModeratedRoom = false;
                    break;
                case RoomType.Meeting:
                    Param.numberOfPartizipants = 5;
                    Param.isModeratedRoom = false;
                    break;
                case RoomType.Lesson:
                    Param.numberOfPartizipants = 50;
                    Param.isModeratedRoom = true;
                    break;
                case RoomType.Conference:
                    Param.numberOfPartizipants = 200;
                    Param.isModeratedRoom = true;
                    break;
                case RoomType.Custom:
                    Param.numberOfPartizipants = 20;
                    Param.isModeratedRoom = false;
                    break;
            }

            return Param;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Controlla la possibilità di aggiungere una mail ad una determinata stanza
        /// </summary>
        /// <param name="Mail">Mail da aggiungere</param>
        /// <param name="RoomKey">Chiave stanza</param>
        /// <returns></returns>
        public override MailCheck MailServiceCheck(string Mail, string RoomKey)
        {
            return MailCheck.MailUnknow;
        }

        /// <summary>
        /// Recupera l'indirizzo per l'accesso alla stanza.
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UserId">Id Utente stanza</param>
        /// <returns>
        /// Stringa vuota: non è possibile accesere
        /// URL accesso alla stanza
        /// </returns>
        /// <remarks>
        /// Per alcune implementazioni è necessario generare l'url di volta in volta ad ogni accesso.
        /// Le "vecchie" logiche sul "pubblica" sono state eliminate.
        /// </remarks>
        public override string AccessUrlExternalGet(long RoomId, long UserId)
        {
            WbUser User = DAL.UserGet(UserId);
            WbRoom Room = DAL.RoomGet(RoomId);

            if (Room != null && (Room.Public || User != null))
            {

                oMRoomParameters Param = null;
                try
                {
                    Param = (oMRoomParameters)Room.Parameter;
                }
                catch
                {
                    return "";
                }


                if (User == null || User.Enabled == false)
                {
                    return "";
                }

                String URL = "";

                // SESSIONE DI LAVORO
                OMuserService.Sessiondata oResponse = UserService.getSession();
                String SessionID = oResponse.session_id;
                OMuserService.ErrorResult oError = null;

                // LOGIN DI UN ADMINISTRATOR
                long UserID = UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd);

                //String Name = User.DisplayName.Split(' ')[0];
                //String SurName = User.DisplayName.Remove(0, Name.Length + 1);

                int becomModerator = 0;
                if (User.IsController || User.IsHost)
                    becomModerator = 1;

                int showAudioVideoTest = 1;
                //0 means don't show Audio/Video Test, 1 means show Audio/Video Test Application before the user is logged into the room

                long ExtRoomId = 0;
                try { ExtRoomId = System.Convert.ToInt64(Room.ExternalId); }
                catch { }


                URL = UserService.setUserObjectAndGenerateRoomHash(
                    SessionID,
                    User.DisplayName,
                    User.Name,
                    User.SName,
                    "xx",
                    User.Mail,
                    User.PersonID.ToString(),
                    "COL",
                    ExtRoomId,
                    becomModerator,
                    showAudioVideoTest);

                //RECUPERARE LA PRIMA PARTE DELL'INDIRIZZO DA CONFIGURAZIONE!!!
                return oMSysParameter.BaseUrl + "?secureHash=" + URL + "&language=1&lzproxied=solo"; // URL;
            }


            return "";
        }
     
    #endregion

    #region Room management

        /// <summary>
        /// Crea una stanza
        /// </summary>
        /// <returns>Id della nuova stanza</returns>
        public override long RoomCreate(WbRoom Room, bool SysHasIdInName)
        {
            if (Room == null || Room.Parameter == null || Room.Parameter.GetType() != typeof(oMRoomParameters)) throw new ArgumentException("Parameter must be eWRoomParameters!");

            oMRoomParameters Param = (oMRoomParameters)Room.Parameter;

            if (Room.StartDate == null)
                Room.StartDate = DateTime.Now;

            if (Room.Type == RoomType.VideoChat)
                Room.SubCommunity = SubscriptionType.None;
            else if (Room.Public)
                Room.SubCommunity = SubscriptionType.Free;
            else
                Room.SubCommunity = SubscriptionType.Moderated;

            if (Room.Type == RoomType.VideoChat)
                Room.SubSystem = SubscriptionType.None;
            else if (Room.Public)
                Room.SubSystem = SubscriptionType.Free;
            else
                Room.SubSystem = SubscriptionType.Moderated;

            if (Room.Type == RoomType.VideoChat)
                Room.SubExternal = SubscriptionType.None;
            else if (Room.Public)
                Room.SubExternal = SubscriptionType.Moderated;
            else
                Room.SubExternal = SubscriptionType.None;


            // SESSIONE DI LAVORO
            OMuserService.Sessiondata oResponse = UserService.getSession();
            String SessionID = oResponse.session_id;
            OMuserService.ErrorResult oError = null;

            // LOGIN DI UN ADMINISTRATOR
            long UserID = UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd);

            //Creazione stanza su OpenMeeting

            //Aggiornamento parametri
            Param.validFromDate = DateTime.Now;
            Param.validToDate = Param.validFromDate;
            Param.validToDate = Param.validToDate.AddYears(1);

            if (Room.StartDate != null)
                Param.validFromDate = (DateTime)Room.StartDate;

            if (Room.EndDate != null)
                Param.validToDate = (DateTime)Room.EndDate;
            else if (Room.Duration != null && Room.Duration > 0)
            {
                Param.validToDate = Param.validFromDate;
                Param.validToDate = Param.validToDate.AddMinutes(Room.Duration);
            }

            //Set RoomParameters
            //new type of room (1 = Conference, 2 = Audience, 3 = Restricted, 4 = Interview)
            //Effective: 
            //1: interview
            //2: conference
            //Param.roomtypes_id = 2;

            Param.isModeratedRoom = false;

            Param.redirectURL = "";
            //Param.appointment = false;
            //Param.isDemoRoom = false;
            //Param.demoTime = 120;
            //Param.reminderTypeId = 1;

            switch (Room.Type)
            {
                case RoomType.VideoChat:
                    Param.numberOfPartizipants = 2;
                    break;
                case RoomType.Meeting:
                    Param.numberOfPartizipants = 5;
                    break;
                case RoomType.Lesson:
                    Param.numberOfPartizipants = 50;
                    break;
                case RoomType.Conference:
                    Param.numberOfPartizipants = 100;
                    break;
            }

            long RoomId = -1;

            /*
             * ATTENZIONE!!!
             *  nonostante le specifiche, il formato data/ora corretto è quello dichiarato ad inizio classe!
             *  Altrimenti, va in eccezione con il seguente errore "unknow"!!!
             */

            try
            {
                RoomId = RoomService.addRoomWithModerationAndExternalTypeAndStartEnd(SessionID,
                    Room.Name,
                    OMP_Roomtypes_id,
                    Room.Description,
                    Param.numberOfPartizipants,
                    Room.Public,
                    OMP_appointment,
                    OMP_isDemoRoom,
                    OMP_demoTime,
                    Param.isModeratedRoom,
                    Room.Type.ToString(),
                    Param.validFromDate.ToString(DateFormat),
                    Param.validFromDate.ToString(TimeFormat),
                    Param.validToDate.ToString(DateFormat),
                    Param.validToDate.ToString(TimeFormat),
                    Param.isPasswordProtected,
                    Param.password,
                    OMP_reminderTypeId,
                    Param.redirectURL
                    );
            }
            catch (Exception ex)
            {
                RoomId = -2;
            }

            if (RoomId > 0)
            {
                Room.ExternalId = RoomId.ToString();
                this.DAL.RoomCreate(Room);

                //Genero anche il codice stanza alla creazione.
                this.RoomCodeGenerate(Room.Id);
            }


            if (SysHasIdInName)
                RoomNameUpdateNameWithId(Room.Id);


            return Room.Id;
        }

        /// <summary>
        /// Modifica una stanza
        /// </summary>
        /// <param name="User">L'utente per cui creare la stanza</param>
        /// <returns>true = updated</returns>
        public override bool RoomUpdate(WbRoom Room)
        {
            if (Room == null || Room.Parameter == null || Room.Parameter.GetType() != typeof(oMRoomParameters)) throw new ArgumentException("Parameter must be eWRoomParameters!");

            oMRoomParameters Param = (oMRoomParameters)Room.Parameter;

            // SESSIONE DI LAVORO
            OMuserService.Sessiondata oResponse = UserService.getSession();
            String SessionID = oResponse.session_id;
            OMuserService.ErrorResult oError = null;

            // LOGIN DI UN ADMINISTRATOR
            long UserID = UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd);



            //Param.isModeratedRoom = false;
            //Param. = 1;
            //Param.redirectURL = "www.ecosia.org";

            Room.Parameter = Param;

            WbRoom OldRoom = DAL.RoomUpdate(Room);

            //OMP_Roomtypes_id
            //OMP_appointment = false;
            //OMP_isDemoRoom = false;
            //OMP_demoTime = 120;
            try
            {
                RoomService.updateRoomWithModerationAndQuestions(
                SessionID,
                Room.Id,
                Room.Name,
                OMP_Roomtypes_id,
                Room.Description,
                Param.numberOfPartizipants,
                Room.Public,
                OMP_appointment,
                OMP_isDemoRoom,
                OMP_demoTime,
                Param.isModeratedRoom,
                Param.allowUserQuestions
                );
            }
            catch { return true; }


            //RoomService.updateRoom(
            //    SessionID,
            //    Room.Id,
            //    Room.Name,
            //    OM_Roomtypes_id,
            //    Room.Description,
            //    Param.numberOfPartizipants,
            //    Room.Public,)

            return false;
        }

        /// <summary>
        /// Recupera l'elenco stanze per un dato utente
        /// </summary>
        /// <param name="CommunityId">Id Comunità</param>
        /// <param name="UserId">Id utente</param>
        /// <param name="Filter">
        ///     User        required
        ///     Community   required
        ///     Other parameter:
        ///     Time range      available in time range
        ///     Invitation      le sessioni a cui l'utente è stato invitato
        ///     Open            le sessioni disponibili a tutti
        ///     ...
        /// </param>
        /// <returns></returns>
        public override IList<WbRoom> RoomsGet(Boolean IsForAdmin, int CommunityId, Int32 UserId, DTO.DTO_RoomListFilter filters, int PageIndex, int PageSize, ref int PageCount)
        {
            return DAL.RoomsGet(IsForAdmin, CommunityId, UserId, filters, PageIndex, PageSize, ref PageCount);
        }

        /// <summary>
        /// Cancella una stanza
        /// </summary>
        /// <param name="RoomId">ID della stanza da cancellare</param>
        /// <returns>True se cancellata</returns>
        public override bool RoomDelete(long RoomId)
        {
            Boolean notError = true;

            WbRoom Room = DAL.RoomGet(RoomId);

            // SESSIONE DI LAVORO
            OMuserService.Sessiondata oResponse = UserService.getSession();
            String SessionID = oResponse.session_id;
            OMuserService.ErrorResult oError = null;

            // LOGIN DI UN ADMINISTRATOR
            long UserID = UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd);

            long ExtRoomId = -1;
            try
            {
                ExtRoomId = System.Convert.ToInt64(Room.ExternalId);
            }
            catch { }


            if (ExtRoomId > 0)
            {
                try
                {
                    RoomService.deleteRoom(SessionID, ExtRoomId);
                }
                catch { notError = false; }

                if (!notError)  //Errore cancellazione. Controllo se la chiave esiste. In tal caso provo comunque a cancellarla da dB.
                {
                    OMroomService.Rooms rm = null;
                    try
                    {
                        rm = RoomService.getRoomById(SessionID, ExtRoomId);
                    }
                    catch
                    {
                        //DA TESTARE!!!
                    }

                    if (rm == null)
                    {
                        notError = true;
                    }
                }
            }

            if (notError)
            {
                notError &= DAL.RoomDelete(Room);
            }

            return notError;
        }

        /// <summary>
        /// Recupera stanza con tutti i dati
        /// </summary>
        /// <param name="RoomId">Id della stanza</param>
        /// <returns>Oggetto WbRoom</returns>
        public override WbRoom RoomGet(long RoomId)
        {
            //' LOGIN DI UN ADMINISTRATOR
            //UserID = oUserService.loginUser(SessionID, Me.SystemSettings.OpenMeetingService.Login, Me.SystemSettings.OpenMeetingService.Password)
            WbRoom Room = DAL.RoomGet(RoomId);
            if (Room != null)
            {
                // SESSIONE DI LAVORO
                OMuserService.Sessiondata oResponse = UserService.getSession();
                String SessionID = oResponse.session_id;
                OMuserService.ErrorResult oError = null;

                // LOGIN DI UN ADMINISTRATOR
                long UserID = UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd);

                oMRoomParameters roomParams = new oMRoomParameters();
                long OMroomId = 0;
                OMroomService.Rooms omRoom = new OMroomService.Rooms();

                try
                {
                    OMroomId = System.Convert.ToInt64(Room.ExternalId);
                    omRoom = RoomService.getRoomById(SessionID, OMroomId);
                }
                catch { return null; }

                if (omRoom != null)
                {
                    roomParams.allowUserQuestions = omRoom.allowUserQuestions ?? true;
                    roomParams.isModeratedRoom = omRoom.isModeratedRoom ?? false;

                    roomParams.ispublic = omRoom.ispublic ?? false;
                    roomParams.numberOfPartizipants = omRoom.numberOfPartizipants ?? 10;

                    roomParams.redirectURL = omRoom.redirectURL ?? "";
                    roomParams.validFromDate = omRoom.starttime ?? DateTime.Now;

                    DateTime EndDate = Room.StartDate ?? roomParams.validFromDate;

                    if (Room.EndDate == null)
                        EndDate = EndDate.AddMinutes(Room.Duration);

                    roomParams.validToDate = Room.EndDate ?? EndDate;

                    Room.Parameter = roomParams;
                }
            }

            return Room;
        }

        /// <summary>
        /// Aggiorna i dati di una stanza
        /// </summary>
        /// <param name="RoomId">ID stanza</param>
        /// <param name="Data">Dati stanza (generici)</param>
        /// <param name="Parameters">Parametri stanza (dati avanzati, basati su integrazione)</param>
        /// <returns>La stanza con i dati aggiornati</returns>
        public override WbRoom RoomUpdateData(long RoomId, DTO.DTO_GenericRoomData Data, WbRoomParameter Parameters, bool HasIdInName)
        {

            if (HasIdInName)
            {
                Data.Name = string.Format("({0}) {1}", RoomId.ToString(), Data.Name);
            }

            Data.HasIdInName = HasIdInName;

            String ExternalId = DAL.RoomGetExternaId(RoomId);
            WbRoom UpdatedRoom = DAL.RoomUpdate(RoomId, Data);


            try
            {
                oMRoomParameters Param = (oMRoomParameters)Parameters;

                // SESSIONE DI LAVORO
                OMuserService.Sessiondata oResponse = UserService.getSession();
                String SessionID = oResponse.session_id;
                OMuserService.ErrorResult oError = null;

                // LOGIN DI UN ADMINISTRATOR
                long UserID = UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd);

                long ExtRoomId = System.Convert.ToInt64(ExternalId);

                RoomService.updateRoomWithModerationAndQuestions(
                SessionID,
                ExtRoomId,
                UpdatedRoom.Name,
                OMP_Roomtypes_id,
                UpdatedRoom.Description,
                Param.numberOfPartizipants,
                UpdatedRoom.Public,
                OMP_appointment,
                OMP_isDemoRoom,
                OMP_demoTime,
                Param.isModeratedRoom,
                Param.allowUserQuestions
                );

            }
            catch (Exception ex)
            {
                UpdatedRoom.Parameter = null;

            }

            return UpdatedRoom;
        }


        //public void UpdateRoomRecording()
        //{
        //    IList<WbRoom> Rooms = DAL.RoomsGetAll();

        //    foreach (WbRoom Room in Rooms)
        //    {
        //        Room.Recording = false;
        //        DAL.RoomUpdate(Room);
        //    }

        //    //return false;
        //}
    #endregion

    #region User management

        /// <summary>
        /// Aggiunge un utente
        /// </summary>
        /// <param name="Users">Dati utente</param>
        /// <param name="RoomId">IdStanza</param>        
        public override void UsersAdd(IList<WbUser> Users, long RoomId)
        {
            WbRoom oRoom = DAL.RoomGet(RoomId);

            if (Users != null && Users.Count() > 0)
            {
                foreach (WbUser usr in Users)
                {
                    if (usr.Id == 0 || String.IsNullOrEmpty(usr.ExternalID))
                    {
                        usr.ExternalID = Guid.NewGuid().ToString();
                    }

                    usr.RoomId = oRoom.Id;
                    usr.ExternalRoomId = oRoom.ExternalId;

                    DAL.UserSaveOrUpdate(usr);
                }
            }

            DAL.RoomUpdateUserNumber(RoomId);
        }

        /// <summary>
        /// Recupera gli iscritti ad una stanza
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>Lista di utente iscritti alla stanza</returns>
        public override IList<WbUser> UsersGet(Int64 RoomId, DTO.DTO_UserListFilters Filters, int PageIndex, int PageSize, ref int PageCount)
        {
            return DAL.UsersGet(RoomId, Filters, PageIndex, PageSize, ref PageCount);
        }

        /// <summary>
        /// Abilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: utente abilitato
        /// False: errore, utente non abilitato
        /// </returns>
        public override bool UserEnable(long UserId, long RoomId)
        {
            bool NeedMail = true;   //Intanto sempre, poi qui false ed attivo in base a configurazione stanza.

            WbUser User = DAL.UserGet(UserId);

            if (User.MailChecked == false && User.Enabled == false)
            {
                NeedMail = true;
            }

            if (User != null)
            {
                User.Enabled = true;

                if (String.IsNullOrEmpty(User.ExternalID))
                    this.UserAddToExternalSystem(ref User);
            }

            User.MailChecked = true;

            DAL.UserSaveOrUpdate(User);

            if (!NeedMail)
            {
                WbRoom oRoom = DAL.RoomGet(RoomId);
                NeedMail = oRoom.NotificationEnableUsr;
            }

            return NeedMail;
        }

        /// <summary>
        /// Disabilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: utente abilitato
        /// False: errore interno, utente non abilitato
        /// </returns>
        public override bool UserDisable(int UserId, long RoomId)
        {

            WbUser User = DAL.UserGet(UserId);

            // SESSIONE DI LAVORO
            OMuserService.Sessiondata oResponse = UserService.getSession();
            String SessionID = oResponse.session_id;

            // LOGIN DI UN ADMINISTRATOR
            long UserID = UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd);

            if (User != null)
            {
                User.Enabled = false;
                UserService.kickUserByPublicSID(SessionID, User.ExternalID);
            }
            DAL.UserSaveOrUpdate(User);


            WbRoom oRoom = DAL.RoomGet(RoomId);
            bool NeedMail = oRoom.NotificationDisableUsr;
            return NeedMail;
        }

        /// <summary>
        /// Cancella utente dalla stanza
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: utente cancellato
        /// False: errore interno, utente non cancellato
        /// </returns>
        public override bool UserDelete(int UserId, long RoomId)
        {
            String RoomKey = DAL.RoomGetExternaId(RoomId);
            WbUser User = DAL.UserGet(UserId);

            if (User != null)
            {

                // SESSIONE DI LAVORO
                OMuserService.Sessiondata oResponse = UserService.getSession();
                String SessionID = oResponse.session_id;
                
                // LOGIN DI UN ADMINISTRATOR
                long UserID = UserService.loginUser(SessionID, this.oMSysParameter.MainUserLogin, this.oMSysParameter.MainUserPwd);

                long ExternalUserID = 0;
                try
                {
                    ExternalUserID = System.Convert.ToInt64(User.ExternalID);
                } catch {}

                if(ExternalUserID > 0)
                    UserService.deleteUserById(SessionID, ExternalUserID);

                DAL.UserDelete(User.Id);
            }

            DAL.RoomUpdateUserNumber(RoomId);
            return true;
        }

        /// <summary>
        /// Aggiorna dati utente
        /// </summary>
        /// <param name="Users">Lista dati utente</param>
        /// <param name="RoomId">Stanza</param>
        /// <returns>
        /// True:   utenti aggiornati
        /// False:  errore aggiornamento
        /// </returns>
        public override bool UsersUpdate(IList<WbUser> Users, long RoomId)
        {
            foreach (WbUser usr in Users)
            {
                if (usr.Id > 0)
                {

                    WbUser CurUsr = DAL.UserGetInRoom(usr.Id, RoomId);  //UserGetInRoomByPerson(usr.PersonID, RoomId);

                    if (CurUsr == null)
                    {
                        //L'utente NON trovato o non di quella stanza!
                        CurUsr = new WbUser();
                        CurUsr.Mail = usr.Mail;
                    }

                    //CurUsr.DisplayName = usr.DisplayName;
                    CurUsr.Name = usr.Name;
                    CurUsr.SName = usr.SName;

                    CurUsr.IsHost = usr.IsHost;
                    CurUsr.IsController = usr.IsController;
                    CurUsr.Video = usr.Video;
                    CurUsr.Audio = usr.Audio;
                    CurUsr.Chat = usr.Chat;

                    DAL.UserSaveOrUpdate(CurUsr);
                    
                }
            }
            DAL.RoomUpdateUserNumber(RoomId);
            return true;
        }

        /// <summary>
        /// Aggiunge un utente GIA' in COMOL al sistema esterno e restituisce il relativo ID.
        /// Se stringa vuota, l'utente non è stato inserito!
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public override void UserAddToExternalSystem(ref WbUser User)
        {
            User.ExternalID = Guid.NewGuid().ToString();
            User.Enabled = true;
        }
    
    #endregion


        public override void RoomRecordingUpdate()
        {
            throw new NotImplementedException();
        }

        public override void UserUpateInternal(long UserId, string Name, string SName, string Mail)
        {
            throw new NotImplementedException();
        }

        public override bool RoomNameExternalUpdate(string RoomExternalId, string NewName)
        {
            throw new NotImplementedException("Function to Update name with Room Id.");
        }
    }
}