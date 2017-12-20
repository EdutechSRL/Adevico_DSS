using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.PolicyManagement.Business;
using lm.Comol.Core.Authentication.Business;

namespace lm.Comol.Core.BaseModules.PolicyManagement.Presentation
{
    public class AcceptLogonPolicyPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private InternalAuthenticationService _InternalService;
        private PolicyService _PolicyService; 
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
            protected virtual IViewAcceptLogonPolicy View
            {
                get { return (IViewAcceptLogonPolicy)base.View; }
            }
            private PolicyService Service
            {
                get
                {
                    if (_PolicyService == null)
                        _PolicyService = new PolicyService(AppContext);
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
            public AcceptLogonPolicyPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AcceptLogonPolicyPresenter(iApplicationContext oContext, IViewAcceptLogonPolicy view)
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
                String url = View.PreloggedProviderUrl;
                View.PreloggedUserId = 0;
                View.PreloggedProviderId = 0;
                View.PreloggedProviderUrl = "";
                if (String.IsNullOrEmpty(url))
                    View.GotoInternalAuthenticationPage();
                else if (url.StartsWith("http"))
                    View.GotoExternalUrl(url);
            }
            else if (Service.UserHasPolicyToAccept(person))
            {
                View.LoggedUserId = idUser;
                View.LoggedProviderId = View.PreloggedProviderId;
                View.LoggedProviderUrl = View.PreloggedProviderUrl;
                View.LoadPolicySubmission(person.Id);
            }
            else
                View.LogonUser(person, InternalService.GetDefaultLogonCommunity(person), View.PreloggedProviderId, View.PreloggedProviderUrl);
        }

        public void AcceptPolicy() {
            Int32 idUser = View.PreloggedUserId;
            String url = View.LoggedProviderUrl;
            if (View.PreloggedUserId == View.LoggedUserId)
            {
                Person person = CurrentManager.GetPerson(idUser);
                View.LogonUser(person, InternalService.GetDefaultLogonCommunity(person), View.PreloggedProviderId, url);
            }
            else
            {
                View.PreloggedUserId = 0;
                View.PreloggedProviderId = 0;
                View.PreloggedProviderUrl = "";
                if (String.IsNullOrEmpty(url))
                    View.GotoInternalAuthenticationPage();
                else if (url.StartsWith("http"))
                    View.GotoExternalUrl(url);
            }
        }
    }
}