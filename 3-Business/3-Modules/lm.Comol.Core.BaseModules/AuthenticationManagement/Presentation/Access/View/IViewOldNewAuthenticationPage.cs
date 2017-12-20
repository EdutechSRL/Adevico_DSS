using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewOldNewAuthenticationPage : IViewAuthenticationPage
    {
        int LoggedUserId { get; set; }
        Boolean UseAuthenticationProviders { get; }
        Boolean AllowRetrievePassword { set; }
        Boolean AllowAuthentication { set; }
        Boolean AllowSubscription { set; }
        Boolean AllowExternalWebAuthentication { set; }
        Boolean SubscriptionActive { get; }

        void DisplayLogonInput();
        void DisplayInvalidCredentials();
        void DisplayAccountDisabled();
        void SetExternalWebLogonUrl(String url);
       
    } 
}