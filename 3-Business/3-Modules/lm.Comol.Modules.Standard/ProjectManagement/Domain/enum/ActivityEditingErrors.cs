using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable, Flags]
    public enum ActivityEditingErrors
    {
        None = 0,
        Cycles = 1,
        Settings = 2,
        Compliteness = 4,
        Assignments = 8,
        All = 16
    }
}