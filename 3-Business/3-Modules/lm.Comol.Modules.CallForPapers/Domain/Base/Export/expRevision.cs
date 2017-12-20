using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable()]
    public class expRevision : DomainObject<long>
    {
        public virtual Int32 Number { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual long IdSubmission { get; set; }
        public virtual RevisionType Type { get; set; }
        public virtual RevisionStatus Status { get; set; }

        public expRevision()
        {
        }
    }
}
