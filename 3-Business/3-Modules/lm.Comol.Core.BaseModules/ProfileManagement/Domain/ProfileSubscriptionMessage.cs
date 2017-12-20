using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum ProfileSubscriptionMessage
    {
        None = 0,
        Created = 1,
        CreatedWithAutoLogon = 2,
        LoginDuplicated = -1,
        TaxCodeDuplicated = -2,
        MailDuplicated = -3,
		MailNotificationNotSent = -4,
		OrganizationCommunityNotSubscribed = -5,
		OrganizationNotSubscribed = -6,
		UnknownError = -7,
		AlreadyCreated = -8,
		FillField = -9,
		MatriculaDuplicated = -10,
		ProfileTypeChanged = 3,
		Edited = 4,
		CreatedAndDisabled = 5,
		CreatedAndWaiting = 6,
        AccountDisabled = 7,
        ProviderUnknown = 8,
        UnableToConnectToInternalProvider = 9,
        externalUniqueIDduplicate = 10,
        SubscriptionNotActive = 11
    }
}
