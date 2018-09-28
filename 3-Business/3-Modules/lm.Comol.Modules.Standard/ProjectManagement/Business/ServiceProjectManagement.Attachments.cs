using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;
using lm.Comol.Modules.Standard.GraphTheory;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.FileRepository.Domain;
namespace lm.Comol.Modules.Standard.ProjectManagement.Business
{
    public partial class ServiceProjectManagement : BaseCoreServices
    {
        #region "Attachments"
            /// <summary>
            /// Get permissions for Community repository
            /// </summary>
            /// <param name="idCommunity"></param>
            /// <returns></returns>
            public ModuleRepository GetRepositoryPermissions(Int32 idCommunity) {
                return GetRepositoryPermissions(idCommunity,UC.CurrentUserID);
            }
            public ModuleRepository GetRepositoryPermissions(Int32 idCommunity,Int32 idUser)
            {
                return new ModuleRepository(Manager.GetModulePermission(idUser, idCommunity, ModuleRepository.UniqueCode));
            }
            /// <summary>
            /// Get available upload options for specific project permissions and community
            /// </summary>
            /// <param name="pPermissions"></param>
            /// <param name="cPermissions"></param>
            /// <returns></returns>
            public List<RepositoryAttachmentUploadActions> AttachmentsGetAvailableUploadActions(Boolean isModuleAdministrator,PmActivityPermission pPermissions, ModuleRepository cPermissions){
                List<RepositoryAttachmentUploadActions> actions = new List<RepositoryAttachmentUploadActions>();
                if (isModuleAdministrator || (pPermissions & PmActivityPermission.ManageProject) == PmActivityPermission.ManageProject || (pPermissions & PmActivityPermission.AddAttachments) == PmActivityPermission.AddAttachments)
                {
                    actions.Add(RepositoryAttachmentUploadActions.uploadtomoduleitem);
                    if (cPermissions!= null){
                        if (cPermissions.Administration || cPermissions.UploadFile)
                            actions.Add(RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity);
                         if (cPermissions.ViewItemsList)
                            actions.Add(RepositoryAttachmentUploadActions.linkfromcommunity);
                    }
                    actions.Add(RepositoryAttachmentUploadActions.addurltomoduleitem);
                }
                return actions;
            }
            //public List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> AttachmentsGetAvailableUploadActions()
            //{
            //    List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = new List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions>();

            //    return actions;
            //}

            //public List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> GetAttachmentsAvailableActions()
            //{

            //    ModuleRepository moduleRepository = View.RepositoryPermission(CommunityId);
            //    CoreItemPermission oPermission = Service.GetItemPermission(oItem, module, moduleRepository);
            //    if (oPermission.AllowAddFiles || oPermission.AllowEdit)
            //    {
            //        if (oItem.CommunityOwner == null)
            //        {
            //            View.AllowCommunityUpload = false;
            //            View.AllowCommunityLink = false;
            //        }
            //        View.AllowCommunityUpload = oPermission.AllowEdit && (moduleRepository.Administration || moduleRepository.UploadFile);
            //        View.AllowCommunityLink = oPermission.AllowEdit && (moduleRepository.Administration || moduleRepository.UploadFile || moduleRepository.ListFiles || moduleRepository.DownLoad);
            //        if (oPermission.AllowEdit && (moduleRepository.Administration || moduleRepository.UploadFile))
            //        {
            //            View.InitializeCommunityUploader(0, CommunityId, moduleRepository);
            //        }
            //        if (oPermission.AllowEdit)
            //        {
            //            View.InitializeModuleUploader(CommunityId);
            //        }
            //        View.AllowUpload = oPermission.AllowAddFiles;
            //        //  this.View.BackToDiary = CommunityID;
            //        View.SetBackToItemsUrl(CommunityId, oItem.Id);
            //        View.SetBackToItemUrl(CommunityId, oItem.Id);
            //        View.SetMultipleUploadUrl(oItem.Id);
            //        LoadItemFiles(oItem, oPermission);
            //    }
            //    else
            //    {
            //        this.View.ReturnToItemsList(CommunityId, oItem.Id);
            //    }


            //}

            #region"Add"
            #endregion

