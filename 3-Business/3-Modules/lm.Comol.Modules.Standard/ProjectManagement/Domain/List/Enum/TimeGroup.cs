using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum TimeGroup
    {
        PreviousYears = 0,
        PreviousYear = 1,
        PreviousDaysInHalfYear = 2,
        PreviousHalfYear = 3,
        PreviousQuarter = 4,
        PreviousDaysInQuarter = 5,
        PreviousMonth = 6,
        PreviousWeeks = 7,
        PreviousWeek = 8,
        PreviousDays = 9,
        Today = 10,
        ThisWeek = 11,
        NextWeek = 12,
        ThisMonth = 13,
        NextMonth = 14,
        ThisQuarter = 15,
        NextQuarter = 16,
        ThisHalfYear = 17,
        NextHalfYear = 18,
        ThisYear = 19,
        NextYears = 20
    }
}