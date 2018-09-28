using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]

    /// Action for tasks reorder
    public enum ReorderAction : int
    {
        Ignore = 0,
        DisableReorder = 3,
        RemoveAllPredecessors = 2,
        RemoveConflictPredecessors =1,
        AskAlways = 4,
        ReloadProjectMap = 5,
        ReloadProjectMapWithReorderedItems = 6
    }
}