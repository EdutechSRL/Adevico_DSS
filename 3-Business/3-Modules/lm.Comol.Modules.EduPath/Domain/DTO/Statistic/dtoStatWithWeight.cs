using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoStatWithWeight
    {

        public Int64 UserTotWeight { get; set; }
        public Int64 Completion { get; set; }
        public Int64 MinCompletion { get; set; }

        public dtoStatWithWeight() { }
    }
}
