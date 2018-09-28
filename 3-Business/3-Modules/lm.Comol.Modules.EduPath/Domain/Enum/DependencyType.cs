using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [FlagsAttribute]
    public enum DependencyType
    {
        None = 0,
        Previous = 1,
        Next = 2,
        AndDep = 4,
        OrDep = 8
    }
}
