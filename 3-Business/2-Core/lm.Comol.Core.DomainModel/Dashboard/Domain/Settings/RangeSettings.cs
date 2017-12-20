using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class RangeSettings:ICloneable, IDisposable 
    {
        public virtual Int32 LowerLimit { get; set; }
        public virtual Int32 HigherLimit { get; set; }
        public virtual Int32 DisplayItems { get; set; }

        public RangeSettings()
        {
        }

        public virtual Boolean IsInRange(Int32 count)
        {
            return (count >= LowerLimit && count <= HigherLimit);
        }

        public virtual Boolean IsValid(Int32 maxItems)
        {
            return (LowerLimit < HigherLimit && HigherLimit > 0 && DisplayItems >= maxItems && DisplayItems > 0);
        }
        public virtual object Clone()
        {
            return new RangeSettings() { DisplayItems = this.DisplayItems, HigherLimit = this.HigherLimit, LowerLimit = this.LowerLimit };
        }

        public void Dispose()
        {
            
        }
    }
}
