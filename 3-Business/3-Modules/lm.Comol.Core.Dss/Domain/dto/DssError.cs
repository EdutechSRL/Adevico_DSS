using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain
{
    [Serializable, Flags ]
    public enum DssError
    {
        None = 0,
        MissingMethod = 1,
        MissingRatingSet = 2,
        MissingWeight = 4,
        InvalidWeight = 8,
        InvalidType = 16,
        MissingRating = 32,
        InvalidRating = 64,
        MissingManualWeight = 128,
        InvalidManualWeight = 256
    }
}