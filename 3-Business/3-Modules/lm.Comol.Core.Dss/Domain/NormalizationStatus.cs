using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain
{
    [Serializable]
    public enum NormalizationStatus
    {
        none = 0,
        fatalerror = 1,
        normalized = 2,
        normalizable = 3,
        impossible = 4,
    }
}