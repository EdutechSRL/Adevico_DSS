//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using lm.Comol.Core.DomainModel;

//namespace lm.Comol.Modules.CallForPapers.Domain
//{
//    [Serializable()]
//    public class FieldDisclaimerDefinition : FieldDefinition
//    {
//        public virtual Int32 MaxOption { get; set; }
//        public virtual Int32 MinOption { get; set; }
//        public virtual IList<FieldOption> Options { get; set; }
//        public virtual DisclaimerType DisclaimerType { get; set; }

//        public FieldDisclaimerDefinition()
//        {
//            Options = new List<FieldOption>();
//            DisclaimerType = DisclaimerType.Standard;
//        }
//    }
//}