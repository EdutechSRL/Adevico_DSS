using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmissionFilters
    {
        public virtual SubmissionsOrder OrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual SubmissionFilterStatus Status { get; set; }
        public virtual String SearchForName { get; set; }
        public virtual CallForPaperType CallType { get; set; }
        public virtual long  IdSubmitterType { get; set; }
        public virtual Dictionary<RevisionStatus, String> TranslationsRevision { get; set; }
        public virtual Dictionary<SubmissionStatus, String> TranslationsSubmission { get; set; }
        public virtual Dictionary<Evaluation.EvaluationStatus, String> TranslationsEvaluationStatus { get; set; }
        public dtoSubmissionFilters()
        {
            TranslationsRevision = new Dictionary<RevisionStatus, String>();
            TranslationsSubmission = new Dictionary<SubmissionStatus, String>();
            TranslationsEvaluationStatus = new Dictionary<Evaluation.EvaluationStatus, String>();
            IdSubmitterType = -1;
        }

    }  
}