using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewRevisionList : IViewBase
    {
        CallForPaperType PreloadCallType { get; }
        CallStandardAction PreloadAction { get; }
        CallStatusForSubmitters PreloadView { get; }
        int PreloadIdCommunity { get; }
        Int32 PreloadPageIndex { get; }
        Int32 PreloadPageSize { get; }
        Boolean PreloadAscending { get; }
        RevisionOrder PreloadOrderBy { get; }
        String PreloadSearchForName { get; }
        dtoRevisionFilters PreloadFilters { get; }


        dtoRevisionFilters CurrentFilters { get; set; }
        String Portalname { get; }
        int IdCallCommunity { get; set; }

        CallForPaperType CallType { get; set; }
        CallStatusForSubmitters CurrentView { get; set; }
        CallStandardAction CurrentAction { get; set; }
        PagerBase Pager { get; set; }

        Boolean AllowManage { get; set; }
        Boolean AllowView { get; set; }

        void SetContainerName(int idCommunity, String name);
        void SetActionUrl(CallStandardAction action, String url);
        void LoadAvailableView(int idCommunity, List<CallStatusForSubmitters> views);

        void LoadNoRevisionsFound();
        void LoadRevisions(List<dtoRevisionDisplayItemPermission> revisions);
        void LoadRevisionStatus(List<RevisionStatus> items, RevisionStatus selected);
        void SendUserAction(int idCommunity, int idModule, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, ModuleRequestForMembership.ActionType action);
    }
}