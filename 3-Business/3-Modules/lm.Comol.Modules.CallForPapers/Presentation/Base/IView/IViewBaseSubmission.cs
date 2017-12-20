using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewBaseSubmission : IViewBase
    {
        Int32 PreloadIdOtherCommunity { get; }
        long PreloadIdCall { get; }
        long PreloadedIdSubmission { get; }
        //long PreloadedIdRevision { get; }
        long PreloadedIdSubmitter { get; }
        System.Guid PreloadedUniqueID { get; }
        int PreloadIdCommunity { get; }
        CallStatusForSubmitters PreloadView { get; }
        CallForPaperType CallType { get; set; }

        long IdCall { get; set; }
        long IdSubmission { get; set; }
        long IdRevision { get; set; }
        long IdSelectedSubmitterType { get; set; }
        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        String Portalname { get; }
        Boolean isAnonymousSubmission { get; set; }
        Boolean TryToComplete { get; set; }
        List<dtoSubmissionValueField> GetValues();
        List<dtoSubmissionFileValueField> GetFileValues(UserSubmission submission, String moduleCode, Int32 idModule, Int32 moduleAction, Int32 objectType);

        void DisableSubmitterTypesSelection();
        void LoadUnknowCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void LoadSections(List<dtoCallSection<dtoSubmissionValueField>> sections);
        void LoadSubmitterTypes(List<dtoSubmitterType> submitters);
        void LoadAttachments(List<dtoAttachmentFile> items);
        void SetContainerName(String communityName, String itemName);

        void SetActionUrl(CallStandardAction action, String url);
        void GoToUrl(CallStandardAction action, String url);
        void GoToUrl(String url);
        void DisplaySessionTimeout(String url);
    }
}
