using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable]
    public enum CommunityAvailability
    {
        None = 0,
        Subscribed = 1,
        NotSubscribed = 2,
        All = 3
    }
}