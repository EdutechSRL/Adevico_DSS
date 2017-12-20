using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewFinalMessage : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long PreloadIdCall { get; }
        long PreloadedIdSubmission { get; }
        long PreloadedIdRevision { get; }
        Int32 PreloadIdOtherCommunity { get; }
        System.Guid PreloadedUniqueID { get; }
        int PreloadIdCommunity { get; }
        CallStatusForSubmitters PreloadView { get; }
        CallForPaperType CallType { get; set; }

        long IdCall { get; set; }
        long IdSubmission { get; set; }
        long IdRevision { get; set; }
        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        String Portalname { get; }
        Boolean isAnonymousSubmission { get; set; }
        Boolean FromPublicList { get; }

        void SetActionUrl(CallStandardAction action, String url);
        void LoadUnknowSubmission(int idCommunity, int idModule, long idSubmission, CallForPaperType type);
        void LoadUnknowCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void LoadDefaultMessage();
        void LoadMessage(String message);
        void GoToUrl(String url);

        void InitializeView(lm.Comol.Core.DomainModel.Helpers.ExternalPageContext skin);
        void SetContainerName(String communityName, String itemName);
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
    }
}
