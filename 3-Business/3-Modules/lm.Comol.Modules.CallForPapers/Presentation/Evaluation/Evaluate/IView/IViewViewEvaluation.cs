using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewViewEvaluation : IViewBase
    {
        long PreloadIdCall { get; }
        long PreloadIdEvaluation { get; }
        long PreloadIdSubmission { get; }
        long PreloadIdEvaluator { get; }
        int PreloadIdCommunity { get; }
        CallForPaperType CallType { get; set; }
      
        long IdCall { get; set; }
        long IdEvaluation { get; set; }
        long IdSubmission { get; set; }
        long IdEvaluator { get; set; }
        DisplayEvaluations CurrentDisplay { get; set; }
        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        String Portalname { get; }
        String AnonymousDisplayName { get; }
        String UnknonwUserName { get; }
        Boolean AllowPrint { get; set; }

        void DisplayUnknownCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void DisplayUnknownSubmission(int idCommunity, int idModule, long idSubmission, CallForPaperType type);
        void DisplayUnknownEvaluation(int idCommunity, int idModule, long idEvaluation);
        void DisplayEvaluationUnavailable();
        void DisplayNoEvaluationsToView();
        void DisplayNoPermissionToView();

        void LoadSubmissionInfo(string owner, string callName, DateTime? submittedOn,string submittedBy);
        void LoadEvaluatorInfo(String name, Int32 committeesCount);
        void LoadCommitteesStatus(List<dtoCommitteeEvaluationInfo> committees, DisplayEvaluations display);
        void SetViewEvaluationUrl(String url);

        void LoadEvaluation(dtoCommitteeEvaluation evaluation);
        void LoadEvaluations(List<dtoCommitteeEvaluation> evaluations);
        void LoadEvaluations(List<dtoCommitteeEvaluationsDisplayItem> comittees);
        void DisplaySessionTimeout(String url);
        void SendUserAction(int idCommunity, int idModule, long idEvaluator, long idEvaluation, long idSubmission, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idEvaluator, long idEvaluation, long idSubmission, ModuleRequestForMembership.ActionType action);

        EvaluationType CurrentEvaluationType { get; set; }
        Boolean CallUseFuzzy { get; set; }
        Dictionary<long, Boolean> CommitteeIsFuzzy { get; set; }
        void DisplayDssWarning(DateTime? lastUpdate, Boolean isCompleted);
        void HideDssWarning();

        long AdvCommId { get; }
    }
}