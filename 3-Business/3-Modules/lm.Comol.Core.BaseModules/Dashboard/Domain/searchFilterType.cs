using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Domain 
{
    [Serializable]
    public enum searchFilterType : int{
        none =-1,
        organization = 0,
        communitytype = 1,
        responsible = 2,
        name = 3,
        status = 4,
        year = 5,
        coursetime = 6,
        degreetype = 7,
        tagassociation = 8,
        tag = 9,
        letters = 10,
       
    }
}