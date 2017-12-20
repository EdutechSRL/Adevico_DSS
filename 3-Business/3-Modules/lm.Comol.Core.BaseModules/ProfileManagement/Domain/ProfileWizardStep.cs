using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum ProfileWizardStep
    {
        None = 0,
        StandardDisclaimer = 1,
        OrganizationSelector = 2,
        ProfileTypeSelector = 3,
        ProfileUserData = 4,
        Privacy = 5,
        Summary = 6,
        InternalCredentials = 7,
        UnknownProfileDisclaimer = 8,
        AuthenticationTypeSelector = 9,
        LdapCredentials = 10,
        SubscriptionError = 11,
        WaitingLogon = 12,
    }
}
