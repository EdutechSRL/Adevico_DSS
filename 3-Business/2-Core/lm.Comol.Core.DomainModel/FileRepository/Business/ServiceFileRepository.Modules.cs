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

        #region Folders path
            public Dictionary<long, String> FoldersFullPathGet(RepositoryIdentifier identifier, String rootFolder)
            {
                return FoldersFullPathGet(GetQuery(identifier).Where(i => i.Deleted == BaseStatusDeleted.None && i.Type == ItemType.Folder).ToList(),rootFolder);
            }
            public Dictionary<long, String> FoldersFullPathGet(List<liteRepositoryItem> folders, String rootFolder)
            {
                Dictionary<long, String> result = new Dictionary<long, string>();
                result.Add(0, rootFolder);
                FoldersFullPathGetRecursive(rootFolder, 0, result, folders);
                return result;
            }
            private void FoldersFullPathGetRecursive(String fatherPath, long idFather, Dictionary<long, String> paths, List<liteRepositoryItem> folders)
            {
                if (!fatherPath.EndsWith("/"))
                    fatherPath += "/";
                foreach (liteRepositoryItem f in folders.Where(f => f.IdFolder == idFather))
                {
                    paths.Add(f.Id, fatherPath + f.Name + "/");
                    if (folders.Any(i => i.IdFolder == f.Id))
                        FoldersFullPathGetRecursive(paths[f.Id], f.Id, paths, folders);
                }
            }
        #endregion

        public List<dtoRepositoryItemToSelect> ItemsToSelectGet(Int32 idCurrentUser, String rootFolder, ModuleRepository module, RepositoryIdentifier identifier, Boolean adminMode, Boolean showHiddenItems, Boolean disableNotAvailableItems, List<ItemType> typesToLoad, ItemAvailability availability, List<StatisticType> displayStatistics, List<long> idRemovedItems, List<long> idSelectedItems, Boolean selectAll, Boolean removeFolder)
        {
            Dictionary<long, String> foldersPath = FoldersFullPathGet(identifier, rootFolder);

            List<dtoRepositoryItemToSelect> results = new List<dtoRepositoryItemToSelect>();
            var query = GetQuery(identifier).Where(i=> i.Deleted== BaseStatusDeleted.None && (showHiddenItems || (i.IsVisible && (i.IdCreatedBy== idCurrentUser && !i.IsVisible ))) && !i.IsInternal);
            query = query.Where(i=> (availability== ItemAvailability.ignore || (availability == i.Availability)) && typesToLoad.Contains(i.Type) && (!removeFolder || (removeFolder && i.IsFile)));

            List<liteRepositoryItem> fItems= query.ToList().Where(i=> !idRemovedItems.Contains(i.Id)).ToList();
            if (!adminMode)
            {
                Int32 idRole = (Int32)RoleTypeStandard.Guest;
                Int32 idProfileType = Manager.GetIdProfileType(idCurrentUser);
                List<liteItemAssignments> assignments = new List<liteItemAssignments>();
                switch (identifier.Type)
                {
                    case RepositoryType.Community:
                    case RepositoryType.Portal:
                        assignments = GetAssignments(fItems.Select(i => i.Id).ToList());
                        idRole = (identifier.Type == RepositoryType.Community) ? Manager.GetActiveSubscriptionIdRole(idCurrentUser, identifier.IdCommunity) : idRole;
                        break;
                }
                fItems = fItems.Where(i => HasValidAssignments(i.Id, idCurrentUser, idRole, idProfileType, assignments)).ToList();
            }
            if (idSelectedItems == null)
                idSelectedItems = new List<long>();
            results = fItems.Select(i => new dtoRepositoryItemToSelect(i, (foldersPath.ContainsKey(i.IdFolder) ? foldersPath[i.IdFolder] : ""), (!disableNotAvailableItems || (disableNotAvailableItems && i.Availability == ItemAvailability.available)), (selectAll ||  idSelectedItems.Contains(i.Id)))).ToList();
            if (displayStatistics.Any())
            {
                List<StatisticType> statistics = new List<StatisticType>();
                if (displayStatistics.Contains(StatisticType.downloads) && (adminMode || module.Administration || module.EditOthersFiles))
                    statistics.Add(StatisticType.downloads);
                if (displayStatistics.Contains(StatisticType.plays) && (adminMode || module.Administration || module.EditOthersFiles))
                    statistics.Add(StatisticType.plays);
                if (statistics.Any())
                {
                    if (statistics.Contains(StatisticType.plays))
                    {
                        Dictionary<long, long> plays = PlayStatisticsGetFull(results.Where(i => i.Type == ItemType.Multimedia || i.Type == ItemType.ScormPackage || i.Type == ItemType.VideoStreaming).Select(i => i.Id).ToList());
                        if (plays != null)
                        {
                            List<long> idItems = plays.Keys.ToList();
                            foreach (dtoRepositoryItemToSelect item in results.Where(r => idItems.Contains(r.Id)))
                            {
                                item.Plays = (plays != null && plays.ContainsKey(item.Id) ? plays[item.Id] : 0);
                            }
                        }
                    }                    
                }
            }
            return results;
        }

        public List<dtoRepositoryItemToSelect> ItemsToSelectGet(Int32 idCurrentUser, String rootFolder, ModuleRepository module, RepositoryIdentifier identifier, Boolean adminMode, List<StatisticType> displayStatistics, List<long> idSelectedItems)
        {
            Dictionary<long, String> foldersPath = FoldersFullPathGet(identifier, rootFolder);

            List<dtoRepositoryItemToSelect> results = new List<dtoRepositoryItemToSelect>();
            var query = GetQuery(identifier);
            List<liteRepositoryItem> fItems = null;
            if (idSelectedItems.Count <= maxItemsForQuery)
                fItems = query.Where(i => idSelectedItems.Contains(i.Id)).ToList();
            else
                fItems = query.ToList().Where(i => idSelectedItems.Contains(i.Id)).ToList();
          
            results = fItems.Select(i => new dtoRepositoryItemToSelect(i, (foldersPath.ContainsKey(i.IdFolder) ? foldersPath[i.IdFolder] : ""),true,true)).ToList();
            if (displayStatistics.Any())
            {
                List<StatisticType> statistics = new List<StatisticType>();
                if (displayStatistics.Contains(StatisticType.downloads) && (adminMode || module.Administration || module.EditOthersFiles))
                    statistics.Add(StatisticType.downloads);
                if (displayStatistics.Contains(StatisticType.plays) && (adminMode || module.Administration || module.EditOthersFiles))
                    statistics.Add(StatisticType.plays);
                if (statistics.Any())
                {
                    if (statistics.Contains(StatisticType.plays))
                    {
                        Dictionary<long, long> plays = PlayStatisticsGetFull(results.Where(i => i.Type == ItemType.Multimedia || i.Type == ItemType.ScormPackage || i.Type == ItemType.VideoStreaming).Select(i => i.Id).ToList());
                        if (plays != null)
                        {
                            List<long> idItems = plays.Keys.ToList();
                            foreach (dtoRepositoryItemToSelect item in results.Where(r => idItems.Contains(r.Id)))
                            {
                                item.Plays = (plays != null && plays.ContainsKey(item.Id) ? plays[item.Id] : 0);
                            }
                        }
                    }
                }
            }
            return results;
        }
        public List<dtoRepositoryItemToSelect> ItemsToSelectGet(List<dtoRepositoryItemToSelect> selectedItems, Boolean updatePath, Int32 idCurrentUser, String rootFolder, ModuleRepository module, RepositoryIdentifier identifier, Boolean adminMode, List<ItemType> typesToLoad, List<StatisticType> displayStatistics)
        {
            List<dtoRepositoryItemToSelect> results = new List<dtoRepositoryItemToSelect>();
            Dictionary<long, String> foldersPath = (updatePath ? FoldersFullPathGet(identifier, rootFolder) : null);
            var query = GetQuery(identifier);
            List<liteRepositoryItem> fItems = null;
            List<long> idSelectedItems = selectedItems.Select(i=> i.Id).ToList();
            if (idSelectedItems.Count <= maxItemsForQuery)
                fItems = query.Where(i => idSelectedItems.Contains(i.Id)).ToList().Where(i => typesToLoad.Contains(i.Type)).ToList();
            else
                fItems = query.ToList().Where(i => idSelectedItems.Contains(i.Id) && typesToLoad.Contains(i.Type)).ToList();

            List<liteRepositoryItemVersion> versions = (selectedItems.Any(s=> !s.IgnoreVersion) ? VersionsGet(identifier,selectedItems.Where(i=> !i.IgnoreVersion && i.IdVersion>0).Select(i=> i.IdVersion).ToList() ) : null);

            foreach (liteRepositoryItem item in fItems)
            {
                dtoRepositoryItemToSelect s = selectedItems.Where(i => i.Id == item.Id).FirstOrDefault();
                if (s != null) {
                    if (updatePath)
                    {
                        s.Path = (foldersPath.ContainsKey(s.IdFolder) ? foldersPath[s.IdFolder] : "");
                        s.Name = item.Name;
                        s.DisplayName = item.DisplayName;
                        s.Extension = item.Extension;
                        s.Url = item.Url;
                    }
                    if (s.IgnoreVersion)
                    {
                        s.Size = item.Size;
                        s.IdVersion = item.IdVersion;
                        s.UniqueIdVersion = item.UniqueIdVersion;
                    }
                    else{
                        liteRepositoryItemVersion version = (versions == null ? null : versions.Where(v=> v.Id==item.IdVersion).FirstOrDefault());
                        if(version!=null){
                            s.Size = version.Size;
                            s.IdVersion = version.Id;
                            s.UniqueIdVersion = version.UniqueIdVersion;
                        }
                    }
                    results.Add(s);
                }
            }
            if (displayStatistics.Any())
            {
                List<StatisticType> statistics = new List<StatisticType>();
                if (displayStatistics.Contains(StatisticType.downloads) && (adminMode || module.Administration || module.EditOthersFiles) )
                    statistics.Add(StatisticType.mydownloads);
                if (displayStatistics.Contains(StatisticType.plays) && (adminMode || module.Administration || module.EditOthersFiles))
                    statistics.Add(StatisticType.plays );
                if (statistics.Any())
                {
                    if (statistics.Contains(StatisticType.plays))
                    {
                        Dictionary<long, long> plays = PlayStatisticsGetFull(results.Where(i => i.Type == ItemType.Multimedia || i.Type == ItemType.ScormPackage || i.Type == ItemType.VideoStreaming).Select(i => i.Id).ToList());
                        if (plays != null)
                        {
                            List<long> idItems = plays.Keys.ToList();
                            foreach (dtoRepositoryItemToSelect item in results.Where(r => idItems.Contains(r.Id)))
                            {
                                item.Plays = (plays != null && plays.ContainsKey(item.Id) ? plays[item.Id] : 0);
                            }
                        }
                    }
                }
            }
            return selectedItems;
        }
        public List<dtoRepositoryItemToSelect> ItemsToSelectReorder(List<dtoRepositoryItemToSelect> items, OrderBy order, Boolean asc)
        {
            var query = items.AsQueryable();
            switch (order)
            {
                case OrderBy.date:
                    query = ((asc) ? query.OrderBy(i => i.OrderByFolder).ThenBy(i => i.DisplayDate).ThenBy(i => i.DisplayName) : query.OrderBy(i => i.OrderByFile).ThenByDescending(i => i.DisplayDate).ThenBy(i => i.DisplayName));
                    break;
                case OrderBy.size:
                    query = ((asc) ? query.OrderBy(i => i.OrderByFile).ThenBy(i => i.Size).ThenBy(i => i.DisplayName) : query.OrderBy(i => i.OrderByFile).ThenByDescending(i => i.Size).ThenBy(i => i.DisplayName));
                    break;
                default:
                    query = ((asc) ? query.OrderBy(i => i.OrderByFolder).ThenBy(i => i.DisplayName) : query.OrderBy(i => i.OrderByFolder).ThenByDescending(i => i.DisplayName));
                    break;
            }
            items = query.ToList();
            return items;
        }


        #region "Items Info"
            public void UpdateUserInfo(liteModuleLink link,  dtoDisplayObjectRepositoryItem dto, String unknownUser)
            {
                List<litePerson> users = Manager.GetLitePersons(new List<Int32>() { dto.IdCreatedBy , link.IdCreatedBy});
                dto.Owner = users.Where(u => u.Id == dto.IdCreatedBy).Select(u => u.SurnameAndName).DefaultIfEmpty(unknownUser).FirstOrDefault();
                dto.SetLinkedInfo(link.IdCreatedBy, link.CreatedOn, users.Where(u => u.Id == link.IdCreatedBy).Select(u => u.SurnameAndName).DefaultIfEmpty(unknownUser).FirstOrDefault());
            }


        #endregion
        #region "Manage Links"
            public List<ModuleActionLink> LinkItemsGetModuleAction(Boolean alwaysLastVersion, List<long> idItems, Int32 permission, Int32 idModule)
            {
                List<ModuleActionLink> links = new List<ModuleActionLink>();
                try
                {
                    List<liteRepositoryItem> files = null;
                    var query = GetQuery<liteRepositoryItem>().Where(i => i.Deleted == BaseStatusDeleted.None && i.IsFile);
                    if (idItems.Count()>maxItemsForQuery)
                        files = query.ToList().Where(i=> idItems.Contains(i.Id)).ToList();
                    else
                        files = query.Where(i=> idItems.Contains(i.Id)).ToList();

                    links = (from f in files
                                select new ModuleActionLink(permission, (Int32)ItemGetDefaultAction(f.Type)) { ModuleObject = CreateModuleObject(alwaysLastVersion,f, idModule), EditEnabled = false }).ToList();

                }
                catch (Exception ex)
                {

                }
                return links;
            }
            public ModuleActionLink CreateModuleAction(Int32 permission, Boolean alwaysLastVersion, liteRepositoryItem item, Int32 idModule)
            {
                return new ModuleActionLink(permission, (Int32)ItemGetDefaultAction(item.Type))
                {
                    ModuleObject= CreateModuleObject(alwaysLastVersion,item, idModule),EditEnabled = false};
            }
            public ModuleObject CreateModuleObject(Boolean alwaysLastVersion, liteRepositoryItem item, Int32 idModule)
            {
                ModuleObject obj = new ModuleObject();
                obj.FQN = item.GetType().FullName;
                obj.ObjectLongID = item.Id;
                obj.ObjectOwner = item;
                obj.ObjectTypeID = (Int32)item.Type;
                if (!alwaysLastVersion)
                    obj.ObjectIdVersion = item.IdVersion;
                else
                    obj.ObjectIdVersion = 0;
                obj.ServiceCode = ModuleRepository.UniqueCode;
                obj.ServiceID = idModule;
                obj.CommunityID = item.IdCommunity;
                return obj;
            }
            public ModuleRepository.ActionType ItemGetDefaultAction(ItemType type){
                switch(type){
                    case ItemType.Multimedia:
                    case ItemType.ScormPackage:
                    case ItemType.VideoStreaming:
                        return ModuleRepository.ActionType.PlayFile;
                    case ItemType.SharedDocument:
                    case ItemType.File:
                    case ItemType.Link:
                        return ModuleRepository.ActionType.DownloadFile;
                    default:
                        return ModuleRepository.ActionType.None;
                }
            }

            public List<ItemType> GetAvailableTypes(RepositoryIdentifier identifier, ModuleRepository module, Int32 idPerson)
            {
                    return GetQuery(identifier).Where(i=> i.Deleted== BaseStatusDeleted.None).Select(i => i.Type).Distinct().ToList();
            }
            
        #endregion

        #region "Add Internal Item"
            public List<dtoModuleUploadedItem> FileAddToRepository(liteRepositorySettings settings,String istance, RepositoryIdentifier identifier, litePerson person, Boolean alwaysLastVersion, List<dtoUploadedItem> itemsToAdd, Object obj, long idObject, Int32 idObjectType, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0)
            {
                List<dtoModuleUploadedItem> files = null;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (person != null )
                    {
                        DateTime date = DateTime.Now;
                        files = new List<dtoModuleUploadedItem>();
                        List<String> brothersFile = new List<String>();
                        files.AddRange(itemsToAdd.Where(i => !i.IsValid).Select(i => new dtoModuleUploadedItem() { UploadedFile = i, Error = ItemUploadError.UnableToSaveFile }).ToList());

                        Int32 idModule = Manager.GetModuleID(moduleCode);
                        Dictionary<ItemType, Boolean> defaultDownload = settings.GetDefaultAllowDownload();
                        Dictionary<ItemType, DisplayMode> defaultDisplayMode = settings.GetDefaultDisplayMode();
                        List<litePlayerSettings> players = (itemsToAdd.Any(i => i.IsValid && (i.Type == ItemType.Multimedia || i.Type == ItemType.ScormPackage || i.Type == ItemType.VideoStreaming)) ? PlayerGetSettings() : null);
                        foreach (dtoUploadedItem dto in itemsToAdd.Where(i => i.IsValid))
                        {
                            ItemUploadError error = ItemUploadError.None;
                            RepositoryItem item = null;
                            RepositoryItemVersion version = null;
                            if (ItemHasDuplicate(brothersFile, dto.Name, ItemType.File,dto.Extension))
                            {
                                dto.OriginalName = dto.DisplayName;
                                dto.Name = ItemGetSuggestedName(brothersFile, dto.Name, ItemType.File, dto.Extension);
                            }
                            try
                            {
                                if (!isInTransaction)
                                    Manager.BeginTransaction();
                                item = RepositoryItem.CreateFile(dto, defaultDownload[dto.Type],defaultDisplayMode[dto.Type], person, date, UC.IpAddress, UC.ProxyIpAddress, identifier, obj, idObject, idObjectType, idModule, moduleCode, idModuleAjaxAction, idModuleAction);
                                item.Thumbnail = dto.ThumbnailFileName;
                                if (!String.IsNullOrWhiteSpace(dto.ThumbnailFileName))
                                {
                                    item.AutoThumbnail = true;
                                    item.DisplayMode = DisplayMode.downloadOrPlayOrModal;
                                }
                                Manager.SaveOrUpdate(item);
                                switch (item.Type)
                                {
                                    case ItemType.VideoStreaming:
                                    case ItemType.ScormPackage:
                                    case ItemType.Multimedia:
                                        item.IdPlayer = (players!=null ? players.Where(p=> p.Type==item.Type && p.EnableForUse && p.EnableForPlay).OrderByDescending(p=>p.Id).Select(p=> p.Id).FirstOrDefault() : 0);
                                        break;
                                }
                                version = item.CreateFirstVersion();
                                Manager.SaveOrUpdate(version);
                                item.IdVersion = version.Id;
                                Manager.SaveOrUpdate(item);

                                switch (item.Type)
                                {
                                    case ItemType.VideoStreaming:
                                    case ItemType.ScormPackage:
                                    case ItemType.Multimedia:
                                        FileTransferAdd(item, dto, person, date);
                                        break;
                                }

                                if (!isInTransaction)
                                    Manager.Commit();

                                brothersFile.Add(dto.Name);
                                dto.Id = item.Id;
                            }
                            catch (Exception ex)
                            {
                                if (!isInTransaction)
                                    Manager.RollBack();
                                if (item != null && dto.Id == 0)
                                {
                                    item = null;
                                    version = null;
                                    error = ItemUploadError.UnableToSaveFile;
                                }
                            }
                            if (item != null)
                                files.Add(new dtoModuleUploadedItem() { Link = CreateModuleAction((Int32)ModuleRepository.Base2Permission.DownloadOrPlay, alwaysLastVersion, Manager.Get<liteRepositoryItem>(item.Id), idModule), ItemAdded = Manager.Get<liteRepositoryItem>(item.Id), VersionAdded = (version != null ? Manager.Get<liteRepositoryItemVersion>(version.Id) : null), UploadedFile = dto, Error = error });
                            else 
                                files.Add(new dtoModuleUploadedItem() { UploadedFile = dto, Error = error });
                        }
                        if (files.Any(i => i.IsValid && i.IsAdded && (i.ItemAdded.Type == ItemType.Multimedia || i.ItemAdded.Type == ItemType.VideoStreaming || i.ItemAdded.Type == ItemType.ScormPackage)))
                        {
                            FileTransferNotifyToTransferService(istance);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return files;
            }
            public List<dtoModuleUploadedItem> FileAddToRepository(liteRepositorySettings settings,String istance, RepositoryIdentifier identifier, litePerson person, Boolean alwaysLastVersion, long idFolder, dtoFolderTreeItem folderQuota, IEnumerable<RepositoryItem> rpItems, List<dtoUploadedItem> itemsToAdd, Object obj, long idObject, Int32 idObjectType, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0)
            {
                List<dtoModuleUploadedItem> files = null;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (person != null)
                    {
                        DateTime date = DateTime.Now;
                        files = new List<dtoModuleUploadedItem>();
                        List<String> brothersFile = rpItems.Where(i => i.IdFolder == idFolder && i.Type != ItemType.Link && i.Type != ItemType.Folder && i.Deleted == BaseStatusDeleted.None).Select(f => f.DisplayName).ToList();
                        RepositoryItem father = rpItems.FirstOrDefault(i => i.Id == idFolder);
                        List<RepositoryItem> fathers = (idFolder == 0) ? new List<RepositoryItem>() : GetFullReverseFathers(idFolder, rpItems);
                        files.AddRange(itemsToAdd.Where(i => !i.IsValid).Select(i => new dtoModuleUploadedItem() { UploadedFile = i, Error = ItemUploadError.UnableToSaveFile }).ToList());
                        Int32 idModule = Manager.GetModuleID(moduleCode);
                        Dictionary<ItemType, Boolean> defaultDownload = settings.GetDefaultAllowDownload();
                        Dictionary<ItemType, DisplayMode> defaultDisplayMode = settings.GetDefaultDisplayMode();

                        List<litePlayerSettings> players = (itemsToAdd.Any(i => i.IsValid && (i.Type == ItemType.Multimedia || i.Type == ItemType.ScormPackage || i.Type == ItemType.VideoStreaming)) ? PlayerGetSettings() : null);
                        foreach (dtoUploadedItem dto in itemsToAdd.Where(i => i.IsValid))
                        {
                            Boolean allowedByQuota = false;
                            ItemUploadError error = ItemUploadError.None;
                            RepositoryItem item = null;
                            RepositoryItemVersion version = null;
                            allowedByQuota = folderQuota.ValidateSpace(dto.Size, true);
                            if (allowedByQuota)
                            {
                                if (idFolder == 0 || father != null)
                                {
                                    long displayOrder = GetCurrentDisplayOrder(idFolder, rpItems);
                                    if (ItemHasDuplicate(brothersFile, dto.Name, ItemType.File, dto.Extension))
                                    {
                                        dto.OriginalName = dto.DisplayName;
                                        dto.Name = ItemGetSuggestedName(brothersFile, dto.Name, ItemType.File, dto.Extension);
                                    }
                                    try
                                    {
                                        var queryAssignments = GetQueryAssignments(identifier).Where(a => a.Deleted == BaseStatusDeleted.None && a.Inherited);
                                        if (!isInTransaction)
                                            Manager.BeginTransaction();
                                        item = RepositoryItem.CreateFile(idFolder, dto,  defaultDownload[dto.Type], defaultDisplayMode[dto.Type], person, date, UC.IpAddress, UC.ProxyIpAddress, identifier);
                                        item.DisplayOrder = displayOrder;

                                        item.Thumbnail = dto.ThumbnailFileName;
                                        if (!String.IsNullOrWhiteSpace(dto.ThumbnailFileName))
                                        {
                                            item.AutoThumbnail = true;
                                            item.DisplayMode = DisplayMode.downloadOrPlayOrModal;
                                        }
                                        switch (item.Type)
                                        {
                                            case ItemType.VideoStreaming:
                                            case ItemType.ScormPackage:
                                            case ItemType.Multimedia:
                                                item.IdPlayer = (players!=null ? players.Where(p=> p.Type==item.Type && p.EnableForUse && p.EnableForPlay).OrderByDescending(p=>p.Id).Select(p=> p.Id).FirstOrDefault() : 0);
                                                break;
                                        }

                                        Manager.SaveOrUpdate(item);
                                        version = item.CreateFirstVersion();
                                        Manager.SaveOrUpdate(version);
                                        item.IdVersion = version.Id;
                                        Manager.SaveOrUpdate(item);
                                       

                                        switch (item.Type)
                                        {
                                            case ItemType.VideoStreaming:
                                            case ItemType.ScormPackage:
                                            case ItemType.Multimedia:
                                                FileTransferAdd(item, dto, person, date);
                                                break;
                                        }

                                        if (!isInTransaction)
                                            Manager.Commit();

                                        if (!isInTransaction)
                                        {
                                            brothersFile.Add(dto.Name);
                                            dto.Id = item.Id;
                                        }

                                        folderQuota.Size += item.Size;
                                        if (fathers != null && fathers.Any())
                                        {
                                            isInTransaction = isInTransaction || isInTransaction;
                                            if (!isInTransaction)
                                                Manager.BeginTransaction();
                                            RecalculateFatherSize(fathers, item.Size, item.VersionsSize, person.Id, date);
                                            if (!isInTransaction)
                                                Manager.Commit();
                                        }


                                        long permissions = (long)ModuleRepository.Base2Permission.DownloadOrPlay;
                                        ItemAssignments assignment = AssignmentAddCommunity(item.Id, item.Repository, item.IdCommunity, false, false, permissions, person, date);
                                        if (idFolder == 0)
                                            assignment = AssignmentAddCommunity(item.Id, item.Repository, item.IdCommunity, false, true, permissions, person, date);
                                        else
                                            ApplyInheritedAssignment(item, GetQuery<ItemAssignments>().Where(a => a.Deleted == BaseStatusDeleted.None && a.Inherited), permissions, person, date);

                                        if (isInTransaction)
                                        {
                                            brothersFile.Add(dto.Name);
                                            dto.Id = item.Id;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        if (!isInTransaction)
                                            Manager.RollBack();
                                        if (item != null && dto.Id == 0)
                                            item = null;
                                    }
                                }
                                else
                                    error = ItemUploadError.UnableToAddFileToUnknownFolder;
                            }
                            else if (folderQuota.ValidateUploadSize(dto.Size))
                                error = ItemUploadError.UnavailableRepositorySpace;
                            else
                                error = ItemUploadError.InvalidFileSize;
                            if (item != null)
                                files.Add(new dtoModuleUploadedItem() { Link = CreateModuleAction((Int32)ModuleRepository.Base2Permission.DownloadOrPlay, alwaysLastVersion, Manager.Get<liteRepositoryItem>(item.Id), idModule), ItemAdded = Manager.Get<liteRepositoryItem>(item.Id), VersionAdded = (version != null ? Manager.Get<liteRepositoryItemVersion>(version.Id) : null), UploadedFile = dto, Error = error });
                            else
                                files.Add(new dtoModuleUploadedItem() { UploadedFile = dto, Error = error });

                        }
                        if (files.Any(i => i.IsValid && i.IsAdded && (i.ItemAdded.Type == ItemType.Multimedia || i.ItemAdded.Type == ItemType.VideoStreaming || i.ItemAdded.Type == ItemType.ScormPackage)))
                        {
                            FileTransferNotifyToTransferService(istance);
                        }
                        if (files.Any(i => i.IsAdded))
                        {
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.Repository(identifier));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersViewOfRepository(identifier));
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(CacheKeys.UsersSizeViewOfRepository(identifier));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return files;
            }
        #endregion
    }
}
