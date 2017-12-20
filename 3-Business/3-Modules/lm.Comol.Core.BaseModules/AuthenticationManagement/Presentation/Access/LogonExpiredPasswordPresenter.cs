using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public class LogonExpiredPasswordPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private InternalAuthenticationService _InternalService;
            private UrlMacAuthenticationService _UrlMacService;
            private UrlAuthenticationService _UrlService;
            private ProfileManagement.Business.ProfileManagementService _ProfileService;
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService _PolicyService;
            private int _ModuleID;
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
            protected virtual IViewLogonExpiredPassword View
            {
                get { return (IViewLogonExpiredPassword)base.View; }
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
            private InternalAuthenticationService InternalService
            {
                get
                {
                    if (_InternalService == null)
                        _InternalService = new InternalAuthenticationService(AppContext);
                    return _InternalService;
                }
            }
            private UrlMacAuthenticationService UrlMacService
            {
                get
                {
                    if (_UrlMacService == null)
                        _UrlMacService = new UrlMacAuthenticationService(AppContext);
                    return _UrlMacService;
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
            private ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (_ProfileService == null)
                        _ProfileService = new ProfileManagement.Business.ProfileManagementService(AppContext);
                    return _ProfileService;
                }
            }
            public LogonExpiredPasswordPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public LogonExpiredPasswordPresenter(iApplicationContext oContext, IViewLogonExpiredPassword view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Int32 idUser = View.PreloggedUserId;
            Person person = CurrentManager.GetPerson(idUser);

            if (person == null)
            {
                View.PreloggedUserId = 0;
                View.PreloggedProviderId = 0;
                View.GotoInternalAuthenticationPage();
            }
            else if (InternalService.ExpiredPassword(person))
            {
                View.LoggedUserId = idUser;
                View.LoggedProviderId = View.PreloggedProviderId;
                lm.Comol.Core.Authentication.InternalLoginInfo loginInfo = InternalService.GetLoginInfo(person);
                if (loginInfo != null && loginInfo.PasswordExpiresOn.HasValue && loginInfo.PasswordExpiresOn.Value < DateTime.Now)
                    View.DisplayPasswordExpiredOn(loginInfo.PasswordExpiresOn.Value);
                else
                    View.DisplayMustChangePassword(loginInfo.ResetType);
            }
            else
                View.LogonUser(person, InternalService.GetDefaultLogonCommunity(person), View.PreloggedProviderId, RootObject.InternalLogin(false), true, CurrentManager.GetUserDefaultIdOrganization(idUser));
        }

        public void EditPassword(String oldPassword, String newPassword)
        {
            try
            {
                Person person = CurrentManager.GetPerson(View.LoggedUserId);
                if (InternalService.EditPassword(person, oldPassword, newPassword, true))
                {
                    if (person.AcceptPolicy || !PolicyService.UserHasPolicyToAccept(person))
                        View.LogonUser(person, InternalService.GetDefaultLogonCommunity(person), View.LoggedProviderId, RootObject.InternalLogin(false), true, CurrentManager.GetUserDefaultIdOrganization(person.Id));
                    else
                        View.DisplayPrivacyPolicy(person.Id, View.LoggedProviderId, RootObject.InternalLogin(false), true);
                }
                else
                    View.DisplayPasswordNotChanged();
            }
            catch (lm.Comol.Core.Authentication.SamePasswordException ex)
            {
                View.DisplaySamePasswordException();
            }
            catch (lm.Comol.Core.Authentication.InvalidPasswordException ex)
            {
                View.DisplayInvalidPassword();
            }
            catch (Exception ex)
            {
                View.DisplayPasswordNotChanged();
            }

        }
        //public void AcceptPolicy() {
        //    Int32 idUser = View.PreloggedUserId;
        //    if (View.PreloggedUserId == View.LoggedUserId)
        //    {
        //        Person person = CurrentManager.GetPerson(idUser);
        //        View.LogonUser(person, InternalService.GetDefaultLogonCommunity(person), View.PreloggedProviderId);
        //    }
        //    else
        //    {
        //        View.PreloggedUserId = 0;
        //        if (ShibbolethService.HasActivePoviders(Authentication.AuthenticationProviderType.Shibboleth))
        //            View.GotoInternalShibbolethAuthenticationPage();
        //        else
        //            View.GotoInternalAuthenticationPage();
        //    }
        //}
    }
}