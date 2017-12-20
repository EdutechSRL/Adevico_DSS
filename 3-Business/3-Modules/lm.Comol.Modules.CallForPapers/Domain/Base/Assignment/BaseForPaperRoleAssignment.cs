using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class BaseForPaperRoleAssignment : BaseForPaperAssignment
    {
        public virtual liteCommunity Community { get; set; }
        public virtual Int32 AssignedTo { get; set; }
    }
}
