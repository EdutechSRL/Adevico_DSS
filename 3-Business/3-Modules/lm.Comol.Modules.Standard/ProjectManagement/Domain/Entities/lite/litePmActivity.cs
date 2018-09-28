using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Business;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class litePmActivity
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual long IdProject { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String Notes { get; set; }
        public virtual int Completeness { get; set; }
        public virtual ProjectItemStatus Status { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual litePmActivity Parent { get; set; }
        public virtual IList<litePmActivity> Children { get; set; }
        public virtual long Depth { get; set; }
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
            public virtual Boolean IsCompleted { get; set; }
            public virtual Boolean IsSummary { get; set; }
            public virtual long WBSindex { get; set; }
            public virtual String WBSstring { get; set; }
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
            public virtual IList<litePmActivityLink> Predecessors { get { return (PredecessorLinks == null) ? new List<litePmActivityLink>() : PredecessorLinks.Where(l => l.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList(); } }
            public virtual IList<litePmActivityLink> Successors { get { return (SuccessorLinks == null) ? new List<litePmActivityLink>() : SuccessorLinks.Where(l => l.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList(); } }

            public virtual IList<litePmActivityLink> PredecessorLinks { get; set; }
            public virtual IList<litePmActivityLink> SuccessorLinks { get; set; }
            
        #endregion
            public virtual IList<liteProjectActivityAssignment> CurrentAssignments { get { return (Assignments == null) ? new List<liteProjectActivityAssignment>() : Assignments.Where(l => l.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList(); } }
            public virtual IList<liteProjectActivityAssignment> Assignments { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }

        public litePmActivity()
        {
            PredecessorLinks = new List<litePmActivityLink>();
            SuccessorLinks = new List<litePmActivityLink>();
            Children = new List<litePmActivity>();
            Assignments = new List<liteProjectActivityAssignment>();
        }

        #region "for cpm"
            public litePmActivity(String name)
                : this()
            {
                Name = name;
            }

            public litePmActivity(Int64 id, String name)
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
}