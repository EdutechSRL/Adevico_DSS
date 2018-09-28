using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProject : dtoBaseProjectItem
    {
        public virtual Boolean isPersonal { get; set; }
        public virtual Boolean isArchived { get; set; }
        public virtual Boolean isPortal { get; set; }
        public virtual Boolean ConfirmCompletion { get; set; }
        public virtual Boolean AllowMilestones { get; set; }
        public virtual Boolean AllowEstimatedDuration { get; set; }
        public virtual Boolean SetDefaultResourcesToNewActivity { get; set; }
        public virtual Boolean AllowSummary { get; set; }
        public virtual FlagDayOfWeek DaysOfWeek { get; set; }
        public virtual List<dtoResource> Resources { get; set; }
        public virtual ProjectVisibility Visibility { get; set; }
        public virtual long DateExceptionsCount { get; set; }
        public virtual long CalendarsCount { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual DateTime? LastMapUpdate { get; set; }
        public virtual List<dtoCalendar> Calendars { get; set; }
        public virtual List<dtoResource> DefaultActivityResources { get; set; }
        public virtual Double Duration { get; set; }
        public virtual Boolean DateCalculationByCpm { get; set; }
        public virtual ProjectAvailability Availability { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }
        public virtual Int32 IdCreator { get; set; }
        public virtual Int32 IdOwner { get; set; }
   
        public virtual Boolean IsValid
        {
            get
            {
                return StartDate.HasValue && (!Deadline.HasValue && (Deadline.HasValue && StartDate.HasValue && StartDate.Value <= Deadline.Value))
                   && (!EndDate.HasValue && (EndDate.HasValue && StartDate.HasValue && StartDate.Value < EndDate.Value));
            }
        }
        public virtual dtoWorkingDay DefaultWorkingDay
        {
            get
            {
                return (Calendars != null && Calendars.Where(c => c.Type == CalendarType.Project).Any()) ?
                    Calendars.Where(c => c.Type == CalendarType.Project).FirstOrDefault().DefaultWorkingDay
                    :
                    dtoWorkingDay.GetDefault();
            }
        }
   
        public dtoProject() 
        {
            Calendars = new List<dtoCalendar>();
            Resources = new List<dtoResource>();
            DefaultActivityResources = new List<dtoResource>();
            DateCalculationByCpm = true;
            Availability = ProjectAvailability.Draft;
            DaysOfWeek = FlagDayOfWeek.WorkWeek;
            AllowEstimatedDuration = false;
            AllowMilestones = true;
            SetDefaultResourcesToNewActivity = false;
            ConfirmCompletion = false;
            IsDurationEstimated = false;
            AllowSummary = true;
            Visibility = ProjectVisibility.Full;
        }

        public dtoProject(Project project)
        {
            Id = project.Id;
            IdCommunity = (project.Community == null) ? 0 : project.Community.Id;
            Name = project.Name;
            Description = project.Description;
            Completeness = project.Completeness;
            Status = project.Status;
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            Deadline = project.Deadline;
            isPersonal = project.isPersonal;
            isArchived = project.isArchived;
            isPortal = project.isPortal;
            Visibility = project.Visibility;
            ConfirmCompletion = project.ConfirmCompletion;
            AllowMilestones = project.AllowMilestones;
            AllowEstimatedDuration = project.AllowEstimatedDuration;
            SetDefaultResourcesToNewActivity = project.SetDefaultResourcesToNewActivity;
            DaysOfWeek = project.DaysOfWeek;
            Resources = (project.Resources == null) ? new List<dtoResource>() : project.Resources.Where(r=> r.Deleted== BaseStatusDeleted.None).Select(r=> new dtoResource() { IdResource=r.Id, LongName= r.GetLongName(), ShortName=r.GetShortName(), DefaultForActivity = r.DefaultForActivity, ProjectRole= r.ProjectRole}).ToList();
            DateExceptionsCount = (project.DateExceptions== null) ? 0 : project.DateExceptions.Where(d=> d.Deleted== BaseStatusDeleted.None).Count();
            CalendarsCount = (project.DateExceptions == null) ? 0 : project.DateExceptions.Where(d => d.Deleted == BaseStatusDeleted.None).Count();
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            Deadline = project.Deadline;
            LastMapUpdate = project.LastMapUpdate;
            DefaultActivityResources = Resources.Where(r=> r.DefaultForActivity).ToList();
            Duration = project.Duration;
            DateCalculationByCpm = project.DateCalculationByCpm;
            Availability = project.Availability;
            IsDurationEstimated = project.IsDurationEstimated;
            AllowSummary = project.AllowSummary;
            Calendars = (project.Calendars != null && project.Calendars.Where(c=> c.Deleted== BaseStatusDeleted.None).Any()) ? project.Calendars.Where(c=> c.Deleted== BaseStatusDeleted.None).Select(c=> new dtoCalendar(c)).ToList() : new List<dtoCalendar>();
            IdCreator = (project.CreatedBy==null ? 0 : project.CreatedBy.Id);
            if (project.Resources.Any(r => r.ProjectRole == ActivityRole.ProjectOwner && r.Person != null && r.Type == ResourceType.Internal))
                IdOwner = project.Resources.Where(r => r.ProjectRole == ActivityRole.ProjectOwner && r.Person != null && r.Type == ResourceType.Internal).Select(r => r.Person.Id).FirstOrDefault();
            else
                IdOwner = IdCreator;
        }
    }
}