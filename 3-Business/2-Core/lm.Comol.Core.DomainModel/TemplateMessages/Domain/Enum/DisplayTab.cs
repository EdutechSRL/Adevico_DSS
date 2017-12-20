using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable, Flags]
    public enum DisplayTab
    {
        None = 0,
        List = 1,
        Send = 2,
        Sent = 4
    }
}