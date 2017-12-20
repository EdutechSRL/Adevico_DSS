using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public enum InputType
    {
        skip = -1,
        unknown = 0,
        strings = 1,
        int16 = 3,
        int32 = 4,
        int64 = 5,
        mail = 6,
        taxCode = 9,
        login = 10,
        password = 11,
        datetime = 12
    }
}
