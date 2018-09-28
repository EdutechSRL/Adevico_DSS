using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum EditingErrors
    {
        None = 0,
        NoResources = 1,
        RemovedResources = 2,
        MultipleLongName = 4,
        MultipleShortName = 8,
    }
}