using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewRetrievePassword : IViewBaseAuthenticationPages
    {
        Boolean AllowAdminAccess { get; }
        Boolean isSystemOutOfOrder { get; }

        void DisplaySystemOutOfOrder();
        void GotoInternalLogin();

        Boolean AllowExternalWebAuthentication { set; }
        Boolean AllowSubscription { set; }
        Boolean SubscriptionActive { get; }
        Boolean AllowBackFromRetrieve { set; }

        void DisplayRetrievePassword();
        void DisplayRetrievePasswordUnknownLogin();
        void DisplayRetrievePasswordError();
        void SetExternalWebLogonUrl(String url);
        void SendMail(InternalLoginInfo user, String password, Language language);
    }
}