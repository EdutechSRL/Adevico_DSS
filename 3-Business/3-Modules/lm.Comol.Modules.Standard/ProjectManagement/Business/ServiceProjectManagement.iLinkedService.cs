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
    public partial class ServiceProjectManagement : BaseCoreServices, iLinkedService
    {
        #region "Implemented"

        #endregion
        #region "IgnoreItems- Only for evaluation (Educational Pah)"
        public void SaveActionExecution(ModuleLink link, bool isStarted, bool isPassed, short Completion, bool isCompleted, short mark, int idUser, bool alreadyCompleted, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
        }
        public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
        }
        public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
                return new List<dtoItemEvaluation<long>>();
            }
            public dtoEvaluation EvaluateModuleLink(ModuleLink link, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
                return null;
            }
        #endregion
        public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, int idUser, int idRole, int idCommunity, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            Int32 idObjectType = 0;
            if (source.ServiceCode == ModuleProjectManagement.UniqueCode)
                idObjectType = source.ObjectTypeID;
            else if (destination.ServiceCode== ModuleProjectManagement.UniqueCode)
                idObjectType = destination.ObjectTypeID;

            switch (idObjectType) { 
                case (Int32)ModuleProjectManagement.ObjectType.Project:
                case (Int32)ModuleProjectManagement.ObjectType.Task:
                    return GetAllowedStandardActionForFile(idUser, source, destination);
            }
            return actions;
        }
        private List<StandardActionType> GetAllowedStandardActionForFile(int idUser, ModuleObject source, ModuleObject destination)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            litePerson person = Manager.Get<litePerson>(idUser);
            Project project = ((Int32)ModuleProjectManagement.ObjectType.Project == source.ObjectTypeID) ? Manager.Get<Project>(source.ObjectLongID) : null;
            PmActivity activity = ((Int32)ModuleProjectManagement.ObjectType.Task == source.ObjectTypeID) ? Manager.Get<PmActivity>(source.ObjectLongID) : null;
            if (project == null && activity != null)
                project = activity.Project;
            if (project != null)
            {
                ProjectAttachment attachment = AttachmentGet(project, activity, destination.ObjectLongID);
                if (attachment != null && attachment.Link != null && destination.ObjectLongID == attachment.Item.Id && destination.FQN == attachment.Item.GetType().FullName)
                {
                    dtoAttachmentPermission permissions = AttachmentGetPermissions(person, attachment, project, activity);
                    if (permissions.SetMetadata)
                        actions.Add(StandardActionType.EditMetadata);
                    if (permissions.Play)
                        actions.Add(StandardActionType.Play);
                    if (permissions.ViewMyStatistics)
                        actions.Add(StandardActionType.ViewPersonalStatistics);
                    if (permissions.ViewOtherStatistics)
                        actions.Add(StandardActionType.ViewAdvancedStatistics);
                }
            }
            return actions;
        }
        private ProjectAttachment AttachmentGet(Project project, PmActivity activity, long idItem)
        {
            ProjectAttachment attachment = null;
            try
            {
                if (activity != null)
                {
                    attachment = (from a in Manager.GetIQ<ProjectAttachment>()
                                  where a.Item != null && a.Project != null && a.Project.Id == project.Id
                                  && a.Activity != null && a.Activity.Id == activity.Id
                                  && a.Item != null && a.Item.Id == idItem
                                  select a).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                else
                    attachment = (from a in Manager.GetIQ<ProjectAttachment>()
                                  where a.Item !=null && a.Project != null && a.Project.Id == project.Id && a.Item.Id == idItem
                                  select a).Skip(0).Take(1).ToList().FirstOrDefault();
                
            }
            catch (Exception ex)
            {

            }
          
            return attachment;
        }
        private dtoAttachmentPermission AttachmentGetPermissions(litePerson person, ProjectAttachment attachment,Project project, PmActivity activity)
        {
            dtoAttachmentPermission result = new dtoAttachmentPermission();
            if (attachment != null && project != null) {
                lm.Comol.Core.FileRepository.Domain.ModuleRepository repositoryPermissions = GetRepositoryPermissions((project.isPortal) ? 0 : (project.Community == null) ? -1 : project.Community.Id, person.Id);
                ModuleProjectManagement mPermission = (project.isPortal) ? ModuleProjectManagement.CreatePortalmodule((person == null) ? (Int32)UserTypeStandard.Guest : person.TypeID) : new ModuleProjectManagement(Manager.GetModulePermission(person.Id, (!project.isPortal && project.Community != null) ? project.Community.Id : 0, GetIdModule()));
                ProjectResource resource = project.Resources.Where(r => r.Deleted == BaseStatusDeleted.None && r.Type == ResourceType.Internal && r.Person == person).FirstOrDefault();
                PmActivityPermission rolePermissions = GetRolePermissions(AttachmentGetContainerRole(project, activity, resource, mPermission));

                switch (attachment.Type)
                {
                    case AttachmentType.file:
                        result.Download = (attachment.Item != null && attachment.Item.IsDownloadable || attachment.Item.Type == ItemType.Link) && (HasPermission(rolePermissions, PmActivityPermission.ViewAttachments) || HasPermission(rolePermissions, PmActivityPermission.DownloadAttacchments));
                        result.Play = (attachment.Item != null && (attachment.Item.Type == ItemType.Multimedia || attachment.Item.Type == ItemType.ScormPackage || attachment.Item.Type == ItemType.VideoStreaming)) && (HasPermission(rolePermissions, PmActivityPermission.ViewAttachments) || HasPermission(rolePermissions, PmActivityPermission.DownloadAttacchments));

                        switch (attachment.Item.Type)
                        {
                            case ItemType.ScormPackage:
                            case ItemType.Multimedia:
                                result.ViewMyStatistics = HasPermission(rolePermissions, PmActivityPermission.ViewAttachments);
                                if (attachment.Item.IsInternal || (repositoryPermissions.Administration || (repositoryPermissions.EditOthersFiles || (repositoryPermissions.EditMyFiles && attachment.Item.IdOwner == person.Id))))
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
                        result.Download = HasPermission(rolePermissions, PmActivityPermission.ViewAttachments) || HasPermission(rolePermissions, PmActivityPermission.DownloadAttacchments);
                        result.Play = result.Download;
                        result.Edit = HasPermission(rolePermissions, PmActivityPermission.ManageAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.IdCreatedBy == person.Id);
                        break;
                }

                result.Delete = false;
                result.UnDelete = (attachment.Deleted != BaseStatusDeleted.None) && (HasPermission(rolePermissions, PmActivityPermission.VirtualUnDeleteAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.IdCreatedBy == person.Id));
                result.Unlink = (attachment.SharedItems.Any() && activity != null && attachment.SharedItems.Where(s => s.Deleted == BaseStatusDeleted.None && s.Activity != null && s.Activity.Id == activity.Id && s.Type == AttachmentLinkType.Shared).Any() && attachment.Deleted == BaseStatusDeleted.None) && (HasPermission(rolePermissions, PmActivityPermission.VirtualDeleteAttachments) || HasPermission(rolePermissions, PmActivityPermission.ManageAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.IdCreatedBy == person.Id));
                result.VirtualDelete = (attachment.Deleted == BaseStatusDeleted.None) && (HasPermission(rolePermissions, PmActivityPermission.VirtualDeleteAttachments) || HasPermission(rolePermissions, PmActivityPermission.ManageAttachments) || (HasPermission(rolePermissions, PmActivityPermission.ManageActivityAttachments) && attachment.IdCreatedBy == person.Id));
            }

            return result;
        }

        public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, int idUser, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            List<StandardActionType> actions = GetAllowedStandardActionForFile(idUser, source, destination);
            return actions.Contains(actionType);
        }

        public bool AllowActionExecution(ModuleLink link, int idUser, int idCommunity, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            litePerson person = Manager.Get<litePerson>(idUser);
            switch (link.SourceItem.ObjectTypeID)
            {
                case (Int32)ModuleProjectManagement.ObjectType.Project:
                case (Int32)ModuleProjectManagement.ObjectType.Task:
                    switch (link.DestinationItem.ServiceCode) { 
                        case  lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode:
                             ProjectAttachment attachment = AttachmentGet(link);
                             if (attachment != null && attachment.Project != null && attachment.Link != null && link.DestinationItem.ObjectLongID == attachment.Item.Id && link.DestinationItem.FQN == attachment.Item.GetType().FullName)
                            {
                                dtoAttachmentPermission permissions = AttachmentGetPermissions(person, attachment, attachment.Project, attachment.Activity);
                                return permissions.Play || permissions.Download;
                            }
                            break;
                        default:
                            return false;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }
        private ProjectAttachment AttachmentGet(ModuleLink link)
        {
            ProjectAttachment attachment = (from a in Manager.GetIQ<ProjectAttachment>()
                                                where a.Link!=null && link.Id == a.Link.Id  select a).Skip(0).Take(1).ToList().FirstOrDefault();
            return attachment;
        }
        #region "IgnoreItems- Delete Community"
            public void PhisicalDeleteCommunity(Int32 idCommunity, Int32 idUser, String baseFilePath, String baseThumbnailPath)
            {
            }
            public void PhisicalDeleteRepositoryItem(long idFileItem, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
            }
        #endregion
        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, int objectTypeId, Dictionary<int, string> translations, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
            {
                return null;
            }
    }
    //    public bool AllowActionExecution(ModuleLink Link, int UserID, int CommunityID, int RoleID)
    //    {
    //        Person person = Manager.GetPerson(UserID);
    //        switch (Link.SourceItem.ObjectTypeID)
    //        {
    //            case (int)ModuleTasklist.ObjectType.Project:
    //                return AllowViewProject(UserID, CommunityID, RoleID);
    //            case (int)ModuleTasklist.ObjectType.Task:
    //                return AllowViewTask(Link.SourceItem.ObjectLongID, UserID, CommunityID, RoleID);
    //            case (int)ModuleTasklist.ObjectType.TaskFile:
    //                return AllowDownloadFileOfItemDescription(Link.SourceItem.ObjectLongID, UserID, CommunityID, RoleID);
    //            case (int)ModuleTasklist.ObjectType.TaskLinkedFile:
    //                return AllowDownloadFileLinkedToItem(Link.SourceItem.ObjectLongID, UserID, CommunityID, RoleID);
    //            default:
    //                return false;
    //        }
    //    }
    //    private Boolean AllowViewProject(int UserID, int CommunityID, int RoleID)
    //    {
    //        Boolean iResponse = false;
    //        long permission = (long)ModuleTasklist.Base2Permission.ViewCommunityProjects | (long)ModuleTasklist.Base2Permission.Administration;
    //        iResponse = Manager.HasModulePermission(UserID, RoleID, CommunityID, this.ServiceModuleID(), permission);
    //        return iResponse;
    //    }
    //    private Boolean AllowViewTask(long itemId, int UserID, int communityID, int RoleID)
    //    {
    //        Boolean iResponse = false;
    //        Task item = GetTask(itemId);
    //        Person person = Manager.GetPerson(UserID);
    //        if (item != null)
    //        {
    //            communityID = item.Community == null ? 0 : item.Community.Id;
    //            lm.Comol.Modules.TaskList.ModuleTasklist modulePermission = ServicePermission(UserID, communityID);
    //            ModuleRepository moduleRepository = GetModuleRepository(UserID, communityID);
    //            CoreItemPermission itemPermission = GetTaskPermission(person, item, modulePermission, moduleRepository);
    //            iResponse = (itemPermission.AllowView);
    //        }
    //        return iResponse;
    //    }
    //    private Boolean AllowDownloadFileOfItemDescription(long itemLinkId, int UserID, int CommunityID, int RoleID)
    //    {
    //        return AllowViewTask(itemLinkId, UserID, CommunityID, RoleID);
    //    }
    //    private Boolean AllowDownloadFileLinkedToItem(long itemFileLinkId, int UserID, int communityID, int RoleID)
    //    {
    //        Boolean iResponse = false;
    //        TaskListFile taskFile = Manager.Get<TaskListFile>(itemFileLinkId);
    //        Person person = Manager.GetPerson(UserID);
    //        if (taskFile != null && taskFile.TaskOwner != null && taskFile.File != null && taskFile.Link != null)
    //        {
    //            Task task = taskFile.TaskOwner;
    //            communityID = task.Community == null ? 0 : task.Community.Id;
    //            lm.Comol.Modules.TaskList.ModuleTasklist modulePermission = ServicePermission(UserID, communityID);
    //            ModuleRepository moduleRepository = GetModuleRepository(UserID, communityID);
    //            CoreItemPermission itemPermission = GetTaskPermission(person, task, modulePermission, moduleRepository);

    //            //permission.Download = itemFileLink.File.IsDownloadable && itemPermissions.AllowViewFiles;
    //            //permission.Play = (itemFileLink.File.isSCORM || itemFileLink.File.isVideocast) && itemPermissions.AllowViewFiles;

    //            iResponse = AllowViewFileFromLink(modulePermission, itemPermission, taskFile, person);
    //        }
    //        return iResponse;
    //    }

    //    private Boolean AllowViewFileFromLink(ModuleTasklist modulePermission, CoreItemPermission itemPermission, TaskListFile taskFile, Person person)
    //    {
    //        Boolean iResponse = false;
    //        iResponse = itemPermission.AllowViewFiles && (taskFile.isVisible || taskFile.Owner == person || taskFile.TaskOwner.MetaInfo.CreatedBy == person || modulePermission.Administration);
    //        return iResponse;
    //    }


    //    public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(int IdCommunity, int IdUser, long objectId, int objectTypeId, Dictionary<int, string> translations)
    //    {
    //        StatTreeNode<StatFileTreeLeaf> node = null;
    //        Person person = Manager.Get<Person>(IdUser);

    //        switch (objectTypeId)
    //        {
    //            case (int)ModuleTasklist.ObjectType.Task:
    //                Task item = Manager.Get<Task>(objectId);
    //                if (item != null)
    //                    IdCommunity = (item.Community == null) ? 0 : item.Community.Id;
    //                break;
    //            case (int)ModuleTasklist.ObjectType.TaskFile:
    //                //item = Manager.Get<CommunityEventItem>(objectId);
    //                //if (item != null)
    //                //    IdCommunity = (item.CommunityOwner == null) ? 0 : item.CommunityOwner.Id;
    //                break;
    //            case (int)ModuleTasklist.ObjectType.TaskLinkedFile:
    //                TaskListFile taskFile = Manager.Get<TaskListFile>(objectTypeId);
    //                if (taskFile != null)
    //                    IdCommunity = (taskFile.CommunityOwner == null) ? 0 : taskFile.CommunityOwner.Id;
    //                break;
    //        }
    //        //ModuleTasklist moduleTasklist = ServicePermission(IdUser, IdCommunity);
    //        //if (moduleTasklist.Administration || moduleTasklist.ViewTaskList || moduleTasklist.UploadFile) //moduleTasklist.Edit ||
    //        //{
    //        //    ModuleRepository repository = GetModuleRepository(IdUser, IdCommunity);
    //        //    switch (objectTypeId)
    //        //    {
    //        //        case (int)ModuleTasklist.ObjectType.Task:
    //        //            Task item = Manager.Get<Task>(objectId);
    //        //            node = LoadDiaryItemForStatistics(item, person, moduleTasklist, repository, translations);
    //        //            break;
    //        //        case (int)ModuleTasklist.ObjectType.TaskLinkedFile:
    //        //            TaskListFile eventItemFile = Manager.Get<TaskListFile>(objectTypeId);
    //        //            if (eventItemFile != null && eventItemFile.ItemOwner != null)
    //        //                node = LoadDiaryItemForStatistics(eventItemFile.ItemOwner, person, moduleTasklist, repository, translations);
    //        //            break;
    //        //        default:

    //        //            node = LoadDiaryForStatistics(Manager.Get<Community>(IdCommunity), person, moduleTasklist, repository, translations);
    //        //            break;
    //        //    }
    //        //}
    //        //else
    //        //    node = CreateTaskListTreeNode(Manager.Get<Community>(IdCommunity), translations); 
    //        return node;
    //    }

    //    #region "Load Files for statistics"
    //    //private StatTreeNode<StatFileTreeLeaf> LoadDiaryForStatistics(Community community, Person person, ModuleTasklist moduleDiary, ModuleRepository repository, Dictionary<int, string> translations)
    //    //    {
    //    //        StatTreeNode<StatFileTreeLeaf> rootNode = CreateDiaryTreeNode(community, translations);
    //    //        List<dtoTask> items = GetDtoTasksForStatistics(community, person, moduleDiary,repository,moduleDiary.Administration);
    //    //        foreach (dtoTask item in items)
    //    //        {
    //    //            StatTreeNode<StatFileTreeLeaf> itemNode = CreateDiaryItemTreeNode(item.EventItem,item.LessonNumber,translations);
    //    //            itemNode.Leaves = (from i in item.FileLinks where i.ItemFileLink !=null && i.ItemFileLink.File!=null select CreateStatFileTreeLeaf(i.ItemFileLink.File, i.Permission.ViewPersonalStatistics, i.Permission.ViewStatistics)).ToList();
    //    //            rootNode.Nodes.Add(itemNode);
    //    //        }
    //    //        return rootNode;
    //    //    }
    //    //private StatTreeNode<StatFileTreeLeaf> LoadDiaryItemForStatistics(CommunityEventItem item, Person person, ModuleTasklist moduleDiary, ModuleRepository repository, Dictionary<int, string> translations)
    //    //    {
    //    //        StatTreeNode<StatFileTreeLeaf> rootNode = CreateDiaryItemTreeNode(item,-1, translations);
    //    //        int lesson = -1;
    //    //        dtoDiaryItem dtoItem = CreateDtoTaskForStatistics(person,item,moduleDiary.Administration,moduleDiary,repository,ref lesson);
    //    //        rootNode.Leaves = (from i in dtoItem.FileLinks where i.ItemFileLink !=null && i.ItemFileLink.File!=null select CreateStatFileTreeLeaf(i.ItemFileLink.File, i.Permission.ViewPersonalStatistics, i.Permission.ViewStatistics)).ToList();
    //    //        return rootNode;
    //    //    }
    //    private StatTreeNode<StatFileTreeLeaf> CreateTaskListTreeNode(Community community, Dictionary<int, string> translations)
    //    {
    //        StatTreeNode<StatFileTreeLeaf> node = new StatTreeNode<StatFileTreeLeaf>() { Id = 0, isVisible = true, NodeObjectTypeId = (int)ModuleTasklist.ObjectType.Project };
    //        node.Name = (community == null) ? translations[(int)TreeItemsTranslations.PortalDiaryName] : string.Format(translations[(int)TreeItemsTranslations.DiaryName], community.Name);
    //        node.ToolTip = (community == null) ? translations[(int)TreeItemsTranslations.PortalDiaryNameToolTip] : string.Format(translations[(int)TreeItemsTranslations.DiaryNameToolTip], community.Name);

    //        return node;
    //    }
    //    private StatTreeNode<StatFileTreeLeaf> CreateTaskTreeNode(CommunityEventItem item, int lessonNumber, Dictionary<int, string> translations)
    //    {
    //        StatTreeNode<StatFileTreeLeaf> node = new StatTreeNode<StatFileTreeLeaf>() { Id = 0, isVisible = true, NodeObjectTypeId = (int)ModuleTasklist.ObjectType.Task };

    //        node.Name = (item.ShowDateInfo) ?
    //                    string.Format(translations[(int)TreeItemsTranslations.StandardDiaryItemName], (lessonNumber < 1) ? "" : lessonNumber.ToString())
    //                    :
    //                    string.Format(translations[(int)TreeItemsTranslations.NoDateDiaryItemName], (lessonNumber < 1) ? "" : lessonNumber.ToString());
    //        node.ToolTip = (item.ShowDateInfo) ?
    //                    string.Format(translations[(int)TreeItemsTranslations.StandardDiaryItemNameToolTip], (lessonNumber < 1) ? "" : lessonNumber.ToString(), item.StartDate.ToString("dd/MM/YY"), item.StartDate.ToString("hh:mm"), item.EndDate.ToString("hh:mm"))
    //                    :
    //                    string.Format(translations[(int)TreeItemsTranslations.NoDateDiaryItemNameToolTip], (lessonNumber < 1) ? "" : lessonNumber.ToString());

    //        return node;
    //    }
    //    private StatFileTreeLeaf CreateStatFileTreeLeaf(BaseCommunityFile item, Boolean viewPersonal, Boolean viewAdvanced)
    //    {
    //        StatFileTreeLeaf leaf = new StatFileTreeLeaf()
    //        {
    //            Id = item.Id,
    //            Extension = item.Extension,
    //            isVisible = item.isVisible,
    //            LinkId = 0,
    //            Name = item.DisplayName,
    //            ToolTip = "",
    //            isScorm = item.isSCORM,
    //            UniqueID = item.UniqueID,
    //            DownloadCount = item.Downloads,
    //            Type = (viewPersonal) ? StatTreeLeafType.Personal : StatTreeLeafType.None
    //        };
    //        if ((viewAdvanced))
    //            leaf.Type = (leaf.Type == StatTreeLeafType.None) ? StatTreeLeafType.Advanced : leaf.Type | StatTreeLeafType.Advanced;
    //        return leaf;
    //    }
    //    //private List<dtoTask> GetDtoTasksForStatistics(Community community, Person person, ModuleTasklist module, ModuleRepository repository, Boolean allVisibleItems)
    //    //{
    //    //    List<dtoTask> items = new List<dtoTask>();
    //    //    ////CommunityEventType eventType = GetDiaryEventType();
    //    //    //if (community != null && person != null)
    //    //    //{
    //    //    //    int lessonNumber = 1;
    //    //    //    items = (from item in CommunityTasksQuery(community, person, allVisibleItems).ToList()
    //    //    //             select CreateDtoTaskForStatistics(person, item, allVisibleItems, module, repository, ref lessonNumber)).ToList();
    //    //    //}
    //    //    return items;
    //    //}



    //    #endregion
    //    #endregion
    //}
}