using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    [Serializable]
    public class dtoEvaluationLink
    {
        public virtual DateTime ModifiedOn { get; set; }
        public virtual long LinkId { get; set; }
        public virtual long Id { get; set; }
    }
}
