using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewSubmitRequest : IViewBaseSubmitCall
    {
        Boolean FromPublicList { get; }
        Boolean AllowSubmitterChange { set; }
        void LoadCallInfo(dtoRequest call);

        void SendStartSubmission(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
    }
}