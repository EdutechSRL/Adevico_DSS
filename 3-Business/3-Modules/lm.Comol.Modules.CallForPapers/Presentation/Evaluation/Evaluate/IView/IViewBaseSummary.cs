using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewBaseSummary : IViewBase
    {
        long PreloadIdCall { get; }
        int PreloadIdCommunity { get; }
        Int32 PreloadPageIndex { get; }
        Int32 PreloadPageSize { get; }

        CallStatusForSubmitters PreloadView { get; }
        Boolean PreloadAscending { get; }
        SubmissionsOrder PreloadOrderBy { get; }
        EvaluationFilterStatus PreloadFilterBy { get; }
        long PreloadIdSubmitterType { get; }
        String PreloadSearchForName { get; }
        dtoEvaluationsFilters PreloadFilters { get; }
        EvaluationType CurrentEvaluationType { get; set; }
        Boolean CallUseFuzzy { get; set; }
        
        CallForPaperType CallType { get; set; }
        Boolean AllowExportAll { get; set; }
        Boolean AllowExportCurrent { get; set; }
        Dictionary<SubmissionsOrder, Boolean> AvailableOrderBy { get; set; }
        
        long IdCall { get; set; }
        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        String Portalname { get; }
        String AnonymousDisplayname { get; }
        String UnknownDisplayname { get; }
        long IdCallAdvCommission { get; }

        dtoEvaluationsFilters CurrentFilters { get; set; }
        SubmissionsOrder CurrentOrderBy { get; set; }
        EvaluationFilterStatus CurrentFilterBy { get; set; }
        //long CurrentIdSubmitterType { get; set; }
        Boolean CurrentAscending { get; set; }
        Int32 PageSize { get; set; }
        PagerBase Pager { get; set; }
        void DisplayUnknownCall(int idCommunity, int idModule, long idCall);
        void DisplayNoEvaluationsFound();
        void DisplayEvaluationUnavailable();
        void DisplayDssWarning(DateTime? lastUpdate, Boolean isCompleted);
        void HideDssWarning();
        void SetContainerName(String callName, DateTime? endEvaluationOn);
        void SetActionUrl(CallStandardAction action, String url);
        void DisplaySessionTimeout(String url);
        void SendUserAction(int idCommunity, int idModule, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, ModuleRequestForMembership.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall,ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);

        void LoadAvailableSubmitterTypes(List<dtoSubmitterType> items, long selected);
        void LoadAvailableStatus(List<EvaluationFilterStatus> items, EvaluationFilterStatus selected);
        String GetItemEncoded(String name);
    }
}