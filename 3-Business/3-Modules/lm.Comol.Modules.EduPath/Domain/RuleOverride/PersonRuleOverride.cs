using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class PersonRuleOverride : BaseRuleOverride
    {
        public virtual litePerson Person { get; set; }
    }
}
