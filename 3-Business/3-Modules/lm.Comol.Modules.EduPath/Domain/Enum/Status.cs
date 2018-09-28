using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [FlagsAttribute]
    public enum Status //status da definire
    {
        None=0,
            
        Locked=2,
        Mandatory = 4,
        EvaluableAnalog =8,
        EvaluableDigital = 16,
        Last = 32,
        Draft = 64,
        Text=128,
        MandatoryPersonalize=132,
        NotMandatory = 256,
        NotLocked = 512,

    }
}
