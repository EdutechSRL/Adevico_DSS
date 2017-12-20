using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Languages
{
    [Serializable, Flags]
    public enum ItemDisplayAs
    {
        first = 1,
        item = 2,
        last = 4
    }
}
