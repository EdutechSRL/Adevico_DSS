using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum UserActivityStatus
    {
        Ignore = -1,
        ToDo = 0,
        Starting = 1,
        Expired = 2,
        Expiring = 3,
    }
}