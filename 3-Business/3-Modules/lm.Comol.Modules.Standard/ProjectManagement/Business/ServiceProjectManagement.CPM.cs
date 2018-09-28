using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using System.Text.RegularExpressions;

namespace lm.Comol.Modules.Standard.ProjectManagement.Business
{
    public partial class ServiceProjectManagement : BaseCoreServices
    {
        public CPMresult DefaultCPM(Boolean allowSummary, List<litePmActivity> activities, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null, Int32 dailyworkhours = 8)
        {
            foreach (var item in activities)
            {
                item.EarlyFinish = 0;
                item.EarlyStart = 0;
                item.LatestFinish = 0;
                item.LatestStart = 0;
            }
            if (allowSummary)
                return CPMWithSummary(GetDetachedActivities(activities), startdate, workweek, exceptions, dailyworkhours);
            else
                return CPM(GetDetachedActivities(activities),  startdate, workweek, exceptions, dailyworkhours);
        }
        public CPMresult DefaultCPM(Boolean allowSummary, List<PmActivity> activities, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null, Int32 dailyworkhours = 8)
        {
            foreach (var item in activities)
            {
                item.EarlyFinish = 0;
                item.EarlyStart = 0;
                item.LatestFinish = 0;
                item.LatestStart = 0;
            }
            if (allowSummary)
                return CPMWithSummary(GetDetachedActivities(activities), startdate, workweek, exceptions, dailyworkhours);
            else
                return CPM(GetDetachedActivities(activities), startdate, workweek, exceptions, dailyworkhours);
        }
        public CPMresult DefaultCPM(Boolean allowSummary, List<dtoCPMactivity> activities, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null, Int32 dailyworkhours = 8)
        {
            foreach (var item in activities)
            {
                item.EarlyFinish = 0;
                item.EarlyStart = 0;
                item.LatestFinish = 0;
                item.LatestStart = 0;
            }
            if (allowSummary)
                return CPMWithSummary(activities, startdate, workweek, exceptions, dailyworkhours);
            else
                return CPM(activities, startdate, workweek, exceptions, dailyworkhours);
        }
        private IEnumerable<dtoCPMactivity> GetDetachedActivities(List<litePmActivity> activities)
        {
            List<dtoCPMactivity> items = activities.Select(a => new dtoCPMactivity(a)).ToList();
            Dictionary<long, dtoCPMactivity> dict = items.ToDictionary(a => a.Id, a=>a);
            foreach (dtoCPMactivity act in items) {
                act.Children = act.IdChildren.Select(c => dict[c]).ToList();
                foreach (dtoCPMlink l in act.Predecessors) { l.Source= dict[l.IdSource]; l.Target= dict[l.IdTarget];}
                foreach (dtoCPMlink l in act.Successors) { l.Source= dict[l.IdSource]; l.Target= dict[l.IdTarget];}
            }
            return items.AsEnumerable<dtoCPMactivity>();
        }
        private IEnumerable<dtoCPMactivity> GetDetachedActivities(List<PmActivity> activities)
        {
            List<dtoCPMactivity> items = activities.Select(a => new dtoCPMactivity(a)).ToList();
            Dictionary<long, dtoCPMactivity> dict = items.ToDictionary(a => a.Id, a => a);
            foreach (dtoCPMactivity act in items)
            {
                act.Children = act.IdChildren.Select(c => dict[c]).ToList();
                foreach (dtoCPMlink l in act.Predecessors) { l.Source = dict[l.IdSource]; l.Target = dict[l.IdTarget]; }
                foreach (dtoCPMlink l in act.Successors) { l.Source = dict[l.IdSource]; l.Target = dict[l.IdTarget]; }
            }
            return items.AsEnumerable<dtoCPMactivity>();
        }
        private CPMresult DefaultCPM(Boolean allowSummary, IEnumerable<dtoCPMactivity> activities, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null, Int32 dailyworkhours = 8, IEnumerable<dtoCPMlink> links = null)
        {
            foreach (var item in activities)
            {
                item.EarlyFinish = 0;
                item.EarlyStart = 0;
                item.LatestFinish = 0;
                item.LatestStart = 0;
            }

            if (allowSummary)
                return CPMWithSummary(activities, startdate, workweek, exceptions, dailyworkhours, links);
            else
                return CPM(activities,  startdate, workweek, exceptions, dailyworkhours, links);
        }
        private CPMresult CPM(IEnumerable<dtoCPMactivity> activities, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null, Int32 dailyworkhours = 8, IEnumerable<dtoCPMlink> links = null)
        {
            CPMresult result = new CPMresult();

            Dictionary<Int64, dtoCPMactivity> dict = new Dictionary<long, dtoCPMactivity>();

            foreach (var act in activities)
            {
                dict.Add(act.Id, act);
            }

            result.Activities = activities.ToList();

            if (!result.Activities.Where(x => x.Id == -1).Any())
            {
                dtoCPMactivity vActivity = new dtoCPMactivity(-1, "end") { Duration = 0, IsVirtual = true };
                result.Activities.Add(vActivity);
                dict.Add(-1, vActivity);

                CPMExtensions.ParseActivityLinks(-1, "*", dict);
            }

            result.Activities.CriticalPathMethod();

            result.ProjectLength = activities.CriticalPathLength();

            if (startdate != null && startdate.HasValue)
            {
                result.AvailableDates = startdate.Value.GenerateAvailableDates(result.ProjectLength, workweek, exceptions).ToList();
                result.Activities.CalculateDate(result.AvailableDates, dailyworkhours);
                result.ProjectStartDate = startdate;
                result.ProjectEndDate = result.Activities.CriticalPathEnd();
                result.isDeadlined = result.Activities.isDeadlined();
                result.DeadlinedActivities = result.Activities.DeadlinedActivities().ToList();
            }

            result.Critical = activities.CriticalPath().ToList();
            result.Activities = result.Activities.Where(x => x.Id > 0).ToList();

            foreach (var item in result.Activities)
            {
                //todo check if it's correct

                item.Successors = item.Successors.Where(x => x.isVirtual == false).ToList();

            }

            //todo remove virtual links and activities

            return result;
        }
        private CPMresult CPMWithSummary(IEnumerable<dtoCPMactivity> activities, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null, Int32 dailyworkhours = 8, IEnumerable<dtoCPMlink> links = null)
        {
            CPMresult result = new CPMresult();

            Dictionary<Int64, dtoCPMactivity> dict = new Dictionary<long, dtoCPMactivity>();

           // activities = PreprocessSummaryMilestones(activities.ToList()).AsEnumerable();

            foreach (var act in activities)
            {
                dict.Add(act.Id, act);
            }

            result.Activities = activities.ToList();

            if (!result.Activities.Where(x => x.Id == -1).Any())
            {
                dtoCPMactivity vActivity = new dtoCPMactivity(-1, "end") { Duration = 0, IsVirtual = true };
                result.Activities.Add(vActivity);
                dict.Add(-1, vActivity);

                CPMExtensions.ParseActivityLinks(-1, "*", dict);
            }

            result.Activities.Where(x => x.IsSummary == false).CriticalPathMethod();
            //result.Activities.CriticalPathMethod();

            if (result.Activities.Where(x => x.IsSummary).Any())
            {
                var list = result.Activities.Reverse();
                foreach (var item in list)
                {
                    if (item.IsSummary)
                    {
                        item.EarlyStart = item.Children.Min(x => x.EarlyStart);
                        item.EarlyFinish = item.Children.Max(x => x.EarlyFinish);

                        item.LatestStart = item.Children.Min(x => x.LatestStart);
                        item.LatestFinish = item.Children.Max(x => x.LatestFinish);

                        item.Duration = item.EarlyFinish - item.EarlyStart;
                    }
                }

                //result.Activities.Where(x => x.IsSummary == false).CriticalPathMethod();
            }

            result.ProjectLength = activities.CriticalPathLength();

            if (startdate != null && startdate.HasValue)
            {
                result.AvailableDates = startdate.Value.GenerateAvailableDates(result.ProjectLength, workweek, exceptions).ToList();
                result.Activities.CalculateDate(result.AvailableDates, dailyworkhours);
                result.ProjectStartDate = startdate;
                result.ProjectEndDate = result.Activities.CriticalPathEnd();
                result.isDeadlined = result.Activities.isDeadlined();
                result.DeadlinedActivities = result.Activities.DeadlinedActivities().ToList();
            }

            result.Critical = activities.CriticalPath().ToList();
            result.Activities = result.Activities.Where(x => x.Id > 0).ToList();

            foreach (var item in result.Activities)
            {
                //todo check if it's correct

                item.Successors = item.Successors.Where(x => x.isVirtual == false).ToList();

            }

            //todo remove virtual links and activities

            return result;
        }


