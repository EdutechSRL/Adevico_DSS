using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ModulesLoader.Presentation
{
    public class UserSessionExpiredPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private ExternalAuthenticationService _Service;
        private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService _ProfileService;
        private ExternalAuthenticationService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ExternalAuthenticationService(AppContext);
                return _Service;
            }
        }
        private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService ProfileService
        {
            get
            {
                if (_ProfileService == null)
                    _ProfileService = new lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(AppContext);
                return _ProfileService;
            }
        }

        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewUserSessionExpired View
        {
            get { return (IViewUserSessionExpired)base.View; }
        }

        public UserSessionExpiredPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public UserSessionExpiredPresenter(iApplicationContext oContext, IViewUserSessionExpired view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView() { 
        
        }
        public void InitView(dtoExpiredAccessUrl expiredInfo) { 
            Person person = null;
            if (expiredInfo.IdPerson > 0)
                person = CurrentManager.GetPerson(expiredInfo.IdPerson);
            InitializeLanguage(person,expiredInfo.CodeLanguage);
            View.UnknownAccessTicket = expiredInfo;
            View.WriteLogonCookies(expiredInfo);
            if (person == null)
                View.GoToDefaultPage();
            else { 
                List<AuthenticationProvider> providers = ProfileService.GetUserAuthenticationProviders(person);
                if (providers.Count == 0 || person.IdDefaultProvider == 0 || !providers.Where(p => p.Id == person.IdDefaultProvider).Any())
                {
                    //if (person.AuthenticationTypeID != 1)
                    //    View.LoadOldAuthenticationPage(person.AuthenticationTypeID);
                    //else
                        View.GoToDefaultPage();
                }
                else
                {
                    AuthenticationProvider provider = providers.Where(p => p.Id == person.IdDefaultProvider).FirstOrDefault();
                    switch (provider.ProviderType)
                    {
                        case AuthenticationProviderType.Internal:
                            View.LoadInternalLoginPage();
                            break;
                        case AuthenticationProviderType.Url:
                            UrlAuthenticationProvider urlProvider = (UrlAuthenticationProvider)provider;
                            View.LoadExternalProviderPage(urlProvider.RemoteLoginUrl);
                            break;
                        default:
                            View.GoToDefaultPage();
                            break;
                    }
                }
            }
        }


        private void InitializeLanguage(Person person, String code)
        {
            Language language = null;
           
            if (person != null)
                language = CurrentManager.GetLanguage(person.LanguageID);
            else
                language = CurrentManager.GetLanguage(code);
            if (language == null)
                language = CurrentManager.GetDefaultLanguage();


            View.LoadLanguage(language);
        }
        //public void InitView(DateTime dateTimeToVerify){
        //    String moduleCode = View.PreLoadedModuleCode;
        //    String modulePage = View.PreloadedModulePage;
        //    String externalProviderCode = View.PreLoadedExternalSource;
        //    String encodedToken = "";

        //    Authentication.AuthenticationProvider provider = Service.GetActiveProvider(externalProviderCode);

        //    if (provider == null)
        //        View.UnknowAuthenticationProvider();
        //    else if (provider.ProviderType == Authentication.AuthenticationProviderType.Url)
        //    {
        //        UrlAuthenticationProvider urlProvider = (UrlAuthenticationProvider)provider;
        //        encodedToken = View.GetEncodedIdUser(urlProvider.UrlIdentifier);
        //        UrlProviderResult response = UrlProviderResult.NotEvaluatedToken;
        //        if (encodedToken == "")
        //            response = UrlProviderResult.UnknowToken;
        //        else
        //            response = urlProvider.ValidateToken(encodedToken, View.PreloadedPreviousUrl, dateTimeToVerify);
        //        switch (response)
        //        {
        //            case UrlProviderResult.ValidToken:
        //                string decodedToken = urlProvider.GetTokenIdentifier(encodedToken);
        //                LoadUser(decodedToken, urlProvider);
        //                break;
        //            //case UrlProviderResult.UnknowToken:
        //            //    View.showLogonInfo(UrlProviderResult.
        //            //    break;
        //            default:
        //                View.showLogonInfo(response, urlProvider.RemoteLoginUrl);
        //                break;
        //        }
        //    }
        //}

        //private void LoadUser(string userRemoteID, AuthenticationProvider provider)
        //{
        //    List<ExternalLoginInfo> infos = Service.FindUserByIdentifier(userRemoteID, provider);
        //    if (infos == null || infos.Count == 0 || (from i in infos where i.Deleted == BaseStatusDeleted.None select i).ToList().Count == 0)
        //        View.ShowAuthenticationResult(AuthenticationResult.UserNotFound);
        //    else { 
        //        ExternalLoginInfo authenticatedUser = (from au in infos where au.Deleted== BaseStatusDeleted.None select au).FirstOrDefault();
        //        if (authenticatedUser == null)
        //            View.ShowAuthenticationResult(AuthenticationResult.UserNotFound);
        //        else if (authenticatedUser != null && !authenticatedUser.isEnabled )
        //            View.ShowAuthenticationResult(AuthenticationResult.UserDisabled);
        //        else
        //            PreLoadModule(authenticatedUser.Person, View.PreLoadedExternalID, provider.UniqueCode);
        //    }
        //}

        //private void PreLoadModule(Person authenticatedUser,String externalIdCommunity, String providerCode)
        //{ 
        //    ExternalCommunityInfo externalCommunity = Service.FindExternalCommunityByIdentifier(View.PreLoadedExternalID,providerCode);
        //    if (externalCommunity == null && string.IsNullOrEmpty(View.PreLoadedExternalID))
        //        View.LoadWaitingMessage(authenticatedUser, View.PortalHome);
        //    else if (externalCommunity != null)
        //    {
        //        Community community = CurrentManager.GetCommunity(externalCommunity.IdCommunity);
        //        if (community == null)
        //            View.LoadUnknowCommunity();
        //        else if (CurrentManager.HasActiveSubscription(authenticatedUser.Id,community.Id)){
        //            View.LoadWaitingMessage(authenticatedUser, community.Name);
        //            String destinationUrl = Service.GetModuleUrlByIdentifier(community.Id, authenticatedUser.Id, authenticatedUser.LanguageID, View.PreLoadedModuleCode, View.PreloadedModulePage);
        //            if (UserContext.isAnonymous)
        //                View.LoadModuleWithLogon(authenticatedUser, community.Id, destinationUrl);
        //            else
        //                View.LoadModule(community.Id, destinationUrl);
        //        }
        //        else
        //            View.LoadUnsubscribedCommunity(community.Name);
        //    }
        //    else
        //        View.LoadUnknowCommunity();
        //}
    }
}