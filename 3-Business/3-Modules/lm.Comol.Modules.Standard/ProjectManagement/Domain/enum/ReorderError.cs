using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]

    /// Action for tasks reorder
    public enum ReorderError : int
    {
        None = 0,
        InConflictPredecessorsFound = 1,
        SummaryWithLinks = 2,
        DataAccess = 3,
        ProjectMapChanged = 4,
        SummaryWithInvalidLinks = 5
    }
}