        public IList<dtoCPMactivity> PreprocessSummaryMilestones(IList<dtoCPMactivity> activities)
        {
            //Start Preparation

            var summaries = activities.Where(x => x.IsSummary).OrderByDescending(x=>x.Id).ToList();
            //var summaries = activities.Where(x => x.IsSummary).ToList();


            foreach (var summary in summaries)
            {


                dtoCPMactivity startMilestone = new dtoCPMactivity() { 
                    Name = String.Format("{0}-{1}", summary.Id, "start"), 
                    Duration = 0, 
                    IsVirtual = true,
                    Id = activities.Max(x => x.Id) + 1,
                    DisplayOrder = activities.Max(x => x.Id) + 1 
                };
                
                dtoCPMactivity endMilestone = new dtoCPMactivity() { 
                    Name = String.Format("{0}-{1}", summary.Id, "end"), 
                    Duration = 0, 
                    IsVirtual = true, 
                    Id = activities.Max(x => x.Id) + 2,
                    DisplayOrder = activities.Max(x => x.Id) + 2
                };

                startMilestone.Parent = summary;
                startMilestone.IdParent = summary.Id;
                endMilestone.Parent = summary;
                endMilestone.IdParent = summary.Id;

                summary.Children.Add(startMilestone);
                summary.Children.Add(endMilestone);

                activities.Add(startMilestone);
                activities.Add(endMilestone);

                if (summary.Parent != null)
                {

                    dtoCPMlink parentlink = new dtoCPMlink()
                    {
                        IdSource = startMilestone.Id,
                        Source = startMilestone,
                        Target = summary.Parent,
                        IdTarget = summary.IdParent,
                        isVirtual = true
                    };

                    startMilestone.Predecessors.Add(parentlink);

                    summary.Parent.Successors.Add(parentlink);
                }

                foreach (var children in summary.Children.Where(x => x.IsVirtual == false && x.IsSummary==false))
                {
                    dtoCPMlink startlink = new dtoCPMlink()
                    {
                        IdSource = children.Id,
                        Source = children,
                        Target = startMilestone,
                        IdTarget = startMilestone.Id,
                        isVirtual = true
                    };

                    startMilestone.Successors.Add(startlink);
                    children.Predecessors.Add(startlink);

                    dtoCPMlink endlink = new dtoCPMlink()
                    {
                        IdSource = endMilestone.Id,
                        Source = endMilestone,
                        Target = children,
                        IdTarget = children.Id,
                        isVirtual = true
                    };

                    children.Successors.Add(endlink);
                    endMilestone.Predecessors.Add(endlink);

                }

                //foreach (var children in summary.Children.Where(x => x.IsVirtual == false))
                //{
                    


                //}

                foreach (var predec in summary.Predecessors.ToList())
                {

                    predec.Source = startMilestone;
                    predec.IdSource = startMilestone.Id;

                    
                    

                    //switch (predec.Type)
                    //{
                    //    case PmActivityLinkType.FinishStart:

                            

                    //        //dtoCPMlink prefs = new dtoCPMlink() { IdTarget = vs.Id, Target = vs, Source = predec.Target, IdSource = predec.IdTarget, isVirtual = true };

                    //        //vs.Predecessors.Add(prefs);

                    //        //predec.Target.Successors.Add(prefs);

                    //        break;

                    //    case PmActivityLinkType.StartStart:

                    //        //dtoCPMlink presss = new dtoCPMlink() { IdTarget = vs.Id, Target = vs, Source = predec.Target, IdSource = predec.IdTarget, isVirtual = true, Type = PmActivityLinkType.StartStart };

                    //        //vs.Predecessors.Add(presss);

                    //        //predec.Target.Successors.Add(presss);

                    //        break;

                    //    default:
                    //        break;
                    //}
                }


                foreach (var succ in summary.Successors.ToList())
                {

                    succ.Target = endMilestone;
                    succ.IdTarget = endMilestone.Id;

                    //succ.Source = ve;
                    //succ.IdSource = ve.Id;

                    //switch (succ.Type)
                    //{
                    //    case PmActivityLinkType.FinishStart:

                    //        //dtoCPMlink prefs = new dtoCPMlink() { IdTarget = ve.Id, Target = ve, Source = succ.Source, IdSource = succ.IdSource, isVirtual = true };

                    //        //ve.Predecessors.Add(prefs);

                    //        //succ.Target.Successors.Add(prefs);

                    //        break;

                    //    case PmActivityLinkType.StartStart:

                    //        //dtoCPMlink presss = new dtoCPMlink() { IdTarget = ve.Id, Target = ve, Source = succ.Source, IdSource = succ.IdSource, isVirtual = true, Type = PmActivityLinkType.StartStart };

                    //        //ve.Predecessors.Add(presss);

                    //        //succ.Target.Successors.Add(presss);

                    //        break;

                    //    default:
                    //        break;
                    //}
                }

                //summary.Successors.Clear();
                //summary.Predecessors.Clear();
            }

            //End Preparation

            return activities;
        }

