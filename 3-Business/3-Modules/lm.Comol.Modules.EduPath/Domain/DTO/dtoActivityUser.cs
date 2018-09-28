using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoActivityUser : IRuleElement
    {
        public long Id { get; set; }

        public Int64 UserCompletion { get; set; }

        public short DisplayOrder { get; set; }

        public short Duration { get; set; }

        public DateTime? EndDate { get; set; }

        public Int64 MinCompletion { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public Status Status { get; set; }

        public Boolean MandatoryStepsCompleted { get; set; }

        public short OverrideRuleCompletionMinValue { get; set; }

        public short OverrideRuleCompletionMaxValue { get; set; }

        public bool OverrideRules { get; set; }

        public RuleRangeType OverrideRuleRangeType { get; set; }

        //activity data

        public Boolean Completed()
        {
            return this.MandatoryStepsCompleted && this.UserCompletion >= this.MinCompletion;
        }



        public short UserMark { get; set; }


        public short OverrideRuleMarkMinValue { get; set; }

        public short OverrideRuleMarkMaxValue { get; set; }
    }

}
