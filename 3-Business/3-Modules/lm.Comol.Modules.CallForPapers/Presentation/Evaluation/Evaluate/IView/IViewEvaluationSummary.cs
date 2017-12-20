using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewEvaluationSummary : IViewBase
    {
        long PreloadIdCall { get; }
        long PreloadIdSubmission { get; }
        Int32 PreloadIdCommunity { get; }

        long AdvCommissionId { get; }
        
        Boolean AllowExportCurrent { get; set; }
        Boolean AllowExportAll { get; set; }

        long IdCall { get; set; }
        long IdCurrentCommittee { get; set; }
        long IdSubmission { get; set; }
        long IdRevision { get; set; }
        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        Int32 CommitteesCount { get; set; }
        String Portalname { get; }
        String AnonymousDisplayName { get; }
        String UnknownDisplayname { get; }

        void DisplayUnknownCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void DisplayEvaluationUnavailable();
        void DisplayNoEvaluationsFound();
        void DisplayNoPermissionToView();
        void DisplayUnknownSubmission(int idCommunity, int idModule, long idSubmission, CallForPaperType type);

        void DisplaySessionTimeout(String url);
        void SendUserAction(int idCommunity, int idModule, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, ModuleRequestForMembership.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall,long idSubmission, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, long idSubmission, ModuleRequestForMembership.ActionType action);

        void SetViewEvaluationsUrl(String url);
        void SetViewSubmissionUrl(String url);
        Boolean AllowHideComments { get; set; }

        void LoadSubmissionInfo(String callName, String submitterName, DateTime? submittedOn, String submittedBy, List<dtoCommitteeEvaluationInfo> committees, long idCommittee, bool isPresident, int minValue);
        void LoadEvaluations(List<dtoSubmissionCommitteeItem> evaluations);

        EvaluationType CurrentEvaluationType { get; set; }
        void DisplayDssWarning(DateTime? lastUpdate, Boolean isCompleted);
        void HideDssWarning();
        Boolean CallUseFuzzy { get; set; }
        Dictionary<long, Boolean> CommitteeIsFuzzy { get; set; }
    }
}