using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class dtoEvaluationsFilters
    {
        public virtual SubmissionsOrder OrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual EvaluationFilterStatus Status { get; set; }
        public virtual String SearchForName { get; set; }
        public virtual Dictionary<EvaluationStatus, String> TranslationsEvaluationStatus { get; set; }
        public virtual long IdSubmitterType { get; set; }
        public dtoEvaluationsFilters()
        {
            TranslationsEvaluationStatus = new Dictionary<EvaluationStatus, String>();
            IdSubmitterType = -1;
        }
    }

    [Serializable]
    public enum EvaluationFilterStatus
    {
        None = 0,
        Evaluating = 1,
        Evaluated = 2,
        Invalidated = 4,
        EvaluatorReplacement = 8,
        AllValid = 16,
        All = 32
    }
}