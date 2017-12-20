using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public enum ConstraintType: int
    {
        None = 0,
        EnrolledTo = 1,
        NotEnrolledTo = 2,
        ActivityCompleted = 3,
        ActivityNotCompleted = 4,
    }
}
