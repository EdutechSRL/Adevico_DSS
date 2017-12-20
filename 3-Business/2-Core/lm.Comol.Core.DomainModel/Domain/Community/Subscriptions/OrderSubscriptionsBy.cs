using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Subscriptions
{
    [Serializable]
    public enum OrderSubscriptionsBy
    {
        None =0,
        Name = 1,
        Role = 2,
        Status = 3,
        SubscriptionDate = 4,
        LastVisit = 5
    }
}