using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class CRoleRuleOverride : BaseRuleOverride
    {
        public virtual Role Role { get; set; }

        public virtual liteCommunity Community { get; set; }
    }
}
