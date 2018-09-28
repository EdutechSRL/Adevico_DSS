using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [FlagsAttribute]
    public enum StatusStatistic
    {              
        None=0,
        Browsed = 1,
        Started=2,   
        BrowsedStarted=Browsed+Started,
        Completed=4,
        Passed=16,
        CompletedPassed= Completed + Passed
    }
}