            #region"Manage"
            //public List<dtoAttachmentItem> GetProjectAttachments(long idProject)
            //    {
            //        List<dtoAttachmentItem> attachments = null;
            //        Boolean isInTransaction = Manager.IsInTransaction();
            //        try
            //        {
            //            if (!isInTransaction)
            //                Manager.BeginTransaction();
            //            Project project = Manager.Get<Project>(idProject);
            //            if (project !=null){
            //               
            //            }
            //            if (!isInTransaction)
            //                Manager.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            if (!isInTransaction)
            //                Manager.RollBack();
            //            attachments = null;
            //        }
            //        return attachments;
                     

            //        PmActivityPermission pPermissions = GetProjectPermission(idProject, UserContext.CurrentUserID)
            //    }
            //public List<dtoAttachmentItem> GetProjectAttachments(long idProject, ModuleProjectManagement mPermission)
            //{

            //}

                public List<dtoAttachmentItem> GetProjectAttachments(long idProject, long idActivity, Boolean getDeleted, String unknownUser, Boolean onlyAvailable = false ) { 
                    List<dtoAttachmentItem> attachments = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        litePerson person = Manager.Get<litePerson>(UC.CurrentUserID);
                        PmActivity activity = (idActivity>0) ? Manager.Get<PmActivity>(idActivity) : null;
                        Project project = (activity!= null && activity.Project !=null) ? activity.Project : Manager.Get<Project>(idProject);
                        if (project != null && person !=null && person.TypeID != (int) UserTypeStandard.Guest && (idActivity<=0 || (idActivity>0 && activity !=null)))
                        {
                            ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((person == null) ? (Int32)UserTypeStandard.Guest : person.TypeID) : new ModuleProjectManagement(Manager.GetModulePermission(person.Id, (!project.isPortal && project.Community != null) ? project.Community.Id : 0, GetIdModule()));
                            ProjectResource resource = project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && r.Type == ResourceType.Internal && r.Person == person).FirstOrDefault();
                            PmActivityPermission rolePermission = GetRolePermissions(AttachmentGetContainerRole(project,activity, resource, mPermission));


                            attachments = AttachmentsGet(person, project, activity, getDeleted, rolePermission, mPermission, unknownUser);
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        attachments = null;
                    }
                    return (onlyAvailable && attachments != null) ? attachments.Where(a=> a.Permissions.Download || a.Permissions.Play).ToList() : attachments; 
                }
                private ActivityRole AttachmentGetContainerRole(Project project,PmActivity activity,ProjectResource resource,ModuleProjectManagement mPermission){
                    ActivityRole role  = (resource!=null) ? resource.ProjectRole : ((!project.isPersonal && mPermission.Administration ) ? ActivityRole.Manager : ActivityRole.None);
                    if(resource!= null && activity != null && resource.Visibility == ProjectVisibility.InvolvedTasks && role != ActivityRole.Manager && role != ActivityRole.ProjectOwner){
                        if (activity.IsSummary)
                            role = (ResourceHasActivityAssignments(resource, activity) ? role : ActivityRole.None);
                        else
                            role = (activity.Assignments.Where(a=> a.Deleted== BaseStatusDeleted.None && a.Resource== resource).Any()) ? role: ActivityRole.None;
                    }     
                    return role;
                }
                private Boolean ResourceHasActivityAssignments(ProjectResource resource,PmActivity father ){
                    Boolean result = false;
                    foreach (PmActivity child in father.Children.Where(c => c.Deleted == BaseStatusDeleted.None))
                    {
                        if (child.IsSummary)
                            result = ResourceHasActivityAssignments(resource,child);
                        else
                            result =child.Assignments.Where(a=> a.Deleted== BaseStatusDeleted.None && a.Resource== resource).Any();
                        if (result)
                            return result;
                    }
                    return result;
                }
                private List<dtoAttachmentItem> AttachmentsGet(litePerson person, Project project, PmActivity activity, Boolean getDeleted, PmActivityPermission rolePermission, ModuleProjectManagement mPermission, String unknownUser)
                {
                    List<dtoAttachmentItem> items = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        if (activity != null || project !=null)
                        {
                            items = new List<dtoAttachmentItem>();
                            List<dtoAttachment> attachments = AttachmentsGet(project, activity, getDeleted, unknownUser);
                            ModuleRepository repositoryPermissions = GetRepositoryPermissions((project.isPortal) ? 0 : (project.Community == null) ? -1 : project.Community.Id);
                            items = attachments.Where(a => a.IdAttachment > 0).OrderBy(a => a.DisplayOrder).ThenBy(a => a.DisplayName).Select(a => new dtoAttachmentItem() { Attachment = a, Permissions = AttachmentGetPermissions(person, a, activity, rolePermission, mPermission, repositoryPermissions) }).ToList();
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        items = null;
                    }
                    return items;
                }
                private List<dtoAttachment> AttachmentsGet(Project project, PmActivity activity, Boolean getDeleted, String unknownUser)
                {
                    List<dtoAttachment> attachments = project.AttachmentLinks.Where(a => ((getDeleted && a.Deleted == BaseStatusDeleted.Manual) || (!getDeleted && a.Deleted == BaseStatusDeleted.None)) 
                                && a.Project==project && a.Activity== activity).OrderBy(a=> a.DisplayOrder).ThenBy(a=> a.Id).Select(a=>
                                    new dtoAttachment(a)).ToList();

                   Dictionary<Int32, String> users = GetUsers(attachments.Select(i => i.IdCreatedBy).Distinct().ToList(), unknownUser);
                   foreach (dtoAttachment item in attachments)
                   {
                       item.CreatedBy = users[item.IdCreatedBy];
                   }
                   return attachments;
                }

