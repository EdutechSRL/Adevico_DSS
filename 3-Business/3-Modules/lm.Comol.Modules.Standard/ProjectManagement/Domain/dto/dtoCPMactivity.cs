using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Business;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoCPMactivity
    {
        public virtual long Id { get; set; }
        public virtual long IdParent { get; set; }
        public virtual dtoCPMactivity Parent { get; set; }
        public virtual String Name { get; set; }
        public virtual int Completeness { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual IList<dtoCPMactivity> Children { get; set; }
        #region "For CPM"
            public virtual Double EarlyStart { get; set; }
            public virtual Double EarlyFinish { get; set; }
            public virtual Double LatestStart { get; set; }
            public virtual Double LatestFinish { get; set; }
            public virtual DateTime? EarlyStartDate { get; set; }
            public virtual DateTime? EarlyFinishDate { get; set; }
            public virtual DateTime? LatestStartDate { get; set; }
            public virtual DateTime? LatestFinishDate { get; set; }
            public virtual DateTime? Deadline { get; set; }
            public virtual Boolean IsSummary { get; set; }
            public virtual Boolean IsVirtual { get; set; }
        #region "don't map"
            public virtual Double Slack { get { return LatestFinish - EarlyFinish; } }

            public virtual Boolean isAfterDeadline
            {
                get
                {
                    return (Deadline != null && Deadline.HasValue) ? (this.EarlyFinishDate >= Deadline.Value) : false;
                }
            }

            public virtual Boolean isCritical
            {
                get
                {
                    return ((this.EarlyFinish - this.LatestFinish >= 0) && (this.EarlyStart - this.LatestStart >= 0)) && !IsSummary && !isMilestone;
                }
            }

            public virtual Boolean isMilestone { get { return Duration == 0; } }
            public virtual Boolean VirtualFinish
            {
                get
                {
                    return Predecessors.Where(x => x.Type == PmActivityLinkType.StartFinish).Any();
                }
            }
        #endregion

            public virtual Double Duration { get; set; }
            public virtual IList<dtoCPMlink> Predecessors { get; set; }
            public virtual IList<dtoCPMlink> Successors { get; set; }
            public virtual List<long> IdChildren { get; set; }
        #endregion
        public virtual BaseStatusDeleted Deleted { get; set; }

        public dtoCPMactivity()
        {
            Predecessors = new List<dtoCPMlink>();
            Successors = new List<dtoCPMlink>();
            Children = new List<dtoCPMactivity>();
            IdChildren = new List<long>();
        }
        public dtoCPMactivity(litePmActivity activity)
        {
            Id = activity.Id;
            IdParent = (activity.Parent != null) ? activity.Parent.Id : 0;
            //Parent = activity.Parent;
            Name = activity.Name;
            Completeness = activity.Completeness;
            IsDurationEstimated = activity.IsDurationEstimated;
            DisplayOrder = activity.DisplayOrder;
            EarlyStart = activity.EarlyStart;
            EarlyFinish = activity.EarlyFinish;
            LatestStart = activity.LatestStart;
            LatestFinish = activity.LatestFinish; 
            EarlyStartDate = activity.EarlyStartDate;
            EarlyFinishDate = activity.EarlyFinishDate;
            LatestStartDate = activity.LatestStartDate;
            LatestFinishDate = activity.LatestFinishDate;
            Deadline = activity.Deadline;
            IsSummary = activity.IsSummary;
            IsVirtual = activity.IsVirtual;
            Duration = activity.Duration;
            
            Predecessors = activity.Predecessors.Where(p=>p.Target != null && p.Source !=null).Select(p => new dtoCPMlink() { IdSource = p.Source.Id, IdTarget = p.Target.Id, LeadLag = p.LeadLag, Type = p.Type }).ToList();
            Successors = activity.Successors.Where(p => p.Target != null && p.Source != null).Select(p => new dtoCPMlink() { IdSource = p.Source.Id, IdTarget = p.Target.Id, LeadLag = p.LeadLag, Type = p.Type }).ToList();
            IdChildren = (activity.Children == null) ? new List<long>() : activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.Id).ToList();
        }

        public dtoCPMactivity(PmActivity activity)
        {
            Id = activity.Id;
            IdParent = (activity.Parent != null) ? activity.Parent.Id : 0;
            Parent = (activity.Parent != null) ? new dtoCPMactivity(activity.Parent) : null;
            Name = activity.Name;
            Completeness = activity.Completeness;
            IsDurationEstimated = activity.IsDurationEstimated;
            DisplayOrder = activity.DisplayOrder;
            EarlyStart = activity.EarlyStart;
            EarlyFinish = activity.EarlyFinish;
            LatestStart = activity.LatestStart;
            LatestFinish = activity.LatestFinish;
            EarlyStartDate = activity.EarlyStartDate;
            EarlyFinishDate = activity.EarlyFinishDate;
            LatestStartDate = activity.LatestStartDate;
            LatestFinishDate = activity.LatestFinishDate;
            Deadline = activity.Deadline;
            IsSummary = activity.IsSummary;
            IsVirtual = false;
            Duration = activity.Duration;
            Predecessors = activity.Predecessors.Where(p => p.Target != null && p.Source != null).Select(p => new dtoCPMlink() { IdSource = p.Source.Id, IdTarget = p.Target.Id, LeadLag = p.LeadLag, Type = p.Type }).ToList();
            Successors = activity.Successors.Where(p => p.Target != null && p.Source != null).Select(p => new dtoCPMlink() { IdSource = p.Source.Id, IdTarget = p.Target.Id, LeadLag = p.LeadLag, Type = p.Type }).ToList();
            IdChildren = (activity.Children == null) ? new List<long>() : activity.Children.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.Id).ToList();
        }
        #region "for cpm"
            public dtoCPMactivity(String name)
                : this()
            {
                Name = name;
            }

            public dtoCPMactivity(Int64 id, String name)
                : this()
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return String.Format("[{0}] {1} {12} [{2}] {3}-{4} {5}-{6} [{7},{8},{9},{10} {11}] ",
                    Id.ToString().PadLeft(3, ' '), Name, Duration.ToString().PadLeft(3, ' '),
                    EarlyStartDate.ToShortDateString(),
                    EarlyFinishDate.ToShortDateString(),
                    LatestStartDate.ToShortDateString(),
                    LatestFinishDate.ToShortDateString(),
                    EarlyStart.ToString().PadLeft(3, ' '),
                    EarlyFinish.ToString().PadLeft(3, ' '),
                    LatestStart.ToString().PadLeft(3, ' '),
                    LatestFinish.ToString().PadLeft(3, ' '),
                    Slack.ToString().PadLeft(3, ' '),
                    DisplayOrder.ToString().PadLeft(4, ' ')
                    );
            }
        #endregion
    }


    [Serializable]
    public class dtoCPMlink
    {
        public virtual long IdSource { get; set; }
        public virtual long IdTarget { get; set; }
        public virtual dtoCPMactivity Source { get; set; }
        public virtual dtoCPMactivity Target { get; set; }
        public virtual PmActivityLinkType Type { get; set; }
        public virtual Double LeadLag { get; set; }
        public virtual Boolean isVirtual { get; set; }

        public dtoCPMlink()
        {
        }
        public dtoCPMlink(dtoCPMactivity source, ParsedActivityLink pal, dtoCPMactivity target)
        {
            try
            {
                Source = source;
                Target = target;
                Type = pal.LinkType;
                LeadLag = pal.LeadLag;

                Source.Predecessors.Add(this);
                Target.Successors.Add(this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public dtoCPMlink(dtoCPMactivity source, ParsedActivityLink pal, Dictionary<Int64, dtoCPMactivity> dict)
        {
            try
            {
                Source = source;
                Target = dict[pal.Id];
                Type = pal.LinkType;
                LeadLag = pal.LeadLag;

                Source.Predecessors.Add(this);
                Target.Successors.Add(this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public dtoCPMlink(Int64 Id, ParsedActivityLink pal, Dictionary<Int64, dtoCPMactivity> dict)
        {
            try
            {
                Source = dict[Id];
                Target = dict[pal.Id];
                Type = pal.LinkType;
                LeadLag = pal.LeadLag;

                Source.Predecessors.Add(this);
                Target.Successors.Add(this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override string ToString()
        {
            return String.Format("{0}->{1} [{2}] {3}",
                this.Source.Id,
                this.Target.Id,
                this.LeadLag > 0 ? (this.LeadLag != 0 ? "+" + this.LeadLag.ToString() : "") : this.LeadLag.ToString(),
                this.Type
                );
        }
    }
}