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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;

namespace lm.Comol.Core.BaseModules.FileRepository.Business
{
    public partial class ServiceRepository : ServiceFileRepository
    {

        #region "Move item/s"

            public liteRepositoryItem ItemMoveTo(long idItem, long idSource, long idDestination, RepositoryType type, Int32 idCommunity)
            {
                liteRepositoryItem item = null;
                try
                {
                    List<liteRepositoryItem> items = GetQuery(type, idCommunity).ToList();

                    item = items.Where(i => i.Id == idItem).FirstOrDefault();
                    litePerson person = GetValidPerson(UC.CurrentUserID);
                    if (person != null && item != null && item.Deleted== BaseStatusDeleted.None && item.IdFolder == idSource)
                    {
                        DateTime modifiedOn = DateTime.Now;
                        liteRepositoryItem sFolder = items.Where(i => i.Id == item.IdFolder && i.Deleted== BaseStatusDeleted.None).FirstOrDefault();

                        Manager.BeginTransaction();
                        if (idDestination == 0 && sFolder!=null)
                        {
                            RecalculateFatherSize(false, item, items, person.Id, modifiedOn);
                            item.IdFolder = 0;
                            if (ItemHasDuplicate(item, 0, items))
                                item.Name = ItemGetSuggestedName(item, items);
                            item.DisplayOrder = long.MaxValue - 1;
                            RecalculateDisplayOrderForFolder(idDestination, items, person.Id , modifiedOn);
                            item.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, modifiedOn);
                            Manager.SaveOrUpdate(item);
                        }
                        else if (idDestination>0)
                        {
                            liteRepositoryItem dFolder = items.Where(i => i.Id == idDestination && i.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                            if (dFolder!=null){
                                if (sFolder != null)
                                    RecalculateFatherSize(false, item, items, person.Id, modifiedOn);
                                item.IdFolder = idDestination;
                                if (ItemHasDuplicate(item, idDestination, items))
                                    item.Name = ItemGetSuggestedName(item, items);
                                RecalculateFatherSize(true, item, items, person.Id, modifiedOn);
                                item.DisplayOrder = long.MaxValue - 1;
                                RecalculateDisplayOrderForFolder(idDestination, items, person.Id, modifiedOn);
                                item.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, modifiedOn);
                                Manager.SaveOrUpdate(item);
                            }
                        }
                        
                        Manager.Commit();
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(type, idCommunity));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(type, idCommunity));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(type, idCommunity));

                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return item;
            }
            public List<liteRepositoryItem> ItemsMoveTo(List<long> idItem, long idStartFolder, long idDestinationFolder)
            {
                List<liteRepositoryItem> movedItems = new List<liteRepositoryItem>();

                return movedItems;
            }

            private void RecalculateFatherSize(Boolean add, liteRepositoryItem child, IEnumerable<liteRepositoryItem> allItems, Int32 idPerson, DateTime date)
            {
                liteRepositoryItem father = allItems.Where(i => i.Id == child.IdFolder && i.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                if (child != null && father!=null)
                {
                    father.Size = (add ? father.Size + child.Size :father.Size - child.Size) ;
                    father.VersionsSize = (add ? father.VersionsSize + child.VersionsSize : father.VersionsSize - child.VersionsSize);
                    father.DeletedSize = (add ? father.DeletedSize + child.DeletedSize : father.DeletedSize - child.DeletedSize);
                    father.UpdateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress, date);
                    Manager.SaveOrUpdate(father);
                    if (father.IdFolder > 0)
                        RecalculateUpperSize(add, father, child.Size, child.VersionsSize,child.DeletedSize ,allItems, idPerson, date);
                }
            }
            private void RecalculateUpperSize(Boolean add, liteRepositoryItem child,long size, long version, long deleted, IEnumerable<liteRepositoryItem> allItems, Int32 idPerson, DateTime date)
            {
                liteRepositoryItem father = allItems.Where(i => i.Id == child.IdFolder && i.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                if (child != null && father != null)
                {
                    father.Size = (add ? father.Size + size : father.Size - size);
                    father.VersionsSize = (add ? father.VersionsSize + version : father.VersionsSize - version);
                    father.DeletedSize = (add ? father.DeletedSize + deleted : father.DeletedSize - deleted);
                    father.UpdateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress, date);
                    Manager.SaveOrUpdate(father);
                    if (father.IdFolder > 0)
                        RecalculateUpperSize(add, father, size, version, deleted, allItems, idPerson, date);
                }
            }


        #endregion
            
        #region "Phisical Delete"
            public dtoItemToDelete ItemPhisicalDelete(long idItem, Dictionary<RepositoryType,String> basefilepaths, RepositoryType type, Int32 idCommunity)
            {
                List<dtoItemToDelete> results = ItemsPhisicalDelete(new List<long>() { idItem }, basefilepaths, type, idCommunity);
                return (results == null) ? null : results.FirstOrDefault();
            }
            public List<dtoItemToDelete> ItemsPhisicalDelete(List<long> idItems, Dictionary<RepositoryType, String> basefilepaths, RepositoryType type, Int32 idCommunity)
            {
                List<dtoItemToDelete> items = null;
                try
                {
                    litePerson person = GetValidPerson(UC.CurrentUserID);
                    List<liteRepositoryItem> repositoryItems = GetQuery(type, idCommunity).ToList();
                    List<liteRepositoryItem> toDelete = repositoryItems.Where(i => idItems.Contains(i.Id) && i.IsFile).ToList();
                    if (repositoryItems.Any() && person != null)
                    {
                        items = toDelete.Select(f => new dtoItemToDelete(f)).ToList();
                        foreach (liteRepositoryItem rItem in repositoryItems.Where(i => idItems.Contains(i.Id)&& !i.IsFile))
                        {
                            items.AddRange(GetCascadePhisicalDelete(rItem.Id,rItem, repositoryItems, toDelete, false));
                        }
                        List<long> idAvailableItems = items.Select(i => i.Id).ToList();
                        Dictionary<long, List<String>> links = GetLinkedModules(idAvailableItems);
                        if (links != null && links.Keys.Any())
                        {
                            links.Keys.ToList().ForEach(l => items.FirstOrDefault(i => i.Id == l).LinkedModules = links[l]);
                            var p = items.Where(i => i.IsAddedForCascade && !i.IsDeleteAllowed).ToList();
                            foreach (long idFather in items.Where(i => i.IsAddedForCascade && !i.IsDeleteAllowed).Select(i => i.IdCascadeFirstFather).Distinct()) {
                                items.FirstOrDefault(f => f.Id == idFather).IsDeleteAllowedFromCascade = false;
                                items.Where(f => f.IdCascadeFirstFather == idFather && f.IsAddedForCascade).ToList().ForEach(f => f.IsDeleteAllowedFromCascade = false);
                            }
                        }
                        List<long> idItemsToDelete = items.Where(i=> i.IsDeleteAllowed).Select(i=> i.Id).Distinct().ToList();
                        if (idItemsToDelete.Any())
                        {
                            List<long> idFilesToDelete = items.Where(i => idItemsToDelete.Contains(i.Id) && i.Type != ItemType.Folder && i.Type != ItemType.Link && i.Type != ItemType.RootFolder).Select(i => i.Id).Distinct().ToList();
                            Dictionary<long, List<String>> fileNames = (idFilesToDelete.Any() ? FilesGetAllVersionFilesName(idFilesToDelete) : new Dictionary<long, List<String>>());
                            List<liteFileTransfer> transfers = FileTransferGetById(idFilesToDelete);


                            if (items.Any(i => !i.HasCascadeItems && i.IsDeleteAllowed && !i.IsAddedForCascade ))
                                DeleteItems(items.Where(i => !i.HasCascadeItems && i.IsDeleteAllowed && !i.IsAddedForCascade), repositoryItems, fileNames, transfers);
                            if (items.Any(i => (i.HasCascadeItems || i.IsAddedForCascade) && i.IsDeleteAllowed))
                                DeleteCascadeItems(items.Where(i => (i.HasCascadeItems || i.IsAddedForCascade) && i.IsDeleteAllowed), repositoryItems, fileNames, transfers);
                        }
                        if (idItemsToDelete.Any() || repositoryItems.Count != idItems.Count())
                        {
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(type, idCommunity));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(type, idCommunity));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return items;
            }

            private void DeleteItems(IEnumerable<dtoItemToDelete> items, List<liteRepositoryItem> rItems, Dictionary<long, List<String>> fileNames, List<liteFileTransfer> transfers)
            {
                foreach (dtoItemToDelete item in items)
                {
                    try
                    {
                        liteRepositoryItem itemToDelete = Manager.Get<liteRepositoryItem>(item.Id);
                        if (itemToDelete != null)
                        {
                            Manager.BeginTransaction();
                            if (item.IsFile)
                            {
                                foreach (liteFileTransfer transfer in transfers.Where(t => t.IdItem == item.Id))
                                {
                                    transfer.Status = TransferStatus.Deleting;
                                    transfer.ModifiedOn = DateTime.Now;
                                }
                            }
                            Manager.DeletePhysical(itemToDelete);
                            rItems = rItems.Where(i => i.Id != item.Id).ToList();
                            RecalculateDeletedSize(rItems.Where(i => i.Id == item.IdFolder).FirstOrDefault(), rItems, true);
                            Manager.Commit();
                            item.IsDeleted = true;
                            if (fileNames.ContainsKey(item.Id))
                            {
                                lm.Comol.Core.File.Delete.Files(fileNames[item.Id]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        item.IsDeleted = false;
                    }
                }
            }
            private void DeleteCascadeItems(IEnumerable<dtoItemToDelete> items, List<liteRepositoryItem> rItems, Dictionary<long, List<String>> fileNames, List<liteFileTransfer> transfers)
            {
                foreach (var gItem in items.GroupBy(i=>i.IdCascadeFirstFather))
                {
                    
                    try
                    {
                        long idFolder = gItem.Where(i => !i.IsAddedForCascade).Select(i => i.IdFolder).FirstOrDefault();
                        List<liteRepositoryItem> itemsToDelete = rItems.Where(i => gItem.Any(g => g.Id == i.Id)).ToList();
                        if (itemsToDelete.Any())
                        {
                            List<long> idItems = itemsToDelete.Select(i=> i.Id).ToList();
                            List<long> idFiles = itemsToDelete.Where(i => i.IsFile).Select(i => i.Id).Distinct().ToList();
                            Manager.BeginTransaction();
                            if (idFiles.Any())
                            {
                                foreach (liteFileTransfer transfer in transfers.Where(t =>idFiles.Contains(t.IdItem)))
                                {
                                    transfer.Status = TransferStatus.Deleting;
                                    transfer.ModifiedOn = DateTime.Now;
                                }
                            }
                            Manager.DeletePhysicalList(itemsToDelete);
                            rItems = rItems.Where(i => !idItems.Contains(i.Id)).ToList();
                            if (idFolder>0)
                                RecalculateDeletedSize(rItems.Where(i => i.Id == idFolder).FirstOrDefault(), rItems, true);
                            Manager.Commit();
                            gItem.ToList().ForEach(i => i.IsDeleted = true);
                            idFiles.Where(i => fileNames.Keys.Contains(i)).ToList().ForEach(i => lm.Comol.Core.File.Delete.Files(fileNames[i]));
                        }
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                }
            }
            private List<dtoItemToDelete> GetCascadePhisicalDelete(long idFirstFather,liteRepositoryItem folder, IEnumerable<liteRepositoryItem> allItems, List<liteRepositoryItem> toDelete, Boolean cascade)
            {
                List<dtoItemToDelete> results = new List<dtoItemToDelete>();
                List<liteRepositoryItem> children = allItems.Where(i => i.IdFolder == folder.Id && i.IsFile).ToList();
                toDelete.AddRange(children);
                results.AddRange(children.Select(c => new dtoItemToDelete(c) { IsAddedForCascade = true, IdCascadeFirstFather = (cascade) ? idFirstFather : folder.Id }).ToList());

                foreach (liteRepositoryItem child in allItems.Where(i => i.IdFolder == folder.Id && !i.IsFile))
                {
                    results.AddRange(GetCascadePhisicalDelete(idFirstFather, child, allItems, toDelete, true));
                }
                Boolean hasCascadeItems = results.Any();
                results.Add(new dtoItemToDelete(folder) { HasCascadeItems = hasCascadeItems, IsAddedForCascade = cascade, IdCascadeFirstFather = idFirstFather });
                toDelete.Add(folder);
               
                
                return results;
            }

            #region "Repository"
                public List<dtoItemToDelete> CommunityRepositoryPhisicalDelete(Int32 idCommunity, String basePath,String thumbnailPath){
                    return RepositoryPhisicalDelete(RepositoryIdentifier.Create( RepositoryType.Community,idCommunity),basePath,thumbnailPath);
                }
                public List<dtoItemToDelete> RepositoryPhisicalDelete(RepositoryIdentifier identifier, String basePath,String thumbnailPath){
                    List<dtoItemToDelete> itemsToDelete = new List<dtoItemToDelete>();
                    try
                    {
                        List<String> filesToDelete = new List<string>();

                        List<liteRepositoryItem> items = GetQuery(identifier).Where(i=>!i.IsInternal).ToList();
                        List<liteRepositoryItemVersion> versions = GetQueryVersions(identifier).ToList();
                        versions = versions.Where(v=> items.Any(i=>i.Id== v.IdItem)).ToList();
                        List<ItemAssignments> assignments = GetQueryAssignments(identifier).ToList();
                        List<liteFileTransfer> mTransfer = FileTransferGetById(items.Where(i => i.Type == ItemType.ScormPackage || i.Type == ItemType.Multimedia).Select(i => i.Id).ToList());

                        Manager.BeginTransaction();
                        Manager.DeletePhysicalList(assignments);
                        Manager.DeletePhysicalList(mTransfer);
                        foreach (liteRepositoryItem i in items.Where(v=> v.Type== ItemType.Multimedia))
                        {
                            List<MultimediaFileObject> mObjects = (from m in Manager.GetIQ<MultimediaFileObject>() where m.IdItem== i.Id select m).ToList();
                            if (mObjects!=null && mObjects.Any())
                                Manager.DeletePhysicalList(mObjects);
                        }
                        foreach (liteRepositoryItem i in items.Where(v=> v.Type== ItemType.ScormPackage))
                        {
                            List<lm.Comol.Core.FileRepository.Domain.ScormSettings.ScormPackageSettings> mObjects = (from m in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.ScormSettings.ScormPackageSettings>() where m.IdItem == i.Id select m).ToList();
                            if (mObjects!=null && mObjects.Any())
                                Manager.DeletePhysicalList(mObjects);
                        }
                        String path = GetItemDiskPath(basePath, identifier);
                        String pathThumbnail = GetItemDiskPath(thumbnailPath, identifier);

                        filesToDelete.AddRange(versions.Where(v => v.Type != ItemType.Folder && v.Type != ItemType.Link).Select(v => System.IO.Path.Combine(path, v.UniqueIdVersion.ToString() + v.Extension)).ToList());
                        filesToDelete.AddRange(versions.Where(v => !String.IsNullOrEmpty(v.Thumbnail)).Select(v => System.IO.Path.Combine(pathThumbnail, v.Thumbnail)).ToList());

                        itemsToDelete = items.Select(i => new dtoItemToDelete(i)).ToList();
                        Manager.DeletePhysicalList(versions);
                        Manager.DeletePhysicalList(items);
                        Manager.Commit();
                        if (filesToDelete.Any())
                            lm.Comol.Core.File.Delete.Files(filesToDelete);
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(identifier));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(identifier));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(identifier));
                    }
                    catch(Exception ex){
                        Manager.RollBack();
                        itemsToDelete = null;
                    }
                    return itemsToDelete;
                }
            #endregion
        #endregion

            #region "Add files"
            public List<dtoModuleUploadedItem> FileAddToInternalRepository(liteRepositorySettings settings, String istance, RepositoryIdentifier identifier, litePerson person,Boolean alwaysLastVersion,  List<dtoUploadedItem> itemsToAdd, Object obj, long idObject, Int32 idObjectType, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0)
            {
                List<dtoModuleUploadedItem> files = null;
                ThumbnailsCreate(settings, itemsToAdd);
                files = FileAddToRepository(settings, istance, identifier, person, alwaysLastVersion, itemsToAdd, obj, idObject, idObjectType, moduleCode, idModuleAjaxAction, idModuleAction);
                if (files == null)
                {
                    itemsToAdd.Where(i => i.IsValid).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.SavedFullPath));
                    itemsToAdd.Where(i => i.IsValid && i.HasThumbnail).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.ThumbnailFullPath));
                }
                else if (files.Any(f => !f.IsAdded))
                {
                    files.Where(f => !f.IsAdded).ToList().ForEach(f => lm.Comol.Core.File.Delete.File(f.UploadedFile.SavedFullPath));
                    files.Where(f => !f.IsAdded && f.UploadedFile.HasThumbnail).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.UploadedFile.ThumbnailFullPath));
                }
                return files;
            }

            public List<dtoModuleUploadedItem> FileAddToRepository(liteRepositorySettings settings, String istance, RepositoryIdentifier identifier, litePerson person, Boolean alwaysLastVersion, ModuleRepository module, String filePath,long idFolder, List<dtoUploadedItem> itemsToAdd, Object obj, long idObject, Int32 idObjectType, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0)
            {
                List<dtoModuleUploadedItem> files = null;
                List<RepositoryItem> repositoryItems = GetFullQuery(identifier).ToList();
                dtoFolderTreeItem dFolder = FolderGetSizeInfoByFathers(idFolder, repositoryItems, settings, module, filePath, identifier.IdCommunity);
                if (dFolder != null)
                {
                    ThumbnailsCreate(settings,itemsToAdd);
                    files = FileAddToRepository(settings, istance, identifier, person, alwaysLastVersion, idFolder, dFolder, repositoryItems, itemsToAdd, obj, idObject, idObjectType, moduleCode, idModuleAjaxAction, idModuleAction);
                }
                if(files==null){
                    itemsToAdd.Where(i => i.IsValid).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.SavedFullPath));
                    itemsToAdd.Where(i => i.IsValid && i.HasThumbnail).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.ThumbnailFullPath));
                }
                else if (files.Any(f => !f.IsAdded)){
                    files.Where(f => !f.IsAdded).ToList().ForEach(f => lm.Comol.Core.File.Delete.File(f.UploadedFile.SavedFullPath));
                    files.Where(f => !f.IsAdded && f.UploadedFile.HasThumbnail).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.UploadedFile.ThumbnailFullPath));
                }
                return files;
            }

            public List<dtoCreatedItem> FileAddToRepository(liteRepositorySettings settings, ModuleRepository module, String filePath, String istance, long idFolder, List<dtoUploadedItem> itemsToAdd, RepositoryType type, Int32 idCommunity)
            {
                List<dtoCreatedItem> files= null;
                List<RepositoryItem> repositoryItems =  GetFullQuery(type, idCommunity).ToList();
                dtoFolderTreeItem dFolder = FolderGetSizeInfoByFathers(idFolder, repositoryItems, settings, module, filePath, idCommunity);
                if (dFolder != null)
                {
                    ThumbnailsCreate(settings,itemsToAdd);
                    files = FileAddToRepository(settings, dFolder, istance, idFolder, repositoryItems, itemsToAdd, type, idCommunity);
                }
                if(files==null){
                    itemsToAdd.Where(i => i.IsValid).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.SavedFullPath));
                    itemsToAdd.Where(i => i.IsValid && i.HasThumbnail).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.ThumbnailFullPath));
                }
                else if (files.Any(f => !f.IsAdded)){
                    files.Where(f => !f.IsAdded).ToList().ForEach(f => lm.Comol.Core.File.Delete.File(f.ToAdd.SavedFullPath));
                    files.Where(f => !f.IsAdded && f.ToAdd.HasThumbnail).ToList().ForEach(i => lm.Comol.Core.File.Delete.File(i.ToAdd.ThumbnailFullPath));
                }
                return files;
            }
            public dtoCreatedItem FileAddVersion(liteRepositorySettings settings, ModuleRepository module, String filePath, String istance, long idItem, dtoUploadedItem version)
            {
                dtoCreatedItem result = null;

                if (version.IsValid)
                {
                    RepositoryItem item = Manager.Get<RepositoryItem>(idItem);
                    if (item != null && item.Type != ItemType.Folder)
                    {
                        List<RepositoryItem> repositoryItems = GetFullQuery(item.Repository.Type, item.Repository.IdCommunity).ToList();
                        dtoFolderTreeItem dFolder = FolderGetSizeInfoByFathers(item.IdFolder, repositoryItems, settings, module, filePath, item.Repository.IdCommunity);
                        if (dFolder != null)
                            result = FileAddVersion(settings,item, version, dFolder, istance, repositoryItems);
                    }
                    else
                        result = new dtoCreatedItem() { ToAdd = version, Error = ItemUploadError.UnableToFindFile };
                }
                else
                    result = new dtoCreatedItem() { ToAdd = version, Error = ItemUploadError.UnableToSaveVersion };

                if (result == null && version.IsValid || (result != null && result.IsValid && !result.IsAdded))
                {
                    lm.Comol.Core.File.Delete.File(version.SavedFullPath);
                    if (version.HasThumbnail)
                        lm.Comol.Core.File.Delete.File(version.ThumbnailFullPath);
                }
                return result;
            }
            public dtoContainerQuota GetFolderQuota(String repositoryPath, long idFolder, RepositoryIdentifier identifier)
            {
                liteRepositorySettings settings = SettingsGetByRepositoryIdentifier(identifier);
                ModuleRepository module = GetPermissions(identifier, UC.CurrentUserID);
                return GetFolderQuota(repositoryPath, idFolder, settings, module, identifier.Type, identifier.IdCommunity);
            }
            public dtoContainerQuota GetFolderQuota(String repositoryPath,long idFolder, RepositoryType type, Int32 idRepositoryCommunity)
            {
                liteRepositorySettings settings = SettingsGetDefault( type, idRepositoryCommunity);
                ModuleRepository module = GetPermissions(type, idRepositoryCommunity,  UC.CurrentUserID);
                return GetFolderQuota(repositoryPath,idFolder, settings, module, type, idRepositoryCommunity);
            }
            public dtoContainerQuota GetFolderQuota(String repositoryPath, long idFolder, liteRepositorySettings settings, ModuleRepository module, RepositoryType type, Int32 idRepositoryCommunity)
            {
                List<RepositoryItem> repositoryItems = GetFullQuery(type, idRepositoryCommunity).ToList();
                dtoFolderTreeItem dFolder = FolderGetSizeInfoByFathers(idFolder, repositoryItems, settings, module, repositoryPath, idRepositoryCommunity);
                return (dFolder == null) ? null : dFolder.Quota;
            }
        #endregion

        #region "Thumbnail Manage"

              private void ThumbnailsCreate(liteRepositorySettings settings, List<dtoUploadedItem> itemsToAdd)
            {
                if (! String.IsNullOrWhiteSpace(settings.AutoThumbnailForExtension)){
                    List<String> extensions = settings.AutoThumbnailForExtension.Split(',').Select(e=> (e.StartsWith(".") ? e : "." + e)).ToList();
                    foreach (var f in itemsToAdd.GroupBy(i => i.Extension).Where(e => !String.IsNullOrEmpty(e.Key) && extensions.Contains(e.Key.ToLower())))
                    {
                        foreach (dtoUploadedItem item in f.ToList())
                        {
                            item.ThumbnailFileName = ThumbnailCreateToDisk(ThumbnailGetItemIdentifier(item),item.SavedFullPath, item.ThumbnailPath, settings.AutoThumbnailWidth, settings.AutoThumbnailHeight);
                            item.AutoThumbnail = !(String.IsNullOrWhiteSpace(item.ThumbnailFileName));
                        }
                    }
                }
            }
            public void ThumbnailsCreate(liteRepositorySettings settings,Guid itemUniqueId, dtoUploadedItem version)
            {
                if (!String.IsNullOrWhiteSpace(settings.AutoThumbnailForExtension))
                {
                    version.ThumbnailFileName = ThumbnailCreateToDisk(ThumbnailGetItemIdentifier(itemUniqueId, version.UniqueId), version.SavedFullPath, version.ThumbnailPath, settings.AutoThumbnailWidth, settings.AutoThumbnailHeight);
                    version.AutoThumbnail = !(String.IsNullOrWhiteSpace(version.ThumbnailFileName));
                }
            }
            public String ThumbnailCreateToDisk(String itemIdentifier, String sourceFile, String destinationFolder, int width, int height)
            {
                String thumbnail = "";
                try
                {
                    if (width <= 0)
                        width = 200;
                    if (height <= 0)
                        height = 200;
                    String destinationFile = "thumb_" + Guid.NewGuid().ToString() +".jpg";
                    if (!lm.Comol.Core.File.Exists.Directory(destinationFolder))
                        lm.Comol.Core.File.Create.Directory(destinationFolder);
                    using (Bitmap srcBmp = new Bitmap(sourceFile))
                    {
                        Bitmap target = null;
                        SizeF newSize = new SizeF(srcBmp.Width, srcBmp.Height);
                        if (srcBmp.Height > height || srcBmp.Width > width)
                        {
                            float ratio = srcBmp.Width / srcBmp.Height;
                            newSize = new SizeF(width, height * ratio);
                            target = new Bitmap((int)newSize.Width, (int)newSize.Height);
                        }
                        else
                        {
                            target = srcBmp;
                        }
                        switch (srcBmp.PixelFormat)
                        {
                            case PixelFormat.Format1bppIndexed:
                            case PixelFormat.Format4bppIndexed:
                            case PixelFormat.Format8bppIndexed:
                                using (System.IO.Stream stream = new System.IO.MemoryStream())
                                {
                                    srcBmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    target = new Bitmap(stream);
                                    thumbnail = ThumbnailCreateToDisk(srcBmp, target, newSize, destinationFile, destinationFolder);
                                }

                                break;
                            default:
                                thumbnail = ThumbnailCreateToDisk(srcBmp, target, newSize, destinationFile, destinationFolder);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return thumbnail;
            }
            private String ThumbnailCreateToDisk(Bitmap srcBmp, Bitmap target, SizeF newSize, String destinationFile, String destinationFolder)
            {
                using (Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);

                    if (lm.Comol.Core.File.Create.Image(target, destinationFolder + destinationFile, ImageFormat.Jpeg) == lm.Comol.Core.File.FileMessage.FileCreated)
                        return destinationFile;
                }
                return "";
            }
            public String ThumbnailGetItemIdentifier(dtoUploadedItem item)
            {
                return ThumbnailGetItemIdentifier(item.UniqueId, item.UniqueId);
            }
            public String ThumbnailGetItemIdentifier(Guid item, Guid version)
            {
                return item.ToString() + "-" + version.ToString();
            }
            public String GetItemDiskPath(String path,RepositoryIdentifier identifier)
            {
                return System.IO.Path.Combine(path, identifier.IdCommunity.ToString());
            }
            public String GetItemDiskFullPath(String baseFilePath, liteRepositoryItem item)
            {
                switch (item.Type)
                {
                    case ItemType.Link:
                    case ItemType.Folder:
                    case ItemType.None:
                    case ItemType.RootFolder:
                        return "";
                    default:
                        return System.IO.Path.Combine(GetItemDiskPath(baseFilePath, item.Repository), item.UniqueId.ToString() + item.Extension);
                }
            }
            public String GetItemDiskFullPath(String baseFilePath, RepositoryItem item)
            {
                switch (item.Type)
                {
                    case ItemType.Link:
                    case ItemType.Folder:
                    case ItemType.None:
                    case ItemType.RootFolder:
                        return "";
                    default:
                        return System.IO.Path.Combine(GetItemDiskPath(baseFilePath, item.Repository), item.UniqueId.ToString() + item.Extension);
                }
            }
            public String GetVersionDiskFullPath(String baseFilePath, liteRepositoryItemVersion version)
            {
                switch (version.Type)
                {
                    case ItemType.Link:
                    case ItemType.Folder:
                    case ItemType.None:
                    case ItemType.RootFolder:
                        return "";
                    default:
                        return System.IO.Path.Combine(GetItemDiskPath(baseFilePath, version.Repository), version.UniqueIdVersion.ToString() + version.Extension);
                }
            }

            public String GetItemThumbnailFullPath(String baseThumbnailPath, liteRepositoryItem item)
            {
                if (item.HasThumbnail)
                    return System.IO.Path.Combine(GetItemDiskPath(baseThumbnailPath, item.Repository), item.Thumbnail);
                else
                    return "";
            }
            public String GetVersionThumbnailFullPath(String baseThumbnailPath, liteRepositoryItemVersion version)
            {
                if (version.HasThumbnail)
                    return System.IO.Path.Combine(GetItemDiskPath(baseThumbnailPath, version.Repository), version.Thumbnail);
                else
                    return "";
            }
        #endregion

        #region "Manage versions"
            public List<dtoDisplayVersionItem> ItemGetVersions(Boolean editMode, dtoDisplayRepositoryItem item,String unknownUser)
            {
                List<dtoDisplayVersionItem> versions = new List<dtoDisplayVersionItem>();
                try
                {
                    versions = (from v in Manager.GetIQ<liteRepositoryItemVersion>()
                               where v.IdItem == item.Id
                               select v).ToList().OrderByDescending(v=>v.CreatedOn).Select(v =>
                                                             new dtoDisplayVersionItem(v) { Repository= item.Repository }).ToList();
                    if (versions.Any())
                    {
                        List<Int32> idUsers = versions.Select(v=> v.IdCreatedBy).Distinct().ToList();
                        idUsers.AddRange(versions.Select(v => v.IdModifiedBy).Distinct().ToList());
                        List<litePerson> persons = Manager.GetLitePersons(idUsers.Where(i=> i>0).Distinct().ToList());
                        long displayOrder = 0;
                        foreach (dtoDisplayVersionItem v in versions.OrderBy(v=> v.Number).ThenBy(v=>v.Id)){
                            v.Author = persons.Where(p => p.Id == v.IdCreatedBy).Select(p => p.SurnameAndName).DefaultIfEmpty(unknownUser).FirstOrDefault();
                            if (v.ModifiedOn.HasValue)
                                v.ModifyBy = persons.Where(p => p.Id == v.IdModifiedBy).Select(p => p.SurnameAndName).DefaultIfEmpty(unknownUser).FirstOrDefault();
                            v.DisplayOrder = displayOrder;
                            v.Permissions.Download = item.Permissions.Download && (!v.IsDeleted || (v.IsDeleted && (item.Permissions.RemoveVersion || item.Permissions.SetVersion || item.Permissions.AddVersion)));
                            v.Permissions.Play = item.Permissions.Play && (!v.IsDeleted || (v.IsDeleted && (item.Permissions.RemoveVersion || item.Permissions.SetVersion || item.Permissions.AddVersion)));
                            v.Permissions.SetActive = !v.IsActive && item.Permissions.SetVersion;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                return versions;
            }
        
            public RepositoryItemVersion VersionSetActive(long idVersion)
            {
                RepositoryItemVersion version = null;
                try
                {
                    version = Manager.Get<RepositoryItemVersion>(idVersion);
                    RepositoryItem file = (version == null ? null : version.File);
                    litePerson person = GetValidPerson(UC.CurrentUserID);
                    if (version != null && person != null && !version.IsActive && file != null)
                    {
                        Manager.BeginTransaction();
                        DateTime date = DateTime.Now;
                        switch (version.Type)
                        {
                            case ItemType.File:
                            case ItemType.Multimedia:
                            case ItemType.ScormPackage:
                                List<RepositoryItem> repositoryItems = GetFullQuery(file.Repository).ToList();
                                List<RepositoryItem> fathers = (file.IdFolder == 0) ? new List<RepositoryItem>() : GetFullReverseFathers(file.IdFolder, repositoryItems);
                                file.Size= version.Size;
                                if (fathers != null && fathers.Any())
                                {
                                    RecalculateFatherSize(fathers, file.Size, file.VersionsSize, person.Id, date);
                                }
                                Manager.SaveOrUpdate(file);
                                break;
                        }
                        version.IsActive = true;
                        version.Status = ItemStatus.Active;
                        version.Deleted = BaseStatusDeleted.None;
                      
                        file.Thumbnail = version.Thumbnail;
                        file.AutoThumbnail = version.AutoThumbnail;
                       
                        file.IdVersion = version.Id;
                        file.UniqueIdVersion = version.UniqueIdVersion;
                        file.DisplayMode = version.DisplayMode;
                        file.Availability = version.Availability;

                        Manager.SaveOrUpdate(file);
                        List<RepositoryItemVersion> versions = (from v in Manager.GetIQ<RepositoryItemVersion>()
                                                                where v.File != null && v.File.Id == file.Id
                                                                    && v.Status == ItemStatus.Active && v.Id != version.Id
                                                                select v).ToList();


                        if (versions.Any())
                        {
                            foreach (RepositoryItemVersion v in versions)
                            {
                                v.Status = ItemStatus.Replaced;
                                v.IsActive = false;
                                v.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                            }
                            Manager.SaveOrUpdateList(versions);
                        }
                        Manager.SaveOrUpdate(version);
                        Manager.Commit();
                        RefreshLiteItem(version.File.Id,version.Id);

                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(file.Repository));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(file.Repository));
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(file.Repository));
                    }
                }
                catch (Exception ex)
                {
                    version = null;
                }
                return version;
            }
        #endregion
    }
}