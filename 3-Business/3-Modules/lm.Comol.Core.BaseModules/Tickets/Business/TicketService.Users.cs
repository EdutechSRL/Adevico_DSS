using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using lm.Comol.Core.Notification.Domain;

namespace lm.Comol.Core.BaseModules.Tickets
{
    public partial class TicketService : CoreServices
    {
        /// <summary>
        /// Crea un utente da una persona
        /// </summary>
        /// <param name="PersonId">Id Persona</param>
        /// <param name="Settings">Impostazioni</param>
        /// <returns>New User Id</returns>
        public Domain.TicketUser UserCreateFromPerson(
            Int32 PersonId,
            Domain.Enums.MailSettings Settings)
        {
            litePerson Person = Manager.GetLitePerson(PersonId);//Manager.Get<Person>(PersonId);

            if (Person == null)
                return null;

            TicketUser User = new TicketUser();

            User.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            User.Person = Person;
            User.Enabled = true;
            User.MailChecked = false;
            //Viene messo a false, poichè NECESSITA comunque di registrazione e convalida via mail da pagine esterne.
            //VEDI: TokenValidate()

            Manager.SaveOrUpdate<TicketUser>(User);

            //Imposto i parametri globali per quell'utente, quando lo aggiungo come User nei Ticket...
            SettingsSetGlobalUser(User.Id, Settings, true, true);

            return User;
        }

        public static liteUser UserConvetToLiteUser(TicketUser user)
        {
            liteUser usr = new liteUser();
            usr.Id = user.Id;
            usr.Name = user.Name;
            usr.Surname = user.Sname;
            usr.Person = user.Person;
            return usr;
        }

        /// <summary>
        /// Crea utente da esterno
        /// </summary>
        /// <param name="Name">Nome</param>
        /// <param name="Sname">cognome</param>
        /// <param name="Mail">Mail</param>
        /// <returns></returns>
        public Domain.DTO.DTO_ExtUserAddResult UserCreateFromExternal(
            String Name,
            String Sname,
            String Mail,
            String LangCode,
            String Code,
            Domain.DTO.DTO_NotificationSettings Settings)
        {

            Domain.DTO.DTO_ExtUserAddResult response = new Domain.DTO.DTO_ExtUserAddResult();
            response.Errors = Domain.Enums.ExternalUserCreateError.none;

            // 1. Check formato MAIL
            if (!MailCheckFormat(Mail))
            {
                response.User.UserId = 0;
                response.User.PersonId = 0;
                response.Errors = Domain.Enums.ExternalUserCreateError.invalidMail;
                return response;
            }



            litePerson SysPerson = null;
            Boolean IsInternal = false;



            // 2. Controllo se In TicketSystem
            TicketUser User = (from TicketUser usr in Manager.GetIQ<TicketUser>()
                               where usr.mail == Mail ||
                               usr.Person != null && usr.Person.Mail == Mail
                               select usr).Skip(0).Take(1).ToList().FirstOrDefault();

            // Utente in Ticket System, con password già impostata: Utilizzare RECUEPRA PASSWORD (err.TicketMail)
            if (User != null && User.Id > 0)
            {
                response.User.UserId = User.Id;

                //Utente INTERNO!    -   MAIL VALIDA!!!
                if (User.Person != null)
                {
                    response.User.PersonId = User.Person.Id;
                    SysPerson = User.Person;
                    IsInternal = true;

                    if (!String.IsNullOrEmpty(User.Code))
                    {
                        response.Errors = Domain.Enums.ExternalUserCreateError.TicketMail;
                        return response;
                    }
                }
                else
                {
                    // UTENTE INTERNO - contorllo se MAIL CHECKATA:
                    // Sì: usare recupera password
                    // NO: ovverride Utente con dati forniti!

                    response.User.PersonId = 0;

                    if (User.MailChecked)
                    {
                        response.Errors = Domain.Enums.ExternalUserCreateError.TicketMail;
                        return response;
                    }
                }

                //L'utente verrà aggiornato!
                User.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                //NUOVO UTENTE TICKET!
                User = new TicketUser();
                User.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                User.Enabled = true;

                //3. Controllo se In Sistema
                SysPerson = (from litePerson prsn in Manager.GetIQ<litePerson>() where prsn.Mail == Mail select prsn).Skip(0).Take(1).ToList().FirstOrDefault();

                if (SysPerson != null && SysPerson.Id > 0)
                {
                    IsInternal = true;
                    response.Errors = Domain.Enums.ExternalUserCreateError.internalMail;
                }
                else
                {
                    IsInternal = false;
                    SysPerson = null;
                }

            }


            if (IsInternal)
            {
                User.Name = "";
                User.Sname = "";
                User.mail = "";
                User.Person = SysPerson;
            }
            else
            {
                User.Name = Name;
                User.Sname = Sname;
                User.mail = Mail;
            }

            User.MailChecked = false;
            User.LanguageCode = LangCode;
            User.Code = AuthenticationHelper.Encrypt(Code);
            Manager.SaveOrUpdate<TicketUser>(User);

            //Aggiorno i campi di REsponse con quelli in dB.
            if (IsInternal)
            {
                response.User.Name = User.Person.Name;
                response.User.SName = User.Person.Surname;
            }
            else
            {
                response.User.Name = User.Name;
                response.User.SName = User.Sname;
            }
            //response.DisplayName = (String.IsNullOrEmpty(User.DisplayName) ? User.Sname + " " + User.Name : User.DisplayName);

            response.User.Mail = User.mail;

            //MAIL CONVALIDA:
            // 1. GENERO CODICE VERIFICA
            Token tok = this.TokenCreate(User.Id, Domain.Enums.TokenType.Registration);

            // 2. Lo invio
            Boolean Sended = this.NotificationSendToken(User, Settings, tok, Code);

            response.Note = "<br/>Token: " + tok.Code.ToString() + "<br/>Expire on: " + tok.CreatedOn.Add(TokenLifeTime).ToString();
            //x TEST:
            if (Sended)
                response.Note += "<br/> - SENDED -";
            else
                response.Note += "<br/> - SENDING ERROR! -";

            // Eventuali errori:    response.Errors = Domain.Enums.ExternalUserCreateError.internalError;

            return response;
        }

