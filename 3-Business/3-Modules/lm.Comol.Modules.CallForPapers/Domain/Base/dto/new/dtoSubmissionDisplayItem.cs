using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmissionDisplayItem :dtoBase 
    {
        public virtual int PersonId { get; set; }
        public virtual String SubmitterName { get; set; }
        public virtual String SubmitterType { get; set; }
        public virtual SubmissionStatus Status { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual DateTime? LastActionOn { 
            get {
                if (SubmittedOn.HasValue)
                    return SubmittedOn.Value;
                else if (ModifiedOn.HasValue)
                    return ModifiedOn.Value;
                else if (CreatedOn.HasValue)
                    return CreatedOn.Value;
                else
                    return null;
            } 
        }
        public virtual SubmissionStatus LastActionStatus
        {
            get
            {
                return (SubmittedOn.HasValue && (Status == SubmissionStatus.valuated || Status == SubmissionStatus.valuating || Status == SubmissionStatus.waitingValuation)) ? SubmissionStatus.submitted : Status;
            }
        }
        public virtual double SumRating { get; set; }
        public virtual Boolean HasEvaluations { get; set; }
        public dtoSubmissionDisplayItem()
            : base()
        {
            Status = SubmissionStatus.none;
        }

        public dtoSubmissionDisplayItem(long id)
            : base(id)
        {
            Status = SubmissionStatus.none;
        }
    }
}