using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public enum OwnerType
    {
        None = 0,
        System = 1,
        Module = 2,
        Community = 3,
        Person = 4,
        Object = 5
    }
}
