using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public enum ContactsSelectionMode
    {
        None = 0,
        SystemUsers = 1,
        CommunityUsers = 2,
        MyCommunities = 4

    }
}