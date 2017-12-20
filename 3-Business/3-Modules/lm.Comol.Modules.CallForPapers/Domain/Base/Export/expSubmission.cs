using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export
{
    [Serializable()]
    public class expSubmission : DomainObject<long>
    {
        public virtual long IdCall { get; set; }
        public virtual expSubmitterType Type { get; set; }
        public virtual SubmissionStatus Status { get; set; }
        public virtual expPerson Owner { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual expPerson SubmittedBy { get; set; }
        public virtual Guid UserCode { get; set; }
        public virtual Boolean isAnonymous { get; set; }
        public virtual Boolean isComplete { get; set; }

        public virtual String OwnerDisplayName(String anonymousUser,String unknownUser)
        {
            return DisplayName(Owner, anonymousUser, unknownUser);
        }
        public virtual String SubmitterDisplayName(String anonymousUser, String unknownUser)
        {
            return DisplayName(SubmittedBy, anonymousUser, unknownUser);
        }
        public expSubmission()
        {
            Status = SubmissionStatus.draft;
        }
        protected virtual String DisplayName(expPerson p , String anonymousUser, String unknownUser)
        {
            return (p != null) ? p.SurnameAndName : ((isAnonymous) ?  anonymousUser :unknownUser);
        }
    }
}