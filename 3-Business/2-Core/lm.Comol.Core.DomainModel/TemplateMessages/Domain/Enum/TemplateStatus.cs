using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public enum TemplateStatus
    {
        Draft = 0,
        Active = 1,
        Replaced = 2,
    }
}