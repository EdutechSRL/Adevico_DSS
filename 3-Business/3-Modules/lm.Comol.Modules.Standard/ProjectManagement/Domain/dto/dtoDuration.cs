using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoDuration : IEquatable<dtoDuration>
    {
        public virtual Double Value { get; set; }
        public virtual Boolean IsEstimated { get; set; }
 
        public dtoDuration() { }
        public dtoDuration(Double value, Boolean isEstimated)
        {
            Value = value;
            IsEstimated = isEstimated;
        }

        public bool Equals(dtoDuration other)
        {
            return other.Value == Value && IsEstimated == other.IsEstimated;
        }
        public override string ToString()
        {
            return Value.ToString() + ((IsEstimated) ? "?" : "");
        }
    }
}