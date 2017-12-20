using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoDisplayRepositoryItem : dtoBaseRepositoryItem , ICloneable 
    {
        public virtual dtoDisplayRepositoryItem Father { get; set; }
        public virtual List<dtoDisplayRepositoryItem> Children { get; set; }
        public virtual Boolean IsUserAvailable { get; set; }
        public virtual Boolean HasDefaultPermissions { get; set; }
        
        public virtual ItemPermission Permissions { get; set; }
        public virtual Boolean IsEmpty { get { return Type != ItemType.Folder || Children == null || !Children.Any(); } }
        public virtual dtoItemIdentifier Identifier { get; set; }
        public virtual List<String> TagsList { get { return (String.IsNullOrWhiteSpace(Tags) ? new List<String>() : Tags.Split(',').ToList()); } }
        public virtual String IdentifierPath { get; set; }
        
        public virtual long MyDownloads { get; set; }
    

        public dtoDisplayRepositoryItem()
        {
            Permissions = new ItemPermission();
            Children = new List<dtoDisplayRepositoryItem>();
            Identifier = new dtoItemIdentifier();
        }
        public dtoDisplayRepositoryItem(dtoRepositoryItem item)
            : base(item)
        {
            Permissions = new ItemPermission();
            Children = new List<dtoDisplayRepositoryItem>();
            Identifier = new dtoItemIdentifier();
        }

        public dtoDisplayRepositoryItem(dtoRepositoryItem item, Int32 idPerson, liteRepositorySettings settings, ModuleRepository modulePermissions, Boolean forAdmin = false, Boolean hasAssignments = false)
            : base(item)
        {
            IsUserAvailable = forAdmin || hasAssignments;
            Children = new List<dtoDisplayRepositoryItem>();
            Permissions = CreatePermission(item, idPerson, settings, modulePermissions, forAdmin);
            Identifier = new dtoItemIdentifier();
        }
        public static dtoDisplayRepositoryItem CreateFromInternal(dtoRepositoryItem source, Int32 idPerson, Boolean allowManage, Boolean allowView)
        {
            dtoDisplayRepositoryItem item = new dtoDisplayRepositoryItem(source);
            item.Children = new List<dtoDisplayRepositoryItem>();
            item.IsUserAvailable = true;
            item.Permissions = new ItemPermission();

            Boolean isOwner = (item.IdOwner == idPerson);
            switch (item.Type)
            {
                case ItemType.File:
                    item.Permissions.Unzip = (item.Availability == ItemAvailability.available) && ((item.Extension == ".zip") && (allowManage || isOwner));
                    item.Permissions.Zip = (item.Availability == ItemAvailability.available) && (allowManage || isOwner);
                    break;
                case ItemType.Folder:
                    break;
                case ItemType.Multimedia:
                case ItemType.ScormPackage:
                    item.Permissions.CanEditSettings = (allowManage || isOwner);
                    item.Permissions.EditSettings = IsAvailableByType(item.Type, item.Availability) && (allowManage || isOwner);
                    item.Permissions.Zip = (item.Availability == ItemAvailability.available) && (allowManage || isOwner);
                    break;
                case ItemType.Link:
                    break;
            }
            item.Permissions.Edit = (allowManage || isOwner);
            item.Permissions.ViewMyStatistics = ((item.Deleted == DomainModel.BaseStatusDeleted.None && item.Type != ItemType.Link && item.Type != ItemType.Folder && item.Type != ItemType.File) && !(allowManage || (isOwner && IsAvailableByType(item.Type, item.Availability))));
            item.Permissions.ViewOtherStatistics = (item.Deleted == DomainModel.BaseStatusDeleted.None && item.Type != ItemType.Link && item.Type != ItemType.Folder && item.Type != ItemType.File) && (allowManage || (isOwner && IsAvailableByType(item.Type, item.Availability)));

            item.Permissions.VersioningAvailable = true;
            item.Permissions.ViewPermission = item.Permissions.Edit;
            item.Permissions.Delete = (item.Deleted != DomainModel.BaseStatusDeleted.None && allowManage);
            item.Permissions.UnDelete = (item.Deleted != DomainModel.BaseStatusDeleted.None && allowManage);
            item.Permissions.VirtualDelete = (item.Deleted == DomainModel.BaseStatusDeleted.None && allowManage);


            item.Permissions.Download = (item.Availability != ItemAvailability.notavailable) && (allowManage || (item.IsDownloadable && item.Type != ItemType.Folder && (isOwner || allowView)));
            item.Permissions.Play = IsAvailableByType(item.Type, item.Availability) && (item.Type == ItemType.Multimedia || item.Type == ItemType.ScormPackage) && ((allowManage || allowView) && item.Type != ItemType.Folder);
            item.Permissions.Preview = IsAvailableByType(item.Type, item.Availability) && (item.DisplayMode == Domain.DisplayMode.inModal || item.DisplayMode == Domain.DisplayMode.downloadOrPlayOrModal);
            return item;
        }
        private ItemPermission CreatePermission(dtoRepositoryItem item, Int32 idPerson,liteRepositorySettings settings, ModuleRepository modulePermissions, Boolean forAdmin = false)
        {
            ItemPermission permissions = new ItemPermission();
            Boolean isOwner = (item.IdOwner==idPerson);
            Boolean manageOthers = (modulePermissions.Administration ||  modulePermissions.ManageItems || modulePermissions.EditOthersFiles);
            if (IsUserAvailable)
                IsUserAvailable = manageOthers || (((isOwner && item.IsVisible) || item.IsVisible) && (item.Status== ItemStatus.Active || isOwner) );
            item.AllowUpload = (!item.IsFile && item.FolderType == Domain.FolderType.standard && (item.AllowUpload || modulePermissions.Administration || modulePermissions.ManageItems || isOwner));
            permissions.AllowUpload = item.AllowUpload;
            switch (item.Type)
            {
                case ItemType.File:
                    permissions.Unzip = (item.Availability== ItemAvailability.available) && ((item.Extension == ".zip") && (manageOthers || (modulePermissions.EditMyFiles && isOwner) || (modulePermissions.UploadFile && isOwner)));
                    permissions.Zip = (item.Availability== ItemAvailability.available) && (manageOthers || modulePermissions.DownloadOrPlay || (modulePermissions.EditMyFiles && isOwner) || (modulePermissions.UploadFile && isOwner));
                    break;
                case ItemType.Folder:
                    break;
                case ItemType.Multimedia:
                case ItemType.ScormPackage:
                    permissions.CanEditSettings = ((item.Deleted == DomainModel.BaseStatusDeleted.None && manageOthers || (modulePermissions.EditMyFiles && isOwner) || (modulePermissions.UploadFile && isOwner && item.Availability == ItemAvailability.available)));
                    permissions.EditSettings =  IsAvailableByType(item.Type,item.Availability) && ((item.Deleted== DomainModel.BaseStatusDeleted.None &&  manageOthers || (modulePermissions.EditMyFiles && isOwner) || (modulePermissions.UploadFile && isOwner && item.Availability== ItemAvailability.available )));
                    permissions.Zip = (item.Availability != ItemAvailability.notavailable) && (manageOthers || modulePermissions.DownloadOrPlay || (modulePermissions.EditMyFiles && isOwner) || (modulePermissions.UploadFile && isOwner));
                    break;
                case ItemType.Link:
                    break;
            }
            permissions.Edit = manageOthers || (modulePermissions.EditMyFiles && isOwner) || (modulePermissions.UploadFile && isOwner);
            permissions.ViewMyStatistics = ((item.Deleted == DomainModel.BaseStatusDeleted.None && item.Type != ItemType.Link && item.Type != ItemType.Folder && item.Type != ItemType.File) && !(manageOthers || (modulePermissions.EditMyFiles && isOwner && IsAvailableByType(item.Type, item.Availability)) || (modulePermissions.UploadFile && isOwner && IsAvailableByType(item.Type, item.Availability))));
            permissions.ViewOtherStatistics = (item.Deleted == DomainModel.BaseStatusDeleted.None && item.Type != ItemType.Link && item.Type != ItemType.Folder && item.Type != ItemType.File) && (manageOthers || (modulePermissions.EditMyFiles && isOwner && IsAvailableByType(item.Type, item.Availability)) || (modulePermissions.UploadFile && isOwner && IsAvailableByType(item.Type, item.Availability)));

            permissions.VersioningAvailable = (item.Availability != ItemAvailability.notavailable) && ((item.Deleted == DomainModel.BaseStatusDeleted.None && item.Type != ItemType.Folder && item.Type != ItemType.Link && settings.AllowVersioning));

            permissions.ViewPermission =  permissions.Edit;
            permissions.EditPermission = manageOthers || (modulePermissions.EditMyFiles && isOwner) || (modulePermissions.UploadFile && isOwner && item.Availability!= ItemAvailability.available);
            permissions.Delete = (item.Deleted != DomainModel.BaseStatusDeleted.None && (manageOthers || (modulePermissions.DeleteMyFiles && isOwner)));
            permissions.UnDelete = (item.Deleted != DomainModel.BaseStatusDeleted.None && (manageOthers || (modulePermissions.DeleteMyFiles && isOwner)));
            permissions.VirtualDelete = (item.Deleted == DomainModel.BaseStatusDeleted.None && (manageOthers || (modulePermissions.DeleteMyFiles && isOwner)));
           

            permissions.Download = (item.Availability != ItemAvailability.notavailable) && (manageOthers || (item.IsDownloadable && item.Type !=  ItemType.Folder && permissions.Download));
            permissions.Play = IsAvailableByType(item.Type, item.Availability) && (item.Type == ItemType.Multimedia || item.Type == ItemType.ScormPackage) && ((manageOthers || permissions.Download) && item.Type != ItemType.Folder);
            permissions.Preview = IsAvailableByType(item.Type, item.Availability) && (item.DisplayMode == Domain.DisplayMode.inModal || item.DisplayMode == Domain.DisplayMode.downloadOrPlayOrModal);
            permissions.AllowSelection = (item.Availability != ItemAvailability.notavailable  && (isOwner || manageOthers || item.IsDownloadable || item.Type == ItemType.Link || item.Type == ItemType.Folder));
            permissions.Hide = (item.Deleted == DomainModel.BaseStatusDeleted.None && permissions.Edit && item.IsVisible);
            permissions.Show = (item.Deleted == DomainModel.BaseStatusDeleted.None && permissions.Edit && !item.IsVisible);
            return permissions;
        }
        public static Boolean IsAvailableByType(ItemType type, ItemAvailability available)
        {
            switch (type)
            {
                case ItemType.File:
                    return available != ItemAvailability.notavailable;
                case ItemType.Folder:
                    return available != ItemAvailability.notavailable;
                case ItemType.Link:
                    return available != ItemAvailability.notavailable;
                case ItemType.Multimedia:
                case  ItemType.ScormPackage:
                case ItemType.SharedDocument:
                case ItemType.VideoStreaming:
                    return (available == ItemAvailability.available || available == ItemAvailability.waitingsettings);
            }
            return false;
        }
        public List<dtoDisplayRepositoryItem> GetFoldersTree(Boolean onlyAvailable, Boolean removeDeleted = true, dtoDisplayRepositoryItem father = null)
        {
            List<dtoDisplayRepositoryItem> results = new List<dtoDisplayRepositoryItem>();
            if (IsAvailableForDelete(Deleted, removeDeleted) && (Father == null || (Father != null && IsAvailableForDelete(Father.Deleted, removeDeleted))))
            {
                dtoDisplayRepositoryItem folder = null;
                if (Type == ItemType.Folder && (!onlyAvailable || (onlyAvailable && IsUserAvailable)))
                {
                    folder = (dtoDisplayRepositoryItem)this.Clone();
                    folder.IdentifierPath = CreateIdentifierPath(folder.Id, (father == null) ? "" : father.IdentifierPath );
                    folder.Father = father;
                }
                if (Type == ItemType.Folder && Children.Any(c => c.Type == ItemType.Folder))
                    Children.Where(c => c.Type == ItemType.Folder).ToList().ForEach(c => folder.Children.AddRange(c.GetFoldersTree(onlyAvailable, removeDeleted, folder)));
                results.Add(folder);
            }
            return results;
        }
        #region "Get items"
            public List<dtoDisplayRepositoryItem> GetDeletedItems()
            {
                List<dtoDisplayRepositoryItem> results = new List<dtoDisplayRepositoryItem>();
                if (Father == null || Father.Deleted == DomainModel.BaseStatusDeleted.None)
                {
                    if (Deleted == DomainModel.BaseStatusDeleted.Manual)
                        results.Add(this);
                    else if (Type == ItemType.Folder && Children.Any())
                        Children.ForEach(c => results.AddRange(c.GetDeletedItems()));
                }
                return results;
            }
            public List<dtoDisplayRepositoryItem> GetItems(ItemType type, Boolean onlyAvailable, Boolean removeDeleted = true  )
            {
                List<dtoDisplayRepositoryItem> results = new List<dtoDisplayRepositoryItem>();
                if (IsAvailableForDelete(Deleted, removeDeleted) && (Father == null || (Father != null && IsAvailableForDelete(Father.Deleted, removeDeleted))))
                {
                    if (type== Type && (!onlyAvailable || (onlyAvailable && IsUserAvailable)))
                        results.Add(this);
                    if (Type == ItemType.Folder && Children.Any())
                        Children.ForEach(c => results.AddRange(c.GetItems(type, onlyAvailable, removeDeleted)));
                }
                return results;
            }
        #endregion

        #region "Search"
            public List<long> GetIdChildrenFolders()
            {
                List<long> idFolders = new List<long>();
                if (Children.Any(c => c.Type == ItemType.Folder && FolderType== Domain.FolderType.standard))
                {
                    idFolders = Children.Where(c => c.Type == ItemType.Folder && FolderType == Domain.FolderType.standard).Select(c => c.Id).ToList();
                    idFolders.AddRange(Children.SelectMany(c => c.GetIdChildrenFolders()).ToList());
                }
                return idFolders;
            }
            public List<dtoItemFile> GetLiteFiles(Boolean onlyAvailable, Boolean removeDeleted = true)
            {
                List<dtoItemFile> files = new List<dtoItemFile>();
                if (Type != ItemType.Folder && IsAvailableForDelete(Deleted, removeDeleted) && (!onlyAvailable || (onlyAvailable && IsUserAvailable)))
                    files.Add(new dtoItemFile() { Id = this.Id, Type = this.Type, ItemReferrer=this  });
                else if (Type == ItemType.Folder && FolderType == Domain.FolderType.standard && Children.Any())
                    files.AddRange(Children.SelectMany(c => c.GetLiteFiles(onlyAvailable, removeDeleted)).ToList());
                return files;
            }
            public Boolean HasAvailable(ItemType type, Boolean removeDeleted = true)
            {
                return ((Type == type && IsUserAvailable && IsAvailableForDelete(Deleted ,removeDeleted)) || Children.Any(c => c.HasAvailable(type, removeDeleted)));
            }
            public Boolean HasAvailable(lm.Comol.Core.DomainModel.BaseStatusDeleted deleted)
            {
                return (Deleted == deleted && IsUserAvailable )|| Children.Any(c => c.HasAvailable(deleted));
            }
            public Boolean ContainsFolder(long idFolder, FolderType folderType, String identifierPath, Boolean removeDeleted = true)
            {
                return (Id == idFolder && FolderType == folderType && (String.IsNullOrWhiteSpace(identifierPath) || identifierPath == IdentifierPath) && IsAvailableForDelete(Deleted, removeDeleted)) || Children.Any(c => c.Type== ItemType.Folder && c.ContainsFolder(idFolder, folderType,identifierPath, removeDeleted));
            }
            private Boolean IsItemAvailable(dtoDisplayRepositoryItem item, List<ItemType> types, Boolean onlyAvailable, List<FolderType> folderTypes, Boolean removeDeleted)
            {
                return types.Contains(item.Type) && IsItemAvailable(item, onlyAvailable, folderTypes, removeDeleted);
            }
            private Boolean IsItemAvailable(dtoDisplayRepositoryItem item, Boolean onlyAvailable, List<FolderType> folderTypes, Boolean removeDeleted)
            {
                return (!onlyAvailable || (onlyAvailable && IsUserAvailable))  && (!removeDeleted || (removeDeleted && item.Deleted == DomainModel.BaseStatusDeleted.None))
                    && (folderTypes == null || !folderTypes.Any() || folderTypes.Contains(item.FolderType));
            }
            private Boolean IsAvailableForDelete(DomainModel.BaseStatusDeleted deleted,Boolean removeDeleted)
            {
                return (!removeDeleted || (removeDeleted && deleted == DomainModel.BaseStatusDeleted.None));
            }
            public dtoDisplayRepositoryItem GetFolder(long idFolder, FolderType folderType,String identifierPath = "", Boolean removeDeleted = true)
            {
                if (Id == idFolder && (String.IsNullOrEmpty(identifierPath) || identifierPath==IdentifierPath)  && FolderType == folderType && IsAvailableForDelete(Deleted, removeDeleted))
                    return this;
                else if (Children.Any(c => c.Type == ItemType.Folder))
                    return Children.Where(c => c.Type == ItemType.Folder && IsAvailableForDelete(c.Deleted, removeDeleted)).Select(c => c.GetFolder(idFolder, folderType, identifierPath, removeDeleted)).Where(f => f != null).FirstOrDefault();
                else
                    return null;
            }

            public List<dtoFolderItem> GetBreadCrumb(Boolean onlyAvailable, RepositoryType type, Int32 idCommunity = -1, Boolean removeDeleted = true)
            {

                List<dtoFolderItem> folders = new List<dtoFolderItem>();
                dtoFolderItem folder = new dtoFolderItem() { Id = Id, FolderType = FolderType, Name = Name, IdentifierPath = IdentifierPath,TemplateUrl = RootObject.FolderUrlTemplate(Id, FolderType, IdentifierPath, type, idCommunity) };
                folder.IsCurrent = true;
                if (Father != null)
                    folders.AddRange(Father.GetBreadCrumb(this, onlyAvailable, type, idCommunity,  removeDeleted));
                folders.Add(folder);
                return folders;
            }
            private List<dtoFolderItem> GetBreadCrumb(dtoDisplayRepositoryItem child, Boolean onlyAvailable, RepositoryType type, Int32 idCommunity = -1, Boolean removeDeleted = true)
            {
                List<dtoFolderItem> folders = new List<dtoFolderItem>();
                dtoFolderItem folder = new dtoFolderItem() { Id = Id, FolderType = FolderType, Name = Name, IdentifierPath = IdentifierPath, TemplateUrl = RootObject.FolderUrlTemplate(Id, FolderType, IdentifierPath, type, idCommunity) };
                folder.IsInCurrentPath = true;
                if (Father != null)
                    folders.AddRange(Father.GetBreadCrumb(this, onlyAvailable, type, idCommunity,  removeDeleted));
                folders.Add(folder);
                return folders;
            }
            public Boolean HasSpecialFolderFather()
            {
                if (Father == null)
                    return FolderType != Domain.FolderType.standard && Type == ItemType.Folder;
                else
                    return Father.HasSpecialFolderFather();
            }
            private List<FolderType> GetFolderTypesForBreadCrumb()
            {
                List<FolderType> fTypes = new List<FolderType>() {FolderType.standard, FolderType.recycleBin};
                return fTypes;
            }
            public dtoDisplayRepositoryItem GetItem(long idItem)
            {
                if (idItem == Id)
                    return this;
                else
                    return Children.Select(c => c.GetItem(idItem)).Where(i => i != null).FirstOrDefault();
            }
            public List<dtoDisplayRepositoryItem> GetItems(List<long> idItems)
            {
                if (idItems.Contains(Id))
                    return new List<dtoDisplayRepositoryItem>() { this };
                else
                    return Children.Select(c => c.GetItems(idItems)).Where(i => i != null).SelectMany(i=> i.ToList()).ToList();
            }
        #endregion


        #region "List Nodes Loading"
            public List<dtoNodeItem> GetFoldersNodes(List<FolderType> folderTypes, dtoDisplayRepositoryItem currentFolder, RepositoryType type, Int32 idCommunity = -1,  Boolean onlyAvailable = true, Boolean alsoTagChildren = false, Boolean removeDeleted = true)
            {
                return GetItemsNodes(new List<ItemType>() { ItemType.Folder }, folderTypes, currentFolder, type, idCommunity,  onlyAvailable, alsoTagChildren, removeDeleted);
            }
            public List<dtoNodeItem> GetItemsNodes(List<ItemType> types, List<FolderType> folderTypes, dtoDisplayRepositoryItem currentFolder, RepositoryType type, Int32 idCommunity = -1,  Boolean onlyAvailable = true, Boolean alsoTagChildren = false, Boolean removeDeleted = true)
            {
                List<dtoNodeItem> nodes = new List<dtoNodeItem>();
                if (IsItemAvailable(this, types, onlyAvailable,folderTypes, removeDeleted))
                    nodes.AddRange(CreateItemsNodes(null, this, types, folderTypes, currentFolder, type, idCommunity,  onlyAvailable, alsoTagChildren, removeDeleted));
                return nodes;
            }
            private List<dtoNodeItem> CreateItemsNodes(dtoNodeItem father, dtoDisplayRepositoryItem item, List<ItemType> types, List<FolderType> folderTypes, dtoDisplayRepositoryItem currentFolder, RepositoryType type, Int32 idCommunity = -1,  Boolean onlyAvailable = true, Boolean alsoTagChildren = false, Boolean removeDeleted = true)
            {
                List<dtoNodeItem> results = new List<dtoNodeItem>();

                dtoNodeItem cNode = CreateItemNode(father, item, currentFolder, type, idCommunity);
                dtoNodeItem openNode = CreateOpenNode(cNode, item.Children.Any());

                results.Add(openNode);
                results.Add(cNode);
                if (item.Children.Any(c => IsItemAvailable(c, types, onlyAvailable, folderTypes, removeDeleted)))
                {
                    results.Add(new dtoNodeItem() { Type = NodeType.OpenChildren });
                    item.Children.Where(c => IsItemAvailable(c, types, onlyAvailable, folderTypes, removeDeleted)).ToList().ForEach(n => results.AddRange(CreateItemsNodes(openNode, n, types, folderTypes, currentFolder, type, idCommunity,  onlyAvailable, alsoTagChildren, removeDeleted)));
                    results.Add(new dtoNodeItem() { Type = NodeType.CloseChildren });
                }
                else
                    results.Add(new dtoNodeItem() { Type = NodeType.NoChildren });

                results.Add(new dtoNodeItem() { Type = NodeType.CloseNode });
                return results;
            }

            private dtoNodeItem CreateItemNode(dtoNodeItem father, dtoDisplayRepositoryItem item, dtoDisplayRepositoryItem currentFolder, RepositoryType type, Int32 idCommunity = -1)
            {
                dtoNodeItem node = new dtoNodeItem() { Type = NodeType.Item, IsCurrent = (currentFolder != null && currentFolder.Id == item.Id && currentFolder.FolderType == item.FolderType && currentFolder.IdentifierPath == item.IdentifierPath) };
                node.Id = item.Id;
                node.IdFolder = item.IdFolder;
                node.FolderType = item.FolderType;
                node.IdCommunity = item.IdCommunity;
                node.ItemType = item.Type;
                node.Name = item.DisplayName;
                node.IdentifierPath = item.IdentifierPath;
                if (father != null) {
                    node.Fathers.Add(father);
                    node.Fathers.AddRange(father.Fathers);
                }
                node.HasCurrent = (currentFolder != null && (currentFolder.GetIdFathers().Where(f => f > 0).Contains(item.Id) || item.ContainsFolder(currentFolder.Id, currentFolder.FolderType, currentFolder.IdentifierPath)));
                switch (item.Type)
                {
                    case ItemType.Folder:
                        switch (item.FolderType)
                        {
                            case Domain.FolderType.standard:
                                node.TemplateNavigateUrl = RootObject.FolderUrlTemplate(item.Id, item.FolderType, type, idCommunity);
                                break;
                            default:
                                node.TemplateNavigateUrl = RootObject.FolderUrlTemplate(item.Id, item.FolderType, item.IdentifierPath, type, idCommunity);
                                break;
                        }
                        break;
                }

                return node;
            }

            private dtoNodeItem CreateOpenNode(dtoNodeItem owner, Boolean hasChildren)
            {
                dtoNodeItem node = new dtoNodeItem() { Type = NodeType.OpenItemNode};
                node.Id = owner.Id;
                node.IdCommunity = owner.IdCommunity;
                node.IdFolder = owner.IdFolder;
                node.FolderType = owner.FolderType;
                node.IsCurrent = owner.IsCurrent;
                node.IsEmpty = !hasChildren;
                node.HasCurrent = owner.HasCurrent;
                node.Selectable = owner.Selectable;
                node.IdentifierPath = owner.IdentifierPath;

                return node;
            }
        #endregion

        #region "Common"
            public String GetSize(Int32 decimals = 2)
            {
                return FolderSizeItem.FormatBytes(Size, decimals);
            }
            public static dtoDisplayRepositoryItem GenerateFolder(FolderType type, String name, dtoDisplayRepositoryItem father = null)
            {
                dtoDisplayRepositoryItem folder = GenerateFolder(name);
                folder.Id = -(int)type;
                folder.FolderType = type;
                folder.IdFolder = (father == null) ? 0 : father.Id;
                folder.IdentifierPath = CreateIdentifierPath(folder.Id, (father == null) ? "": father.IdentifierPath);
                folder.Father = father;
                folder.Identifier.Type = ItemIdentifierType.standard;
                folder.Identifier.FolderType = type;
                return folder;
            }
        
            public static dtoDisplayRepositoryItem GenerateFolder(String name)
            {
                dtoDisplayRepositoryItem folder = new dtoDisplayRepositoryItem();
                folder.UniqueId = Guid.Empty;
                folder.Name = name;
                folder.IsVisible = true;
                folder.Availability = ItemAvailability.available;
                folder.IsUserAvailable = true;
                folder.Status = ItemStatus.Active;
                folder.Type = ItemType.Folder;
                folder.Children = new List<dtoDisplayRepositoryItem>();
                folder.IsVirtual = true;
                return folder;
            }
            public object Clone()
            {
                dtoDisplayRepositoryItem clone = new dtoDisplayRepositoryItem();
                clone.Children = new List<dtoDisplayRepositoryItem>();
                clone.IsUserAvailable = this.IsUserAvailable;
                clone.Permissions = this.Permissions;
                clone.Id = this.Id;
                clone.IdFolder = this.IdFolder;
                clone.UniqueId = this.UniqueId;
                clone.IdVersion = this.IdVersion;
                clone.UniqueIdVersion = this.UniqueIdVersion;
                clone.IdCommunity = IdCommunity;
                clone.Type = Type;
                clone.Name = Name;
                clone.Extension = Extension;
                clone.Url = Url;
                clone.ContentType = ContentType;
                clone.IsFile = IsFile;
                clone.IdOwner = IdOwner;
                clone.OwnerName = OwnerName;
                clone.CreatedOn = CreatedOn;
                clone.IdModifiedBy = IdModifiedBy;
                clone.ModifiedOn = ModifiedOn;
                clone.ModifiedBy = ModifiedBy;
                clone.Size = Size;
                clone.VersionsSize = VersionsSize;
                clone.DeletedSize = DeletedSize;
                clone.Description = Description;
                clone.Tags = Tags;
                clone.IsDownloadable = IsDownloadable;
                clone.Deleted = Deleted;
                clone.CloneOf = CloneOf;
                clone.CloneOfUniqueId = CloneOfUniqueId;
                clone.AllowUpload = AllowUpload;
                clone.FolderType = FolderType;
                clone.IsVisible = IsVisible;
                clone.DisplayOrder = DisplayOrder;
                clone.Availability = Availability;
                clone.Status = Status;
                clone.HasVersions = HasVersions;
                clone.DisplayMode = DisplayMode;
                clone.RevisionsNumber = RevisionsNumber;
                clone.Thumbnail = Thumbnail;
                clone.AutoThumbnail = AutoThumbnail;
                clone.PreviewTime = PreviewTime;
                clone.Time = Time;
                clone.Number = Number;
                clone.IdPlayer = IdPlayer;
                clone.Downloaded = Downloaded;
                clone.IsInternal = IsInternal;
                clone.Module = Module;
                clone.IsVirtual = IsVirtual;
                clone.Path = Path;
                if (Identifier!=null)
                    clone.Identifier = (dtoItemIdentifier)Identifier.Clone();
                return clone;
            }
            public List<dtoDisplayRepositoryItem> GetPlainList()
            {
                List<dtoDisplayRepositoryItem> items = new List<dtoDisplayRepositoryItem>();
                items.Add((dtoDisplayRepositoryItem)Clone());
                items.AddRange(Children.SelectMany(c=>c.GetPlainList()));
                return items;
            }

            public void AddFiles(IEnumerable<dtoDisplayRepositoryItem> children)
            {
                foreach (dtoDisplayRepositoryItem child in children) {
                    child.IdentifierPath = CreateIdentifierPath(child.Id,IdentifierPath);

                    Children.Add(child);
                }
            }
            public String ToString()
            {
                return "Id " + Id + " " + DisplayName + " " + Type.ToString() + "[" + Path + "] " + (Type == ItemType.Folder ? "-Ftype=" + FolderType.ToString() : "");
            }
            public static String CreateIdentifierPath(long idItem, String previousPath = "")
            {
                return String.IsNullOrEmpty(previousPath) ? "." + idItem + "." : previousPath + idItem + ".";
            }
            public List<long> GetIdFathers()
            {
                if (Father == null)
                    return new List<long>();
                else{
                    List<long> items = new List<long>() { Father.Id };
                    items.AddRange(Father.GetIdFathers());
                    return items;
                }
            }
            public List<ItemAction> GetChildrenActions()
            {
                return (Children == null || !Children.Any()) ? new List<ItemAction>() : Children.SelectMany(c => c.Permissions.GetMultipleActions()).Distinct().ToList();
            }
        #endregion
    }
  }