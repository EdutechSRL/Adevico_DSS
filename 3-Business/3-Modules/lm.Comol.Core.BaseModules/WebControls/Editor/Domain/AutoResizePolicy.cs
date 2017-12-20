using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Editor
{
    [Serializable()]
    public enum ItemPolicy
    {
        byconfiguration = 0,
        allow = 1,
        notallowed = 2,
    }
}