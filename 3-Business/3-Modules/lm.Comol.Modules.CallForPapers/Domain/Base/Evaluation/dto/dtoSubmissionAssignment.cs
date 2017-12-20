using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoSubmissionAssignment
    {
        public virtual long IdSubmission { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual long IdSubmitterType { get; set; }
        public virtual String SubmitterType { get; set; }
        public virtual DateTime? SubmittedOn { get; set; }
        public virtual List<long> Evaluators { get; set; }
   
        public dtoSubmissionAssignment()
        {
            Evaluators = new List<long>();
        }
        public dtoSubmissionAssignment(Dictionary<long, long> info)
        {
            Evaluators = info.Keys.ToList();
        }
    }
}