using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum TaskConstraint
    {
        AsSoonAsPossible = 0,
        StartNoEarlierThan = 1,
        FinishNoLaterThan = 2
    }
}
