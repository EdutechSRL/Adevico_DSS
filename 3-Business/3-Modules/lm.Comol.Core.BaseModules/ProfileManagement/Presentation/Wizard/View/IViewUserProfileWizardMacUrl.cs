using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProfileManagement;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewUserProfileWizardMacUrl : IViewUserProfileWizardExternal
    {
        String PostUserIdentifier { get; set; }
        String PostInternalMac { get; set; }
        Int32 TokenIdOrganization { get; set; }
        Int32 TokenIdProfileType { get; set; }

        dtoMacUrlProviderIdentifier GetUrlIdentifier(List<dtoMacUrlProviderIdentifier> availableItems);
        List<dtoMacUrlUserAttribute> GetTokenAttributes(List<dtoMacUrlUserAttribute> attributes);


        List<dtoMacUrlUserAttribute> TokenAttributes { get; set; }
        dtoExternalCredentials ExternalCredentials { get; set; }
        void DisableInput(List<lm.Comol.Core.Authentication.ProfileAttributeType> items);

        void GotoDefaultPage();
        void GotoRemoteLogonPage(String url);
    }
}