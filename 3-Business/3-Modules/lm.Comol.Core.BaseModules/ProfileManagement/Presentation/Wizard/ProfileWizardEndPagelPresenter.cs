using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class ProfileWizardEndPagelPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private InternalAuthenticationService _InternalService;
            private ProfileManagementService _Service;
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
            protected virtual IViewUserProfileWizardEndPage View
            {
                get { return (IViewUserProfileWizardEndPage)base.View; }
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
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            public ProfileWizardEndPagelPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProfileWizardEndPagelPresenter(iApplicationContext oContext, IViewUserProfileWizardEndPage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean accessAvailable = !(View.isSystemOutOfOrder);
            View.AllowInternalAuthentication = !View.isSystemOutOfOrder;
            View.AllowBackToStartPage = View.isSystemOutOfOrder;
            if (View.CurrentMessage == ProfileSubscriptionMessage.None && View.PreloadedMessage != ProfileSubscriptionMessage.None) {
                View.CurrentMessage = View.PreloadedMessage;
            }
            ProfileSubscriptionMessage message = View.CurrentMessage;
            View.LoadMessage(message);
        }
    }
}