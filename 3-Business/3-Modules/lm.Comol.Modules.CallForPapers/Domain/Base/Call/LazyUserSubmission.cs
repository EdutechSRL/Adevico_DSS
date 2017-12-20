using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class LazyUserSubmission
    {
        public virtual long Id { get; set; }
        public virtual BaseForPaper Call { get; set; }
        public virtual SubmitterType Type { get; set; }
        public virtual SubmissionStatus Status { get; set; }
        public virtual litePerson Owner { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual byte[] TimeStamp { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual ModuleLink LinkZip { get; set; }
        public LazyUserSubmission()
        {
            Status = SubmissionStatus.draft;
        }

    }
}
