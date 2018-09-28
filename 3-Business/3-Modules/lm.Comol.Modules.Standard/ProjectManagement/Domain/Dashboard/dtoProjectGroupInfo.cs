using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectGroupInfo
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String VirtualProjectName { get { return Name + Id.ToString(); } }
        public virtual ProjectItemStatus Status { get; set; }
        public virtual Boolean isPersonal { get; set; }
        public virtual Boolean isPortal { get; set; }

        public virtual int Completeness { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual Boolean IsApproved { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual DateTime? VirtualDeadline { get { return (Deadline.HasValue) ? Deadline : EndDate; } }
        public virtual DateTime VirtualEndDate { get { return (EndDate.HasValue) ? EndDate.Value : StartDate.Value; } }

        public virtual Int32 IdCommunity { get; set; }
        public virtual String CommunityName { get; set; }
        public virtual String VirtualCommunityName { get { return CommunityName + IdCommunity.ToString(); } }
        

        public virtual Boolean SetMyCompletion { get; set; }
        public virtual Boolean ViewMyCompletion { get; set; }
        public virtual Boolean SetOthersCompletion { get; set; }
        public virtual String ProjectDashboard { get; set; }
        public virtual dtoProjectUrls ProjectUrls { get; set; }
        public virtual Dictionary<ResourceActivityStatus, long> UserCompletion { get; set; }
        private PmActivityPermission projectPermissions;
        public virtual PmActivityPermission ProjectPermissions { get { return projectPermissions; } }
        public virtual BaseStatusDeleted Deleted { get; set; }

        public dtoProjectGroupInfo() {
            projectPermissions = PmActivityPermission.None;
            ProjectUrls = new dtoProjectUrls();
            UserCompletion = new Dictionary<ResourceActivityStatus, long>();
        }

        public dtoProjectGroupInfo(Project project, PmActivityPermission permissions = PmActivityPermission.None, ProjectResource resource = null)
        {
            ProjectUrls = new dtoProjectUrls();
            UserCompletion = new Dictionary<ResourceActivityStatus, long>();
            if (project == null)
            {
                Id = 0;
                Name = "";
                IdCommunity = -1;
            }
            else {
                Id = project.Id;
                IsCompleted = project.IsCompleted;
                IdCommunity = (project.Community == null) ? (project.isPortal ? 0 : -1) : project.Community.Id;
                Name = project.Name;
                Completeness = project.Completeness;
                Status = project.Status;
                CommunityName = (project.Community != null) ? project.Community.Name : "";
                Deleted = project.Deleted;
                isPersonal = project.isPersonal;
                isPortal = project.isPortal;
                Deleted = project.Deleted;
                StartDate = project.StartDate;
                EndDate = project.EndDate;
                Deadline = project.Deadline;
            }
            if (resource != null) {
                long notStarted = resource.AssignedActivities;
                UserCompletion[ResourceActivityStatus.completed] = resource.CompletedActivities;
                notStarted -= resource.CompletedActivities;
                if (project.ConfirmCompletion)
                {
                    UserCompletion[ResourceActivityStatus.confirmed] = resource.ConfirmedActivities;
                    notStarted -= resource.ConfirmedActivities;
                }
                notStarted = notStarted - resource.LateActivities - resource.StartedActivities;
                if (notStarted < 0)
                    notStarted = 0;
                UserCompletion[ResourceActivityStatus.late] = resource.LateActivities;
                UserCompletion[ResourceActivityStatus.started] = resource.StartedActivities;
                UserCompletion[ResourceActivityStatus.notstarted] = notStarted;
                UserCompletion[ResourceActivityStatus.all] = resource.AssignedActivities;

            }
            SetPermissions(permissions);
        }
        public dtoProjectGroupInfo(liteProjectSettings project, PmActivityPermission permissions = PmActivityPermission.None, liteResource resource = null)
        {
            ProjectUrls = new dtoProjectUrls();
            UserCompletion = new Dictionary<ResourceActivityStatus, long>();
            if (project == null)
            {
                Id = 0;
                Name = "";
                IdCommunity = -1;
            }
            else
            {
                Id = project.Id;
                IsCompleted = project.IsCompleted;
                IdCommunity = project.IdCommunity;
                Name = project.Name;
                Completeness = project.Completeness;
                Status = project.Status;
                Deleted = project.Deleted;
                isPersonal = project.isPersonal;
                isPortal = project.isPortal;
                Deleted = project.Deleted;
                StartDate = project.StartDate;
                EndDate = project.EndDate;
                Deadline = project.Deadline;
            }
            if (resource != null)
            {
                long notStarted = resource.AssignedActivities;
                UserCompletion[ResourceActivityStatus.completed] = resource.CompletedActivities;
                notStarted -= resource.CompletedActivities;
                if (project.ConfirmCompletion)
                {
                    UserCompletion[ResourceActivityStatus.confirmed] = resource.ConfirmedActivities;
                    notStarted -= resource.ConfirmedActivities;
                }
                notStarted = notStarted - resource.LateActivities - resource.StartedActivities;
                if (notStarted < 0)
                    notStarted = 0;
                UserCompletion[ResourceActivityStatus.late] = resource.LateActivities;
                UserCompletion[ResourceActivityStatus.started] = resource.StartedActivities;
                UserCompletion[ResourceActivityStatus.notstarted] = notStarted;
                UserCompletion[ResourceActivityStatus.all] = resource.AssignedActivities;

            }
            SetPermissions(permissions);
        }


        public void SetPermissions(PmActivityPermission pPermissions)
        {
            projectPermissions = pPermissions;
            SetMyCompletion = ((pPermissions & PmActivityPermission.SetMyCompleteness) == PmActivityPermission.SetMyCompleteness);
            ViewMyCompletion = SetMyCompletion;
            SetMyCompletion = (Status != ProjectItemStatus.completed) && SetMyCompletion;
            SetOthersCompletion = ((pPermissions & PmActivityPermission.SetCompleteness) == PmActivityPermission.SetCompleteness);
        }
        public Boolean HasPermission(PmActivityPermission permission)
        {
            return ((ProjectPermissions & permission) == permission);
        }
    }
}