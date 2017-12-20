using lm.Comol.Core.BaseModules.FileRepository.Domain;
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
        public VersionErrors AddVersionToFile(String istanceIdentifier, long idItem, dtoUploadedItem version, String unknownUser, String repositoryPath, RepositoryIdentifier identifier, Boolean allowManage, Boolean allowView, Boolean updateLinks)
        {
            VersionErrors error =  VersionErrors.unabletoadd;
            liteRepositoryItem rItem = ItemGet(idItem);
            ModuleRepository.ObjectType oType = ModuleRepository.ObjectType.File;
            dtoDisplayRepositoryItem dItem =  null;
            if (rItem != null)
            {
                ModuleRepository module = GetPermissions(rItem.Repository, UC.CurrentUserID);
                Boolean allowCreate = false;
                Int32 idCommunity = rItem.Repository.IdCommunity;
                if (rItem.IsInternal)
                    allowCreate = true;
                else
                {
                    dItem = GetItemWithPermissions(idItem, UC.CurrentUserID, rItem.Repository, unknownUser, allowManage, allowView);
                    if (dItem == null)
                        error = VersionErrors.unavailableItem;
                    else
                    {
                        Boolean allowAdd = dItem.Permissions.GetActions().Contains(ItemAction.addVersion);
                        oType = ModuleRepository.GetObjectType(dItem.Type);
                        if (!allowAdd)
                            error = VersionErrors.nopermission;
                        else
                            allowCreate = true;
                    }
                }
                if (allowCreate){
                    liteRepositorySettings settings = SettingsGetByRepositoryIdentifier(rItem.Repository);
                    ThumbnailsCreate(settings, rItem.UniqueId, version);
                    dtoCreatedItem addedVersion = FileAddVersion(settings, module, repositoryPath, istanceIdentifier, idItem, version);

                    if (addedVersion != null && addedVersion.IsAdded)
                    {
                        if (dItem != null)
                        {
                            dItem.IdVersion = addedVersion.Added.IdVersion;
                            dItem.UniqueIdVersion = addedVersion.Added.UniqueIdVersion;
                        }
                        if (updateLinks)
                        {
                            List<liteModuleLink> links = (from l in GetQuery<liteModuleLink>()
                                                          where
                                                              l.DestinationItem.ServiceCode == ModuleRepository.UniqueCode
                                                              && l.DestinationItem.ObjectLongID == idItem
                                                              && l.DestinationItem.ObjectIdVersion > 0
                                                          select l).ToList();
                            if (links.Any())
                            {
                                links.ForEach(l => l.DestinationItem.ObjectIdVersion = addedVersion.Added.IdVersion);
                                Manager.SaveOrUpdateList(links);
                            }
                        }
                        error = VersionErrors.none;
                    }
                    else
                    error = VersionErrors.unabletoadd;
                }
            }
            return error;
        }
    }
}