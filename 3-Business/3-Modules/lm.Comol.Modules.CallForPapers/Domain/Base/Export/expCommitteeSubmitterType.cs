using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable]
    public class expCommitteeSubmitterType: DomainObject<long>
    {
        public virtual long IdSubmitterType { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public expCommitteeSubmitterType()
        {

        }
    }
}