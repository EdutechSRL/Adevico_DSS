using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class DTOSummaryPathUser
    {
        public virtual Path Path { get; set; }
        public virtual liteCommunity Community { get; set; }
        public virtual litePerson person { get; set; }
    }
}