                private dtoAttachmentPermission AttachmentGetPermissions(litePerson person, dtoAttachment attachment, PmActivity activity, PmActivityPermission rolePermissions, ModuleProjectManagement mPermission,ModuleRepository repositoryPermissions)
                {
                    dtoAttachmentPermission result = new dtoAttachmentPermission();
                  
                    switch (attachment.Type){
                        case AttachmentType.file:
                            result.Download = (attachment.File != null && (attachment.File.IsDownloadable || attachment.File.Type== ItemType.Link )) && (HasPermission(rolePermissions,PmActivityPermission.ViewAttachments)|| HasPermission(rolePermissions,PmActivityPermission.DownloadAttacchments));
                            result.Play = (attachment.File != null && (attachment.File.Type== ItemType.Multimedia || attachment.File.Type == ItemType.ScormPackage || attachment.File.Type == ItemType.VideoStreaming)) && (HasPermission(rolePermissions, PmActivityPermission.ViewAttachments) || HasPermission(rolePermissions, PmActivityPermission.DownloadAttacchments));

                            switch (attachment.File.Type)
                            {
                                case ItemType.ScormPackage:
                                case ItemType.Multimedia:
                                    result.ViewMyStatistics = HasPermission(rolePermissions,PmActivityPermission.ViewAttachments);
                                    if (attachment.File.IsInternal || (repositoryPermissions.Administration || (repositoryPermissions.EditOthersFiles || (repositoryPermissions.EditMyFiles && attachment.File.IdOwner == person.Id))))
                                    {
                                        result.ViewOtherStatistics = HasPermission(rolePermissions, PmActivityPermission.ManageAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.IdCreatedBy == person.Id);
                                        result.SetMetadata = result.ViewOtherStatistics;
                                    }
                                    break;

                                case ItemType.Link:
                                    result.Play = result.Download;
                                    break;
                            }
                            result.Edit = false;
                            break;
                        case AttachmentType.url:
                            result.Download = HasPermission(rolePermissions,PmActivityPermission.ViewAttachments) || HasPermission(rolePermissions,PmActivityPermission.DownloadAttacchments);
                            result.Play = result.Download;
                            result.Edit = HasPermission(rolePermissions, PmActivityPermission.ManageAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.IdCreatedBy == person.Id);
                            break;
                    }

                    result.Delete = false;
                    result.UnDelete = (attachment.Deleted != BaseStatusDeleted.None) && (HasPermission(rolePermissions, PmActivityPermission.VirtualUnDeleteAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.IdCreatedBy == person.Id));
                    result.Unlink = (attachment.InSharing && attachment.Deleted == BaseStatusDeleted.None) && (HasPermission(rolePermissions, PmActivityPermission.VirtualDeleteAttachments) || HasPermission(rolePermissions, PmActivityPermission.ManageAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.CreatedBy != null && attachment.IdCreatedBy == person.Id));
                    result.VirtualDelete = (attachment.Deleted == BaseStatusDeleted.None) && (HasPermission(rolePermissions, PmActivityPermission.VirtualDeleteAttachments) || HasPermission(rolePermissions, PmActivityPermission.ManageAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.IdCreatedBy == person.Id));
                    return result;
                }

