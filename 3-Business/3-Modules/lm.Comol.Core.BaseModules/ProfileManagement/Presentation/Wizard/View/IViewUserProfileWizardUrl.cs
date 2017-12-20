using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProfileManagement;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewUserProfileWizardUrl : IViewUserProfileWizardExternal
    {
        String UrlIdentifier { get; set; }
        String UrlIdentifierValue { get; set; }
        dtoExternalCredentials ExternalCredentials { get; set; }

        void GotoDefaultPage();
        void GotoRemoteLogonPage(String url);
    }
}