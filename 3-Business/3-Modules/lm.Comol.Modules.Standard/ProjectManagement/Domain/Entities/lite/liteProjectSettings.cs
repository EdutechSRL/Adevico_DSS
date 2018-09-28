using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class liteProjectSettings
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual String Name { get; set; }
        public virtual int Completeness { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual ProjectItemStatus Status { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }

        public virtual Boolean isPersonal { get; set; }
        public virtual Boolean isArchived { get; set; }
        public virtual Boolean isPortal { get; set; }
        public virtual Boolean ConfirmCompletion { get; set; }
        public virtual Boolean AllowSummary { get; set; }
        public virtual Boolean AllowMilestones { get; set; }
        public virtual Boolean AllowEstimatedDuration { get; set; }
        public virtual FlagDayOfWeek DaysOfWeek { get; set; }
        public virtual IList<ProjectResource> Resources { get; set; }
        public virtual ProjectVisibility Visibility { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual Boolean DateCalculationByCpm { get; set; }
        public virtual Double Duration { get; set; }
        public virtual ProjectAvailability Availability { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public liteProjectSettings()
        {
            Resources = new List<ProjectResource>();
            StartDate = DateTime.Now;
            ConfirmCompletion = false;
            AllowMilestones = false;
            AllowEstimatedDuration = false;
            Availability = ProjectAvailability.Draft;
        }
    }
}