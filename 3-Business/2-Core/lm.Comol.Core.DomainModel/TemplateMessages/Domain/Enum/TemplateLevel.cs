using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public enum TemplateLevel
    {
        None = 0,
        Portal = 1,
        Organization = 2,
        Community = 4,
        Personal = 8,
        Object = 16,
        Removed = 32
    }
}