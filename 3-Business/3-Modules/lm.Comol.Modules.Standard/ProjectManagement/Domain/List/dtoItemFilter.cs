using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoItemFilter<T>
    {
        public T Value { get; set; }
        public lm.Comol.Core.DomainModel.ItemDisplayOrder DisplayAs { get; set; }
        public Boolean Selected { get; set; }
        public String Url { get; set; }
        //public FilterContext Context { get; set; }
        public dtoItemFilter(){
            DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item;
        }
    }

    //[Serializable]
    //public class FilterContext
    //{
    //    public dtoItemsFilter Filters { get; set; }
    //    public dtoProjectContext Current { get; set; }
    //}  
}