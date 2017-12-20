using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum OrderUsersBy
    {
        None = 0,
        SurnameAndName = 1,
        Role = 2,
        Status = 3,
        SubscriptionDate = 4,
        Profile = 5
    }
}