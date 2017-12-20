using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewCommitteesSubmissionAssignments : IViewBaseEditEvaluationSettings
    {
        Int32 PreloadPageIndex { get; }
        Int32 PreloadPageSize { get; }
        Boolean PreloadAscending { get; }
        SubmissionsOrder PreloadOrderBy { get; }
        dtoSubmissionFilters PreloadFilters { get; }

        
        Int32 PageSize { get; set; }
        PagerBase Pager { get; set; }
        SubmissionsOrder CurrentOrderBy { get; set; }
        SubmissionFilterStatus CurrentFilterBy { get; set; }
        Boolean CurrentAscending { get; set; }


        Boolean AllowAssignEvaluatorsToAll { get; set; }
        Boolean AllowChangeAssignModeToEvalutors{ get; set; }
        Dictionary<long, Dictionary<long, String>> AvailableEvaluators { get; set; }
        dtoSubmissionFilters CurrentFilters { get; set; }

        void ReloadEditor(String url);
        List<dtoSubmissionMultipleAssignment> GetAssignments();

        void DisplayNoAvailableSubmission(long count, long rejected);
        void DisplayStartup(long count, long approved, long rejected);
        void LoadSubmissions(List<dtoSubmissionMultipleAssignment> assignments, Dictionary<long, Dictionary<long, String>> evaluators);
        void DisplayError(EvaluationEditorErrors err);
        void DisplayWarning(EvaluationEditorErrors err);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
    }
}