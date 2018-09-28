using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{

    public class BaseRuleOverride : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual Path Path { get; set; }

        public virtual RuleOverrideType RuleOverrideType { get; protected set; }

        public virtual RuleRangeType RuleRangeType { get; set; }

        public virtual short NewMaxValue { get; set; }

        public virtual short NewMinValue { get; set; }
    }

}