        public Domain.Enums.ExternalUserPasswordErrors UserExtChangePassword(
            String OldPassword, String NewPassword, Int64 UserId)
        {
            Domain.TicketUser Usr = Manager.Get<Domain.TicketUser>(UserId);

            if (Usr == null || Usr.Id <= 0)
                return Domain.Enums.ExternalUserPasswordErrors.UserNotFound;

            if (Usr.Code != AuthenticationHelper.Encrypt(OldPassword))
            {
                return Domain.Enums.ExternalUserPasswordErrors.InvalidPassword;
            }

            Usr.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            Usr.Code = AuthenticationHelper.Encrypt(NewPassword);

            Manager.SaveOrUpdate<Domain.TicketUser>(Usr);

            return Domain.Enums.ExternalUserPasswordErrors.none;
        }


        /// <summary>
        /// Valida la mail di un utente, in automatico al primo tentativo di accesso
        /// </summary>
        /// <param name="Mail">Mail utente</param>
        /// <param name="Code">Codice utente</param>
        /// <returns>Eventuali segnalazioni errori</returns>
        public Domain.DTO.DTO_User UserValidateExternal(
            String Mail,
            String Code,
            String Token,
            ref Domain.Enums.ExternalUserValidateError Err)
        {
            Err = Domain.Enums.ExternalUserValidateError.none;

            TicketUser User = Manager.GetAll<TicketUser>(u => u.mail == Mail || (u.Person != null && u.Person.Mail == Mail)).Skip(0).Take(1).ToList().FirstOrDefault();

            if (User == null || User.Id <= 0)
            {
                Err = Domain.Enums.ExternalUserValidateError.invalidMail;
                return new Domain.DTO.DTO_User();
            }
            //else if (String.IsNullOrEmpty(User.Code))
            //{
            //    Err = Domain.Enums.ExternalUserValidateError.invalidCode;
            //    return new Domain.DTO.DTO_User();
            //}
            else if (User.Code != AuthenticationHelper.Encrypt(Code))
            {
                Err = Domain.Enums.ExternalUserValidateError.invalidCode;
                return new Domain.DTO.DTO_User();
            }

            //Controllo Token
            if (User.MailChecked == false)
            {
                Domain.Enums.TokenValidationResult TokErr = this.TokenValidate(Token, User.Id, Domain.Enums.TokenType.Registration);

                switch (TokErr)
                {
                    //case Domain.Enums.TokenValidationResult.UserNotFound
                    case Domain.Enums.TokenValidationResult.TokenNotFound:
                        Err = Domain.Enums.ExternalUserValidateError.TokenEmpty;
                        //return new Domain.DTO.DTO_User();
                        break;
                    case Domain.Enums.TokenValidationResult.InvalidFormat:
                        Err = Domain.Enums.ExternalUserValidateError.TokenInvalid;
                        //return new Domain.DTO.DTO_User();
                        break;
                    case Domain.Enums.TokenValidationResult.Exired:
                        Err = Domain.Enums.ExternalUserValidateError.TokenExpired;
                        //return new Domain.DTO.DTO_User();
                        break;
                    //case Domain.Enums.TokenValidationResult.Validated:
                }

            }

            Boolean IsInternal = (User.Person != null);

            String LangCode = "";
            if (!String.IsNullOrEmpty(User.LanguageCode))
                LangCode = User.LanguageCode;
            else if (IsInternal)
            {
                Language lang = Manager.Get<Language>(User.Person.LanguageID);
                if (lang != null)
                    LangCode = lang.Code;
            }
            else
            {
                Language lang = Manager.GetDefaultLanguage();
                LangCode = lang.Code;
            }

            Domain.DTO.DTO_User Usr = new Domain.DTO.DTO_User
            {
                UserId = User.Id,
                PersonId = (IsInternal) ? User.Person.Id : -1,
                LanguageCode = LangCode,
                Mail = (IsInternal) ? User.Person.Mail : User.mail,
                Name = (IsInternal) ? User.Person.Name : User.Name,
                SName = (IsInternal) ? User.Person.Surname : User.Sname,
                IsOwnerNotificationEnable = User.IsNotificationActiveUser
            };

            //UserId = User.Id;

            return Usr;
        }

