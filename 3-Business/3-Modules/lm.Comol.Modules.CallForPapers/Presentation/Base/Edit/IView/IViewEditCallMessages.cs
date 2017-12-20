using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditCallMessages : IViewBaseEditCall
    {
        String StartMessage { get; set; }
        String EndMessage { get; set; }

        void InitializeControl(string startMessage,String endMessage);
        void DisplayInvalidType();
        void DisplaySaveErrors(Boolean display);
        void DisplayInvalidMessage();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
    }
}