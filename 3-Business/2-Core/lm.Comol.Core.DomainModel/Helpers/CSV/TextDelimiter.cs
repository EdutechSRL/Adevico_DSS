using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public enum TextDelimiter
    {
        None = 0,
        Semicolon = 1,
        Colon = 2,
        Comma = 3,
        Tab = 4,
        VerticalBar = 5,
        CrLf = 6,
        Cr = 7,
        Lf = 8
    }
}