        /// <summary>
        /// Dato un ID utente, recupera il numero di Ticket da lui aperti
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int UserGetTicketNum(Int64 UserId)
        {
            return (from Ticket tk in Manager.GetIQ<Ticket>()
                    where tk.Owner.Id == UserId
                    select tk.Id).Count();
        }

        public Int64 UserGetTicketID(Int64 UserId)
        {
            return (from Ticket tk in Manager.GetIQ<Ticket>()
                    where tk.Owner.Id == UserId
                    select tk.Id).FirstOrDefault();
        }


        /// <summary>
        /// Recupera un utente specifico
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <returns>Oggetto User</returns>
        public Domain.TicketUser UserGet(Int64 UserId)
        {
            return Manager.Get<Domain.TicketUser>(UserId);
        }

        public bool UsersCommunityHasManRes(int CommunityId, Int64 UnloadedUserID = 0)
        {
            IEnumerable<Domain.LK_UserCategory> sLKCate = from Domain.LK_UserCategory lkuc
                                                              in Manager.GetIQ<LK_UserCategory>()
                                                          where lkuc.Category != null
                                                          && lkuc.Category.IdCommunity == CommunityId
                                                          && lkuc.User != null
                                                          && lkuc.User.Id != UnloadedUserID
                                                          select lkuc;

            return (from Domain.LK_UserCategory lkuc in sLKCate
                                               select lkuc.User).Any();
        }
        /// <summary>
        /// Recupera l'elenco dei manager/resolver all'interno di una comunità
        /// </summary>
        /// <param name="CommunityId"></param>
        /// <param name="GetManager"></param>
        /// <param name="GetResolver"></param>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_User> UsersGetManRes(int CommunityId, bool GetManager, bool GetResolver, Int64 UnloadedUserID = 0)
        {
            if (!GetManager && !GetResolver)
                return null;

            IEnumerable<Domain.LK_UserCategory> sLKCate = from Domain.LK_UserCategory lkuc
                                                              in Manager.GetIQ<LK_UserCategory>()
                                                          where lkuc.Category != null
                                                          && lkuc.Category.IdCommunity == CommunityId
                                                          && lkuc.User != null
                                                          && lkuc.User.Id != UnloadedUserID
                                                          select lkuc;

            if (GetManager & !GetResolver)
                sLKCate = sLKCate.Where(lkuc => lkuc.IsManager == true);

            if (!GetManager & GetResolver)
                sLKCate = sLKCate.Where(lkuc => lkuc.IsManager == false);

            IEnumerable<TicketUser> TkUsers = (from Domain.LK_UserCategory lkuc in sLKCate
                                               select lkuc.User).Distinct();

            IList<Domain.DTO.DTO_User> users = (from Domain.TicketUser User in TkUsers
                                                select new Domain.DTO.DTO_User
                                                {
                                                    Name = (User.Person == null) ? User.Name : User.Person.Name,
                                                    SName = (User.Person == null) ? User.Sname : User.Person.Surname,
                                                    UserId = User.Id,
                                                    LanguageCode = User.LanguageCode,
                                                    PersonId = (User.Person == null) ? -1 : User.Person.Id,
                                                    Mail = (User.Person == null) ? User.mail : User.Person.Mail
                                                }).ToList();

            return users;
        }

