using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoActivity
    {
        public virtual long Id { get; set; }
        public virtual long IdProject { get; set; }
        public virtual long IdParent { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String Note { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual long WBSindex { get; set; }
        public virtual String WBSstring { get; set; }
        public virtual long Depth { get; set; }

        public virtual DateTime? Deadline { get; set; }
        public virtual Int32 Completeness { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual Boolean IsSummary { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }
        public virtual DateTime? EarlyStartDate { get; set; }
        public virtual DateTime? EarlyFinishDate { get; set; }
        public virtual DateTime? LatestStartDate { get; set; }
        public virtual DateTime? LatestFinishDate { get; set; }
        public virtual Boolean isAfterDeadline { get; set; }
        public virtual Boolean isCritical { get; set; }
        public virtual Boolean isMilestone { get {return Duration.Value == 0;}}
        public virtual dtoDuration Duration { get; set; }
        public virtual List<long> IdResources { get; set; }
        public virtual List<dtoActivityLink> Links { get; set; }
        public virtual List<dtoActivityCompletion> Assignments { get; set; }
        
        public virtual dtoActivityPermission Permission { get; set; }
        public virtual ProjectItemStatus Status { get; set; }

        public dtoActivity(){
            Permission = new dtoActivityPermission();
            IdResources = new List<long>();
            Duration = new dtoDuration();
            Links = new List<dtoActivityLink>();
            Assignments = new List<dtoActivityCompletion>();
        }
        public dtoActivity(litePmActivity activity)
        {
            Id = activity.Id;
            IdProject = activity.IdProject;
            IdParent = (activity.Parent != null) ? activity.Parent.Id : 0;
            Name = activity.Name;
            Description = activity.Description;
            Note = activity.Notes;
            DisplayOrder = activity.DisplayOrder;
            WBSindex = activity.WBSindex;
            WBSstring = activity.WBSstring;
            Depth = activity.Depth;
            Deadline = activity.Deadline;
            IsCompleted = activity.IsCompleted;
            IsSummary = activity.IsSummary;
            EarlyStartDate = activity.EarlyStartDate;
            EarlyFinishDate = activity.EarlyFinishDate;
            LatestStartDate = activity.LatestStartDate;
            LatestFinishDate = activity.LatestFinishDate;
            isAfterDeadline = activity.isAfterDeadline;
            isCritical = activity.isCritical;
            Duration = new dtoDuration(activity.Duration, activity.IsDurationEstimated);
            Links = activity.Predecessors.Where(p => p.Target != null).Select(p => new dtoActivityLink() { Id = p.Id, IdTarget = p.Target.Id, IdSource = Id , LeadLag = p.LeadLag, Type = p.Type }).ToList();
            IdResources = activity.CurrentAssignments.Select(a => a.Resource.Id).ToList();
            Assignments = activity.CurrentAssignments.Select(a => new dtoActivityCompletion() { Id = Id, IdResource = a.Resource.Id, Completeness = a.Completeness, IsApproved = a.IsApproved }).ToList();
            Completeness = activity.Completeness;
            Status = activity.Status;
            Permission = new dtoActivityPermission();
        }
    }

    [Serializable]
    public class dtoActivityLink
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual long RowNumber { get; set; }
        public virtual long IdTarget { get; set; }
        public virtual long IdSource { get; set; }
        public virtual Double LeadLag { get; set; }
        public PmActivityLinkType Type { get; set; }
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean Deleted  { get; set; }
        public virtual Boolean InCycles { get; set; }
        public virtual String UniqueId { get; set; }
        public dtoActivityLink()
        {
            UniqueId = Guid.NewGuid().ToString();
        }
    }
}