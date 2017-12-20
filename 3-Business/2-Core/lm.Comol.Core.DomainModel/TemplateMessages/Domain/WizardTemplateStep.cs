using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public enum WizardTemplateStep
    {
        None = -1,
        Settings = 0,
        Translations = 1,
        Permission = 2,
    }
}
