using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewCommitteesSummary : IViewBaseSummary
    {
        List<dtoCommittee> CurrentCommittees { get; set; }
        Int32 CommitteesCount { get; set; }
        Boolean AllowBackToSummary { get; set; }
        long PreloadIdCommittee { get; }
        Int32 CurrentPageIndex { get; set; }
        void DisplayEvaluationInfo(DateTime? endEvaluationOn);
        void DisplaySingleCommittee(long idCommittee, long idCall, Int32 idCommunity, dtoEvaluationsFilters filters);
        void LoadEvaluations(List<dtoCommittee> committees, List<dtoCommitteesSummaryItem> statistics);
        void SetBackToSummary(String url);

        void RedirectToAdvance(long CallId);
    }
}