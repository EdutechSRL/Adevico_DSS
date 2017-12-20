using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Wizard
{
    [Serializable]
    public class dtoNavigableWizardItem<T>
    {
        public T Id { get; set; }
        public Int32 DisplayOrder { get; set; }
        public Boolean Visible { get; set; }
        public DisplayOrderEnum DisplayOrderDetail { get; set; }
        public WizardItemStatus Status { get; set; }
        public Boolean Active { get; set; }
        public Boolean IsSeparator { get; set; }
        public String Url { get; set; }
        public Boolean AutoPostBack { get; set; }
   
        public dtoNavigableWizardItem()
        {
            DisplayOrderDetail = DisplayOrderEnum.none;
        }
    }
}