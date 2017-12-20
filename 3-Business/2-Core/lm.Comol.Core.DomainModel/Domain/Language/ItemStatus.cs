using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Languages
{
    [Serializable]
    public enum ItemStatus
    {
        none = 0,
        valid = 2,
        warning = 3,
        wrong = 4
    }
}
