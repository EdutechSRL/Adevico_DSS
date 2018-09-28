using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.GraphTheory;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    ///
    public class dtoActivityTreeItem
    {
        public virtual long Id { get; set; }
        public virtual long IdParent { get; set; }
        public virtual long RowNumber { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual String Name { get; set; }
        public virtual long WBSindex { get; set; }
        public virtual long Depth { get; set; }
        public virtual FieldStatus Status { get; set; }
        public virtual String WBSstring { get; set; }
       
        
        public virtual Boolean IsSummary { get; set; }
        public virtual DateTime? EarlyStartDate { get; set; }
        public virtual DateTime? EarlyFinishDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual Boolean isAfterDeadline
        {
            get
            {
                return (Deadline != null && Deadline.HasValue) ? (this.EarlyFinishDate >= Deadline.Value) : false;
            }
        }
        public virtual Boolean isCritical { get; set; }
        public virtual Boolean isMilestone { get {
            return (Duration.Value==0);
        }}
        public virtual dtoDuration Duration { get; set; }
        public virtual String Predecessors { get; set; }
        public virtual Boolean HasLinks { get { return Links != null || Links.Any() || (Children != null && (Children.Where(c=> c.HasLinks).Any())); } }
        public virtual List<dtoActivityTreeItem> Children { get; set; }
        internal virtual List<ParsedActivityLink> Links { get; set; }
        public dtoActivityTreeItem(){
            Children = new List<dtoActivityTreeItem>();
            Duration = new dtoDuration();
        }

        public dtoActivityTreeItem(litePmActivity task, long rowNumber)
        {
            Id = task.Id;
            IdParent = (task.Parent != null) ? task.Parent.Id : 0;
            RowNumber = rowNumber;
            Name =  task.Name;
            DisplayOrder = task.DisplayOrder;
            WBSindex = task.WBSindex;
            WBSstring = task.WBSstring;
            Deadline = task.Deadline;
            IsSummary = task.IsSummary;
            EarlyStartDate =  task.EarlyStartDate;
            EarlyFinishDate =  task.EarlyFinishDate;
            isCritical = task.isCritical;
            Duration = new dtoDuration(task.Duration, task.IsDurationEstimated);
            Links = task.Predecessors.Where(p => p.Target != null).Select(p => new ParsedActivityLink() { Id = p.Target.Id, LeadLag = p.LeadLag, LinkType = p.Type }).ToList();
            Children = new List<dtoActivityTreeItem>();
            Depth = task.Depth;
        }

        public long GetMaxDisplayOrder() {
            return (Children == null || !Children.Any()) ? DisplayOrder : Children.Select(c => c.GetMaxDisplayOrder()).Max();
        }
        public override string ToString()
        {
            return String.Format("[{0}]  idParent:{1} d{2} [{3}-{4}",
             Id.ToString().PadLeft(3, ' '), IdParent.ToString().PadLeft(3, ' '),
             (EarlyStartDate.HasValue ? EarlyStartDate.Value.ToShortDateString() : ""),(EarlyFinishDate.HasValue ? EarlyFinishDate.Value.ToShortDateString() : "")
             );
        }
    }
}