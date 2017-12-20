using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.FileRepository.Business;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.BaseModules.FileRepository.Domain;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public class RepositoryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private ServiceRepository service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewRepository View
            {
                get { return (IViewRepository)base.View; }
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
            public Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleRepository.UniqueCode);
                    return currentIdModule;
                }
            }
            public RepositoryPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RepositoryPresenter(iApplicationContext oContext, IViewRepository view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(RepositoryType type, long idFolder, String path, FolderType folderType, Int32 idCommunity = -1)
        {
            View.RepositoryType = type;
            View.RepositoryIdCommunity = idCommunity;
            View.PageIdentifier = Guid.NewGuid();
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.IsInitialized = true;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.None;
                Int32 idCurrentUser = UserContext.CurrentUserID;
                liteCommunity community = null;
                switch (type)
                {
                    case RepositoryType.Community:
                        idCommunity = (idCommunity == -1) ? UserContext.CurrentCommunityID : idCommunity;
                        community = CurrentManager.GetLiteCommunity(idCommunity);
                        break;
                    default:
                        idCommunity = 0;
                        break;
                }
                View.RepositoryIdentifier = CacheKeys.RepositoryIdentifierKey(type, idCommunity);
                View.RepositoryIdCommunity = idCommunity;
   
                liteRepositorySettings settings = Service.SettingsGetDefault( type, idCommunity);
                SetPageTitle(settings, type, idCommunity);
                if (type == RepositoryType.Community && (community == null || community.Id == 0))
                {
                    View.DisplayUnknownCommunity();
                    uAction = ModuleRepository.ActionType.UnableToLoadRepository;
                }
                else
                {
                    ModuleRepository module = Service.GetPermissions(type, idCommunity, idCurrentUser);
                    if (module.ViewRepository)
                    {
                        InitializeView(type, idFolder, path, folderType, settings, module, idCommunity, idCurrentUser, View.PreloadOrderBy, View.PreloadAscending);
                        uAction = ModuleRepository.ActionType.LoadRepository;
                    }
                    else
                    {
                        View.DisplayNoPermission(idCommunity, Service.GetIdModule());
                        uAction = ModuleRepository.ActionType.NoPermission;
                    }
                }
                View.SendUserAction(idCommunity, Service.GetIdModule(), uAction);
            }
        }
        public void InitViewFromCookies(RepositoryType type, long idFolder, String path, FolderType folderType, Int32 idCommunity = -1)
        {
            Int32 idCurrentUser = UserContext.CurrentUserID;
            liteCommunity community = null;
            switch (type)
            {
                case RepositoryType.Community:
                    idCommunity = (idCommunity == -1) ? UserContext.CurrentCommunityID : idCommunity;
                    community = CurrentManager.GetLiteCommunity(idCommunity);
                    break;
                case RepositoryType.Portal:
                    idCommunity = 0;
                    break;
            }
            RepositoryIdentifier identifier = new RepositoryIdentifier();
            identifier.IdCommunity=idCommunity;
            identifier.Type=type;
            cookieRepository cookie = View.GetRepositoryCookie(identifier);
            if (cookie == null)
                InitView(type, idFolder, path, folderType, idCommunity);
            else
                View.RedirectToUrl(RootObject.RepositoryItems(identifier.Type, identifier.IdCommunity,  0, cookie.IdFolder, cookie.Type, cookie.ItemsOrderBy, cookie.Ascending, cookie.IdentifierPath));
        }

        #region "Initialize Page"
            private void SetPageTitle(liteRepositorySettings settings, RepositoryType type, Int32 idCommunity = -1)
            {
                switch (type)
                {
                    case RepositoryType.Portal:
                        View.SetTitle(type, "", (UserContext.CurrentCommunityID == 0), (settings != null));
                        break;
                    case RepositoryType.Community:
                        String name = (idCommunity == -1) ? "" : CurrentManager.GetCommunityName(idCommunity);
                        View.SetTitle(type, name, (UserContext.CurrentCommunityID == idCommunity), (settings != null));
                        break;
                }
            }

            private void InitializeView(RepositoryType type, long idFolder, String path, FolderType folderType, liteRepositorySettings settings, ModuleRepository module, Int32 idCommunity = -1,  Int32 idCurrentUser = -1, OrderBy orderBy = OrderBy.name, Boolean? ascending = null)
            {
                Boolean admin =  module.ManageItems || module.Administration;
                List<dtoDisplayRepositoryItem> items = 
                    Service.GetAvailableRepositoryItems(settings, idCurrentUser, type, idCommunity,
                        View.GetUnknownUserName(), View.GetFolderTypeTranslation(),View.GetTypesTranslations(),
                        module, admin, admin);

                List<dtoNodeItem> folders = new List<dtoNodeItem>();
                dtoDisplayRepositoryItem currentFolder = Service.GetFolder(items, idFolder, path, folderType);
                View.CurrentPath = "/";
                if (currentFolder == null)
                {
                    idFolder = 0;
                    folderType = FolderType.standard;
                    View.CurrentFolderIdentifierPath = "";
                   
                }
                else
                {
                    View.CurrentPath = currentFolder.FullPath;
                    View.CurrentFolderIdentifierPath = currentFolder.IdentifierPath;
                }
                View.IdCurrentFolder = idFolder;
                View.CurrentFolderType = folderType;
                if (items == null)
                    return;

                LoadOrderByItems(items, currentFolder, ref orderBy, ref ascending);
                View.CurrentAscending = (ascending.HasValue)? ascending.Value : true;
                View.CurrentOrderBy = orderBy;
                if (currentFolder == null)
                    WriteCookie(0, "", FolderType.standard, orderBy, ascending.Value, type, idCommunity);
                else
                    WriteCookie(currentFolder.Id, currentFolder.IdentifierPath, currentFolder.FolderType, orderBy, ascending.Value, type, idCommunity);

                List<ViewOption> availableOptions = InitializeViewOptions(type, settings, module, Service.HasFolders(items), idCommunity);
                if (items != null && (availableOptions.Contains(ViewOption.Tree) || availableOptions.Contains(ViewOption.FolderPath) || availableOptions.Contains(ViewOption.AvailableSpace) && (admin || module.EditOthersFiles || (currentFolder != null && currentFolder.AllowUpload))))
                {
                    List<FolderType> fTypes = new List<FolderType>() {FolderType.standard,  FolderType.recycleBin};
                    items.ForEach(i => folders.AddRange(i.GetFoldersNodes(fTypes, currentFolder, type, idCommunity,  true, false)));
                }
                InitializeAvailableSpace(availableOptions.Contains(ViewOption.AvailableSpace),settings, idCurrentUser, type, idCommunity,  module, items, currentFolder);
                if (items == null)
                    return;
                if (availableOptions.Contains(ViewOption.Tree) || availableOptions.Contains(ViewOption.FolderPath))
                {
                    if (availableOptions.Contains(ViewOption.Tree))
                        View.InitializeTree(currentFolder, items.Where(i => i.Type == ItemType.Folder).ToList(), RepositoryIdentifier.Create((idCommunity>0)? RepositoryType.Community : RepositoryType.Portal,idCommunity) , View.RepositoryIdentifier);
                    if (availableOptions.Contains(ViewOption.FolderPath))
                        InitializeFolderPath(currentFolder, type, idCommunity,  orderBy, ascending);
                    else
                        View.HideBreadCrumb();
                }
                LoadFolderCommands(idFolder, settings, currentFolder, items,module, type, idCommunity);
                LoadItems(settings, RepositoryIdentifier.Create(type, idCommunity), items, currentFolder, orderBy, (ascending.HasValue) ? ascending.Value : true);
            }

            private List<ViewOption> InitializeViewOptions(RepositoryType type, liteRepositorySettings settings, ModuleRepository module, Boolean hasFolders, Int32 idCommunity = -1){
                Boolean isGeneric = settings.IsGenericFor(type);;
                PresetType currentSet = PresetType.Standard;
                Dictionary<PresetType, List<ViewOption>> availableOptions = new Dictionary<PresetType,List<ViewOption>>();
                Dictionary<PresetType, List<ViewOption>> activeOptions = new Dictionary<PresetType,List<ViewOption>>();
                Dictionary<PresetType, TreeViewOption> treeAvailability = new Dictionary<PresetType,TreeViewOption>();
                List<PresetType> sets = null;
                if (!settings.Views.Any(v => v.Deleted == BaseStatusDeleted.None)){
                    availableOptions[currentSet] = liteViewSettings.GetListOfDefaultAvaliableOptions(PresetType.Standard);
                    activeOptions[currentSet] = liteViewSettings.GetListOfDefaultAvaliableOptions(PresetType.Standard);
                    treeAvailability[currentSet] = liteViewSettings.GetDefaultTree(PresetType.Standard);
                }
                else {
                    treeAvailability = settings.Views.Where(v=> v.Deleted== BaseStatusDeleted.None).ToDictionary(v=> v.Type, v=> v.Tree);
                    availableOptions = settings.Views.Where(v=> v.Deleted== BaseStatusDeleted.None).ToDictionary(v=> v.Type, v=> liteViewSettings.FromFlagToList(v.AvailableOptions));
                    activeOptions = settings.Views.Where(v=> v.Deleted== BaseStatusDeleted.None).ToDictionary(v=> v.Type,  v=> liteViewSettings.FromFlagToList(v.ActiveOptions));

                    liteViewSettings vSettings = settings.Views.FirstOrDefault(v => v.Deleted == BaseStatusDeleted.None && v.Type == settings.DefaultView);
                    currentSet = (vSettings == null) ? settings.Views.FirstOrDefault().Type :  vSettings.Type;
                    sets = settings.Views.Where(v => v.Deleted == BaseStatusDeleted.None).Select(v => v.Type).ToList();
                }
                foreach(var item in availableOptions){
                    if (item.Value.Contains(ViewOption.Tree) && treeAvailability[item.Key]== TreeViewOption.Never)
                        item.Value.Remove(ViewOption.Tree);
                }
                foreach(var item in activeOptions){
                    if (item.Value.Contains(ViewOption.Tree) && (treeAvailability[item.Key]== TreeViewOption.Never || (treeAvailability[item.Key]== TreeViewOption.OnlyWithFolders && !hasFolders)))
                        item.Value.Remove(ViewOption.Tree);
                }
                View.InitializePresets( availableOptions[currentSet], currentSet,sets);
                View.InitializeHeaderSettings(isGeneric, currentSet,availableOptions,activeOptions,idCommunity);
                return availableOptions[currentSet];
            }
            private void InitializeAvailableSpace(Boolean displayAvailableSpace,liteRepositorySettings settings, Int32 idCurrentUser, RepositoryType type, Int32 idCommunity,  ModuleRepository permissions, List<dtoDisplayRepositoryItem> treeItems, dtoDisplayRepositoryItem currentFolder = null)
            {
                String diskPath = View.GetRepositoryDiskPath();
                List<dtoFolderSize> items = Service.GetRepositoryAvailabilityInfo(diskPath, settings, idCurrentUser, type, idCommunity,  permissions, treeItems);
                if ((permissions.ManageItems || permissions.Administration || permissions.EditOthersFiles) && displayAvailableSpace)
                {
                    if( items != null && (items.Count>0 || (items.Count==1 && items.FirstOrDefault().Items.Any(i=> i.Type!= FolderSizeItemType.freespace && (i.Number>0 || i.Size>0)))))
                        View.LoadDiskStatistics(items.Where(i => i.FolderType == FolderType.none || (currentFolder != null && (currentFolder.Id == i.IdFolder && currentFolder.IdentifierPath == i.IdentifierPath))).ToList());
                }

                if (permissions.ManageItems || permissions.Administration || permissions.EditOthersFiles || (currentFolder != null && currentFolder.AllowUpload))
                {
                    dtoFolderSize repositoryInfo = items.Where(i => i.FolderType == FolderType.none).FirstOrDefault();
                    repositoryInfo.Quota.DiskSize = Service.GetDiskSize(diskPath);
                    if (repositoryInfo.FreeSpace==0)
                        View.DisplayFolderInfo(repositoryInfo);
                    else
                        View.HideFolderInfo();
                }
                else
                    View.HideFolderInfo();
            }
            private void InitializeFolderPath(dtoDisplayRepositoryItem folder, RepositoryType type,Int32 idCommunity = -1, OrderBy orderBy = OrderBy.name, Boolean? ascending = null)
            {
                View.InitializeBreadCrumb((folder != null) ? folder.GetBreadCrumb(true, type, idCommunity) : null, RootObject.RepositoryItems(type, idCommunity,  0, 0, FolderType.none, orderBy, ascending), orderBy, (ascending.HasValue ? ascending.Value : true));
            }
            private void LoadOrderByItems(List<dtoDisplayRepositoryItem> items, dtoDisplayRepositoryItem currentFolder, ref OrderBy orderby, ref Boolean? ascending)
            {
                List<OrderBy> orderItems = GetOrderByItems((currentFolder != null) ? currentFolder.Children : (items != null && items.Where(i => i.IdFolder == 0).Any() ? items.Where(i => i.IdFolder == 0).ToList() : null), currentFolder);
                if (orderItems.Any()){
                    if (!orderItems.Contains(orderby)){
                        orderby = OrderBy.name;
                        ascending = true;
                    }
                    View.InitializeAvailableOrderBy(orderItems,orderby);
                }
                else
                    View.HideOrderBySelector();
                if (!ascending.HasValue)
                    ascending = !(orderby == OrderBy.date);
            }
            private List<OrderBy> GetOrderByItems(List<dtoDisplayRepositoryItem> fItems, dtoDisplayRepositoryItem currentFolder = null)
            {
                List<OrderBy> result = new List<OrderBy>();
                if (fItems != null && fItems.Count>1)
                {
                    if (currentFolder == null || currentFolder.FolderType== FolderType.standard)
                        result.Add(OrderBy.displayorder);
                    result.Add(OrderBy.date);
                    result.Add(OrderBy.name);
                    if (fItems.Where(i=>i.Type != ItemType.Folder).Select(i => i.Size).Distinct().Count() > 1)
                        result.Add(OrderBy.size);
                }
                return result;
            }
            private void LoadFolderCommands(long idFolder,liteRepositorySettings settings, dtoDisplayRepositoryItem currentFolder, List<dtoDisplayRepositoryItem> items, ModuleRepository module, RepositoryType type, Int32 idCommunity)
            {
                List<dtoDisplayRepositoryItem> dItems = null;
                if (currentFolder == null)
                    dItems = (items == null) ? new List<dtoDisplayRepositoryItem>() : items.Where(i => i.IdFolder == 0 && i.Deleted == BaseStatusDeleted.None && (i.IsFile || i.FolderType == FolderType.standard)).ToList();
                else
                    dItems = currentFolder.Children.Where(c=> currentFolder.FolderType== FolderType.recycleBin || c.Deleted== BaseStatusDeleted.None).ToList();
                List<ItemAction> actions = dItems.SelectMany(i => i.Permissions.GetMultipleActions()).Distinct().ToList();
                if (currentFolder != null && currentFolder.Type != ItemType.RootFolder)
                    actions.Add(ItemAction.gotofolderfather);
                if (currentFolder == null || (currentFolder.Deleted == BaseStatusDeleted.None && currentFolder.FolderType == FolderType.standard))
                {
                    if (module.Administration || module.ManageItems || module.UploadFile || (currentFolder != null && (currentFolder.AllowUpload || currentFolder.Permissions.AllowUpload) && module.ViewItemsList))
                    {
                        actions.Add(ItemAction.addfolder);
                        actions.Add(ItemAction.addlink);
                    }
                }
                
                
                dtoContainerQuota quota = null;
                List<dtoFolderTreeItem> foldersTree = null;
                String fatherUrl = "";

                if (currentFolder == null)
                    quota = Service.FolderGetHomeAvailableSize(View.GetRepositoryDiskPath(), settings, module, type, idCommunity);
                else
                {
                    if (currentFolder.IdFolder == 0)
                        fatherUrl = RootObject.FolderUrlTemplate(0, FolderType.standard, type, idCommunity);
                    else if (currentFolder.Father != null)
                        fatherUrl = RootObject.FolderUrlTemplate(currentFolder.Father.Id, currentFolder.Father.FolderType, currentFolder.Father.IdentifierPath, type, idCommunity);
                    quota = Service.FolderGetHomeAvailableSize(View.GetRepositoryDiskPath(), settings, module, type, idCommunity);
                }

                List<dtoNodeFolderItem> folders = null;
                if (quota != null)
                {
                    folders = Service.GetFoldersForUpload(View.GetRepositoryDiskPath(), idFolder, UserContext.CurrentUserID, type, idCommunity,  Service.GetPermissions(type, idCommunity, UserContext.CurrentUserID), View.GetUnknownUserName(), View.GetRootFolderFullname(), foldersTree);
                    if (currentFolder == null || (currentFolder.Deleted == BaseStatusDeleted.None && currentFolder.FolderType == FolderType.standard))
                    {
                        if (quota.HasAllowedSpace() && (module.Administration || module.ManageItems || module.UploadFile || (currentFolder != null && (currentFolder.AllowUpload || currentFolder.Permissions.AllowUpload) && module.ViewItemsList)))
                            actions.Add(ItemAction.upload);
                    }
                }
                else
                    actions = actions.Where(a => a != ItemAction.addfolder && a != ItemAction.addlink).ToList();

                long idFather = (currentFolder == null ? 0 : currentFolder.IdFolder);
                String fatherPath = (currentFolder == null || currentFolder.Father == null ? "" : currentFolder.Father.IdentifierPath);
                FolderType fatherType = (currentFolder == null || currentFolder.Father == null) ? FolderType.standard : currentFolder.Father.FolderType;
                UpdateCookies(currentFolder);
                View.InitializeFolderCommands(actions, quota, idFather, fatherPath, fatherType, (idFolder == 0) ? View.GetRootFolderFullname() : currentFolder.Name, fatherUrl, (idFolder == 0 ? "" : (currentFolder.IdFolder == 0 ? View.GetRootFolderFullname() : currentFolder.Father.Name)), folders, (settings.ItemTypes == null ? null : settings.ItemTypes.Where(t => t.Deleted == BaseStatusDeleted.None && t.Type != ItemType.Folder && t.Type != ItemType.Link && t.Type != ItemType.RootFolder).Select(t => t.Type).Distinct().ToList()));
            }
            private void UpdateCookies(dtoDisplayRepositoryItem currentFolder)
            {
                if (currentFolder != null)
                {
                    View.SetFoldersCookies(currentFolder.GetIdFathers());
                }
            }
        #endregion

        #region "Manage"
            private void LoadItems(liteRepositorySettings settings, RepositoryIdentifier rIdentifier, List<dtoDisplayRepositoryItem> items, dtoDisplayRepositoryItem currentFolder, OrderBy order, Boolean asc)
            {
                List<dtoDisplayRepositoryItem> dItems = null;
                if (currentFolder == null)
                    dItems = items.Where(i => i.IdFolder == 0 && i.Deleted == BaseStatusDeleted.None && (i.IsFile || i.FolderType == FolderType.standard)).ToList();
                else
                    dItems = currentFolder.Children.Where(c => currentFolder.FolderType == FolderType.recycleBin || c.Deleted == BaseStatusDeleted.None).ToList();
                if (dItems.Any(i => i.Type != ItemType.Folder))
                {
                    Dictionary<long, long> dStatistics = Service.DownloadStatisticsGetFull(dItems.Where(i => i.Type != ItemType.Folder).Select(i => i.Id).ToList(), UserContext.CurrentUserID);
                    dItems.Where(i => i.Type != ItemType.Folder && dStatistics.ContainsKey(i.Id)).ToList().ForEach(i => i.MyDownloads = dStatistics[i.Id]);
                }
                var query = dItems.AsQueryable();
                switch( order){
                    case OrderBy.date:
                        query = ((asc) ? query.OrderBy(i => i.OrderByFolder).ThenBy(i => i.CreatedOn).ThenBy(i => i.DisplayName) : query.OrderBy(i => i.OrderByFile).ThenByDescending(i => i.CreatedOn).ThenBy(i => i.DisplayName));
                        break;
                    case OrderBy.displayorder:
                        query = ((asc) ? query.OrderBy(i => i.DisplayOrder).ThenBy(i => i.DisplayName) : query.OrderByDescending(i => i.DisplayOrder).ThenBy(i => i.DisplayName));
                        break;
                    case OrderBy.name:
                        query = ((asc) ? query.OrderBy(i => i.OrderByFolder).ThenBy(i => i.DisplayName) : query.OrderBy(i => i.OrderByFolder).ThenByDescending(i => i.DisplayName));
                        break;
                    case OrderBy.size:
                        query = ((asc) ? query.OrderBy(i => i.OrderByFile).ThenBy(i => i.Size).ThenBy(i => i.DisplayName) : query.OrderBy(i => i.OrderByFile).ThenByDescending(i => i.Size).ThenBy(i => i.DisplayName));
                        break;
                }
                dItems = query.ToList();

                if (dItems.Any(i => i.Type == ItemType.Multimedia || i.Type == ItemType.ScormPackage))
                {
                    Service.ItemsRefreshAvailability(dItems.Where(i => i.Type == ItemType.Multimedia || i.Type == ItemType.ScormPackage), rIdentifier);
                }

                View.LoadItems(dItems, (currentFolder!=null && currentFolder.FolderType!= FolderType.standard),  (currentFolder != null && (currentFolder.FolderType == FolderType.recycleBin )), GetColumns(currentFolder, dItems, order));

            }
            private List<lm.Comol.Core.BaseModules.FileRepository.Domain.Column> GetColumns(dtoDisplayRepositoryItem currentFolder,List<dtoDisplayRepositoryItem> items,OrderBy order)
            {
                List<lm.Comol.Core.BaseModules.FileRepository.Domain.Column> columns = new List<Column>();
                columns.Add(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.displayorder);
                columns.Add(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.name);
                if (items.Any() && (currentFolder == null || currentFolder.FolderType == FolderType.standard))
                    columns.Add(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.date);
                if (items.Any(i => i.IsUserAvailable) && (currentFolder == null || currentFolder.FolderType == FolderType.standard))
                    columns.Add(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.actions);
                if (items.Any(i => i.IsUserAvailable && (i.HasVersions || !i.HasDefaultPermissions)) && (currentFolder == null || currentFolder.FolderType == FolderType.standard))
                    columns.Add(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.indicators);
                if (items.Any(i => i.IsUserAvailable && (i.Permissions.ViewMyStatistics || i.Permissions.ViewOtherStatistics)))
                    columns.Add(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.stats);
                if (items.Any(i => i.IsUserAvailable && i.Permissions.AllowSelection))
                    columns.Add(lm.Comol.Core.BaseModules.FileRepository.Domain.Column.selectitem);
                return columns;
            }
        #endregion

        #region "Actions"
            public void SelectFolder(long idFolder, String identifierPath, FolderType folderType, OrderBy orderBy, Boolean ascending, RepositoryType type, Int32 idCommunity)
            {
                if (!SessionTimeout()) 
                {
                    ModuleRepository.ActionType uAction = ModuleRepository.ActionType.NoPermission;

                    Int32 idCurrentUser = UserContext.CurrentUserID;
                    ModuleRepository module = Service.GetPermissions(type, idCommunity,  idCurrentUser);
                    if (module.ViewRepository)
                    {
                        LoadFolderContent(type, idFolder, identifierPath, folderType, Service.SettingsGetDefault( type, idCommunity), module, idCommunity,  idCurrentUser, orderBy, ascending);
                        uAction = ModuleRepository.ActionType.LoadRepository;
                    }
                    else
                        View.DisplayNoPermission(idCommunity, Service.GetIdModule());
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction);
                   WriteCookie(idFolder,identifierPath,folderType,orderBy,ascending,type,idCommunity);
                }
            }
            private void WriteCookie(long idFolder, String identifierPath, FolderType folderType, OrderBy orderBy, Boolean ascending, RepositoryType type, Int32 idCommunity){
                cookieRepository cookie = new cookieRepository();
                cookie.IdFolder=idFolder;
                cookie.Type=folderType;
                cookie.IdentifierPath=identifierPath;
                cookie.ItemsOrderBy=orderBy;
                cookie.Ascending=ascending;
                cookie.Repository.IdCommunity= idCommunity;
                cookie.Repository.Type= type;
                View.WriteRepositoryCookie(cookie);
            }
            private void LoadFolderContent(RepositoryType type, long idFolder, String path, FolderType folderType, liteRepositorySettings settings, ModuleRepository module, Int32 idCommunity = -1,  Int32 idCurrentUser = -1, OrderBy orderBy = OrderBy.name, Boolean? ascending = null)
            {
                Boolean admin = module.ManageItems || module.Administration;
                List<dtoDisplayRepositoryItem> items = Service.GetAvailableRepositoryItems(settings, idCurrentUser, type, idCommunity,  View.GetUnknownUserName(), View.GetFolderTypeTranslation(), View.GetTypesTranslations(), module, admin, admin);
                List<dtoNodeItem> folders = new List<dtoNodeItem>();
                dtoDisplayRepositoryItem currentFolder = Service.GetFolder(items, idFolder, path, folderType);
                View.CurrentPath = "/";
                if (currentFolder == null)
                {
                    idFolder = 0;
                    folderType = FolderType.standard;
                    View.CurrentFolderIdentifierPath = "";
                }
                else
                {
                    View.CurrentPath = currentFolder.FullPath;
                    View.CurrentFolderIdentifierPath = currentFolder.IdentifierPath;
                }
                View.IdCurrentFolder = idFolder;
                View.CurrentFolderType = folderType;
                if (items == null)
                    return;

                LoadOrderByItems(items, currentFolder, ref orderBy, ref ascending);
                View.CurrentAscending = (ascending.HasValue) ? ascending.Value : true;
                View.CurrentOrderBy = orderBy;

                List<ViewOption> availableOptions = InitializeViewOptions(type, settings, module, Service.HasFolders(items), idCommunity);
                if (items != null && (availableOptions.Contains(ViewOption.Tree) || availableOptions.Contains(ViewOption.FolderPath) || availableOptions.Contains(ViewOption.AvailableSpace) && (admin || module.EditOthersFiles || (currentFolder != null && currentFolder.AllowUpload))))
                {
                    List<FolderType> fTypes = new List<FolderType>() { FolderType.standard, FolderType.recycleBin};
                    items.ForEach(i => folders.AddRange(i.GetFoldersNodes(fTypes, currentFolder, type, idCommunity,  true, false)));
                }

                InitializeAvailableSpace(availableOptions.Contains(ViewOption.AvailableSpace), settings, idCurrentUser, type, idCommunity,  module, items, currentFolder);
                if (items == null)
                    return;
                if (availableOptions.Contains(ViewOption.Tree) || availableOptions.Contains(ViewOption.FolderPath))
                {
                    if (availableOptions.Contains(ViewOption.FolderPath))
                        InitializeFolderPath(currentFolder, type, idCommunity,  orderBy, ascending);
                    else
                        View.HideBreadCrumb();
                }
                LoadFolderCommands(idFolder, settings, currentFolder, items, module, type, idCommunity);
                LoadItems(settings,RepositoryIdentifier.Create(type, idCommunity), items, currentFolder, orderBy, (ascending.HasValue) ? ascending.Value : true);
            }
            public void ReorderItems(long idFolder,FolderType folderType,String identifierPath, OrderBy orderBy, Boolean ascending, RepositoryType type, Int32 idCommunity)
            {
               liteRepositorySettings settings = Service.SettingsGetDefault( type, idCommunity);
               ModuleRepository module = Service.GetPermissions(type, idCommunity, UserContext.CurrentUserID);
                if (settings == null)
                    View.GoToUrl(RootObject.RepositoryItems(type, idCommunity,  -1, idFolder, View.PreloadFolderType, orderBy, ascending));
                else
                {
                    Boolean admin = module.ManageItems || module.Administration;
                    List<dtoDisplayRepositoryItem> items = Service.GetAvailableRepositoryItems(settings, UserContext.CurrentUserID, type, idCommunity,  View.GetUnknownUserName(), View.GetFolderTypeTranslation(), View.GetTypesTranslations(), module, admin, admin);
                    List<dtoNodeItem> folders = new List<dtoNodeItem>();
                    dtoDisplayRepositoryItem currentFolder = Service.GetFolder(items, idFolder, identifierPath, folderType);

                    View.CurrentPath = "/";
                    if (currentFolder == null)
                    {
                        folderType = FolderType.standard;
                        idFolder = 0;
                        View.CurrentFolderType = folderType;
                        View.CurrentFolderIdentifierPath = "";
                        WriteCookie(0, "", FolderType.standard, orderBy, ascending, type, idCommunity);
                    }
                    else
                    {
                        View.CurrentPath = currentFolder.FullPath;
                        WriteCookie(currentFolder.Id,currentFolder.IdentifierPath, currentFolder.FolderType, orderBy, ascending, type,idCommunity);
                    }
                    View.IdCurrentFolder = idFolder;
                    if (items == null)
                        return;
                    View.CurrentAscending = ascending;
                    View.CurrentOrderBy = orderBy;

                    List<ViewOption> availableOptions = InitializeViewOptions(type, settings, module, Service.HasFolders(items), idCommunity);
                    if (items != null && (availableOptions.Contains(ViewOption.Tree) || availableOptions.Contains(ViewOption.FolderPath) || availableOptions.Contains(ViewOption.AvailableSpace) && (admin || module.EditOthersFiles || (currentFolder != null && currentFolder.AllowUpload))))
                    {
                        List<FolderType> fTypes = new List<FolderType>() { FolderType.standard,  FolderType.recycleBin};
                        items.ForEach(i => folders.AddRange(i.GetFoldersNodes(fTypes, currentFolder, type, idCommunity,  true, false)));
                    }


                    if (items == null)
                        return;
                    LoadItems(settings,RepositoryIdentifier.Create(type, idCommunity), items, currentFolder, orderBy, ascending);

                }
            }
            public void ExecuteAction(long idItem, ItemAction action, RepositoryType type, Int32 idCommunity)
            {
                Boolean reloadItems = true;
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.GenericError;
                if (SessionTimeout())
                    return;
                liteRepositoryItem item = Service.ItemGet(idItem);
                long idCurrentFolder = View.IdCurrentFolder;
                if (item == null)
                {
                    View.DisplayUnknownItem(action);
                    uAction = ModuleRepository.ActionType.UnknownItemFound;
                }
                else
                {
                    liteRepositoryItem rItem = null;
                    dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idItem, UserContext.CurrentUserID, type, idCommunity,  View.GetUnknownUserName());

                    if (dItem != null)
                        oType = ModuleRepository.GetObjectType(dItem.Type);

                    if (dItem == null || !dItem.Permissions.GetActions().Contains(action))
                    {
                        View.DisplayUnavailableItem(action);
                        uAction = ModuleRepository.ActionType.UnavailableItem;
                    }
                    else
                    {
                        String path = "";
                        String folderName = (dItem.IdFolder == 0 ? View.GetRootFolderFullname() : Service.FolderGetName(dItem.IdFolder));
                        String folderUrl = RootObject.RepositoryItems(dItem.Repository.Type, dItem.Repository.IdCommunity,  -1, dItem.IdFolder);
                        Boolean executed = false;
                        switch (action)
                        {
                            case ItemAction.addVersion:
                                reloadItems = false;
                                path = View.GetRepositoryDiskPath();
                                switch(type){
                                    case RepositoryType.Portal:
                                        path += "\\0";
                                        break;
                                    case RepositoryType.Community:
                                        path +=  "\\" + idCommunity.ToString();
                                        break;
                                }
                                uAction = ModuleRepository.ActionType.VersionAddingToFile;
                                View.DisplayAddVersion(dItem, Service.GetFolderQuota(path, dItem.IdFolder, dItem.Repository.Type, dItem.Repository.IdCommunity));
                                break;
                            case ItemAction.delete:
                                reloadItems = false;
                                #region "delete"
                                path = View.GetRepositoryDiskPath();

                                Dictionary<RepositoryType, String> paths = new Dictionary<RepositoryType, String>();
                                paths.Add(RepositoryType.Portal, path + "\\0");
                                paths.Add(RepositoryType.Community, path + "\\" + idCommunity.ToString());

                                dtoItemToDelete deletedItem = Service.ItemPhisicalDelete(idItem, paths, dItem.Repository.Type, dItem.Repository.IdCommunity);
                                if (deletedItem == null )
                                {
                                    uAction = ModuleRepository.ActionType.UnavailableItem;
                                    oType = ModuleRepository.GetObjectType(dItem.Type);
                                    View.DisplayUnavailableItem(action);
                                }
                                else
                                {
                                    executed = deletedItem.IsDeleted;
                                    View.DisplayDeletedItem(deletedItem);
                                    if (executed){
                                        uAction =  ModuleRepository.ActionType.PhisicalDeleteItem;
                                        folderUrl = RootObject.RepositoryItems(dItem.Repository.Type, dItem.Repository.IdCommunity,  -1, (-1 * (int)FolderType.recycleBin), FolderType.recycleBin);
                                        View.NotifyDelete(folderUrl, deletedItem);
                                    }
                                    else
                                        uAction = ModuleRepository.ActionType.UnableToPhisicalDeleteItem;
                                    reloadItems = executed;
                                }
                                 #endregion
                                break;
                            case ItemAction.move:
                                #region "move items"
                                String rootFolderName = View.GetRootFolderFullname();
                                List<long> idFolders = (dItem.Type == ItemType.Folder) ? new List<long>() { dItem.Id } : new List<long>();
                                List<dtoNodeFolderItem> folders = Service.GetFoldersForMove(View.GetRepositoryDiskPath(), idCurrentFolder, UserContext.CurrentUserID, type, idCommunity,  Service.GetPermissions(type, idCommunity,  UserContext.CurrentUserID), View.GetUnknownUserName(), rootFolderName, item.Size + item.DeletedSize + item.VersionsSize, idFolders);
                                if (folders == null)
                                {
                                    uAction = ModuleRepository.ActionType.UnableToTryToMoveItem;
                                    View.DisplayUnableToInitialize(action, item.DisplayName, item.Extension, item.Type);
                                }
                                else
                                {
                                    View.DisplayMoveItemSelector(idItem,idCurrentFolder, (idCurrentFolder == 0 ? rootFolderName : Service.ItemGetName(idCurrentFolder)), folders, item.DisplayName, item.Extension, item.Type);
                                    uAction = ModuleRepository.ActionType.TryToMoveItem;
                                }
                                #endregion
                                break;
                            case ItemAction.hide:
                            case ItemAction.show:
                                #region "hide/Show"
                                rItem = Service.ItemSetVisibility(idItem, (action == ItemAction.show), type, idCommunity);
                                if (rItem == null)
                                {
                                    View.DisplayUnavailableItem(action);
                                    uAction = ModuleRepository.ActionType.UnavailableItem;
                                }
                                else
                                {
                                    executed = (rItem.IsVisible == (action == ItemAction.show));
                                    View.DisplayMessage(action, executed, rItem.DisplayName, rItem.Extension, rItem.Type);
                                    if (executed){
                                        View.NotifyVisibilityChanged(rItem.IdFolder, folderName, folderUrl, rItem);
                                        switch(rItem.Type){
                                            case  ItemType.Folder:
                                                if (action== ItemAction.show)
                                                    uAction = ModuleRepository.ActionType.ShowFolder;
                                                else
                                                    uAction = ModuleRepository.ActionType.HideFolder;
                                                break;
                                            default:
                                                if (action== ItemAction.show)
                                                    uAction = ModuleRepository.ActionType.ShowItem;
                                                else
                                                    uAction = ModuleRepository.ActionType.HideItem;
                                                break;
                                        }
                                        reloadItems = executed;
                                    }
                                    else{
                                        if (action== ItemAction.show)
                                            uAction = ModuleRepository.ActionType.UnableToShow;
                                        else
                                            uAction = ModuleRepository.ActionType.UnableToHide;
                                    }  
                                }
                                #endregion
                                break;
                            case ItemAction.undelete:
                            case ItemAction.virtualdelete:
                                #region "virtualdelete/undelete"
                                rItem = Service.ItemVirtualDelete(idItem, action, type, idCommunity);
                                if (rItem == null)
                                {
                                    View.DisplayUnavailableItem(action);
                                    uAction = ModuleRepository.ActionType.UnavailableItem;
                                }
                                else
                                {
                                    executed = ((rItem.Deleted == BaseStatusDeleted.None && action == ItemAction.undelete) || (rItem.Deleted == BaseStatusDeleted.Manual && action == ItemAction.virtualdelete));
                                    View.DisplayMessage(action, executed, rItem.DisplayName, rItem.Extension, rItem.Type);
                                    if (executed)
                                    {
                                        uAction = (action == ItemAction.undelete) ? ModuleRepository.ActionType.UndeleteItem : ModuleRepository.ActionType.VirtualDeleteItem;
                                        View.NotifyVirtualDelete(rItem.IdFolder, folderName, folderUrl, rItem, (action == ItemAction.virtualdelete));
                                    }
                                    else
                                        uAction = (action == ItemAction.undelete) ? ModuleRepository.ActionType.UnableToUndeleteItem : ModuleRepository.ActionType.UnableToVirtualDeleteItem;
                                    reloadItems = executed;
                                }
                                #endregion
                                break;
                        }
                    }
                }
                View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
                if (reloadItems)
                    InitializeView(type, View.IdCurrentFolder, View.CurrentFolderIdentifierPath, View.CurrentFolderType, Service.SettingsGetDefault( type, idCommunity), Service.GetPermissions(type, idCommunity,  UserContext.CurrentUserID), idCommunity,  UserContext.CurrentUserID, View.CurrentOrderBy, View.CurrentAscending);
            }
            public void ExecuteAction(List<long> idItems, ItemAction action, RepositoryType type, Int32 idCommunity)
            {
                Boolean reloadItems = true;
                Boolean multipleItems = (idItems.Count > 1);
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.GenericError;
                if (SessionTimeout())
                    return;

                List<long> idActionItems = null;
                List<dtoDisplayRepositoryItem> pItems = Service.GetItemsWithPermissions(idItems, UserContext.CurrentUserID, RepositoryIdentifier.Create(type, idCommunity), View.GetUnknownUserName());
                if (pItems == null || !pItems.Any())
                {
                    uAction = (multipleItems) ? ModuleRepository.ActionType.UnknownItemsFound : ModuleRepository.ActionType.UnknownItemFound;
                    View.DisplayUnknownItem(action,multipleItems);
                }
                else if (!pItems.Any(i => i.Permissions.GetActions().Contains(action)))
                {
                    uAction = (multipleItems) ? ModuleRepository.ActionType.UnavailableItems : ModuleRepository.ActionType.UnavailableItem;
                    if (multipleItems)
                        View.DisplayUnavailableItems(action);
                    else
                    {
                        oType = ModuleRepository.GetObjectType(pItems.FirstOrDefault().Type);
                        View.DisplayUnavailableItem(action);
                    }
                }
                else
                {
                    long idCurrentFolder = View.IdCurrentFolder;
                    List<liteRepositoryItem> items = null;
                    idActionItems = pItems.Where(i => i.Permissions.GetActions().Contains(action)).Select(i => i.Id).Distinct().ToList();
                    String folderUrl = RootObject.RepositoryItems(type, idCommunity,  -1, idCurrentFolder);

                    Boolean executed = false;
                    switch (action)
                    {
                        case ItemAction.hide:
                        case ItemAction.show:
                            #region "hide/Show"
                            items = Service.ItemsSetVisibility(idActionItems, (action == ItemAction.show), type, idCommunity);
                            if (items == null)
                            {
                                uAction = (multipleItems) ? ModuleRepository.ActionType.UnavailableItems : ModuleRepository.ActionType.UnavailableItem;
                                if (idActionItems.Count>1)
                                    View.DisplayUnavailableItems(action);
                                else
                                {
                                    oType = ModuleRepository.GetObjectType(pItems.Where(i=> idActionItems.Contains(i.Id)).FirstOrDefault().Type);
                                    View.DisplayUnavailableItem(action);
                                }
                            }
                            else
                            {
                                executed = (items.Any(i=> i.IsVisible) == (action == ItemAction.show));
                                View.DisplayMessage(action, executed, items.GroupBy(i=>i.Type).ToDictionary(i=> i.Key, i=> (long)i.Count()), pItems.Where(i=>idActionItems.Contains(i.Id)).GroupBy(i=>i.Type).ToDictionary(i=> i.Key, i=> (long)i.Count()));
                                if (executed)
                                    uAction = (action == ItemAction.show) ? ModuleRepository.ActionType.ShowItems : ModuleRepository.ActionType.HideItems;
                                else
                                    uAction =  (action == ItemAction.show) ? ModuleRepository.ActionType.UnableToShow :ModuleRepository.ActionType.UnableToHide;
                                reloadItems = executed;
                            }
                            #endregion
                            break;
                        case ItemAction.undelete:
                        case ItemAction.virtualdelete:
                            #region "virtualdelete/undelete"
                            items = Service.ItemsVirtualDelete(idActionItems, action, type, idCommunity);
                            if (items == null)
                            {
                                uAction = (multipleItems) ? ModuleRepository.ActionType.UnavailableItems : ModuleRepository.ActionType.UnavailableItem;
                                if (idActionItems.Count > 1)
                                    View.DisplayUnavailableItems(action);
                                else
                                {
                                    oType = ModuleRepository.GetObjectType(pItems.Where(i => idActionItems.Contains(i.Id)).FirstOrDefault().Type);
                                    View.DisplayUnavailableItem(action);
                                }
                            }
                            else
                            {
                                executed = items.Any(i => (i.Deleted == BaseStatusDeleted.None && action == ItemAction.undelete) || (i.Deleted == BaseStatusDeleted.Manual && action == ItemAction.virtualdelete));
                                View.DisplayMessage(action, executed, items.GroupBy(i => i.Type).ToDictionary(i => i.Key, i => (long)i.Count()), pItems.Where(i => idActionItems.Contains(i.Id)).GroupBy(i => i.Type).ToDictionary(i => i.Key, i => (long)i.Count()));
                                if (executed)
                                    uAction = (action == ItemAction.undelete) ? ModuleRepository.ActionType.UndeleteItems : ModuleRepository.ActionType.VirtualDeleteItems;
                                else
                                    uAction = (action == ItemAction.undelete) ? ModuleRepository.ActionType.UnableToUndeleteItems : ModuleRepository.ActionType.UnableToVirtualDeleteItems;
                                reloadItems = executed;
                            }
                            #endregion
                            break;
                        case ItemAction.delete:
                            reloadItems =false;
                            #region "delete"
                            String path  = View.GetRepositoryDiskPath();

                            Dictionary<RepositoryType, String> paths = new Dictionary<RepositoryType, String>();
                            paths.Add(RepositoryType.Portal, path + "\\0");
                            paths.Add(RepositoryType.Community, path + "\\" + idCommunity.ToString());

                            List<dtoItemToDelete> deletedItems = Service.ItemsPhisicalDelete(idActionItems, paths, type, idCommunity);
                            if (deletedItems == null || !deletedItems.Any())
                            {
                                uAction = (multipleItems) ? ModuleRepository.ActionType.UnavailableItems : ModuleRepository.ActionType.UnavailableItem;
                                if (idActionItems.Count > 1)
                                    View.DisplayUnavailableItems(action);
                                else
                                {
                                    oType = ModuleRepository.GetObjectType(pItems.Where(i => idActionItems.Contains(i.Id)).FirstOrDefault().Type);
                                    View.DisplayUnavailableItem(action);
                                }
                            }
                            else
                            {
                                executed = deletedItems.Any(i => i.IsDeleted);
                                View.DisplayDeletedItems( deletedItems);
                                if (executed)
                                {
                                    uAction = (deletedItems.Count == 1) ? ModuleRepository.ActionType.PhisicalDeleteItem : ModuleRepository.ActionType.PhisicalDeleteItems;
                                    folderUrl = RootObject.RepositoryItems(type, idCommunity,  -1, (-1 * (int)FolderType.recycleBin), FolderType.recycleBin);
                                    View.NotifyDelete(folderUrl, deletedItems.Where(i=> i.IsDeleted).ToList());
                                }
                                else
                                    uAction = (deletedItems.Count == 1) ? ModuleRepository.ActionType.UnableToPhisicalDeleteItem : ModuleRepository.ActionType.UnableToPhisicalDeleteItems;
                                reloadItems = executed;
                            }
                            if (executed)
                                idActionItems = deletedItems.Where(i=> i.IsDeleted).Select(i=> i.Id).ToList();
                            #endregion
                            break;
                    }
                }
     
                if (pItems==null){
                    switch (idItems.Count)
                    {
                        case 1:
                            View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItems.FirstOrDefault(), oType);
                            break;
                        case 0:
                            View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, 0, oType);
                            break;
                        default:
                            View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, new Dictionary<lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType, List<long>> { { oType, idItems } });
                            break;
                    }
                }
                else if (idActionItems==null || !idActionItems.Any()){
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, pItems.GroupBy(i => i.Type).ToDictionary(i => ModuleRepository.GetObjectType(i.Key), i => i.Select(f => f.Id).ToList()));
                }
                else if (action!= ItemAction.delete)
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, pItems.Where(i=> idActionItems.Contains(i.Id)).GroupBy(i => i.Type).ToDictionary(i => ModuleRepository.GetObjectType(i.Key), i => i.Select(f => f.Id).ToList()));
                if (reloadItems)
                    InitializeView(type, View.IdCurrentFolder, View.CurrentFolderIdentifierPath, View.CurrentFolderType, Service.SettingsGetDefault( type, idCommunity), Service.GetPermissions(type, idCommunity,  UserContext.CurrentUserID), idCommunity,  UserContext.CurrentUserID, View.CurrentOrderBy, View.CurrentAscending);
            }
            public void MoveToFolder(long idItem, long idDestinationFolder, RepositoryType type, Int32 idCommunity){
                Boolean reloadItems = true;
                ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                ModuleRepository.ActionType uAction = ModuleRepository.ActionType.GenericError;
                if (SessionTimeout())
                    return;
                liteRepositoryItem item = Service.ItemGet(idItem);
                long idCurrentFolder = View.IdCurrentFolder;
                if (item == null)
                {
                    View.DisplayUnknownItem(ItemAction.move);
                    uAction = ModuleRepository.ActionType.UnknownItemFound;
                }
                else
                {
                    liteRepositoryItem movedItem = null;
                    dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idItem, UserContext.CurrentUserID, type, idCommunity,  View.GetUnknownUserName());

                    if (dItem != null)
                        oType = ModuleRepository.GetObjectType(dItem.Type);

                    if (dItem == null || !dItem.Permissions.GetActions().Contains(ItemAction.move))
                    {
                        View.DisplayUnavailableItem(ItemAction.move);
                        uAction = ModuleRepository.ActionType.UnavailableItem;
                    }
                    else
                    {
                        if (dItem.IdFolder == idDestinationFolder)
                        {
                            reloadItems = false;
                            uAction = ModuleRepository.ActionType.NotMovedItems;
                            View.DisplaySameDirectory();
                        }
                        else
                        {
                            String rFolderName = View.GetRootFolderFullname();
                            movedItem = Service.ItemMoveTo(idItem, View.IdCurrentFolder, idDestinationFolder, type, idCommunity);
                            Boolean executed = (movedItem != null && movedItem.IdFolder == idDestinationFolder);
                            uAction = (executed ? ModuleRepository.ActionType.MovedItem : ModuleRepository.ActionType.NotMovedItem);
                            reloadItems = executed;
                            View.DisplayMessage(ItemAction.move, executed, dItem.DisplayName, dItem.Extension, dItem.Type, (dItem.IdFolder == 0 ? rFolderName : Service.ItemGetName(dItem.IdFolder)), (idDestinationFolder == 0 ? rFolderName : Service.ItemGetName(idDestinationFolder)));
                        }
                    }
                }
                View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
                if (reloadItems)
                    InitializeView(type, View.IdCurrentFolder, View.CurrentFolderIdentifierPath, View.CurrentFolderType, Service.SettingsGetDefault( type, idCommunity), Service.GetPermissions(type, idCommunity,  UserContext.CurrentUserID), idCommunity,  UserContext.CurrentUserID, View.CurrentOrderBy, View.CurrentAscending);
            }

            #region "Add Items"
                public void AddFolders(long idFather, List<dtoFolderName> names, RepositoryType type, Int32 idCommunity)
                 {
                     ModuleRepository module = Service.GetPermissions(type, idCommunity,  UserContext.CurrentUserID);
                    Boolean reloadItems = true;
                    ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                    ModuleRepository.ActionType uAction = ModuleRepository.ActionType.UnableToAddFolder;
                    if (SessionTimeout())
                        return;
                    liteRepositoryItem item = Service.ItemGet(idFather);
                    long idCurrentFolder = View.IdCurrentFolder;
                    if (item == null && idFather> 0)
                    {
                        View.DisplayUnknownItem(ItemAction.addfolder);
                        uAction = ModuleRepository.ActionType.UnknownItemFound;
                    }
                    else
                    {
                        Boolean allowAdd = module.Administration || module.ManageItems || module.UploadFile;
                        dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idFather, UserContext.CurrentUserID, type, idCommunity,  View.GetUnknownUserName());

                        if (dItem != null)
                        {
                            oType = ModuleRepository.GetObjectType(dItem.Type);
                            allowAdd = allowAdd || dItem.Permissions.GetActions().Contains(ItemAction.addfolder);
                        }
                        else if (idFather == 0)
                            oType = ModuleRepository.ObjectType.Folder;

                        if (!allowAdd)
                        {
                            if (idFather > 0)
                                View.DisplayUnavailableItem(ItemAction.addfolder, item.DisplayName);
                            else
                                View.DisplayUnavailableItem(ItemAction.addfolder, View.GetRootFolderFullname());
                            uAction = ModuleRepository.ActionType.UnavailableItem;
                        }
                        else
                        {
                            Boolean executed = false;
                            List<RepositoryItem> folders = Service.FolderAddToRepository(idFather, names, type, idCommunity);
                            executed = folders.Any();
                            reloadItems = executed;
                            uAction = (executed ? ModuleRepository.ActionType.AddFolder : ModuleRepository.ActionType.UnableToAddFolder);
                            String folderName = (idFather == 0 ? View.GetRootFolderFullname() : dItem.Name);
                            if (executed)
                            {
                                View.DisplayAddedFolders(folderName, names.Where(n => n.Id > 0).LongCount(), names.Where(n => n.Id == 0).LongCount());
                                View.NotifyAddedItems(idFather, folderName, RootObject.RepositoryItems(type, idCommunity,  -1, idFather), folders);
                            }
                            else
                                View.DisplayUnableToAddFolders(folderName, names.Count);
                        }
                    }
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idFather, oType);
                    if (reloadItems)
                        InitializeView(type, View.IdCurrentFolder, View.CurrentFolderIdentifierPath, View.CurrentFolderType, Service.SettingsGetDefault( type, idCommunity), module, idCommunity,  UserContext.CurrentUserID, View.CurrentOrderBy, View.CurrentAscending);
                }
                public void AddLinks(long idFather, List<dtoUrlItem> links, RepositoryType type, Int32 idCommunity)
                {
                    ModuleRepository module = Service.GetPermissions(type, idCommunity,  UserContext.CurrentUserID);
                    Boolean reloadItems = true;
                    ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                    ModuleRepository.ActionType uAction = ModuleRepository.ActionType.UnableToAddLink;
                    if (SessionTimeout())
                        return;
                    liteRepositoryItem item = Service.ItemGet(idFather);
                    long idCurrentFolder = View.IdCurrentFolder;
                    if (item == null && idFather > 0)
                    {
                        View.DisplayUnknownItem(ItemAction.addlink);
                        uAction = ModuleRepository.ActionType.UnknownItemFound;
                    }
                    else
                    {
                        Boolean allowAdd = module.Administration || module.ManageItems || module.UploadFile;
                        dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idFather, UserContext.CurrentUserID, type, idCommunity,  View.GetUnknownUserName());

                        if (dItem != null)
                        {
                            oType = ModuleRepository.GetObjectType(dItem.Type);
                            allowAdd = allowAdd || dItem.Permissions.GetActions().Contains(ItemAction.addlink);
                        }
                        else if (idFather == 0)
                            oType = ModuleRepository.ObjectType.Folder;

                        if (!allowAdd)
                        {
                            if (idFather > 0)
                                View.DisplayUnavailableItem(ItemAction.addlink, item.DisplayName);
                            else
                                View.DisplayUnavailableItem(ItemAction.addlink, View.GetRootFolderFullname());
                            uAction = ModuleRepository.ActionType.UnavailableItem;
                        }
                        else
                        {
                            Boolean executed = false;
                            List<RepositoryItem> addedLinks = Service.LinkAddToRepository(idFather, links, type, idCommunity);
                            executed = addedLinks.Any();
                            reloadItems = executed;
                            uAction = (executed ? ModuleRepository.ActionType.AddLink : ModuleRepository.ActionType.UnableToAddLink);
                            String folderName = (idFather == 0 ? View.GetRootFolderFullname() : dItem.Name);
                            if (executed)
                            {
                                View.DisplayAddedLinks(folderName, links.Where(n => n.Id > 0).LongCount(), links.Where(n => n.Id == 0).LongCount());
                                View.NotifyAddedItems(idFather, folderName, RootObject.RepositoryItems(type, idCommunity,  -1, idFather), addedLinks);
                            }
                            else
                                View.DisplayUnableToAddLinks(folderName, links.LongCount());
                        }
                    }
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idFather, oType);
                    if (reloadItems)
                        InitializeView(type, View.IdCurrentFolder, View.CurrentFolderIdentifierPath, View.CurrentFolderType, Service.SettingsGetDefault( type, idCommunity), module, idCommunity,  UserContext.CurrentUserID, View.CurrentOrderBy, View.CurrentAscending);
                }
                public void AddFiles(String istanceIdentifier, long idFather, List<dtoUploadedItem> files, RepositoryType type, Int32 idCommunity)
                {
                    liteRepositorySettings settings = null;
                    ModuleRepository module = Service.GetPermissions(type, idCommunity,  UserContext.CurrentUserID);
                    Boolean reloadItems = true;
                    ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                    ModuleRepository.ActionType uAction = ModuleRepository.ActionType.UnableToAddFile;
                    if (SessionTimeout())
                        return;
                    liteRepositoryItem item = Service.ItemGet(idFather);
                    long idCurrentFolder = View.IdCurrentFolder;
                    if (item == null && idFather > 0)
                    {
                        View.DisplayUnknownItem(ItemAction.upload);
                        uAction = ModuleRepository.ActionType.UnknownItemFound;
                    }
                    else
                    {
                        Boolean allowAdd = module.Administration || module.ManageItems || module.UploadFile;
                        dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idFather, UserContext.CurrentUserID, type, idCommunity,  View.GetUnknownUserName());

                        if (dItem != null)
                        {
                            oType = ModuleRepository.GetObjectType(dItem.Type);
                            allowAdd = allowAdd || dItem.Permissions.GetActions().Contains(ItemAction.upload);
                        }
                        else if (idFather == 0)
                            oType = ModuleRepository.ObjectType.Folder;

                        if (!allowAdd)
                        {
                            if (idFather > 0)
                                View.DisplayUnavailableItem(ItemAction.upload, item.DisplayName);
                            else
                                View.DisplayUnavailableItem(ItemAction.upload, View.GetRootFolderFullname());
                            uAction = ModuleRepository.ActionType.UnavailableItem;
                        }
                        else
                        {
                            Boolean executed = false;
                            settings = Service.SettingsGetDefault( type, idCommunity);

                            List<dtoCreatedItem> addedFiles = Service.FileAddToRepository(settings,module, View.GetRepositoryDiskPath(), istanceIdentifier, idFather, files, type, idCommunity);
                            executed = (addedFiles != null && addedFiles.Any(a => a.IsAdded));
                            reloadItems = executed;
                           
                            uAction = (executed ? (addedFiles.Any(a => !a.IsAdded) ? ModuleRepository.ActionType.UnableToAddSomeFile : ModuleRepository.ActionType.AddFile) : ModuleRepository.ActionType.UnableToAddFile);
                            String folderName = (idFather == 0 ? View.GetRootFolderFullname() : dItem.Name);
                            if (executed)
                            {
                                View.DisplayAddedFiles(folderName, addedFiles.Where(a => a.IsAdded).LongCount(), addedFiles.Where(a => !a.IsAdded).LongCount());
                                View.NotifyAddedItems(idFather, folderName, RootObject.RepositoryItems(type, idCommunity,  -1, (dItem == null ? 0 : dItem.Id)), addedFiles.Where(a => a.Added != null).Select(f => f.Added).ToList());
                            }
                            else
                                View.DisplayUnableToAddFiles(folderName, (addedFiles == null) ? (long)0 : addedFiles.Where(a => !a.IsAdded).LongCount());
                        }
                    }
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idFather, oType);
                    if (reloadItems)
                    {
                        if (settings == null)
                            settings = Service.SettingsGetDefault( type, idCommunity);
                        InitializeView(type, View.IdCurrentFolder, View.CurrentFolderIdentifierPath, View.CurrentFolderType, settings, module, idCommunity,  UserContext.CurrentUserID, View.CurrentOrderBy, View.CurrentAscending);
                    }
                }
                public void AddVersionToFile( String istanceIdentifier, long idItem, dtoUploadedItem version, RepositoryType type, Int32 idCommunity)
                {
                    liteRepositorySettings settings = null;
                    ModuleRepository module = Service.GetPermissions(type, idCommunity,  UserContext.CurrentUserID);
                    Boolean reloadItems = true;
                    ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
                    ModuleRepository.ActionType uAction = ModuleRepository.ActionType.VersionUnableToAdd;
                    if (SessionTimeout())
                        return;
                    dtoDisplayRepositoryItem dItem = Service.GetItemWithPermissions(idItem, UserContext.CurrentUserID, type, idCommunity,  View.GetUnknownUserName());
                    long idCurrentFolder = View.IdCurrentFolder;
                    if (dItem == null)
                    {
                        View.DisplayUnknownItem(ItemAction.addVersion);
                        uAction = ModuleRepository.ActionType.UnknownItemFound;
                    }
                    else
                    {
                        Boolean allowAdd = dItem.Permissions.GetActions().Contains(ItemAction.addVersion);
                        oType = ModuleRepository.GetObjectType(dItem.Type);
                        if (!allowAdd)
                        {
                            View.DisplayUnavailableItem(ItemAction.addVersion);
                            uAction = ModuleRepository.ActionType.UnavailableItem;
                        }
                        else
                        {
                            settings = Service.SettingsGetDefault( type, idCommunity);

                            Service.ThumbnailsCreate(settings,dItem.UniqueId, version);
                            dtoCreatedItem addedVersion = Service.FileAddVersion(settings, module, View.GetRepositoryDiskPath(), istanceIdentifier, idItem, version);
                            reloadItems = (addedVersion != null && addedVersion.IsAdded);
                            String folderName = (dItem.IdFolder == 0 ? View.GetRootFolderFullname() : Service.FolderGetName(dItem.IdFolder));
                            uAction = (reloadItems ? ModuleRepository.ActionType.VersionAddedToFile : ModuleRepository.ActionType.VersionUnableToAdd);
                            View.DisplayAddedVersion(folderName,  addedVersion);

                            View.NotifyAddedVersion(dItem.IdFolder, folderName, RootObject.RepositoryItems(dItem.Repository.Type, dItem.Repository.IdCommunity,  dItem.Id, dItem.IdFolder), addedVersion);
                        }
                    }
                    View.SendUserAction(idCommunity, Service.GetIdModule(), uAction, idItem, oType);
                    if (reloadItems)
                    {
                        if (settings == null)
                            settings = Service.SettingsGetDefault( type, idCommunity);
                        InitializeView(type, View.IdCurrentFolder, View.CurrentFolderIdentifierPath, View.CurrentFolderType, settings, module, idCommunity,  UserContext.CurrentUserID, View.CurrentOrderBy, View.CurrentAscending);
                    }
                }
            #endregion
        #endregion

        public Boolean SessionTimeout()
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