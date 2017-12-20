using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class RepositoryItem : BaseRepositoryItem
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual long IdFolder { get; set; }
        public virtual System.Guid UniqueId { get; set; }
   
        public virtual String Extension { get; set; }
        public virtual String Url { get; set; }
        public virtual String DisplayName
        {
            get
            {
                switch (Type)
                {
                    case ItemType.File:
                        return Name + Extension;
                    case ItemType.Multimedia:
                    case ItemType.ScormPackage:
                    case ItemType.Folder:
                        return Name;
                    case ItemType.Link:
                        return (String.IsNullOrWhiteSpace(Name) ? Url : Name);
                    default:
                        return Name;
                }
            }
        }
        public virtual String DisplayFullName
        {
            get
            {
                switch (Type)
                {
                    case ItemType.File:
                    case ItemType.Multimedia:
                    case ItemType.ScormPackage:
                    case ItemType.VideoStreaming:
                        return Name + Extension;
                    case ItemType.Folder:
                        return Name;
                    case ItemType.Link:
                        return (String.IsNullOrWhiteSpace(Name) ? Url : Name);
                    default:
                        return Name;
                }
            }
        }
        

        public virtual String ContentType { get; set; }

        public virtual Boolean IsFile { get; set; }
        public virtual long VersionsSize { get; set; }
        public virtual long DeletedSize { get; set; }
        public virtual Boolean HasVersions { get; set; }
        public virtual IList<RepositoryItemVersion> Versions { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual String Tags { get; set; }
        public virtual long CloneOf { get; set; }
        public virtual System.Guid CloneOfUniqueId { get; set; }
        public virtual Boolean IsDownloadable { get; set; }

        public virtual Boolean IsInternal { get; set; }
        public virtual ItemModuleSettings Module { get; set; }
        public virtual Boolean IsVirtual { get; set; }
        public virtual Boolean IsVisible { get; set; }
        public virtual Boolean IsPersonal { get; set; }
        
        public virtual long DisplayOrder { get; set; }
        public virtual long RevisionsNumber { get; set; }
        
        public virtual Boolean AllowUpload { get; set; }
        public virtual long IdPlayer { get; set; }
       
        public RepositoryItem() {
            Versions = new List<RepositoryItemVersion>();
            Module = new ItemModuleSettings();
            Repository = new RepositoryIdentifier();
        }

        public static RepositoryItem CreateFolder(long idFather, dtoFolderName dto, lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress, RepositoryType type, Int32 idCommunity)
        {
            RepositoryItem folder = RepositoryItem.CreateBaseItem(idFather, ItemType.Folder, dto.Name, "", "", true, DisplayMode.downloadOrPlay, dto.IsVisible, person, date, ipAddress, proxyIpAddress,  type, idCommunity);
            folder.AllowUpload = dto.AllowUpload;
            return folder;
        }
        public static RepositoryItem CreateLink(long idFather, dtoUrlItem dto, lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress, RepositoryType type, Int32 idCommunity)
        {
            return RepositoryItem.CreateBaseItem(idFather, ItemType.Link, dto.Name, "", dto.Address, true, DisplayMode.downloadOrPlay, dto.IsVisible, person, date, ipAddress, proxyIpAddress, type, idCommunity);
        }
        public static RepositoryItem CreateFile(long idFather, dtoUploadedItem dto, Boolean isDownloadable, DisplayMode mode,lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress, RepositoryIdentifier identifier)
        {
            return RepositoryItem.CreateBaseItem(idFather, dto, isDownloadable, mode, person, date, ipAddress, proxyIpAddress, identifier);
        }
        public static RepositoryItem CreateFile(long idFather, dtoUploadedItem dto, Boolean isDownloadable,DisplayMode mode, lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress, RepositoryType type, Int32 idCommunity)
        {
            return CreateFile(idFather, dto, isDownloadable, mode, person, date, ipAddress, proxyIpAddress, RepositoryIdentifier.Create(type, idCommunity));
        }
        public static RepositoryItem CreateFile(dtoUploadedItem dto, Boolean isDownloadable,  DisplayMode mode,lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress, RepositoryIdentifier identifier, Object obj, long idObject, Int32 idObjectType, Int32 idModule, String moduleCode, Int32 idModuleAjaxAction, Int32 idModuleAction = 0)
        {
            RepositoryItem item = RepositoryItem.CreateBaseItem(0, dto, isDownloadable,  mode,person, date, ipAddress, proxyIpAddress, identifier.Type, identifier.IdCommunity);
            item.IsInternal = true;
            item.Module = new ItemModuleSettings();
            item.Module.ModuleCode = moduleCode;
            item.Module.IdObjectType = idObjectType;
            item.Module.IdObject = idObject;
            item.Module.IdModuleAction = idModuleAction;
            item.Module.FullyQualifiedName = obj.GetType().FullName;
            item.Module.IdModuleAjaxAction = idModuleAjaxAction;
            return item;
        }
    

        public virtual RepositoryItemVersion CreateFirstVersion()
        {
            RepositoryItemVersion version = new RepositoryItemVersion();
            version.Availability = Availability;
            version.CreateMetaInfo(IdCreatedBy, CreatorIpAddress,CreatorProxyIpAddress, CreatedOn);
            version.Deleted= DomainModel.BaseStatusDeleted.None;
            version.DisplayMode = DisplayMode;
            version.Downloaded = Downloaded;
            version.File = this;
            version.IdCommunity = IdCommunity;
            version.IdPlayer = IdPlayer;
            version.IsActive = true;
            version.Number = 0;
            version.IdOwner = IdOwner;
            version.PreviewTime = PreviewTime;
            version.Size = Size;
            version.Status = Status;
            version.Thumbnail = Thumbnail;
            version.AutoThumbnail = AutoThumbnail;
            version.Time = Time;
            version.Type = Type;
            version.UniqueIdItem = UniqueId;
            version.UniqueIdVersion = UniqueId;
            version.Name = Name;
            version.Description = Description;
            version.Url = Url;
            version.Extension = Extension;
            version.ContentType = ContentType;
            version.Repository = Repository;
            return version;
        }

        public static RepositoryItemVersion CreateNewVersion(RepositoryItem file, dtoUploadedItem versionToAdd, Int32 idUser, String ipAddress, String proxyIpAddress)
        {
            RepositoryItemVersion version = new RepositoryItemVersion();
            version.Availability =  ItemAvailability.available;
            version.CreateMetaInfo(idUser, ipAddress, proxyIpAddress);
            version.Deleted = DomainModel.BaseStatusDeleted.None;
            version.DisplayMode = file.DisplayMode;
            version.Downloaded = 0;
            version.File = file;
            version.IdCommunity = file.IdCommunity;
            version.IdPlayer = file.IdPlayer;
            version.IsActive = true;
            version.Number = file.Number + 1;
            version.IdOwner = file.IdOwner;
            version.PreviewTime = file.PreviewTime;
            version.Size = versionToAdd.Size;
            version.Status = file.Status;
            if (versionToAdd.HasThumbnail)
            {
                version.Thumbnail = versionToAdd.ThumbnailFileName;
                version.AutoThumbnail = versionToAdd.AutoThumbnail;
                file.Thumbnail = versionToAdd.ThumbnailFileName;
                file.AutoThumbnail = versionToAdd.AutoThumbnail;
            }
            else
            {
                version.Thumbnail = (file.AutoThumbnail ? "" : file.Thumbnail);
                version.AutoThumbnail = file.AutoThumbnail;
            }
            version.Time = file.Time;
            version.Type = file.Type;
            version.UniqueIdItem = file.UniqueId;
            version.UniqueIdVersion = versionToAdd.UniqueId;
            version.Name = file.Name;
            version.Description = file.Description;
            version.Url = file.Url;
            version.Extension = file.Extension;
            version.ContentType = file.ContentType;
            version.Repository = file.Repository;
            return version;
        }

        private static RepositoryItem CreateBaseItem(long idFather, dtoUploadedItem dto, Boolean isDownloadable, DisplayMode mode, lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress, RepositoryType type, Int32 idCommunity)
        {
            return CreateBaseItem(idFather, dto, isDownloadable, mode, person, date, ipAddress, proxyIpAddress, RepositoryIdentifier.Create(type, idCommunity));
        }
        private static RepositoryItem CreateBaseItem(long idFather, dtoUploadedItem dto, Boolean isDownloadable, DisplayMode mode, lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress, RepositoryIdentifier identifier)
        {
            RepositoryItem item = RepositoryItem.CreateBaseItem(idFather, dto.Type, dto.Name, dto.Extension, "", isDownloadable, mode, dto.IsVisible, person, date, ipAddress, proxyIpAddress, identifier);
            item.UniqueId = dto.UniqueId;
            item.UniqueIdVersion = dto.UniqueId;
            item.Size = dto.Size;
            item.ContentType = dto.ContentType;
            item.IsDownloadable = isDownloadable;
            item.Type = dto.Type;
            switch(dto.Type){
                case ItemType.Multimedia:
                case ItemType.ScormPackage:
                    item.Availability = ItemAvailability.transfer;
                    item.Status= ItemStatus.Active;
                    break;
                default:
                    item.Availability = ItemAvailability.available;
                    item.Status = ItemStatus.Active;
                    break;
            }
            return item;
        }

        private static RepositoryItem CreateBaseItem(long idFather, ItemType itemType, String name, String extension, String url, Boolean isDownloadable, DisplayMode mode, Boolean isVisible, lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress, RepositoryType type, Int32 idCommunity)
        {
            return CreateBaseItem(idFather, itemType, name, extension, url, isDownloadable, mode, isVisible, person, date, ipAddress, proxyIpAddress, RepositoryIdentifier.Create(type, idCommunity));
        }
        private static RepositoryItem CreateBaseItem(long idFather, ItemType itemType, String name, String extension, String url,Boolean isDownloadable, DisplayMode mode, Boolean isVisible, lm.Comol.Core.DomainModel.litePerson person, DateTime date, String ipAddress, String proxyIpAddress,RepositoryIdentifier identifier)
        {
            RepositoryItem item = new RepositoryItem();
            item.AllowUpload = false;
            item.Availability = ItemAvailability.available;
            item.CloneOf = 0;
            item.CloneOfUniqueId = Guid.Empty;
            item.ContentType = "";
            item.CreateMetaInfo(person.Id, ipAddress, proxyIpAddress, date);
            item.DeletedSize = 0;
            item.Description = "";
            item.DisplayMode = mode;
            item.DisplayOrder = 0;
            item.Downloaded = 0;
            item.IdCommunity = (identifier.Type == RepositoryType.Community) ? identifier.IdCommunity : 0;
            item.Repository = identifier;
            item.IdFolder = idFather;
            item.IsDownloadable = isDownloadable;
            item.IsFile = (itemType!= ItemType.Folder);
            item.IsInternal = false;
            item.UniqueId = Guid.NewGuid();
            item.UniqueIdVersion = item.UniqueId;
            item.VersionsSize = 0;
            item.Name = name;
            item.Extension = extension;
            item.Url = url;
            item.IsVirtual = false;
            item.IsVisible = isVisible;
            item.Number = 0;
            item.IdOwner = person.Id;
            item.Size = 0;
            item.Status = ItemStatus.Active;
            item.Tags = "";
            item.Thumbnail = "";
            item.AutoThumbnail = false;
            item.Type = itemType;
            item.Module = null;
            return item;
        }
    }
}