        public Boolean IsDurationValid(String duration)
        {
            Regex r = new Regex(@"^(\d+)(\?)?$");

            return r.IsMatch(duration);
        }

        public Boolean IsDoubleDurationValid(String duration)
        {
            Regex r = new Regex(@"^[0-9]*\.?[0-9]{0,1}(\?)?$");

            return r.IsMatch(duration);
        }

        public Boolean IsLinkValid(String link)
        {
            Regex r = new Regex(@"^(\d+)(FF|FS|SS|SF)?([+-]\d+)?(;\s*(\d+)(FF|FS|SS|SF)?([+-]\d+)?)*$");

            return r.IsMatch(link);
        }


        //public CPMresult CPMWithSummaryEnabledLinks(IEnumerable<litePmActivity> activities, IEnumerable<litePmActivityLink> links = null, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null, Int32 dailyworkhours = 8)
        //{
        //    CPMresult result = new CPMresult();

        //    Dictionary<Int64, litePmActivity> dict = new Dictionary<long, litePmActivity>();

        //    foreach (var act in activities)
        //    {
        //        dict.Add(act.Id, act);
        //    }

        //    result.Activities = activities.ToList();

        //    if (!result.Activities.Where(x => x.Id == -1).Any())
        //    {
        //        litePmActivity vActivity = new litePmActivity(-1, "end") { Duration = 0, IsVirtual = true };
        //        result.Activities.Add(vActivity);
        //        dict.Add(-1, vActivity);

