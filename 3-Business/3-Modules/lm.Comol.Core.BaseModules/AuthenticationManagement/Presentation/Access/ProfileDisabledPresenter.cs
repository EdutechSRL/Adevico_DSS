using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public class ProfileDisabledPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;

            private CoreAuthenticationsService _CoreService;
            private UrlAuthenticationService _UrlService;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewProfileDisabled View
            {
                get { return (IViewProfileDisabled)base.View; }
            }
            private CoreAuthenticationsService Service
            {
                get
                {
                    if (_CoreService == null)
                        _CoreService = new CoreAuthenticationsService(AppContext);
                    return _CoreService;
                }
            }
            private UrlAuthenticationService UrlService
            {
                get
                {
                    if (_UrlService == null)
                        _UrlService = new UrlAuthenticationService(AppContext);
                    return _UrlService;
                }
            }
            public ProfileDisabledPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProfileDisabledPresenter(iApplicationContext oContext, IViewProfileDisabled view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean accessAvailable = !(View.isSystemOutOfOrder);

            Int32 idProfile = View.PreloadedIdProfile;
            long idProvider = View.PreloadedIdProvider;

            View.IdProfile = idProfile;
            View.IdProvider = idProvider;
            if (idProfile == 0)
                View.GotoAuthenticationSelctorPage();
            else
            {
                AuthenticationProvider provider = CurrentManager.Get<AuthenticationProvider>(idProvider);
                Person person = CurrentManager.GetPerson(idProfile);
                if (person == null)
                    View.GotoAuthenticationSelctorPage();
                else {
                    switch (provider.ProviderType) { 
                        case AuthenticationProviderType.Internal:
                            View.AllowInternalAuthentication = true && accessAvailable;
                            break;
                        case AuthenticationProviderType.Url:
                            UrlAuthenticationProvider urlProvider = (UrlAuthenticationProvider)provider;
                            if (!string.IsNullOrEmpty(urlProvider.RemoteLoginUrl))
                                View.AllowExternalWebAuthentication(urlProvider.RemoteLoginUrl);
                            else if (!string.IsNullOrEmpty(urlProvider.SenderUrl))
                                View.AllowExternalWebAuthentication(urlProvider.SenderUrl);
                            break;
                    }

                    if (Service.IsProfileWaitingForActivation(person))
                        View.DisplayNotActivatedAccount(person.Name);
                    else
                        View.DisplayDisabledAccount(person.Name);
                }
            }
        }
    }
}