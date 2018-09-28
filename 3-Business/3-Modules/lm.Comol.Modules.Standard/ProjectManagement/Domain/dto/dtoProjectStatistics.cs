using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectStatistics
    {
        public virtual Boolean ConfirmCompletion { get; set; }
        public virtual Boolean AllowMilestones { get; set; }
        public virtual Boolean AllowEstimatedDuration { get; set; }
        public virtual Boolean AllowSummary { get; set; }
        public virtual FlagDayOfWeek DaysOfWeek { get; set; }
        public virtual List<dtoResource> Resources { get; set; }
        public virtual ProjectVisibility Visibility { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual Boolean DateCalculationByCpm { get; set; }
        public virtual ProjectAvailability Availability { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }
        public virtual long EstimatedActivities { get; set; }
        public virtual long Activities { get; set; }
        public virtual long Summaries { get; set; }
        public virtual long Milestones { get; set; }

        public dtoProjectStatistics() 
        {
            Resources = new List<dtoResource>();
            DateCalculationByCpm = true;
            Availability = ProjectAvailability.Draft;
            DaysOfWeek = FlagDayOfWeek.WorkWeek;
            AllowEstimatedDuration = false;
            AllowMilestones = true;
            ConfirmCompletion = false;
            IsDurationEstimated = false;
            AllowSummary = true;
            Visibility = ProjectVisibility.Full;
        }

        public dtoProjectStatistics(Project project)
        {
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            Deadline = project.Deadline;
            Visibility = project.Visibility;
            ConfirmCompletion = project.ConfirmCompletion;
            AllowMilestones = project.AllowMilestones;
            AllowEstimatedDuration = project.AllowEstimatedDuration;
            DaysOfWeek = project.DaysOfWeek;
            Resources = (project.Resources == null) ? new List<dtoResource>() : project.Resources.Where(r=> r.Deleted== BaseStatusDeleted.None).Select(r=> new dtoResource() { IdResource=r.Id, LongName= r.GetLongName(), ShortName=r.GetShortName(), DefaultForActivity = r.DefaultForActivity, ProjectRole= r.ProjectRole}).ToList();
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            Deadline = project.Deadline;
            DateCalculationByCpm = project.DateCalculationByCpm;
            Availability = project.Availability;
            IsDurationEstimated = project.IsDurationEstimated;
            AllowSummary = project.AllowSummary;
        }
    }
}