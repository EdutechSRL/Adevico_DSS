using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain
{
    [Serializable]
    public enum NormalizeTo: int 
    {
        none = 0,
        simple = 1,
        standard = 2,
        toOne = 3
    }
}