using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class UnitCRoleRuleOverride : CRoleRuleOverride
    {
        public virtual Unit Unit { get; set; }
    }
}
