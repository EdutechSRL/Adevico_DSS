using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.BaseModules.ProviderManagement;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewAddProfileToPortalWizard : IViewProfileWizard
    {
        Boolean AllowBackTomanagement { set; }
        Int32 PreloadedIdProfileType { get; }
        List<Int32> OtherSelectedOrganizationId { get; }

        long SelectedProviderId { get; set; }
        lm.Comol.Core.Authentication.AuthenticationProviderType SelectedProviderType { get;}
        List<long> AvailableProvidersId { get; }
        Boolean SelectedProviderAllowAdminProfileInsert { get; }

        dtoExternalCredentials GetExternalCredentials { get; }

        ProfileSubscriptionMessage CreateProfile(dtoBaseProfile profile, Int32 idProfileType, String profileName, Int32 idOrganization, List<Int32> otherOrganizations, dtoBaseProvider provider, dtoExternalCredentials credentials);
        void InitializeAuthenticationTypeSelectorStep(List<dtoBaseProvider> providers, long idProvider);
        void InitializeProfileTypeSelector(Int32 IdDefaultRole);
        void DisplayNoPermission();
        void LoadProfiles(lm.Comol.Core.DomainModel.Person person);

    }
}