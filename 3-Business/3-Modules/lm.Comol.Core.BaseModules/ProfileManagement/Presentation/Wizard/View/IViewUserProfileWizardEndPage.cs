using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.PolicyManagement;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewUserProfileWizardEndPage : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isSystemOutOfOrder { get; }
        ProfileSubscriptionMessage PreloadedMessage { get; }
        ProfileSubscriptionMessage CurrentMessage { get; set; }
        Boolean AllowExternalWebAuthentication { set; }
        Boolean AllowInternalAuthentication { set; }
        Boolean AllowBackToStartPage { set; }

        void LoadMessage(ProfileSubscriptionMessage message);
        void DisplaySystemOutOfOrder();
        void SetExternalWebLogonUrl(String url);
        void GotoInternalLogin();
        void GotoExternalLoginPage(String url);
    }
}