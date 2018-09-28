using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoTimelineSummary
    {
        public long Quantity { get { return (Activities.Any() ? Activities.Select(a => a.Quantity).Sum() : 0); } }
        public List<dtoTimelineActivity> Activities { get; set; }
        public lm.Comol.Core.DomainModel.ItemDisplayOrder DisplayAs { get; set; }
        public PageListType DashboardPage { get; set; }
        public dtoTimelineSummary()
        {
            DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item;
            Activities = new List<dtoTimelineActivity>();
        }
    }
    [Serializable]
    public class dtoTimelineActivity
    {
        public long IdProject { get; set; }
        public long Quantity { get; set; }
        public UserActivityStatus Status { get; set; }
        public SummaryTimeLine TimeLine { get; set; }
        public PageListType DashboardPage { get; set; }
        public dtoTimelineActivity() {
        }
    }


    [Serializable]
    public class dtoDisplayTimelineSummary
    {
        public long Quantity { get { return (Activities.Any() ? Activities.Select(a => a.Quantity).Sum() : 0); } }
        public List<dtoDisplayTimelineActivity> Activities { get; set; }
        public lm.Comol.Core.DomainModel.ItemDisplayOrder DisplayAs { get; set; }
        public PageListType ToPage { get; set; }
        public dtoDisplayTimelineSummary()
        {
            DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item;
            Activities = new List<dtoDisplayTimelineActivity>();
        }
        public dtoDisplayTimelineSummary(dtoTimelineSummary item, dtoDisplayTimelineContext context)
        {
            DisplayAs = item.DisplayAs;
            ToPage = item.DashboardPage;
            Activities = item.Activities.Where(a => a.TimeLine == SummaryTimeLine.Today || a.TimeLine == context.ContainerTimeline).Select(a => new dtoDisplayTimelineActivity(a, GenerateUrl(a, context)) { ToPage = GetDashboardPageByContainer(context.Container,item.DashboardPage) }).ToList();
            if (Activities.Count == 1)
                Activities[0].DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
            else if (Activities.Any())
            {
                Activities.First().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first;
                Activities.Last().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
            }
        }
        private PageListType GetDashboardPageByContainer(PageContainerType container, PageListType page)
        {
            switch (container)
            {
                case PageContainerType.ProjectDashboard:
                    return (page == PageListType.DashboardResource) ? PageListType.ProjectDashboardResource : PageListType.ProjectDashboardManager;
                default:
                    return page;
            }
        }
        private String GenerateUrl(dtoTimelineActivity activity, dtoDisplayTimelineContext context)
        {
            return RootObject.Dashboard(context, context.IdProject, GetDashboardPageByContainer(context.Container,activity.DashboardPage), activity.Status, activity.TimeLine);
            //switch(display){
            //    case SummaryDisplay.All:
            //        return RootObject.Dashboard(context,fromPage,idContainerCommunity, activity.DashboardPage, timeline, activity.IdProject, ItemsGroupBy.Plain, ProjectFilterBy.All, ItemListStatus.Late, display);
            //    case SummaryDisplay.Filtered:
            //        return RootObject.Dashboard(context, fromPage, idContainerCommunity, activity.DashboardPage, timeline, activity.IdProject, ItemsGroupBy.Plain, ProjectFilterBy.All, activity.Status , display);
            //    case SummaryDisplay.Project:
            //        return null;
            //}
        }
        //public dtoDisplayTimelineSummary(dtoTimelineSummary item, SummaryTimeLine timeline)
        //{
        //    DisplayAs = item.DisplayAs;
        //    ToPage = item.DashboardPage;
        //    Activities = item.Activities.Where(a => a.TimeLine == SummaryTimeLine.Today || a.TimeLine == timeline).Select(a => new dtoDisplayTimelineActivity(a, "")).ToList();
        //    if (Activities.Count == 1)
        //        Activities[0].DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
        //    else if (Activities.Any())
        //    {
        //        Activities.First().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first;
        //        Activities.Last().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last;
        //    }
        //}
    }
    [Serializable]
    public class dtoDisplayTimelineActivity
    {
        public long Quantity { get; set; }
        public PageListType ToPage { get; set; }
        public UserActivityStatus Status { get; set; }
        public SummaryTimeLine TimeLine { get; set; }
        public String DestinationUrl { get; set; }
        public lm.Comol.Core.DomainModel.ItemDisplayOrder DisplayAs { get; set; }
        public dtoDisplayTimelineActivity()
        {
            DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item;
        }

        public dtoDisplayTimelineActivity(dtoTimelineActivity dto, String url)
        {
            Quantity = dto.Quantity;
            Status = dto.Status;
            TimeLine = dto.TimeLine;
            DestinationUrl = url;
            DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item;
        }
    }
}