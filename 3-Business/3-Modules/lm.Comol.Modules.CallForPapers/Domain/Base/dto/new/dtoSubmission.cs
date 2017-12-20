using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmission :dtoBase 
    {
        public virtual dtoSubmitterType Type { get; set; }
        public virtual long CallForPaperId { get; set; }
        public virtual SubmissionStatus Status { get; set; }
        public virtual Person Owner { get; set; }
        public virtual int PersonId { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual Person ModifiedBy { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual Person SubmittedBy { get; set; }
        public virtual DateTime? ExtensionDate { get; set; }
        public virtual ModuleLink LinkZip { get; set; }
        public dtoSubmission() :base() {
            Status = SubmissionStatus.none;
        }

        public dtoSubmission(long id)
            : base(id)
        {
            Status = SubmissionStatus.none;
        }
    }
}