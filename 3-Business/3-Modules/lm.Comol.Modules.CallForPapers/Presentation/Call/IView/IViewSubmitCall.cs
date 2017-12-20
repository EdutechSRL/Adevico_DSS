using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewSubmitCall : IViewBaseSubmitCall
    {
        Boolean FromPublicList { get; }
        void LoadCallInfo(dtoCall call);
        void LoadRequiredFiles(List<dtoCallSubmissionFile> files);
        List<dtoRequestedFileUpload> GetRequiredSubmittedFiles(UserSubmission submission, String moduleCode, Int32 idModule, Int32 moduleAction, Int32 objectType);

        void SendStartSubmission(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);

        void SendToList();

        void SetTextForSignatures();
    }
}