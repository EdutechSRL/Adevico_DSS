using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [FlagsAttribute]
    public enum StatusAssignment
    {
    
        None=0,      
        Locked=1,
        Inherited = 4,
    }

    [FlagsAttribute]
    public enum CopyOfStatusAssignment
    {

        None = 0,
        Locked = 1,
        Inherited = 4,
    }
}
