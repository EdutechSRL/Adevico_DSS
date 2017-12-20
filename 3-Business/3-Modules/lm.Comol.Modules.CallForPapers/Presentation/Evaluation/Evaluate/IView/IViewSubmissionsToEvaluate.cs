using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public interface IViewSubmissionsToEvaluate : IViewBase
    {
        long PreloadIdCall { get; }
        long PreloadIdCommittee { get; }
        int PreloadIdCommunity { get; }
        CallStatusForSubmitters PreloadView { get; }
        //Int32 PreloadPageIndex { get; }
        //Int32 PreloadPageSize { get; }
        Boolean PreloadAscending { get; }
        long PreloadIdSubmitterType { get; }
        SubmissionsOrder PreloadOrderBy { get; }
        EvaluationFilterStatus PreloadFilterBy { get; }
        String PreloadSearchForName { get; }
        dtoEvaluationsFilters PreloadFilters { get; }


        CallForPaperType CallType { get; set; }
        Boolean AllowEvaluate { get; set; }
        Boolean AllowExportAll { get; set; }
        Boolean AllowExportCurrent { get; set; }
        Dictionary<SubmissionsOrder,Boolean > AvailableOrderBy { set; }
        
        long IdCall { get; set; }
        long IdEvaluator { get; set; }
        long IdCurrentCommittee { get; set; }

        long IdAdvCommittee { get; set; }

        Int32 IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }
        Int32 CriteriaCount { get; set; } 
        String Portalname { get; }
        String AnonymousDisplayname { get; }
        String UnknownDisplayname { get; }

        dtoEvaluationsFilters CurrentFilters { get; set; }
        EvaluationType CurrentEvaluationType { get; set; }
        SubmissionsOrder CurrentOrderBy { get; set; }
        //SubmissionFilterStatus CurrentFilterBy { get; set; }
        Boolean CurrentAscending { get; set; }

        //PagerBase Pager { get; set; }
        //Int32 PageSize { get; set; }

        void DisplayUnknownCall(int idCommunity, int idModule, long idCall, CallForPaperType type);
        void DisplayEvaluationInfo(DateTime? endEvaluationOn);
        void DisplayEvaluationUnavailable();
        void DisplayNotEvaluationPermission();
        String GetItemEncoded(String name);
        void LoadAvailableSubmitterTypes(List<dtoSubmitterType> items, long selected);
        void LoadAvailableStatus(List<EvaluationFilterStatus> items, EvaluationFilterStatus selected);
        void LoadEvaluationData(dtoBaseEvaluatorStatistics globalStat, List<dtoCommitteeEvaluationsInfo> committees, dtoEvaluatorCommitteeStatistic committeeStatistics, long idType , EvaluationFilterStatus status, String name);
        void SetContainerName(String callName, DateTime? endEvaluationOn);
        void SetActionUrl(CallStandardAction action, String url, bool isAdvance, string advUrl);
        void DisplaySessionTimeout(String url);

        void SendUserAction(int idCommunity, int idModule, long idEvaluation, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idEvaluation, ModuleRequestForMembership.ActionType action);
    }
}