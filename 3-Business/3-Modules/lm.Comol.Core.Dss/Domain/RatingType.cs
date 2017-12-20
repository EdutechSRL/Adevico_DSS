using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain
{
    [Serializable, Flags]
    public enum RatingType : int 
    {
        none = 0,
        simple = 1,
        extended = 2,
        intermediateValues = 4,
    }
}