using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ProjectFilterBy
    {
        All = 0,
        CurrentCommunity = 1,
        AllPersonal = 2,
        AllPersonalFromCurrentCommunity = 4,
        FromPortal = 8,
    }
}