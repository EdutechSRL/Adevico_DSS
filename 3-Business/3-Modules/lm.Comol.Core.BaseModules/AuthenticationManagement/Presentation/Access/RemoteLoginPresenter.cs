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
    public class RemoteLoginPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private InternalAuthenticationService _InternalService;
            private UrlAuthenticationService _UrlService;
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService _PolicyService;
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
            protected virtual IViewRemoteLogin View
            {
                get { return (IViewRemoteLogin)base.View; }
            }
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService PolicyService
            {
                get
                {
                    if (_PolicyService == null)
                        _PolicyService = new lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService(AppContext);
                    return _PolicyService;
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
            private InternalAuthenticationService InternalService
            {
                get
                {
                    if (_InternalService == null)
                        _InternalService = new InternalAuthenticationService(AppContext);
                    return _InternalService;
                }
            }
            public RemoteLoginPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RemoteLoginPresenter(iApplicationContext oContext, IViewRemoteLogin view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean accessAvailable = !(View.isSystemOutOfOrder && !View.AllowAdminAccess);
            View.AllowAuthentication = accessAvailable;
            View.AllowSubscription = !View.isSystemOutOfOrder;
            if (!accessAvailable)
                View.DisplaySystemOutOfOrder();
            else
            {
                dtoUrlToken urlToken = null;
                if (View.HasUrlValues)
                    urlToken = View.GetUrlToken(UrlService.GetActiveUrlIdentifiers());
                if (urlToken != null && !String.IsNullOrEmpty(urlToken.Value))
                    UrlProviderLogon(urlToken);
            }
        }

        private void UrlProviderLogon(dtoUrlToken urlToken)
        {
            UrlAuthenticationProvider urlProvider = UrlService.GetPovider(urlToken.Identifier);
            if (urlProvider != null)
            {
                //UrlProviderResult result = urlProvider.ValidateToken(urlToken.Value);
                dtoUrlToken vToken = urlProvider.ValidateToken(urlToken);
                if (!String.IsNullOrEmpty(urlProvider.RemoteLoginUrl))
                        View.SetExternalWebLogonUrl(urlProvider.RemoteLoginUrl);
                    else if (!String.IsNullOrEmpty(urlProvider.SenderUrl))
                        View.SetExternalWebLogonUrl(urlProvider.SenderUrl);

                //urlToken.DecriptedValue = urlProvider.GetTokenIdentifier(urlToken.Value);
                List<ExternalLoginInfo> users = UrlService.FindUserByIdentifier(vToken.DecriptedValue, urlProvider);
                switch (vToken.Evaluation.Result)
                {
                    case UrlProviderResult.ValidToken:
                        if (users.Count == 1)
                            ExternalLogonManage(users[0], urlProvider);
                        else if (!String.IsNullOrEmpty(vToken.DecriptedValue))
                            View.GoToProfile(urlProvider.Id, vToken, lm.Comol.Core.BaseModules.ProfileManagement.RootObject.UrlProfileWizard(urlProvider.Id));
                        break;
                    default:
                        int idPerson = (users.Count == 1 && users[0].Person != null) ? users[0].Person.Id : 0;
                        String tokenUrl = RootObject.InvalidToken(idPerson, urlProvider.Id, vToken.Evaluation.Result);
                        //urlToken.FullDecriptedValue = urlProvider.FullDecryptToken(urlToken.Value);
                        View.DisplayInvalidToken(tokenUrl, idPerson, vToken, vToken.Evaluation.Result);
                        //if (!String.IsNullOrEmpty(urlProvider.RemoteLoginUrl))
                        //    View.GotoRemoteUrl(urlProvider.RemoteLoginUrl);
                        //else if (!String.IsNullOrEmpty(urlProvider.SenderUrl))
                        //    View.GotoRemoteUrl(urlProvider.SenderUrl);
                        break;
                }
            }

        }
        private void ExternalLogonManage(ExternalLoginInfo userInfo, AuthenticationProvider provider)
        {
            String wizardUrl = "";
            String defaultUrl = "";
            Boolean internalPage = true;
            switch (provider.ProviderType)
            {
                case AuthenticationProviderType.Url:
                    wizardUrl = lm.Comol.Core.BaseModules.ProfileManagement.RootObject.UrlProfileWizard(provider.Id);
                    internalPage = false;
                    defaultUrl = ((UrlAuthenticationProvider)provider).RemoteLoginUrl;
                    break;

            }

            if (userInfo.Person == null)
                View.GoToProfile(wizardUrl);
            else if (!userInfo.isEnabled || userInfo.Person.isDisabled)
                View.DisplayAccountDisabled(lm.Comol.Core.BaseModules.ProfileManagement.RootObject.DisabledProfile(provider.Id, userInfo.Person.Id));
            else
            {
               InternalService.UpdateUserAccessTime(userInfo.Person);
                if (userInfo.Person.AcceptPolicy || !PolicyService.UserHasPolicyToAccept(userInfo.Person))
                    View.LogonUser(userInfo.Person, provider.Id, defaultUrl, internalPage, CurrentManager.GetUserDefaultIdOrganization(userInfo.Person.Id));
                else
                    View.DisplayPrivacyPolicy(userInfo.Person.Id, provider.Id, defaultUrl, internalPage);
            }

        }
    }
}