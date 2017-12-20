using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewViewBaseSubmission : IViewBase
    {
        long PreloadIdCall { get; }
        long PreloadedIdSubmission { get; }
        long PreloadedIdRevision { get; }
        Int32 PreloadIdOtherCommunity { get; }
        System.Guid PreloadedUniqueID { get; }
        int PreloadIdCommunity { get; }
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
        String AnonymousOwnerName { get; }
        Boolean isAnonymousSubmission { get; set; }

        void DisplayUnknownCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void DisplayUnknownSubmission(int idCommunity, int idModule, long idSubmission, CallForPaperType type);
        void DisplayCallUnavailableForPublic();
        void DisplaySubmissionUnavailable();

        void LoadCallInfo(dtoCall call);
        void LoadCallInfo(dtoRequest call);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status, DateTime submittedOn);
        void LoadSubmissionInfo(string submitterName, string ownerName, SubmissionStatus status, DateTime submittedOn, string submittedBy);
      
        void LoadSections(List<dtoCallSection<dtoSubmissionValueField>> sections);
        void LoadAttachments(List<dtoAttachmentFile> items);
        void LoadRequiredFiles(List<dtoCallSubmissionFile> files);
        void SetContainerName(String communityName, String itemName);

        //void SetBackUrlToManagement(int communityID, long callForPaperId, CallStatusForSubmitters view, FilterSubmission filter, OrderSubmission order, int pageIndex);


        void InitializeExportControl(
            Boolean isOwner,
            Int32 idUser, 
            long idCall,
            long idSubmission, 
            long idRevision, 
            Int32 idModule, 
            Int32 idCallCommunity, 
            CallForPaperType callType,
            Int64 SubmitterType,
            bool IsDraft);

        void SetActionUrl(CallStandardAction action, String url);
        void GoToUrl(String url);
        void DisplaySessionTimeout(String url);

        void DisplayOutOfTime(String Info, bool ShowMessage, bool showAsError);

        void SendUserAction(int idCommunity, int idModule, long idSubmission, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idSubmission, ModuleRequestForMembership.ActionType action);
    }
}
