using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    [Serializable]
    public class GenericOrderItem<T>
    {
        public virtual T Item { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual ItemDisplayOrder DisplayAs { get; set; }
    }
}
