using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public enum SearchCommunityFor
    {
        SystemManagement = 1,
        CommunityManagement = 2,
        Subscribed = 3,
        Subscribe = 4,
        ModuleAccess = 5
    }
}