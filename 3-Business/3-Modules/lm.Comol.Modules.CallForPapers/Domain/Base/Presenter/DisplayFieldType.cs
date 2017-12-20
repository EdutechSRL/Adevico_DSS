using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class DisplayFieldType {
        public virtual String Identifyer { get { return String.Format("{0}.{1}", (Int32)Type, (Int32)DisclaimerType); } }
        public virtual FieldType Type { get; set; }
        public virtual String Name { get; set; }
        public virtual DisclaimerType DisclaimerType { get; set; }
        public virtual Boolean isGeneric {  
            get{
                return (Type == FieldType.SingleLine || Type == FieldType.Note || Type == FieldType.MultiLine || Type == FieldType.RadioButtonList || Type == FieldType.DropDownList || Type == FieldType.CheckboxList); 
            }
        }
    }
}