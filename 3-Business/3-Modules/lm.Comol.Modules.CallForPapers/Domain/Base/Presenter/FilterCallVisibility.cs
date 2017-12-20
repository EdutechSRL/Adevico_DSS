using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum  FilterCallVisibility
    {
        None = 0,
        OnlyVisible = 1,
        OnlyDeleted = 2,
        All = 3
    }
}
