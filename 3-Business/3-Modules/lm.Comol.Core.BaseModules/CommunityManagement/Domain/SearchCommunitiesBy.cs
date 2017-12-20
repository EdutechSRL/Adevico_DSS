using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable]
    public enum SearchCommunitiesBy
    {
        All = -1,
        None = 0,
        Contains = 1,
        StartAs = 2,
        Owner = 3,
        Responsible = 4,
        NameStartAs = 5
    }
}
