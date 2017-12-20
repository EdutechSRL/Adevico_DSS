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
    public class RetrievePasswordPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
              private InternalAuthenticationService _InternalService;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewRetrievePassword View
            {
                get { return (IViewRetrievePassword)base.View; }
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
            public RetrievePasswordPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RetrievePasswordPresenter(iApplicationContext oContext, IViewRetrievePassword view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean accessAvailable = !(View.isSystemOutOfOrder && !View.AllowAdminAccess);
            View.AllowSubscription = !View.isSystemOutOfOrder && View.SubscriptionActive;
              if (!accessAvailable)
                View.DisplaySystemOutOfOrder();
              else
              {
                  View.DisplayRetrievePassword();
                  View.AllowBackFromRetrieve = accessAvailable;
              }
        }

        public void RetrievePassword(string mail) {
            InternalLoginInfo user = InternalService.FindUserByMail(mail);
            if (user == null || user.Person ==null)
                View.DisplayRetrievePasswordUnknownLogin();
            else {
                String newPassword = InternalService.RenewPassword(user, user.Person,EditType.retrieve);
                if (String.IsNullOrEmpty(newPassword))
                    View.DisplayRetrievePasswordError();
                else
                {
                    View.SendMail(user, newPassword, CurrentManager.GetAllLanguages().Where(l=>l.Id== user.Person.LanguageID).FirstOrDefault());
                    View.GotoInternalLogin();
                }
            }
        }
    }
}