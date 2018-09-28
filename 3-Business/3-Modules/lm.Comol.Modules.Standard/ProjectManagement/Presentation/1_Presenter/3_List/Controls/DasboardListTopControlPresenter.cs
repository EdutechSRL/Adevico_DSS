using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Modules.Standard.ProjectManagement.Business;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public class DasboardListTopControlPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceProjectManagement service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewDashBoardListTopControl View
            {
                get { return (IViewDashBoardListTopControl)base.View; }
            }
            private ServiceProjectManagement Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceProjectManagement(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleProjectManagement.UniqueCode);
                    return currentIdModule;
                }
            }
            public DasboardListTopControlPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DasboardListTopControlPresenter(iApplicationContext oContext, IViewDashBoardListTopControl view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoProjectContext context, Int32 idContainerCommunity, Boolean loadFromCookies, TabListItem tab, PageContainerType container, PageListType fromPage, PageListType currentPageType, ItemsGroupBy preloadGroupBy, ProjectFilterBy pFilterBy, ItemListStatus pProjectsStatus, ItemListStatus pActivitiesStatus, SummaryTimeLine pFilterTimeLine = SummaryTimeLine.Week, SummaryDisplay pFilterDisplay = SummaryDisplay.All, long idProject = 0, UserActivityStatus pUserActivitiesStatus = UserActivityStatus.Ignore, SummaryTimeLine pUserActivitiesTimeLine = SummaryTimeLine.Week) 
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            View.PageContext = context;
            View.PageContainer = container;
            View.PageType = currentPageType;
            View.CurrentFromPage = fromPage;
            View.CurrendIdProject = idProject;
            View.IdContainerCommunity = idContainerCommunity;
            View.CurrentActivityStatus = pUserActivitiesStatus;
            View.CurrentActivityTimeLine = pUserActivitiesTimeLine;
            if (UserContext.isAnonymous || p==null)
                View.DisplaySessionTimeout();
            else
            {
                View.CookieName = View.CookieStartName + container.ToString() + tab.ToString();
                dtoItemsFilter filter = null;
                if (loadFromCookies)
                {
                    filter = View.GetSavedFilters;
                    View.CurrentActivityStatus = filter.UserActivitiesStatus;
                    View.CurrentActivityTimeLine = filter.UserActivitiesTimeLine;
                    idProject = filter.IdProject;
                    View.CurrendIdProject = idProject;
                }
                if (filter == null)
                {
                    filter = dtoItemsFilter.GenerateForGroup(container,(preloadGroupBy == ItemsGroupBy.None) ? ItemsGroupBy.Plain : preloadGroupBy);
                    if (preloadGroupBy != ItemsGroupBy.None)
                    {
                        filter.FilterBy = pFilterBy;
                        filter.ProjectsStatus = pProjectsStatus;
                        filter.Display = pFilterDisplay;
                        filter.TimeLine = pFilterTimeLine;
                        filter.ActivitiesStatus = pActivitiesStatus;
                        filter.UserActivitiesStatus = pUserActivitiesStatus;
                        filter.UserActivitiesTimeLine = pUserActivitiesTimeLine;
                    }
                    filter.IdProject = idProject;
                }
                LoadFilters(p, context, filter, container, currentPageType,fromPage, pFilterTimeLine, idProject);
                View.SaveCurrentFilters(filter);
                View.InitializeTabs(Service.GetAvailableTabs(p.Id, context, container), tab, filter, context);
                LoadSummary(context, idContainerCommunity, currentPageType, fromPage, p, idProject, filter, pFilterTimeLine, pFilterDisplay, (context.IdCommunity > 0 ? context.IdCommunity : -100));
            }
        }

        #region "SummaryItems"
            public void RefreshSummary( dtoItemsFilter filter,long idProject)
            {
                Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
                if (UserContext.isAnonymous || p == null)
                    View.DisplaySessionTimeout();
                else {
                    dtoProjectContext context = View.PageContext;
                    LoadSummary(View.PageContext, View.IdContainerCommunity, View.PageType, View.CurrentFromPage , p, idProject, filter, View.CurrentTimeLine, View.CurrentDisplayMode, (context.IdCommunity > 0 ? context.IdCommunity : -100));
                }
            }
            private void LoadSummary(dtoProjectContext context, Int32 idContainerCommunity, PageListType currentPageType, PageListType fromPage, Person user, long idProject, dtoItemsFilter filter, SummaryTimeLine timeline, SummaryDisplay displayMode, Int32 idCommunity = -100)
            {
                if (idProject > 0)
                    LoadProjectSummary(context, idContainerCommunity, currentPageType, fromPage, user.Id, idProject, timeline, displayMode);
                else if (displayMode == SummaryDisplay.Filtered)
                {
                    if (filter.FilterBy != ProjectFilterBy.CurrentCommunity && filter.FilterBy != ProjectFilterBy.AllPersonalFromCurrentCommunity)
                        idCommunity = -100;
                    LoadSummary(context, idContainerCommunity, currentPageType, fromPage, user, timeline, displayMode, filter.FilterBy, filter.GetContainerStatus(), idCommunity);
                }
                else
                    LoadSummary(context, idContainerCommunity, currentPageType, fromPage, user, timeline, displayMode);
            }
            private void LoadProjectSummary(dtoProjectContext context, Int32 idContainerCommunity, PageListType currentPageType, PageListType fromPage, Int32 idPerson, long idProject, SummaryTimeLine timeline, SummaryDisplay display)
            {
                dtoProject project = Service.GetdtoProject(idProject);
                if (project != null)
                    View.DisplayProjectName(project.Name, Service.GetProjectAttachments(idProject, 0, false, View.UnknownUserTranslation, true));
                LoadSummary(context, idContainerCommunity, currentPageType, fromPage, Service.GetSummary(idPerson, idProject), timeline, display);
            }
            private void LoadSummary(dtoProjectContext context, Int32 idContainerCommunity, PageListType currentPageType, PageListType fromPage, Person person, SummaryTimeLine timeline, SummaryDisplay display, ProjectFilterBy filter = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, Int32 idCommunity = -100)
            {
                if (person.Id != UserContext.CurrentUserID)
                    View.DisplayUserName((person == null) ? "" : person.SurnameAndName);
                LoadSummary(context, idContainerCommunity, currentPageType, fromPage, Service.GetSummary(person.Id, filter, filterStatus, idCommunity), timeline, display);
            }
            private void LoadSummary(dtoProjectContext context, Int32 idContainerCommunity, PageListType currentPageType, PageListType fromPage, List<dtoTimelineSummary> items, SummaryTimeLine timeline, SummaryDisplay display)
            {
                List<dtoDisplayTimelineSummary> results = new List<dtoDisplayTimelineSummary>();
                if (items != null)
                {
                    List<dtoItemFilter<SummaryTimeLine>> timeLines = items.SelectMany(t => t.Activities.Select(a => a.TimeLine)).Distinct().Where(t => t != SummaryTimeLine.Today).Select(t => new dtoItemFilter<SummaryTimeLine>() { Value = t, Selected = (t == timeline) }).ToList();

                    if (timeLines.Count == 1)
                        timeLines[0].DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
                    else if (timeLines.Count > 1)
                    {
                        timeLines[0].DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first;
                        timeLines.Last().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
                    }
                    if (!timeLines.Where(t => t.Selected).Any())
                    {
                        timeline = timeLines.FirstOrDefault().Value;
                        timeLines.FirstOrDefault().Selected = true;
                    }

                    View.LoadTimeLines(timeLines);
                    List<dtoItemFilter<SummaryDisplay>> dItems = new List<dtoItemFilter<SummaryDisplay>>();
                    if (display == SummaryDisplay.Project)
                        dItems.Add(new dtoItemFilter<SummaryDisplay>() { DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last, Value = SummaryDisplay.Project, Selected = (display == SummaryDisplay.Project) });
                    else
                    {
                        if (View.PageContainer == PageContainerType.ProjectsList)
                            dItems.Add(new dtoItemFilter<SummaryDisplay>() { DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first | ItemDisplayOrder.last, Value = SummaryDisplay.All, Selected = true });
                        else
                        {
                            dItems.Add(new dtoItemFilter<SummaryDisplay>() { DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first, Value = SummaryDisplay.All, Selected = (display == SummaryDisplay.All) });
                            dItems.Add(new dtoItemFilter<SummaryDisplay>() { DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last, Value = SummaryDisplay.Filtered, Selected = (display == SummaryDisplay.Filtered) });
                        }
                    }
                    if (!dItems.Where(t => t.Selected).Any())
                        dItems.FirstOrDefault().Selected = true;
                    View.LoadDisplayMode(dItems);


                    UserActivityStatus aStatus = View.CurrentActivityStatus;
                    if (aStatus != UserActivityStatus.Ignore)
                    {
                        SummaryTimeLine aTimeline = View.CurrentActivityTimeLine;
                        if (items.Where(i => i.Activities.Where(a => a.Status == aStatus && a.TimeLine == aTimeline).Any()).Any())
                            View.CurrentStatus = ItemListStatus.Ignore;
                        else
                            View.CurrentActivityStatus = UserActivityStatus.Ignore;
                    }
                    dtoItemsFilter filters= View.GetCurrentFilters;
                    items.ForEach(i => results.Add(new dtoDisplayTimelineSummary(i,
                 new dtoDisplayTimelineContext(context, idContainerCommunity, View.PageContainer, currentPageType, fromPage, filters, (View.PageContainer != PageContainerType.ProjectsList && currentPageType == i.DashboardPage) ? ItemsGroupBy.None : ItemsGroupBy.Plain))));

                }
                
             
                View.LoadSummaries(results);
            }
        #endregion

        #region "Filters Load",
            private void LoadFilters(Person user, dtoProjectContext context, dtoItemsFilter filter, PageContainerType containerType, PageListType currentPageType, PageListType fromPage, SummaryTimeLine timeline, long idProject = 0)
            {
                LoadAvailableStatus(user, context, filter, containerType, currentPageType,fromPage, timeline, idProject);
                LoadAvailableGroupBy(context, filter, currentPageType, fromPage);
                if (containerType != PageContainerType.ProjectDashboard)
                    LoadAvailableFiltersBy(user, context, filter, currentPageType);
            }
            private void LoadAvailableStatus(Person user, dtoProjectContext context, dtoItemsFilter filter, PageContainerType containerType, PageListType currentPageType, PageListType fromPage, SummaryTimeLine timeline, long idProject = 0)
            {
                View.LoadStatusFilters(GetStatusItems(Service.GetAvailableFilterStatus(user, context, filter, containerType, currentPageType, timeline, idProject), context, filter, currentPageType, fromPage));
            }
            private List<dtoItemFilter<ItemListStatus>> GetStatusItems(List<ItemListStatus> availableStatus, dtoProjectContext context, dtoItemsFilter filter, PageListType currentPageType, PageListType fromPage)
            {
                Int32 idContainer = View.IdContainerCommunity;
                ItemListStatus current = filter.GetContainerStatus();
                switch (filter.Container) {
                    case PageContainerType.ProjectsList:
                        if (!availableStatus.Contains(current))
                            current = (availableStatus.Contains(ItemListStatus.Active) ? ItemListStatus.Active : availableStatus.First());
                        break;
                    default:
                        if (!availableStatus.Contains(current))
                            current = (availableStatus.Contains(ItemListStatus.Active) ? ItemListStatus.Active : availableStatus.First());
                        break;
                }
                List<dtoItemFilter<ItemListStatus>> items = (from i in availableStatus select new dtoItemFilter<ItemListStatus>() { Url = GenerateItemUrl(currentPageType, fromPage, filter, context, idContainer, filter.GroupBy, i), Value = i, Selected = (filter.GetContainerStatus() == i), DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item }).ToList();
                if (!items.Where(i => i.Selected).Any())
                {
                    items.First().Selected = true;
                    switch (filter.Container) { 
                        case PageContainerType.ProjectsList:
                            filter.ProjectsStatus = items.First().Value;
                            break;
                        default:
                            filter.ActivitiesStatus = items.First().Value;
                            break;
                    }
                }
                items.First().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first;
                items.Last().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
                return items;
            }
            private void LoadAvailableGroupBy(dtoProjectContext context, dtoItemsFilter filter, PageListType currentPageType, PageListType fromPage)
            {
                List<ItemsGroupBy> availableItems = new List<ItemsGroupBy>();

                switch (currentPageType)
                {
                    case PageListType.DashboardAdministrator:
                    case PageListType.DashboardManager:
                    case PageListType.DashboardResource:
                        availableItems.Add(ItemsGroupBy.CommunityProject);
                        availableItems.Add(ItemsGroupBy.Project);
                        availableItems.Add(ItemsGroupBy.Community);
                        break;
                    case PageListType.ProjectDashboardManager:
                    case PageListType.ProjectDashboardResource:
                        break;
                    default:
                        availableItems.Add(ItemsGroupBy.Community);
                        break;
                }
                    

                availableItems.Add(ItemsGroupBy.EndDate);
                availableItems.Add(ItemsGroupBy.Plain);
                if (!availableItems.Contains(filter.GroupBy))
                    filter.GroupBy = ItemsGroupBy.Plain;

                Int32 idContainer = View.IdContainerCommunity;
                List<dtoItemFilter<ItemsGroupBy>> items = (from i in availableItems select new dtoItemFilter<ItemsGroupBy>() { Url = GenerateItemUrl(currentPageType, fromPage, filter, context, idContainer, i, filter.GetContainerStatus(), View.CurrentActivityStatus), Value = i, Selected = (filter.GroupBy == i), DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item }).ToList();
                if (items.Count == 1)
                    items[0].DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
                else if (items.Any())
                {
                    items.First().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first;
                    items.Last().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
                }
                View.LoadGroupByFilters(items);
            }
            private void LoadAvailableFiltersBy(Person user, dtoProjectContext context, dtoItemsFilter filter, PageListType currentPageType)
            {
                List<ProjectFilterBy> fItems = Service.GetAvailableFilterBy(user.Id, context, currentPageType);
                if (!fItems.Contains(filter.FilterBy))
                    filter.FilterBy = ProjectFilterBy.All;
                String name = "";
                if (fItems.Contains(ProjectFilterBy.CurrentCommunity))
                {
                    Community community = CurrentManager.GetCommunity(context.IdCommunity);
                    if (community != null)
                        name = community.Name;
                }
                View.LoadFilterBy(fItems, filter.FilterBy, context.isForPortal, name);
            }
        #endregion

        public void Applyfilters(dtoItemsFilter filter, dtoProjectContext context, PageContainerType containerType, PageListType currentPageType, PageListType fromPage, Boolean summaryUpdate)
        {
            Person user = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || user ==null)
                View.DisplaySessionTimeout();
            else
            {
                long idProject = View.CurrendIdProject;
                Int32 idCommunity = View.IdContainerCommunity;
                if (summaryUpdate)
                    LoadSummary(context, idCommunity, currentPageType,fromPage, user, idProject, filter, filter.TimeLine, View.CurrentDisplayMode, (idCommunity > 0 ? idCommunity : -100));

                LoadFilters(user, context, filter, containerType, currentPageType, fromPage, filter.TimeLine, idProject);
                View.SaveCurrentFilters(filter);
            }
        }
        public void ReloadAvailableStatus(dtoItemsFilter filter, dtoProjectContext context, PageContainerType containerType, PageListType currentPageType, PageListType fromPage)
        {
            Person user = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || user == null)
                View.DisplaySessionTimeout();
            else
            {
                LoadAvailableStatus(user, context, filter, containerType, currentPageType, fromPage, filter.TimeLine);
            }
        }
        //public void AddDeletedStatus(ref dtoItemsFilter filter, dtoProjectContext context, PageContainerType itemsView, PageListType currentView, Boolean moveToStatus)
        //{
        //    if (moveToStatus) {
        //        filter.Status = ItemListStatus.Deleted;
        //        filter.PageIndex = 0;
        //        switch (filter.GroupBy) { 
        //            case ItemsGroupBy.Plain:
        //                filter.OrderBy = ProjectOrderBy.Name;
        //                filter.Ascending = true;
        //                break;
        //            case ItemsGroupBy.EndDate:
        //                filter.OrderBy = ProjectOrderBy.EndDate;
        //                filter.Ascending = true;
        //                break;
        //            case ItemsGroupBy.Community:
        //                filter.OrderBy = ProjectOrderBy.CommunityName;
        //                filter.Ascending = true;
        //                break;
        //        }
        //    }
        //    ItemListStatus status = filter.Status;
        //    List<ItemListStatus> sItems = Service.GetAvailableFilterStatus(CurrentManager.GetPerson(UserContext.CurrentUserID), context, filter, currentView);
        //    sItems.Add(ItemListStatus.Deleted);
           
        //    Int32 idCommunity = View.IdCurrentCommunityForList;

        //    View.InitializeSummary(UserContext.CurrentUserID, filter.TimeLine, filter.Display, filter.FilterBy, filter.Status, (idCommunity > 0 ? idCommunity : -100));
        //    View.LoadStatus(GetStatusItems(sItems, context, filter, currentView));
        //    View.LoadGroupBy(GetGroupByItems(filter, context, itemsView, currentView));

        //    View.SaveCurrentFilters(filter);
        //}
        public void RemoveDeletedStatus(ref dtoItemsFilter filter, dtoProjectContext context, PageContainerType containerType, PageListType currentPageType, PageListType fromPage)
        {
            Person user = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || user == null)
                View.DisplaySessionTimeout();
            else
            {
                long idProject = View.CurrendIdProject;
                List<ItemListStatus> avalableStatus = Service.GetAvailableFilterStatus(user, context, filter, containerType, currentPageType, filter.TimeLine, idProject);
                View.LoadStatusFilters(GetStatusItems(avalableStatus, context, filter, currentPageType, fromPage));
                if (!avalableStatus.Contains(ItemListStatus.Deleted))
                {
                    filter = dtoItemsFilter.GenerateForGroup(containerType,filter.GroupBy);
                    Int32 idCommunity = View.IdContainerCommunity;
                    LoadAvailableGroupBy(context, filter, currentPageType, fromPage);
                    LoadSummary(context, idCommunity, currentPageType,fromPage, user, idProject, filter, filter.TimeLine, View.CurrentDisplayMode, (idCommunity > 0 ? idCommunity : -100));
                    View.SaveCurrentFilters(filter);
                }
            }
       }
        public String GetDashboardUrl(PageListType pageType)
        {
            //switch (pageType) { 
            //}

            return "";
        }
        private String GenerateItemUrl(PageListType currentPageType, PageListType fromPage, dtoItemsFilter filter, dtoProjectContext context, Int32 idContainerCommunity, ItemsGroupBy groupBy, ItemListStatus status,UserActivityStatus activitiesStatus = UserActivityStatus.Ignore)
        {
            switch (currentPageType)
            {
                case PageListType.ListAdministrator:
                    return RootObject.ProjectListAdministrator(context.IdCommunity, context.isForPortal, context.isPersonal, false, 0, groupBy, filter.FilterBy, status, filter.TimeLine, filter.Display);
                case PageListType.ListManager:
                    return RootObject.ProjectListManager(context.IdCommunity, context.isForPortal, context.isPersonal, false, 0, groupBy, filter.FilterBy, status, filter.TimeLine, filter.Display);
                case PageListType.ListResource:
                    return RootObject.ProjectListResource(context.IdCommunity, context.isForPortal, context.isPersonal, false, 0, groupBy, filter.FilterBy, status, filter.TimeLine, filter.Display);
                case PageListType.DashboardManager:
                case PageListType.DashboardResource:
                case PageListType.DashboardAdministrator:
                    return RootObject.Dashboard(context, idContainerCommunity, View.PageContainer, fromPage, currentPageType, filter.TimeLine, filter.Display, filter.FilterBy, groupBy, status, activitiesStatus, View.CurrentActivityTimeLine);
                case PageListType.ProjectDashboardResource:
                case PageListType.ProjectDashboardManager:
                    return RootObject.ProjectDashboard(context, idContainerCommunity, filter.IdProject, fromPage, currentPageType, groupBy, status, filter.TimeLine,activitiesStatus, View.CurrentActivityTimeLine);
                
                default:
                    return "";
            }

        }
    }
}