        /// <summary>
        /// Recupera un utente da una Person,
        /// SE non trova l'utente, lo crea da Person.
        /// </summary>
        /// <param name="PersonId">Id Person</param>
        /// <returns>Oggetto User</returns>
        public Domain.TicketUser UserGetfromPerson(int PersonId)
        {
            Domain.TicketUser User = Manager.GetAll<Domain.TicketUser>(tu => tu.Person != null && tu.Person.Id == PersonId).FirstOrDefault();

            if (User == null)
            {
                Domain.Enums.MailSettings settings = Domain.Enums.MailSettings.Default;
                User = UserCreateFromPerson(PersonId, settings);
            }

            return User;
        }

        /// <summary>
        /// Recupera un utente da una Person,
        /// SE non trova l'utente, lo crea da Person.
        /// </summary>
        /// <param name="PersonId">Id Person</param>
        /// <returns>Oggetto User</returns>
        public Int64 UserGetIdfromPerson(int PersonId)
        {
            if (PersonId <= 0)
                return -1;

            Int64 UserId = (from TicketUser tu in Manager.GetIQ<Domain.TicketUser>()
                            where tu.Person != null
                                && tu.Person.Id == PersonId
                            select tu.Id).FirstOrDefault();

            if (UserId > 0)
                return UserId;
            else
            {
                TicketUser TkUser = this.UserGetfromPerson(PersonId);
                if (TkUser != null && TkUser.Id > 0)
                    return TkUser.Id;
            }

            return -1;

        }

        /// <summary>
        /// Controlla se l'utente CORRENTE è Manager o Resolver in una qualunque categoria.
        /// SE non lo è, restituisce FALSE!
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Mi serve nella lista utente per attivare eventualmente il tasto "Management"
        /// </remarks>
        public bool UserIsManagerOrResolver()
        {
            if (CategoryIdManRes.Any())
                return true;

            return UserHasAnyAssignment();
        }

        public bool UserIsManagerOrResolver(Int64 UserId)
        {
            //IList<Int64> CatIdMan = CategoriesIdGetManager(UserId);
            ////IDictionary<Int64, bool> DICT_TkIdMan = CatIdMan.Distinct().ToDictionary(k => k, v => true);
            //IList<Int64> CatIdRes = CategoriesIdGetResolver(UserId).Except(CatIdMan).ToList();
            //return CatIdMan.Any() && CatIdRes.Any();

            if (CategoryIdManRes.Any())
                return true;

            return UserHasAnyAssignment();
        }

        public bool UserIsManagerOrResolverInCategory(Int64 CategoryId, Int64 UserId)
        {
            return (from liteLK_UserCategory llk in Manager.GetIQ<liteLK_UserCategory>() 
                             where llk.IdCategory != null 
                             && llk.IdCategory == CategoryId
                             && llk.IdUser == UserId
                             select llk.Id).Any();

        }

