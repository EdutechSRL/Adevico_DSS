using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ProjectAvailability
    {
        Draft = 1,
        Active = 2,
        Closed = 4,
        Suspended = 8
    }
}