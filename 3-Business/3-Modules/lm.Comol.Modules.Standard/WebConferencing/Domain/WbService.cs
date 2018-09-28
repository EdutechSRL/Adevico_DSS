using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// Contiene i metodi standard di accesso ai Webinars.
    /// Molte delle funzioni possono/saranno implementate internamente, come potenzialmente gli inviti (da valutare),
    /// magari con implementazione di default e possibilità di override,
    /// oppure con un manager a sè stante da usare per l'implementazione.
    /// Altre, invece, andranno implementate per ogni singola implementazione.
    /// In questo senso, tale interfaccia può diventare una classe base, ma che necessita di implementazioni.
    /// </summary>
    public abstract class WbService : lm.Comol.Core.Business.BaseCoreServices
    {
    #region Costruttori
        public WbService(WbSystemParameter SysParameter, iApplicationContext oContext) : base( oContext)
        {
            if (SysParameter == null) throw new ArgumentNullException("SysParameter cannot be null!");
            SysParameter = this.SysParameter;
        }

        public WbService(WbSystemParameter SysParameter, iDataContext oDC)
            : base(oDC)
        {
            if (SysParameter == null) throw new ArgumentNullException("SysParameter cannot be null!");
            SysParameter = this.SysParameter;
        }
    #endregion

    #region "Permission - Implemented"
        #region Public

        /// <summary>
        /// Get Service Module Id
        /// </summary>
        /// <returns>Service Module Id</returns>
        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(ModuleWebConferencing.UniqueCode);
        }

        /// <summary>
        /// Get Specific Service Permission
        /// </summary>
        /// <param name="personId">
        /// Specific Person ID
        /// </param>
        /// <param name="idCommunity">
        /// Specific Community ID
        /// </param>
        /// <returns>ModuleDocTemplate permission</returns>
        public ModuleWebConferencing GetServicePermission(int personId, int idCommunity)
        {
            Person person = Manager.GetPerson(personId);
            return GetServicePermission(person, idCommunity);
        }

        /// <summary>
        /// Get Specific Service Permission
        /// </summary>
        /// <param name="person">
        /// Specific Person
        /// </param>
        /// <param name="idCommunity">
        /// Specific Community ID
        /// </param>
        /// <returns>ModuleDocTemplate permission</returns>
        public ModuleWebConferencing GetServicePermission(Person person, int idCommunity)
        {
            ModuleWebConferencing module = new ModuleWebConferencing();
            if (person == null)
                person = (from p in Manager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
            if (idCommunity == 0)
                module = ModuleWebConferencing.CreatePortalmodule((person == null) ? (int)UserTypeStandard.Guest : person.TypeID);
            else
                module = new ModuleWebConferencing(Manager.GetModulePermission(person.Id, idCommunity, ServiceModuleID()));
            return module;
        }

        #endregion
        #region Internal

        private int _CurrentPersonId;
        /// <summary>
        /// Get Current UserId
        /// </summary>
        private int CurrentPersonId
        {
            get
            {
                if (_CurrentPersonId == null || _CurrentPersonId == 0)
                {
                    _CurrentPersonId = base.UC.CurrentUserID;
                }
                return _CurrentPersonId;
            }
        }

        private ModuleWebConferencing _Permission;
        /// <summary>
        /// Get ModuleDocTemplate Permission for current User and Current Community
        /// </summary>
        private ModuleWebConferencing Permission
        {
            get
            {
                if (_Permission == null)
                {
                    _Permission = this.GetServicePermission(this.UC.CurrentUserID, this.UC.CurrentCommunityID);
                }
                return _Permission;
            }
        }

        private Person _CU;
        /// <summary>
        /// Utente corrente
        /// </summary>
        public Person CurrentUser
        {
            get
            {
                if (_CU == null)
                {
                    _CU = Manager.GetPerson(CurrentPersonId);
                }
                return _CU;
            }
        }

    #endregion
    #endregion

    #region System/Generics

        #region Implemented
        
        /// <summary>
        /// Data Access Layer
        /// </summary>
        protected DAL.WbGenericDAL DAL;

        /// <summary>
        /// Da una lista di Id Lingua, recupera i relativi DTO con nome e codice
        /// </summary>
        /// <param name="LanguagesId">Lista di Id Lingua</param>
        /// <returns>Lista di DTO con Codice, nome ed icona lingua</returns>
        public IList<Domain.DTO.DTO_Language> LanguagesGet(IList<Int32> LanguagesId)
        {
            return DAL.LanguagesGet(LanguagesId);
        }

        /// <summary>
        /// Parametri di sistema (configurazione)
        /// </summary>
        public WbSystemParameter SysParameter;

        #endregion

        #region Abstract
        
        /// <summary>
        /// Controlla lo stato del server
        /// </summary>
        /// <returns>
        /// True: server funziona
        /// False: server non raggiungibile o errore server (controllare configurazione e stato server)
        /// </returns>
        public abstract Boolean ServerCheck();

        /// <summary>
        /// Recupera i dati avanzati di una stanza in base al tipo della stessa
        /// </summary>
        /// <param name="Type">Tipo di stanza</param>
        /// <returns>Dati avanzati TIPIZZATI sul sistema in uso</returns>
        public abstract WbRoomParameter ParameterGetByType(RoomType Type);

        /// <summary>
        /// Controlla la possibilità di aggiungere una mail ad una determinata stanza
        /// </summary>
        /// <param name="Mail">Mail da aggiungere</param>
        /// <param name="RoomKey">Chiave stanza</param>
        /// <returns></returns>
        public abstract MailCheck MailServiceCheck(String Mail, String RoomKey);

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
        public abstract String AccessUrlExternalGet(Int64 RoomId, Int64 UserId);
    
        #endregion

    #endregion

    #region Room management

        #region Implemented

        /// <summary>
        /// Usato in creazione per definire i parametri standard dato un tipo di stanza
        /// </summary>
        /// <param name="RoomType">Tipo di stanza</param>
        /// <returns>DTO con dati generici</returns>
        public DTO.DTO_GenericRoomData RoomGetGenericData(RoomType RoomType)
        {
            Domain.DTO.DTO_GenericRoomData GenRD = new Domain.DTO.DTO_GenericRoomData();
            GenRD.Description = "";
            GenRD.Duration = 0;
            GenRD.StartDate = DateTime.Now;
            switch (RoomType)
            {
                case Domain.RoomType.VideoChat:
                    GenRD.MaxAllowUsers = 2;
                    GenRD.Public = false;
                    GenRD.NotificationDisableUsr = true;
                    GenRD.NotificationEnableUsr = true;
                    break;
                case Domain.RoomType.Meeting:
                    GenRD.Public = false;
                    GenRD.MaxAllowUsers = 5;
                    GenRD.NotificationDisableUsr = true;
                    GenRD.NotificationEnableUsr = true;
                    break;
                case Domain.RoomType.Lesson:
                    GenRD.Public = false;
                    GenRD.MaxAllowUsers = 0;
                    GenRD.NotificationDisableUsr = false;
                    GenRD.NotificationEnableUsr = false;
                    break;
                case Domain.RoomType.Conference:
                    GenRD.Public = true;
                    GenRD.MaxAllowUsers = 0;
                    GenRD.NotificationDisableUsr = false;
                    GenRD.NotificationEnableUsr = false;
                    break;
                default:    //Domain.RoomType.Advance
                    GenRD.Public = false;
                    GenRD.MaxAllowUsers = 0;
                    GenRD.NotificationDisableUsr = false;
                    GenRD.NotificationEnableUsr = false;
                    break;
            }
            return GenRD;
        }

        #endregion

        #region Abstract

        /// <summary>
        /// Crea una stanza
        /// </summary>
        /// <returns>Id della nuova stanza</returns>
        public abstract Int64 RoomCreate(WbRoom Room, bool SysHasIdInName);

        /// <summary>
        /// Modifica una stanza
        /// </summary>
        /// <param name="User">L'utente per cui creare la stanza</param>
        /// <returns>true = updated</returns>
        public abstract Boolean RoomUpdate(WbRoom Room);

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
        /// <remarks>
        /// MUST BE UPDATE Pager.Count !!!
        /// </remarks>
        public abstract IList<WbRoom> RoomsGet(Boolean IsForAdmin, Int32 CommunityId, Int32 UserId, DTO.DTO_RoomListFilter filters, int PageIndex, int PageSize, ref int PageCount);

        /// <summary>
        /// Cancella una stanza
        /// </summary>
        /// <param name="RoomId">ID della stanza da cancellare</param>
        /// <returns>True se cancellata</returns>
        public abstract bool RoomDelete(Int64 RoomId);

        /// <summary>
        /// Recupera stanza con tutti i dati
        /// </summary>
        /// <param name="RoomId">Id della stanza</param>
        /// <returns>Oggetto WbRoom</returns>
        public abstract WbRoom RoomGet(Int64 RoomId);

        /// <summary>
        /// Aggiorna i dati di una stanza
        /// </summary>
        /// <param name="RoomId">ID stanza</param>
        /// <param name="Data">Dati stanza (generici)</param>
        /// <param name="Parameters">Parametri stanza (dati avanzati, basati su integrazione)</param>
        /// <returns>La stanza con i dati aggiornati</returns>
        public abstract WbRoom RoomUpdateData(Int64 RoomId, Domain.DTO.DTO_GenericRoomData Data, Domain.WbRoomParameter Parameters, bool HasIdInName);

        /// <summary>
        /// Aggiorna il campo "Record" in tutte le stanze in piattaforma.
        /// </summary>
        public abstract void RoomRecordingUpdate();
        #endregion

#endregion

        //public void RoomUpdateTemplate(Int64 RoomId, Int64 TemplateId)
        //{
        //    DAL.RoomTemplateUpdate(RoomId, TemplateId);
        //}

    #region Room Code (Implemented)
        /// <summary>
        /// Dato un ID restituisce l'eventuale codice di accesso esterno di una stanza
        /// </summary>
        /// <param name="RoomId">L'ID della stanza</param>
        /// <returns></returns>
        public String RoomCodeGet(Int64 RoomId)
        {
            return DAL.RoomCodeGet(RoomId);
        }

        /// <summary>
        /// Recupera i dati di una chiave stanza
        /// </summary>
        /// <param name="UrlCode">Codice url</param>
        /// <returns>Oggetto con Id e codice stanza</returns>
        public Domain.WbAccessCode RoomCodeDataGet(String UrlCode)
        {
            return DAL.CodeGet(UrlCode);
        }

        /// <summary>
        /// Genera un codice per la stanza. Questo và a sostituire l'eventuale precedente
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        public void RoomCodeGenerate(Int64 RoomId)
        {
            DAL.RoomCodeGenerate(RoomId);
        }

        /// <summary>
        /// Cancella l'eventuale codice di una stanza. NON sarà possibile l'accesso esterno.
        /// </summary>
        /// <param name="RoomId">ID Stanza</param>
        public void RoomCodeDelete(Int64 RoomId)
        {
            DAL.RoomCodeDelete(RoomId);
        }
    #endregion

    #region User management

        #region abstract

        /// <summary>
        /// Aggiunge un utente
        /// </summary>
        /// <param name="Users">Dati utente</param>
        /// <param name="RoomId">IdStanza</param>        
        public abstract void UsersAdd(IList<WbUser> Users, Int64 RoomId);

        /// <summary>
        /// Recupera gli iscritti ad una stanza
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>Lista di utente iscritti alla stanza</returns>
        public abstract IList<WbUser> UsersGet(Int64 RoomId, DTO.DTO_UserListFilters Filters, int PageIndex, int PageSize, ref int PageCount);

        /// <summary>
        /// Abilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: INVIA MAIL!
        /// False: NON inviare MAIL!
        /// </returns>
        public abstract bool UserEnable(Int64 UserId, Int64 RoomId);

        /// <summary>
        /// Disabilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: utente abilitato
        /// False: errore interno, utente non abilitato
        /// </returns>
        public abstract bool UserDisable(Int32 UserId, Int64 RoomId);

        /// <summary>
        /// Cancella utente dalla stanza
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: utente cancellato
        /// False: errore interno, utente non cancellato
        /// </returns>
        public abstract bool UserDelete(Int32 UserId, Int64 RoomId);

        /// <summary>
        /// Aggiorna dati utente
        /// </summary>
        /// <param name="Users">Lista dati utente</param>
        /// <param name="RoomId">Stanza</param>
        /// <returns>
        /// True:   utenti aggiornati
        /// False:  errore aggiornamento
        /// </returns>
        public abstract bool UsersUpdate(IList<WbUser> Users, Int64 RoomId);

        /// <summary>
        /// Aggiunge un utente GIA' in COMOL al sistema esterno e restituisce il relativo ID.
        /// Se stringa vuota, l'utente non è stato inserito!
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public abstract void UserAddToExternalSystem(ref WbUser User);

        /// <summary>
        /// Aggiorna TUTTI gli utenti di piattaforma con i dati reali
        /// </summary>
        public void UsersUpdateSystem()
        {
            IList<WbUser> Users = Manager.GetAll<WbUser>(u => u.PersonID > 0).ToList();

            if (Users != null && Users.Count() > 0)
            {
                foreach (WbUser usr in Users)
                {
                    Person Prsn = Manager.GetPerson(usr.PersonID);
                    UserUpateInternal(usr.Id, Prsn.Name, Prsn.Surname, Prsn.Mail);
                }
            }
        }

        /// <summary>
        /// Aggiorna gli utenti DI UNA SPECIFICA COMUNITA' con i dati reali
        /// </summary>
        /// <param name="CommunityId"></param>
        public void UsersUpdateCommunity(int CommunityId)
        {
            IList<Int64> RoomsIds = (from WbRoom room in Manager.GetIQ<WbRoom>()
                                     where room.CommunityId == CommunityId
                                     select room.Id).ToList();

            if (RoomsIds == null || RoomsIds.Count() <= 0)
                return;

            IList<WbUser> Users = Manager.GetAll<WbUser>(u => u.PersonID > 0 && RoomsIds.Contains(u.RoomId)).ToList();

            if (Users != null && Users.Count() > 0)
            { 
                foreach (WbUser usr in Users)
                {
                    Person Prsn = Manager.GetPerson(usr.PersonID);
                    UserUpateInternal(usr.Id, Prsn.Name, Prsn.Surname, Prsn.Mail);
                }
            }
        }

        /// <summary>
        /// Aggiorna i campi di un SINGOLO UTENTE
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Name"></param>
        /// <param name="SName"></param>
        /// <param name="Mail"></param>
        public abstract void UserUpateInternal(Int64 UserId, String Name, String SName, String Mail);
        #endregion

        #region implemented

        /// <summary>
        /// Rigenera i codici utente
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UsersId">Elenco ID utenti</param>
        /// <param name="RegenerateAll">
        /// False = genera SOLO per gli utenti "vuoti"
        /// True = genera per TUTTI gli utenti
        /// </param>
        /// <returns>Il numero di codici generati</returns>
        public int GenerateUsersCode(Int64 RoomId, IList<Int64> UsersId, Boolean RegenerateAll)
        {
            return DAL.UserKeysGenerate(RoomId, UsersId, RegenerateAll);
        }

        /// <summary>
        /// Genera il codice per un utente già iscritto alla stanza
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UserId">Id utente stanza</param>
        public void GenerateUserCode(Int64 RoomId, Int64 UserId)
        {
            DAL.UserKeyGenerate(RoomId, UserId);
        }

        /// <summary>
        /// Recupera un utente iscritto ad una stanza, partendo dalla mail
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="mail">eMail utente</param>
        /// <returns>Oggetto con i dati dell'utente</returns>
        public WbUser GetUserFromMail(Int64 RoomId, String mail)
        {
            return DAL.UserGet(RoomId, mail);
        }

        /// <summary>
        /// Aggiunge un utente ad una stanza, dato un ID Persona
        /// </summary>
        /// <param name="RoomId">ID Stanza</param>
        /// <param name="PersonId">ID Persona</param>
        /// <returns>I dati dell'utente creato</returns>
        public WbUser GetUserFromSystem(Int64 RoomId, Int32 PersonId)
        {
            return DAL.UserGetInRoomByPerson(PersonId, RoomId);
        }

        /// <summary>
        /// Iscrive un nuovo utente esterno.
        /// </summary>
        /// <param name="RoomCode">Codice stanza</param>
        /// <param name="Name">Nome utente</param>
        /// <param name="SecondName">Cognome utente</param>
        /// <param name="Mail">eMail utente</param>
        /// <param name="LangCode">Codice lingue</param>
        /// <param name="ErrorString">Eventuale segnalazione errore</param>
        /// <returns>Oggetto utente con tutti i dati appena inseriti</returns>
        public WbUser UserSubscribeNewExternal(
            String RoomCode,
            String Name, String SecondName, String Mail,
            String LangCode,
            ref Domain.ExtSubscriptionStatus ErrorString)
        {
            //Controllo eistenza codice
            Domain.WbAccessCode Code = this.DAL.CodeGet(RoomCode);
            if (Code == null || Code.RoomId <= 0) // || Code.UserId >= 0)
            {
                ErrorString = ExtSubscriptionStatus.ParametersError;    //"Error.InvalidCode";
                return null;
            }

            //Controllo esistenza stanza
            Domain.WbRoom Room = this.DAL.RoomGet(Code.RoomId);
            if (Room == null || Room.Id <= 0)
            {
                ErrorString = ExtSubscriptionStatus.ParametersError;
                return null;
            }

            //Controllo permessi iscrizione
            if (Room.SubExternal == SubscriptionType.Free)
                ErrorString = ExtSubscriptionStatus.MailSended;
            else if (Room.SubExternal == SubscriptionType.Moderated)
                ErrorString = ExtSubscriptionStatus.RequestSended;
            else
            {
                ErrorString = ExtSubscriptionStatus.NoPermission;
                return null;
            }

            Domain.WbUser User = new Domain.WbUser();

            //User.DisplayName = Name + " " + SecondName;
            User.Name = Name;
            User.SName = SecondName;

            User.Mail = Mail;
            User.MailChecked = false;
            User.RoomId = Room.Id;
            User.LanguageCode = LangCode;
            //tipo dei parametri di Room per definire se Audio, video, etc.. (SOLO eWORKS)
            User.Audio = false;
            User.Video = false;
            User.Enabled = true;
            //User.ExternalID = ???
            //User.ExternalRoomId
            User.IsController = false;
            User.IsHost = false;
            User.PersonID = -1;
            User.SendedInvitation = 0;
            User.ShowMail = true;
            User.ShowStatus = true;

            //MOLTO IMPORTANTE l'EXTERNAL ID!!!
            User.ExternalRoomId = Room.ExternalId;

            DAL.UserSaveOrUpdate(User);

            if (User.Id <= 0)
            {
                ErrorString = ExtSubscriptionStatus.InternalError;
                return null;
            }

            String UserKey = Domain.CodeHelper.GenerateCode(User.Id);
            User.UserKey = UserKey;

            DAL.UserSaveOrUpdate(User);
            DAL.RoomUpdateUserNumber(Room.Id);

            return User;
        }

        /// <summary>
        /// Valida la mail di un utente esterno
        /// </summary>
        /// <param name="UserID">ID Utente</param>
        /// <returns>
        /// True: utente aggiornato
        /// False: errore aggiornamento
        /// </returns>
        public bool UserExternalValidate(Int64 UserID)
        {
            WbUser usr = DAL.UserGet(UserID);

            usr.MailChecked = true;

            if (String.IsNullOrEmpty(usr.ExternalID))
            {
                this.UserAddToExternalSystem(ref usr);
            }

            if (String.IsNullOrEmpty(usr.ExternalID))
            {
                return false;
            }

            DAL.UserSaveOrUpdate(usr);
            return true;
        }

        /// <summary>
        /// Aggiunge una lista di utenti ad una stanza
        /// </summary>
        /// <param name="RoomId">ID Stanza</param>
        /// <param name="Users">Lista di utenti da aggiungere</param>
        /// <returns>Lista di utenti che non sono stati aggiunti per vari motivi</returns>
        public IList<Domain.DTO.DTO_ExtUser> UsersExternalsAdd(Int64 RoomId, IList<Domain.DTO.DTO_ExtUser> Users)
        {
            IList<Domain.DTO.DTO_ExtUser> ErrorUsers = new List<Domain.DTO.DTO_ExtUser>();
            IList<Domain.WbUser> AddUsers = new List<Domain.WbUser>();

            foreach (Domain.DTO.DTO_ExtUser usr in Users)
            {
                Domain.MailCheck MailInSystem = DAL.MailDoCheck(usr.Mail, RoomId);

                Boolean Error = true;

                if (usr.MailCheckFormat())
                {
                    switch (MailInSystem)
                    {
                        case MailCheck.MailInRoom:
                            usr.InsertError = ErrorAddExtUser.MailInRoom;
                            break;
                        case MailCheck.MailInRoomdB:
                            usr.InsertError = ErrorAddExtUser.MailInRoom;
                            break;
                        case MailCheck.MailInSystem:
                            usr.InsertError = ErrorAddExtUser.MailInSystem;
                            break;
                        case MailCheck.ParameterError:
                            usr.InsertError = ErrorAddExtUser.InternalError;
                            break;
                        case MailCheck.MailUnknow:
                            WbUser WbUsr = usr.ToWbUser(true);
                            if (WbUsr != null)
                            {
                                Error = false;
                                AddUsers.Add(WbUsr);
                            }
                            else
                                usr.InsertError = ErrorAddExtUser.ParameterError;
                            break;
                    }
                }
                else
                {
                    usr.InsertError = ErrorAddExtUser.MailFormatError;
                }

                if (Error)
                    ErrorUsers.Add(usr);
            }

            if (AddUsers != null && AddUsers.Count() > 0)
            {
                this.UsersAdd(AddUsers, RoomId);
            }

            return ErrorUsers;
        }

        /// <summary>
        /// Recupera i dati di un utente
        /// </summary>
        /// <param name="UserId">ID UTENTE (deve essere nella stanza)</param>
        /// <returns>Oggetto con i dati dell'utente</returns>
        public WbUser UserGet(Int64 UserId)
        {
            return DAL.UserGet(UserId);
        }

        /// <summary>
        /// Aggiunge un utente alla stanza da un PersonId
        /// </summary>
        /// <param name="PersonId">ID Persona</param>
        /// <param name="RoomId">ID Stanza</param>
        /// <param name="Enabled">Se l'utente sarà abilitato ad accedere o meno</param>
        /// <returns>
        /// True: se inserito correttamente
        /// False: errori di inserimento
        /// </returns>
        public bool UserPersonAdd(int PersonId, long RoomId, Boolean Enabled)
        {

            WbUser User = DAL.UserGetForRoomByPerson(PersonId, RoomId);

            if (User != null)
            {
                //Boh! Rivedere!
                //User = DAL.GetUserForRoomByPerson(PersonId, RoomId);
                //User.Audio = Param.isAudioOnly;
                //User.Chat = !Param.hideChat;
                User.IsController = false;
                User.IsHost = false;
                User.Enabled = Enabled;
                //User.Video = !Param.isAudioOnly;

                //DAL.UserSaveOrUpdate(User);
                List<WbUser> Users = new List<WbUser>();

                Users.Add(User);

                this.UsersAdd(Users, RoomId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Aggiunge utenti in una stanza da una lista di ID Persona
        /// </summary>
        /// <param name="PersonsIds">Lista ID Person</param>
        /// <param name="RoomId">ID stanza</param>
        /// <param name="Audio">Se attivare l'audio</param>
        /// <param name="Video">Se attivare l'video</param>
        /// <param name="Chat">Se attivare l'chat</param>
        /// <param name="Admin">Se l'utente avrà particolari privilegi nella stanza (a seconda delle implementazioni)</param>
        public void UserPersonAddIds(IList<Int32> PersonsIds, Int64 RoomId, bool Audio, bool Video, bool Chat, bool Host, bool Controller)
        {
            if (PersonsIds != null && PersonsIds.Count() > 0)
            {
                IList<WbUser> Users = DAL.UsersGetForRoomByPersons(PersonsIds, RoomId, Audio, Video, Chat, Host, Controller);
                this.UsersAdd(Users, RoomId);
            }
        }

        /// <summary>
        /// Controllo mail prima dell'inserimento
        /// </summary>
        /// <param name="Mail">Mail da controllare</param>
        /// <param name="RoomID">ID stanza in cui potenzialmente andrà a finire</param>
        /// <returns>Enum sullo stato del controllo</returns>
        public MailCheck UserMailCheck(String Mail, Int64 RoomID)
        {

            WbRoom Room = DAL.RoomGet(RoomID);

            MailCheck mc = this.MailServiceCheck(Mail, Room.ExternalId);

            if (mc != MailCheck.MailUnknow)
                return mc;

            //Nella stanza o sistema
            return DAL.MailDoCheck(Mail, RoomID);
        }

        /// <summary>
        /// Controlla se una data persona è iscritta alla comunità
        /// </summary>
        /// <param name="RoomId">Id stanza</param>
        /// <param name="PersonId">Id persona</param>
        /// <returns>
        /// True:   se è iscritta alla comuntà
        /// False:  se non è iscritta alla comunità
        /// </returns>
        /// <remarks>
        /// RIVEDERE stato attivazione!
        /// </remarks>
        public bool UserPersonIsInCommunity(Int64 RoomId, int PersonId)
        {
            WbRoom oRoom = DAL.RoomGet(RoomId);

            if (oRoom == null || oRoom.Id <= 0 || oRoom.CommunityId <= 0)
                return false;

            Subscription oSubs = Manager.GetSubscription(PersonId, oRoom.CommunityId);

            // CONTROLLARE QUI:
            if (oSubs == null || oSubs.Id <= 0 || !oSubs.Enabled)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Restituisce la lista degli stati relativi alla validazione o meno della mail degli utenti
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="removeUsers"></param>
        /// <returns></returns>
        public List<MailStatus> GetAvailableMailStatus(long idRoom, UserTypeFilter userType, UserStatus status,List<long> removeUsers)
        {
            return DAL.GetAvailableMailStatus(idRoom,userType,status ,removeUsers);
        }
        /// <summary>
        /// Restituisce la lista degli stati relativi all'utente
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="removeUsers"></param>
        /// <returns></returns>
        public List<UserStatus> GetAvailableUserStatus(long idRoom, UserTypeFilter userType,List<long> removeUsers)
        {
            return DAL.GetAvailableUserStatus(idRoom, userType,removeUsers);
        }

        /// <summary>
        /// Restituisce la lista delle tipologie di utente
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="removeUsers"></param>
        /// <returns></returns>
        public List<UserTypeFilter> GetAvailableUserTypes(ModuleObject obj, List<long> removeUsers, lm.Comol.Core.Mail.Messages.MailMessagesService service)
        {
            return DAL.GetAvailableUserTypes(obj, removeUsers, service);
        }

        /// <summary>
        /// Mi dice se trova utenti con una determinata tipologia di profilo
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="idProfileType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Boolean HasUsersWithProfileType(long idRoom, int idProfileType,UserTypeFilter type, UserStatus status) {
            return DAL.HasUsersWithProfileType(idRoom, idProfileType,type, status);
        }

        /// <summary>
        /// Data una stanza, in base al filtro sul tipo di utente e sullo status di attivazione recupera la lista degli eventuali enti associati
        /// </summary>
        /// <param name="idRoom">Identificativo stanza</param>
        /// <param name="type">Tipo di utente</param>
        /// <param name="status">Status iscrizione alla stanza</param>
        /// <returns></returns>
        public Dictionary<long, String> GetAgenciesForUsers(long idRoom,UserTypeFilter type, UserStatus status)
        {
            return DAL.GetAgenciesForUsers(idRoom,type, status);
        }

        /// <summary>
        /// Restituisce una lista filtrata di utenti che possono ricevere dei messaggi di notifica
        /// </summary>
        /// <param name="pService">Servizio gestione utenti</param>
        /// <param name="recipients">lòista degli utenti completa ed oggetto del filtro e del riordino</param>
        /// <param name="filter">filtri da applicare</param>
        /// <param name="loadAllInfo">definisce se le informazioni relative a campi internazionalizzati o enti devono o meno essere caricate in toto</param>
        /// <returns></returns>
        public List<dtoWebConferenceMessageRecipient> GetUsersForMessages(lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService, List<dtoWebConferenceMessageRecipient> recipients, dtoUsersByMessageFilter filter, List<TranslatedItem<Int32>> profileTypes, List<TranslatedItem<Int32>> roles, Boolean loadAllInfo = true)
        {
            return DAL.GetUsersForMessages(pService, recipients, filter,profileTypes,roles, loadAllInfo);
        }

        /// <summary>
        /// REcupera la lista dei possibili destinatari di un messaggio relativo al servizio di vide conferenza
        /// </summary>
        /// <param name="unknownUser"></param>
        /// <param name="anonymousUser"></param>
        /// <param name="obj">oggetto proprietario (la stanza)</param>
        /// <param name="removeUsers"></param>
        /// <param name="filter">filtri da applicare</param>
        /// <param name="service"></param>
        /// <param name="pService"></param>
        /// <returns></returns>
        public List<dtoWebConferenceMessageRecipient> GetAvailableUsersForMessages(String unknownUser, String anonymousUser, ModuleObject obj, List<long> removeUsers, dtoUsersByMessageFilter filter, lm.Comol.Core.Mail.Messages.MailMessagesService service, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService) {
            return DAL.GetAvailableUsersForMessages(unknownUser, anonymousUser, obj, removeUsers, filter, service, pService);
        }
        public List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> GetSelectedUsers(ModuleObject obj, List<long> removeUsers, dtoUsersByMessageFilter filter, Boolean selectAll, List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> sItems, lm.Comol.Core.Mail.Messages.MailMessagesService service, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService)
        {
            List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> items = new List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient>();
            try
            {
                List<dtoWebConferenceMessageRecipient> recipients = GetAvailableUsersForMessages("","",obj, removeUsers, filter, service, pService);
                recipients = ParseMessageRecipients(pService, recipients, filter);
                if (selectAll)
                    items = recipients.Select(r => (lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient)r).ToList();
                else
                    items = recipients.Where(r => sItems.Where(s => s.IsEqualsTo(r)).Any()).Select(r => (lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient)r).ToList();
            }
            catch (Exception ex)
            {

            }
            return items;
        }
        private List<dtoWebConferenceMessageRecipient> ParseMessageRecipients(lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService, List<dtoWebConferenceMessageRecipient> recipients, dtoUsersByMessageFilter filter)
        {
            List<dtoWebConferenceMessageRecipient> items = new List<dtoWebConferenceMessageRecipient>();
            try
            {
                var query = (from r in recipients where (filter.IdProfileType <= 0 || filter.IdProfileType == r.IdProfileType) && (filter.IdRole == r.IdRole || filter.IdRole == -1) select r);

                if (!string.IsNullOrEmpty(filter.Value) && string.IsNullOrEmpty(filter.Value.Trim()) == false)
                {
                    switch (filter.SearchBy)
                    {
                        case Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains:
                            List<String> values = filter.Value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                            if (values.Any() && values.Count == 1)
                                query = query.Where(r =>!String.IsNullOrEmpty(r.DisplayName) && r.DisplayName.ToLower().Contains(filter.Value.ToLower()));
                            else if (values.Any() && values.Count > 1)
                                query = query.Where(r => (!String.IsNullOrEmpty(r.Name) && values.Any(r.Name.ToLower().Contains)) || (!String.IsNullOrEmpty(r.Surname) && values.Any(r.Surname.ToLower().Contains)) || values.Any(r.MailAddress.ToLower().Contains) || values.Any(r.DisplayName.ToLower().Contains));
                            break;
                        case Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail:
                            query = query.Where(r => r.MailAddress.ToLower().Contains(filter.Value.ToLower()));
                            break;
                        case Core.BaseModules.ProfileManagement.SearchProfilesBy.Name:
                            query = query.Where(r => r.Name.ToLower().StartsWith(filter.Value.ToLower()));
                            break;
                        case Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname:
                            query = query.Where(r => r.Surname.ToLower().StartsWith(filter.Value.ToLower()));
                            break;
                    }
                }
                if ((filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.Name || filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.All || filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains || string.IsNullOrEmpty(filter.Value)) && !string.IsNullOrEmpty(filter.StartWith))
                {
                    if (filter.StartWith != "#")
                        query = query.Where(r => r.FirstLetter == filter.StartWith.ToLower());
                    else
                        query = query.Where(r => pService.DefaultOtherChars().Contains(r.FirstLetter));
                }
                if (filter.IdAgency == -3)
                    query = query.Where(r => r.IdProfileType != (int)UserTypeStandard.Employee && (filter.IdProfileType <= 0 || filter.IdProfileType == r.IdProfileType));
                else
                {
                    Dictionary<long, List<Int32>> agencyInfos = pService.GetUsersWithAgencies(query.Where(r => r.IsInternal).Select(r => r.IdPerson).ToList().Distinct().ToList());
                    if (filter.IdAgency == -2)
                        query = query.Where(r => r.IdProfileType == (int)UserTypeStandard.Employee);
                    else if (agencyInfos.ContainsKey(filter.IdAgency))
                        query = query.Where(r => r.IdProfileType == (int)UserTypeStandard.Employee && agencyInfos[filter.IdAgency].Contains(r.IdPerson));
                    else if (filter.IdAgency > 0)
                        query = query.Where(r => 1 == 2);
                }
                items = query.ToList();
            }
            catch (Exception ex)
            {

            }
            return items;
        }


        public lm.Comol.Core.Mail.dtoRecipient UserGetRecipient(Int64 UserId)
        {
            WbUser User = this.UserGet(UserId);

            if (User == null || User.Id <= 0)
                return null;

            lm.Comol.Core.Mail.dtoRecipient recipient = new Core.Mail.dtoRecipient();

            if (User.PersonID > 0)
            {
                Person prsn = Manager.Get<Person>(User.PersonID);

                if (prsn != null)
                {
                    recipient.DisplayName = prsn.SurnameAndName;
                    recipient.MailAddress = prsn.Mail;
                }
            }

            if(String.IsNullOrEmpty(recipient.DisplayName))
            {
                recipient.DisplayName = User.DisplayName;
                recipient.MailAddress = User.Mail;
            }

            return recipient;
        }
#endregion
    #endregion

        #region "Sending Messages"
            public List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> GetMessagesToSend(long idRoom, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations, String websiteUrl,String webSiteUrlNoSsl, Dictionary<String, Dictionary<PlaceHoldersType, String>> emptyItems, Dictionary<String, String> datetimeFormats, String portalName)
            {
                List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> messages = new List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage>();
                if (translations.Any())
                {
                    WbRoom room = Manager.Get<WbRoom>(idRoom);
                    if (room != null)
                    {
                        String roomCode = DAL.RoomCodeGet(idRoom);
                        String cName = (room.CommunityId > 0) ? DAL.GetCommunityName(room.CommunityId) : portalName;
                        messages = translations.Select(t => new lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage()
                        {
                            IdLanguage = t.IdLanguage,
                            CodeLanguage = t.LanguageCode,
                            Subject = (!String.IsNullOrEmpty(t.Translation.Subject) && t.Translation.Subject.Contains("[") && t.Translation.Subject.Contains("]")) ? AnalyzeContent(room, roomCode, cName, null, t.Translation.IsHtml, t.Translation.Subject, websiteUrl,webSiteUrlNoSsl, emptyItems[t.LanguageCode], datetimeFormats[t.LanguageCode], t.LanguageCode ) : t.Translation.Subject,
                            Body = (!String.IsNullOrEmpty(t.Translation.Body) && t.Translation.Body.Contains("[") && t.Translation.Body.Contains("]")) ? AnalyzeContent(room, roomCode, cName, null, t.Translation.IsHtml, t.Translation.Body, websiteUrl, webSiteUrlNoSsl, emptyItems[t.LanguageCode], datetimeFormats[t.LanguageCode], t.LanguageCode) : t.Translation.Body
                        }).ToList();
                    }
                }
                return messages;
            }
            public List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> GetMessagesToSend(long idRoom, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translations, List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> recipients, String websiteUrl, String webSiteUrlNoSsl, Dictionary<String, Dictionary<PlaceHoldersType, String>> emptyItems, Dictionary<String, String> datetimeFormats, String portalName)
            {
                List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> messages = new List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage>();
                if (recipients.Any())
                {
                    WbRoom room = Manager.Get<WbRoom>(idRoom);
                    if (room != null)
                    {
                        Language l = Manager.GetDefaultLanguage();
                        foreach (lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient recipient in recipients) //.Where(r => r.IsFromModule))
                        {
                            lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation content = translations.Where(t => t.IdLanguage == recipient.IdLanguage && t.LanguageCode == recipient.CodeLanguage).FirstOrDefault();
                            if (content == null)
                                content = translations.Where(t => t.LanguageCode == "multi").FirstOrDefault();
                            if (content == null)
                                content = translations.Where(t => t.IdLanguage == l.Id).FirstOrDefault();
                    //        //Boolean hasSubmissionLink = (content != null && content.Translation.Body.Contains(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.SubmissionUrl)));
                            List<WbUser> users = (from s in Manager.GetIQ<WbUser>()
                                                        where s.Deleted == BaseStatusDeleted.None && s.RoomId == idRoom &&
                                                          s.Id == recipient.IdUserModule && (recipient.IdModuleObject == 0 || recipient.IdModuleObject == s.Id)
                                                        select s).ToList();
                            String roomCode = DAL.RoomCodeGet(idRoom);
                            String cName = (room.CommunityId>0 ) ? DAL.GetCommunityName(room.CommunityId) : portalName;
                            String displayName = "";
                            if (content != null) {
                                if (users.Any())
                                {
                                    foreach (WbUser u in users)
                                    {
                                        Person p = (u.PersonID > 0) ? Manager.GetPerson(u.PersonID) : null;
                                        displayName = (p == null) ? "" : p.SurnameAndName;
                                        messages.Add(new lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage(recipient)
                                        {
                                            IdLanguage = content.IdLanguage,
                                            CodeLanguage = content.LanguageCode,
                                            Subject = (!String.IsNullOrEmpty(content.Translation.Subject) && content.Translation.Subject.Contains("[") && content.Translation.Subject.Contains("]")) ? AnalyzeContent(room, roomCode, cName, u, content.Translation.IsHtml, content.Translation.Subject, websiteUrl, webSiteUrlNoSsl, emptyItems[content.LanguageCode], datetimeFormats[content.LanguageCode], content.LanguageCode, displayName) : content.Translation.Subject,
                                            Body = (!String.IsNullOrEmpty(content.Translation.Body) && content.Translation.Body.Contains("[") && content.Translation.Body.Contains("]")) ? AnalyzeContent(room, roomCode, cName, u, content.Translation.IsHtml, content.Translation.Body, websiteUrl, webSiteUrlNoSsl, emptyItems[content.LanguageCode], datetimeFormats[content.LanguageCode], content.LanguageCode, displayName) : content.Translation.Body,
                                        });
                                    }
                                }
                                else if (recipient.IdUserModule == 0) {
                                    messages.Add(new lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage(recipient)
                                    {
                                        IdLanguage = content.IdLanguage,
                                        CodeLanguage = content.LanguageCode,
                                        Subject = (!String.IsNullOrEmpty(content.Translation.Subject) && content.Translation.Subject.Contains("[") && content.Translation.Subject.Contains("]")) ? AnalyzeContent(room, roomCode, cName, null, content.Translation.IsHtml, content.Translation.Subject, websiteUrl, webSiteUrlNoSsl, emptyItems[content.LanguageCode], datetimeFormats[content.LanguageCode], content.LanguageCode, displayName) : content.Translation.Subject,
                                        Body = (!String.IsNullOrEmpty(content.Translation.Body) && content.Translation.Body.Contains("[") && content.Translation.Body.Contains("]")) ? AnalyzeContent(room, roomCode, cName, null, content.Translation.IsHtml, content.Translation.Body, websiteUrl, webSiteUrlNoSsl, emptyItems[content.LanguageCode], datetimeFormats[content.LanguageCode], content.LanguageCode, displayName) : content.Translation.Body,
                                    });
                                }
                            }
                        }
                    //    //if (recipients.Where(r => !r.IsInternal).Any()) {
                    //    //    messages.AddRange(translations.Select(t => new lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage()
                    //    //    {
                    //    //        IdLanguage = t.IdLanguage,
                    //    //        CodeLanguage = t.LanguageCode,
                    //    //        Subject = (!String.IsNullOrEmpty(t.Translation.Subject) && t.Translation.Subject.Contains("[") && t.Translation.Subject.Contains("]")) ? AnalyzeContent(call, t.Translation.IsHtml, t.Translation.Subject, null, null, null, websiteUrl, disclaimerTranslation, yesOrNoTranslation) : t.Translation.Subject,
                    //    //        Body = (!String.IsNullOrEmpty(t.Translation.Body) && t.Translation.Body.Contains("[") && t.Translation.Body.Contains("]")) ? AnalyzeContent(call, t.Translation.IsHtml, t.Translation.Body, null, null, null, websiteUrl, disclaimerTranslation, yesOrNoTranslation) : t.Translation.Body,
                    //    //        RemovedRecipients = recipients.Where(r => !r.IsInternal && r.IdLanguage== t.IdLanguage && r.CodeLanguage==t.LanguageCode).ToList()
                    //    //    }).ToList().Where(r=>r.RemovedRecipients.Any()).ToList());
                    //    //}
                    }
                }
                return messages;
            }

        #endregion
        #region TagReplacer

        /// <summary>
        /// Recupera la stanza e l'utente
        /// </summary>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <param name="isHtml"></param>
        /// <param name="idRoom"></param>
        /// <param name="DestUserId"></param>
        /// <param name="WcBaseUrl"></param>
        /// <param name="content"></param>
        /// <param name="fakeSubmitter"></param>
        /// <returns></returns>
        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTemplateContentPreview(
            Boolean isHtml,
            long idRoom,
            long DestUserId,
            String BaseUrl,
            String  webSiteUrlNoSsl,
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content,
            string DateTimeFormat, string VoidDateTime, string PortalName)
        {
            WbRoom oRoom = DAL.RoomGet(idRoom);
            WbUser usr = DAL.UserGet(DestUserId);

            //Se non ho una chiave utente, la genero.
            if (String.IsNullOrEmpty(usr.UserKey))
            {
                this.GenerateUserCode(idRoom, DestUserId);
            }



            DTO.DTO_RoomTagData dtoRoom;

            if (oRoom != null && usr != null)
            {
                dtoRoom = new DTO.DTO_RoomTagData();
                dtoRoom.Code = DAL.RoomCodeGet(idRoom);
                dtoRoom.CommunityName = DAL.GetCommunityName(oRoom.CommunityId);
                if (String.IsNullOrEmpty(dtoRoom.CommunityName))
                    dtoRoom.CommunityName = PortalName;

                dtoRoom.CreatedBy = oRoom.CreatedBy;
                dtoRoom.CreatedOn = oRoom.CreatedOn;
                dtoRoom.Description = oRoom.Description;

                dtoRoom.FinishOn = oRoom.EndDate;
                dtoRoom.Name = oRoom.Name;
                dtoRoom.StartOn = oRoom.StartDate;

                return GetTemplateContentPreview(
                   isHtml,
                   dtoRoom, DestUserId,
                   BaseUrl,webSiteUrlNoSsl, content,
                   DateTimeFormat, VoidDateTime);
            }
            return content;
        }

        /// <summary>
        /// Ad una data stanza, aggiunge l'utente.
        /// </summary>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <param name="isHtml"></param>
        /// <param name="Room">Si presume di avere già TUTTI i dati della stanza, compreso il nome comunità o portale.</param>
        /// <param name="DestUserId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTemplateContentPreview(
            Boolean isHtml,
            DTO.DTO_RoomTagData Room,
            long DestUserId,
            string BaseUrl,
            string webSiteUrlNoSsl,
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content,
            string DateTimeFormat, string VoidDateTime)
        {

            WbUser usr = DAL.UserGet(DestUserId);

            if (Room != null && usr != null)
            {
                Room.DestUser = new DTO.DTO_UserTagData();
                Room.DestUser.AccessKey = usr.UserKey;
                //Room.DestUser.FullName = usr.DisplayName;
                Room.DestUser.Name = usr.Name;
                Room.DestUser.SName = usr.SName;

                Room.DestUser.LanguageCode = usr.LanguageCode;
                Room.DestUser.Mail = usr.Mail;

                ////////content = GetTemplateContentPreview(false,
                ////////    openTag, closeTag,
                ////////    isHtml, content,
                ////////    oRoom, Users,
                ////////    fakeSubmitter,
                ////////    currentUser,
                ////////    disclaimerTranslation, yesOrNoTranslation);

                return GetTemplateContentPreview(
                    isHtml,
                    Room, BaseUrl,webSiteUrlNoSsl, content,
                    DateTimeFormat, VoidDateTime);
            }

            return content;
        }

        /// <summary>
        /// Utilizza esclusivamente i parametri passati
        /// </summary>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <param name="isHtml"></param>
        /// <param name="Room">Si presume di avere già TUTTI i dati della stanza, compreso il nome comunità o portale e l'utente.</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation GetTemplateContentPreview(
            Boolean isHtml,
            DTO.DTO_RoomTagData Room,
            string BaseUrl,
            string webSiteUrlNoSsl,
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content,
            string DateTimeFormat, string VoidDateTime)
        {
            if (Room != null && Room.DestUser != null)
            {
                //Alterazione content
                if (!String.IsNullOrEmpty(content.Body) && content.Body.Contains(TemplatePlaceHolders.OpenTag) && content.Body.Contains(TemplatePlaceHolders.CloseTag))
                {
                    content.Body = AnalyzeContent(
                        isHtml,
                        content.Body,
                        Room,
                        DateTimeFormat, VoidDateTime,
                        BaseUrl, webSiteUrlNoSsl);
                }

                //if (!String.IsNullOrEmpty(content.Name) && content.Name.Contains(openTag) && content.Name.Contains(closeTag))
                //{

                //}

                if (!String.IsNullOrEmpty(content.ShortText) && 
                    content.ShortText.Contains(TemplatePlaceHolders.OpenTag) && 
                    content.ShortText.Contains(TemplatePlaceHolders.CloseTag))
                {
                    content.Body = AnalyzeContent(
                        isHtml,
                        content.ShortText,
                        Room,
                        DateTimeFormat, VoidDateTime,
                        BaseUrl, webSiteUrlNoSsl);
                }

                if (!String.IsNullOrEmpty(content.Subject) && 
                    content.Subject.Contains(TemplatePlaceHolders.OpenTag) && 
                    content.Subject.Contains(TemplatePlaceHolders.CloseTag))
                {
                    content.Subject = AnalyzeContent(
                        isHtml,
                        content.Subject,
                        Room,
                        DateTimeFormat, VoidDateTime,
                        BaseUrl, webSiteUrlNoSsl);
                }
            }

            return content;
        }

        private String AnalyzeContent(
            Boolean isHtml,
            String content,
            DTO.DTO_RoomTagData Room,
            string DateTimeFormat, string VoidDateTime,
            string BaseUrl, String webSiteUrlNoSsl)
        {

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomName), Room.Name);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomDescription), Room.Description);

            if (Room.StartOn != null)
            {
                DateTime Start = Room.StartOn ?? DateTime.Now;
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomStartDate), Start.ToString(DateTimeFormat));
            }
            else
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomStartDate), VoidDateTime);
            }
            if (Room.FinishOn != null)
            {
                DateTime End = Room.FinishOn ?? DateTime.Now;
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomEndDate), End.ToString(DateTimeFormat));
            }
            else
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomEndDate), VoidDateTime);
            }


            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomUrl), webSiteUrlNoSsl + Room.AccessUrl);

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserKey), Room.DestUser.AccessKey);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserDisplayName), Room.DestUser.FullName);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserMail), Room.DestUser.Mail);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCommunity), Room.CommunityName);
            //content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomDuration), Room.);

            if (Room.CreatedOn != null)
            {
                DateTime create = Room.CreatedOn ?? DateTime.Now;
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCreateDate), create.ToString(DateTimeFormat));
            }
            else
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCreateDate), VoidDateTime);

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCode), Room.Code);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCreatedBy), ( Room.CreatedBy== null) ? "--" : Room.CreatedBy.SurnameAndName);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserLanguageCode), Room.DestUser.LanguageCode);
            return content;
        }

        private String AnalyzeContent(WbRoom room, String roomCode, String cName, WbUser user, Boolean isHtml, String content, String webSiteUrl, String webSiteUrlNoSsl, Dictionary<PlaceHoldersType, String> emptyItems, String dateTimeFormat, String lCode, String displayName = "")
        {

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomName), room.Name);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomDescription), room.Description);

            if (room.StartDate.HasValue)
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomStartDate), room.StartDate.Value.ToString(dateTimeFormat));
            else
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomStartDate), emptyItems[PlaceHoldersType.RoomStartDate]);
            if (room.EndDate.HasValue)
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomEndDate), room.EndDate.Value.ToString(dateTimeFormat));
            else
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomEndDate), emptyItems[PlaceHoldersType.RoomEndDate]);


            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomUrl), webSiteUrlNoSsl + RootObject.ExternalAccess(roomCode));
            if (user == null)
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserKey), emptyItems[PlaceHoldersType.UserKey]);
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserDisplayName), emptyItems[PlaceHoldersType.UserDisplayName]);
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserMail), emptyItems[PlaceHoldersType.UserMail]);
            }
            else
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserKey), user.UserKey);
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserDisplayName), (String.IsNullOrEmpty(displayName) ? user.SName : displayName));
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserMail), user.Mail);
            }
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserLanguageCode), lCode);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCommunity), (String.IsNullOrEmpty(cName) && room.CommunityId>0) ? emptyItems[ PlaceHoldersType.RoomCommunity]: cName );
            //content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomDuration), Room.);

            if (room.CreatedOn.HasValue)
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCreateDate), room.CreatedOn.Value.ToString(dateTimeFormat));
            else
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCreateDate), emptyItems[PlaceHoldersType.RoomCreateDate]);

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCode), roomCode);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.RoomCreatedBy), (room.CreatedBy == null) ? emptyItems[PlaceHoldersType.RoomCreatedBy] : room.CreatedBy.SurnameAndName);
            return content;
        }

    #endregion

        #region ID nel nome
        public int RoomNameUpdateALL(bool hasIdInName, int skip, int take)
        {
            int changed = 0;

            var ormList = Manager.GetIQ<WbRoom>()
                .Where(r => r.hasIdInName != hasIdInName);

            IList<WbRoom> roomsList;
            
            if(take > 0)
                roomsList = ormList.Skip(skip).Take(take).ToList();
            else
                roomsList = ormList.ToList();

            if (!roomsList.Any())
                return 0;

            if(Manager.IsInTransaction())
                Manager.Commit();

            foreach (WbRoom room in roomsList)
            {
                string OldName = room.Name;
                Manager.BeginTransaction();

                String idInName = string.Format("({0}) ", room.Id.ToString());
                
                //IMPOSTO l'id dentro il nome
                if (hasIdInName)
                {
                    room.Name = idInName + room.Name;
                }
                else
                {
                    room.Name = room.Name.Remove(0, idInName.Count());
                }

                room.hasIdInName = hasIdInName;

                bool success = this.RoomNameExternalUpdate(room.ExternalId, room.Name);

                //bool successdB = false;

                if (success)
                {
                    try
                    {
                        Manager.SaveOrUpdate<WbRoom>(room);
                        Manager.Commit();

                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
                //else
                //{
                //    room.Name = OldName;
                //    room.hasIdInName = !hasIdInName;
                //}

                //Nel caso in cui o il service esterno o il dB non riesca a salvare il nuovo nome, eseguo un rollback, sia su dB che su server esterno.
                if (!success)
                {
                    Manager.RollBack();
                    this.RoomNameExternalUpdate(room.ExternalId, OldName);
                    //room.Name = OldName;
                    //room.hasIdInName = !hasIdInName;
                    Manager.Detach(room);
                }
                else
                {
                    changed++;    
                }

            }

            //Manager.DetachList(roomsList);
            
            return changed;
        }

        public bool RoomNameUpdateNameWithId(Int64 RoomId)
        {
            //int changed = 0;

            WbRoom room = Manager.Get<WbRoom>(RoomId);

            if (room == null)
            {
                return false;
            }

            if (Manager.IsInTransaction())
                Manager.Commit();

            string OldName = room.Name;
                Manager.BeginTransaction();

                String idInName = string.Format("({0}) ", room.Id.ToString());

                //IMPOSTO l'id dentro il nome
                room.Name = idInName + room.Name;
                //}
                //else
                //{
                //    room.Name = room.Name.Remove(0, idInName.Count());
                //}

                room.hasIdInName = true;

                bool success = this.RoomNameExternalUpdate(room.ExternalId, room.Name);

                //bool successdB = false;

                if (success)
                {
                    try
                    {
                        Manager.SaveOrUpdate<WbRoom>(room);
                        Manager.Commit();

                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
                //else
                //{
                //    room.Name = OldName;
                //    room.hasIdInName = !hasIdInName;
                //}

                //Nel caso in cui o il service esterno o il dB non riesca a salvare il nuovo nome, eseguo un rollback, sia su dB che su server esterno.
                if (!success)
                {
                    Manager.RollBack();
                    this.RoomNameExternalUpdate(room.ExternalId, OldName);
                    //room.Name = OldName;
                    //room.hasIdInName = !hasIdInName;
                    Manager.Detach(room);
                    return false;
                }
            

            //Manager.DetachList(roomsList);

            return true;
        }

        /// <summary>
        /// Aggiorna il nome di una stanza (uso interno)
        /// </summary>
        /// <returns>true se è stato modificato, altrimenti false</returns>
        public abstract bool RoomNameExternalUpdate(String RoomExternalId, String NewName);

        #endregion
    }
}