        /// <summary>
        /// Controlla se per la comunità indicata esistono Ticket associati DIRETTAMENTE all'utente
        /// </summary>
        /// <param name="CommunityId">0 o -1 ==> TUTTE!</param>
        /// <returns></returns>
        public bool UserHasTicketAssociationCom(int CommunityId)
        {
            Int64 UserId = this.CurrentUser.Id;

            IEnumerable<Domain.Assignment> Asgn = (from Domain.Assignment ass in Manager.GetIQ<Domain.Assignment>()
                                                   where (ass.Type == Domain.Enums.AssignmentType.Manager
                                                   || ass.Type == Domain.Enums.AssignmentType.Resolver)
                                                   && ass.Ticket != null
                                                   && ass.AssignedCategory != null
                                                   && ass.AssignedTo != null && ass.AssignedTo.Id == UserId
                                                   && ass.Deleted == BaseStatusDeleted.None
                                                   select ass);

            if (CommunityId > 0)
                Asgn = Asgn.Where(ass => ass.AssignedCategory.IdCommunity == CommunityId);
            else
                Asgn = Asgn.Where(ass => ass.AssignedCategory.IdCommunity >= 0);

            Boolean HasPermission = (from Domain.Assignment ass in Asgn select ass.Id).Skip(0).Take(1).ToList().Any();

            return HasPermission;
        }

        /// <summary>
        /// Se SONO l'AUTORE del Ticket: controlla se il ticket indicato è o è stato associato direttamente all'utente
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public bool UserHasManResTicketPermission(Int64 TicketId)
        {
            return UserHasManResTicketPermission(TicketId, this.CurrentUser.Id);
        }

        /// <summary>
        /// Se sono MANAGER o RESOLVER!
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="PersonId"></param>
        /// <returns></returns>
        public bool UserHasTicketPermissionPerson(Int64 TicketId, int PersonId)
        {
            Int64 UsrId = this.UserGetIdfromPerson(PersonId);
            return UserHasManResTicketPermission(TicketId, UsrId);
        }

        /// <summary>
        /// Se sono MANAGER o RESOLVER!
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool UserHasManResTicketPermission(Int64 TicketId, Int64 UserId)
        {
            return UserHasManResTicketPermission(TicketUserTypeGet(TicketId, UserId, 0));
            ////Int64 UserId = this.CurrentUser.Id;
            //Boolean IsCreator = (from Ticket tk in Manager.GetIQ<Ticket>()
            //                         where tk.Id == TicketId && tk.Creator != null && tk.Creator.Id == UserId).Any();

            //IEnumerable<Domain.Assignment> AsgnGeneric = (from Domain.Assignment ass in Manager.GetIQ<Domain.Assignment>()
            //                                              where ass.Ticket != null
            //                                              && ass.Ticket.Id == ass.Ticket.Id
            //                                              select ass);
            ////Utente ASSEGNATO DIRETTAMENTE al Ticket
            //Boolean HasPermission = (from Domain.Assignment ass in AsgnGeneric
            //                         where (ass.Type == Domain.Enums.AssignmentType.Manager
            //                                || ass.Type == Domain.Enums.AssignmentType.Resolver)
            //                                && ass.AssignedTo != null && ass.AssignedTo.Id == UserId
            //                         select ass.Id).Skip(0).Take(1).ToList().Any();

            //if (HasPermission)
            //    return true;

            ////Utente ASSEGNATO ad una categoria
            //IList<Int64> AssCategories = (from Domain.Assignment ass in AsgnGeneric
            //                              where ass.AssignedCategory != null
            //                              select ass.AssignedCategory.Id).Distinct().ToList();

            //foreach (Int64 CtId in AssCategories)
            //{
            //    if (this.CategoryIdManRes.ContainsKey(CtId))
            //        return true;
            //}

            //return false;



        }

        public bool UserHasManResTicketPermission(Domain.Enums.MessageUserType UserType)
        {
            return (UserType == MessageUserType.Manager 
                || UserType == MessageUserType.Resolver
                || UserType == MessageUserType.CategoryManager
                || UserType == MessageUserType.CategoryResolver);
        }

