using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable, Flags]
    public enum FieldStatus
    {
        none = 0,
        notstarted = 1,
        started = 2,
        completed = 4,
        recalc = 8,
        updated = 16,
        error = 32,
        errorfatherlinked = 64,
        errorsummarylinked = 128,
        removed = 256
    }
}