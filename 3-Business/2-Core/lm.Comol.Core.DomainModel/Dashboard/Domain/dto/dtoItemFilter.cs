using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class dtoItemFilter<T>
    {
        public T Value { get; set; }
        public lm.Comol.Core.DomainModel.ItemDisplayOrder DisplayAs { get; set; }
        public Boolean Selected { get; set; }
        public String Url { get; set; }
        //public FilterContext Context { get; set; }
        public dtoItemFilter()
        {
            DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item;
        }
    }
}