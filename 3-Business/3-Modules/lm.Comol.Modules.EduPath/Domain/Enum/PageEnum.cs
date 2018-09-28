using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{

    public enum EvaluationFilter
    {        
        Evaluated=0,
        NotEvaluated=1    
    }

    public enum EpError
    {
        Generic,
        NotPermission,
        Url,
        NoAssignedPath,
        PathNotFind,
        DisplayOrder,
        UserNotInEp,
        Data,
        EmptyActivities,
        NotManageable,
        PathTimeLowerThanActivities,
        PathTimeHigherThanActivities,
        PathTimeNotEqualToActivities
    }

    public enum EpViewModeType
    {

        None = 0,
        View = 1,
        Manage = 2,
        Stat=3,
    }

    public enum OrderType
    {
        Ascending = 0,
        Descending = 1
    }

    public enum ItemType
    {
        None=-1,
        Path = 1,
        Unit=2,
        Activity=3,
        SubActivity=4
    }
    public enum EpStatPage
    { 
        None=0,
        UsersStat=1,
        UserStat=2,
        GlobalStat=2
    }

    public enum CopyOfEpStatPage
    {
        None = 0,
        UsersStat = 1,
        UserStat = 2,
        GlobalStat = 2
    }
}