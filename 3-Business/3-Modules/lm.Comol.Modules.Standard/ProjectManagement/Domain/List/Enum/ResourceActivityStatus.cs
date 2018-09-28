using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ResourceActivityStatus
    {
        none = 0,
        confirmed = 1,
        completed = 2,
        started = 3,
        late = 4,
        notstarted = 5,
        all = 6
    }
}