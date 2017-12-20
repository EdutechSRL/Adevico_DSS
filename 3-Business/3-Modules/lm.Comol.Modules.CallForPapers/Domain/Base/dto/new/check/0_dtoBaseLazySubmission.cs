using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoBaseLazySubmission : dtoBase
    {
        public virtual litePerson Owner { get; set; }
        public virtual SubmissionStatus Status { get; set; }
        public virtual DateTime? ExtensionDate { get; set; }
        public virtual System.Guid UniqueId { get; set; }

        public dtoBaseLazySubmission()
            : base()
        {
            Status = SubmissionStatus.none;
        }

        public dtoBaseLazySubmission(long id)
            : base(id)
        {
            Status = SubmissionStatus.none;
        }
    } 
}