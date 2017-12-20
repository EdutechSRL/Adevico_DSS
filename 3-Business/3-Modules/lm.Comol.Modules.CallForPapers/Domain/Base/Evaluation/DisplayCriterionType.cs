using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class DisplayCriterionType {
        public virtual Int32 Id { get; set; }
        public virtual CriterionType Type { get; set; }
        public virtual String Name { get; set; }
        //public virtual Boolean isGeneric {  
        //    get{
        //        return (Type == FieldType.SingleLine || Type == FieldType.Note || Type == FieldType.MultiLine || Type == FieldType.RadioButtonList || Type == FieldType.DropDownList || Type == FieldType.CheckboxList); 
        //    }
        //}
    }
}