using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewReviewSubmission : IViewBase
    {
        long PreloadIdCall { get; }
        long PreloadedIdSubmission { get; }
        long PreloadedIdRevision { get; }
        System.Guid PreloadedUniqueID { get; }
        int PreloadIdCommunity { get; }
        Int32 PreloadIdOtherCommunity { get; }
        CallStatusForSubmitters PreloadView { get; }
        CallForPaperType CallType { get; set; }
        lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier CallRepository { get; set; }
        long IdCall { get; set; }
        long IdSubmission { get; set; }
        long IdRevision { get; set; }
        long IdSubmitterType { get; set; }
        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        String Portalname { get; }
        String AnonymousOwnerName { get;  }
        Boolean TryToComplete { get; set; }
        DateTime InitSubmissionTime { get; set; }
        List<long> FieldsToReview { get; set; }
        Boolean AllowSave { get; set; }
        Boolean AllowCompleteSubmission { get; set; }
        Boolean AllowDeleteSubmission { get; set; }

        List<dtoSubmissionValueField> GetValues();
        List<dtoSubmissionFileValueField> GetFileValues(UserSubmission submission, String moduleCode, Int32 idModule, Int32 moduleAction, Int32 objectType);

        void InitializeView(Boolean displayProgress);
        void DisplayPendingRequest(dtoRevisionRequest revision);
        void DisplayRevisionTimeExpired();
        void DisplayCallUnavailableForPublic();
        void DisplayCallUnavailable();
        void DisplayRevisionUnavailable();
        void DisplayRevisionUnknown();
        void LoadError(RevisionErrorView error);

        void LoadUnknowCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void LoadSections(List<dtoCallSection<dtoSubmissionValueField>> sections);
        void LoadAttachments(List<dtoAttachmentFile> items);
        void SetContainerName(String communityName, String itemName);

        void LoadCallInfo(dtoRequest call);
        void LoadCallInfo(dtoCall call);

        void SendUserAction(int idCommunity, int idModule, long idRevision, ModuleRequestForMembership.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idRevision, ModuleCallForPaper.ActionType action);


        void SetActionUrl(CallStandardAction action, String url);
        //void GoToUrl(CallStandardAction action, String url);
        void GoToUrl(String url);
        void DisplaySessionTimeout(String url);
    }
}
