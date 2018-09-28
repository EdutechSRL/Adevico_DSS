using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public enum SummaryType
    {
        Unknown = 0,
        PortalIndex = 1,
        CommunityIndex = 2,
        Organization = 3,
        Community = 5,
        Path = 6,
        User = 7,
        MySummary = 8
    }
}