using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class BaseForPaperCommunityAssignment : BaseForPaperAssignment
    {
        public virtual liteCommunity AssignedTo { get; set; }
    }
}
