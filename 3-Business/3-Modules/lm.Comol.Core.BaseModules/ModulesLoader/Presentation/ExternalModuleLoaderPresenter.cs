using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;

namespace lm.Comol.Core.BaseModules.ModulesLoader.Presentation
{
    public class ExternalModuleLoaderPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private ExternalAuthenticationService _Service;
        private ExternalAuthenticationService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ExternalAuthenticationService(AppContext);
                return _Service;
            }
        }

        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewExternalModuleLoader View
        {
            get { return (IViewExternalModuleLoader)base.View; }
        }

        public ExternalModuleLoaderPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public ExternalModuleLoaderPresenter(iApplicationContext oContext, IViewExternalModuleLoader view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView(DateTime dateTimeToVerify){
            String moduleCode = View.PreLoadedModuleCode;
            String modulePage = View.PreloadedModulePage;
            String externalProviderCode = View.PreLoadedExternalSource;
            String encodedToken = "";

            Authentication.AuthenticationProvider provider = Service.GetActiveProvider(externalProviderCode);

            if (provider == null)
                View.UnknowAuthenticationProvider();
            else if (provider.ProviderType == Authentication.AuthenticationProviderType.Url)
            {
                UrlAuthenticationProvider urlProvider = (UrlAuthenticationProvider)provider;
                encodedToken = View.GetEncodedIdUser(urlProvider.UrlIdentifier);
                UrlProviderResult response = UrlProviderResult.NotEvaluatedToken;
                if (encodedToken == "")
                    response = UrlProviderResult.UnknowToken;
                else
                    response = urlProvider.ValidateToken(encodedToken, View.PreloadedPreviousUrl, dateTimeToVerify);
                switch (response)
                {
                    case UrlProviderResult.ValidToken:
                        string decodedToken = urlProvider.GetTokenIdentifier(encodedToken);
                        LoadUser(decodedToken, urlProvider);
                        break;
                    //case UrlProviderResult.UnknowToken:
                    //    View.showLogonInfo(UrlProviderResult.
                    //    break;
                    default:
                        View.showLogonInfo(response, urlProvider.RemoteLoginUrl);
                        break;
                }
            }
        }

        private void LoadUser(string userRemoteID, AuthenticationProvider provider)
        {
            List<ExternalLoginInfo> infos = Service.FindUserByIdentifier(userRemoteID, provider);
            if (infos == null || infos.Count == 0 || (from i in infos where i.Deleted == BaseStatusDeleted.None select i).ToList().Count == 0)
                View.ShowAuthenticationResult(AuthenticationResult.UserNotFound);
            else { 
                ExternalLoginInfo authenticatedUser = (from au in infos where au.Deleted== BaseStatusDeleted.None select au).FirstOrDefault();
                if (authenticatedUser == null)
                    View.ShowAuthenticationResult(AuthenticationResult.UserNotFound);
                else if (authenticatedUser != null && !authenticatedUser.isEnabled )
                    View.ShowAuthenticationResult(AuthenticationResult.UserDisabled);
                else
                    PreLoadModule(authenticatedUser.Person, View.PreLoadedExternalID, provider.UniqueCode);
            }
        }

        private void PreLoadModule(Person authenticatedUser,String externalIdCommunity, String providerCode)
        { 
            ExternalCommunityInfo externalCommunity = Service.FindExternalCommunityByIdentifier(View.PreLoadedExternalID,providerCode);
            if (externalCommunity == null && string.IsNullOrEmpty(View.PreLoadedExternalID))
                View.LoadWaitingMessage(authenticatedUser, View.PortalHome);
            else if (externalCommunity != null)
            {
                Community community = CurrentManager.GetCommunity(externalCommunity.IdCommunity);
                if (community == null)
                    View.LoadUnknowCommunity();
                else if (CurrentManager.HasActiveSubscription(authenticatedUser.Id,community.Id)){
                    View.LoadWaitingMessage(authenticatedUser, community.Name);
                    String destinationUrl = Service.GetModuleUrlByIdentifier(community.Id, authenticatedUser.Id, authenticatedUser.LanguageID, View.PreLoadedModuleCode, View.PreloadedModulePage);
                    if (UserContext.isAnonymous)
                        View.LoadModuleWithLogon(authenticatedUser, community.Id, destinationUrl);
                    else
                        View.LoadModule(community.Id, destinationUrl);
                }
                else
                    View.LoadUnsubscribedCommunity(community.Name);
            }
            else
                View.LoadUnknowCommunity();
        }
    }
}