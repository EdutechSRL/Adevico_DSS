using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewSystemOutOfOrder : IViewBaseAuthenticationPages
    {
        Boolean isSystemOutOfOrder { get; }

        void GotoAuthenticationSelctorPage();
        void GotoInternalLogin();

        void AllowExternalWebAuthentication(String url);
    }
}