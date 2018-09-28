using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks
{
    
    public partial class eWService : WbService
    {
        
    #region Properties
        private lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService ServiceTemplate;
        private eWSystemParameter eWSysParameter
        {
            get
            {
                return (eWSystemParameter)base.SysParameter;
            }
        }

    #endregion
       
    #region Costruttori

        public eWService(eWSystemParameter SysParameter, iApplicationContext oContext)
            : base(SysParameter, oContext)
        {
            if (SysParameter.GetType() != typeof(eWSystemParameter)) throw new ArgumentException("Wrong SysParameter Type. Must be eWSystemParameter!");

            base.SysParameter = SysParameter;
            ServiceTemplate = new Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
            this.DAL = new DAL.WbGenericDAL(oContext);
        }

        public eWService(eWSystemParameter SysParameter, iDataContext oDC)
            : base(SysParameter, oDC)
        {
            if (SysParameter.GetType() != typeof(eWSystemParameter)) throw new ArgumentException("Wrong SysParameter Type. Must be eWSystemParameter!");
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
            Int32 Size = 0;
            try
            {
                Size = eWAPIConnector.GetMaxFileSize(
               this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
               this.eWSysParameter.MainUserId,
               this.eWSysParameter.MainUserPwd);
            }
            catch
            { return false; }

            if (Size != -9) return true;
            return false;
        }

        /// <summary>
        /// Recupera i dati avanzati di una stanza in base al tipo della stessa
        /// </summary>
        /// <param name="Type">Tipo di stanza</param>
        /// <returns>Dati avanzati TIPIZZATI sul sistema in uso</returns>
        public override WbRoomParameter ParameterGetByType(RoomType Type)
        {
            return eWAPIConnector.GetParameterByType(Type);
        }

        /// <summary>
        /// Controlla la possibilità di aggiungere una mail ad una determinata stanza
        /// </summary>
        /// <param name="Mail">Mail da aggiungere</param>
        /// <param name="RoomKey">Chiave stanza</param>
        /// <returns></returns>
        public override MailCheck MailServiceCheck(string Mail, String RoomKey)
        {
            if (String.IsNullOrEmpty(Mail) || String.IsNullOrEmpty(RoomKey))
            {
                return MailCheck.ParameterError;
            }


            String Key = eWAPIConnector.RetrieveKey(
                this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                this.eWSysParameter.MainUserId,
                this.eWSysParameter.MainUserPwd,
                RoomKey, Mail
                );

            if (!String.IsNullOrEmpty(Key))
                return MailCheck.MailInRoom;

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
            //Controllo l'External Id dell'utente.
            String ExternalId = DAL.UserGetExternalIdFromUser(UserId);

            if (!String.IsNullOrEmpty(ExternalId))
                return eWurlHelper.GetAccessUrl(
                this.eWSysParameter.BaseUrl,
                this.eWSysParameter.MainUserId,
                ExternalId,
                this.eWSysParameter.Version
                );

            return "";
        }

    #endregion

    #region Room management

        /// <summary>
        /// Crea una stanza
        /// </summary>
        /// <returns>Id della nuova stanza</returns>
        public override Int64 RoomCreate(WbRoom Room, bool SysHasIdInName)
        {
            if (Room == null || Room.Parameter == null || Room.Parameter.GetType() != typeof(eWRoomParameters)) throw new ArgumentException("Parameter must be eWRoomParameters!");

            eWRoomParameters Param = (eWRoomParameters)Room.Parameter;

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
            

            Room.ExternalId = eWAPIConnector.CreateMasterKey(
                this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                this.eWSysParameter.MainUserId,
                this.eWSysParameter.MainUserPwd,
                this.eWSysParameter.MainUserId,
                "Comol",
                Room.StartDate,
                Room.Name,
                Room.Duration);

            //Necessario!
            //Altrimenti il SET dei parametri "ARA" la data di partenza...
            Param.meetingstart = Room.StartDate;
            Param.meetingduration = Room.Duration;

            //allinea la descrizione "comol" con quella "eworks"
            Param.description = Room.Description;

            eWAPIConnector.SetParameters(
                this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                this.eWSysParameter.MainUserId,
                this.eWSysParameter.MainUserPwd,
                Room.ExternalId,
                Param,
                1500, true, this.eWSysParameter.Version);

            Room.Recording = Param.recording;

            this.DAL.RoomCreate(Room);

            //Genero anche il codice stanza alla creazione.
            this.RoomCodeGenerate(Room.Id);

            if (SysHasIdInName)
                RoomNameUpdateNameWithId(Room.Id);
            
            return Room.Id;
        }

        /// <summary>
        /// Modifica una stanza
        /// </summary>
        /// <param name="User">L'utente per cui creare la stanza</param>
        /// <returns>true = updated</returns>
        /// <remarks>
        /// SE viene impostata data di inizio e fine, viene ricalcolata la Duration
        /// SE viene impostata data di inizio e durata, ma non la data di fine, viene calcolata la data di fine
        /// </remarks>
        public override Boolean RoomUpdate(WbRoom Room)
        {
            if (Room == null)
            {
                throw new ArgumentNullException("Room", "Cannot be null.");
            }

            if (Room.Parameter.GetType() != typeof(eWRoomParameters))
            {
                throw new ArgumentException("Room paramter must be eWRoomParameters", "Room.Parameter");
            }

            eWRoomParameters param = new eWRoomParameters();
            
            try
            {
                param = (eWRoomParameters)Room.Parameter;
            } catch 
            {
                param = null;
            }
            
            if(param != null)
            {
                Room.Recording = param.recording;

                WbRoom OldRoom = DAL.RoomUpdate(Room);

                try
                {

                    eWAPIConnector.SetParameters(
                        this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                        this.eWSysParameter.MainUserId, this.eWSysParameter.MainUserPwd,
                        OldRoom.ExternalId, param, this.eWSysParameter.MaxUrlChars, false, this.eWSysParameter.Version);
                }
                catch (Exception ex)
                {
                    OldRoom.Id = -2;
                }

                if (OldRoom.Id > 0)
                    return true;
            }
            
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
        public override IList<WbRoom> RoomsGet(Boolean IsForAdmin, int CommunityId, Int32 UserId, Domain.DTO.DTO_RoomListFilter filters, int PageIndex, int PageSize, ref int PageCount)
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
            WbRoom Room = DAL.RoomGet(RoomId);
            Boolean notError = true;
            try
            {
                eWAPIConnector.DeleteKey(
                    this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                    this.eWSysParameter.MainUserId,
                    this.eWSysParameter.MainUserPwd,
                    Room.ExternalId, Room.ExternalId);
            }
            catch { notError = false; }

            if (!notError)
            {
                //Ci sono stati errori.
                //Potenzialmente la chiave potrebbe non esistere sul server...
                try
                {
                    DTO.DTOKeyInfo ki = eWAPIConnector.GetKeyInfo(this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                    this.eWSysParameter.MainUserId,
                    this.eWSysParameter.MainUserPwd,
                    Room.ExternalId);

                    notError = false;
                }
                catch
                {
                    //ERROR: La chiave non esiste. Verrà cancellata da dB.
                    notError = true;
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
            WbRoom Room = DAL.RoomGet(RoomId);
            if (Room != null)
            {
                Room.Parameter = eWAPIConnector.GetRoomParameters(
                    this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                    this.eWSysParameter.MainUserId,
                    this.eWSysParameter.MainUserPwd,
                    Room.ExternalId);
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
        public override WbRoom RoomUpdateData(
            long RoomId, Domain.DTO.DTO_GenericRoomData Data,
            WbRoomParameter Parameters, bool HasIdInName)
        {
            //this.eWSysParameter.

            if (HasIdInName)
            {
                Data.Name = string.Format("({0}) {1}", RoomId.ToString(), Data.Name);
            }

            Data.HasIdInName = HasIdInName;


            String ExternalId = DAL.RoomGetExternaId(RoomId);
            WbRoom UpdatedRoom = new WbRoom();

            if (Parameters == null)
            {
                WbRoom OldRoom = this.RoomGet(RoomId);
                if (OldRoom.Parameter != null)
                    Parameters = OldRoom.Parameter;
                else
                {
                    UpdatedRoom = DAL.RoomUpdate(RoomId, Data);
                    return UpdatedRoom;
                }
            }



            try
            {
                eWRoomParameters param = (eWRoomParameters)Parameters;

                Data.Recording = param.recording;

                UpdatedRoom = DAL.RoomUpdate(RoomId, Data);

                

                param.meetingtitle = Data.Name;
                param.description = Data.Description;
                param.meetingstart = Data.StartDate;
                if (Data.Duration > 0)
                {
                    param.meetingduration = Data.Duration;
                }
                else
                {
                    if (Data.StartDate != null && Data.EndDate != null)
                    {
                        TimeSpan TS = (Data.EndDate ?? DateTime.Now) - (Data.StartDate ?? DateTime.Now);
                        try
                        {
                            param.meetingduration = Convert.ToInt32(TS.TotalMinutes);
                        }
                        catch
                        {
                            param.meetingduration = 0;
                        }
                    }
                }

                eWAPIConnector.SetParameters(
                    this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                    this.eWSysParameter.MainUserId, this.eWSysParameter.MainUserPwd,
                    ExternalId, param, this.eWSysParameter.MaxUrlChars, false, this.eWSysParameter.Version);
            }
            catch (Exception ex)
            {
                UpdatedRoom.Parameter = null;
                
            }

            return UpdatedRoom;
        }

        //public void UpdateRoomRecording()
        //{

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
            String MasterKey = oRoom.ExternalId;

            IList<DTO.DTOuser> EWusers =
                eWAPIConnector.RetrieveUsers(
                this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                this.eWSysParameter.MainUserId,
                this.eWSysParameter.MainUserPwd,
                MasterKey
                );

            //IList<String> CurrentKeys = (from DTO.DTOuser usr in EWusers select usr.Key).ToList();
            //IList<String> NewKeys = (from WbUser usr in Users select usr.ExternalID).ToList();

            

            
            foreach (WbUser usr in Users)
            {
                Boolean usrExistEW = false;
                

                Boolean error = false;

                Boolean usrExistdB = false;
                WbUser dBUser = DAL.UserGet(RoomId, usr.Mail);
                if (dBUser != null)
                {
                    usrExistdB = true;
                }

                usr.RoomId = oRoom.Id;
                usr.ExternalRoomId = oRoom.ExternalId;


                String EWkey = (from DTO.DTOuser ewu in EWusers where ewu.UserId == usr.Mail select ewu.Key).FirstOrDefault();

                if (String.IsNullOrEmpty(EWkey))
                    usrExistEW = false;
                else 
                    usrExistEW = true;


                // C'è sul dB, ma non in eWorks!!!
                //   Cancello da dB!!!
                if (usrExistdB & !usrExistEW)
                {
                    DAL.UserDelete(oRoom.Id, usr.Mail);
                    usrExistdB = false;
                    usrExistEW = false;
                } 
                else if (!usrExistdB & usrExistEW)
                {
                    
                    eWAPIConnector.DeleteKey(
                            this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                                this.eWSysParameter.MainUserId,
                                this.eWSysParameter.MainUserPwd,
                                oRoom.ExternalId,
                                EWkey
                                );

                    usrExistdB = false;
                    usrExistEW = false;
                }

                if (!usrExistdB && !usrExistEW)
                {
                    //Aggiungo l'utente.
                    //Aggiungo l'utente su eWorks
                    try
                    {
                        usr.ExternalID = eWAPIConnector.CreateKey(
                        this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                        this.eWSysParameter.MainUserId,
                        this.eWSysParameter.MainUserPwd,
                        MasterKey,
                        usr.Mail,
                        usr.DisplayName
                        );

                        //Reimposto i parametri dell'utente
                        eWAPIConnector.SetUserParameter(
                                this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                                this.eWSysParameter.MainUserId,
                                this.eWSysParameter.MainUserPwd,
                                usr.ExternalID,
                                usr.IsHost,
                                usr.IsController,
                                usr.Audio,
                                usr.Video,
                                usr.Chat
                                );
                    }
                    catch (Exception Ex)
                    {
                        error = true;
                    }

                    if (String.IsNullOrEmpty(usr.ExternalID))
                    {
                        error = true;
                    }
                    //}

                    if (!error)
                    {
                        try
                        {
                            DAL.UserSaveOrUpdate(usr);
                        }
                        catch
                        {
                            error = true;
                        }
                    }

                    if (error)
                    {
                        try
                        {
                            DAL.UserDelete(RoomId, usr.Mail);
                        }
                        catch { }
                    }
                }
            }
            DAL.RoomUpdateUserNumber(RoomId);
        }

        



        /// <summary>
        /// Recupera gli iscritti ad una stanza
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>Lista di utente iscritti alla stanza</returns>
        public override IList<WbUser> UsersGet(Int64 RoomId, Domain.DTO.DTO_UserListFilters Filters, int PageIndex, int PageSize, ref int PageCount)
        {
            return DAL.UsersGet(RoomId, Filters, PageIndex, PageSize, ref PageCount);
        }

        /// <summary>
        /// Abilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: Invia MAIL di prima iscrizione
        /// False: Non inviare mail...
        /// </returns>
        public override bool UserEnable(Int64 UserId, long RoomId)
        {

            bool NeedMail = false;

            String RoomKey = DAL.RoomGetExternaId(RoomId);

            WbUser User = DAL.UserGet(UserId); //DAL.GetUserInRoomByPerson(PersonId, RoomId);

            if (User.MailChecked == false && User.Enabled == false)
            {
                NeedMail = true;
            }

            if (String.IsNullOrEmpty(User.ExternalID))
            {
                UserAddToExternalSystem(ref User);
            }

            if (User.Enabled == false)
            {
                eWAPIConnector.EnableKey(
                this.eWSysParameter.BaseUrl,
                this.eWSysParameter.ProxyUrl,
                this.eWSysParameter.MainUserId,
                this.eWSysParameter.MainUserPwd,
                RoomKey,
                User.ExternalID
                );
            }

            User.MailChecked = true;

            if (User != null)
            {
                User.Enabled = true;
            }
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
        /// <remarks>
        /// Al momento disabilitata lato eWorks,
        /// finchè manca l'enable key.
        /// </remarks>
        public override bool UserDisable(int UserId, long RoomId)
        {
            

            String RoomKey = DAL.RoomGetExternaId(RoomId);

            WbUser User = DAL.UserGet(UserId);
            
            if (User != null)
            {

                eWAPIConnector.DisableKey(
                    this.eWSysParameter.BaseUrl,
                    this.eWSysParameter.ProxyUrl,
                    this.eWSysParameter.MainUserId,
                    this.eWSysParameter.MainUserPwd,
                    RoomKey,
                    User.ExternalID
                );

                User.Enabled = false;

                DAL.UserSaveOrUpdate(User);
            }
            

            WbRoom oRoom = DAL.RoomGet(RoomId);
            bool NeedMail = oRoom.NotificationDisableUsr;       //Intanto mando SEMPRE! POI su configurazione stanza.
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

            if (User != null && !String.IsNullOrEmpty(User.ExternalID))
            {
                eWAPIConnector.DeleteKey(
                    this.eWSysParameter.BaseUrl,
                    this.eWSysParameter.ProxyUrl,
                    this.eWSysParameter.MainUserId,
                    this.eWSysParameter.MainUserPwd,
                    RoomKey,
                    User.ExternalID
                );
            }

            DAL.UserDelete(User.Id);
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
                    WbUser CurUsr = DAL.UserGetInRoom(usr.Id, RoomId);

                    if (CurUsr == null)
                    {
                        // !!!! UTENTE NON NELLA STANZA O NON RICONOSCIUTO!
                        CurUsr = new WbUser();
                        CurUsr.Mail = usr.Mail;
                    }

                    if (CurUsr != null)
                    {
                        //CurUsr.DisplayName = usr.DisplayName;
                        CurUsr.Name = usr.Name;
                        CurUsr.SName = usr.SName;

                        CurUsr.IsHost = usr.IsHost;
                        CurUsr.IsController = usr.IsController;
                        CurUsr.Video = usr.Video;
                        CurUsr.Audio = usr.Audio;
                        CurUsr.Chat = usr.Chat;

                        try
                        {
                            eWAPIConnector.SetUserParameter(
                            this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                            this.eWSysParameter.MainUserId,
                            this.eWSysParameter.MainUserPwd,
                            CurUsr.ExternalID,
                            CurUsr.IsHost,
                            CurUsr.IsController,
                            CurUsr.Audio,
                            CurUsr.Video,
                            CurUsr.Chat
                            );
                        }
                        catch { }
                        

                        DAL.UserSaveOrUpdate(CurUsr);
                    }
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
            if (String.IsNullOrEmpty(User.ExternalID))
            {
                try
                {
                    User.ExternalID = eWAPIConnector.CreateKey(
                        this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                        this.eWSysParameter.MainUserId,
                        this.eWSysParameter.MainUserPwd,
                        User.ExternalRoomId,
                        User.Mail,
                        User.DisplayName
                        );

                    eWAPIConnector.SetUserParameter(
                        this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                        this.eWSysParameter.MainUserId,
                        this.eWSysParameter.MainUserPwd,
                        User.ExternalID,
                        User.IsHost,
                        User.IsController,
                        User.Audio,
                        User.Video,
                        User.Chat
                        );

                }
                catch (Exception ex)
                {
                    User.ExternalID = "";
                }
            }
        }

    
    #endregion


        public override void RoomRecordingUpdate()
        {
            IList<WbRoom> Rooms = DAL.RoomsGetAll();

            foreach (WbRoom Room in Rooms)
            {
                eWRoomParameters Parameters = new eWRoomParameters();
                try
                {
                    Parameters = eWAPIConnector.GetRoomParameters(
                        this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
                        this.eWSysParameter.MainUserId,
                        this.eWSysParameter.MainUserPwd,
                        Room.ExternalId);

                    Room.Recording = Parameters.recording;

                    DAL.RoomUpdate(Room);
                }
                catch
                {
                    //Room.Deleted = BaseStatusDeleted.Automatic;   //  <-- CONTROLLARE RECUPERO DATI DAL DAL!
                }

                //if (Parameters == null)
                //{
                //    Room.Deleted = BaseStatusDeleted.Automatic;
                //}


                //if (Room.Deleted == BaseStatusDeleted.None)
                //{
                //    
                //}
            }
        }

        public override void UserUpateInternal(long UserId, string Name, string SName, string Mail)
        {
            return;

            string DisplayName = SName + " " + Name;

            WbUser User = Manager.Get<WbUser>(UserId);

            //eWAPIConnector.SetUserInfo(
            //    this.eWSysParameter.BaseUrl, this.eWSysParameter.ProxyUrl,
            //    this.eWSysParameter.MainUserId,
            //    this.eWSysParameter.MainUserPwd,
            //    UserId, DisplayName, Mail);



            throw new NotImplementedException();
            
        }

        //
        //{
        //    throw new NotImplementedException();
        //}

        public override bool RoomNameExternalUpdate(string RoomExternalId, string NewName)
        {
            //eWRoomParameters param = new eWRoomParameters();

            //param.meetingtitle = NewName;
            bool success = true;

            try
            {
                //SetName
                eWAPIConnector.SetName(
                       this.eWSysParameter.BaseUrl,
                       this.eWSysParameter.ProxyUrl,
                       this.eWSysParameter.MainUserId, 
                       this.eWSysParameter.MainUserPwd,
                       RoomExternalId,
                       NewName,
                       this.eWSysParameter.MaxUrlChars,
                       this.eWSysParameter.Version);
            }
            catch (Exception)
            {
                success = false;
            }
            
            return success;
        }
    }
}