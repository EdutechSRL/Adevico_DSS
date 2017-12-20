using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Editor
{
    [Serializable()]
    public enum EditorType
    {
        none = 0,
        textarea = 1,
        lite = 2,
        tiny = 3,
        telerik = 4
    }
}