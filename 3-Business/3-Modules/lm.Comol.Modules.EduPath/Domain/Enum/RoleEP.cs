using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{

    [FlagsAttribute]
    public enum RoleEP //Da definire i ruoli
    {
   
        None=0,
        Participant=1,
        Evaluator = 2,
        Manager=4,
        StatViewer=8
    }
}


