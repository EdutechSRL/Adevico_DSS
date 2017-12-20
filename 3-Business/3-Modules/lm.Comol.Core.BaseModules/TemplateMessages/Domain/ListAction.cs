using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Domain
{
    [Serializable]
    public enum ListAction
    {
        None = 0,
        Delete = 1,
        VirtualDelete =2,
        VirtualUndelete = 3,
        Clone =4,
        AddVersion = 5

    }
}
