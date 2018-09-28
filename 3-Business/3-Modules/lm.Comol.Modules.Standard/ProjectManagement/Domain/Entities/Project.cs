using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class Project : BaseProjectItem
    {
        public virtual Boolean isPersonal { get; set; }
        public virtual Boolean isArchived { get; set; }
        public virtual Boolean isPortal { get; set; }
        public virtual Boolean ConfirmCompletion { get; set; }
        public virtual Boolean AllowSummary { get; set; }
        public virtual Boolean AllowMilestones { get; set; }
        public virtual Boolean AllowEstimatedDuration { get; set; }
        public virtual Boolean SetDefaultResourcesToNewActivity { get; set; }
        public virtual FlagDayOfWeek DaysOfWeek { get; set; }
        public virtual IList<ProjectResource> Resources { get; set; }
        public virtual IList<PmActivity> Children { get { return (Activities == null) ? new List<PmActivity>() : Activities.Where(a => a.Depth == 0 && a.Parent == null).ToList(); } }
        public virtual IList<PmActivity> Activities { get; set; }
        public virtual ProjectVisibility Visibility { get; set; }
        public virtual IList<ProjectDateException> DateExceptions { get; set; }
        public virtual IList<ProjectCalendar> Calendars { get; set; }
        public virtual IList<ProjectAttachment> Attachments { get; set; }
        public virtual IList<ProjectAttachmentLink> AttachmentLinks { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual DateTime? LastMapUpdate { get; set; }
        
        public virtual Boolean DateCalculationByCpm { get; set; }
        public virtual List<ProjectResource> DefaultActivityResources { get { return (Resources == null) ? new List<ProjectResource>() : Resources.Where(r => r.Deleted == BaseStatusDeleted.None && r.DefaultForActivity).Select(r => r).ToList(); } }
        public virtual Double Duration { get; set; }
        public virtual ProjectAvailability Availability { get; set; }

        public virtual WorkingDay DefaultWorkingDay
        {
            get
            {
                return (Calendars != null && Calendars.Where(c => c.Type == CalendarType.Project).Any()) ?
                    Calendars.Where(c => c.Type == CalendarType.Project).FirstOrDefault().DefaultWorkingDay
                    :
                     WorkingDay.GetDefault();
            }
        }
 
        public Project()
        {
            Calendars = new List<ProjectCalendar>();
            DateExceptions = new List<ProjectDateException>();
            Activities = new List<PmActivity>();
            Attachments = new List<ProjectAttachment>();
            AttachmentLinks = new List<ProjectAttachmentLink>();
            Resources = new List<ProjectResource>();
            StartDate = DateTime.Now;
            ConfirmCompletion = false;
            AllowMilestones = false;
            AllowEstimatedDuration = false;
            SetDefaultResourcesToNewActivity = false;
            Availability = ProjectAvailability.Draft;
        }

        public virtual void RecalcDurationEstimated()
        {
            Boolean isEstimated = IsDurationEstimated;
            if (Children != null)
                IsDurationEstimated = Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Where(c => c.IsDurationEstimated).Any();
        }
        //public virtual void RecalcDuration()
        //{
        //    if (Children != null && Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Any())
        //        Duration = Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Select(c => c.Duration).Sum();

        //}
        public virtual long GetMaxDisplayOrder()
        {
            long displayOrder = 0;
            if (Children != null && Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).Any())
            {
                PmActivity child = Children.Where(c => c.Deleted == Core.DomainModel.BaseStatusDeleted.None).OrderByDescending(c => c.DisplayOrder).FirstOrDefault();
                displayOrder = (child == null) ? 0 : child.GetMaxChildrenDisplayOrder();
            }
            return displayOrder;
        }
    }
}