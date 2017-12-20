using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoDssRating
    {
        public virtual double Ranking { get; set; }
        public virtual double Value { get; set; }
        public virtual String ValueFuzzy { get; set; }
        public virtual Boolean IsFuzzy { get; set; }
        public virtual Boolean IsValid { get; set; }
        public virtual Boolean IsCompleted { get; set; }
    }
}