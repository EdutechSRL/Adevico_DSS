using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.FileRepository.Business
{
    public partial class ServiceRepository : ServiceFileRepository
    {
          #region initClass
            public ServiceRepository() :base() { }
            public ServiceRepository(iApplicationContext oContext)
                : base(oContext)
            {

            }
            public ServiceRepository(iDataContext oDC)
                : base(oDC)
            {
              
            }
        #endregion


        #region "Folders for selection"
            public List<dtoNodeFolderItem> GetFoldersForUpload(String filePath, long idCurrentFolder, Int32 idCurrentUser, RepositoryIdentifier identifier, ModuleRepository permissions, String unkownUser, String rootFolderName, List<dtoFolderTreeItem> folders = null)
            {
                return FoldersGetForSelection(filePath, idCurrentFolder, idCurrentUser, identifier.Type, identifier.IdCommunity, permissions, unkownUser, rootFolderName, true, 0, null, false, folders);
            }
            public List<dtoNodeFolderItem> GetFoldersForUpload(String filePath, long idCurrentFolder, Int32 idCurrentUser, RepositoryType type, Int32 idCommunity, ModuleRepository permissions, String unkownUser, String rootFolderName, List<dtoFolderTreeItem> folders = null)
            {
                return FoldersGetForSelection(filePath, idCurrentFolder, idCurrentUser, type, idCommunity,  permissions, unkownUser, rootFolderName, true, 0, null, false, folders);
            }
            public List<dtoNodeFolderItem> GetFoldersForMove(String filePath, long idCurrentFolder, Int32 idCurrentUser, RepositoryType type, Int32 idCommunity,  ModuleRepository permissions, String unkownUser, String rootFolderName, long sizeToMove, List<long> foldersToRemove, Boolean removeCurrent = false)
            {
                return FoldersGetForSelection(filePath, idCurrentFolder, idCurrentUser, type, idCommunity, permissions, unkownUser,rootFolderName, false, sizeToMove, foldersToRemove, removeCurrent);
            }

            public List<dtoNodeFolderItem> FoldersGetForSelection(String filePath, long idCurrentFolder, Int32 idCurrentUser, RepositoryType type, Int32 idCommunity,  ModuleRepository permissions, String unkownUser, String rootFolderName, Boolean forUpload, long sizeToMove = 0, List<long> foldersToRemove = null, Boolean removeCurrent = false, List<dtoFolderTreeItem> folders= null)
            {
                List<dtoNodeFolderItem> nodes = new List<dtoNodeFolderItem>();
                liteRepositorySettings settings = SettingsGetDefault( type, idCommunity);
                if (folders==null)
                    folders = GetAllFoldersForSelect(filePath, idCurrentFolder,settings, idCurrentUser, type, idCommunity,  permissions,rootFolderName);

                if (folders != null)
                {
                    Boolean admin = permissions.ManageItems || permissions.Administration;
                    List<dtoDisplayRepositoryItem> items = GetAvailableRepositoryItems(settings, idCurrentUser,RepositoryIdentifier.Create(type, idCommunity), unkownUser, permissions, admin, admin);
                    List<long> idAvailable = items.SelectMany(i => i.GetItems(ItemType.Folder, true)).Select(i => i.Id).ToList();
                    idAvailable.Add(0);
                    if (removeCurrent)
                        foldersToRemove.Add(idCurrentFolder);
                    if (foldersToRemove != null)
                        idAvailable = idAvailable.Except(foldersToRemove).ToList();
                    folders.ForEach(f => nodes.AddRange(CreateFolderNodes(null, f, sizeToMove, idAvailable, forUpload)));

                }
                return nodes;
            }

            private List<dtoNodeFolderItem> CreateFolderNodes(dtoNodeFolderItem father, dtoFolderTreeItem folder, long sizeToMove, List<long> idAvailable, Boolean forUpload)
            {
                List<dtoNodeFolderItem> results = new List<dtoNodeFolderItem>();

                dtoNodeFolderItem cNode = CreateItemNode(father, folder, sizeToMove, idAvailable, forUpload);
                dtoNodeFolderItem openNode = CreateOpenNode(cNode, folder.Children.Any());

                results.Add(openNode);
                results.Add(cNode);
                if (folder.Children.Any(c=> idAvailable.Contains(c.Id)))
                {
                    results.Add(new dtoNodeFolderItem() { Type = NodeType.OpenChildren });
                    folder.Children.Where(c => idAvailable.Contains(c.Id)).ToList().ForEach(n => results.AddRange(CreateFolderNodes(openNode, n, sizeToMove, idAvailable, forUpload)));
                    results.Add(new dtoNodeFolderItem() { Type = NodeType.CloseChildren });
                }
                results.Add(new dtoNodeFolderItem() { Type = NodeType.CloseNode });
                return results;
            }
            private dtoNodeFolderItem CreateItemNode(dtoNodeFolderItem father, dtoFolderTreeItem folder, long sizeToMove, List<long> idAvailable, Boolean forUpload)
            {
                dtoNodeFolderItem node = new dtoNodeFolderItem() { Type = NodeType.Item, IsCurrent = folder.IsCurrent};
                node.Id = folder.Id;
                node.Name = folder.Name;
                node.IsHome = folder.IsHome;
                node.Selected = folder.IsCurrent;
                node.Selectable = (sizeToMove == 0 || folder.ValidateSpace(sizeToMove, forUpload));
                return node;
            }

            private dtoNodeFolderItem CreateOpenNode(dtoNodeFolderItem owner, Boolean hasChildren)
            {
                dtoNodeFolderItem node = new dtoNodeFolderItem() { Type = NodeType.OpenItemNode};
                node.Id = owner.Id;
                node.Name = owner.Name;
                node.IsEmpty = hasChildren;
                node.IsCurrent = owner.IsCurrent;
                node.IsEmpty = !hasChildren;
                node.Selectable = owner.Selectable;
                node.Selected = owner.Selected;
                return node;
            }

            private List<dtoFolderTreeItem> GetAllFoldersForSelect(String filePath, long idCurrentFolder, Int32 idCurrentUser, RepositoryType type, Int32 idCommunity,  ModuleRepository permissions,String rootFolderName)
            {
                return GetAllFoldersForSelect(filePath, idCurrentFolder, SettingsGetDefault( type, idCommunity), idCurrentUser, type, idCommunity,  permissions, rootFolderName);
            }
            private List<dtoFolderTreeItem> GetAllFoldersForSelect(String filePath, long idCurrentFolder, liteRepositorySettings settings, Int32 idCurrentUser, RepositoryType type, Int32 idCommunity,  ModuleRepository permissions, String rootFolderName)
            {
                List<dtoFolderTreeItem> folders = new List<dtoFolderTreeItem>();
                try
                {
                    if (settings != null)
                    {
                        List<liteRepositoryItem> items = GetQuery(type, idCommunity).ToList();
                        dtoFolderTreeItem root = CreateRootFolderTreeItem(filePath, idCurrentFolder, items.Where(i => i.IdFolder == 0), settings.DiskSpace, permissions);
                        root.Name = rootFolderName;
                        folders.Add(root);
                        foreach (liteRepositoryItem fItem in items.Where(i => i.Type == ItemType.Folder && i.IdFolder == 0 && i.Deleted== BaseStatusDeleted.None).OrderBy(i=>i.Name).ThenBy(i=> i.Id))
                        {
                            root.Children.Add(CreateFolderTreeItem(fItem, idCurrentFolder,  items.Where(i => i.IdFolder != 0), root,  permissions));
                        }
                    }
                }
                catch (Exception ex)
                {
                    folders = null;
                }
                return folders;
            }
            private dtoFolderTreeItem CreateRootFolderTreeItem(String filePath, long idCurrentFolder, IEnumerable<liteRepositoryItem> items, DiskSettings diskSpace, ModuleRepository permissions)
            {
                dtoFolderTreeItem root = new dtoFolderTreeItem() { Id = 0, IsHome = true, Name = "", IsCurrent = (idCurrentFolder == 0) };
                if (items.Any(i => i.Type != ItemType.Link && i.Deleted == BaseStatusDeleted.None))
                    root.Size = items.Where(i => i.Type != ItemType.Link && i.Deleted == BaseStatusDeleted.None).Select(i => i.Size).Sum();

                if (items.Any(i => i.Deleted != BaseStatusDeleted.None))
                    root.DeletedSize = items.Where(i => i.Deleted == BaseStatusDeleted.Manual).Select(i => i.Size + i.VersionsSize).Sum();
                if (items.Any(i => i.Type == ItemType.Folder && i.Deleted == BaseStatusDeleted.None && i.DeletedSize > 0))
                    root.DeletedSize += items.Where(i => i.Type == ItemType.Folder && i.Deleted == BaseStatusDeleted.None && i.DeletedSize > 0).Select(i => i.DeletedSize).Sum();

                if (items.Any(i => i.Deleted == BaseStatusDeleted.None && i.HasVersions))
                    root.VersionsSize = items.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type != ItemType.Folder && i.HasVersions).Select(i => i.VersionsSize).Sum();

                root.Quota = CalculateRepositoryMaxSize(filePath, diskSpace, IsValidAdministrator(UC.CurrentUserID), root.FullSize);
                root.UploadAvailable = root.MoveIntoAvailable && (permissions.Administration || permissions.ManageItems || permissions.UploadFile || permissions.EditOthersFiles);
                return root;
            }
            private dtoFolderTreeItem CreateRootFolderTreeItem(String filePath, long idCurrentFolder, IEnumerable<RepositoryItem> items, DiskSettings diskSpace, ModuleRepository permissions)
            {
                dtoFolderTreeItem root = new dtoFolderTreeItem() { Id = 0, IsHome = true, Name = "", IsCurrent = (idCurrentFolder == 0) };
                if (items.Any(i => i.Type != ItemType.Link && i.Deleted == BaseStatusDeleted.None))
                    root.Size = items.Where(i => i.Type != ItemType.Link && i.Deleted == BaseStatusDeleted.None).Select(i => i.Size).Sum();

                if (items.Any(i => i.Deleted != BaseStatusDeleted.None))
                    root.DeletedSize = items.Where(i => i.Deleted == BaseStatusDeleted.Manual).Select(i => i.Size + i.VersionsSize).Sum();
                if (items.Any(i => i.Type == ItemType.Folder && i.Deleted == BaseStatusDeleted.None && i.DeletedSize > 0))
                    root.DeletedSize += items.Where(i => i.Type == ItemType.Folder && i.Deleted == BaseStatusDeleted.None && i.DeletedSize > 0).Select(i => i.DeletedSize).Sum();

                if (items.Any(i => i.Deleted == BaseStatusDeleted.None && i.HasVersions))
                    root.VersionsSize = items.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type != ItemType.Folder && i.HasVersions).Select(i => i.VersionsSize).Sum();

                root.Quota = CalculateRepositoryMaxSize(filePath, diskSpace, IsValidAdministrator(UC.CurrentUserID), root.FullSize);
                root.UploadAvailable = root.MoveIntoAvailable && (permissions.Administration || permissions.ManageItems || permissions.UploadFile || permissions.EditOthersFiles);
                return root;
            }
            private dtoFolderTreeItem CreateFolderTreeItem(liteRepositoryItem fItem, long idCurrentFolder,  IEnumerable<liteRepositoryItem> items, dtoFolderTreeItem father, ModuleRepository permissions)
            {
                dtoFolderTreeItem folder = new dtoFolderTreeItem() { Id = fItem.Id, Name = fItem.Name, IsCurrent = (idCurrentFolder == fItem.Id), IsInCurrentPath= (father.IsCurrent || father.IsInCurrentPath || (idCurrentFolder == fItem.Id))  };
                folder.Size = fItem.Size;
                folder.VersionsSize = fItem.VersionsSize;
                folder.DeletedSize = fItem.DeletedSize;
                folder.Quota = dtoContainerQuota.Create(father.Quota,  folder.FullSize);
                folder.UploadAvailable = folder.MoveIntoAvailable && (permissions.Administration || permissions.ManageItems || permissions.UploadFile || permissions.EditOthersFiles || fItem.AllowUpload);

                foreach (liteRepositoryItem child in items.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type == ItemType.Folder && i.IdFolder == folder.Id).OrderBy(i => i.Name).ThenBy(i => i.Id))
                {
                    folder.Children.Add(CreateFolderTreeItem(child, idCurrentFolder, items, folder, permissions));
                }

                return folder;
            }
            private dtoFolderTreeItem CreateFolderTreeItemByFathers(List<long> idFolders,RepositoryItem fItem, long idCurrentFolder,  IEnumerable<RepositoryItem> items, dtoFolderTreeItem father, ModuleRepository permissions)
            {
                dtoFolderTreeItem folder = new dtoFolderTreeItem() { Id = fItem.Id, Name = fItem.Name, IsCurrent = (idCurrentFolder == fItem.Id), IsInCurrentPath = (father.IsCurrent || father.IsInCurrentPath || (idCurrentFolder == fItem.Id)) };
                folder.Size = fItem.Size;
                folder.VersionsSize = fItem.VersionsSize;
                folder.DeletedSize = fItem.DeletedSize;
                folder.Quota = dtoContainerQuota.Create(father.Quota,  folder.FullSize);
                folder.UploadAvailable = folder.MoveIntoAvailable && (permissions.Administration || permissions.ManageItems || permissions.UploadFile || permissions.EditOthersFiles || fItem.AllowUpload);

                foreach (RepositoryItem child in items.Where(i => idFolders.Contains(i.Id) && i.Deleted == BaseStatusDeleted.None && i.Type == ItemType.Folder && i.IdFolder == folder.Id))
                {
                    folder.Children.Add(CreateFolderTreeItemByFathers(idFolders,child, idCurrentFolder, items, folder, permissions));
                }

                return folder;
            }
            public dtoFolderTreeItem FolderGetSizeInfoByFathers(long idFolder, IEnumerable<RepositoryItem> repositoryItems, liteRepositorySettings settings, ModuleRepository module, String filePath, Int32 idCommunity)
            {
                return CreateRootFolderTreeItem(filePath, idFolder, repositoryItems.Where(i => i.IdFolder == 0), settings.DiskSpace, module);
            }
        #endregion
        #region "Manage Items"
            #region "Folder Size"
                public dtoContainerQuota FolderGetHomeAvailableSize(String filePath, liteRepositorySettings settings, ModuleRepository module, RepositoryIdentifier identifier)
                {
                    dtoContainerQuota container = null;
                    try
                    {
                        IEnumerable<liteRepositoryItem> query = GetQuery(identifier);
                        long usedSize = query.Where(i => i.Deleted == BaseStatusDeleted.None).Select(i => i.Size + i.DeletedSize + i.VersionsSize).DefaultIfEmpty(0).Sum();
                        usedSize += query.Where(i => i.Deleted == BaseStatusDeleted.Manual).Select(i => i.DeletedSize).DefaultIfEmpty(0).Sum();
                        container = CalculateRepositoryMaxSize(filePath, settings, IsValidAdministrator(UC.CurrentUserID));
                        container.UsedSize = usedSize;
                    }
                    catch (Exception ex)
                    {

                    }
                    return container;
                }
                public dtoContainerQuota FolderGetHomeAvailableSize(String filePath, liteRepositorySettings settings, ModuleRepository module, RepositoryType type, Int32 idCommunity)
                {
                   return FolderGetHomeAvailableSize(filePath,settings,module,RepositoryIdentifier.Create(type,idCommunity));
                }

                #region "Repository Size"
                    private dtoContainerQuota CalculateRepositoryMaxSize(String filePath, liteRepositorySettings settings, Boolean overrideQuota, long usedSize = 0)
                    {
                        return CalculateRepositoryMaxSize(filePath, (settings != null ? settings.DiskSpace : null),overrideQuota, usedSize);
                    }
                    private dtoContainerQuota CalculateRepositoryMaxSize(String filePath, DiskSettings settings, Boolean overrideQuota, long usedSize = 0)
                    {
                        dtoContainerQuota item = new dtoContainerQuota();
                        item.UsedSize = usedSize;
                        try
                        {
                            if (!String.IsNullOrEmpty(filePath))
                                item.DiskSize = -1;
                            ulong space = 0;
                            if (!String.IsNullOrEmpty(filePath))
                            {
                                File.ContentOf.DriveFreeBytes(filePath, out space);
                                item.DiskSize = (long)space;
                            }
                            if (settings != null)
                                item.Initialize(settings, overrideQuota, usedSize);
                        }
                        catch (Exception ex)
                        {

                        }
                        return item;
                    }
                    public long GetDiskSize(String filePath)
                    {
                        long diskSize = -1;
                        try
                        {
                            ulong space = 0;
                            if (!String.IsNullOrEmpty(filePath))
                            {
                                File.ContentOf.DriveFreeBytes(filePath, out space);
                                diskSize = (long)space;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        return diskSize;
                    }
                #endregion

                public List<dtoFolderSize> GetRepositoryAvailabilityInfo(String filePath, liteRepositorySettings settings, Int32 idCurrentPerson, RepositoryType type, Int32 idCommunity, String unknownUser, Dictionary<FolderType, String> fTranslations, Dictionary<ItemType, String> iTranslations, ModuleRepository permissions, Boolean forAdmin = false, Boolean alsoHidden = false, Boolean onlyFolder = false, Boolean useCache = true)
                {
                    Boolean admin = permissions.ManageItems || permissions.Administration;
                    return GetRepositoryAvailabilityInfo(
                        filePath,
                        settings, 
                        idCurrentPerson, 
                        type, 
                        idCommunity,  
                        permissions, 
                        GetAvailableRepositoryItems(
                            settings, 
                            idCurrentPerson, 
                            type, 
                            idCommunity,  
                            unknownUser, 
                            fTranslations, 
                            iTranslations, 
                            permissions, 
                            admin, 
                            admin),
                        useCache);

                }
                public List<dtoFolderSize> GetRepositoryAvailabilityInfo(String filePath, liteRepositorySettings settings, Int32 idCurrentUser, RepositoryType type, Int32 idCommunity, ModuleRepository permissions, List<dtoDisplayRepositoryItem> treeItems, Boolean useCache = true)
                {
                    String key = CacheKeys.UserSizeOfRepository(idCurrentUser, type, idCommunity);
                    List<dtoFolderSize> fItems = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<dtoFolderSize>>(key) : null;
                    if (fItems == null || !fItems.Any() )
                    {
                        fItems = new List<dtoFolderSize>();
                        List<liteRepositoryItem> items = GetQuery(type,idCommunity).ToList();
                        fItems.Add(GetRepositoryAvailabilityInfo(filePath, (settings != null && settings.DiskSpace != null) ? settings.DiskSpace : null, items, GetAllItems(treeItems.AsQueryable(), i => (i.Type != ItemType.Folder || i.FolderType == FolderType.standard))));

                        foreach (dtoDisplayRepositoryItem folder in treeItems.Where(f => f.Type == ItemType.Folder))
                        {
                            fItems.AddRange(GetFolderAvailabilityInfo(fItems[0], fItems[0], folder, items));    
                        }
                        if (fItems != null)
                            CacheHelper.AddToCache<List<dtoFolderSize>>(key, fItems, CacheExpiration.Day);
                    }
                    return fItems;
                }

                private dtoFolderSize GetRepositoryAvailabilityInfo(String filePath, DiskSettings diskSettings, List<liteRepositoryItem> items, IEnumerable<dtoDisplayRepositoryItem> allItems)
                {
                    dtoFolderSize rFolder = new dtoFolderSize() { FolderType = FolderType.none, IdFolder = 0 };
                    long viewSize = 0;
                    long usedSize = 0;
                    long versionSize = 0;
                    long deletedSize = 0;
                    long childrenSize = allItems.Where(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type == ItemType.Folder).Select(i => i.Size).DefaultIfEmpty().Sum();

                    if (items.Any(i => i.Type != ItemType.Folder && i.Type != ItemType.Link))
                        usedSize = items.Where(i => i.Type != ItemType.Folder && i.Type != ItemType.Link).Select(i=> i.Size + i.VersionsSize).Sum();
                    if (allItems.Any(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type != ItemType.Folder && i.Type != ItemType.Link))
                        viewSize = allItems.Where(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type != ItemType.Folder && i.Type != ItemType.Link).Select(i=> i.Size).Sum();

                    rFolder.Items.Add(new FolderSizeItem() { Number = allItems.Where(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type == ItemType.Folder).Count(), Type = FolderSizeItemType.folder });
                    rFolder.Items.Add(new FolderSizeItem() { Number = allItems.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type != ItemType.Folder && i.Type != ItemType.Link).Count(), Type = FolderSizeItemType.file ,Size = viewSize});
                    viewSize += childrenSize;
                    if (allItems.Any(i => i.Deleted == BaseStatusDeleted.None && i.Type == ItemType.Link))
                        rFolder.Items.Add(new FolderSizeItem() { Number = allItems.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type == ItemType.Link).Count(), Type = FolderSizeItemType.link });

                    if (allItems.Any(i => i.Deleted != BaseStatusDeleted.None))
                    {
                        deletedSize = allItems.Where(i => i.Deleted == BaseStatusDeleted.Manual).Select(i => i.Size + i.VersionsSize).Sum();
                        rFolder.Items.Add(new FolderSizeItem()
                        {
                            Number = allItems.Where(i => i.Deleted != BaseStatusDeleted.None).Count(),
                            Type = FolderSizeItemType.deleted,
                            Size = deletedSize
                        });
                    }
                    if (allItems.Any(i => i.Deleted == BaseStatusDeleted.None && i.HasVersions))
                    {
                        versionSize = allItems.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type != ItemType.Folder && i.HasVersions).Select(i => i.VersionsSize).Sum();
                        rFolder.Items.Add(new FolderSizeItem()
                        {
                            Number = allItems.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type != ItemType.Folder && i.HasVersions).Count(),
                            Type = FolderSizeItemType.version,
                            Size = versionSize
                        });
                    }
                    if ((viewSize+versionSize+deletedSize) < usedSize)
                        rFolder.Items.Add(new FolderSizeItem() { Number = items.Where(i => !allItems.Any(it=> it.Id == i.Id) ).Count(), Type = FolderSizeItemType.unavailableItems, Size = usedSize - (viewSize + versionSize + deletedSize) });
                    
                    if (rFolder.Items.Any())
                        rFolder.Size = rFolder.Items.Select(i => i.Size).DefaultIfEmpty(0).Sum();

                    rFolder.Quota = CalculateRepositoryMaxSize(filePath, diskSettings, IsValidAdministrator(UC.CurrentUserID), rFolder.Size);

                    rFolder.UploadAvailable = (rFolder.Quota.AvailableSize == -1 || rFolder.FreeSpace > 0);
                    if (rFolder.OverSize>0)
                        rFolder.Items.Add(new FolderSizeItem() { Number = 0, Type = FolderSizeItemType.overflow, Size = rFolder.OverSize });
                   
                    if (rFolder.UploadAvailable)
                        rFolder.Items.Add(new FolderSizeItem() { Number = 0, Size = rFolder.FreeSpace, Type = FolderSizeItemType.freespace });
                    else if (rFolder.Quota.HasAllowedSpace())
                    {
                        rFolder.UploadAvailable = true;
                    }
                    rFolder.SetPercentage();
                    return rFolder;
                }

                private List<dtoFolderSize> GetFolderAvailabilityInfo(dtoFolderSize repository, dtoFolderSize father, dtoDisplayRepositoryItem folder, List<liteRepositoryItem> items)
                {
                    List<dtoFolderSize> folders = new List<dtoFolderSize>();
                    dtoFolderSize rFolder = new dtoFolderSize() { IdFolder = folder.Id, Name = folder.Name, IdentifierPath = folder.IdentifierPath, FolderType=folder.FolderType };
                    switch (folder.FolderType)
                    {
                        case FolderType.recycleBin:
                            SetRecycleBinInfo(rFolder, folder, items);
                            break;
                        case FolderType.standard:
                            SetStandardFolderInfo(rFolder, folder, items);
                            break;
                    }

                    if (rFolder.Items.Any())
                        rFolder.Size = rFolder.Items.Select(i => i.Size).Sum();

                    rFolder.Quota = dtoContainerQuota.Create(father.Quota, rFolder.Size);
                    rFolder.UploadAvailable = (rFolder.FolderType == FolderType.standard) && (rFolder.Quota.AvailableSize == -1 || rFolder.FreeSpace > 0);

                    switch (rFolder.FolderType)
                    {
                        case FolderType.standard:
                        case FolderType.recycleBin:
                            rFolder.Items.Add(new FolderSizeItem() { Number = 0, Type = FolderSizeItemType.fullSize, Size = rFolder.Size });
                            break;

                    }
                    rFolder.SetPercentage();


                    switch (folder.FolderType)
                    {
                        case FolderType.recycleBin:
                            break;
                        case FolderType.standard:
                            foreach (dtoDisplayRepositoryItem child in folder.Children.Where(f => f.Type == ItemType.Folder && f.Deleted == BaseStatusDeleted.None))
                            {
                                folders.AddRange(GetFolderAvailabilityInfo(repository,rFolder, child, items));
                            }
                            break;
                    }
                    folders.Insert(0, rFolder);
                    return folders;
                }

                private void SetRecycleBinInfo(dtoFolderSize folder,dtoDisplayRepositoryItem treeFolder, List<liteRepositoryItem> items)
                {
                    long fileSize = 0;
                    long folderSize = 0;
                    long viewSize = 0;
                    long usedSize = 0;

                    if (items.Any(i=> i.Deleted== BaseStatusDeleted.Manual))
                        usedSize = items.Where(i=> i.Deleted== BaseStatusDeleted.Manual).Select(i=> i.VersionsSize + i.Size).Sum(); 

                    if (treeFolder.Children.Any(i => i.Deleted == BaseStatusDeleted.Manual && i.Type == ItemType.Folder))
                        folderSize = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.Manual && i.Type == ItemType.Folder).Select(f => f.Size + f.VersionsSize).Sum();
                    if (treeFolder.Children.Any(i => i.Deleted == BaseStatusDeleted.Manual && i.Type != ItemType.Folder))
                        fileSize = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.Manual && i.Type != ItemType.Folder).Select(f => f.Size + f.VersionsSize).Sum();

                    folder.Items.Add(new FolderSizeItem() { Number = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.Manual && i.Type == ItemType.Folder).Count(), Type = FolderSizeItemType.folder, Size = folderSize });
                    folder.Items.Add(new FolderSizeItem() { Number = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.Manual && i.Type != ItemType.Folder && i.Type != ItemType.Link).Count(), Type = FolderSizeItemType.file, Size = fileSize });

                    viewSize = fileSize + folderSize;
                    if (items.Any(i => i.Deleted != BaseStatusDeleted.None && i.Type == ItemType.Link))
                        folder.Items.Add(new FolderSizeItem() { Number = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.Manual && i.Type == ItemType.Link).Count(), Type = FolderSizeItemType.link });

                    if (viewSize < usedSize)
                        folder.Items.Add(new FolderSizeItem() { Number = items.Where(i =>  i.Deleted == BaseStatusDeleted.Manual && !treeFolder.Children.Any(it => it.Id == i.Id)).Count(), Type = FolderSizeItemType.unavailableItems, Size = usedSize - viewSize  });
                    
                }
                private void SetStandardFolderInfo(dtoFolderSize folder, dtoDisplayRepositoryItem treeFolder, List<liteRepositoryItem> items)
                {
                    long viewSize = 0;
                    long usedSize = treeFolder.Size; 
                    long versionSize = 0;
                    long deletedSize = 0;
                    long deletedChildrenSize = 0;
                    long childrenSize = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type == ItemType.Folder).Select(i => i.Size).DefaultIfEmpty().Sum();

                    folder.Items.Add(new FolderSizeItem() { Number = treeFolder.Children.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == ItemType.Folder && c.IsUserAvailable).Count(), Type = FolderSizeItemType.folder, Size = childrenSize });
                    if (treeFolder.Children.Any(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type != ItemType.Folder && i.Type != ItemType.Link))
                        viewSize = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type != ItemType.Folder && i.Type != ItemType.Link).Select(i => i.Size).Sum();

                    folder.Items.Add(new FolderSizeItem() { Number = treeFolder.Children.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type != ItemType.Folder && c.Type != ItemType.Link).Count(), Type = FolderSizeItemType.file, Size = viewSize });
                    viewSize += childrenSize;
                    if (treeFolder.Children.Any(c => c.Deleted == BaseStatusDeleted.None && c.Type == ItemType.Link))
                        folder.Items.Add(new FolderSizeItem() { Number = treeFolder.Children.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == ItemType.Link).Count(), Type = FolderSizeItemType.link, Size = deletedSize });

                    if (treeFolder.Children.Any(i => i.Deleted != BaseStatusDeleted.None)) {
                        deletedSize = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.Manual).Select(i => i.Size + i.VersionsSize).Sum();
                        folder.Items.Add(new FolderSizeItem()
                        {
                            Number = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.Manual).Count(),
                            Type = FolderSizeItemType.deleted,
                            Size = deletedSize
                        });
                    }
                    if (treeFolder.Children.Any(i => i.Deleted == BaseStatusDeleted.None && i.HasVersions)){
                        versionSize = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type != ItemType.Folder && i.HasVersions).Select(i => i.VersionsSize).Sum();
                        folder.Items.Add(new FolderSizeItem()
                        {
                            Number = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.None && i.Type != ItemType.Folder && i.HasVersions).Count(),
                            Type = FolderSizeItemType.version,
                            Size = versionSize
                        });
                    }
                    deletedChildrenSize = treeFolder.Children.Where(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type == ItemType.Folder ).Select(i => i.DeletedSize).DefaultIfEmpty(0).Sum();
                    if (deletedChildrenSize>0)
                        folder.Items.Add(new FolderSizeItem() { Number = treeFolder.Children.Count(i => i.Deleted == BaseStatusDeleted.None && i.IsUserAvailable && i.Type == ItemType.Folder &&i.DeletedSize>0), Type = FolderSizeItemType.deletedonsubfolders, Size = deletedChildrenSize });
                    

                    if ((viewSize + versionSize + deletedSize + deletedChildrenSize) < usedSize)
                        folder.Items.Add(new FolderSizeItem() { Number = items.Where(i => i.IdFolder == folder.IdFolder && !treeFolder.Children.Any(it => it.Id == i.Id)).Count(), Type = FolderSizeItemType.unavailableItems, Size = usedSize - (viewSize + versionSize + deletedSize + deletedChildrenSize) });

                }
                private IEnumerable<dtoDisplayRepositoryItem> GetAllItems(IQueryable<dtoDisplayRepositoryItem> treeItems, Expression<Func<dtoDisplayRepositoryItem, bool>> conditions)
                {
                    return (treeItems == null) ? (IEnumerable<dtoDisplayRepositoryItem>) new List<dtoDisplayRepositoryItem>() : treeItems.Where(conditions).SelectMany(c => c.GetPlainList());
                }
                public static String FormatBytes(long bytes, Int32 decimals = 2){
                    if (bytes==0) return "0 Byte";
                    Int32 k = 1000;
                    var sizes =  new List<String>() {"Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
                    Int32 i = (Int32)Math.Floor(Math.Log(bytes) / Math.Log(k));
                    return (bytes / Math.Pow(k, i)).ToString("N" + decimals) + ' ' + sizes[i];
                  }
            #endregion
        #endregion
    }
}