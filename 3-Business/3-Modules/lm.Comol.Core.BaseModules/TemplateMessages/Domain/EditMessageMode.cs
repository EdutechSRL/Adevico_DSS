using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Domain
{
    [Serializable]
    public enum EditMessageMode
    {
        None = 0,
        Edit = 1,
        SelectUsers =2,
        MessageSent = 3,
    }
}
