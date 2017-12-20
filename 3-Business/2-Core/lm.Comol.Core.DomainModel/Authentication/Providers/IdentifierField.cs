using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable, Flags ]
    public enum IdentifierField
    {
        none = 0,
        longField = 1,
        stringField = 2
    }
}
