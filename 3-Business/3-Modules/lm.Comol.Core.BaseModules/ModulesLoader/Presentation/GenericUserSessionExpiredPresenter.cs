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
    public class GenericUserSessionExpiredPresenter: lm.Comol.Core.DomainModel.Common.DomainPresenter
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
        protected virtual IViewGenericUserSessionExpired View
        {
            get { return (IViewGenericUserSessionExpired)base.View; }
        }

        public GenericUserSessionExpiredPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public GenericUserSessionExpiredPresenter(iApplicationContext oContext, IViewGenericUserSessionExpired view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView(Int32 idPerson, dtoExpiredAccessUrl expiredInfo)
        { 
            Person person = null;
            if (idPerson > 0)
                person = CurrentManager.GetPerson(idPerson);
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
    }
}