using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum PmActivityLinkType
    {
        FinishStart = 0,
        FinishFinish = 1,
        StartStart = 2,
        StartFinish = 3
    }
}
