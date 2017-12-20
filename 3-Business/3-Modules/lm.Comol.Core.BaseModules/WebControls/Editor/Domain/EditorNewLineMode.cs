using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Editor
{
    [Serializable()]
    public enum EditorNewLineModes
    {
        // Summary:
        //     Insert a BR element.
        Br = 1,
        //
        // Summary:
        //     Insert a P (paragraph) element.
        P = 2,
        //
        // Summary:
        //     Insert a DIV element.
        Div = 4,
    }
}