using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoPlainProject
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual String Name { get; set; }
        public virtual int Completeness { get; set; }
        public virtual Boolean IsCompleted { get; set; }
        public virtual ProjectItemStatus Status { get; set; }
        public virtual String CommunityName { get; set; }
        public virtual String VirtualName { get { return CommunityName + IdCommunity.ToString(); } }
        public virtual String IdFatherRow { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual Boolean isPersonal { get; set; }
        public virtual Boolean isPortal { get; set; }
        
        public virtual List<dtoProjectRole> Roles { get; set; }
       
        public virtual ProjectVisibility Visibility { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual DateTime? VirtualDeadline { get { return (Deadline.HasValue) ? Deadline : EndDate; } }
        public virtual DateTime VirtualEndDate { get { return (EndDate.HasValue) ? EndDate.Value : StartDate.Value; } }

        public virtual ProjectAvailability Availability { get; set; }
        public virtual dtoProjectPermission Permissions { get; set; }
        public virtual dtoProjectUrls Urls { get; set; }
        public virtual dtoTimeGroup TimeGroup { get; set; }
        public virtual Dictionary<ResourceActivityStatus, long> UserCompletion { get; set; }

        public virtual Boolean HasProjectAttachments { get; set; }
        public virtual long  ProjectAttachmentsCount { get; set; }
        public virtual Boolean HasActivitiesAttachments { get; set; }
        public dtoPlainProject() 
        {
            Availability = ProjectAvailability.Draft;
            Visibility = ProjectVisibility.Full;
            Permissions = new dtoProjectPermission();
            Roles = new List<dtoProjectRole>();
            Urls = new dtoProjectUrls();
            UserCompletion = new Dictionary<ResourceActivityStatus, long>();
        }

        public dtoPlainProject(Project project, ActivityRole projectRole, ProjectVisibility visibility, Dictionary<ActivityRole,String> roleTranslations, Boolean alsoAsResource = false)
        {
            Id = project.Id;
            IsCompleted = project.IsCompleted;
            IdCommunity = (project.Community==null) ? (project.isPortal ? 0 : -1) : project.Community.Id;
            Name = project.Name;
            Completeness = project.Completeness;
            Status = project.Status;
            CommunityName = (project.Community!=null) ? project.Community.Name:"";
            Deleted = project.Deleted;
            isPersonal = project.isPersonal;
            isPortal = project.isPortal;
            Deleted = project.Deleted;
            Visibility = visibility;
            Roles = new List<dtoProjectRole>();
            Roles.Add(new dtoProjectRole() { ProjectRole = projectRole, LongName = roleTranslations[projectRole], ShortName = roleTranslations[projectRole].ToCharArray()[0].ToString() });
            if (projectRole == ActivityRole.ProjectOwner)
                Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Manager, LongName = roleTranslations[ActivityRole.Manager], ShortName = roleTranslations[ActivityRole.Manager].ToCharArray()[0].ToString() });
            if (alsoAsResource)
                Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Resource, LongName = roleTranslations[ActivityRole.Resource], ShortName = roleTranslations[ActivityRole.Resource].ToCharArray()[0].ToString() });

            StartDate = project.StartDate;
            EndDate = project.EndDate;
            Deadline = project.Deadline;
            Availability = project.Availability;
            Permissions = new dtoProjectPermission();
            Urls = new dtoProjectUrls();
            ProjectAttachmentsCount = (project.Attachments != null ? project.Attachments.Where(a => a.Deleted == BaseStatusDeleted.None && a.IsForProject).LongCount() : 0);
            HasProjectAttachments = !(ProjectAttachmentsCount == 0);
            HasActivitiesAttachments = (project.AttachmentLinks != null && project.AttachmentLinks.Where(a => a.Deleted == BaseStatusDeleted.None && !a.IsForProject).Any());
        }

        public dtoPlainProject(ModuleProjectManagement modulePermission, Project project, ProjectVisibility visibility, Dictionary<ActivityRole, String> roleTranslations, Boolean alsoAsResource = false)
        {
            Id = project.Id;
            IsCompleted = project.IsCompleted;
            IdCommunity = (project.Community==null) ? (project.isPortal ? 0 : -1) : project.Community.Id;
            Name = project.Name;
            Completeness = project.Completeness;
            Status = project.Status;
            CommunityName = (project.Community!=null) ? project.Community.Name:"";
            Deleted = project.Deleted;
            isPersonal = project.isPersonal;
            isPortal = project.isPortal;
            Deleted = project.Deleted;
            Visibility = visibility;
            Roles = new List<dtoProjectRole>();
            if (alsoAsResource)
                Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Resource, LongName = roleTranslations[ActivityRole.Resource], ShortName = roleTranslations[ActivityRole.Resource].ToCharArray()[0].ToString() });
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            Deadline = project.Deadline;
            Availability = project.Availability;
            Urls = new dtoProjectUrls();
            Permissions = new dtoProjectPermission();
            Permissions.Edit = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            Permissions.EditMap = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            Permissions.EditResources = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            Permissions.PhisicalDelete = (modulePermission.Administration || modulePermission.Edit) && project.Deleted != BaseStatusDeleted.None;
            Permissions.VirtualDelete = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            Permissions.VirtualUndelete = (modulePermission.Administration || modulePermission.Edit) && project.Deleted != BaseStatusDeleted.None;
            Permissions.ViewAttachments = (modulePermission.Administration || modulePermission.Edit );
            Permissions.EditAttachments = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            //Permissions.ViewAttachments = ((modulePermission.Administration && !project.isPersonal) || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            //Permissions.EditAttachments = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            ProjectAttachmentsCount = (project.Attachments != null ? project.Attachments.Where(a => a.Deleted == BaseStatusDeleted.None && a.IsForProject).LongCount() : 0);
            HasProjectAttachments = !(ProjectAttachmentsCount == 0);
            HasActivitiesAttachments = (project.AttachmentLinks != null && project.AttachmentLinks.Where(a => a.Deleted == BaseStatusDeleted.None && !a.IsForProject).Any());
        }

        public ActivityRole GetMajorRole(PageListType view) {
            if (Roles==null || !Roles.Any())
                return ActivityRole.None;
            else
                return Roles.Where(r=> (view== PageListType.ListResource &&  r.ProjectRole> ActivityRole.Manager) || view!= PageListType.ListResource).Select(r => r.ProjectRole).OrderBy(r => r).FirstOrDefault(); 
        }

        public static dtoPlainProject CreateForResource(Project project, ProjectResource resource, Dictionary<ActivityRole, String> roleTranslations, PmActivityPermission pPermissions)
        {
            dtoPlainProject result = new dtoPlainProject();
            result.Id = project.Id;
            result.IsCompleted = project.IsCompleted;
            result.IdCommunity = (project.Community == null) ? (project.isPortal ? 0 : -1) : project.Community.Id;
            result.Name = project.Name;
            result.Completeness = project.Completeness;
            result.Status = project.Status;
            result.CommunityName = (project.Community != null) ? project.Community.Name : "";
            result.Deleted = project.Deleted;
            result.isPersonal = project.isPersonal;
            result.isPortal = project.isPortal;
            result.Deleted = project.Deleted;
            result.Visibility = resource.Visibility;
            result.Roles = new List<dtoProjectRole>();
            result.Roles.Add(new dtoProjectRole() { ProjectRole = resource.ProjectRole, LongName = roleTranslations[resource.ProjectRole], ShortName = roleTranslations[resource.ProjectRole].ToCharArray()[0].ToString() });
            if (resource.ProjectRole == ActivityRole.ProjectOwner)
                result.Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Manager, LongName = roleTranslations[ActivityRole.Manager], ShortName = roleTranslations[ActivityRole.Manager].ToCharArray()[0].ToString() });
            if (resource.ProjectRole != ActivityRole.Visitor)
                result.Roles.Add(new dtoProjectRole() { ProjectRole = ActivityRole.Resource, LongName = roleTranslations[ActivityRole.Resource], ShortName = roleTranslations[ActivityRole.Resource].ToCharArray()[0].ToString() });

            result.StartDate = project.StartDate;
            result.EndDate = project.EndDate;
            result.Deadline = project.Deadline;
            result.Availability = project.Availability;
            result.Permissions.ViewMap = ((pPermissions & PmActivityPermission.ViewProjectMap) == PmActivityPermission.ViewProjectMap);
            result.Permissions.ViewMyCompletion = (result.GetMajorRole(PageListType.ListResource) == ActivityRole.Resource);
            result.Permissions.ViewAttachments = result.Permissions.ViewMap;
            result.Permissions.EditAttachments = ((pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject);
            //Permissions.ViewAttachments = ((modulePermission.Administration && !project.isPersonal) || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            //Permissions.EditAttachments = (modulePermission.Administration || modulePermission.Edit) && project.Deleted == BaseStatusDeleted.None;
            //Boolean allowSave = (mPermission.Administration && !isPersonal) || ((pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject);

            long notStarted = resource.AssignedActivities;
            result.UserCompletion[ResourceActivityStatus.completed] = resource.CompletedActivities;
            notStarted -= resource.CompletedActivities;
            if (project.ConfirmCompletion)
            {
                result.UserCompletion[ResourceActivityStatus.confirmed] = resource.ConfirmedActivities;
                notStarted -= resource.ConfirmedActivities;
            }
            notStarted = notStarted - resource.LateActivities - resource.StartedActivities;
            if (notStarted < 0)
                notStarted = 0;
            result.UserCompletion[ResourceActivityStatus.late] = resource.LateActivities;
            result.UserCompletion[ResourceActivityStatus.started] = resource.StartedActivities;
            result.UserCompletion[ResourceActivityStatus.notstarted] = notStarted;
            result.UserCompletion[ResourceActivityStatus.all] = resource.AssignedActivities;

            result.ProjectAttachmentsCount = (project.Attachments != null ? project.Attachments.Where(a => a.Deleted == BaseStatusDeleted.None && a.IsForProject).LongCount() : 0);
            result.HasProjectAttachments = !(result.ProjectAttachmentsCount == 0);
            result.HasActivitiesAttachments = (project.AttachmentLinks != null && project.AttachmentLinks.Where(a => a.Deleted == BaseStatusDeleted.None && !a.IsForProject).Any());
            return result;
        }
        public void UpdatePermissions(PmActivityPermission pPermissions, PageListType view)
        {
            if (view == PageListType.ListAdministrator || view == PageListType.ListManager)
            {
                Permissions.Edit = Deleted == BaseStatusDeleted.None && ((pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject);
                Permissions.EditMap = Deleted == BaseStatusDeleted.None && ((pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject);
                Permissions.EditResources = Deleted == BaseStatusDeleted.None && ((pPermissions & PmActivityPermission.ManageResources) == PmActivityPermission.ManageResources);
                Permissions.PhisicalDelete = (Deleted != BaseStatusDeleted.None && ((pPermissions & PmActivityPermission.PhisicalDelete) == PmActivityPermission.PhisicalDelete));
                Permissions.VirtualDelete = (Deleted == BaseStatusDeleted.None && ((pPermissions & PmActivityPermission.VirtualDelete) == PmActivityPermission.VirtualDelete));
                Permissions.VirtualUndelete = (Deleted != BaseStatusDeleted.None && ((pPermissions & PmActivityPermission.VirtualUnDelete) == PmActivityPermission.VirtualUnDelete));
                Permissions.ViewAttachments = ((pPermissions & PmActivityPermission.DownloadAttacchments) == PmActivityPermission.DownloadAttacchments) ||
                    ((pPermissions & PmActivityPermission.ManageAttachments) == PmActivityPermission.ManageAttachments);
                Permissions.EditAttachments = (Deleted != BaseStatusDeleted.None && ((pPermissions & PmActivityPermission.ManageAttachments) == PmActivityPermission.ManageAttachments));
            }
            Permissions.ViewMap = ((pPermissions & PmActivityPermission.ViewProjectMap)== PmActivityPermission.ViewProjectMap);
            Permissions.ViewMyCompletion = (GetMajorRole(view) == ActivityRole.Resource);
        }
    }
}