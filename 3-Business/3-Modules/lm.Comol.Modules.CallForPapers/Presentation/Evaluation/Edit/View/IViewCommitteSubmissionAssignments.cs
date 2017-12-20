using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewCommitteeSubmissionAssignments : IViewBaseEditEvaluationSettings
    {
        Boolean AllowAssignEvaluatorsToAll { get; set; }
        Boolean AllowChangeAssignModeToEvalutors { get; set; }
        long IdCommittee { get; set; }
        Dictionary<long, String> AvailableEvaluators { get; set; }
        List<dtoSubmissionAssignment> GetAssignments();
        void ReloadEditor(String url);

        void DisplayNoAvailableSubmission(long count, long rejected);
        void DisplayStartup(long count, long approved, long rejected);
        void LoadSubmissions(List<dtoSubmissionAssignment> assignments, Dictionary<long, String> evaluators);
        void DisplayError(EvaluationEditorErrors err);
        void DisplayWarning(EvaluationEditorErrors err);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
    }
}