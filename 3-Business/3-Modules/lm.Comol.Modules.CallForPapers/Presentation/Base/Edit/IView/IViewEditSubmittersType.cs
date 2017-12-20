using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditSubmittersType : IViewBaseEditCall
    {
        void LoadSubmitters(List<dtoSubmitterTypePermission> items);
        void DisplayError(SubmitterTypeError errors);
        void DisplayErrors(Dictionary<long,SubmitterTypeError> errors);
        void DisplayNoSubmitters();
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
    }
}