using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [FlagsAttribute]
    public enum EPType
    {
        None=0,
        Mark=1,
        Time=2,
        Manual=4,
        Auto=8,
        PlayMode=16,
        AlwaysStat=32,

        MarkManual =5,
        MarkAuto = 9,
        TimeManual = 6,
        TimeAuto = 10,
    }

    public enum MoocType : int
    {
        None = 0,
        Info = 1,
        Cockade = 2,
        Certification = 3
    }
}
