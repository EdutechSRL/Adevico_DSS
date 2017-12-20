using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository 
    {
        #region "Settings"
            #region"Manage"
                public virtual Dictionary<ItemType, Boolean> GetDefaultAllowDownload(liteRepositorySettings settings)
                {
                    return (from i in Enum.GetValues(typeof(ItemType)).OfType<ItemType>() where i != ItemType.None && i != ItemType.RootFolder select i).ToDictionary(i => i, i => (settings.ItemTypes == null || !settings.ItemTypes.Where(x => x.Type == i && x.Deleted == DomainModel.BaseStatusDeleted.None && !x.AllowDownload).Any()));
                }

                public virtual Dictionary<ItemType, DisplayMode> GetDefaultDisplayMode(liteRepositorySettings settings)
                {
                    return (from i in Enum.GetValues(typeof(ItemType)).OfType<ItemType>() where i != ItemType.None && i != ItemType.RootFolder select i).ToDictionary(i => i, i => ((settings.ItemTypes == null || !settings.ItemTypes.Where(x => x.Type == i && x.Deleted == DomainModel.BaseStatusDeleted.None).Any()) ? DisplayMode.downloadOrPlay : settings.ItemTypes.Where(x => x.Type == i && x.Deleted == DomainModel.BaseStatusDeleted.None).Select(x => x.DefaultDisplayMode).FirstOrDefault()));
                }
            #endregion

            #region "Search"
                public liteRepositorySettings SettingsGetByRepositoryItem(long idItem)
                {
                    return SettingsGetByRepositoryIdentifier(ItemGetRepositoryIdentifier(idItem));
                }
                public liteRepositorySettings SettingsGetByRepositoryIdentifier(RepositoryIdentifier item)
                {
                    liteRepositorySettings result = null;
                    if (item != null)
                    {
                        try
                        {
                            switch (item.Type)
                            {
                                case RepositoryType.Portal:
                                    result = SettingsGetDefault(SettingsGet(s => s.Deleted == DomainModel.BaseStatusDeleted.None && s.Status == ItemStatus.Active && (s.Type == SettingsType.Istance || s.Type == SettingsType.Portal)));
                                    break;
                                default:
                                    Int32 idOrganization = Manager.GetIdOrganizationFromCommunity(item.IdCommunity);
                                    result = SettingsGetDefault(SettingsGet(s => s.Deleted == DomainModel.BaseStatusDeleted.None && s.Status == ItemStatus.Active && (s.Type == SettingsType.Istance || s.Type == SettingsType.Portal || (s.Type == SettingsType.Organization && s.IdOrganization == idOrganization) || (s.Type == SettingsType.Community && s.IdCommunity == item.IdCommunity))));
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    return result;
                }

                public liteRepositorySettings SettingsGetDefault(lm.Comol.Core.FileRepository.Domain.RepositoryType type, Int32 idCommunity = -1)
                {
                    liteRepositorySettings result = null;
                    switch (type)
                    {
                        case RepositoryType.Portal:
                            result = SettingsGetDefault(SettingsGet(s => s.Deleted == DomainModel.BaseStatusDeleted.None && s.Status== ItemStatus.Active && (s.Type == SettingsType.Istance || s.Type == SettingsType.Portal)));
                            break;
                        default:
                            if (idCommunity == -1)
                                idCommunity = UC.CurrentCommunityID;
                            Int32 idOrganization = Manager.GetIdOrganizationFromCommunity(idCommunity);
                            result = SettingsGetDefault(SettingsGet(s => s.Deleted == DomainModel.BaseStatusDeleted.None && s.Status == ItemStatus.Active && (s.Type == SettingsType.Istance || s.Type == SettingsType.Portal || (s.Type == SettingsType.Organization && s.IdOrganization == idOrganization) || (s.Type == SettingsType.Community && s.IdCommunity == idCommunity))));
                            break;
                    }
                    return result;
                }
                private liteRepositorySettings SettingsGetDefault(IEnumerable<liteRepositorySettings> settings)
                {
                    try{

                        return (settings==null) ? null : settings.OrderByDescending(s => (int)s.Type).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                private IEnumerable<liteRepositorySettings> SettingsGet(Expression<Func<liteRepositorySettings, bool>> filter)
                {
                    return Manager.GetIQ<liteRepositorySettings>().Where(filter);
                }
                private List<Int32> SettingsGetProfileTypesAvailability()
                {
                    return (from a in Manager.GetIQ<liteProfileTypeAvailability>() where a.Deleted == DomainModel.BaseStatusDeleted.None select a.IdProfileType).ToList();
                }
            #endregion
        #endregion
    }
}