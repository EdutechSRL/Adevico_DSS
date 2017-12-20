using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewPreviewRequest : IViewPreviewBaseForPaper
    {
        void LoadCallInfo(dtoRequest call);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
    }
}
