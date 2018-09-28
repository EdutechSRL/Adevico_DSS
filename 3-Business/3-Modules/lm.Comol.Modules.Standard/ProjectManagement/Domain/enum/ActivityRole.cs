using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ActivityRole
    {
        None = 0,
        ProjectOwner = 1,
        Manager = 2,
        Resource = 3,
        Visitor = 4,
    }
}