using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewProfileDisabled : IViewBaseAuthenticationPages
    {
        Boolean AllowInternalAuthentication { set; }
        Boolean isSystemOutOfOrder { get; }

        long PreloadedIdProvider {get;}
        Int32 PreloadedIdProfile { get; }
        long IdProvider { get; set; }
        Int32 IdProfile { get; set; }



        void GotoAuthenticationSelctorPage();
        void GotoInternalLogin();
        void GotoRemoteUrl(String url);

        void AllowExternalWebAuthentication(String url);
        void DisplayDisabledAccount(String name);
        void DisplayNotActivatedAccount(String name);
    }
}