using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using WCMod = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing;

namespace lm.Comol.Modules.Standard.WebConferencing.Presentation
{
    /// <summary>
    /// L'accesso esterno funziona nel seguente modo:
    /// UTENTI INTERNI:
    ///     - SE GIA' ISCRITTI:
    ///         potranno inserire mail e chiave (o farsi mandare nuovamente la chiave)
    ///     - SE NON ISCRITTI:
    ///         Devono effettuare login per poter accedere. Una volta effettuato, in base ai permessi, potranno:
    ///         - Accedere direttamente alla stanza
    ///         - Essere iscritti, ma attendere notifica di avvenuta abilitazione
    ///         - Essere bloccati
    /// UTENTI ESTERNI:
    ///     - Accedere con Mail e Chiave precedentemente ricevuta
    ///     - Iscriversi (se permesso dalla stanza). In tal caso:
    ///         - Dovranno attendere abilitazione da parte dell'amministratore (invio credenziali)
    ///         - Riceveranno le credenziali d'accesso (mail+chiave)
    /// </summary>
    public class WbExtAccessPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region Costruttori/view/service/module

        private lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService ServiceTemplate;

        public WbExtAccessPresenter(iApplicationContext oContext)
            : base(oContext)
         { }

         public WbExtAccessPresenter(iApplicationContext oContext, View.iViewWbExtAccess view)
                : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
            this.ServiceTemplate = new Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
        }

        protected virtual View.iViewWbExtAccess View
        {
            get { return (View.iViewWbExtAccess)base.View; }
        }

        private Domain.ModuleWebConferencing _module;
        private Domain.ModuleWebConferencing Module
        {
            get
            {
                if ((_module == null))
                {
                    Int32 idUser = UserContext.CurrentUserID;
                    Int32 idCommunity = UserContext.CurrentCommunityID;
                    _module = Service.GetServicePermission(idUser, idCommunity);
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
        /// Inizializza la pagina.
        /// Se l'utente viene riconosciuto e può accedere, accede direttamente!
        /// </summary>
        public void Init()
        {
            if(!ServerStatus())
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoServer);
                View.ShowError(Domain.ErrorExtAccess.NoServer);
                View.BindSkin(Service.ServiceModuleID(), 0, 0);
                View.Show(true, false, false, false, true);
                return;
            }

            Domain.WbAccessCode Code = null;

            if (!string.IsNullOrEmpty(View.UrlCode))
            {
                Code = this.Service.RoomCodeDataGet(View.UrlCode);
            } else if (View.RoomId > 0)
            {
                Code = new Domain.WbAccessCode() { RoomId = View.RoomId, UrlCode = "" };
            } else {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoRoom);
                View.BindSkin(Service.ServiceModuleID(), 0, 0);
                View.ShowError(Domain.ErrorExtAccess.UnknowRoom);
                View.Show(false, false, false, false, true);
                return;
            }

            if (Code == null || Code.RoomId <= 0)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoRoom);
                View.BindSkin(Service.ServiceModuleID(), 0, 0);
                View.ShowError(Domain.ErrorExtAccess.UnknowRoom);
                View.Show(true, false, false, false, true);
                return;
            }

            Domain.WbRoom oRoom = this.Service.RoomGet(Code.RoomId);
            if (oRoom == null)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoRoom);
                View.BindSkin(Service.ServiceModuleID(), 0, 0);
                View.ShowError(Domain.ErrorExtAccess.UnknowRoom);
                View.Show(true, false, false, false, true);
                return;
            }

            View.Room = oRoom;
            int OrgnId = 0;
            View.BindSkin(Service.ServiceModuleID(), oRoom.CommunityId, OrgnId);

            //UTENTE INTERNO
            if (UserContext != null && UserContext.CurrentUserID > 0)
            {
                //Iscritto alla stanza
                Domain.WbUser CurWBUser = Service.GetUserFromSystem(Code.RoomId, UserContext.CurrentUserID);
                
                if (CurWBUser == null)
                {
                    // UTENTE LOGGATO, NON ISCRITTO ALLA CONFERENZE:

                    if (Service.UserPersonIsInCommunity(Code.RoomId, UserContext.CurrentUserID))
                    {
                        // UTENTE iscritto alla comunità
                        switch (oRoom.SubCommunity)
                        {
                            case Domain.SubscriptionType.Free:
                                if (!this.Service.UserPersonAdd(UserContext.CurrentUserID, oRoom.Id, true))
                                {
                                    View.ShowError(Domain.ErrorExtAccess.InternalSubScriptionError);
                                    return;
                                }
                                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserSubscribeSelfInternal);
                                break;

                            case Domain.SubscriptionType.Moderated:
                                if (!this.Service.UserPersonAdd(UserContext.CurrentUserID, oRoom.Id, false))
                                {
                                    View.ShowError(Domain.ErrorExtAccess.InternalSubScriptionError);
                                    return;
                                }
                                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddInternal);
                                View.ShowError(Domain.ErrorExtAccess.AdminConfirmRequired);
                                View.Show(true, false, false, false, true);
                                return;

                            case Domain.SubscriptionType.None:
                                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                                View.ShowError(Domain.ErrorExtAccess.NoPermission);
                                View.Show(true, false, false, false, true);
                                return;
                        }
                    }
                    else
                    {
                        // UTENTE NON iscritto alla comunità
                        switch (oRoom.SubSystem)
                        {
                            case Domain.SubscriptionType.Free:
                                if (!this.Service.UserPersonAdd(UserContext.CurrentUserID, oRoom.Id, true))
                                {
                                    View.ShowError(Domain.ErrorExtAccess.InternalSubScriptionError);
                                    return;
                                }
                                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserSubscribeSelfInternal);
                                break;

                            case Domain.SubscriptionType.Moderated:
                                if (!this.Service.UserPersonAdd(UserContext.CurrentUserID, oRoom.Id, false))
                                {
                                    View.ShowError(Domain.ErrorExtAccess.InternalSubScriptionError);
                                    View.Show(true, false, false, false, true);
                                    return;
                                }
                                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddInternal);
                                View.ShowError(Domain.ErrorExtAccess.AdminConfirmRequired);
                                View.Show(true, false, false, false, true);
                                return;

                            case Domain.SubscriptionType.None:
                                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                                View.ShowError(Domain.ErrorExtAccess.NoPermission);
                                View.Show(true, false, false, false, true);
                                return;
                        }

                        
                    }

                    // Ora, se arriva QUI l'utente è iscritto alla stanza e lo recupero.
                    CurWBUser = Service.GetUserFromSystem(Code.RoomId, UserContext.CurrentUserID);
                }

                // QUI ho l'utente, iscritto alla stanza. Mostro la stanza ed esco dalla funzione.
                if (CurWBUser == null)
                {
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                    View.ShowError(Domain.ErrorExtAccess.NoPermission);
                    View.Show(true, false, false, false, true);
                    return;
                }

                if (CurWBUser.Enabled == true)
                {
                    String Url = this.Service.AccessUrlExternalGet(Code.RoomId, CurWBUser.Id);
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomAccess);
                    this.View.ShowConference(Url);
                    View.Show(true, false, false, true, false);
                    return;
                }
                else
                {
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                    View.ShowError(Domain.ErrorExtAccess.AdminConfirmRequired);
                    View.Show(true, false, false, false, true);
                    return;
                }
                
            }
            else // Utente esterno o sconosciuto
            {
                // Mostro l'accesso (per utenti iscritti, interni o esterni)
                View.ShowAccess();

                Boolean ShowSubscription = true;
                if (oRoom.SubExternal == Domain.SubscriptionType.None)
                {
                    //this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                    ShowSubscription = false;
                }

                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserSubscribeLogin);
                View.Show(true, true, ShowSubscription, false, false);
            }
        }

        /// <summary>
        /// Invia una mail con il codice all'utente indicato
        /// </summary>
        /// <param name="Mail">Indirizzo mail a cui spedire il codice</param>
        public void SendNewMail(String Mail)
        {
            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.MailRequestSend);
        }

        /// <summary>
        /// Accede alla stanza
        /// </summary>
        /// <param name="Key">Chiave accesso alla stanza</param>
        public void EnterRoom()
        {
            Domain.WbAccessCode wbCode = Service.RoomCodeDataGet(View.UrlCode);//WbDal.GetCode(View.UrlCode);
            
            if (wbCode == null)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoRoom);
                View.ShowError(Domain.ErrorExtAccess.UnknowRoom);
                View.Show(true, true, false, false, true);
                return;
            }

            Domain.WbRoom wbRoom = Service.RoomGet(wbCode.RoomId);//WbDal.GetRoom(wbCode.RoomId);
            
            if (wbRoom == null)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoRoom);
                View.ShowError(Domain.ErrorExtAccess.UnknowRoom);
                View.Show(true, true, false, false, true);
                return;
            }

   
            //  NOOOOOO - ISCRIZIONE UTENTE!!!
            Domain.WbUser User = Service.GetUserFromMail(wbCode.RoomId, View.Mail);

            if (User == null)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoRoom);
                View.ShowError(Domain.ErrorExtAccess.NoPermission);
                return;
                //int PersonId = -1;
                //Domain.UserCheck CheckUser = Service.CheckUser(View.Mail, wbCode.RoomId, ref PersonId);
                //if(CheckUser == Domain.UserCheck.UserInCommunity)
                //{
                //    //Utente iscritto alla comunità
                //    switch(wbRoom.SubCommunity)
                //    {
                //        case Domain.SubscriptionType.Moderated:
                //            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddInternal);
                //            Service.PersonAdd(PersonId, wbRoom.Id, false);
                //            break;
                //        case Domain.SubscriptionType.Free:
                //            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddInternal);
                //            Service.PersonAdd(PersonId, wbRoom.Id, true);
                //            break;
                //        case Domain.SubscriptionType.None:
                //            View.ShowError("NOT ALLOW");
                //            return;
                //    }
                //}
                //else if (CheckUser == Domain.UserCheck.UserInSystem)
                //{
                //    //Utente iscritto al sistema
                //    switch (wbRoom.SubSystem)
                //    {
                //        case Domain.SubscriptionType.Moderated:
                //            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddInternal);
                //            Service.PersonAdd(PersonId, wbRoom.Id, false);
                //            break;
                //        case Domain.SubscriptionType.Free:
                //            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UsersAddInternal);
                //            Service.PersonAdd(PersonId, wbRoom.Id, true);
                //            break;
                //        case Domain.SubscriptionType.None:
                //            View.ShowError("NOT ALLOW");
                //            return;
                //    }
                //}
                //else
                //{
                //    //UTENTE ESTERNO!!
                //    if (!User.MailChecked && String.IsNullOrEmpty(User.ExternalID))
                //    {
                //        switch (wbRoom.SubExternal)
                //        {
                //            case Domain.SubscriptionType.Free:
                //                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserSubscribeSelfExternal);
                //                Service.ValidateExternalUser(User.Id);
                //                break;
                //            case Domain.SubscriptionType.Moderated:
                //                View.ShowError("NeedAdminconfirmation");
                //                return;
                //            case Domain.SubscriptionType.None:
                //                View.ShowError("NOT ALLOW!");
                //                return;
                //        }
                //    }
                //}
            }
            else if (User.UserKey != View.Key)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoRoom);
                View.ShowError(Domain.ErrorExtAccess.WrongUserKey);
                return;
            }

            if (User.MailChecked == false)
            {
                Service.UserEnable(User.Id, wbRoom.Id);
            }

            String Url = this.Service.AccessUrlExternalGet(wbRoom.Id, User.Id);

            this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomAccess);
            this.View.ShowConference(Url);
        }

        /// <summary>
        /// Iscrive l'utente agli utenti in lista.
        /// A seconda delle ipostazioni della stanza, l'utente potrà:
        /// - Essere già attivato e ricevere una mail con il codice
        /// - Essere iscritto come disattivatto/In attesa di attivazione. La mail verrà inviata dall'amministratore
        /// </summary>
        /// <param name="UserName">Nome utente</param>
        /// <param name="UserSecondName">cognome utente</param>
        /// <param name="Mail">Indirizzo eMail</param>
        /// <param name="LangCode">Codice lingua</param>
        public void SubscribeUser(String name, String secondName, String mail, String languageCode)
        {

            //Codice stanza
            Domain.WbAccessCode Code = this.Service.RoomCodeDataGet(View.UrlCode);

            //Stato iscrizione
            Domain.ExtSubscriptionStatus Status = Domain.ExtSubscriptionStatus.NoPermission;

            //Controllo mail
            Domain.MailCheck MailCK = Service.UserMailCheck(mail, Code.RoomId);

            //In base al controllo mail:
            switch(MailCK)
            {
                case Domain.MailCheck.MailUnknow:
                    Domain.WbUser user = this.Service.UserSubscribeNewExternal(View.UrlCode, name, secondName, mail, languageCode, ref Status);
                    //INVIO KEY VIA MAIL E/O CONTROLLO SU BLOCCO ISCRIZIONI!
                    if (user != null)
                    {
                        //Invio Mail
                        Int32 idLanguage = ServiceTemplate.GetIdLanguage(languageCode);
                        lm.Comol.Core.Notification.Domain.dtoNotificationMessage msg = null;
                        if (idLanguage>0)
                            msg = ServiceTemplate.GetNotificationMessage(idLanguage, WCMod.UniqueCode,(Int64)WCMod.MailSenderActionType.Credential);
                        else
                            msg = ServiceTemplate.GetNotificationMessage(user.LanguageCode,WCMod.UniqueCode,(Int64)WCMod.MailSenderActionType.Credential);
                        if (msg == null)
                        {
                            View.ShowError(Domain.ErrorExtAccess.NoTemplate);
                            View.Show(true, true, false, false, true);
                            return;
                        }


                        Domain.DTO.DTO_MailTagSettings mts = View.GetMailTagSetting(); // 2 = Language ID!

                        msg.Translation = Service.GetTemplateContentPreview(
                            true,
                            Code.RoomId,
                            user.Id,
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
                            user.Mail);


                        if (sentMail == false)
                        {
                            View.ShowError(Domain.ErrorExtAccess.MailSenderError);
                            View.Show(true, true, true, false, true);
                        }
                        
                        this.SendUserAction(Domain.ModuleWebConferencing.ActionType.UserSubscribeSelfExternal);
                    }
                    else
                    {
                        this.SendUserAction(Domain.ModuleWebConferencing.ActionType.GenericError);
                    }
                    break;
                case Domain.MailCheck.MailInRoom:
                    Status = Domain.ExtSubscriptionStatus.MailInRoom;
                    break;
                case Domain.MailCheck.MailInRoomdB:
                    Status = Domain.ExtSubscriptionStatus.MailInRoom;
                    break;
                case Domain.MailCheck.MailInSystem:
                    Status = Domain.ExtSubscriptionStatus.MailInSystem;
                    break;
                case Domain.MailCheck.ParameterError:
                    Status = Domain.ExtSubscriptionStatus.ParametersError;
                    break;
            }

            View.ShowSubStatus(Status);
        }

        /// <summary>
        /// Stato del server
        /// </summary>
        /// <returns>
        /// True: server ok
        /// False: server non raggiungibile
        /// </returns>
        public Boolean ServerStatus()
        {
            return this.Service.ServerCheck();
        }

        /// <summary>
        /// Invio azioni utente
        /// </summary>
        /// <param name="Action">Azione utente</param>
        private void SendUserAction(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType Action)
        {            
            View.SendUserAction(UserContext.CurrentCommunityID, Service.ServiceModuleID(), Action);
        }

        /// <summary>
        /// Elenco lingue disponibili
        /// </summary>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_Language> GetLanguages()
        {
            List<Int32> LanguagesId = new List<int>();

            foreach (KeyValuePair<int, string> LangKeyPair in View.SystemLanguages)
            {
                LanguagesId.Add(LangKeyPair.Key);
            }

            return Service.LanguagesGet(LanguagesId);
        }

        
    }
}