using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository 
    {
        
        #region "Permissions"
            private Int32 _idModule;
            private Dictionary<Int32, String> _modulesCode;
            public Int32 GetIdModule()
            {
                if (_idModule < 1)
                    _idModule = Manager.GetModuleID(ModuleRepository.UniqueCode);
                return _idModule;
            }
            public String GetModuleCode(Int32 idModule)
            {
                if (_modulesCode == null)
                    _modulesCode = new Dictionary<int, string>();
                if (_modulesCode.ContainsKey(idModule))
                    return _modulesCode[idModule];
                else
                {
                    if (idModule == _idModule)
                        _modulesCode.Add(idModule, ModuleRepository.UniqueCode);
                    else
                        _modulesCode.Add(idModule, Manager.GetModuleCode(idModule));
                    return _modulesCode[idModule];
                }
            }
            public ModuleRepository GetPermissions(RepositoryIdentifier identifier, Int32 idCurrentPerson) {
                return GetPermissions(identifier.Type, identifier.IdCommunity,  idCurrentPerson);
            }
            public ModuleRepository GetPermissions(RepositoryType type, Int32 idRepositoryCommunity, Int32 idCurrentPerson )
            {
                litePerson person = null;
                idCurrentPerson = (idCurrentPerson <= 0) ? UC.CurrentUserID : idCurrentPerson;
                switch (type)
                {
                    case RepositoryType.Portal:
                        person = Manager.GetLitePerson(idCurrentPerson);
                        return ModuleRepository.CreatePortalmodule((person == null || person.Id == 0) ? (int)UserTypeStandard.Guest : person.TypeID);
                    case RepositoryType.Community:
                        return new ModuleRepository(Manager.GetModulePermission(idCurrentPerson, idRepositoryCommunity, GetIdModule()));
                    default:
                        return new ModuleRepository();
                }
            }

            public Int32 GetIdAnonymousUser()
            {
                return Manager.GetIdUnknownUser();
            }

            #region "Assignments"
                #region "Get assignments for editing"
                    public List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> AssignmentNotAssignedRoles(long idItem){
                        return AssignmentNotAssignedRoles(idItem, ItemGetRepositoryIdentifier(idItem));
                    }
                    public List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> AssignmentNotAssignedRoles(long idItem, RepositoryIdentifier identifier){
                        if (identifier != null)
                        {
                            List<Int32> idRoles = GetQueryAssignments(identifier).Where(a=> a.IdItem == idItem && a.Type == AssignmentType.role && a.Deleted == BaseStatusDeleted.None).Select(a=> a.IdRole).ToList();

                            return Manager.GetTranslatedRoles(identifier.IdCommunity, UC.Language.Id).Where(t=> !idRoles.Contains(t.Id)).OrderBy(t=> t.Name).ToList();
                        }
                        else
                            return new List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType>();
                    }
                    public List<Int32> AssignmentAssignedUsersId(long idItem, RepositoryIdentifier identifier)
                    {
                        return GetQueryAssignments(identifier).Where(a => a.IdItem == idItem && a.Type == AssignmentType.person && a.Deleted == BaseStatusDeleted.None).Select(a => a.IdPerson).ToList();
                    }
                    public List<dtoEditAssignment> ItemGetAssignmentsForEditing(long idItem, Dictionary<AssignmentType, String> tTranslations, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
                    {
                        List<dtoEditAssignment> assignments = new List<dtoEditAssignment>();
                        try
                        {
                            RepositoryItem item = Manager.Get<RepositoryItem>(idItem);
                            if (item != null)
                            {
                                Manager.Refresh(item);
                                assignments = (from a in Manager.GetIQ<ItemAssignments>() where a.Deleted == BaseStatusDeleted.None && a.IdItem == idItem select a).ToList().Select(a => dtoEditAssignment.Create(a)).ToList();

                                Boolean allowUpload = item.AllowUpload;
                                Int32 idLanguage = UC.Language.Id;
                                List<dtoTranslatedRoleType> tRoles = (assignments.Any(a => a.Type == AssignmentType.role) ? Manager.GetTranslatedRoles(idLanguage) : null);
                                if (item.IdFolder == 0)
                                    assignments = assignments.Where(a => !a.Inherited).ToList();
                                else
                                {
                                    List<dtoEditAssignment> inherited = assignments.Where(a => a.Inherited).ToList();
                                    if (inherited.Any())
                                    {
                                        inherited = inherited.Where(i => !assignments.Any(a => !a.Inherited && a.Type == i.Type && a.Denyed == i.Denyed && a.IdCommunity == i.IdCommunity && a.IdRole == i.IdRole && a.IdPerson == i.IdPerson )).ToList();
                                        assignments = assignments.Where(a => !a.Inherited || (a.Inherited && inherited.Any(i=> i.Id == a.Id))).ToList();
                                    }
                                }
                                if (tRoles != null && tRoles.Any())
                                    SetRoles(allowUpload, tRoles.ToDictionary(r => r.Id, r => r.Name), assignments, translations, pTranslations);
                                if (assignments.Any(a => a.Type == AssignmentType.community))
                                    SetCommunity(allowUpload, assignments, translations, pTranslations);
                                else if (item.Repository.Type== RepositoryType.Community)
                                {
                                    assignments.Add(new dtoEditAssignment() {IsAutoAdded = true, Denyed = true, IdCommunity = item.Repository.IdCommunity, Type = AssignmentType.community, Permissions = 0 });
                                    SetCommunity(allowUpload, assignments, translations, pTranslations);
                                }

                                if (assignments.Any(a => a.IdPerson > 0))
                                    SetPerson(allowUpload, assignments.Where(a => a.IdPerson > 0).ToList(), translations, pTranslations);
                                assignments = assignments.OrderBy(a => a.OrderByInherited()).ThenBy(a => a.OrderByDenyed()).ThenBy(a => a.OrderByType()).ThenBy(a => a.DisplayName).ToList(); 
                            }
                        }
                        catch (Exception ex)
                        {
                            assignments = null;
                        }
                        return assignments;
                    }
                    #region "Set Assignments Translations"
                        private void SetRoles(Boolean allowUpload, Dictionary<Int32, String> roles, List<dtoEditAssignment> assignments, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
                        {
                            foreach (dtoEditAssignment a in assignments.Where(a => a.Type == AssignmentType.role))
                            {
                                a.DisplayName = (roles.ContainsKey(a.IdRole) ? roles[a.IdRole] : "");
                                a.SetPermissionsTranslation(TranslatePermissions(allowUpload, a, translations, pTranslations));
                            }
                        }
                        private void SetPerson(Boolean allowUpload, List<dtoEditAssignment> assignments, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
                        {
                            List<litePerson> persons = Manager.GetLitePersons(assignments.Where(a => a.IdPerson > 0).Select(a => a.IdPerson).ToList());
                            foreach (dtoEditAssignment a in assignments.Where(a => a.Type == AssignmentType.person))
                            {
                                if (persons.Any(p => p.Id == a.IdPerson))
                                    a.DisplayName = persons.Where(p => p.Id == a.IdPerson).Select(p => p.SurnameAndName).FirstOrDefault();
                                else
                                    a.DisplayName = translations[PermissionsTranslation.UnknownUser];
                                a.SetPermissionsTranslation(TranslatePermissions(allowUpload, a, translations, pTranslations));
                            }
                        }
                        private void SetCommunity(Boolean allowUpload, List<dtoEditAssignment> assignments, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
                        {
                            foreach (dtoEditAssignment a in assignments.Where(a => a.Type == AssignmentType.community))
                            {
                                a.DisplayName = translations[PermissionsTranslation.CommunityPermission];
                                a.SetPermissionsTranslation(TranslatePermissions(allowUpload, a, translations, pTranslations));
                            }
                        }
                        private String TranslatePermissions(Boolean allowUpload, dtoEditAssignment assignment, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
                        {
                            String result = (assignment.Inherited ? translations[PermissionsTranslation.InheritedPermission] : "")
                                   + ((assignment.Denyed && assignment.Permissions > 0) ? translations[PermissionsTranslation.DenyedPermission] : "")
                                   + TranslatePermissions(allowUpload, assignment.Permissions, translations, pTranslations);
                            return result;
                        }
                        private String TranslatePermissions(Boolean allowUpload, long permissions, Dictionary<PermissionsTranslation, String> translations, Dictionary<ModuleRepository.Base2Permission, String> pTranslations)
                        {
                            String result = "";
                            if (permissions <= 0)
                                return translations[PermissionsTranslation.NoPermission];
                            if (PermissionHelper.CheckPermissionSoft((long)ModuleRepository.Base2Permission.DownloadOrPlay, permissions))
                            {
                                result = pTranslations[ModuleRepository.Base2Permission.DownloadOrPlay];
                            }
                            if (allowUpload && PermissionHelper.CheckPermissionSoft((long)ModuleRepository.Base2Permission.Upload, permissions))
                            {
                                if (!String.IsNullOrWhiteSpace(result))
                                    result += ", ";
                                result = pTranslations[ModuleRepository.Base2Permission.Upload];
                            }
                            return result;
                        }
                    #endregion
                #endregion
                
                #region "Set assignments for item"
                    public Boolean AssignmentsAddToItem(long idItem, List<dtoEditAssignment> items, Boolean applyToContent){
                        Boolean added = false;
                        try
                        {
                            liteRepositoryItem item = Manager.Get<liteRepositoryItem>(idItem);
                            litePerson person = GetValidPerson(UC.CurrentUserID);
                            if (item != null && person !=null)
                            {
                                DateTime date = DateTime.Now;
                                applyToContent = applyToContent && item.Type == ItemType.Folder;
                                Manager.BeginTransaction();
                                List<ItemAssignments> allAssignments = GetQueryAssignments(item.Repository).ToList();
                                #region "Set assignment to item"
                                List<long> idAssignments = items.Where(i=> i.Id>0).Select(i=> i.Id).Distinct().ToList();
                                List<ItemAssignments> itemAssignments = (from a in Manager.GetIQ<ItemAssignments>() where a.IdItem == idItem && !a.Inherited  select a).ToList();
                                foreach(ItemAssignments assignment in itemAssignments.Where(a=> idAssignments.Contains(a.Id))){
                                    dtoEditAssignment dto = items.Where(i=> i.Id== assignment.Id).FirstOrDefault();
                                    assignment.UpdateMetaInfo(person,UC.IpAddress, UC.ProxyIpAddress,date);
                                    assignment.Deleted= BaseStatusDeleted.None;
                                    assignment.Denyed=dto.Denyed;
                                    assignment.Permissions=dto.Permissions;
                                }
                                foreach(ItemAssignments assignment in itemAssignments.Where(a=> !idAssignments.Contains(a.Id))){
                                    assignment.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress,date);
                                }
                                foreach(dtoEditAssignment dto in items.Where(a=> a.Id==0 && !a.IsAutoAdded || (a.IsAutoAdded && !a.Denyed))){
                                    ItemAssignments assignment = itemAssignments.Where(i=> i.Type== dto.Type && i.IdCommunity == dto.IdCommunity 
                                                                            && i.IdPerson== dto.IdPerson
                                                                            && i.IdRole == dto.IdRole).FirstOrDefault();
                                    if (assignment==null){
                                        assignment = new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date) { IdItem = idItem, Type = dto.Type, Inherited = false, Denyed = dto.Denyed, Permissions = dto.Permissions, Repository = item.Repository };
                                        switch (dto.Type)
                                        {
                                            case AssignmentType.community:
                                                assignment.IdCommunity = item.Repository.IdCommunity;
                                                break;
                                            case AssignmentType.role:
                                                assignment.IdCommunity = item.Repository.IdCommunity;
                                                assignment.IdRole = dto.IdRole;
                                                break;
                                            case AssignmentType.person:
                                                assignment.IdPerson = dto.IdPerson;
                                                break;
                                        }
                                        itemAssignments.Add(assignment);
                                    }
                                    else if (assignment.Deleted != BaseStatusDeleted.None ){
                                        assignment.UpdateMetaInfo(person,UC.IpAddress, UC.ProxyIpAddress,date);
                                        assignment.Deleted= BaseStatusDeleted.None;
                                        assignment.Denyed=dto.Denyed;
                                        assignment.Permissions=dto.Permissions;
                                    }
                                }
                                if (itemAssignments.Where(a => a.Deleted != BaseStatusDeleted.None).Count() > 20)
                                    Manager.DeletePhysicalList(itemAssignments.Where(a => a.Deleted != BaseStatusDeleted.None).ToList());
                                Manager.SaveOrUpdateList(itemAssignments);

                                Manager.DeletePhysicalList(allAssignments.Where(a => a.Inherited && a.IdItem ==idItem).ToList());
                                allAssignments = allAssignments.Where(a => !(a.Inherited && a.IdItem == idItem)).ToList();
                                List<ItemAssignments> inheritedAssignments = AssignmentsCreateInherited(person, date, item, itemAssignments.Where(i => i.Deleted == BaseStatusDeleted.None).ToList());
                                if (inheritedAssignments.Any())
                                    Manager.SaveOrUpdateList(inheritedAssignments);
                                if (applyToContent)
                                    AssignmentsApplyTocontent(person, date, item.Id, itemAssignments.Where(i => i.Deleted == BaseStatusDeleted.None).ToList(), allAssignments, inheritedAssignments);
                                else if (item.Type== ItemType.Folder)
                                {
                                    AssignmentsApplyInheritedToContent(person, date, item.Id, inheritedAssignments, allAssignments);
                                }
                                Manager.Commit();

                                added = true;
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(item.Repository));
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(item.Repository));
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(item.Repository));
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            added = false;
                            Manager.RollBack();
                        }
                        return added;
                    }

                    private List<ItemAssignments> AssignmentsCreateInherited(litePerson person, DateTime date, liteRepositoryItem item, List<ItemAssignments> currentAssignments)
                    {
                        List<ItemAssignments> assignments = new List<ItemAssignments>();
                        if (item.IdFolder == 0)
                        {
                            currentAssignments.ForEach(a => assignments.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                               {
                                   Denyed = a.Denyed,
                                   IdItem = item.Id,
                                   IdCommunity = a.IdCommunity,
                                   IdPerson = a.IdPerson,
                                   IdRole = a.IdRole,
                                   Inherited = true,
                                   Permissions = a.Permissions,
                                   Repository = a.Repository,
                                   Type = a.Type
                               }));
                        }
                        else
                        {
                            List<ItemAssignments> fAssignments = (from a in Manager.GetIQ<ItemAssignments>() where a.IdItem == item.IdFolder && a.Deleted== BaseStatusDeleted.None && a.Inherited select a).ToList();
                            List<ItemAssignments> notEquals = fAssignments.Where(fa => !currentAssignments.Any(c => c.isSettingsEqual(fa))).ToList();
                            List<ItemAssignments> equals = fAssignments.Where(fa => !notEquals.Any(e=> e.Id==fa.Id)).ToList();

                            equals.ForEach(a => assignments.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                            {
                                Denyed = a.Denyed,
                                IdItem = item.Id,
                                IdCommunity = a.IdCommunity,
                                IdPerson = a.IdPerson,
                                IdRole = a.IdRole,
                                Inherited = true,
                                Permissions = a.Permissions,
                                Repository = a.Repository,
                                Type = a.Type
                            }));
                            notEquals.Where(e=> e.Denyed).ToList().ForEach(a => assignments.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                            {
                                Denyed = a.Denyed,
                                IdItem = item.Id,
                                IdCommunity = a.IdCommunity,
                                IdPerson = a.IdPerson,
                                IdRole = a.IdRole,
                                Inherited = true,
                                Permissions = a.Permissions,
                                Repository = a.Repository,
                                Type = a.Type
                            }));
                            List<ItemAssignments> others = currentAssignments.Where(c => !assignments.Any(a=> a.isSettingsEqual(c,true))).ToList();

                            others.ForEach(a => assignments.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                            {
                                Denyed = a.Denyed,
                                IdItem = item.Id,
                                IdCommunity = a.IdCommunity,
                                IdPerson = a.IdPerson,
                                IdRole = a.IdRole,
                                Inherited = true,
                                Permissions = a.Permissions,
                                Repository = a.Repository,
                                Type = a.Type
                            }));
                        }

                        return assignments;
                    }
                    private void AssignmentsApplyTocontent(litePerson person, DateTime date, long idFather, List<ItemAssignments> assignmentsToApply, List<ItemAssignments> allAssignments,List<ItemAssignments> inheritedAssignments )
                    {
                        try
                        {
                            List<ItemAssignments> toAdd = new List<ItemAssignments>();
                            List<liteRepositoryItem> items = (from i in Manager.GetIQ<liteRepositoryItem>() where i.IdFolder == idFather select i).ToList();
                            Manager.DeletePhysicalList(allAssignments.Where(a => items.Any(i => i.Id == a.IdItem)).ToList());
                            foreach (liteRepositoryItem item in items)
                            {
                                assignmentsToApply.ForEach(a => toAdd.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                                {
                                    Denyed = a.Denyed,
                                    IdItem = item.Id,
                                    IdCommunity = a.IdCommunity,
                                    IdPerson = a.IdPerson,
                                    IdRole = a.IdRole,
                                    Inherited = false,
                                    Permissions = a.Permissions,
                                    Repository = a.Repository,
                                    Type = a.Type
                                }));

                                inheritedAssignments.ForEach(a => toAdd.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                                {
                                    Denyed = a.Denyed,
                                    IdItem = item.Id,
                                    IdCommunity = a.IdCommunity,
                                    IdPerson = a.IdPerson,
                                    IdRole = a.IdRole,
                                    Inherited = true,
                                    Permissions = a.Permissions,
                                    Repository = a.Repository,
                                    Type = a.Type
                                }));
                                
                                if (!item.IsFile)
                                    AssignmentsApplyTocontent(person, date, item.Id, assignmentsToApply, allAssignments, inheritedAssignments);
                            }
                            if (toAdd.Any())
                                Manager.SaveOrUpdateList(toAdd);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    private void AssignmentsApplyInheritedToContent(litePerson person, DateTime date, long idFather, List<ItemAssignments> fatherInheritedAssignments, List<ItemAssignments> allAssignments)
                    {
                        try
                        {
                            List<ItemAssignments> toAdd = new List<ItemAssignments>();
                            List<RepositoryItem> items = (from i in Manager.GetIQ<RepositoryItem>() where i.IdFolder == idFather select i).ToList();

                            Manager.DeletePhysicalList(allAssignments.Where(a => a.Inherited && items.Any(i => i.Id == a.IdItem)).ToList());


                            foreach (RepositoryItem item in items)
                            {
                                List<ItemAssignments> currentAssignments = allAssignments.Where(a => a.IdItem == item.Id && a.Deleted == BaseStatusDeleted.None && !a.Inherited).ToList();
                                List<ItemAssignments> notEquals = fatherInheritedAssignments.Where(fa => !currentAssignments.Any(c => c.isSettingsEqual(fa))).ToList();
                                List<ItemAssignments> equals = fatherInheritedAssignments.Where(fa => !notEquals.Any(e => e.Id == fa.Id)).ToList();

                                equals.ForEach(a => toAdd.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                                {
                                    Denyed = a.Denyed,
                                    IdItem = item.Id,
                                    IdCommunity = a.IdCommunity,
                                    IdPerson = a.IdPerson,
                                    IdRole = a.IdRole,
                                    Inherited = true,
                                    Permissions = a.Permissions,
                                    Repository = a.Repository,
                                    Type = a.Type
                                }));
                                notEquals.Where(e => e.Denyed).ToList().ForEach(a => toAdd.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                                {
                                    Denyed = a.Denyed,
                                    IdItem = item.Id,
                                    IdCommunity = a.IdCommunity,
                                    IdPerson = a.IdPerson,
                                    IdRole = a.IdRole,
                                    Inherited = true,
                                    Permissions = a.Permissions,
                                    Repository = a.Repository,
                                    Type = a.Type
                                }));
                                List<ItemAssignments> others = currentAssignments.Where(c => !toAdd.Any(a => a.isSettingsEqual(c, true))).ToList();

                                others.ForEach(a => toAdd.Add(new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date)
                                {
                                    Denyed = a.Denyed,
                                    IdItem = item.Id,
                                    IdCommunity = a.IdCommunity,
                                    IdPerson = a.IdPerson,
                                    IdRole = a.IdRole,
                                    Inherited = true,
                                    Permissions = a.Permissions,
                                    Repository = a.Repository,
                                    Type = a.Type
                                }));


                                if (toAdd.Any())
                                    Manager.SaveOrUpdateList(toAdd);
                                if (!item.IsFile)
                                    AssignmentsApplyInheritedToContent(person, date, item.Id, toAdd, allAssignments);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                #endregion
                public List<dtoDisplayAssignment> GetAssignments(long idItem)
                {
                    List<dtoDisplayAssignment> items = new List<dtoDisplayAssignment>();
                    try
                    {
                        items = (from a in Manager.GetIQ<liteItemAssignments>() where a.Deleted == BaseStatusDeleted.None && a.IdItem == idItem select a).ToList().Select(a => dtoDisplayAssignment.Create(a)).ToList();
                    }
                    catch (Exception ex)
                    {
                        items = null;
                    }
                    return items;
                }
                public List<liteItemAssignments> GetAssignments(List<long> idRepositoryItems)
                {
                    List<liteItemAssignments> assignments = new List<liteItemAssignments>();
                    try
                    {
                        if (idRepositoryItems.Count <= maxItemsForQuery)
                            assignments = (from a in Manager.GetIQ<liteItemAssignments>() where a.Deleted== BaseStatusDeleted.None && idRepositoryItems.Contains(a.IdItem) select a).ToList();
                        else
                        {
                            Int32 pageIndex = 0;
                            List<long> idPagedItems = idRepositoryItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (idPagedItems.Any())
                            {
                                assignments.AddRange((from a in Manager.GetIQ<liteItemAssignments>() where a.Deleted == BaseStatusDeleted.None && idPagedItems.Contains(a.IdItem) select a).ToList());
                                pageIndex++;
                                idPagedItems = idRepositoryItems.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        assignments = null;
                    }
                    return assignments;
                }

                private Boolean HasValidAssignments(long idItem, Int32 idCurrentUser, Int32 idRole, List<liteItemAssignments> assignments)
                {
                    return HasValidAssignments(idItem, idCurrentUser, idRole, Manager.GetIdProfileType(idCurrentUser), assignments);
                }
                private Boolean HasValidAssignments(long idItem, Int32 idCurrentUser, Int32 idRole, Int32 idProfileType, List<liteItemAssignments> assignments)
                {
                    Boolean result = false;
                    List<liteItemAssignments> aAssignments = GetAvailableAssignments(idItem, idCurrentUser, idRole, idProfileType, assignments);
                    Dictionary<AssignmentType, Boolean> permissions = aAssignments.Where(a => a.Deleted == BaseStatusDeleted.None).GroupBy(a => a.Type).ToDictionary(i => i.Key, i => i.OrderByDescending(x => x.Denyed).Select(x => !x.Denyed).FirstOrDefault());
                    if (permissions.ContainsKey(AssignmentType.person))
                        result = permissions[AssignmentType.person];
                    else if (permissions.ContainsKey(AssignmentType.role))
                        result = permissions[AssignmentType.role];
                    else if (permissions.ContainsKey(AssignmentType.community))
                        result = permissions[AssignmentType.community];

                    return result;
                }

                private List<liteItemAssignments> GetAvailableAssignments(long idItem, Int32 idCurrentUser, Int32 idRole, Int32 idProfileType, List<liteItemAssignments> assignments)
                {
                    List<liteItemAssignments> aAssignments = null;

                    aAssignments = assignments.Where(a => a.IdItem == idItem && a.Type == AssignmentType.community && a.Inherited ).ToList();
                    aAssignments.AddRange(assignments.Where(a => a.IdItem == idItem && a.Type == AssignmentType.role && a.IdRole == idRole && a.Inherited).ToList());
                    aAssignments.AddRange(assignments.Where(a => a.IdItem == idItem && a.Type == AssignmentType.person && a.IdPerson == idCurrentUser && a.Inherited).ToList());

                    return aAssignments;
                }

                public ItemAssignments AssignmentAddCommunity(long idItem, RepositoryIdentifier identifier, Int32 idCommunity, Boolean denyed, Boolean inherited, long permissions, litePerson person, DateTime? date = null)
                {
                    ItemAssignments assignment = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        List<ItemAssignments> assignments = AssignmentCreateForCommunity(idItem, identifier,idCommunity, denyed, inherited, permissions, person, date);
                        if (assignments.Any())
                            Manager.SaveOrUpdateList(assignments);
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                    }
                    return assignment;
                }

                public ItemAssignments AssignmentAddPerson(long idItem, RepositoryIdentifier identifier, Int32 idPerson, Boolean denyed, Boolean inherited, long permissions, litePerson person, DateTime? date = null)
                {
                    ItemAssignments assignment = null;
                    Boolean isInTransaction = Manager.IsInTransaction();
                    try
                    {
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        List<ItemAssignments> assignments = AssignmentCreateForUsers(idItem, identifier, new List<Int32>() { idPerson }, denyed, inherited, permissions, person, date);
                        if (assignments.Any())
                            Manager.SaveOrUpdateList(assignments);
                        if (!isInTransaction)
                            Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!isInTransaction)
                            Manager.RollBack();
                    }
                    return assignment;
                }
                private List<ItemAssignments> AssignmentCreateForCommunity(long idItem, RepositoryIdentifier identifier, Int32 idCommunity, Boolean denyed, Boolean inherited, long permissions, litePerson person, DateTime? date = null)
                {
                    return AssignmentCreateFor(idItem,identifier, AssignmentType.community, new List<Int32> {idCommunity}, denyed, inherited, permissions, person, date, idCommunity); ;
                }
                private List<ItemAssignments> AssignmentCreateForRoles(long idItem, RepositoryIdentifier identifier, Int32 idCommunity, List<Int32> roles, Boolean denyed, Boolean inherited, long permissions, litePerson person, DateTime? date = null)
                {
                    return AssignmentCreateFor(idItem, identifier, AssignmentType.role, roles, denyed, inherited, permissions, person, date, idCommunity);
                }
                private List<ItemAssignments> AssignmentCreateForUsers(long idItem, RepositoryIdentifier identifier, List<Int32> users, Boolean denyed, Boolean inherited, long permissions, litePerson person, DateTime? date = null)
                {
                    return AssignmentCreateFor(idItem, identifier, AssignmentType.person, users, denyed, inherited, permissions, person, date);
                }
                private List<ItemAssignments> AssignmentCreateFor(long idItem,RepositoryIdentifier identifier,AssignmentType type,List<Int32> items,Boolean denyed,Boolean inherited,long permissions, litePerson person, DateTime? date=null,Int32 idCommunity=-1){
                    List<ItemAssignments> results = new List<ItemAssignments>();
                    foreach (Int32 id in items)
                    {
                        ItemAssignments assignment = new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date) { IdItem = idItem, Type = type, Inherited = inherited, Denyed = denyed, Permissions = permissions, Repository = identifier };
                        switch (type)
                        {
                            case AssignmentType.community:
                                assignment.IdCommunity = idCommunity;
                                break;
                            case AssignmentType.role:
                                assignment.IdCommunity = idCommunity;
                                assignment.IdRole = id;
                                break;
                            case AssignmentType.person:
                                assignment.IdPerson = id;
                                break;
                        }
                        results.Add(assignment);
                    }
                    return results;
                }
                private IEnumerable<ItemAssignments> GetQueryAssignments(RepositoryType type, Int32 idCommunity, Int32 idPerson = -1)
                {
                    var query = (from a in Manager.GetIQ<ItemAssignments>()
                                 where a.Repository != null && a.Repository.Type == type
                                 select a);
                    switch (type)
                    {
                        case RepositoryType.Community:
                            query = query.Where(a => idCommunity > 0 && a.IdCommunity == idCommunity);
                            break;
                        case RepositoryType.Portal:
                            query = query.Where(a => a.IdCommunity == 0);
                            break;
                    }
                    return query;
                }
                protected IEnumerable<ItemAssignments> GetQueryAssignments(RepositoryIdentifier identifier)
                {
                    var query = (from a in Manager.GetIQ<ItemAssignments>()
                                 where a.Repository != null && a.Repository.Type == identifier.Type
                                 select a);
                    switch (identifier.Type)
                    {
                        case RepositoryType.Community:
                            query = query.Where(a => identifier.IdCommunity > 0 && a.Repository.IdCommunity == identifier.IdCommunity);
                            break;
                    }
                    return query;
                }


                private void ApplyInheritedAssignment(RepositoryItem item, IEnumerable<ItemAssignments> fAssignments, long permissions, litePerson person, DateTime? date = null)
                {
                    try
                    {
                        Manager.DeletePhysicalList(GetQuery<ItemAssignments>().Where(a=> a.IdItem==item.Id && a.Inherited));

                        List<ItemAssignments> realAssigned = GetQuery<ItemAssignments>().Where(a => a.IdItem == item.Id && !a.Inherited).ToList();

                        Boolean itemAllowForAll = realAssigned.Any(a => a.Type == AssignmentType.community && !a.Denyed);
                        Boolean fatherAllowForAll = fAssignments.Any(a => a.Type == AssignmentType.community && !a.Denyed);

                        if (itemAllowForAll && fatherAllowForAll)
                            AssignmentAddCommunity(item.Id,item.Repository, item.IdCommunity,false, true, permissions, person, date);
                        else
                        {
                            if (!itemAllowForAll)
                            {
                                if (fatherAllowForAll)
                                    AddAssignmentsFromOthers(item.Id,item.Repository, realAssigned, true, permissions, person, date);
                                else
                                {
                                    List<ItemAssignments> sourceAssignments = new List<ItemAssignments>();
                                    sourceAssignments.AddRange(fAssignments.Where(a => a.Type == AssignmentType.role && a.Deleted == BaseStatusDeleted.None));
                                    sourceAssignments.AddRange(fAssignments.Where(a => a.Type == AssignmentType.person && a.Deleted == BaseStatusDeleted.None));
                                    AddAssignmentsFromOthers(item.Id, item.Repository, sourceAssignments, true, permissions, person, date);
                                }
                            }
                            else
                                AddAssignmentsFromOthers(item.Id, item.Repository, fAssignments.Where(a => a.Deleted == BaseStatusDeleted.None).ToList(), true, permissions, person, date);
                        }

                        if (item.Type == ItemType.Folder)
                        {
                            IEnumerable<ItemAssignments> itemAssignments = GetQuery<ItemAssignments>().Where(a => a.IdItem == item.Id && a.Inherited);
                            List<RepositoryItem> children = GetQuery<RepositoryItem>().Where(a => a.IdFolder == item.Id).ToList();
                            children.ForEach(c => ApplyInheritedAssignment(c, itemAssignments, permissions, person, date));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception();
                    }
                }

                private List<ItemAssignments> AddAssignmentsFromOthers(long idItem, RepositoryIdentifier identifier, List<ItemAssignments> sourceAssignments, Boolean inherited, long permissions, litePerson person, DateTime? date = null)
                {
                    List<ItemAssignments> assignments = null;
                    if (sourceAssignments.Any())
                    {
                        Boolean isInTransaction = Manager.IsInTransaction();
                        try
                        {
                            assignments = new List<ItemAssignments>();
                            if (!isInTransaction)
                                Manager.BeginTransaction();
                            foreach (var query in sourceAssignments.Where(a=> a.Deleted== BaseStatusDeleted.None).GroupBy(s => s.Type))
                            {
                                switch (query.Key)
                                {
                                    case AssignmentType.community:
                                        assignments.AddRange(query.Select(a => new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date) { Repository=identifier, Denyed = a.Denyed, Inherited = inherited, IdItem = idItem, Type = query.Key, IdCommunity = a.IdCommunity }).ToList());
                                        break;
                                    case AssignmentType.person:
                                        assignments.AddRange(query.Select(a => new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date) { Repository = identifier, Denyed = a.Denyed, Inherited = inherited, IdItem = idItem, Type = query.Key, IdPerson = a.IdPerson }).ToList());
                                        break;
                                    case AssignmentType.role:
                                        assignments.AddRange(query.Select(a => new ItemAssignments(person, UC.IpAddress, UC.ProxyIpAddress, date) { Repository = identifier, Denyed = a.Denyed, Inherited = inherited, IdItem = idItem, Type = query.Key, IdRole = a.IdRole, IdCommunity = a.IdCommunity }).ToList());
                                        break;
                                }
                            }
                            if (assignments.Any())
                                Manager.SaveOrUpdateList(assignments);
                            if (!isInTransaction)
                                Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            if (!isInTransaction)
                                Manager.RollBack();
                        }
                    }
                    return assignments;
                }
            #endregion
            public Boolean HasPermissionToSeeItem(Int32 idUser,long idItem, ModuleRepository.ActionType action){
                return HasPermissionToSeeItem(idUser, Manager.Get<liteRepositoryItem>(idItem), null, action);
            }
            public Boolean HasPermissionToSeeItem(Int32 idUser, long idItem, long idVersion, ModuleRepository.ActionType action)
            {
                return HasPermissionToSeeItem(idUser, Manager.Get<liteRepositoryItem>(idItem), Manager.Get<liteRepositoryItemVersion>(idVersion), action);
            }
            public Boolean HasPermissionToSeeItem(Int32 idUser, liteRepositoryItem item, liteRepositoryItemVersion version, ModuleRepository.ActionType action)
            {
                litePerson person = Manager.GetLitePerson(idUser);
                if (item==null)
                    return false;
                RepositoryType type = (item.IdCommunity>0) ? RepositoryType.Community : RepositoryType.Portal;
                ModuleRepository mPermissions = GetPermissions(item.Repository, idUser);
                if (mPermissions.Administration || mPermissions.ManageItems )
                    return (item.Availability!= ItemAvailability.notavailable || (person!=null && person.TypeID==(int) UserTypeStandard.SysAdmin));
                else
                {
                    List<long> idItems = GetIdFathers(item);
                    idItems.Insert(0,item.Id);
                    List<liteItemAssignments> assignments = new List<liteItemAssignments>();
                    Int32 idRole = (Int32)RoleTypeStandard.Guest;
                    Int32 idProfileType = Manager.GetIdProfileType(idUser);
                    switch (type)
                    {
                        case RepositoryType.Community:
                        case RepositoryType.Portal:
                            assignments = GetAssignments(idItems.ToList());
                            idRole = (type== RepositoryType.Community ) ? Manager.GetActiveSubscriptionIdRole(idUser, item.IdCommunity) : idRole;
                            break;
                    }
                    Boolean available = true;
                    if (idItems.Any(i=> i != item.Id))
                        available = (from f in Manager.GetIQ<liteRepositoryItem>() where f.Type== ItemType.Folder && f.IsVisible && f.IdOwner!=idUser && idItems.Contains(f.Id) select f.Id).Any();

                    if(available){
                        available = HasValidAssignments(item.Id, idUser, idRole, idProfileType, assignments);
                        if(available && idItems.Any(i=> i != item.Id)){
                            foreach(long idFolder in idItems.Where(i=> i!=item.Id)){
                                available = HasValidAssignments(idFolder, idUser, idRole, idProfileType, assignments);
                                if (!available)
                                    break;
                            }
                        }
                    }
                    switch (action)
                    {
                        case ModuleRepository.ActionType.DownloadFile:
                            return available && (((item.IsVisible && item.IsDownloadable) || item.IdOwner == idUser) && mPermissions.DownloadOrPlay)
                                && (version == null || (version.Status != ItemStatus.Removed || version.IdOwner == idUser));
                        case ModuleRepository.ActionType.PlayFile:
                            return available && ((item.IsVisible || item.IdOwner == idUser) && mPermissions.DownloadOrPlay)
                                && (version == null || (version.Status != ItemStatus.Removed || version.IdOwner == idUser));
                        default:
                            return available && ((item.IsVisible || item.IsDownloadable || item.IdOwner == idUser) && mPermissions.DownloadOrPlay)
                               && (version == null || (version.Status != ItemStatus.Removed || version.IdOwner == idUser));
                    }

                }
            }

            #region "For community news"
                public List<Int32> GetUsersWithPermissions(long idItem, long idVersion, Int32 permission)
                {
                    return GetUsersWithPermissions(ItemGetVersion(idItem, idVersion), permission);
                }
                public List<Int32> GetUsersWithPermissions(liteRepositoryItemVersion version, Int32 permission)
                {
                    List<Int32> results = new List<Int32>();
                    if (version != null)
                    {
                        try
                        {
                            Int32 idModule = GetIdModule();
                            List<ItemAssignments> assignments = (from a in Manager.GetIQ<ItemAssignments>()
                                                                 where a.IdItem == version.IdItem && a.Deleted == BaseStatusDeleted.None && a.Inherited 
                                                                 select a).ToList();

                         
                            switch (version.Repository.Type)
                            {
                                case RepositoryType.Community:
                                    List<LazySubscription> subscriptions = (from s in Manager.GetIQ<LazySubscription>()
                                                                            where s.Accepted && s.Enabled && s.IdCommunity == version.Repository.IdCommunity
                                                                            select s).ToList();

                                    List<Int32> idRoles = (from p in Manager.GetIQ<LazyCommunityModulePermission>()
                                                           where p.CommunityID == version.Repository.IdCommunity && p.ModuleID == idModule
                                                           select p).ToList().Where(p => PermissionHelper.CheckPermissionSoft(permission, p.PermissionLong)).Select(p => p.RoleID).Distinct().ToList();


                                    List<Int32> usersRemove = assignments.Where(a => a.Type == AssignmentType.person && (a.Denyed || !subscriptions.Any(s => s.IdPerson == a.IdPerson && idRoles.Contains(s.IdRole)))).Select(a => a.IdPerson).Distinct().ToList();
                                    List<Int32> usersToAdd = assignments.Where(a => a.Type == AssignmentType.person && !usersRemove.Contains(a.IdPerson)).Select(a => a.IdPerson).Distinct().ToList();

                                    if (assignments.Any(a => a.Type == AssignmentType.community && !a.Denyed))
                                    {
                                        idRoles = idRoles.Where(i=> !assignments.Any(a=> a.IdRole== i && a.Type== AssignmentType.role && a.Denyed)).ToList();
                                        usersToAdd.AddRange(subscriptions.Where(s => !usersRemove.Contains(s.IdPerson) && idRoles.Contains(s.IdRole)).Select(s => s.IdPerson).ToList());
                                    }
                                    else
                                    {
                                        usersToAdd.AddRange(subscriptions.Where(s => !usersRemove.Contains(s.IdPerson) && idRoles.Contains(s.IdRole) && assignments.Any(a => a.Type == AssignmentType.role && !a.Denyed && a.IdRole == s.IdRole)).Select(s => s.IdPerson).ToList());
                                    }
                                    results.AddRange(usersToAdd);
                                    break;
                                case RepositoryType.Portal:
                                    break;
                            }
                          
                        }
                        catch (Exception ex)
                        {
                            results = new List<Int32>();
                        }
                    }
                    return results.Distinct().ToList();
                }
       
            #endregion
        #endregion
    }
}