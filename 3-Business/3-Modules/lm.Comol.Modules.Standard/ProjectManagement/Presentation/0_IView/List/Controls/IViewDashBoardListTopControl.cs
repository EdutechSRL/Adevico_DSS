using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewDashBoardListTopControl : IViewBase
    {
        #region "Context"
            dtoProjectContext PageContext { get; set; }
            String PortalName { get; }
            TabListItem CurrentTab { get; set; }
            Int32 IdContainerCommunity { get; set; }
            PageContainerType PageContainer { get; set; }
            PageListType PageType { get; set; }
            PageListType CurrentFromPage { get; set; }
            String UnknownUserTranslation { get; }
        #endregion

        #region "Filters Context"
            String CookieName { get; set; }
            String CookieStartName { get; }

            SummaryDisplay CurrentDisplayMode { get; set; }
            SummaryTimeLine CurrentTimeLine { get; set; }
            ProjectFilterBy CurrentFilterBy { get; set; }
            ItemListStatus CurrentStatus { get; set; }
            ItemsGroupBy CurrentGroupBy { get; set; }

            UserActivityStatus CurrentActivityStatus { get; set; }
            SummaryTimeLine CurrentActivityTimeLine { get; set; }
            long CurrendIdProject { get; set; }
        #endregion

        void InitializeControl(dtoProjectContext context, Int32 idContainerCommunity, Boolean loadFromCookies, TabListItem tab, PageContainerType container, PageListType currentPageType, PageListType fromPage, ItemsGroupBy dGroupBy = ItemsGroupBy.None, ProjectFilterBy filterBy = ProjectFilterBy.All, ItemListStatus projectStatus = ItemListStatus.All, ItemListStatus activitiesStatus = ItemListStatus.All, SummaryTimeLine timeline = SummaryTimeLine.Week, SummaryDisplay display = SummaryDisplay.All, long idProject = 0, UserActivityStatus preloadActivityStatus = UserActivityStatus.Ignore, SummaryTimeLine preloadActivityTimeline = SummaryTimeLine.Week);
        void RefreshSummary(dtoItemsFilter filter,long idProject=0);
        void InitializeTabs(List<TabListItem> tabs, TabListItem selected,dtoItemsFilter filter, dtoProjectContext context);
        void DisplayUserName(String name);
        void DisplayProjectName(String name, List<dtoAttachmentItem> items);
        #region "Manage Filters"
            void SaveCurrentFilters(dtoItemsFilter filter);
            dtoItemsFilter GetSavedFilters { get; }
            dtoItemsFilter GetCurrentFilters { get; }
        #endregion

        /// <summary>
        /// Caricamento Filtri
        /// </summary>
        /// <param name="items"></param>
            /// 
        #region "Load Filters"
            void LoadDisplayMode(List<dtoItemFilter<SummaryDisplay>> items);
            void LoadTimeLines(List<dtoItemFilter<SummaryTimeLine>> items);
            void LoadSummaries(List<dtoDisplayTimelineSummary> items);


            void LoadGroupByFilters(List<dtoItemFilter<ItemsGroupBy>> items);
            void LoadStatusFilters(List<dtoItemFilter<ItemListStatus>> items);
            void LoadFilterBy(List<ProjectFilterBy> items, ProjectFilterBy selected, Boolean isPortal=false, String communityName ="");
        #endregion

    }
}