        public bool UserHasAnyAssignment()
        {
            return Manager.GetAll<Assignment>(
                a => a.AssignedTo != null
                     && a.AssignedTo.Person != null
                     && a.AssignedTo.Person.Id != CurrentUser.Id).Any();

            //return UserHasManResTicketPermission(TicketId, this.CurrentUser.Id);
        }

        /// <summary>
        /// Person corrente (per created/modify by)
        /// </summary>
        private lm.Comol.Core.DomainModel.Person CurrentPerson
        {
            get
            {
                if (_currentPerson == null)
                {
                    _currentPerson = Manager.Get<lm.Comol.Core.DomainModel.Person>(UC.CurrentUserID);
                    if (_currentPerson == null)
                    {
                        //_currentPerson = Manager.GetAll<lm.Comol.Core.DomainModel.litePerson>(p => p.TypeID == (int)UserTypeStandard.Guest)
                        //.Skip(0)
                        //.Take(1)
                        //.FirstOrDefault();
                        _currentPerson = Manager.GetUnknownUser();
                    }
                }
                return _currentPerson;
            }
        }
        private lm.Comol.Core.DomainModel.Person _currentPerson { get; set; }

        private lm.Comol.Core.DomainModel.litePerson CurrentLitePerson
        {
            get
            {
                if (_CurrentLitePerson == null)
                    _CurrentLitePerson = Manager.GetLitePerson(UC.CurrentUserID);
                return _CurrentLitePerson;
            }
        }
        private lm.Comol.Core.DomainModel.litePerson _CurrentLitePerson { get; set; }
        private Domain.TicketUser _user;
        /// <summary>
        /// USER corrente (per created/modify by)
        /// </summary>
        private Domain.TicketUser CurrentUser
        {
            get
            {
                if (_user == null)
                {
                    try
                    { _user = this.UserGetfromPerson(UC.CurrentUserID); }
                    catch { }
                }

                if (_user == null)
                {
                    _user = new Domain.TicketUser();
                    _user.Id = -1;
                }


                return _user;
            }
        }

        /// <summary>
        /// Recupera il tipo utente, dato un ticket ed uno user o person ID
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="UserId">ID utente corrente.</param>
        /// <param name="PersonId">ID person corrente</param>
        /// <returns></returns>
        /// <remarks>
        /// UserId o PersonId possono essere usati in modo distinto per individuare se l'utente è creatore del ticket:
        /// Ticket.Creator.Id == UserId || Ticket.CreatedBy.Id == PersonId
        /// </remarks>
        public Domain.Enums.MessageUserType TicketUserTypeGet(Int64 TicketId, Int64 UserId, int PersonId)
        {
            return TicketUserTypeGet(Manager.Get<Ticket>(TicketId), UserId, PersonId);
        }

        /// <summary>
        /// Recupera il tipo utente, dato un ticket ed uno user o person ID
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <param name="UserId">ID utente corrente.</param>
        /// <param name="PersonId">ID person corrente</param>
        /// <returns></returns>
        /// <remarks>
        /// UserId o PersonId possono essere usati in modo distinto per individuare se l'utente è creatore del ticket:
        /// Ticket.Creator.Id == UserId || Ticket.CreatedBy.Id == PersonId
        /// </remarks>
        public Domain.Enums.MessageUserType TicketUserTypeGet(Ticket ticket, Int64 UserId, int PersonId)
        {

            //IList<Domain.Assignment> AsgnGeneric = (from Domain.Assignment ass in Manager.GetIQ<Domain.Assignment>()
            //                                              where ass.Ticket != null
            //                                              && ass.Ticket.Id == Ticket.Id
            //                                              && ass.Deleted == BaseStatusDeleted.None
            //                                              select ass).ToList();


            //Assegnazioni diretta al Ticket
            IList<Domain.Assignment> AsgnGeneric = (from Assignment ass in ticket.Assignemts 
                                where ass.Deleted == BaseStatusDeleted.None
                                && ass.AssignedTo != null && ass.AssignedTo.Id == UserId
                                select ass).ToList();

            //Tra le associazioni: Manager
            Boolean tkManager = (from Assignment ass in AsgnGeneric 
                                 where ass.Type == Domain.Enums.AssignmentType.Manager 
                                 select ass.Id).Any();

            if (tkManager)
                return Domain.Enums.MessageUserType.Manager;

            //Tra le associazioni: Resolver
            Boolean tkResolver = (from Assignment ass in AsgnGeneric 
                                  where ass.Type == Domain.Enums.AssignmentType.Resolver
                                  select ass.Id).Any();

            if (tkResolver)
                return Domain.Enums.MessageUserType.Resolver;


            //Tra le categorie!
            IList<Int64> TkCateIds = (from Assignment ass in ticket.Assignemts
                                      where ass.Type == Domain.Enums.AssignmentType.Category 
                                      && ass.Deleted == BaseStatusDeleted.None
                                      select ass.AssignedCategory.Id).Distinct().ToList();


            if (TkCateIds != null && TkCateIds.Any())
            {
                Boolean IsManager = false;
                Boolean IsResolver = false;

                foreach (Int64 CateId in TkCateIds)
                {
                    if (this.CategoryIdManRes.ContainsKey(CateId))
                    {
                        IsResolver = true;
                        IsManager = IsManager || this.CategoryIdManRes[CateId];
                        if (IsManager)
                            return Domain.Enums.MessageUserType.CategoryManager;
                    }
                }
                if (IsResolver)
                    return Domain.Enums.MessageUserType.CategoryResolver;
            }

            if (ticket.Owner != null && ticket.Owner.Id == UserId
                || ticket.CreatedBy.Id == PersonId)
                return Domain.Enums.MessageUserType.Partecipant;

            return Domain.Enums.MessageUserType.none;
        }

