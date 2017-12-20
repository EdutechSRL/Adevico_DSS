using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable, Flags]
    public enum ItemDisplayOrder
    {
        item = 0,
        first = 1,
        last = 2
    }
}
