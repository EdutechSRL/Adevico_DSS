using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    /// <summary>
    /// Subscription constraints
    /// </summary>
    public enum EnrollingStatus
    {
        None = 0,
        Available = 1,
        StartDate = 2,
        EndDate = 3,
        Seats = 4,
        Constraints = 5,
        Unavailable = 6
    }
}
