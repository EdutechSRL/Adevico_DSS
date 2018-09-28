using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum PageListType
    {
        Ignore = -1,
        None = 0,
        ListResource = 1,
        ListManager = 2,
        ListAdministrator = 3,
        DashboardManager = 4,
        DashboardResource = 5,
        DashboardAdministrator = 6,
        ProjectDashboardManager = 7,
        ProjectDashboardResource = 8,
    }
}