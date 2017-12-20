using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable()]
    public class expSubmitterType : DomainObject<long>
    {
        public virtual String Name { get; set; }
        public virtual Int32 DisplayOrder { get; set; }

        public expSubmitterType(){
            Name = "";
        }
    }
}
