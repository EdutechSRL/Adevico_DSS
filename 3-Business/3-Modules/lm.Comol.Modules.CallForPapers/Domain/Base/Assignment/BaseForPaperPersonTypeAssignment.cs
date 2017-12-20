using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class BaseForPaperPersonTypeAssignment : BaseForPaperAssignment
    {
        public virtual int AssignedTo { get; set; }
    }
}
