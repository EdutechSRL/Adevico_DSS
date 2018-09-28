using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Header.Domain
{
    [Serializable]
    public enum DisplayNameMode
    {
        none = 0,
        surname = 1,
        name = 2,
        surnamename = 4,
        namesurname =5
    }
}
