using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
//using lm.Comol.Core.BaseModules.Tickets;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class TicketAccessPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
          
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewTicketAccess View
        {
            get { return (View.iViewTicketAccess)base.View; }
        }

        public TicketAccessPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public TicketAccessPresenter(iApplicationContext oContext, View.iViewTicketAccess view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

    #endregion

        public bool InitView()
        {
            Domain.DTO.DTO_Access Access = service.SettingsAccessGet(false);

            //Non attivato Oppure
            //Non può vedere Ticket
            
            return true;


            if (!Access.IsActive || !Access.CanShowTicket)
            {
                View.ShowServiceDisabled();
                return false;
            }
            return true;
            //Boolean HasToken = false;
            //String Token = View.UrlToken;
            //if (!String.IsNullOrEmpty(Token))
            //{
            //    //Check token
            //    Guid Code = System.Guid.Empty;
            //    try
            //    {
            //        Code = new System.Guid(Token);
            //    } catch {
                    
            //    }

            //    if(Code != null && Code != Guid.Empty)
            //    {
            //        Domain.Token Tok = service.TokenGet(Code);
            //        HasToken = true;
            //    }
            //}

            //View.DisplayLogin(Domain.Enums.LoginStatus.normal); //, HasToken);
            //TEMPORANEO
            //IList<Domain.DTO.DTO_TkLanguage> Languages = new List<Domain.DTO.DTO_TkLanguage>();

            //Languages.Add(new Domain.DTO.DTO_TkLanguage { Id = 1, Code = "it-IT", DisplayName = "Italiano" });
            //Languages.Add(new Domain.DTO.DTO_TkLanguage { Id = 2, Code = "en-US", DisplayName = "English" });
            //Languages.Add(new Domain.DTO.DTO_TkLanguage { Id = 3, Code = "de-DE", DisplayName = "Deutsch" });

            //View.InitDDL(Languages);
        }


        #region Access
        /// <summary>
        /// Accede alla pagina
        /// </summary>
        /// <param name="Mail">Mail utente</param>
        /// <param name="Password">Password accesso utente</param>
        /// In futuro potenzialmente usata come chiave per l'utilizzo via Web Service</param>
        /// <returns></returns>
        public void Enter(
            String Mail,
            String Password,
            String Token)
        {
            if (String.IsNullOrEmpty(Token))
                Token = View.UrlToken;

            Domain.Enums.ExternalUserValidateError err = Domain.Enums.ExternalUserValidateError.none;
            Domain.DTO.DTO_User User = service.UserValidateExternal(Mail, Password, Token, ref err);
            
            if (err == Domain.Enums.ExternalUserValidateError.none)
            {
                View.SetCurrentUser(User);
                RedirectAfterLogin(User.UserId);
            }
            else if ((err == Domain.Enums.ExternalUserValidateError.TokenEmpty ||
                err == Domain.Enums.ExternalUserValidateError.TokenExpired ||
                err == Domain.Enums.ExternalUserValidateError.TokenInvalid)
                && User != null && User.UserId > 0)
            {
                View.SetCurrentUser(User);
                View.ShowAccessError(err);
            }
            else
            {
                View.ShowAccessError(err);
            }
        }

        #endregion

        #region Subscribe

        /// <summary>
        /// Iscrive l'utente al sistema Ticket
        /// </summary>
        /// <param name="Mail">Mail dell'utente</param>
        /// <param name="Name">Nome dell'utente</param>
        /// <param name="SName">Cognome dell'utente</param>
        /// <param name="LanguageCode">Codice lingua scelta</param>
        public void Register(
            String Mail,
            String Name,
            String SName,
            String LanguageCode,
            String Password)
        {

            Domain.DTO.DTO_ExtUserAddResult Result = service.UserCreateFromExternal(Name, SName, Mail, LanguageCode, Password, View.Settings);

            if (Result.Errors == Domain.Enums.ExternalUserCreateError.none || Result.Errors == Domain.Enums.ExternalUserCreateError.TicketMail)
            {

                if (Result.User.UserId > 0)
                    View.SetCurrentUser(Result.User);
            }

            View.ShowRegistered(Result);

        }

        //public Domain.Enums.ExternalUserCreateError CheckMail(String mail)
        //{
        //    Domain.Enums.ExternalUserCreateError result = Domain.Enums.ExternalUserCreateError.none;
        //    aaa
        //    return result;
        //}

        #endregion

        #region RecoverCode


        public void Recover(
            String Mail
            )
        {
            Domain.Enums.AccessRecoverError err = service.UserRecover(Mail, View.Settings);//Domain.Enums.AccessRecoverError.none;

            if (err == Domain.Enums.AccessRecoverError.none)
            {
                View.ShowView(Domain.Enums.AccessView.recoverRequestSended);
            }
            else if (err == Domain.Enums.AccessRecoverError.InternalError)
            {
                View.ShowView(Domain.Enums.AccessView.recoverRequestSended);
                View.ShowRecoverError(err);
            }
            else
            {
                View.ShowRecoverError(err);
            }
        }

        #endregion

        #region Token
        public void TokenValidate(
            String Token,
            Int64 CurrentUserId)
        {
            //NOTA: l'utente deve essere già presente in sessione!

            Domain.Enums.TokenValidationResult result = service.TokenValidate(Token, CurrentUserId, Domain.Enums.TokenType.Registration);
            if (result == Domain.Enums.TokenValidationResult.Validated)
            {
                //View.SetCurrentUser(User);

                RedirectAfterLogin(CurrentUserId);
                //View.RedirectToCreate(); //rivedere
            }
            else
            {
                View.ShowValidationError(result);
            }
        }

        private void RedirectAfterLogin(Int64 CurrentUserId)
        {
            int TkCount = service.UserGetTicketNum(CurrentUserId);
            if (TkCount > 1)
                View.RedirectToList();
            else if (TkCount > 0)
                View.RedirectToTicket(service.UserGetTicketID(CurrentUserId));
            else
                View.RedirectToCreate();
        }
        #endregion

        
    }
}
