using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewPreviewCall : IViewPreviewBaseForPaper
    {
        void LoadCallInfo(dtoCall call);
        void LoadRequiredFiles(List<dtoCallSubmissionFile> files);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
    }
}
