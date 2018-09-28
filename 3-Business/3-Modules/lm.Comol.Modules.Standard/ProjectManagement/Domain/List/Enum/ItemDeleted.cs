using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ItemDeleted
    {
        Ignore = 0,
        Yes = 1,
        No = 2,
    }
}