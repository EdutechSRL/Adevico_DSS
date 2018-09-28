using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable, Flags]
    public enum DateType
    {
        Single = 1,
        Range = 2,
        Multiple = 4
    }
}