using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation{
    public interface IViewAssignSubmissionWithNoEvaluation : IViewBaseEditEvaluationSettings
    {
        Boolean MultiComittees { get; set; }
        long IdCommittee { get; set; }
        Dictionary<long, Dictionary<long, String>> AvailableEvaluators { get; set; }
        List<dtoSubmissionAssignment> GetAssignments();
        void ReloadEditor(String url);

        void DisplayNoAvailableSubmission(long count, long rejected);
        void LoadSubmissions(List<dtoSubmissionAssignment> assignments, Dictionary<long, String> evaluators);
        void DisplayError(EvaluationEditorErrors err);
        void DisplayWarning(EvaluationEditorErrors err);
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
       

        SubmissionsOrder CurrentOrderBy { get; set; }
        Boolean CurrentAscending { get; set; }

        List<dtoSubmissionMultipleAssignment> GetMultipleAssignment();

        void DisplayStartup(long count, long approved, long rejected);
        void ConfirmSettings(List<dtoCommitteePartialAssignment> items);
        void LoadSubmissions(List<dtoSubmissionMultipleAssignment> assignments, Dictionary<long, Dictionary<long, String>> evaluators);
    }
}