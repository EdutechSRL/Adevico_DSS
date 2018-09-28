using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoStatusCompletion
    {
        public Int64 Completion { get; set; }
        public StatusStatistic Status { get; set; }

        public dtoStatusCompletion() 
        { }

        public dtoStatusCompletion(StatusStatistic Status, Int64 Completion)
        {
            this.Status = Status;
            this.Completion = Completion;
        }
    }
}
