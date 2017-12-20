using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum CommunitySubscriptionWizardStep
    {
        None = 0,
        SelectCommunities = 1,
        SubscriptionsSettings = 2,
        RemoveSubscriptions = 4,
        Summary = 8,
        Errors = 16
    }
   
}
