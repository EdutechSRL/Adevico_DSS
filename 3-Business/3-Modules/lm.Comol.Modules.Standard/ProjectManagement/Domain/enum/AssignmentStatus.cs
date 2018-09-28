using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum AssignmentStatus  
    {
       notstarted = 0,
       started = 1,
       completed = 2,
       suspended = 4,
       pendingapproval = 8,
       notapproved = 16
    }
}