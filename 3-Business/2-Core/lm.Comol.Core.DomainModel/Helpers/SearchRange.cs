using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public enum SearchRange
    {
        None = 0,
        Portal = 1,
        Communities = 2,
        Community = 4,
        All = 3
    }
}