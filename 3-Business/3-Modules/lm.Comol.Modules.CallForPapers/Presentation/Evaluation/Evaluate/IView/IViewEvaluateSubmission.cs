using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewEvaluateSubmission : IViewBase
    {
        long PreloadIdCall { get; }
        long PreloadedIdEvaluation { get; }
        int PreloadIdCommunity { get; }
        String GeneralTabName { get; }
        CallForPaperType CallType { get; set; }
        Boolean AllowEvaluate { get; set; }
        Boolean AllowCompleteEvaluation { get; set; }
        Boolean SavingForComplete { get; set; }
        //String SumbitterTypeName { set; }

        long IdCall { get; set; }
        long IdEvaluation { get; set; }
        long IdSubmission { get; set; }
        long IdEvaluator { get; set; }
        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        String Portalname { get; }
        String AnonymousOwnerName { get; }
        String UnknownDisplayname { get; }

        void DisplayError(EvaluationEditorErrors err);
        void DisplayWarning(EvaluationEditorErrors err, long criteriaCount);
        void DisplaySettingsSaved();
        void HideErrorMessages();

        List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated> GetCriteria();
        void DisplayUnknownCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void DisplayUnknownSubmission(int idCommunity, int idModule, long idSubmission, CallForPaperType type);
        void DisplayUnknownEvaluation(int idCommunity, int idModule, long idEvaluation);
        void DisplayEvaluationUnavailable();
        void DisplayNotEvaluationPermission();

        //void LoadCallInfo(dtoCall call);
        //void LoadCallInfo(dtoRequest call);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status, DateTime submittedOn);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status, DateTime submittedOn, string submittedBy);

        void InitializeEvaluationSettings(List<dtoCriterionForEvaluation> tabs, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation);
        void LoadSections(List<dtoCallSection<dtoSubmissionValueField>> sections);
        //void LoadAttachments(List<dtoAttachmentFile> items);
        void SetContainerName(String callName, String userName, String typeName, string submittedBy);

        void InitializeExportControl(Boolean isOwner, Int32 idUser, long idCall, long idSubmission, long idRevision, Int32 idModule, Int32 idCallCommunity, CallForPaperType callType);
        void GoToUrl(String url);
        void DisplaySessionTimeout(String url);


        void SendUserAction(int idCommunity, int idModule, long idEvaluation, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idEvaluation, ModuleRequestForMembership.ActionType action);


        void BindTag(String tags);
    }
}