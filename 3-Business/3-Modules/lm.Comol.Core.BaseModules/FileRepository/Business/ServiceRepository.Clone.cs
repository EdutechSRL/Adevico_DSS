using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Business
{
    public partial class ServiceRepository
    {
        public Boolean CloneInternalItem(long idObjectOwner, Int32 idModule, String moduleCode, long idItem, long idVersion, litePerson person, String baseFilePath, String baseThumbnailPath, RepositoryIdentifier destIdentifier, ref RepositoryItem itemCloned, ref RepositoryItemVersion versionCloned, Boolean onlyLastVersion = true)
        {
            Boolean cloned = false;
            Boolean isInTransaction = Manager.IsInTransaction();
            String destFile = "";
            String dThumbnail = "";
            try
            {
                if (!isInTransaction)
                    Manager.BeginTransaction();
                RepositoryItem item = Manager.Get<RepositoryItem>(idItem);
                if (item != null)
                {
                    RepositoryItemVersion version = Manager.Get<RepositoryItemVersion>((idVersion>0 ? idVersion : item.IdVersion));
                    if (version != null)
                    {
                        String sourcePath = GetItemDiskPath(baseFilePath, item.Repository);
                        String destinationPath = System.IO.Path.Combine(baseFilePath, item.Repository.IdCommunity.ToString());
                        itemCloned = CloneItem(person.Id, item, version, destIdentifier, !onlyLastVersion, idObjectOwner);

                        if(!String.IsNullOrWhiteSpace(itemCloned.Thumbnail)){
                            String stPath = GetItemDiskPath(baseThumbnailPath, item.Repository);
                            String dtPath = GetItemDiskPath(baseThumbnailPath, itemCloned.Repository);
                            if (!lm.Comol.Core.File.Exists.Directory(dtPath))
                                lm.Comol.Core.File.Create.Directory(dtPath);
                            if (!lm.Comol.Core.File.Create.CopyFile(System.IO.Path.Combine(stPath, item.Thumbnail),System.IO.Path.Combine(dtPath, itemCloned.Thumbnail)))
                                itemCloned.Thumbnail = "";
                        }
                        String sourceFile = "";
                        switch(itemCloned.Type){
                            case ItemType.Folder:
                            case ItemType.Link:
                                break;
                            default:
                                sourceFile = System.IO.Path.Combine(sourcePath, item.UniqueId.ToString() + item.Extension);
                                destFile = System.IO.Path.Combine(destinationPath, itemCloned.UniqueId.ToString() + itemCloned.Extension);
                                if (!lm.Comol.Core.File.Exists.Directory(destinationPath))
                                    lm.Comol.Core.File.Create.Directory(destinationPath);
                                if (!lm.Comol.Core.File.Create.CopyFile(sourceFile, destFile))
                                {
                                    itemCloned = null;
                                    destFile = "";
                                }
                                break;
                        }
                        if (itemCloned != null)
                        {
                            Manager.SaveOrUpdate(itemCloned);

                            List<String> tags = null;
                            if (!String.IsNullOrWhiteSpace(itemCloned.Tags))
                                tags = itemCloned.Tags.Split(',').ToList();
                            if (tags!=null && tags.Any())
                                ServiceTags.SaveTags(person, destIdentifier.IdCommunity, idModule, moduleCode, tags);


                            versionCloned = itemCloned.CreateFirstVersion();
                            Manager.SaveOrUpdate(versionCloned);
                            itemCloned.IdVersion = versionCloned.Id;
                            Manager.SaveOrUpdate(itemCloned);

                            switch (itemCloned.Type)
                            {
                                case ItemType.VideoStreaming:
                                case ItemType.ScormPackage:
                                case ItemType.Multimedia:
                                    FileTransferAdd(item, destinationPath, itemCloned.UniqueId.ToString() + itemCloned.Extension, person, itemCloned.CreatedOn);
                                    break;
                            }
                        }
                    }
                }
                if (!isInTransaction)
                    Manager.Commit();
                cloned = true;
            }
            catch (Exception ex)
            {
                if (!isInTransaction && Manager.IsInTransaction())
                    Manager.RollBack();
                cloned = false;
                itemCloned = null;
                versionCloned = null;
                if (!String.IsNullOrWhiteSpace(destFile))
                    lm.Comol.Core.File.Delete.File(destFile);
                if (!String.IsNullOrWhiteSpace(dThumbnail))
                    lm.Comol.Core.File.Delete.File(dThumbnail);
            }
            return cloned;
        }
        private RepositoryItem CloneItem(Int32 idPerson, RepositoryItem item, RepositoryItemVersion version, RepositoryIdentifier destIdentifier, Boolean hasVersions, long idObjectOwner =0)
        {
            RepositoryItem clone = new RepositoryItem();
            clone.CreateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress);
            clone.AllowUpload = item.AllowUpload;
            clone.AutoThumbnail = item.AutoThumbnail;
            clone.CloneOf = item.Id;
            clone.CloneOfUniqueId = item.UniqueId;
            clone.ContentType = item.ContentType;
            clone.Description = item.Description;
            clone.DisplayMode = item.DisplayMode;
            clone.DisplayOrder = item.DisplayOrder;
            clone.Downloaded = 0;
            clone.Extension = item.Extension;
            clone.HasVersions = hasVersions;
            clone.IdCommunity = item.IdCommunity;
            clone.IdFolder = item.IdFolder;
            clone.IdOwner = idPerson;
            clone.IsDownloadable = item.IsDownloadable;
            clone.IsFile = item.IsFile;
            clone.IsInternal = item.IsInternal;
            clone.IsPersonal = item.IsPersonal;
            clone.IsVirtual = item.IsVirtual;
            clone.IsVisible = item.IsVisible;
            clone.Name = item.Name;           
            clone.PreviewTime = item.PreviewTime;
            clone.Size = item.Size;
            clone.Status = item.Status;
            clone.Time = item.Time;
            clone.Type = item.Type;
            clone.UniqueId = Guid.NewGuid();
            clone.UniqueIdVersion = Guid.NewGuid();
            clone.Url = item.Url;
            clone.Versions = new List<RepositoryItemVersion>();

            if (item.IsInternal && item.Module != null)
            {
                clone.Module = item.Module;
                clone.Module.IdObject = idObjectOwner;
            }
            clone.IdPlayer = item.IdPlayer;
            clone.IdVersion = 0;
            clone.VersionsSize = 0;
            clone.Tags = item.Tags;
            if (!String.IsNullOrWhiteSpace(clone.Thumbnail))
                clone.Thumbnail = "thumb_" + Guid.NewGuid().ToString() + ".jpg";
            else
                clone.AutoThumbnail = false;
            
            clone.Repository = destIdentifier;
            if (hasVersions)
                clone.Number = item.Versions.Count(v=> v.Deleted== BaseStatusDeleted.None);
            else
                clone.Number = 0;
            clone.RevisionsNumber = 0;


            switch (clone.Type)
            {
                case ItemType.Multimedia:
                case ItemType.ScormPackage:
                    clone.Availability = ItemAvailability.transfer;
                    clone.Status = ItemStatus.Active;
                    break;
                default:
                    clone.Availability = ItemAvailability.available;
                    clone.Status = ItemStatus.Active;
                    break;
            }
            return clone;
        }

    }
}
