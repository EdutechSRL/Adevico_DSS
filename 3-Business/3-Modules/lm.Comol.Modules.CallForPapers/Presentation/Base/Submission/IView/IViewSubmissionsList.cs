using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewSubmissionsList : IViewBase
    {
        CallForPaperType PreloadCallType { get; }
        CallStandardAction PreloadAction { get; }
        CallStatusForSubmitters PreloadView { get; }
        long PreloadIdCall { get; }
        long PreloadIdSubmission{ get; }
        long PreloadIdRevision { get; }
        int PreloadIdCommunity { get; }
        Int32 PreloadPageIndex { get; }
        Int32 PreloadPageSize { get; }
        Boolean PreloadAscending { get; }
        SubmissionsOrder PreloadOrderBy { get; }
        SubmissionFilterStatus PreloadFilterBy { get; }
        String PreloadSearchForName { get; }
        dtoSubmissionFilters PreloadFilters { get; }

        dtoSubmissionFilters CurrentFilters { get; set; }
        String Portalname { get; }
        int IdCallCommunity { get; set; }
        Int32 IdCallModule { get; set; }

        CallForPaperType CallType { get; set; }
        CallStatusForSubmitters CurrentView { get; set; }
        CallStandardAction CurrentAction { get; set; }
        SubmissionsOrder CurrentOrderBy { get; set; }
        SubmissionFilterStatus CurrentFilterBy { get; set; }
        Boolean CurrentAscending { get; set; }

        bool IsAdvance { get; set; }
        PagerBase Pager { get; set; }
        long IdCall  { get; set; }
        Int32 PageSize {get; set; }

        Boolean AllowManage { get; set; }
        Boolean AllowView { get; set; }
        Boolean AllowExport { get; set; }
     
        void SetContainerName(String callName,String edition, CallForPaperType type);
        void SetActionUrl(CallStandardAction action, String url);

        void DisplayUnknownCall();
        void LoadNoSubmissionsFound();
        void LoadSubmissions(List<dtoSubmissionDisplayItemPermission> submissions);
        void LoadSubmissionStatus(List<SubmissionFilterStatus> items, SubmissionFilterStatus selected);

        void SendUserAction(int idCommunity, int idModule, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, ModuleRequestForMembership.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idSubmission, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idSubmission, ModuleRequestForMembership.ActionType action);
        void GotoUrl(string url);

        Boolean HasSignature { get; set; }
    }
}