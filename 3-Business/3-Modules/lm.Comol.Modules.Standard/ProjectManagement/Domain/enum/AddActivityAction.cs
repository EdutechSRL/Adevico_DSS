using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum AddActivityAction
    {
        ToProject = 0,
        Before = 1,
        After = 2,
        AsChildren = 3
    }
}