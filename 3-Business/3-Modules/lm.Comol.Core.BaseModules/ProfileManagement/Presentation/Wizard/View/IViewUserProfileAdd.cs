using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.PolicyManagement;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewUserProfileAdd: lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }
        long IdProvider { get; set; }
        dtoExternalCredentials ExternalCredentials { get; set; }

        ProfileSubscriptionMessage CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String profileName, Int32 idOrganization, AuthenticationProviderType authentication, long idProvider);
        ProfileSubscriptionMessage CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String profileName, Int32 idOrganization, List<Int32> otherOrganizations, AuthenticationProviderType authentication, long idProvider);
        ProfileSubscriptionMessage CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String profileName, Int32 idOrganization, AuthenticationProviderType authentication, long idProvider, dtoExternalCredentials credentials);
        ProfileSubscriptionMessage CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String profileName, Int32 idOrganization, List<Int32> otherOrganizations,  lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider provider, dtoExternalCredentials credentials);
    }
}