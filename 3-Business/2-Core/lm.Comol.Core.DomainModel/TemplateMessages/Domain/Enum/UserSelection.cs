using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable, Flags]
    public enum UserSelection
    {
        None = 0,
        FromModule = 1,
        FromInputText = 2
    }
}
