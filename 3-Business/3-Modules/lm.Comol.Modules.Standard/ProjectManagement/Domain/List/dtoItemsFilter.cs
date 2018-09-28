using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoItemsFilter : ICloneable
    {
        public long IdProject { get; set; }
        public ProjectFilterBy FilterBy { get; set; }
        public ItemListStatus ProjectsStatus { get; set; }
        public ItemListStatus ActivitiesStatus { get; set; }
        public ItemsGroupBy GroupBy { get; set; }
        public SummaryDisplay Display { get; set; }
        public SummaryTimeLine TimeLine { get; set; }
        public ProjectOrderBy OrderBy { get; set; }
        public Boolean Ascending { get; set; }
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public UserActivityStatus UserActivitiesStatus { get; set; }
        public SummaryTimeLine UserActivitiesTimeLine { get; set; }
        public PageContainerType Container { get; set; }
        
        //public static dtoItemsFilter GenerateForGroup(ItemsGroupBy groupBy, Int32 idCommunity = -1, long idProject =0) {
        public static dtoItemsFilter GenerateForGroup(PageContainerType pageContainer, ItemsGroupBy groupBy) {
            return GenerateForGroup(pageContainer, groupBy, -1, 0);
        }
        public static dtoItemsFilter GenerateForGroup(PageContainerType pageContainer, ItemsGroupBy groupBy, Int32 idCommunity, long idProject)
        {
            dtoItemsFilter filter = new dtoItemsFilter();
            switch (idCommunity) { 
                case 0:
                    filter.FilterBy = ProjectFilterBy.FromPortal;
                    break;
                case -1:
                    filter.FilterBy = ProjectFilterBy.All;
                    break;
                default:
                    filter.FilterBy = ProjectFilterBy.CurrentCommunity;
                    break;
            }
            filter.Container = pageContainer;
            filter.PageIndex = 0;
            filter.PageSize = 25;
            if (idProject == 0)
                filter.Display = SummaryDisplay.All;
            else
                filter.Display = SummaryDisplay.Project;
            filter.IdProject = idProject;

            filter.ProjectsStatus = (pageContainer == PageContainerType.ProjectDashboard) ? ItemListStatus.Late : ItemListStatus.All;
            filter.ActivitiesStatus = (pageContainer == PageContainerType.Dashboard) ? ItemListStatus.Late : ItemListStatus.All;
            filter.TimeLine = SummaryTimeLine.Week;
            filter.GroupBy = groupBy;
            switch (groupBy) { 
                case ItemsGroupBy.Plain:
                    filter.Ascending = true;
                    filter.OrderBy = ProjectOrderBy.Deadline;
                    break;
                case ItemsGroupBy.Community:
                    filter.Ascending = true;
                    filter.OrderBy = ProjectOrderBy.CommunityName;
                    break;
                case ItemsGroupBy.EndDate:
                    filter.Ascending = false;
                    filter.OrderBy = ProjectOrderBy.EndDate;
                    filter.PageIndex = -1;
                    break;
                case ItemsGroupBy.Project:
                    filter.Ascending = true;
                    filter.OrderBy = ProjectOrderBy.Name;
                    break;
                case ItemsGroupBy.CommunityProject:
                    filter.Ascending = true;
                    filter.OrderBy = ProjectOrderBy.CommunityName;
                    break;
            }
            return filter;
        }

        public ItemListStatus GetContainerStatus()
        {
            return GetStatus(Container);
        }
        public ItemListStatus GetStatus(PageContainerType pageContainer) {
            return (pageContainer == PageContainerType.ProjectsList) ? ProjectsStatus : ActivitiesStatus;
        }


        public object Clone()
        {
            dtoItemsFilter clone = new dtoItemsFilter();
            clone.FilterBy = FilterBy;
            clone.ProjectsStatus = ProjectsStatus;
            clone.ActivitiesStatus = ActivitiesStatus;
            clone.UserActivitiesStatus = UserActivitiesStatus;
            clone.UserActivitiesTimeLine = UserActivitiesTimeLine;
            clone.GroupBy = GroupBy;
            clone.Display = Display;
            clone.TimeLine = TimeLine;
            clone.OrderBy = OrderBy;
            clone.Ascending = Ascending;
            clone.PageIndex = PageIndex;
            clone.PageSize = PageSize ;
            clone.IdProject = IdProject;
            clone.Container = Container;
            return clone;
        }
    }
}