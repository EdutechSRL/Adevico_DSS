using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProfileManagement;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewUserProfileWizardExternal : IViewUserProfileWizard
    {
        long PreloadedIdProvider { get; }
        long idProvider { get; set; }
        AuthenticationProviderType SelectedProvider { get; }
        dtoInternalCredentials GetInternalCredentials { get; }
        String ExternalUserInfo { get; set; }

        void InitializeUnknownProfileStep(AuthenticationProviderType dProvider, List<AuthenticationProviderType> providers);
        void InitializeProfileTypeSelector(List<Int32> OtherRolesId, Int32 IdDefaultRole);
        void GotoStepProfileInfo(dtoBaseProfile profile);
        void DisplayInvalidCredentials();
        void DisplayInternalCredentialsMessage(ProfileSubscriptionMessage message);
        void DisplayPrivacyPolicy(Int32 idUser, long idProvider, String defaultUrl, Boolean internalPage);
    }
}