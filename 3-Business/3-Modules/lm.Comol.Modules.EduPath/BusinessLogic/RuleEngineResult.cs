using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public class RuleEngineResult<Telement> where Telement : IRuleElement
    {
        public Boolean isValid { get; set; }

        public IList<RuleBase<Telement>> ViolatedRules { get; set; }

        public IList<RuleBase<Telement>> ValidRules { get; set; }

        public RuleEngineResult()
        {
            isValid = false;
            ViolatedRules = new List<RuleBase<Telement>>();
            ValidRules = new List<RuleBase<Telement>>();
        }
    }
}
