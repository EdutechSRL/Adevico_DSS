using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoDssEvaluationResultItem
    {
        public virtual long IdObject { get; set; }
        public virtual long IdSubmission {get;set;}
        public virtual double Ranking { get; set; }
        public virtual double Value { get; set; }
        public virtual String ValueFuzzy { get; set; }
        public virtual DssEvaluationType Type { get; set; }
        public virtual Boolean IsFuzzyValue { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual Boolean IsValid { get; set; }
        public virtual Boolean HasEmptyValues { get; set; }
    }
}