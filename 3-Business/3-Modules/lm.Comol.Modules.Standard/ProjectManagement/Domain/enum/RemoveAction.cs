using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum RemoveAction
    {
        None = 0,
        FromNotCompletedAssignments = 1,
        FromNotStartedAssignments = 2,
        FromAllAndRecalculateCompletion = 4,
        FromNotCompletedAssignmentsAndRecalculateCompletion = 8,
        FromAllAssignments = 16
    }
}