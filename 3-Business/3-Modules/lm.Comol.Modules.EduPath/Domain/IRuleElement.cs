using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    public interface IRuleElement
    {

        Int64 Id { get; }        

        Int64  UserCompletion { get; set; }

        short UserMark { get; set; }

        short DisplayOrder { get; set; }

        short Duration { get; set; }

        DateTime? EndDate { get; set; }

        Int64 MinCompletion { get; set; }

        string Name { get; set; }

        System.DateTime? StartDate { get; set; }

        Status Status { get; set; }

        Boolean MandatoryStepsCompleted { get; set; }

        Boolean Completed();

        short OverrideRuleCompletionMinValue { get; set; }

        short OverrideRuleCompletionMaxValue { get; set; }

        short OverrideRuleMarkMinValue { get; set; }

        short OverrideRuleMarkMaxValue { get; set; }

        //RuleRangeType OverrideRuleRangeType { get; set; }

        Boolean OverrideRules { get; set; }
        
    }
}
