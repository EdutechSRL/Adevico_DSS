using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public enum ModuleStatus //status da definire
    {
        None = 0,
        ActiveForSystem = 1,
        ActiveForCommunity = 2,
        DisableForCommunity = 3,
        DisableForSystem = 4,
        DisableForCommunityAvailableForAdmin = 5,
        DisableForSystemAvailableForAdmin = 6
    }
}