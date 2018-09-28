using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoPlainTask
    {
        public virtual long Id { get; set; }
        public virtual long IdAssignment { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String Notes { get; set; }
        public virtual Double Duration { get; set; }
        public virtual Boolean IsDurationEstimated { get; set; }
        public virtual int Completeness { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual dtoField<String> MyCompleteness { get; set; }
        public virtual Boolean MyAssignmentIsApproved { get; set; }
        public virtual List<dtoResource> ProjectResources { get; set; }
    
        public virtual String IdFatherRow { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual ProjectItemStatus Status { get; set; }
     
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual DateTime? VirtualDeadline { get { return (Deadline.HasValue) ? Deadline : EndDate; } }
        public virtual DateTime VirtualEndDate { get { return (EndDate.HasValue) ? EndDate.Value : StartDate.Value; } }

        public virtual List<long> IdResources { get; set; }
        public virtual List<dtoActivityCompletion> Assignments { get; set; }
        public virtual dtoTimeGroup TimeGroup { get; set; }
        public virtual dtoProjectGroupInfo ProjectInfo { get; set; }
        public virtual String ProjectDashboard {get{ return (ProjectInfo==null) ? "" : ProjectInfo.ProjectDashboard;}}
        public dtoPlainTask() 
        {
            ProjectInfo = new dtoProjectGroupInfo();
            ProjectResources = new List<dtoResource>(); 
        }
        public dtoPlainTask(PmActivity task, PmActivityPermission permissions =  PmActivityPermission.None)
        {
            ProjectResources = new List<dtoResource>();
            setDtoSettings(task, permissions);
        }
        public dtoPlainTask(ProjectActivityAssignment assignment,  Dictionary<ActivityRole, PmActivityPermission> permissions)
        {
            ProjectResources = new List<dtoResource>();
            if (assignment == null || assignment.Activity == null ){
                Id = 0;
                Deleted = BaseStatusDeleted.Manual;
            }
            else{
                setDtoSettings(assignment.Activity, (permissions.ContainsKey(assignment.Resource.ProjectRole) ? permissions[assignment.Resource.ProjectRole] : permissions[ActivityRole.Resource]), assignment.Resource );

                MyCompleteness = new dtoField<String>(assignment.Completeness.ToString() + "%");
                MyAssignmentIsApproved = assignment.IsApproved;
            }
        }
        public dtoPlainTask(liteProjectActivityAssignment assignment, Dictionary<ActivityRole, PmActivityPermission> permissions)
        {
            ProjectResources = new List<dtoResource>();
            if (assignment == null || assignment.Activity == null)
            {
                Id = 0;
                Deleted = BaseStatusDeleted.Manual;
            }
            else
                setDtoSettings(assignment, (permissions.ContainsKey(assignment.Resource.ProjectRole) ? permissions[assignment.Resource.ProjectRole] : permissions[ActivityRole.Resource]), assignment.Resource);
        }

        private void setDtoSettings(PmActivity task, PmActivityPermission permissions,ProjectResource resource = null) {
            Id = task.Id;
            ProjectInfo = new dtoProjectGroupInfo(task.Project, permissions, resource);
            
            Name = task.Name;
            Description = task.Description;
            Notes = task.Notes;
            Completeness = task.Completeness;
            IsCompleted = task.IsCompleted;
            Deleted = task.Deleted;
            Status = task.Status;
            StartDate = task.EarlyStartDate;
            EndDate = task.EarlyFinishDate;
            Deadline = task.Deadline;
            Duration = task.Duration;
            IsDurationEstimated  = task.IsDurationEstimated;
            IdResources = task.CurrentAssignments.Select(a => a.Resource.Id).ToList();
            Assignments = task.CurrentAssignments.Select(a => new dtoActivityCompletion() { Id = a.Id, IdResource = a.Resource.Id, Completeness = a.Completeness, IsApproved = a.IsApproved }).ToList();
        }
        private void setDtoSettings(liteProjectActivityAssignment assignment, PmActivityPermission permissions, liteResource resource = null)
        {
            litePmActivity task = assignment.Activity;
            Id = task.Id;
            IdAssignment = assignment.Id;
            ProjectInfo = new dtoProjectGroupInfo(assignment.Project, permissions, resource);

            Name = task.Name;
            Description = task.Description;
            Notes = task.Notes;
            Completeness = task.Completeness;
            IsCompleted = task.IsCompleted;
            Deleted = task.Deleted;
            Status = task.Status;
            StartDate = task.EarlyStartDate;
            EndDate = task.EarlyFinishDate;
            Deadline = task.Deadline;
            Duration = task.Duration;
            IsDurationEstimated = task.IsDurationEstimated;
            IdResources = task.CurrentAssignments.Select(a => a.Resource.Id).ToList();
            Assignments = task.CurrentAssignments.Select(a => new dtoActivityCompletion() { Id = Id, IdResource = a.Resource.Id, Completeness = a.Completeness, IsApproved = a.IsApproved }).ToList();
            
            MyCompleteness = new dtoField<String>(assignment.Completeness.ToString() + "%");
            MyAssignmentIsApproved = assignment.IsApproved;
        }
       // public dtoPlainTask(PmActivity task, PmActivityPermission permissions, PageContainerType container, PageListType fromPage, PageListType displayPage) 
       // {
       //     Id = task.Id;
       //     if (task.Project==null){
       //         IdProject = 0;
       //         ProjectName ="";
       //         IdCommunity = -1;
       //     }
       //     else{
               
       //         IdProject = task.Project.Id;
       //         ProjectName =task.Project.Name;
       //         CommunityName = (task.Community==null) ? "" : task.Community.Name;
       //     }
       //     Name= task.Name;
       //     Description= task.Description;
       //     Notes= task.Notes;
       //     Completeness= task.Completeness;
       //     IsCompleted= task.IsCompleted;
       //     Deleted= task.Deleted;
       //     Status= task.Status;
       //     StartDate= task.EarlyStartDate;
       //     EndDate= task.EarlyFinishDate;
       //     Deadline= task.Deadline;
       //     IdResources = task.CurrentAssignments.Select(a=> a.Resource.Id).ToList();
       //     Assignments = task.CurrentAssignments.Select(a => new dtoActivityCompletion() { Id = Id, IdResource = a.Resource.Id, Completeness = a.Completeness, IsApproved = a.IsApproved }).ToList();
       //}
        
        //public dtoPlainProject(Project project, ActivityRole projectRole, ProjectVisibility visibility, Dictionary<ActivityRole,String> roleTranslations, Boolean alsoAsResource = false)
        //{
        //    Id = project.Id;
        //    IsCompleted = project.IsCompleted;
        //    IdCommunity = (project.Community==null) ? (project.isPortal ? 0 : -1) : project.Community.Id;
        //    Name = project.Name;
        //    Completeness = project.Completeness;
        //    Status = project.Status;
        //    CommunityName = (project.Community!=null) ? project.Community.Name:"";
        //    Deleted = project.Deleted;
        //    isPersonal = project.isPersonal;
        //    isPortal = project.isPortal;
        //    Deleted = project.Deleted;
        //    Visibility = visibility;
        //    Roles = new List<dtoProjectRole>();
        //    Roles.Add(new dtoProjectRole() { ProjectRole = projectRole, LongName = roleTranslations[projectRole], ShortName = roleTranslations[projectRole].ToCharArray()[0].ToString() });
        //    if (projectRole == ActivityRole.ProjectOwner)
        //        Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Manager, LongName = roleTranslations[ActivityRole.Manager], ShortName = roleTranslations[ActivityRole.Manager].ToCharArray()[0].ToString() });
        //    if (alsoAsResource)
        //        Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Resource, LongName = roleTranslations[ActivityRole.Resource], ShortName = roleTranslations[ActivityRole.Resource].ToCharArray()[0].ToString() });

        //    StartDate = project.StartDate;
        //    EndDate = project.EndDate;
        //    Deadline = project.Deadline;
        //    Availability = project.Availability;
        //    Permissions = new dtoProjectPermission();
        //    Urls = new dtoProjectUrls();
        //}

        //public dtoPlainProject(ModuleProjectManagement modulePermission, Project project, ProjectVisibility visibility, Dictionary<ActivityRole, String> roleTranslations, Boolean alsoAsResource = false)
        //{
        //    Id = project.Id;
        //    IsCompleted = project.IsCompleted;
        //    IdCommunity = (project.Community==null) ? (project.isPortal ? 0 : -1) : project.Community.Id;
        //    Name = project.Name;
        //    Completeness = project.Completeness;
        //    Status = project.Status;
        //    CommunityName = (project.Community!=null) ? project.Community.Name:"";
        //    Deleted = project.Deleted;
        //    isPersonal = project.isPersonal;
        //    isPortal = project.isPortal;
        //    Deleted = project.Deleted;
        //    Visibility = visibility;
        //    Roles = new List<dtoProjectRole>();
        //    if (alsoAsResource)
        //        Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Resource, LongName = roleTranslations[ActivityRole.Resource], ShortName = roleTranslations[ActivityRole.Resource].ToCharArray()[0].ToString() });
        //    StartDate = project.StartDate;
        //    EndDate = project.EndDate;
        //    Deadline = project.Deadline;
        //    Availability = project.Availability;
        //    Urls = new dtoProjectUrls();
        //    Permissions = new dtoProjectPermission();
        //    Permissions.Edit = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
        //    Permissions.EditMap = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
        //    Permissions.EditResources = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
        //    Permissions.PhisicalDelete = (modulePermission.Administration || modulePermission.Edit) && project.Deleted != BaseStatusDeleted.None;
        //    Permissions.VirtualDelete = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
        //    Permissions.VirtualUndelete = (modulePermission.Administration || modulePermission.Edit) && project.Deleted != BaseStatusDeleted.None;
        //}

        //public ActivityRole GetMajorRole(PageListType view) {
        //    if (Roles==null || !Roles.Any())
        //        return ActivityRole.None;
        //    else
        //        return Roles.Where(r=> (view== PageListType.ListResource &&  r.ProjectRole> ActivityRole.Manager) || view!= PageListType.ListResource).Select(r => r.ProjectRole).OrderBy(r => r).FirstOrDefault(); 
        //}

        //public static dtoPlainProject CreateForResource(Project project, ProjectResource resource, Dictionary<ActivityRole, String> roleTranslations, PmActivityPermission pPermissions)
        //{
        //    dtoPlainProject result = new dtoPlainProject();
        //    result.Id = project.Id;
        //    result.IsCompleted = project.IsCompleted;
        //    result.IdCommunity = (project.Community == null) ? (project.isPortal ? 0 : -1) : project.Community.Id;
        //    result.Name = project.Name;
        //    result.Completeness = project.Completeness;
        //    result.Status = project.Status;
        //    result.CommunityName = (project.Community != null) ? project.Community.Name : "";
        //    result.Deleted = project.Deleted;
        //    result.isPersonal = project.isPersonal;
        //    result.isPortal = project.isPortal;
        //    result.Deleted = project.Deleted;
        //    result.Visibility = resource.Visibility;
        //    result.Roles = new List<dtoProjectRole>();
        //    result.Roles.Add(new dtoProjectRole() { ProjectRole = resource.ProjectRole, LongName = roleTranslations[resource.ProjectRole], ShortName = roleTranslations[resource.ProjectRole].ToCharArray()[0].ToString() });
        //    if (resource.ProjectRole == ActivityRole.ProjectOwner)
        //        result.Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Manager, LongName = roleTranslations[ActivityRole.Manager], ShortName = roleTranslations[ActivityRole.Manager].ToCharArray()[0].ToString() });
        //    if (resource.ProjectRole != ActivityRole.Visitor)
        //        result.Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Resource, LongName = roleTranslations[ActivityRole.Resource], ShortName = roleTranslations[ActivityRole.Resource].ToCharArray()[0].ToString() });

        //    result.StartDate = project.StartDate;
        //    result.EndDate = project.EndDate;
        //    result.Deadline = project.Deadline;
        //    result.Availability = project.Availability;
        //    result.Permissions.ViewMap = ((pPermissions & PmActivityPermission.ViewProjectMap) == PmActivityPermission.ViewProjectMap);
        //    result.Permissions.ViewMyCompletion = (result.GetMajorRole(PageListType.ListResource) == ActivityRole.Resource);
        //    long notStarted = resource.AssignedActivities;
        //    result.UserCompletion[ResourceActivityStatus.completed] = resource.CompletedActivities;
        //    notStarted -= resource.CompletedActivities;
        //    if (project.ConfirmCompletion)
        //    {
        //        result.UserCompletion[ResourceActivityStatus.confirmed] = resource.ConfirmedActivities;
        //        notStarted -= resource.ConfirmedActivities;
        //    }
        //    notStarted = notStarted - resource.LateActivities - resource.StartedActivities;
        //    if (notStarted < 0)
        //        notStarted = 0;
        //    result.UserCompletion[ResourceActivityStatus.late] = resource.LateActivities;
        //    result.UserCompletion[ResourceActivityStatus.started] = resource.StartedActivities;
        //    result.UserCompletion[ResourceActivityStatus.notstarted] = notStarted;
        //    result.UserCompletion[ResourceActivityStatus.all] = resource.AssignedActivities;

        //    return result;
        //}
        public void SetPermissions(PmActivityPermission pPermissions)
        {
            ProjectInfo.SetPermissions(pPermissions);
        }
    }
}
