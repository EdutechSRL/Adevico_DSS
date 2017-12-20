using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public class UserLogoutPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
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
            protected virtual IViewUserLogout View
            {
                get { return (IViewUserLogout)base.View; }
            }

            public UserLogoutPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public UserLogoutPresenter(iApplicationContext oContext, IViewUserLogout view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean defaultPageForInternal) { 
            Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
            {
                List<AuthenticationProvider> providers = ProfileService.GetUserAuthenticationProviders(person);
                if (providers.Count == 0 || person.IdDefaultProvider == 0 || !providers.Where(p => p.Id == person.IdDefaultProvider).Any())
                {
                    //if (IsShibbolethSessionActive())

                    ////if (person.AuthenticationTypeID != 1)
                    ////    View.LoadOldAuthenticationPage(person.AuthenticationTypeID);
                    ////else
                    //    View.GoToDefaultPage();
                    LoadData(LogoutMode.logoutMessage, AuthenticationProviderType.ActiveDirectory, "");
                }
                else
                {

                    lm.Comol.Core.DomainModel.Helpers.dtoLoginCookie userInfo= View.UserAccessInfo;
                    AuthenticationProvider provider = providers.Where(p => p.Id == userInfo.IdProvider).FirstOrDefault();
                    if (provider==null)
                        provider = providers.Where(p => p.Id == person.IdDefaultProvider).FirstOrDefault();

                    AuthenticationProviderType providerType = (provider == null) ? AuthenticationProviderType.None : provider.ProviderType;

                    switch (providerType)
                    {
                        case AuthenticationProviderType.Internal:
                            if (defaultPageForInternal || provider.LogoutMode== LogoutMode.portalPage)
                                LoadData(LogoutMode.portalPage, providerType, "");
                            else
                                LoadData(provider.LogoutMode, providerType, "");
                            break;
                        case AuthenticationProviderType.Url:
                            UrlAuthenticationProvider urlProvider = (UrlAuthenticationProvider)provider;
                            LoadData(provider.LogoutMode, providerType, urlProvider.RemoteLoginUrl);
                            break;
                        case AuthenticationProviderType.UrlMacProvider:
                            MacUrlAuthenticationProvider mProvider = (MacUrlAuthenticationProvider)provider;
                            LoadData(provider.LogoutMode, providerType, mProvider.RemoteLoginUrl);
                            break;
                        default:
                            LoadData(LogoutMode.portalPage, providerType, "");
                            break;
                    }
                }
            }
            else
                View.GoToDefaultPage();
        }
        private void LoadData(LogoutMode mode, AuthenticationProviderType type, String url)
        {
            switch (mode) {
                case LogoutMode.externalPage:
                    View.LoadExternalProviderPage(url);
                    break;
                case LogoutMode.internalLogonPage:
                    View.LoadInternalLoginPage();
                    break;
                case LogoutMode.logoutMessageAndUrl:
                case LogoutMode.logoutMessageAndClose:
                case LogoutMode.logoutMessage:
                    View.LoadLogoutMessage(mode,type,url);
                    break;
                case LogoutMode.portalPage:
                     View.GoToDefaultPage();
                     break;
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
    }
}