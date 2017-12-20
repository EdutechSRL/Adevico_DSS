using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.DomainModel.Helpers;
using System.Linq.Expressions;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository : CoreServices
    {
        protected const Int32 maxItemsForQuery = 500;

        #region initClass
            protected lm.Comol.Core.InLineTags.Business.ServiceInLineTags ServiceTags;
            public ServiceFileRepository() :base() { }
            public ServiceFileRepository(iApplicationContext oContext)
                : base(oContext)
            {
                ServiceTags = new lm.Comol.Core.InLineTags.Business.ServiceInLineTags(oContext);
            }
            public ServiceFileRepository(iDataContext oDC)
                : base(oDC)
            {
                ServiceTags = new lm.Comol.Core.InLineTags.Business.ServiceInLineTags(oDC);
            }
        #endregion

            #region "Repository folder"
                public liteRepositoryItem FolderGetRepository(RepositoryType type, Int32 idCommunity)
                {
                    liteRepositoryItem root = null;
                    try
                    {
                        root = GetQuery(type, idCommunity).Where(i => i.Type == ItemType.RootFolder).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return root;
                }
                public Boolean FolderAllowUpload(long idFolder)
                {
                    Boolean allow = false;
                    try
                    {
                        allow = (from i in Manager.GetIQ<liteRepositoryItem>() where i.Id == idFolder && i.Type == ItemType.Folder && i.AllowUpload select i).Any();
                    }
                    catch (Exception ex)
                    {
                        allow = false;
                    }
                    return allow;
                }
            #endregion

            #region "Manage Items"
            public Boolean HasFolders(Int32 idPerson, RepositoryType type, Int32 idCommunity, ModuleRepository permissions)
            {
                return true;
            }
            public Boolean HasFolders(List<dtoDisplayRepositoryItem> items)
            {
                return items.Where(i => i.FolderType == FolderType.standard || (!i.IsEmpty && i.FolderType!= FolderType.standard && i.FolderType != FolderType.none)).Any();
            }

        
            #region "Folders Loading"
                public dtoDisplayRepositoryItem GetItemWithPermissions(long idItem, Int32 idCurrentPerson, RepositoryIdentifier identifier, String unknownUser,Boolean externalAllowManage = false, Boolean externalAllowView = false)
                {
                    liteRepositoryItem rItem = ItemGet(idItem);
                    if (rItem == null)
                        return null;
                    else if (rItem.IsInternal)
                        return dtoDisplayRepositoryItem.CreateFromInternal(new dtoRepositoryItem(rItem, null, Manager.GetLitePerson(rItem.IdOwner), Manager.GetLitePerson(rItem.IdModifiedBy), unknownUser), UC.CurrentUserID, externalAllowManage, externalAllowView);
                    else
                        return GetItemWithPermissions(idItem, idCurrentPerson, identifier.Type, identifier.IdCommunity,  unknownUser);
                }
                public dtoDisplayRepositoryItem GetItemWithPermissions(long idItem, Int32 idCurrentPerson, RepositoryType type, Int32 idCommunity,  String unknownUser)
                {
                    liteRepositorySettings settings = SettingsGetDefault(type, idCommunity);
                    return (settings == null) ? null : GetItemWithPermissions(idItem, idCurrentPerson, RepositoryIdentifier.Create(type,idCommunity), settings, unknownUser);
                }
                public dtoDisplayRepositoryItem GetItemWithPermissions(long idItem, Int32 idCurrentPerson, RepositoryIdentifier identifier, liteRepositorySettings settings, String unknownUser)
                {
                    String key = CacheKeys.UserViewOfPartialRepository(idCurrentPerson, identifier);
                    List<dtoDisplayRepositoryItem> rItems = lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoDisplayRepositoryItem>>(key);
                    if (rItems == null || !rItems.Any())
                    {
                        rItems = new List<dtoDisplayRepositoryItem>();
                        List<dtoRepositoryItem> fItems = GetFullRepository(identifier, unknownUser, true);
                        if (fItems == null)
                            return null;
                        ModuleRepository module = GetPermissions(identifier, idCurrentPerson);
                        List<dtoDisplayRepositoryItem> items = GetRepositoryItemsWithPermissions(settings, UC.CurrentUserID, identifier, fItems, module, module.Administration || module.ManageItems, module.Administration || module.ManageItems, false, false);
                        rItems.AddRange(items);
                        if (rItems != null)
                            CacheHelper.AddToCache<List<dtoDisplayRepositoryItem>>(key, rItems, CacheExpiration._5minutes);
                    }
                    return GetItemWithPermissions(idItem, rItems);
                }
                public List<dtoDisplayRepositoryItem> GetItemsWithPermissions(List<long> idItems, Int32 idCurrentPerson, RepositoryIdentifier identifier, String unknownUser, Boolean useCache=false)
                {
                    liteRepositorySettings settings = SettingsGetByRepositoryIdentifier(identifier);
                    return (settings == null) ? null : GetItemsWithPermissions(idItems, idCurrentPerson, identifier, settings, unknownUser, useCache);
                }
                public List<dtoDisplayRepositoryItem> GetItemsWithPermissions(List<long> idItems, Int32 idCurrentPerson, RepositoryIdentifier identifier, liteRepositorySettings settings, String unknownUser, Boolean useCache = false)
                {
                    String key = CacheKeys.UserViewOfPartialRepository(idCurrentPerson, identifier);
                    ModuleRepository module = GetPermissions(identifier, idCurrentPerson);
                    List<dtoDisplayRepositoryItem> rItems = lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoDisplayRepositoryItem>>(key);
                    if (rItems == null || !rItems.Any() || !useCache)
                    {
                        rItems = new List<dtoDisplayRepositoryItem>();
                        List<dtoRepositoryItem> fItems = GetFullRepository(identifier, unknownUser, true);
                        if (fItems == null)
                            return null;
                       
                        List<dtoDisplayRepositoryItem> items = GetRepositoryItemsWithPermissions(settings, UC.CurrentUserID, identifier, fItems, module, module.Administration || module.ManageItems, module.Administration || module.ManageItems, false, false);
                        if (items!=null)
                            rItems.AddRange(items);
                        if (useCache && rItems.Any())
                            CacheHelper.AddToCache<List<dtoDisplayRepositoryItem>>(key, rItems, CacheExpiration._5minutes);
                    }
                    return GetItemsWithPermissions(idItems, rItems, settings, idCurrentPerson, module);
                }
                public dtoDisplayRepositoryItem GetItemWithPermissionsAndStatistics(long idItem, List<dtoDisplayRepositoryItem> items, Int32 idPerson)
                {
                    dtoDisplayRepositoryItem item = GetItemWithPermissions(idItem, items);
                    if (item != null)
                    {
                        item.Downloaded = (from i in Manager.GetIQ<liteRepositoryItem>() where i.Id == idItem select i.Downloaded).Skip(0).Take(1).ToList().FirstOrDefault();
                        item.MyDownloads = DownloadsCountForItem(idItem, idPerson);
                        if (item.MyDownloads>item.Downloaded )
                            item.MyDownloads = DownloadsCountForItem(idItem);
                        ItemRefreshAvailability(item);
                    }
                    return item;
                }
                public dtoDisplayRepositoryItem GetItemWithPermissions(long idItem, List<dtoDisplayRepositoryItem> items)
                {
                    dtoDisplayRepositoryItem item = items.Where(i => i.Type != ItemType.Folder || (i.FolderType == FolderType.standard || i.FolderType == FolderType.recycleBin)).Select(c => c.GetItem(idItem)).Where(i => i != null).FirstOrDefault();
                    if (item != null && (item.Type == ItemType.ScormPackage || item.Type == ItemType.Multimedia))
                        ItemRefreshAvailability(item);
                    return item;
                }
                public List<dtoDisplayRepositoryItem> GetItemsWithPermissions(List<long> idItems, List<dtoDisplayRepositoryItem> items, liteRepositorySettings settings, Int32 idCurrentUser, ModuleRepository permissions)
                {
                    List<dtoDisplayRepositoryItem> results = items.Where(i => i.Type != ItemType.Folder && idItems.Contains(i.Id)).ToList();
                    List<long> idNotRetrieved = idItems.Where(i => !results.Any(r => r.Id == i)).ToList();
                    if (idNotRetrieved.Any())
                    {
                        results.AddRange(items.Where(i => i.FolderType == FolderType.standard).Select(c => c.GetItems(idNotRetrieved)).Where(i => i != null).SelectMany(i => i.ToList()).ToList());
                        idNotRetrieved = idItems.Where(i => !results.Any(r => r.Id == i)).ToList();
                        if (idNotRetrieved.Any())
                        {
                            results.AddRange(items.Where(i => i.FolderType == FolderType.recycleBin).Select(c => c.GetItems(idNotRetrieved)).Where(i => i != null).SelectMany(i => i.ToList()).ToList());
                        }
                    }

                    if (settings.AllowVersioning)
                    {
                        List<dtoItemFile> files = results.SelectMany(i => i.GetLiteFiles(true)).ToList();
                        foreach (dtoDisplayRepositoryItem file in files.Where(f => f.ItemReferrer.Permissions.VersioningAvailable).Select(f => f.ItemReferrer))
                        {
                            file.Permissions.AddVersion = (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles || ((file.Permissions.Edit || file.IdOwner == idCurrentUser)));
                            file.Permissions.SetVersion = file.Permissions.AddVersion;
                            file.Permissions.RemoveVersion = file.Permissions.AddVersion;
                            file.Permissions.AddVersionFromModule = file.Permissions.AddVersion;
                        }
                    }

                    return results;
                }
                public List<dtoDisplayRepositoryItem> GetAvailableRepositoryItems(
                    liteRepositorySettings settings, 
                    Int32 idCurrentPerson, 
                    RepositoryType type, 
                    Int32 idCommunity,  
                    String unknownUser, 
                    Dictionary<FolderType, String> fTranslations, 
                    Dictionary<ItemType, String> iTranslations, 
                    ModuleRepository permissions, 
                    Boolean forAdmin = false, 
                    Boolean alsoHidden = false, 
                    Boolean onlyFolder = false, 
                    Boolean useCache = true)
                {
                    return GetAvailableRepositoryItems(
                        settings, 
                        idCurrentPerson, 
                        RepositoryIdentifier.Create(type, idCommunity), 
                        unknownUser, 
                        fTranslations, 
                        iTranslations, 
                        permissions, 
                        forAdmin, 
                        alsoHidden, 
                        onlyFolder, 
                        useCache);
                }

                public List<dtoDisplayRepositoryItem> GetAvailableRepositoryItems(
                    liteRepositorySettings settings,
                    Int32 idCurrentPerson, 
                    RepositoryIdentifier identifier, 
                    String unknownUser, 
                    Dictionary<FolderType, String> fTranslations, 
                    Dictionary<ItemType, String> iTranslations, 
                    ModuleRepository permissions, 
                    Boolean forAdmin = false, 
                    Boolean alsoHidden = false, 
                    Boolean onlyFolder = false, 
                    Boolean useCache = true)
                {
                    String key = CacheKeys.UserViewOfRepository(idCurrentPerson, identifier, onlyFolder);
                    List<dtoDisplayRepositoryItem> results = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoDisplayRepositoryItem>>(key) : null;
                    if (results == null || !results.Any())
                    {
                        results = new List<dtoDisplayRepositoryItem>();
                        List<dtoRepositoryItem> fItems = GetFullRepository(identifier, unknownUser, useCache);
                        if (fItems == null)
                            return null;
                        List<dtoDisplayRepositoryItem> items = GetRepositoryItemsWithPermissions(settings, UC.CurrentUserID, identifier, fItems, permissions, forAdmin, alsoHidden, onlyFolder, useCache);
                        results.AddRange(items);
                        if (permissions.Administration || permissions.ManageItems || permissions.DeleteMyFiles)
                            results.Add(GetRecycleBinFolder(fTranslations[FolderType.recycleBin], items));

                        if (results != null && useCache)
                            CacheHelper.AddToCache<List<dtoDisplayRepositoryItem>>(key, results, CacheExpiration.Day);
                    }
                    return results;
                }
                protected List<dtoDisplayRepositoryItem> GetAvailableRepositoryItems(liteRepositorySettings settings, Int32 idCurrentPerson, RepositoryIdentifier identifier, String unknownUser, ModuleRepository permissions, Boolean forAdmin = false, Boolean alsoHidden = false, Boolean onlyFolder = false, Boolean useCache = true)
                {
                    String key = CacheKeys.UserViewOfRepository(idCurrentPerson, identifier, onlyFolder);
                    List<dtoDisplayRepositoryItem> results = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoDisplayRepositoryItem>>(key) : null;
                    if (results == null || !results.Any())
                    {
                        List<dtoRepositoryItem> fItems = GetFullRepository(identifier, unknownUser, useCache);
                        if (fItems == null)
                            return null;
                        return GetRepositoryItemsWithPermissions(settings, UC.CurrentUserID, identifier, fItems, permissions, forAdmin, alsoHidden, onlyFolder, useCache);
                    }
                    return results;
                }
                #region "Special folders"
                    private dtoDisplayRepositoryItem GetRecycleBinFolder(String name, List<dtoDisplayRepositoryItem> items)
                    {
                        dtoDisplayRepositoryItem folder = dtoDisplayRepositoryItem.GenerateFolder(FolderType.recycleBin, name);
                        folder.IdCommunity = items.Select(i => i.IdCommunity).FirstOrDefault();
                        items.ForEach(i => folder.Children.AddRange(i.GetDeletedItems()));
                        folder.Children = folder.Children.OrderBy(c => c.Path).ThenBy(c => c.Name).ToList();
                        return folder;
                    }
                    public dtoDisplayRepositoryItem GetFolder(List<dtoDisplayRepositoryItem> items, long idFolder, String identifierPath, FolderType folderType)
                    {
                        List<dtoDisplayRepositoryItem> folders = GetFolderListFromTree(items);
                        return folders.Where(f => f.Id == idFolder && (String.IsNullOrEmpty(identifierPath) || identifierPath == f.IdentifierPath) && f.FolderType == folderType && f.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                    }
                    private List<dtoDisplayRepositoryItem> GetFolderListFromTree(List<dtoDisplayRepositoryItem> items)
                    {
                        List<dtoDisplayRepositoryItem> folders = items.Where(i => i.Type == ItemType.Folder).ToList();
                        items.Where(i => i.Type == ItemType.Folder).ToList().ForEach(c => folders.AddRange(GetFolderListFromTree(c.Children)));
                        return folders;
                    }
                #endregion 

            #endregion

            #region "Items With Permissions Tree"

                private List<dtoDisplayRepositoryItem> GetRepositoryItemsWithPermissions(liteRepositorySettings settings, Int32 idCurrentUser, RepositoryIdentifier identifier,List<dtoRepositoryItem> fItems, ModuleRepository permissions,Boolean forAdmin = false, Boolean alsoHidden = false,Boolean onlyFolder = false ,Boolean useCache = true)
                {
                    Int32 idRole = (Int32)RoleTypeStandard.Guest;
                    Int32 idProfileType = Manager.GetIdProfileType(idCurrentUser);
                    List<dtoDisplayRepositoryItem> results = new List<dtoDisplayRepositoryItem>();
              
                    List<liteItemAssignments> assignments = new List<liteItemAssignments>();
                    switch (identifier.Type)
                    {
                        case RepositoryType.Community:
                        case RepositoryType.Portal:
                            assignments = GetAssignments(fItems.SelectMany(f => f.GetAllId()).Distinct().ToList());
                            idRole = (identifier.Type == RepositoryType.Community) ? Manager.GetActiveSubscriptionIdRole(UC.CurrentUserID, identifier.IdCommunity) : idRole;
                            break;
                    }
                    foreach (dtoRepositoryItem item in fItems.Where(i=> (!onlyFolder || (onlyFolder && i.Type== ItemType.Folder)) && (forAdmin || alsoHidden || (i.IsVisible || i.IdOwner== idCurrentUser)))){
                        dtoDisplayRepositoryItem dItem = null;
                        if (forAdmin)
                            dItem = new dtoDisplayRepositoryItem(item, idCurrentUser, settings, permissions, forAdmin);
                        else
                            dItem = new dtoDisplayRepositoryItem(item, idCurrentUser, settings, permissions, forAdmin, HasValidAssignments(item.Id, idCurrentUser, idRole, idProfileType, assignments));

                        dItem.HasDefaultPermissions = !(assignments != null && assignments.Any(a => a.IdItem == dItem.Id && a.Type != AssignmentType.community));
                        if (dItem.IsUserAvailable)
                        {
                            if (item.Children.Any())
                                AddChildren(item.Children, dItem, settings, idCurrentUser, idRole,idProfileType, permissions, forAdmin, alsoHidden, onlyFolder, assignments);
                            results.Add(dItem);
                        }
                    }
                    if (!onlyFolder)
                        SetForTreeItemsPermissions(results, settings, idCurrentUser, permissions);
                    return results;
                }
                private void AddChildren(List<dtoRepositoryItem> children, dtoDisplayRepositoryItem father, liteRepositorySettings settings, Int32 idCurrentUser,  Int32 idRole,Int32 idProfileType,ModuleRepository permissions, Boolean forAdmin = false, Boolean alsoHidden = false, Boolean onlyFolder = false, List<liteItemAssignments> assignments=null)
                {
                    foreach (dtoRepositoryItem item in children.Where(i => (!onlyFolder || (onlyFolder && i.Type == ItemType.Folder)) && (forAdmin || alsoHidden || (i.IsVisible || i.IdOwner == idCurrentUser))))
                    {
                        dtoDisplayRepositoryItem child = null;
                        if (forAdmin)
                            child = new dtoDisplayRepositoryItem(item, idCurrentUser, settings, permissions, forAdmin);
                        else
                            child = new dtoDisplayRepositoryItem(item, idCurrentUser, settings, permissions, forAdmin, HasValidAssignments(item.Id, idCurrentUser, idRole, idProfileType, assignments));
                        child.HasDefaultPermissions = !(assignments != null && assignments.Any(a => a.IdItem == child.Id && a.Type != AssignmentType.community));
                        if (child.IsUserAvailable)
                        {
                            child.Father = father;
                            if (item.Children.Any())
                                AddChildren(item.Children, child, settings, idCurrentUser, idRole, idProfileType,permissions, forAdmin, alsoHidden, onlyFolder, assignments);
                            father.Children.Add(child);
                        }
                    }
                }

                private void SetForTreeItemsPermissions(List<dtoDisplayRepositoryItem> items, liteRepositorySettings settings, Int32 idCurrentUser,ModuleRepository permissions)
                {
                    List<dtoDisplayRepositoryItem> folders = items.SelectMany(i => i.GetItems(ItemType.Folder, true)).ToList();

                    if (folders.Any())
                    {
                        switch (folders.Count)
                        {
                            case 0:
                                break;
                            case 1:
                                folders[0].Permissions.Move = false;
                                if (items.Any(i => i.Type != ItemType.Folder))
                                    items.Where(i => i.Type != ItemType.Folder).ToList().ForEach(i => i.Permissions.Move = i.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles || ((i.Permissions.Edit || i.IdOwner==idCurrentUser) && folders[0].AllowUpload )));
                                if (folders[0].Children.Any())
                                    folders[0].Children.ForEach(i => i.Permissions.Move = i.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles));
                                break;
                            default:
                                Dictionary<long, List<long>> folderExceptions = new Dictionary<long, List<long>>();
                                folderExceptions = folders.ToDictionary(f => f.Id, f => f.GetIdChildrenFolders());
                                folderExceptions.Keys.ToList().ForEach(k => folderExceptions[k].Add(k));

                                items.ForEach(i => i.Permissions.Move = (i.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles)));
                                foreach (dtoDisplayRepositoryItem folder in folders.Where(f=> f.Father==null))
                                {
                                    folderExceptions[folder.Id].Add(0);
                                    folder.Permissions.Move = (folder.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles) && folders.Any(f=> !folderExceptions[folder.Id].Contains(f.Id)));
                                    if (folder.Children.Any())
                                    {
                                        folder.Children.Where(i => i.Type != ItemType.Folder).ToList().ForEach(i => i.Permissions.Move = i.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles || ((i.Permissions.Edit || i.IdOwner == idCurrentUser) && folders.Any(f => !folderExceptions[folder.Id].Contains(f.Id) && f.AllowUpload))));
                                        if (folder.Children.Any(c => c.Type == ItemType.Folder))
                                            SetForTreeItemsPermissions(folder.Children.Where(i => i.Type == ItemType.Folder), idCurrentUser, permissions, folders, folderExceptions);
                                    }
                                }
                                break;
                        }
                    }
                    if (settings.AllowVersioning)
                    {
                        List<dtoItemFile> files = items.SelectMany(i => i.GetLiteFiles(true)).ToList();
                       
                        foreach (dtoDisplayRepositoryItem file in files.Where(f => f.ItemReferrer.Permissions.VersioningAvailable).Select(f => f.ItemReferrer))
                        {
                            file.Permissions.AddVersion = (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles || ((file.Permissions.Edit || file.IdOwner == idCurrentUser)));
                            file.Permissions.SetVersion = file.Permissions.AddVersion;
                            file.Permissions.RemoveVersion = file.Permissions.AddVersion;
                            file.Permissions.AddVersionFromModule = file.Permissions.AddVersion;
                        }

                    }
                }
                private void SetForTreeItemsPermissions(IEnumerable<dtoDisplayRepositoryItem> items, Int32 idCurrentUser, ModuleRepository permissions, List<dtoDisplayRepositoryItem> folders, Dictionary<long, List<long>> folderExceptions)
                {
                    foreach (dtoDisplayRepositoryItem folder in items)
                    {
                        folder.Permissions.Move = (folder.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles || ((folder.Permissions.Edit || folder.IdOwner == idCurrentUser) && folders.Any(f => !folderExceptions[folder.Id].Contains(f.Id) && f.AllowUpload)) && folders.Any(f => !folderExceptions[folder.Id].Contains(f.Id))));
                        folder.Children.Where(i => i.Type != ItemType.Folder).ToList().ForEach(i => i.Permissions.Move = i.Deleted == BaseStatusDeleted.None && (permissions.Administration || permissions.ManageItems || permissions.EditOthersFiles || ((i.Permissions.Edit || i.IdOwner == idCurrentUser) && folders.Any(f => !folderExceptions[folder.Id].Contains(f.Id) && f.AllowUpload))));
                        if (folder.Children.Any(c => c.Type == ItemType.Folder))
                            SetForTreeItemsPermissions(folder.Children.Where(i => i.Type == ItemType.Folder), idCurrentUser, permissions, folders, folderExceptions);
                    }
                }
            #endregion 

            #region "Repository tree"
                protected List<dtoRepositoryItem> GetFullRepository(RepositoryType type, Int32 idCommunity, String unknownUser, Boolean useCache = true)
                {
                    return GetFullRepository(RepositoryIdentifier.Create(type,idCommunity),unknownUser,useCache);
                }

                protected List<dtoRepositoryItem> GetFullRepository(RepositoryIdentifier identifier, String unknownUser, Boolean useCache = true)
                {
                    String key = CacheKeys.Repository(identifier);
                    List<dtoRepositoryItem> items = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoRepositoryItem>>(key) : null;
                    if (items == null || !items.Any())
                    {
                        items = GetRepositoryTree(identifier, unknownUser);
                        if (items != null && useCache)
                            CacheHelper.AddToCache<List<dtoRepositoryItem>>(key, items, CacheExpiration.Week);
                    }
                    return items;
                }
                private List<dtoRepositoryItem> GetRepositoryTree(RepositoryType type, Int32 idCommunity, String unknownUser)
                {
                    return GetRepositoryTree(RepositoryIdentifier.Create(type, idCommunity), unknownUser);
                }
                private List<dtoRepositoryItem> GetRepositoryTree(RepositoryIdentifier identifier, String unknownUser)
                {
                    List<dtoRepositoryItem> results = null;
                    try
                    {
                        List<liteRepositoryItem> items = GetQuery(identifier).ToList();
                        List<Int32> idUsers = items.Select(i => i.IdOwner).Distinct().ToList();
                        idUsers.AddRange(items.Where(i => i.IdModifiedBy > 0).Select(i => i.IdModifiedBy).Distinct().ToList());
                        List<litePerson> owners = Manager.GetLitePersons(idUsers.Distinct().ToList());
                        results = CreateNodes(items, null, owners, unknownUser);
                    }
                    catch (Exception ex)
                    {

                    }
                    return results;
                }
                private List<dtoRepositoryItem> CreateNodes(List<liteRepositoryItem> items, dtoRepositoryItem father, List<litePerson> users, String unknownUser)
                {
                    List<dtoRepositoryItem> results = items.Where(i => (father == null && i.IdFolder == 0) || (father != null && father.Id == i.IdFolder)).OrderBy(i => i.IsFile).ThenBy(i => i.Name).Select(i => new dtoRepositoryItem(i, father, users.Where(o => o.Id == i.IdOwner).FirstOrDefault(), users.Where(o => o.Id == i.IdModifiedBy).FirstOrDefault(), unknownUser) { Path = (father == null) ? "/" : father.Path + father.Name + "/" }).ToList();
                    results.Where(i => i.Type == ItemType.Folder).ToList().ForEach(i => i.Children = CreateNodes(items, i, users, unknownUser));
             
                    return results;
                }


        
                protected IEnumerable<liteRepositoryItem> GetQuery(RepositoryIdentifier identifier){
                    return GetQuery(identifier.Type, identifier.IdCommunity);
                }
                protected IEnumerable<liteRepositoryItem> GetQuery(RepositoryType type, Int32 idCommunity)
                {
                    var query = GetQuery<liteRepositoryItem>().Where(i => !i.IsInternal && !i.IsVirtual && i.Repository != null);
                    switch (type)
                    {
                        case RepositoryType.Portal:
                            query = query.Where(i => i.Repository.Type == RepositoryType.Portal);
                            break;
                        case RepositoryType.Community:
                            query = query.Where(i => i.Repository.Type == RepositoryType.Community && i.Repository.IdCommunity == idCommunity);
                            break;
                    }
                    return query;
                }
                protected IEnumerable<RepositoryItem> GetFullQuery(RepositoryIdentifier identifier)
                {
                    return GetFullQuery(identifier.Type, identifier.IdCommunity);
                }
                protected IEnumerable<RepositoryItem> GetFullQuery(RepositoryType type, Int32 idCommunity)
                {
                    var query = GetQuery<RepositoryItem>().Where(i => !i.IsInternal && !i.IsVirtual && i.Repository != null);
                    switch (type)
                    {
                        case RepositoryType.Portal:
                            query = query.Where(i => i.Repository.Type== RepositoryType.Portal);
                            break;
                        case RepositoryType.Community:
                            query = query.Where(i => i.Repository.Type == RepositoryType.Community && i.Repository.IdCommunity == idCommunity);
                            break;
                    }
                    return query;
                }
                protected IEnumerable<T> GetQuery<T>()
                {
                    return (from i in Manager.GetIQ<T>() select i);
                }
            #endregion

            public List<long> GetIdFathers(liteRepositoryItem item)
            {
                List<long> fathers = new List<long>();
                if (item != null && item.IdFolder>0){
                    fathers.Add(item.IdFolder);
                    fathers.AddRange(GetIdFathers(item.IdFolder));
                }
                return fathers;
            }
            public List<long> GetIdFathers(long idItem)
            {
                List<long> fathers = new List<long>();
                long idFather = GetIdFather(idItem);
                if (idFather>0){
                    fathers.Add(idFather);
                    fathers.AddRange(GetIdFathers(idFather));
                }
                return fathers;
            }
            public List<liteRepositoryItem> GetLiteFathers(long idFather, IEnumerable<liteRepositoryItem> repositoryItems)
            {
                List<liteRepositoryItem> fathers = new List<liteRepositoryItem>();
                liteRepositoryItem father = repositoryItems.Where(f => f.Id == idFather).FirstOrDefault();
                if (father != null && idFather > 0)
                {
                    fathers.Add(father);
                    fathers.AddRange(GetLiteFathers(father.IdFolder, repositoryItems));
                }
                return fathers;
            }


            public List<long> GetFullReverseIdFathers(long idItem, IEnumerable<RepositoryItem> repositoryItems)
            {
                List<long> fathers = new List<long>();
                long idFather = repositoryItems.Where(f => f.Id == idItem).Select(f => f.IdFolder).FirstOrDefault();
                if (idFather > 0)
                {
                    fathers.AddRange(GetFullReverseIdFathers(idFather, repositoryItems));
                    fathers.Add(idFather);
                }
                else
                    fathers.Add(idFather);
                return fathers;
            }
            public List<RepositoryItem> GetFullReverseFathers(long idFather, IEnumerable<RepositoryItem> repositoryItems)
            {
                List<RepositoryItem> fathers = new List<RepositoryItem>();
                RepositoryItem father = repositoryItems.Where(f => f.Id == idFather).FirstOrDefault();
                if (father !=null && idFather>0)
                {
                    fathers.AddRange(GetFullReverseFathers(father.IdFolder, repositoryItems));
                    fathers.Add(father);
                }
                return fathers;
            }
            public long GetIdFather(long idItem)
            {
                return (from i in Manager.GetIQ<liteRepositoryItem>() where i.Id == idItem select i.IdFolder).Skip(0).Take(1).ToList().FirstOrDefault();
            }
        #endregion
    }
}