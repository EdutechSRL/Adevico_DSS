using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.PolicyManagement;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewUserProfileWizard : IViewProfileWizard 
    {
        Boolean SubscriptionActive { get; }
        Boolean isSystemOutOfOrder { get; }
        Int32 SelectedStartLanguageID { get; set; }
        Boolean AcceptedMandatoryPolicy { get; }
        List<dtoUserPolicyInfo> GetPolicyInfo { get; set; }
       

        ProfileSubscriptionMessage CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String ProfileName, Int32 idOrganization, AuthenticationProviderType authentication, long idProvider);
        void LogonUser(Person user, long idProvider,String providerUrl, Boolean internalPage, Int32 idUserDefaultIdOrganization);
        void DisplaySystemOutOfOrder();
    }
}