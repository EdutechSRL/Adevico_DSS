using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewManageReview : IViewBase
    {
        SubmissionFilterStatus PreloadFilterSubmission { get; }
        SubmissionsOrder PreloadOrderSubmission { get; }
        Boolean PreloadAscending { get; }
        Boolean PreloadFromManagement { get; }
        CallStandardAction PreloadAction { get; }

        int PreloadPageIndex { get; }
        int PreloadPageSize { get; }
        long PreloadIdCall { get; }
        long PreloadedIdSubmission { get; }
        long PreloadedIdRevision { get; }
        int PreloadIdCommunity { get; }
        CallStatusForSubmitters PreloadView { get; }
        CallForPaperType CallType { get; set; }

        long IdCall { get; set; }
        long IdSubmission { get; set; }
        long IdRevision { get; set; }
        long IdSubmitterType { get; set; }
        RevisionStatus CurrentStatus { get; set; }
        List<long> FieldsToCheck { get; set; }
        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        Int32 IdUserSubmitter { get; set; }
        String Portalname { get; }
        String AnonymousOwnerName { get;  }


        List<dtoRevisionItem> GetFieldsToReview();

        void DisplayRevisionInfo(dtoRevisionRequest revision);
        void DisplayRevisionUnavailable();
        void DisplayRevisionUnknown();


        void LoadSections(List<dtoCallSection<dtoSubmissionValueField>> sections);
        //void SetContainerName(String communityName, String itemName);

        void LoadCallInfo(dtoRequest call);
        void LoadCallInfo(dtoCall call);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status, DateTime submittedOn);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status, DateTime submittedOn, string submittedBy);

        void SendUserAction(int idCommunity, int idModule, long idRevision, ModuleRequestForMembership.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idRevision, ModuleCallForPaper.ActionType action);
        void InitializeExportControl(Boolean isOwner, Int32 idUser, long idCall, long idSubmission, long idRevision, Int32 idModule, Int32 idCallCommunity, CallForPaperType callType);

        void DisplayRevisionError(RevisionErrorView errors);
        void SetActionUrl(String url);
        void GoToUrl(String url);
        void DisplaySessionTimeout(String url);
    }
}