using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectDashboard : IViewBaseDashboard
    {
        long PreloadIdProject { get; }
        //PageListType PreloadToPage { get; }
        //TabListItem DefaultTab { get; }
        #region "Common"
        void InitializeProjectTopControl(dtoProjectContext context, Int32 idContainerCommunity, Boolean loadFromCookies, TabListItem tab, PageContainerType container, long idProject, ItemsGroupBy dGroupBy = ItemsGroupBy.None, ItemListStatus projectsStatus = ItemListStatus.All, ItemListStatus activitiesStatus = ItemListStatus.Late, SummaryTimeLine timeline = SummaryTimeLine.Week);
            void SetLinkToProjectsAsManager(String url);
            void SetLinkToProjectsAsResource(String url);
            void SetLinkToDashBoardAsManager(String url);
            void SetLinkToDashBoardAsResource(String url);
        #endregion
    }
}