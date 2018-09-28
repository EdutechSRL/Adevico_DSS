using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol;
using System.Text.RegularExpressions;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Business
{
    public static class CPMExtensions
    {        

        public static Boolean IsInteger(this double d)
        {
            double epsilon = 0.001;

            return (d % 1) <= epsilon;
        }

        //public static Boolean IsInteger(this double d)
        //{
        //    double epsilon = 0.001;

        //    return (d % 1) <= epsilon;
        //}

        //public static double IsInteger(this double d, double workhours=8)
        //{
        //    var x = (d % 1);

        //}


        #region "Full"
            public static void ParseActivityLinks(Int64 id, string link, Dictionary<Int64, dtoCPMactivity> dict)
            {
                if (link != "*")
                {
                    String[] links = link.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in links)
                    {
                        dtoCPMlink x = new dtoCPMlink(id, item.ParseActivityLink(), dict);
                        x.Source.Predecessors.Add(x);
                        x.Target.Successors.Add(x);
                    }
                }
                else
                {
                    foreach (var item in dict)
                    {
                        if (item.Key != id && item.Value.IsSummary==false)
                        {
                            dtoCPMlink x = new dtoCPMlink() { isVirtual = true, Source = dict[id], Target = item.Value };
                            x.Source.Predecessors.Add(x);
                            x.Target.Successors.Add(x);
                        }
                    }
                }

            }
            //public static dtoCPMactivityLink CreateLink(dtoCPMactivity source, dtoCPMactivity target, Int32 leadlag = 0, PmActivityLinkType type = PmActivityLinkType.FinishStart)
            //{
            //    dtoCPMactivityLink link = new dtoCPMactivityLink() { Source = source, Target = target, LeadLag = leadlag, Type = type };

            //    return link;
            //}
            public static IEnumerable<dtoCPMactivity> Predecessors(this dtoCPMactivity activity)
            {
                return (from item in activity.Predecessors select item.Target).ToList();
            }
            public static IEnumerable<dtoCPMactivity> Shuffle(this IEnumerable<dtoCPMactivity> e)
            {
                var r = new Random();
                return e.OrderBy(x => r.Next());
            }
            public static Boolean HasValidDependencies(Project project) {
                return HasValidDependencies(project.Activities.Where(a => a.Deleted == Core.DomainModel.BaseStatusDeleted.None).AsEnumerable());
            }
            public static Boolean HasValidDependencies(this IEnumerable<PmActivity> list)
            {
                var processedPairs = new HashSet<PmActivity>();
                var totalCount = list.Count();
                var rc = new List<PmActivity>(totalCount);
                while (rc.Count < totalCount)
                {
                    bool foundSomethingToProcess = false;
                    foreach (var kvp in list)
                    {
                        var pred = kvp.Predecessors.Select(p=> p.Target).ToList();

                        if (!processedPairs.Contains(kvp)
                            && pred.All(processedPairs.Contains))
                        {
                            rc.Add(kvp);
                            processedPairs.Add(kvp);
                            return true;
                        }
                    }
                    if (!foundSomethingToProcess)
                        return false;
                }
                return true;
            }
            public static Boolean HasValidDependencies(this IEnumerable<dtoGraphActivity> list)
            {
                var processedPairs = new HashSet<dtoGraphActivity>();
                var totalCount = list.Count();
                var rc = new List<dtoGraphActivity>(totalCount);
                while (rc.Count < totalCount)
                {
                    bool foundSomethingToProcess = false;
                    foreach (var kvp in list)
                    {
                        var pred = list.Where(a=> kvp.Links.Select(l => l.IdPredecessor).ToList().Contains(a.IdActivity)).ToList();

                        if (!processedPairs.Contains(kvp)
                            && pred.All(processedPairs.Contains))
                        {
                            rc.Add(kvp);
                            processedPairs.Add(kvp);
                            return true;
                        }
                    }
                    if (!foundSomethingToProcess)
                        return false;
                }
                return true;
            }


            public static IEnumerable<dtoCPMactivity> OrderByDependencies(this IEnumerable<dtoCPMactivity> list)
            {
                var processedPairs = new HashSet<dtoCPMactivity>();
                var totalCount = list.Count();
                var rc = new List<dtoCPMactivity>(totalCount);
                while (rc.Count < totalCount)
                {
                    bool foundSomethingToProcess = false;
                    foreach (var kvp in list)
                    {
                        var pred = kvp.Predecessors();

                        if (!processedPairs.Contains(kvp)
                            && pred.All(processedPairs.Contains))
                        {
                            rc.Add(kvp);
                            processedPairs.Add(kvp);
                            foundSomethingToProcess = true;
                            yield return kvp;
                        }
                    }
                    if (!foundSomethingToProcess)
                        throw new InvalidOperationException("Loop detected inside path");
                }
            }
            public static void FillEarliestValues(this IEnumerable<dtoCPMactivity> list)
            {
                if (!list.Any()) return;

                foreach (var item in list)
                {


                    foreach (var predecessor in item.Predecessors)
                    {
                        switch (predecessor.Type )
                        {
                            case PmActivityLinkType.FinishStart:
                                if (item.EarlyStart < predecessor.Target.EarlyFinish + predecessor.LeadLag)
                                    item.EarlyStart = predecessor.Target.EarlyFinish + predecessor.LeadLag;
                                break;
                            case PmActivityLinkType.FinishFinish:
                                if (item.EarlyStart < predecessor.Target.EarlyFinish + predecessor.LeadLag - item.Duration)
                                    item.EarlyStart = predecessor.Target.EarlyFinish + predecessor.LeadLag - item.Duration;
                                break;
                            case PmActivityLinkType.StartStart:
                                if (item.EarlyStart < predecessor.Target.EarlyStart + predecessor.LeadLag)
                                    item.EarlyStart = predecessor.Target.EarlyStart + predecessor.LeadLag;
                                break;
                            case PmActivityLinkType.StartFinish:
                                if (item.EarlyStart < predecessor.Target.EarlyStart + predecessor.LeadLag - item.Duration)
                                    item.EarlyStart = predecessor.Target.EarlyStart + predecessor.LeadLag - item.Duration;
                                break;
                            default:
                                break;
                        }
                    }

                    //if (item.Father != null)
                    //{
                    //    item.EarlyStart = Math.Max(item.Father.EarlyStart, item.EarlyStart);
                    //}

                    item.EarlyFinish = item.EarlyStart + item.Duration;
                }
            }
            public static void FillLatestValues(this IEnumerable<dtoCPMactivity> list)
            {
                var reversedList = list.Reverse();
                var isFirst = true;

                foreach (var node in reversedList)
                {
                    if (isFirst)
                    {
                        node.LatestFinish = node.EarlyFinish;
                        isFirst = false;
                    }

                    foreach (var successor in node.Successors)
                    {

                        switch (successor.Type)
                        {
                            case PmActivityLinkType.FinishStart:
                                if (node.LatestFinish == 0)
                                    node.LatestFinish = successor.Source.LatestStart + successor.LeadLag;
                                else
                                    if (node.LatestFinish > successor.Source.LatestStart + successor.LeadLag)
                                        node.LatestFinish = successor.Source.LatestStart + successor.LeadLag;
                                break;
                            case PmActivityLinkType.FinishFinish:
                                if (node.LatestFinish == 0)
                                    node.LatestFinish = successor.Source.LatestFinish + successor.LeadLag;
                                else
                                    if (node.LatestFinish > successor.Source.LatestFinish + successor.LeadLag)
                                        node.LatestFinish = successor.Source.LatestFinish + successor.LeadLag;
                                break;
                            case PmActivityLinkType.StartStart:
                                if (node.LatestFinish == 0)
                                    node.LatestFinish = successor.Source.LatestStart + successor.LeadLag + node.Duration;
                                else
                                    if (node.LatestFinish > successor.Source.LatestStart + successor.LeadLag + node.Duration)
                                        node.LatestFinish = successor.Source.LatestStart + successor.LeadLag + node.Duration;
                                break;
                            case PmActivityLinkType.StartFinish:
                                //if (node.LatestFinish == 0)
                                //    node.LatestFinish = successor.Source.LatestFinish + successor.LeadLag;
                                //else
                                //    if (node.LatestFinish > successor.Source.LatestFinish + successor.LeadLag)
                                //        node.LatestFinish = successor.Source.LatestFinish + successor.LeadLag;
                                break;
                            default:
                                break;
                        }
                    }

                    //if (node.Children != null && node.Children.Count > 0)
                    //{
                    //    node.LatestFinish = Math.Max(node.LatestFinish, node.Children.Max(x => x.LatestFinish));
                    //}

                    node.LatestStart = node.LatestFinish - node.Duration;

                }
            }
            public static IEnumerable<dtoCPMactivity> CriticalPath(this IEnumerable<dtoCPMactivity> activities)
            {
                return activities.Where(x => x.isCritical).ToList();
            }
            public static double CriticalPathLength(this IEnumerable<dtoCPMactivity> activities)
            {
                if (activities.Count() > 0 && activities.CriticalPath().Count() > 0)
                {
                    return activities.CriticalPath().Max(x => x.LatestFinish);
                }
                else
                {
                    return 0;
                }
            }
            public static DateTime CriticalPathEnd(this IEnumerable<dtoCPMactivity> activities)
            {
                if (activities.Count() > 0 && activities.CriticalPath().Count() > 0)
                {
                    return activities.CriticalPath().Max(x => x.LatestFinishDate.Value);
                }
                else
                {
                    return activities.Max(x => x.LatestFinishDate.Value);
                }
            }
            public static void CalculateDate(this IEnumerable<dtoCPMactivity> activities, IEnumerable<DateTime> availabledates, Int32 dailyworkhours = 8)
            {

                foreach (var activity in activities)
                {



                    activity.EarlyStartDate = availabledates.ByIdx(activity.EarlyStart);

                    if (activity.isMilestone)
                    {
                        activity.EarlyFinishDate = availabledates.ByIdx(activity.EarlyFinish);
                    }
                    else
                    {

                        activity.EarlyFinishDate = availabledates.ByIdx(activity.EarlyFinish - 1).AddHours(dailyworkhours);
                    }

                    activity.LatestStartDate = availabledates.ByIdx(activity.LatestStart);

                    if (activity.VirtualFinish)
                    {
                        activity.EarlyFinishDate = availabledates.ByIdx(activity.EarlyFinish);
                    }
                    else
                    {

                        //activity.EarlyFinishDate = availabledates.ByIdx(activity.EarlyFinish - 1).AddHours(dailyworkhours);
                    }
                    if(activity.isMilestone)
                    {
                        activity.LatestFinishDate = availabledates.ByIdx(activity.LatestFinish);
                    }else{
                        activity.LatestFinishDate = availabledates.ByIdx(activity.LatestFinish - 1).AddHours(dailyworkhours);
                    }

                    if (!activity.EarlyStart.IsInteger())
                    {

                    }

                    if (!activity.EarlyFinish.IsInteger())
                    {

                    }

                    if (!activity.LatestStart.IsInteger())
                    {

                    }

                    if (!activity.LatestFinish.IsInteger())
                    {

                    }

                }

                // return activities;
            }
            public static Boolean isDeadlined(this IEnumerable<dtoCPMactivity> activities)
            {
                return activities.Where(x => x.isAfterDeadline).Any();
            }
            public static IEnumerable<dtoCPMactivity> DeadlinedActivities(this IEnumerable<dtoCPMactivity> activities)
            {
                return activities.Where(x => x.isAfterDeadline).ToList();
            }
            public static void CriticalPathMethod(this IEnumerable<dtoCPMactivity> activities)
            {
                IEnumerable<dtoCPMactivity> ordered = activities.OrderByDependencies().ToList();

                ordered.FillEarliestValues();
                ordered.FillLatestValues();

                //return activities;
            }
        #endregion

        #region "Common"
            public static IEnumerable<ParsedActivityLink> ParseActivityLinks(this string link)
            {
                if (!String.IsNullOrEmpty(link))
                    return (from item in link.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries) select item.ParseActivityLink()).ToList();
                else
                    return new List<ParsedActivityLink>();
            }
            public static ParsedActivityLink ParseActivityLink(this string link)
            {
                ParsedActivityLink pl = new ParsedActivityLink();

                Regex rex = new Regex(@"^(?<id>\d+)(?<type>FF|FS|SS|SF|SY)?(?<lead>[+-]\d+)?", RegexOptions.None);

                Match m = rex.Match(link.Trim().ToUpper());

                switch (m.Groups["type"].Value)
                {
                    case "FF":
                        pl.LinkType = PmActivityLinkType.FinishFinish;
                        break;
                    case "FS":
                        pl.LinkType = PmActivityLinkType.FinishStart;
                        break;
                    case "SS":
                        pl.LinkType = PmActivityLinkType.StartStart;
                        break;
                    case "SF":
                        pl.LinkType = PmActivityLinkType.StartFinish;
                        break;
                    default:
                        break;
                }

                Double leadlag = 0;
                if (Double.TryParse(m.Groups["lead"].Value, out leadlag))
                {
                    pl.LeadLag = leadlag;
                }

                Int64 id = 0;
                if (Int64.TryParse(m.Groups["id"].Value, out id))
                {
                    pl.Id = id;
                }

                return pl;
            }
            public static FlagDayOfWeek ToFlagDayOfWeek(this DayOfWeek dow)
            {
                switch (dow)
                {
                    case DayOfWeek.Friday:
                        return FlagDayOfWeek.Friday;

                    case DayOfWeek.Monday:
                        return FlagDayOfWeek.Monday;
                    case DayOfWeek.Saturday:
                        return FlagDayOfWeek.Saturday;
                    case DayOfWeek.Sunday:
                        return FlagDayOfWeek.Sunday;
                    case DayOfWeek.Thursday:
                        return FlagDayOfWeek.Thursday;
                    case DayOfWeek.Tuesday:
                        return FlagDayOfWeek.Tuesday;
                    case DayOfWeek.Wednesday:
                        return FlagDayOfWeek.Wednesday;
                    default:
                        return FlagDayOfWeek.None;
                }
            }
            public static Boolean IsInFlagDayOfWeek(this DayOfWeek dow, FlagDayOfWeek week)
            {
                return ((dow.ToFlagDayOfWeek() & week) != FlagDayOfWeek.None);
            }
            //public static DayOfWeek ToWeekOfDay(this FlagDayOfWeek fdow)
            //{
            //    return DayOfWeek.Sunday;
            //}
            public static Boolean isExceptionDate(this DateTime date, FlagDayOfWeek week = FlagDayOfWeek.AllWeek, IEnumerable<DateTime> exceptions = null)
            {
                Boolean isException = false;

                isException = (week == FlagDayOfWeek.AllWeek) ? false : !date.DayOfWeek.IsInFlagDayOfWeek(week);

                //var test = date.DayOfWeek.IsInFlagDayOfWeek(week);

                if (exceptions != null && exceptions.Count() != 0)
                {
                    isException |= exceptions.Where(x => x.Date == date.Date).Any();

                }

                return isException;

            }
            public static IEnumerable<DateTime> GenerateAvailableDates(this DateTime startdate, double projectlenght, FlagDayOfWeek week = FlagDayOfWeek.AllWeek, IEnumerable<DateTime> exceptions = null)
            {
                List<DateTime> available = new List<DateTime>();

                DateTime firstdate = startdate;
                while (firstdate.isExceptionDate(week, exceptions))
                {
                    firstdate = firstdate.AddDays(1);
                }
                available.Add(firstdate);

                while (available.Count() <= projectlenght)
                {
                    DateTime next = available.Last().AddDays(1);
                    while (next.isExceptionDate(week, exceptions))
                    {
                        next = next.AddDays(1);
                    }
                    available.Add(next);
                }

                return available;
            }
            public static T ByIdx<T>(this IEnumerable<T> list, Int32 idx)
            {
                return list.Skip(idx).Take(1).FirstOrDefault();
            }
            public static T ByIdx<T>(this IEnumerable<T> list, double idx)
            {
                Int32 index = (Int32)Math.Ceiling(idx);
                return list.Skip(index).Take(1).FirstOrDefault();
            }
            public static String ActivityLinksToString(List<ParsedActivityLink> links, List<dtoMapActivity> activities)
            {
                String value = "";
                if (links.Where(l => l.LinkType != PmActivityLinkType.FinishStart).Any())
                {
                    foreach (ParsedActivityLink link in (from l in links join a in activities on l.Id equals a.IdActivity orderby a.RowNumber select l))
                    {
                        value += (String.IsNullOrEmpty(value) ? "" :  ";")+activities.Where(a => a.IdActivity == link.Id).FirstOrDefault().RowNumber;
                        switch (link.LinkType)
                        {
                            case PmActivityLinkType.FinishFinish:
                                value += "FF";
                                break;
                            case PmActivityLinkType.FinishStart:
                                value += "FS";
                                break;
                            case PmActivityLinkType.StartFinish:
                                value += "SF";
                                break;
                            case PmActivityLinkType.StartStart:
                                value += "SS";
                                break;
                        }
                        value += (link.LeadLag == 0) ? "" : (link.LeadLag > 0 ? "+" + link.LeadLag.ToString() : link.LeadLag.ToString());
                    }
                }
                else{
                    value = String.Join(";", (from l in links join a in activities on l.Id equals a.IdActivity orderby a.RowNumber select a.RowNumber.ToString() + ((l.LeadLag == 0) ? "" : (l.LeadLag > 0 ? "+" + l.LeadLag.ToString() : l.LeadLag.ToString()))).ToList());
                }
                return value;
            }
            /// <summary>
            /// Gives string rapresentation of task predecessors
            /// </summary>
            /// <param name="links">predecessors links</param>
            /// <param name="tasks">all project tasks</param>
            /// <returns></returns>
            public static String TreeItemLinksToString(List<ParsedActivityLink> links, List<dtoActivityTreeItem> tasks)
            {
                String value = "";
                if (links.Where(l => l.LinkType != PmActivityLinkType.FinishStart).Any())
                {
                    foreach (ParsedActivityLink link in (from l in links join a in tasks on l.Id equals a.Id orderby a.RowNumber select l))
                    {
                        value += (String.IsNullOrEmpty(value) ? "" : ";") + tasks.Where(a => a.Id == link.Id).FirstOrDefault().RowNumber;
                        switch (link.LinkType)
                        {
                            case PmActivityLinkType.FinishFinish:
                                value += "FF";
                                break;
                            case PmActivityLinkType.FinishStart:
                                value += "FS";
                                break;
                            case PmActivityLinkType.StartFinish:
                                value += "SF";
                                break;
                            case PmActivityLinkType.StartStart:
                                value += "SS";
                                break;
                        }
                        value += (link.LeadLag == 0) ? "" : (link.LeadLag > 0 ? "+" + link.LeadLag.ToString() : link.LeadLag.ToString());
                    }
                }
                else{
                    value = String.Join(";", (from l in links join a in tasks on l.Id equals a.Id orderby a.RowNumber select a.RowNumber.ToString() + ((l.LeadLag == 0) ? "" : (l.LeadLag > 0 ? "+" + l.LeadLag.ToString() : l.LeadLag.ToString()))).ToList());
                }
                return value;
            }
       
            //public static String ActivityLinksToIdString(string links, List<dtoLiteMapActivity> activities)
            //{
            //    List<ParsedActivityLink> pLinks = ParseActivityLinks(links).ToList();
            //    foreach (ParsedActivityLink l in pLinks)
            //    {
            //        l.Id = activities.Where(a => a.RowNumber == l.Id).FirstOrDefault().IdActivity;
            //    }
            //    return ActivityLinksToIdString(pLinks);
            //}
            //public static String ActivityLinksToIdString(List<PmActivityLink> links)
            //{
            //    String value = "";
            //    if (links.Where(l => l.Type != PmActivityLinkType.FinishStart).Any())
            //    {
            //        foreach (PmActivityLink link in links.Where(l => l.Source != null).OrderBy(l => l.Source.Id))
            //        {
            //            value += link.Source.Id;
            //            switch (link.Type)
            //            {
            //                case PmActivityLinkType.FinishFinish:
            //                    value += "FF";
            //                    break;
            //                case PmActivityLinkType.FinishStart:
            //                    value += "FS";
            //                    break;
            //                case PmActivityLinkType.StartFinish:
            //                    value += "SF";
            //                    break;
            //                case PmActivityLinkType.StartStart:
            //                    value += "SS";
            //                    break;
            //            }
            //            value += (link.LeadLag == 0) ? "" : (link.LeadLag > 0 ? "+" + link.LeadLag.ToString() : link.LeadLag.ToString());
            //            value += ";";
            //        }
            //    }
            //    else
            //    {
            //        value = String.Join(";", (from l in links.Where(l => l.Source != null).OrderBy(l => l.Source.Id) select l.Source.Id.ToString() + ((l.LeadLag == 0) ? "" : (l.LeadLag > 0 ? "+" + l.LeadLag.ToString() : l.LeadLag.ToString()))).ToList());
            //    }
            //    return value;
            //}

            //public static String ActivityLinksToIdString(List<ParsedActivityLink> links)
            //{
            //    String value = "";
            //    if (links.Where(l => l.LinkType != PmActivityLinkType.FinishStart).Any())
            //    {
            //        foreach (ParsedActivityLink link in links.OrderBy(l => l.Id))
            //        {
            //            value += link.Id;
            //            switch (link.LinkType)
            //            {
            //                case PmActivityLinkType.FinishFinish:
            //                    value += "FF";
            //                    break;
            //                case PmActivityLinkType.FinishStart:
            //                    value += "FS";
            //                    break;
            //                case PmActivityLinkType.StartFinish:
            //                    value += "SF";
            //                    break;
            //                case PmActivityLinkType.StartStart:
            //                    value += "SS";
            //                    break;
            //            }
            //            value += (link.LeadLag == 0) ? "" : (link.LeadLag > 0 ? "+" + link.LeadLag.ToString() : link.LeadLag.ToString());
            //            value += ";";
            //        }
            //    }
            //    else
            //    {
            //        value = String.Join(";", (from l in links.OrderBy(l => l.Id) select l.Id.ToString() + ((l.LeadLag == 0) ? "" : (l.LeadLag > 0 ? "+" + l.LeadLag.ToString() : l.LeadLag.ToString()))).ToList());
            //    }
            //    return value;
            //}
        #endregion
    }
}