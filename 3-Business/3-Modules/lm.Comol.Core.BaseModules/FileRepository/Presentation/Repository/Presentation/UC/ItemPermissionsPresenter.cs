using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class ItemPermissionsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.InLineTags.Business.ServiceInLineTags _ServiceInLineTags;
            private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewItemPermissions View
            {
                get { return (IViewItemPermissions)base.View; }
            }
            private ServiceRepository Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceRepository(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleRepository.UniqueCode);
                    return currentIdModule;
                }
            }
            public ItemPermissionsPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ItemPermissionsPresenter(iApplicationContext oContext, IViewItemPermissions view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean editMode, dtoDisplayRepositoryItem item,String unknownUser, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
        {
            if (!SessionTimeout())
            {
                if (editMode)
                    InitializeSelectors(item);
                LoadAssignments(editMode, item.Id, tTranslations, translations, pTranslations);
            }
        }

        private void InitializeSelectors(dtoDisplayRepositoryItem item)
        {
            Boolean allowAddRole = item.Permissions.EditPermission && item.Repository.Type == RepositoryType.Community;
            Boolean allowAddUsers = item.Permissions.EditPermission;
            if (allowAddRole)
            {
                List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> roles = Service.AssignmentNotAssignedRoles(item.Id, item.Repository);
                allowAddRole = (roles != null && roles.Any());
                View.InitializeRolesSelector(roles);
            }
            if (allowAddUsers)
            {
                switch (item.Repository.Type)
                {
                    case RepositoryType.Community:
                        View.InitializeUsersSelector(item.Repository.IdCommunity, Service.AssignmentAssignedUsersId(item.Id, item.Repository));
                        break;
                    case RepositoryType.Portal:
                        View.InitializePortalUsersSelector(Service.AssignmentAssignedUsersId(item.Id, item.Repository));
                        break;
                    default:
                        allowAddUsers = false;
                        break;
                }
            }
            View.AllowUpload = item.AllowUpload;
            View.InitializeCommands(allowAddRole,  allowAddUsers);
        }
        private void InitializeSelectors(RepositoryIdentifier identifier, List<Int32> removeRoles,List<Int32> removeUsers)
        {
            Boolean allowAddRole = identifier.Type == RepositoryType.Community;
            Boolean allowAddUsers = true;
            if (allowAddRole)
            {
                List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> roles = CurrentManager.GetTranslatedRoles(identifier.IdCommunity, UserContext.Language.Id).Where(r => !removeRoles.Contains(r.Id)).ToList();
                allowAddRole = (roles != null && roles.Any());
                View.InitializeRolesSelector(roles);
            }

            if (allowAddUsers)
            {
                switch (identifier.Type)
                {
                    case RepositoryType.Community:
                        View.InitializeUsersSelector(identifier.IdCommunity, removeUsers);
                        break;
                    case RepositoryType.Portal:
                        View.InitializePortalUsersSelector(removeUsers);
                        break;
                    default:
                        allowAddUsers = false;
                        break;
                }
            }
            View.InitializeCommands(allowAddRole,  allowAddUsers);
        }

        public void LoadAssignments(Boolean editMode, long idItem, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations, List<dtoEditAssignment> assignments = null)
        {
            RepositoryIdentifier identifier = Service.ItemGetRepositoryIdentifier(idItem);
            if (identifier!=null){
                List<dtoEditAssignment> items = Service.ItemGetAssignmentsForEditing(idItem, tTranslations, translations, pTranslations);
                if (assignments != null && items!=null)
                {
                    foreach (dtoEditAssignment i in items.Where(x=> !x.Inherited && assignments.Any(a=> a.Id== x.Id && a.Type== x.Type  && a.Denyed != x.Denyed)))
                    {
                        i.Denyed = assignments.Any(a => a.Id == i.Id && a.Denyed);
                    }

                    foreach (dtoEditAssignment nItem in items.Where(i => !i.Inherited && !assignments.Any(a => a.Id == i.Id))){
                        switch (nItem.Type)
                        {
                            case AssignmentType.community:
                                dtoEditAssignment vItem = assignments.Where(a => a.Type== nItem.Type).FirstOrDefault();
                                if (vItem != null && !vItem.Denyed && nItem.Denyed)
                                    nItem.Denyed = false;
                                assignments = assignments.Where(a=> a.Type!= AssignmentType.community).ToList();
                                break;
                            case AssignmentType.person:
                                dtoEditAssignment pItem = assignments.Where(a => a.Type == nItem.Type && a.IdPerson == nItem.IdPerson).FirstOrDefault();
                                if (pItem!=null && !pItem.Denyed && nItem.Denyed)
                                    nItem.Denyed = false;
                                assignments = assignments.Where(a => a.Type != nItem.Type && a.IdPerson != nItem.IdPerson).ToList();
                                break;
                            case AssignmentType.role:
                                dtoEditAssignment rItem = assignments.Where(a => a.Type== nItem.Type && a.IdRole==nItem.IdRole).FirstOrDefault();
                                if (rItem!=null && !rItem.Denyed && nItem.Denyed)
                                    nItem.Denyed = false;
                                assignments = assignments.Where(a => a.Type != nItem.Type && a.IdRole != nItem.IdRole).ToList();
                                break;
                        }
                    }

                    assignments.AddRange(items.Where(i => !assignments.Any(a => a.Id == i.Id)).ToList());
                    assignments = assignments.OrderBy(a => a.OrderByInherited()).ThenBy(a => a.OrderByType()).ThenBy(a => a.DisplayName).ToList();
                    InitializeSelectors(identifier, assignments.Select(i => i.IdRole).ToList(), assignments.Where(a=> a.IdPerson>0).Select(i => i.IdPerson).ToList());
                    View.LoadAssignments(assignments);
                }
                else if (items!=null)
                {
                    InitializeSelectors(identifier, items.Select(i => i.IdRole).ToList(), items.Where(a => a.IdPerson > 0).Select(i => i.IdPerson).ToList());
                    View.LoadAssignments(items.OrderBy(a => a.OrderByInherited()).ThenBy(a => a.OrderByType()).ThenBy(a => a.DisplayName).ToList());
                }
            }
        }

        public void AddRoles(Boolean editMode, long idItem, List<dtoTranslatedRoleType> items, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations, List<dtoEditAssignment> assignments)
        {
            if (!SessionTimeout()){
                liteRepositoryItem item  = Service.ItemGet(idItem);
                if (item!=null){
                    DateTime date = DateTime.Now;
                    long permissions = (long)((item.AllowUpload) ? ModuleRepository.Base2Permission.DownloadOrPlay | ModuleRepository.Base2Permission.Upload : ModuleRepository.Base2Permission.DownloadOrPlay);
                    assignments.Where(a => !a.Denyed && a.Permissions == 0).ToList().ForEach(a => a.Permissions = permissions);

                    items = items.Where(i => !assignments.Any(a => a.IdRole == i.Id)).ToList();
                    View.HasPendingChanges = items.Any();
                    assignments.AddRange(items.Select(i => new dtoEditAssignment() { CreatedOn=date, IdCommunity = item.Repository.IdCommunity, IdRole = i.Id, DisplayName = i.Name, Denyed = false, Permissions = permissions, Type= AssignmentType.role }));

                    LoadAssignments(editMode, idItem, tTranslations, translations, pTranslations, assignments);
                }
            }
        }
        public void AddUsers(Boolean editMode, long idItem, List<Int32> idUsers, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations, List<dtoEditAssignment> assignments)
        {
            if (!SessionTimeout())
            {
                liteRepositoryItem item = Service.ItemGet(idItem);
                if (item != null)
                {
                    DateTime date = DateTime.Now;
                    long permissions = (long)((item.AllowUpload) ? ModuleRepository.Base2Permission.DownloadOrPlay | ModuleRepository.Base2Permission.Upload : ModuleRepository.Base2Permission.DownloadOrPlay );
                    assignments.Where(a => !a.Denyed && a.Permissions == 0).ToList().ForEach(a => a.Permissions = permissions);

                    idUsers = idUsers.Where(i => !assignments.Any(a => a.IdPerson == i)).ToList();
                    View.HasPendingChanges = idUsers.Any();

                    List<litePerson> persons = CurrentManager.GetLitePersons(idUsers);

                    assignments.AddRange(persons.Select(i => new dtoEditAssignment() { CreatedOn = date, IdCommunity = item.Repository.IdCommunity, IdPerson = i.Id, DisplayName = i.SurnameAndName, Denyed = false, Permissions = permissions, Type = AssignmentType.person }));

                    LoadAssignments(editMode, idItem, tTranslations, translations, pTranslations, assignments.Where(a => !a.IsDeleted).ToList());
                }
            }
        }
        public void RemoveItem(long idItem, dtoEditAssignment assignment, List<dtoEditAssignment> assignments)
        {
            if (!SessionTimeout())
            {
                liteRepositoryItem item = Service.ItemGet(idItem);
                if (item != null)
                {
                    long permissions = (long)((item.AllowUpload) ? ModuleRepository.Base2Permission.DownloadOrPlay | ModuleRepository.Base2Permission.Upload : ModuleRepository.Base2Permission.DownloadOrPlay);
                    assignments.Where(a => !a.Denyed && a.Permissions == 0).ToList().ForEach(a => a.Permissions = permissions);
                    switch(assignment.Type){
                        case AssignmentType.community:
                            if (!assignments.Any(a => a.IdPerson == UserContext.CurrentUserID))
                                assignments.Add(new dtoEditAssignment() { Id = 0, IdPerson = UserContext.CurrentUserID, CreatedOn = DateTime.Now, Permissions = permissions, Type = AssignmentType.person });
                            break;
                        case AssignmentType.person:
                            break;
                        case AssignmentType.role:
                            break;
                    }
                    View.HasPendingChanges = true;
                }
                assignments = assignments.Where(a => !a.Equals(assignment)).ToList();
                InitializeSelectors(item.Repository, assignments.Where(a => !a.IsDeleted && a.IdRole > 0).Select(a => a.IdRole).ToList(), assignments.Where(a => !a.IsDeleted && a.IdPerson > 0).Select(a => a.IdPerson).ToList());
            }
        }
        public void SaveAssignments(long idItem, List<dtoEditAssignment> assignments, Boolean applyToContent, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
        {
            if (!SessionTimeout())
            {
                liteRepositoryItem rItem = Service.ItemGet(idItem);
                Int32 idCommunity = UserContext.CurrentCommunityID;
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.VersionUnableToAdd;

                if (rItem != null)
                {
                    oType = ModuleRepository.GetObjectType(rItem.Type);
                    ModuleRepository module = Service.GetPermissions(rItem.Repository, UserContext.CurrentUserID);
                    Boolean reloadItems = false;
                    idCommunity = rItem.Repository.IdCommunity;
                    dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idItem, UserContext.CurrentUserID, rItem.Repository, View.GetUnknownUserName());
                    if (dItem == null)
                    {
                        View.DisplayUserMessage(UserMessageType.detailsNoPermissionToSave);
                        uAction = ModuleRepository.ActionType.UnknownItemFound;
                    }
                    else
                    {
                        oType = ModuleRepository.GetObjectType(dItem.Type);
                        applyToContent = applyToContent && dItem.Type == ItemType.Folder;
                        if (!dItem.Permissions.EditPermission)
                        {
                            View.DisplayUserMessage(UserMessageType.permissionsNoPermissionToSave);
                            uAction = ModuleRepository.ActionType.UnavailableItem;
                        }
                        else if (!assignments.Any(a=> !a.Denyed))
                        {
                            View.DisplayUserMessage(UserMessageType.permissionsNoItemToSave);
                            uAction = ModuleRepository.ActionType.PermissionsNothingToSave;
                        }
                        else
                        {
                            long permissions = (long)((rItem.AllowUpload) ? ModuleRepository.Base2Permission.DownloadOrPlay | ModuleRepository.Base2Permission.Upload : ModuleRepository.Base2Permission.DownloadOrPlay);
                            assignments.Where(a => !a.Denyed && a.Permissions == 0).ToList().ForEach(a => a.Permissions = permissions);

                            reloadItems = Service.AssignmentsAddToItem(idItem, assignments.Where(a=> !a.IsDeleted).ToList(), applyToContent);
                            View.HasPendingChanges = !reloadItems;
                            uAction = (reloadItems ? ModuleRepository.ActionType.PermissionsSaved : ModuleRepository.ActionType.PermissionsNotSaved);
                            View.DisplayUserMessage((reloadItems? UserMessageType.permissionsSaved : UserMessageType.permissionsUnableToSave));
                        }
                    }
                    if (reloadItems)
                    {
                        InitializeSelectors(dItem);
                        LoadAssignments(true, dItem.Id, tTranslations, translations, pTranslations);
                    }
                }
                View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
            }
        }
        public void ReloadPermissions(long idItem, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
        {
            if (!SessionTimeout())
            {
                LoadAssignments(true, idItem, tTranslations, translations, pTranslations);
                View.HasPendingChanges = false;
                View.DisplayUserMessage(UserMessageType.permissionsReloaded);
            }
        }

        public void TryToSave(long idItem, List<dtoEditAssignment> assignments, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
        {
            if (!SessionTimeout())
            {
                liteRepositoryItem rItem = Service.ItemGet(idItem);
                Int32 idCommunity = UserContext.CurrentCommunityID;
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.VersionUnableToAdd;

                if (rItem != null)
                {
                    ModuleRepository module = Service.GetPermissions(rItem.Repository, UserContext.CurrentUserID);
                    idCommunity = rItem.Repository.IdCommunity;
                    dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idItem, UserContext.CurrentUserID, rItem.Repository, View.GetUnknownUserName());
                    if (dItem == null)
                    {
                        View.DisplayUserMessage(UserMessageType.detailsNoPermissionToSave);
                        oType = ModuleRepository.GetObjectType(rItem.Type);
                        uAction = ModuleRepository.ActionType.UnknownItemFound;
                    }
                    else if (rItem.Type == ItemType.Folder)
                        View.AskUserForApply(rItem.DisplayName);
                    else
                        SaveAssignments(idItem, assignments,false, tTranslations, translations, pTranslations);
                }
                else
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
            }
        }
        public void UpdateForUpload(long idItem, Boolean allowUpload, List<dtoEditAssignment> assignments, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
        {
            if (!SessionTimeout())
            {
                liteRepositoryItem rItem = Service.ItemGet(idItem);
                Int32 idCommunity = UserContext.CurrentCommunityID;
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.VersionUnableToAdd;
                View.AllowUpload = allowUpload;
                if (rItem != null)
                {
                    ModuleRepository module = Service.GetPermissions(rItem.Repository, UserContext.CurrentUserID);
                    idCommunity = rItem.Repository.IdCommunity;

                    long permissions = (long)((rItem.AllowUpload) ? ModuleRepository.Base2Permission.DownloadOrPlay | ModuleRepository.Base2Permission.Upload : ModuleRepository.Base2Permission.DownloadOrPlay);
                    assignments.Where(a => !a.Denyed && a.Permissions == 0).ToList().ForEach(a => a.Permissions = permissions);

                    Boolean reloadItems = Service.AssignmentsAddToItem(idItem, assignments.Where(a => !a.IsDeleted).ToList(), false);
                    View.HasPendingChanges = !reloadItems;
                    uAction = (reloadItems ? ModuleRepository.ActionType.PermissionsSaved : ModuleRepository.ActionType.PermissionsNotSaved);
                    if (reloadItems)
                        LoadAssignments(true, idItem, tTranslations, translations, pTranslations);
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
                }
                else
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
            }
        }
        private Boolean SessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return true;
            }
            else
                return false;
        }
    }
}