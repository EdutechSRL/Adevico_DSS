using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoDisplayTimelineContext : ICloneable
    {
        public long IdProject { get; set; }
        public Int32 IdContainerCommunity { get; set; }
        public ProjectFilterBy FilterBy { get; set; }
        public SummaryDisplay Display { get; set; }
        public dtoProjectContext ProjectContext { get; set; }
        public PageContainerType Container { get; set; }
        public PageListType FromPage { get; set; }
        public PageListType CurrentPage { get; set; }
        public SummaryTimeLine ContainerTimeline { get; set; }
        public ItemsGroupBy GroupBy { get; set; }
        public ItemListStatus Status { get; set; }
        

        public dtoDisplayTimelineContext() { }
        public dtoDisplayTimelineContext(dtoProjectContext context, Int32 idContainerCommunity, PageContainerType container, PageListType currentPage, PageListType fromPage, dtoItemsFilter filters, ItemsGroupBy groupBy = ItemsGroupBy.None)
        {
            FromPage = fromPage;
            ProjectContext = context;
            FromPage = fromPage;
            CurrentPage = currentPage;
            IdContainerCommunity = idContainerCommunity;
            Display = filters.Display;
            FilterBy = filters.FilterBy;
            ContainerTimeline = filters.TimeLine;
            Container = container;
            IdProject = filters.IdProject;
            if (groupBy != ItemsGroupBy.None)
                GroupBy = groupBy;
            else
                GroupBy = filters.GroupBy;
        }
        public dtoDisplayTimelineContext(dtoProjectContext context, Int32 idContainerCommunity, PageContainerType container, PageListType fromPage, SummaryTimeLine timeline, SummaryDisplay display, ProjectFilterBy filterBy = ProjectFilterBy.All, ItemsGroupBy groupBy = ItemsGroupBy.None, ItemListStatus status = ItemListStatus.All)
        {
            FromPage = fromPage;
            ProjectContext = context;
            FromPage = fromPage;
            CurrentPage = fromPage;
            IdContainerCommunity = idContainerCommunity;
            Display = display;
            FilterBy = filterBy;
            ContainerTimeline = timeline;
            Container = container;
            GroupBy = groupBy;
            Status = status;
        }
        public dtoDisplayTimelineContext(dtoProjectContext context, Int32 idContainerCommunity, PageContainerType container, PageListType currentPage, PageListType fromPage, SummaryTimeLine timeline, SummaryDisplay display, ProjectFilterBy filterBy = ProjectFilterBy.All, ItemsGroupBy groupBy = ItemsGroupBy.None, ItemListStatus status = ItemListStatus.All)
        {
            FromPage = fromPage;
            ProjectContext = context;
            FromPage = fromPage;
            CurrentPage = currentPage;
            IdContainerCommunity = idContainerCommunity;
            Display = display;
            FilterBy = filterBy;
            ContainerTimeline = timeline;
            Container = container;
            GroupBy = groupBy;
            Status = status;
        }

        public object Clone()
        {
            dtoDisplayTimelineContext clone = new dtoDisplayTimelineContext();
            clone.IdContainerCommunity = IdContainerCommunity;
            clone.FilterBy = FilterBy;
            clone.Display = Display;
            clone.ProjectContext = ProjectContext;
            clone.FromPage = FromPage;
            clone.CurrentPage = CurrentPage;
            clone.ContainerTimeline = ContainerTimeline;
            clone.Container = Container;
            clone.GroupBy = GroupBy;
            clone.Status = Status;
            clone.IdProject = clone.IdProject;
            return clone;
        }
    }
}