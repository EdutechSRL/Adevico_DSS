using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public enum ItemPriority
    {
        unassigned = 0,
        low = 1,
        normal = 2,
        high = 4,
        critical = 8,
        important = 16
    }
}