        //        CPMExtensions.ParseActivityLinks(-1, "*", dict);
        //    }

        //    result.Activities.Where(x => x.IsSummary == false).CriticalPathMethod();

        //    if (result.Activities.Where(x => x.IsSummary).Any())
        //    {
        //        var list = result.Activities.Reverse();
        //        foreach (var item in list)
        //        {
        //            if (item.IsSummary)
        //            {
        //                item.EarlyStart = item.Children.Min(x => x.EarlyStart);
        //                item.EarlyFinish = item.Children.Max(x => x.EarlyFinish);

        //                item.LatestStart = item.Children.Min(x => x.LatestStart);
        //                item.LatestFinish = item.Children.Max(x => x.LatestFinish);

        //                item.Duration = item.EarlyFinish - item.EarlyStart;
        //            }
        //        }

        //        result.Activities.CriticalPathMethod();
        //    }

        //    result.ProjectLenght = activities.CriticalPathLength();

        //    if (startdate != null && startdate.HasValue)
        //    {
        //        result.AvailableDates = startdate.Value.GenerateAvailableDates(result.ProjectLenght, workweek, exceptions).ToList();
        //        result.Activities.CalculateDate(result.AvailableDates, dailyworkhours);
        //        result.ProjectStartDate = startdate;
        //        result.ProjectEndDate = result.Activities.CriticalPathEnd();
        //        result.isDeadlined = result.Activities.isDeadlined();
        //        result.DeadlinedActivities = result.Activities.DeadlinedActivities().ToList();
        //    }

        //    result.Critical = activities.CriticalPath().ToList();
        //    result.Activities = result.Activities.Where(x => x.Id > 0).ToList();

        //    foreach (var item in result.Activities)
        //    {
        //        //todo check if it's correct

        //        item.SuccessorLinks = item.SuccessorLinks.Where(x => x.isVirtual == false).ToList();

        //    }

        //    //todo remove virtual links and activities

        //    return result;
        //}
        //public CPMresult CPM(IEnumerable<litePmActivity> activities, IEnumerable<litePmActivityLink> links = null, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null)
        //{
          
        //}
        //public CPMresult CPM(IEnumerable<PmActivity> activities, IEnumerable<PmActivityLink> links = null, DateTime? startdate = null, FlagDayOfWeek workweek = FlagDayOfWeek.WorkWeek, IEnumerable<DateTime> exceptions = null) { 
        
        //}
    }
}