using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class UnitPersonRuleOverride : PersonRuleOverride
    {
        public virtual Unit Unit { get; set; }
    }
}
