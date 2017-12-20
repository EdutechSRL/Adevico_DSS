using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewCommitteeSummary : IViewBaseSummary
    {
        long PreloadIdCommittee { get; }
        long CurrentIdCommittee { get; set; }
        Int32 EvaluatorsCount { get; set; }
        Boolean AllowBackToSummary { get; set; }
        Boolean AllowHideComments { get; set; }
        Int32 CurrentPageIndex { get; set; }

        void SetCommitteeName(String name);
        void DisplayCommitteesSummary(long idCall, Int32 idCommunity, dtoEvaluationsFilters filters);
        void DisplayEvaluationInfo(DateTime? endEvaluationOn);
        void LoadEvaluations(long currentCommittee, List<dtoCommittee> committees, List<dtoBaseCommitteeMember> evaluators, List<dtoCommitteeSummaryItem> statistics);
        void SetBackToSummary(String url);
       
    }
}