        /// <summary>
        /// Aggiorna l'ultimo accesso del Manager/Resolver CORRENTE ad un ticket
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <remarks>
        /// Aggiornato in fase di:
        /// Get del Ticket,
        /// Invio Messaggio Ticket,
        /// Mostra/nascondi messaggio,
        /// (opzionale) Cambio stato - SOLO pagina modifica!
        /// </remarks>
        public DateTime? UserAccessUpdate(Int64 TicketId)
        {
            DateTime? LastAccess = null;

            Int64 UserId = this.CurrentUser.Id;
            if (UserId > 0 && TicketId > 0)
            {
                Domain.liteManResTicketAccess Access = Manager.GetAll<Domain.liteManResTicketAccess>(ta => ta.TicketId == TicketId && ta.UserId == UserId).FirstOrDefault();

                if (Access == null)
                {
                    Access = new liteManResTicketAccess();
                    Access.TicketId = TicketId;
                    Access.UserId = UserId;
                }
                else
                {
                    LastAccess = Access.LastAccess;
                }

                Access.LastAccess = DateTime.Now;

                Manager.SaveOrUpdate<Domain.liteManResTicketAccess>(Access);
            }

            return LastAccess;
        }



        /// <summary>
        /// Recupera la data di accesso sell'utente (Manager o Resolver) corrente al Ticket
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public DateTime UserAccessGet(Int64 TicketId)
        {
            Int64 UserId = this.CurrentUser.Id;
            if (UserId > 0 && TicketId > 0)
            {
                Domain.liteManResTicketAccess Access = Manager.GetAll<Domain.liteManResTicketAccess>(ta => ta.TicketId == TicketId && ta.UserId == UserId).FirstOrDefault();

                if (Access != null)
                    return Access.LastAccess;

            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Controlla se l'utente corrente è amministratore di sistema
        /// </summary>
        /// <returns></returns>
        public Boolean PersonCurrentIsSysAdmin()
        {
            if (UC.isAnonymous || UC.CurrentUser == null)
            {
                return false;
            }
            else
            {
                int CurrentTypeID = (from Person p in Manager.GetIQ<Person>() where p.Id == UC.CurrentUserID select p.TypeID).Skip(0).Take(1).FirstOrDefault();
                return (CurrentTypeID == (int)UserTypeStandard.SysAdmin);
            }
        }

        /// <summary>
        /// Recupera l'elenco dei USER(DTO) che sono associati come manager o resolver a categorie della comunità
        /// </summary>
        /// <param name="CommunityId">L'ID della comunità</param>
        /// <returns>La relativa lista di utenti</returns>
        public IList<Domain.DTO.DTO_User> UserCommunityManResGet(int CommunityId)
        {
            IList<Domain.DTO.DTO_User> DtoUsers = (from LK_UserCategory lkuc in Manager.GetIQ<LK_UserCategory>()
                                                   where lkuc.Category != null &&
                                                   lkuc.User != null &&
                                                   lkuc.Category.IdCommunity == CommunityId
                                                   select new Domain.DTO.DTO_User
                                                   {
                                                       UserId = lkuc.User.Id,
                                                       LanguageCode = lkuc.User.LanguageCode,
                                                       PersonId = (lkuc.User.Person != null) ? lkuc.User.Person.Id : -1,
                                                       Mail = (lkuc.User.Person != null) ? lkuc.User.Person.Mail : lkuc.User.mail,
                                                       Name = (lkuc.User.Person != null) ? lkuc.User.Person.Name : lkuc.User.Name,
                                                       SName = (lkuc.User.Person != null) ? lkuc.User.Person.Surname : lkuc.User.Sname
                                                   }).ToList();

            return DtoUsers;
        }

        public Domain.Enums.AccessRecoverError UserRecover(String Mail,
            Domain.DTO.DTO_NotificationSettings Settings)
        {
            if (!MailCheckFormat(Mail))
                return Domain.Enums.AccessRecoverError.MailFormat;

            Domain.TicketUser User = Manager.GetAll<Domain.TicketUser>(u => u.mail == Mail || u.Person != null && u.Person.Mail == Mail).FirstOrDefault();

            if (User == null || User.Id <= 0)
                return Domain.Enums.AccessRecoverError.MailNotFound;

            if (!User.MailChecked)
                return Domain.Enums.AccessRecoverError.MailNotChecked;

            //Setto nuova password per l'utente
            String Pwd = PasswordGetNew();
            String EncodedPwd = AuthenticationHelper.Encrypt(Pwd);


            User.Code = EncodedPwd;
            User.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            Manager.SaveOrUpdate<Domain.TicketUser>(User);

            if (!NotificationSendRecover(User, Settings, Pwd))
                return Domain.Enums.AccessRecoverError.InternalError;


            return Domain.Enums.AccessRecoverError.none;
        }

        public Person PersonGet(Int32 Id)
        {
            return Manager.GetPerson(Id);
        }





        private litePerson _PersonUnknow = null;

        //private Int32 PersonTypeExternalId
        //{
        //    get
        //    {
        //        if (_PersonUnknow == null)
        //            _PersonUnknow = Manager.GetLiteUnknownUser();

        //        return _PersonUnknow.TypeID;
        //    }
        //}

        //private Int32 PersonExternalId
        //{
        //    get
        //    {
        //        if (_PersonUnknow == null)
        //            _PersonUnknow = Manager.GetLiteUnknownUser();

        //        return _PersonUnknow.Id;
        //    }
        //}

        private litePerson PersonExternal
        {
            get
            {
                if (_PersonUnknow == null)
                    _PersonUnknow = Manager.GetLiteUnknownUser();

                return _PersonUnknow;
            }
        }



        /////// X NOTIFICATION!!!

        public Domain.DTO.Notification.DTO_UserNotificationData UserGetNotificationData(TicketUser user)
        {
            if (user == null || (!user.MailChecked && user.Person == null) || !user.Enabled)
                return null;

            Domain.DTO.Notification.DTO_UserNotificationData data = new DTO_UserNotificationData();

            data.Channel = NotificationChannel.Mail;

            data.LanguageCode = (user.Person == null) ? user.LanguageCode : this.Manager.GetLanguageByIdOrDefault(user.Person.LanguageID).Code;
            //data.LanguageCode = user.LanguageCode;
                
            
            data.UserId = user.Id;

            if (user.Person != null)
            {
                data.ChannelAddress = user.Person.Mail;
                data.FullUserName = user.Person.SurnameAndName;
                data.PersonId = user.Person.Id;
            }
            else
            {
                data.ChannelAddress = string.Format("{0} {1}", user.Sname, user.Name);
                data.FullUserName = user.mail;
                data.PersonId = -1;
            }


            //NOTA DATI FARLOCCHI!
            //RETURN VALORI OK con configurazione!


            return data;
        }
    }
}
