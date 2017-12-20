using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public enum searchFilterType : int{
        none =-1,
        organization = 0,
        createdby = 1,
        type = 2,
        status = 3,
        name = 4,
        letters = 5
    }
}