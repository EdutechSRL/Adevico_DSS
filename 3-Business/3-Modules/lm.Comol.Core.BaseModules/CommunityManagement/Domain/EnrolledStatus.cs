using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    /// <summary>
    /// Subscription status
    /// </summary>
    public enum EnrolledStatus
    {
        None = 0,
        Available = 1,
        NeedConfirm = 2,
        PreviousEnrolled = 3,
        NotAvailable = 4,
        UnableToEnroll = 5
    }
}