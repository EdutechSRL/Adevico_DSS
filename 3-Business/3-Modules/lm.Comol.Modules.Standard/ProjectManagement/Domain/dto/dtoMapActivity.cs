using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.GraphTheory;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoMapActivity
    {
        public virtual long IdActivity { get; set; }
        public virtual long IdParent { get; set; }
        public virtual long RowNumber { get; set; }
        public virtual dtoField<String> Name { get; set; }
        public virtual dtoField<long> DisplayOrder { get; set; }
        public virtual long WBSindex { get; set; }
        public virtual String WBSstring { get; set; }
        public virtual long Depth { get; set; }

        public virtual dtoField<DateTime?> Deadline { get; set; }
        public virtual Int32 Completeness { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual Boolean IsSummary { get; set; }
        public virtual Boolean IsNew { get; set; }

        public virtual dtoField<DateTime?> EarlyStartDate { get; set; }
        public virtual dtoField<DateTime?> EarlyFinishDate { get; set; }
        public virtual DateTime? LatestStartDate { get; set; }
        public virtual DateTime? LatestFinishDate { get; set; }
        public virtual Boolean isAfterDeadline { get; set; }
        public virtual Boolean isCritical { get; set; }
        public virtual dtoField<Boolean> isMilestone { get {
            Boolean init = (Duration.Init == null) ? false : (Duration.Init.Value == 0);
            Boolean current = (Duration.Current == null) ? false : (Duration.Current.Value == 0);
            Boolean edit = (Duration.Edit == null) ? false : (Duration.Edit.Value == 0);

            return new dtoField<Boolean>(init, current, edit, Duration.InEditMode, Duration.IsUpdated, Duration.WillBeUpdated);
        }
        }
        public virtual dtoField<dtoDuration> Duration { get; set; }
        public virtual dtoField<String> Predecessors { get; set; }
        internal virtual dtoField<List<ParsedActivityLink>> Links { get; set; }
        internal virtual List<long> IdLinkedActivities { get; set; }
        public virtual dtoField<List<dtoResource>> Resources { get; set; }
        internal virtual dtoField<List<long>> IdResources { get; set; }
        public virtual FieldStatus Status  { get; set; }
        public virtual dtoActivityPermission Permission { get; set; }
        public virtual Boolean InEditMode { get; set; }
        public virtual Boolean InEditPredecessorsMode { get; set; }
        public virtual dtoLiteValue Previous { get; set; }
        public dtoMapActivity(){
            Permission = new dtoActivityPermission();
            Duration = new dtoField<dtoDuration>();
        }

        
        public dtoMapActivity(litePmActivity activity, long rowNumber, Boolean onlyview = false ){
            IdActivity = activity.Id;
            IdParent = (activity.Parent!=null) ? activity.Parent.Id : 0;
            RowNumber = rowNumber;
            Name =  new dtoField<string>(activity.Name);
            DisplayOrder =  new dtoField<long>(activity.DisplayOrder);
            WBSindex = activity.WBSindex;
            WBSstring = activity.WBSstring;
            Depth = activity.Depth;
            Deadline =  new dtoField<DateTime?>(activity.Deadline);
            IsCompleted = activity.IsCompleted;
            IsSummary = activity.IsSummary;
            EarlyStartDate =  new dtoField<DateTime?>(activity.EarlyStartDate);
            EarlyFinishDate =  new dtoField<DateTime?>(activity.EarlyFinishDate);
            LatestStartDate = activity.LatestStartDate;
            LatestFinishDate = activity.LatestFinishDate;
            isAfterDeadline = activity.isAfterDeadline;
            isCritical = activity.isCritical;
            Duration = new dtoField<dtoDuration>(new dtoDuration(activity.Duration, activity.IsDurationEstimated));
            Links = new dtoField<List<ParsedActivityLink>>(activity.Predecessors.Where(p => p.Source != null).Select(p => new ParsedActivityLink() { Id = p.Target.Id, LeadLag = p.LeadLag, LinkType = p.Type }).ToList());
            IdLinkedActivities = activity.Predecessors.Where(p => p.Target != null).Select(p => p.Target.Id).ToList();
            Predecessors = new dtoField<String>();
            IdResources = new dtoField<List<long>>(activity.CurrentAssignments.Select(a => a.Resource.Id).ToList());
            Resources = new dtoField<List<dtoResource>>();
            Status = FieldStatus.none;
            Completeness = activity.Completeness;
            Permission = new dtoActivityPermission();
        }
        public dtoMapActivity(PmActivity activity, long rowNumber, Boolean onlyview = false)
        {
            IdActivity = activity.Id;
            IdParent = (activity.Parent!=null) ? activity.Parent.Id : 0;
            RowNumber = rowNumber;
            Name =  new dtoField<string>(activity.Name);
            DisplayOrder =  new dtoField<long>(activity.DisplayOrder);
            WBSindex = activity.WBSindex;
            WBSstring = activity.WBSstring;
            Depth = activity.Depth;
            Deadline =  new dtoField<DateTime?>(activity.Deadline);
            IsCompleted = activity.IsCompleted;
            IsSummary = activity.IsSummary;
            EarlyStartDate =  new dtoField<DateTime?>(activity.EarlyStartDate);
            EarlyFinishDate =  new dtoField<DateTime?>(activity.EarlyFinishDate);
            LatestStartDate = activity.LatestStartDate;
            LatestFinishDate = activity.LatestFinishDate;
            isAfterDeadline = activity.isAfterDeadline;
            isCritical = activity.isCritical;
            Duration = new dtoField<dtoDuration>(new dtoDuration(activity.Duration, activity.IsDurationEstimated));
            Links = new dtoField<List<ParsedActivityLink>>(activity.Predecessors.Where(p => p.Target != null).Select(p => new ParsedActivityLink() { Id = p.Target.Id, LeadLag = p.LeadLag, LinkType = p.Type }).ToList());
            IdLinkedActivities = activity.Predecessors.Where(p => p.Target != null).Select(p => p.Target.Id).ToList();
            Predecessors = new dtoField<String>();
            IdResources = new dtoField<List<long>>(activity.CurrentAssignments.Select(a => a.Resource.Id).ToList());
            Resources = new dtoField<List<dtoResource>>();
            Status = FieldStatus.none;
            Completeness = activity.Completeness;
            Permission = new dtoActivityPermission();
        }

        //public dtoLiteMapActivity ToLite() {
        //    dtoLiteMapActivity lite = new dtoLiteMapActivity();
        //    lite.IdActivity = IdActivity;
        //    lite.IdParent = IdParent;
        //    lite.RowNumber = RowNumber;
        //    lite.IsSummary = IsSummary;
        //    lite.Current.Name = Name.;
        //    lite.Current.EarlyStartDate = EarlyStartDate.Value;
        //    lite.Current.EarlyFinishDate = EarlyFinishDate.Value;
        //    lite.Current.Deadline = Deadline.Value;
        //    lite.Current.Duration = Duration.Value;
        //    lite.Current.Predecessors = Predecessors.Value;
        //    return lite;
        //}

        internal String PredecessorsToIdString()
        {
            return String.Join(";", Links.Init.OrderBy(p => p.Id).Select(p => p.Id.ToString() + p.LinkType.ToString() + p.LeadLag.ToString()).ToList());
        }

        public override string ToString()
        {
            return String.Format("[{0}]  edit:{5} idParent:{1} d{2} r{3} d.{4} [{6}-{7}",
             IdActivity.ToString().PadLeft(3, ' '), IdParent.ToString().PadLeft(3, ' '),
             RowNumber.ToString().PadLeft(3, ' '), DisplayOrder.ToString().PadLeft(4, ' '), InEditMode.ToString().PadLeft(4, ' '),
             (EarlyStartDate.GetValue().HasValue ? EarlyStartDate.GetValue().Value.ToShortDateString() : ""),(EarlyFinishDate.GetValue().HasValue ? EarlyFinishDate.GetValue().Value.ToShortDateString() : "")
             );
        }
    }

    [Serializable]
    public class dtoLiteMapActivity
    {
        public virtual long IdActivity { get; set; }
        public virtual long IdParent { get; set; }
        public virtual long RowNumber { get; set; }
        public virtual Boolean IsSummary { get; set; }
        public virtual Boolean isMilestone { get { return Current.Duration.Value == 0; } }
        public virtual Boolean InEditMode { get { return !Current.Equals(Previous); } }
        public virtual Boolean InEditPredecessorsMode { get { return Current.PredecessorsIdString != Previous.PredecessorsIdString; } }
        public virtual dtoLiteValue Current { get; set; }
        public virtual dtoLiteValue Previous { get; set; }
        public virtual Boolean IsDeleted { get; set; }
        public dtoLiteMapActivity()
        {
            Current = new dtoLiteValue();
            Previous = new dtoLiteValue();
            Cycles = new List<DirectedGraphCycle>();
        }
        public virtual void UpdateIdPredecessors(List<dtoLiteMapActivity> activities){
            List<ParsedActivityLink> links = null;
            if (!String.IsNullOrEmpty(Previous.Predecessors))
            {
                links = lm.Comol.Modules.Standard.ProjectManagement.Business.CPMExtensions.ParseActivityLinks(Previous.Predecessors).ToList().Where(l => l.Id > 0).ToList();
                foreach (ParsedActivityLink l in links)
                {
                    dtoLiteMapActivity a = activities.Where(act => act.RowNumber == l.Id && !act.IsDeleted).FirstOrDefault();

                    if (a != null)
                    {
                        l.Id = a.IdActivity;
                        l.IsSummary = a.IsSummary;
                    }
                }
                if (links.Where(l=>!l.IsSummary).Any())
                    Previous.PredecessorsIdString = String.Join(";", links.Where(l => !l.IsSummary).OrderBy(p => p.Id).Select(p => p.Id.ToString() + p.LinkType.ToString() + p.LeadLag.ToString()).ToList());
                else
                    Previous.PredecessorsIdString = "";
                Previous.IdActivityLinks = links.Where(l => !l.IsSummary).Select(l => l.Id).ToList();
                Previous.Links = links.Where(l => !l.IsSummary).ToList();
                Previous.IdSummaryLinks = links.Where(l => l.IsSummary).Select(l => l.Id).ToList();
                Previous.SummaryLinks = links.Where(l => l.IsSummary).ToList();
            }
            else
                Previous.PredecessorsIdString = "";
            if (!String.IsNullOrEmpty(Current.Predecessors))
            {
                links = lm.Comol.Modules.Standard.ProjectManagement.Business.CPMExtensions.ParseActivityLinks(Current.Predecessors).ToList().Where(l => l.Id > 0).ToList();
                foreach (ParsedActivityLink l in links)
                {
                    dtoLiteMapActivity a = activities.Where(act => act.RowNumber == l.Id && !act.IsDeleted).FirstOrDefault();
                    if (a != null)
                    {
                        l.Id = a.IdActivity;
                        l.IsSummary = a.IsSummary;
                    }
                }
                if (links.Where(l => !l.IsSummary).Any())
                    Current.PredecessorsIdString = String.Join(";", links.Where(l => !l.IsSummary).OrderBy(p => p.Id).Select(p => p.Id.ToString() + p.LinkType.ToString() + p.LeadLag.ToString()).ToList());
                else
                    Current.PredecessorsIdString = "";
                Current.IdActivityLinks = links.Where(l => !l.IsSummary).Select(l => l.Id).ToList();
                Current.Links = links.Where(l => !l.IsSummary).ToList();
                Current.IdSummaryLinks = links.Where(l => l.IsSummary).Select(l => l.Id).ToList();
                Current.SummaryLinks = links.Where(l => l.IsSummary).ToList();
            }
            else
                Current.PredecessorsIdString = "";
        }
        public virtual List<DirectedGraphCycle> Cycles { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}]  edit:{4} idParent:{1} d{2} r{3}",
             IdActivity.ToString().PadLeft(3, ' '), IdParent.ToString().PadLeft(3, ' '),
             RowNumber.ToString().PadLeft(3, ' '), InEditMode.ToString().PadLeft(4, ' ')
              
             );
        }
    }

    [Serializable]
    public class dtoLiteValue : IEquatable<dtoLiteValue> {
        public virtual String Name { get; set; }
        public virtual DateTime? EarlyStartDate { get; set; }
        public virtual DateTime? EarlyFinishDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual dtoDuration Duration { get; set; }
        public virtual String Predecessors { get; set; }
        public virtual String PredecessorsIdString { get; set; }
        public virtual List<long> IdActivityLinks { get; set; }
        public virtual List<ParsedActivityLink> Links { get; set; }
        public virtual List<long> IdSummaryLinks { get; set; }
        public virtual List<ParsedActivityLink> SummaryLinks { get; set; }
        public dtoLiteValue() {
            Duration = new dtoDuration();
            IdActivityLinks = new List<long>();
            Links = new List<ParsedActivityLink>();
            IdSummaryLinks = new List<long>();
            SummaryLinks = new List<ParsedActivityLink>();
            PredecessorsIdString = "";
        }
        public bool Equals(dtoLiteValue other)
        {
            return Name == other.Name && EarlyStartDate == other.EarlyStartDate && EarlyFinishDate == other.EarlyFinishDate && Deadline == other.Deadline && Duration.Equals(other.Duration) && Predecessors == other.Predecessors;
        }
    }


    [Serializable]
    public class dtoGraphActivity
    {
        public virtual long IdActivity { get; set; }
        public virtual long IdParent { get; set; }
        public virtual long RowNumber { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual long Depth { get; set; }

        public virtual Boolean IsSummary { get; set; }
        public virtual List<dtoGraphActivityLink> Links { get; set; }
        //public virtual List<long> IdLinkedActivities { get; set; }
        public dtoGraphActivity()
        {
            Links = new List<dtoGraphActivityLink>();
            //IdLinkedActivities = new List<long>();
        }

        public dtoGraphActivity(litePmActivity activity, long rowNumber, Boolean onlyview = false)
        {
            IdActivity = activity.Id;
            IdParent = (activity.Parent!=null) ? activity.Parent.Id : 0;
            RowNumber = rowNumber;
            DisplayOrder = activity.DisplayOrder;
            Depth = activity.Depth;
            IsSummary = activity.IsSummary;
            Links = (activity.Predecessors.Where(p => p.Target != null && !p.Target.IsSummary ).Select(p => new dtoGraphActivityLink() { IdPredecessor = p.Target.Id, LeadLag = p.LeadLag, Type = p.Type }).ToList());
            //IdLinkedActivities = activity.Predecessors.Where(p => p.Target != null).Select(p => p.Target.Id).ToList();

        }
        public dtoGraphActivity(PmActivity activity, long rowNumber, Boolean onlyview = false)
        {
            IdActivity = activity.Id;
            IdParent = (activity.Parent!=null) ? activity.Parent.Id : 0;
            RowNumber = rowNumber;
            DisplayOrder = activity.DisplayOrder;
            Depth = activity.Depth;
            IsSummary = activity.IsSummary;
            Links = (activity.Predecessors.Where(p => p.Target != null && !p.Target.IsSummary ).Select(p => new dtoGraphActivityLink() { IdPredecessor = p.Target.Id, LeadLag = p.LeadLag, Type = p.Type }).ToList());
            //IdLinkedActivities = activity.Predecessors.Where(p => p.Target != null).Select(p => p.Target.Id).ToList();

        }
        
        internal String PredecessorsToIdString()
        {
            return String.Join(";", Links.OrderBy(p => p.IdPredecessor).Select(p => p.IdPredecessor.ToString() + p.Type.ToString() + p.LeadLag.ToString()).ToList());
        }
        public override string ToString()
        {
            return String.Format("[{0}] p:{1} d{2} r{3} d.{4}",
                IdActivity.ToString().PadLeft(3, ' '), IdParent.ToString().PadLeft(3, ' '),
                RowNumber.ToString().PadLeft(3, ' '), DisplayOrder.ToString().PadLeft(4, ' ')

                );
        }
    }

    [Serializable]
    public class dtoGraphActivityLink
    {
        public virtual long IdPredecessor { get; set; }
        public virtual PmActivityLinkType Type { get; set; }
        public virtual Double LeadLag { get; set; }

        public dtoGraphActivityLink()
        {

        }
        public override string ToString()
        {
            return String.Format("[{0}] {1} - [{2}]",
                IdPredecessor.ToString().PadLeft(3, ' '),
                Type.ToString(), LeadLag.ToString().PadLeft(3, ' ')
                );
        }
    }

    [Serializable]
    public class dtoReorderGraphActivity : dtoGraphActivity
    {
        public virtual FieldStatus Status { get; set; }
        public dtoReorderGraphActivity() {
            Status = FieldStatus.none;
        }
    }
}