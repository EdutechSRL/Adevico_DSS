using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Domain 
{
    [Serializable]
    public enum searchColumn : int
    {
        none =0,
        degreetype = 1,
        communitytype = 2,
        status = 3,
        year = 4,
        coursetime = 5,
        actions = 6,
        subscriptioninfo = 7,
        startsubscriptionon = 8,
        endsubscriptionon = 9,
        maxsubscribers = 10,
        genericdate = 11,
        select = 12,
        name = 13,
        owner = 14,
        info = 15
   }
}