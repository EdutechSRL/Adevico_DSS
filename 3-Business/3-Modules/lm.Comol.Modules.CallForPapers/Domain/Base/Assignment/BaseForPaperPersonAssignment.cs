using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class BaseForPaperPersonAssignment : BaseForPaperAssignment
    {
        public virtual litePerson AssignedTo { get; set; }
    }
}