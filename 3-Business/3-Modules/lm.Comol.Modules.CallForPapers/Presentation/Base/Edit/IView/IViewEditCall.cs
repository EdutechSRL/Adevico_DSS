using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditCall : IViewBaseEditCall
    {
        CallStandardAction PreloadAction { get; }
        CallForPaperStatus CurrentStatus { get; set; }
        CallStandardAction CurrentAction { get; set; }
        void GoToUrl(CallStandardAction action, String url);
       

        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
        //void SendUserActionOnField(int idCommunity, int idModule, long idCall, long idField, ModuleCallForPaper.ActionType action);
        //void SendUserActionOnSection(int idCommunity, int idModule, long idCall, long idSection, ModuleCallForPaper.ActionType action);
        //void LoadInvalidStatus(CallForPaperStatus status, DateTime? endDate);
    }
}