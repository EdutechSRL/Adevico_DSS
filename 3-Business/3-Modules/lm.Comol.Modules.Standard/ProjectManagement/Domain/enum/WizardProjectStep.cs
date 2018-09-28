using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum WizardProjectStep
    {
        None = 0,
        Settings = 1,
        ProjectUsers = 2,
        Calendars = 3,
        Documents = 4,
    }
}
