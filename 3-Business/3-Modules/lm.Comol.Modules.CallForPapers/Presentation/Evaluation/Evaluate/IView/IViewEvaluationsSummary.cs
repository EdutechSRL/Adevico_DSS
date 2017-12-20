using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewEvaluationsSummary : IViewBaseSummary
    {
     
        void DisplayLinkToSingleCommittee(long idCommittee);  
        void DisplayEvaluationInfo(DateTime? endEvaluationOn);
        void LoadEvaluations(List<dtoEvaluationSummaryItem> items, Int32 committeesCount);

        void ShowCloseCommission(bool show);


        int minRange { get; set; }
        bool LockBool { get; set; }

        void SetStepSummaryLink(long StepId, long CommId, bool visible);
        //void DisplayByPermission(bool canClose, bool canViewSubmission);

    }
}