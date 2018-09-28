using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class PmActivity : BaseProjectItem
    {
        public virtual Project Project { get; set; }
        public virtual String Notes { get; set; }
        public virtual PmActivity Parent { get; set; }
        public virtual IList<PmActivity> Children { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual long WBSindex { get; set; }
        public virtual String WBSstring { get; set; }
        public virtual long Depth { get; set; }
        #region "For cpm"
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
            public virtual Double ActualDuration { get; set; }
        
            public virtual IList<PmActivityLink> Predecessors { get { return (PredecessorLinks == null) ? new List<PmActivityLink>() : PredecessorLinks.Where(l => l.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList(); } }
            public virtual IList<PmActivityLink> Successors { get { return (SuccessorLinks == null) ? new List<PmActivityLink>() : SuccessorLinks.Where(l => l.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList(); } }

            public virtual IList<PmActivityLink> PredecessorLinks { get; set; }
            public virtual IList<PmActivityLink> SuccessorLinks { get; set; }
        #endregion
            public virtual IList<ProjectActivityAssignment> CurrentAssignments { get { return (Assignments == null) ? new List<ProjectActivityAssignment>() : Assignments.Where(l => l.Deleted == Core.DomainModel.BaseStatusDeleted.None && l.Resource !=null).ToList(); } }
            public virtual IList<ProjectActivityAssignment> Assignments { get; set; }

        public PmActivity()
        {
            PredecessorLinks = new List<PmActivityLink>();
            SuccessorLinks = new List<PmActivityLink>();
            Children = new List<PmActivity>();
            Assignments = new List<ProjectActivityAssignment>();
        }

        public virtual Boolean isEqualTo(dtoLiteMapActivity activity)
        {
            return Id == activity.IdActivity && ((activity.IdParent == 0 && Parent == null) || (Parent != null && activity.IdParent > 0 && Parent.Id == activity.IdParent))
                && (DisplayOrder == activity.RowNumber) && (IsSummary == activity.IsSummary) && (activity.Previous.Name == Name)
                && (activity.Previous.EarlyStartDate == EarlyStartDate) && (activity.Previous.Duration.Value == Duration && activity.Previous.Duration.IsEstimated== IsDurationEstimated)
                && (
                (
                String.IsNullOrEmpty(activity.Previous.Predecessors ) && (PredecessorLinks == null || (PredecessorLinks !=null && PredecessorLinks.Where(p=> p.Deleted== Core.DomainModel.BaseStatusDeleted.None).Any())
                )
                ||
                (PredecessorLinks!=null && PredecessorsToIdString() == activity.Previous.PredecessorsIdString)

                ));
        }
        public virtual Boolean isModified(dtoLiteMapActivity activity,ModifyPolicy policy)
        {
            switch (policy) { 
                case ModifyPolicy.FullFields:
                    return isEqualTo(activity);
                case ModifyPolicy.DateCalculationFields:
                    return Id == activity.IdActivity && ((activity.IdParent == 0 && Parent == null) || (Parent != null && activity.IdParent > 0 && Parent.Id == activity.IdParent))
                           && (IsSummary == activity.IsSummary)
                           && (activity.Previous.EarlyStartDate == EarlyStartDate) && (activity.Previous.Duration.Value == Duration && activity.Previous.Duration.IsEstimated== IsDurationEstimated)
                           && (
                           (
                           String.IsNullOrEmpty(activity.Previous.Predecessors) && (PredecessorLinks == null || (PredecessorLinks != null && PredecessorLinks.Where(p => p.Deleted == Core.DomainModel.BaseStatusDeleted.None).Any())
                           )
                           ||
                           (PredecessorLinks != null && PredecessorsToIdString() == activity.Previous.PredecessorsIdString)

                           ));
            }
            return false;
        }
        public virtual long GetMaxChildrenDisplayOrder(){
            long displayOrder = DisplayOrder;
            if (Children != null && Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Any()) {
                PmActivity child = Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).OrderByDescending(c=> c.DisplayOrder).FirstOrDefault();
                displayOrder = (child.Children == null || !Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Any()) ? child.DisplayOrder : child.GetMaxChildrenDisplayOrder();
            }
            return displayOrder;
        }
        public virtual void SetDurationEstimated(Boolean value)
        {
            Boolean isEstimated = IsDurationEstimated;
            IsDurationEstimated = value;
            if (isEstimated != IsDurationEstimated || (Parent != null && Parent.IsDurationEstimated != IsDurationEstimated) || (Parent==null && Depth==0 && Project !=null && Project.IsDurationEstimated != IsDurationEstimated))
            {
                if (Parent != null)
                    Parent.RecalcDurationEstimated();
                else
                    Project.RecalcDurationEstimated();
            }
        }
        public virtual void RecalcDurationEstimated()
        {
            Boolean isEstimated = IsDurationEstimated;
            if (Children != null)
                IsDurationEstimated = Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Where(c => c.IsDurationEstimated).Any();
            if (isEstimated!=IsDurationEstimated){
                if (Parent !=null)
                    Parent.RecalcDurationEstimated();
                else
                    Project.RecalcDurationEstimated();
            }
        }
        public virtual Boolean isLate(DateTime date)
        {
            return (EarlyStartDate != null && EarlyStartDate.Value.Date.Ticks < date.Ticks) || (Deadline != null && Deadline.Value.Date.Ticks < date.Ticks);
        }
        //public virtual void RecalcDuration()
        //{
        //    if (Children != null && Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Any())
        //        Duration =  Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Select(c=>c.Duration).Sum();
        //    if (Parent == null)
        //        Project.RecalcDuration();
        //    else
        //        Parent.RecalcDuration();
         
        //}
        private String PredecessorsToIdString() {
            return String.Join(";", Predecessors.Where(p => p.Target != null).OrderBy(p => p.Target.Id).Select(p => p.Target.Id.ToString() + p.Type.ToString() + p.LeadLag.ToString()).ToList());
        }
    }
}