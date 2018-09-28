using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace lm.Comol.Modules.EduPath.Domain
{
    public class EngineRuleCompletion<Telement> : RuleBase<Telement> where Telement : IRuleElement
    {
        ///// <summary>
        ///// Obsolete
        ///// </summary>
        //public virtual RuleRangeType RangeType { get; set; }

        ///// <summary>
        ///// Obsolete
        ///// </summary>
        //public virtual short MinValue { get; set; }
        ///// <summary>
        ///// Obsolete
        ///// </summary>
        //public virtual short MaxValue { get; set; }

        public const short LowerBound = 0;
        public const short UpperBound = 100;


        public virtual short CompletionMinValue { get; set; }
        public virtual short CompletionMaxValue { get; set; }
        public virtual short MarkMinValue { get; set; }
        public virtual short MarkMaxValue { get; set; }

        protected virtual Boolean CompletionEnabled
        {
            get
            {
                return CompletionMinValue != LowerBound || CompletionMaxValue != UpperBound;
            }
        }

        protected virtual Boolean MarkEnabled
        {
            get
            {
                return MarkMinValue != LowerBound || MarkMaxValue != UpperBound;
            }
        }

        public virtual ElementType ElementType { get; protected set; }

        #region Constructors
        
        public EngineRuleCompletion()
        {
            //RangeType = RuleRangeType.Between;
            CompletionType = CompletionType.NotNeeded;
        }

        //public EngineRuleCompletion(RuleRangeType type)
        //{
        //    RangeType = type;
        //    CompletionType = CompletionType.NotNeeded;
        //}

        public EngineRuleCompletion(short Cmin, short Cmax, short Mmin, short Mmax)
        {
            //RangeType = RuleRangeType.Between;
            //this.MinValue = min;
            //this.MaxValue = max;

            this.MarkMaxValue = Mmax;
            this.MarkMinValue = Mmin;
            this.CompletionMaxValue = Cmax;
            this.CompletionMinValue = Cmin;

            CompletionType = CompletionType.NotNeeded;
        }

        //public EngineRuleCompletion(RuleRangeType type, short value)
        //{
        //    this.RangeType = type;

        //    switch (type)
        //    {
        //        case RuleRangeType.GreaterThan:
        //            this.MinValue = value;
        //            break;
        //        case RuleRangeType.LowerThan:
        //            this.MaxValue = value;
        //            break;
        //        case RuleRangeType.Between:
        //            this.MinValue = value;
        //            this.MaxValue = UpperBound;
        //            break;
        //        default:
        //            break;
        //    }
        //    CompletionType = CompletionType.NotNeeded;
        //}
        #endregion
        
        public override bool Execute()
        {
            //Replaced
            //switch (RangeType)
            //{
            //    case RuleRangeType.GreaterThan:
            //        return (Source.Completed() || CompletionType == CompletionType.NotNeeded) && Source.UserCompletion >= this.MinValue && Source.UserCompletion <= UpperBound;
            //    case RuleRangeType.LowerThan:
            //        return (Source.Completed() || CompletionType == CompletionType.NotNeeded) && Source.UserCompletion <= this.MaxValue && Source.UserCompletion >= LowerBound;
            //    case RuleRangeType.Between:
            //        return (Source.Completed() || CompletionType == CompletionType.NotNeeded) && Source.UserCompletion <= this.MaxValue && Source.UserCompletion >= LowerBound && Source.UserCompletion >= this.MinValue && Source.UserCompletion <= UpperBound;
            //    default:
            //        return false;
            //}

            //Logica spostata all'esterno
            //Boolean retval = (Source.Completed() || CompletionType == CompletionType.NotNeeded);
            Boolean retval = true;

            retval = retval && Source.UserCompletion >= this.CompletionMinValue && Source.UserCompletion <= this.CompletionMaxValue;
            retval = retval && Source.UserMark >= this.MarkMinValue && Source.UserMark <= this.MarkMaxValue;

            return retval;
            
        }

        //public override string ToString()
        //{
        //    String description = "";

        //    switch (RangeType)
        //    {
        //        case RuleRangeType.GreaterThan:
                    
        //        case RuleRangeType.LowerThan:
                    
        //        case RuleRangeType.Between:
                    
                
                    
        //    }

        //    return description;
        //}
    }
}