                public T GetItem<T>(long idItem)
                {
                    return Manager.Get<T>(idItem);
                }
                public dtoAttachment AttachmentGetByIdLink(long idAttachmentLink,String unknownUser) { 
                    dtoAttachment attachment = null;
                    try
                    {
                        ProjectAttachmentLink l = Manager.Get<ProjectAttachmentLink>(idAttachmentLink);
                        if (l != null && l.Attachment != null) {
                            List<Int32> idUsers = new List<int>();
                            idUsers.Add(l.Attachment.IdCreatedBy);
                            idUsers.Add(l.Attachment.IdModifiedBy);
                            attachment = new dtoAttachment(l, GetUsers(idUsers, unknownUser), unknownUser);
                        }
                    }
                    catch (Exception ex) { 
                    
                    }
                    return attachment;
                }
                public ProjectAttachment AttachmentSaveUrl(dtoUrl item)
                {
                    ProjectAttachment attachment = null;
                    try
                    {
                        Manager.BeginTransaction();
                        attachment = Manager.Get<ProjectAttachment>(item.Id);
                        Person person = Manager.GetPerson(UC.CurrentUserID);
                        if (attachment != null && attachment.Type== AttachmentType.url && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser )
                        {
                            attachment.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                            attachment.Url = item.Address;
                            attachment.UrlName = item.DisplayName;
                            Manager.SaveOrUpdate(attachment);
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                        attachment = null;
                    }
                    return attachment;
                }

        
                public Boolean AttachmentLinksSetVirtualDelete(List<long>idLinks, Boolean delete)
                {
                    Boolean result = false;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {

                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        Person person = Manager.GetPerson(UC.CurrentUserID);
                        List<ProjectAttachmentLink> links = null;
                        if (idLinks.Count > maxItemsForQuery)
                            links = (from a in Manager.GetIQ<ProjectAttachmentLink>()
                                     where ((delete && a.Deleted == BaseStatusDeleted.None) || (!delete && a.Deleted == BaseStatusDeleted.Manual))
                                     select a).ToList().Where(l => idLinks.Contains(l.Id)).ToList();
                        else
                            links = (from a in Manager.GetIQ<ProjectAttachmentLink>()
                                     where ((delete && a.Deleted == BaseStatusDeleted.None) || (!delete && a.Deleted == BaseStatusDeleted.Manual)) && idLinks.Contains(a.Id)
                                     select a).ToList();

                        if (person != null && links != null && links.Any())
                        {
                            DateTime date = DateTime.Now;
                            foreach (ProjectAttachmentLink link in links)
                            {
                                link.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                link.UpdateMetaInfo(person.Id);
                                if (!delete && link.Attachment !=null && link.Attachment.Deleted != BaseStatusDeleted.None){
                                    link.Attachment.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                    link.Attachment.Deleted= BaseStatusDeleted.None;
                                    Manager.SaveOrUpdate(link.Attachment);
                                }
                                else if (delete && link.Attachment !=null && link.Attachment.Deleted==  BaseStatusDeleted.None){
                                    if (link.Type == AttachmentLinkType.Owner) {
                                        link.Attachment.SetDeleteMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                        Manager.SaveOrUpdate(link.Attachment);
                                        foreach (ProjectAttachmentLink shared in link.Attachment.SharedItems.Where(s => s.Deleted == BaseStatusDeleted.None && s.Id != link.Id)) {
                                            shared.SetDeleteMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                            shared.Deleted = link.Deleted | BaseStatusDeleted.Cascade;
                                            Manager.SaveOrUpdate(shared);
                                        }
                                    }
                                }
                                Manager.SaveOrUpdate(link);
                            }
                        }
                        if (!isInTransaction)
                            Manager.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                        else
                            throw ex;
                    }
                    return result;
                }
            #endregion
            public List<ProjectAttachment> AttachmentsAddUrl(long idProject, long idActivity, List<dtoUrl> urls)
            {
                List<ProjectAttachment> attachments = null;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (!isInTransaction)
                        Manager.BeginTransaction();
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (urls != null && urls.Where(u=> !String.IsNullOrEmpty(u.Address)).Any() && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser )
                    {
                        PmActivity activity = (idActivity > 0) ? Manager.Get<PmActivity>(idActivity) : null;
                        Project project = (idActivity == 0 && idProject > 0) ? Manager.Get<Project>(idProject) : ((activity == null) ? null : activity.Project);
                        if (project != null && (idActivity == 0 || activity != null)) {
                            long dOrder = AttachmentsGetMaxDisplayOrder(project, activity);
                            DateTime date = DateTime.Now;
                            attachments = new List<ProjectAttachment>();
                            foreach (dtoUrl item in urls.Where(u => !String.IsNullOrEmpty(u.Address) && Uri.IsWellFormedUriString(u.Address, UriKind.RelativeOrAbsolute)))
                            {
                                ProjectAttachment attachment = new ProjectAttachment();
                                attachment.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                attachment.Activity = activity;
                                attachment.Description = "";
                                attachment.IsForProject = (activity==null);
                                attachment.Project = project;
                                attachment.Type = AttachmentType.url;
                                attachment.Url = item.Address;
                                attachment.UrlName = item.Name;
                                Manager.SaveOrUpdate(attachment);

                                ProjectAttachmentLink aLink = GenerateLink(attachment, dOrder++);
                                Manager.SaveOrUpdate(aLink);
                                attachment.SharedItems.Add(aLink);
                                attachments.Add(attachment);
                                if(idActivity==0)
                                    project.Attachments.Add(attachment);
                                project.AttachmentLinks.Add(aLink);
                            }
                        }
                    }
                    if (!isInTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    attachments = null;
                }
                return attachments;

            }

            public List<ProjectAttachment> AttachmentsAddFiles(long idProject, long idActivity, List<dtoModuleUploadedItem> files)
            {
                List<ProjectAttachment> attachments = null;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (!isInTransaction)
                        Manager.BeginTransaction();
                        PmActivity activity = (idActivity > 0) ? Manager.Get<PmActivity>(idProject) : null;
                        Project project = (idActivity == 0 && idProject > 0) ? Manager.Get<Project>(idProject) : ((activity == null) ? null : activity.Project);
                        attachments = AttachmentsAddFiles(project, activity, files);
                    if (!isInTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    attachments = null;
                }
                return attachments;

            }
            public List<ProjectAttachment> AttachmentsAddFiles(Project project, PmActivity activity, List<dtoModuleUploadedItem> items)
            {
                List<ProjectAttachment> attachments = null;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (!isInTransaction)
                        Manager.BeginTransaction();
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (items.Any() && project != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        long dOrder = AttachmentsGetMaxDisplayOrder(project, activity);
                        attachments = new List<ProjectAttachment>();
                        DateTime date = DateTime.Now;
                        foreach (dtoModuleUploadedItem item in items)
                        {
                            ProjectAttachment attachment = new ProjectAttachment();
                            attachment.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                            attachment.Activity = activity;
                            attachment.Description = "";
                            attachment.IsForProject = (activity == null);
                            attachment.Project = project;
                            attachment.Type = AttachmentType.file;
                            attachment.Item = item.ItemAdded;
                            Manager.SaveOrUpdate(attachment);
                            ProjectAttachmentLink aLink = GenerateLink(attachment, dOrder++);
                            Manager.SaveOrUpdate(aLink);
                            attachment.SharedItems.Add(aLink);
                            Manager.SaveOrUpdate(attachment);
                            ModuleLink link = new ModuleLink(item.Link.Description, item.Link.Permission, item.Link.Action);
                            link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, date);
                            link.DestinationItem = (ModuleObject)item.Link.ModuleObject;
                            link.AutoEvaluable = false;
                            if (attachment.IsForProject )
                                link.SourceItem = ModuleObject.CreateLongObject(project.Id, project, (int)ModuleProjectManagement.ObjectType.Project, (project.Community != null) ? project.Community.Id : 0, ModuleProjectManagement.UniqueCode, GetIdModule());
                            else
                                link.SourceItem = ModuleObject.CreateLongObject(activity.Id, activity, (int)ModuleProjectManagement.ObjectType.Task, (project.Community != null) ? project.Community.Id : 0, ModuleProjectManagement.UniqueCode, GetIdModule());
                            Manager.SaveOrUpdate(link);
                            attachment.Link = Manager.Get<liteModuleLink>(link.Id);
                            Manager.SaveOrUpdate(attachment);
                            attachments.Add(attachment);
                            if (activity== null)
                                project.Attachments.Add(attachment);
                            project.AttachmentLinks.Add(aLink);
                        }
                    }
                    if (!isInTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    attachments = null;
                }
                return attachments;

            }
            public List<ProjectAttachment> AttachmentsLinkFiles(long idProject, long idActivity, List<ModuleActionLink> links)
            {
                List<ProjectAttachment> attachments = null;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (!isInTransaction)
                        Manager.BeginTransaction();
                    PmActivity activity = (idActivity > 0) ? Manager.Get<PmActivity>(idProject) : null;
                    Project project = (idActivity == 0 && idProject > 0) ? Manager.Get<Project>(idProject) : ((activity == null) ? null : activity.Project);
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (links.Any() && project != null && (idActivity== 0 || activity !=null) && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        DateTime date = DateTime.Now;
                        long dOrder = AttachmentsGetMaxDisplayOrder(project, activity);
                        attachments = new List<ProjectAttachment>();
                        foreach (ModuleActionLink link in links)
                        {
                            ProjectAttachment attachment = (from a in project.Attachments.Where(a=> (idActivity == 0 && a.IsForProject ) || (!a.IsForProject && a.Activity== activity))
                                                            select a).ToList().Where(a => a.Item == (liteRepositoryItem)link.ModuleObject.ObjectOwner).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (attachment == null)
                            {
                                attachment = new ProjectAttachment();
                                attachment.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                attachment.Activity = activity;
                                attachment.Description = "";
                                attachment.IsForProject = (activity == null);
                                attachment.Project = project;
                                attachment.Type = AttachmentType.file;
                                attachment.Item = (liteRepositoryItem)link.ModuleObject.ObjectOwner;
                                attachment.Version = null;

                                Manager.SaveOrUpdate(attachment);
                                ProjectAttachmentLink aLink = GenerateLink(attachment, dOrder++);
                                Manager.SaveOrUpdate(aLink);
                                attachment.SharedItems.Add(aLink);
                                Manager.SaveOrUpdate(attachment);


                                ModuleLink mLink = new ModuleLink(link.Description, link.Permission, link.Action);
                                mLink.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, date);
                                mLink.DestinationItem = (ModuleObject)link.ModuleObject;
                                mLink.AutoEvaluable = false;
                                if (attachment.IsForProject)
                                    mLink.SourceItem = ModuleObject.CreateLongObject(project.Id, project, (int)ModuleProjectManagement.ObjectType.Project, (project.Community != null) ? project.Community.Id : 0, ModuleProjectManagement.UniqueCode, GetIdModule());
                                else
                                    mLink.SourceItem = ModuleObject.CreateLongObject(activity.Id, activity, (int)ModuleProjectManagement.ObjectType.Task, (project.Community != null) ? project.Community.Id : 0, ModuleProjectManagement.UniqueCode, GetIdModule());
                                Manager.SaveOrUpdate(mLink);
                                attachment.Link = Manager.Get<liteModuleLink>(mLink.Id);
                                Manager.SaveOrUpdate(attachment);
                                if (activity == null)
                                    project.Attachments.Add(attachment);
                                project.AttachmentLinks.Add(aLink);
                            }
                            else if (attachment.Deleted != BaseStatusDeleted.None)
                            {
                                attachment.RecoverMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                foreach (ProjectAttachmentLink l in attachment.SharedItems.Where(s => s.Type == AttachmentLinkType.Owner))
                                {
                                    l.DisplayOrder = dOrder++;
                                    l.RecoverMetaInfo(person.Id, attachment.ModifiedIpAddress, attachment.ModifiedProxyIpAddress, attachment.ModifiedOn);
                                }
                            }

                            attachments.Add(attachment);  
                        }
                    }
                    if (!isInTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    attachments = null;
                }
                return attachments;

            }
            public List<RepositoryItemLinkBase<long>> AttachmentsGetBaseLinkedFiles(long idProject, long idActivity, Boolean loadDeleted = false)
            {
                PmActivity activity = (idActivity > 0) ? Manager.Get<PmActivity>(idProject) : null;
                Project project = (idActivity == 0 && idProject > 0) ? Manager.Get<Project>(idProject) : ((activity == null) ? null : activity.Project);
                return AttachmentsGetBaseLinkedFiles(project, activity, loadDeleted);
            }
            public IList<RepositoryItemLinkBase<long>> AttachmentsGetBaseLinkedFiles(Project project, Boolean loadDeleted)
            {
                return AttachmentsGetBaseLinkedFiles(project, null, loadDeleted);
            }
            public List<RepositoryItemLinkBase<long>> AttachmentsGetBaseLinkedFiles( Project project, PmActivity activity, Boolean loadDeleted = false)
            {
                List<RepositoryItemLinkBase<long>> items = new List<RepositoryItemLinkBase<long>>();
                try
                {
                    List<ProjectAttachment> attachments = project.Attachments.Where(a => (loadDeleted || (!loadDeleted && a.Deleted == BaseStatusDeleted.None)) && a.Type == AttachmentType.file &&
                        ((activity == null && a.IsForProject) || (activity != null && !a.IsForProject && a.Activity == activity))).ToList();

                    attachments.ForEach(a => items.Add(new RepositoryItemLinkBase<long>()
                    {
                        Deleted = a.Deleted,
                        File = new RepositoryItemObject(a.Item),
                        IdObjectLink = a.Id,
                        AlwaysLastVersion =true,
                        IsVisible = (a.Deleted == BaseStatusDeleted.None && a.Item != null && a.Item.Deleted== BaseStatusDeleted.None && (a.Version== null || a.Version.Deleted== BaseStatusDeleted.None))
                    }));

                    if (activity != null)
                    {
                        (from s in Manager.GetIQ<ProjectAttachmentLink>()
                         where (loadDeleted || (!loadDeleted && s.Deleted == BaseStatusDeleted.None)) && s.Activity == activity && s.Attachment != null
                         select s).ToList().Where(s => s.Attachment.Activity != activity).ToList().ForEach(s => items.Add(new RepositoryItemLinkBase<long>()
                         {
                            Deleted = s.Deleted,
                            File = new RepositoryItemObject(s.Attachment.Item),
                            IdObjectLink = s.Id,
                            AlwaysLastVersion = true,
                            IsVisible = (s.Deleted == BaseStatusDeleted.None && s.Attachment.Item !=null  && s.Attachment.Item.Deleted== BaseStatusDeleted.None && (s.Attachment.Version== null || s.Attachment.Version.Deleted== BaseStatusDeleted.None))
                         }));
                    }
                }
                catch (Exception ex)
                {

                }
                return items;
            }

            public List<RepositoryItemLink<long>> AttachmentsGetLinkedFiles(String unknownUser,long idProject, long idActivity, Boolean loadDeleted = false)
            {
                PmActivity activity = (idActivity > 0) ? Manager.Get<PmActivity>(idProject) : null;
                Project project = (idActivity == 0 && idProject > 0) ? Manager.Get<Project>(idProject) : ((activity == null) ? null : activity.Project);
                return AttachmentsGetLinkedFiles(unknownUser,project, activity, loadDeleted);
            }
            public IList<RepositoryItemLink<long>> AttachmentsGetLinkedFiles(String unknownUser, Project project, Boolean loadDeleted)
            {
                return AttachmentsGetLinkedFiles(unknownUser,project, null, loadDeleted);
            }
            public List<RepositoryItemLink<long>> AttachmentsGetLinkedFiles(String unknownUser, Project project, PmActivity activity, Boolean loadDeleted = false)
            {
                List<RepositoryItemLink<long>> items = new List<RepositoryItemLink<long>>();
                try
                {
                    List<ProjectAttachment> attachments = project.Attachments.Where(a => (loadDeleted || (!loadDeleted && a.Deleted == BaseStatusDeleted.None)) && a.Type == AttachmentType.file &&
                        ((activity == null && a.IsForProject) || (activity!=null && !a.IsForProject && a.Activity==activity ))).ToList();

                    attachments.ForEach(a => items.Add(new RepositoryItemLink<long>()
                         {
                             IdCreatedBy = a.IdCreatedBy,
                             CreatedOn = a.CreatedOn,
                             Deleted = a.Deleted,
                             File = new RepositoryItemObject(a.Item),
                             IdObjectLink = a.Id,
                             IdStatus = 0,
                             Link = a.Link,
                             IdModifiedBy = a.IdModifiedBy,
                             ModifiedOn = a.ModifiedOn,
                             IdOwner = a.IdCreatedBy,
                             IsVisible = (a.Deleted == BaseStatusDeleted.None && a.Item != null && a.Item.Deleted == BaseStatusDeleted.None && (a.Version == null || a.Version.Deleted == BaseStatusDeleted.None))
                         }));

                    if (activity!=null) {
                        (from s in Manager.GetIQ<ProjectAttachmentLink>()
                           where (loadDeleted || (!loadDeleted && s.Deleted == BaseStatusDeleted.None)) && s.Activity==activity && s.Attachment!=null
                         select s).ToList().Where(s => s.Attachment.Activity != activity).ToList().ForEach(s => items.Add(new RepositoryItemLink<long>()
                         {
                             IdCreatedBy = s.IdCreatedBy,
                             CreatedOn = s.CreatedOn,
                             Deleted = s.Deleted,
                             File = new RepositoryItemObject(s.Attachment.Item),
                             IdObjectLink = s.Id,
                             AlwaysLastVersion = true,
                             IdStatus = 0,
                             Link = s.Attachment.Link,
                             IdModifiedBy = s.IdModifiedBy,
                             ModifiedOn = s.ModifiedOn,
                             IdOwner = s.IdCreatedBy,
                             IsVisible = (s.Deleted == BaseStatusDeleted.None && s.Attachment.Item != null && s.Attachment.Item.Deleted == BaseStatusDeleted.None && (s.Attachment.Version == null || s.Attachment.Version.Deleted == BaseStatusDeleted.None))
                         }));
                    }
                }
                catch(Exception ex){
                
                }
                Dictionary<Int32, String> users = GetUsers(items.Select(i => i.IdOwner).Distinct().ToList(), items.Select(i => i.IdModifiedBy).Distinct().ToList(), items.Select(i => i.IdCreatedBy).Distinct().ToList(), unknownUser);
                foreach (RepositoryItemLink<long> item in items)
                {
                    item.Owner = users[item.IdOwner];
                    item.ModifiedBy = users[item.IdModifiedBy];
                    item.CreatedBy = users[item.IdCreatedBy];
                }
                List<RepositoryItemLink<long>> orderedFiles;
                orderedFiles = (from f in items where f.File == null || f.Link == null orderby f.CreatedOn select f).ToList();
                orderedFiles.AddRange((from f in items where f.File != null && f.Link != null orderby f.File.DisplayName select f).ToList());
                return orderedFiles;
            }

            private Dictionary<Int32, String> GetUsers(List<Int32> idCreators, List<Int32> idOwners, List<Int32> idModifiers, String unknownUser)
            {
                List<Int32> idUsers = new List<Int32>();
                idUsers.AddRange(idCreators);
                idUsers.AddRange(idOwners);
                idUsers.AddRange(idModifiers);
                return GetUsers(idUsers.Distinct().ToList(), unknownUser);
            }
            private Dictionary<Int32, String> GetUsers(List<Int32> idUsers,String unknownUser)
            {
                List<litePerson> persons = Manager.GetLitePersons(idUsers);
                return idUsers.ToDictionary(i => i, i => persons.Where(p => p.Id == i).Select(p => p.SurnameAndName).DefaultIfEmpty(unknownUser).FirstOrDefault());
            }
            private ProjectAttachmentLink GenerateLink(ProjectAttachment attachment,long displayOrder,litePerson person = null, Project project =null, PmActivity activity = null) {
                ProjectAttachmentLink link = new ProjectAttachmentLink();
                if (person != null && project != null)
                {
                    link.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress);
                    link.Project = project;
                    link.Activity = activity;
                    link.Attachment = attachment;
                    link.DisplayOrder = displayOrder;
                    link.Type = AttachmentLinkType.Shared;
                    link.IsForProject = (activity == null);
                }
                else
                    link = ProjectAttachmentLink.CreateFromAttachment(attachment,displayOrder++);
                return link;
            }
            private long AttachmentsGetMaxDisplayOrder(Project project , PmActivity activity = null) { 
                long dOrder = 1;

                var query = (from s in Manager.GetIQ<ProjectAttachmentLink>()
                             where s.Deleted == BaseStatusDeleted.None && ((activity == null && s.IsForProject) || (!s.IsForProject && s.Activity == activity)) && s.Project == project
                             select s);
                foreach (ProjectAttachmentLink a in query.ToList().OrderBy(a => a.DisplayOrder).ThenBy(a => a.CreatedOn))
                {
                    a.DisplayOrder = dOrder++;
                }
                return dOrder;
            }

        #endregion
        
    }
}