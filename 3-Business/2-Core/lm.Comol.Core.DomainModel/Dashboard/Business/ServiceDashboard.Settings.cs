using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using NHibernate.Linq;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.Dashboard.Business
{
    public partial class ServiceDashboard : CoreServices
    {
        #region "Settings"
            public liteDashboardSettings GetDefaultSettings(DashboardType type, Int32 idCommunity = -1)
            {
                liteDashboardSettings result = null;
                try{
                    var query = (from t in Manager.GetIQ<liteDashboardSettings>()
                                 where t.Active && t.Deleted == BaseStatusDeleted.None
                                 select t);
                    switch (type)
                    {
                        case DashboardType.Portal:
                            query = query.Where(s => s.Type == DashboardType.Portal && s.ForAll);
                            break;
                        case DashboardType.AllCommunities:
                            query = query.Where(s => s.Type == DashboardType.AllCommunities && s.ForAll);
                            break;
                        case DashboardType.Community:
                            query = query.Where(s => ((s.Type == DashboardType.Community && s.IdCommunity== idCommunity) || s.Type == DashboardType.AllCommunities )&& s.ForAll).OrderByDescending(s=> s.IdCommunity);
                            break;
                    }
                    result = query.Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch(Exception ex){


                }
                return result;
            }
            public String GetSettingsName(long idDashboard)
            {
                String name = "";
                try
                {
                    name = (from t in Manager.GetIQ<liteDashboardSettings>()
                            where t.Id == idDashboard select t.Name).Skip(0).Take(1).ToList().FirstOrDefault();
                 }
                catch (Exception ex)
                {


                }
                return name;
            }
        #endregion

        #region "Manage Settings"

            public List<dtoDashboardSettings> DashboardGetSettings(ModuleDashboard permissions, DashboardType type, String unknownUser, Dictionary<lm.Comol.Core.Dashboard.Domain.AvailableStatus, String> status, Int32 idCommunity = -1, Boolean fromRecycleBin = false, OrderSettingsBy orderBy = OrderSettingsBy.Default, Boolean ascending = true )
            {
                List<dtoDashboardSettings> items = null;
                try
                { 
                    BaseStatusDeleted deleted = (fromRecycleBin) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                    var query = (from d in Manager.GetIQ<liteDashboardSettings>() where d.Deleted == deleted && d.Type == type select d);
                    switch (type)
                    {
                        case DashboardType.Community:
                            query = query.Where(s => s.IdCommunity == idCommunity);
                            break;
                    }
                    items = query.AsEnumerable().Select(s => new dtoDashboardSettings(s, permissions, UC.CurrentUserID, status)).ToList();

                    AnalizeSettings(items, unknownUser);
                    var sQuery = (from s in items select s);
                    switch (orderBy)
                    {
                        case OrderSettingsBy.Name:
                            if (ascending)
                                sQuery = sQuery.OrderBy(t => t.Name);
                            else
                                sQuery = sQuery.OrderByDescending(t => t.Name);
                            break;
                        case OrderSettingsBy.ModifiedBy:
                            if (ascending)
                                sQuery = sQuery.OrderBy(t => t.ModifiedBy);
                            else
                                 sQuery = sQuery.OrderByDescending(t => t.ModifiedBy);
                            break;
                        case OrderSettingsBy.ModifiedOn:
                            if (ascending)
                                sQuery = sQuery.OrderBy(t => t.ModifiedOn);
                            else
                                 sQuery = sQuery.OrderByDescending(t => t.ModifiedOn);
                            break;
                        case OrderSettingsBy.Status:
                            if (ascending)
                                sQuery = sQuery.OrderBy(t => t.TranslatedStatus);
                            else
                                sQuery = sQuery.OrderByDescending(t => t.TranslatedStatus);
                            break;
                        default:
                            sQuery = sQuery.OrderByDescending(t => t.ForAll).ThenBy(t => t.Name).ThenByDescending(t => t.Active);
                            if (!ascending)
                                sQuery = sQuery.Reverse();
                            break;
                    }
                    return sQuery.ToList();
                }
                catch (Exception ex)
                {
                    items = null;
                }
                return items;
            }
            private void AnalizeSettings(List<dtoDashboardSettings> items,String unknownUser)
            {
                Dictionary<Int32, String> cNames = GetCommunitiesName(items.Where(a => a.IdCommunity > 0).Select(i => i.IdCommunity).Distinct().ToList());
                Dictionary<Int32, String> rNames = GetRolesName(items.Where(s => s.Assignments.Any() && s.Assignments.Where(a=> a.Type== DashboardAssignmentType.RoleType).Any()).SelectMany(i=> i.Assignments.Where(a=> a.Type== DashboardAssignmentType.RoleType).Select(a=> a.IdRole)).Distinct().ToList());
                Dictionary<Int32, String> tNames = GetProfileTypeName(items.Where(s => s.Assignments.Any() && s.Assignments.Where(a => a.Type == DashboardAssignmentType.ProfileType).Any()).SelectMany(i => i.Assignments.Where(a => a.Type == DashboardAssignmentType.ProfileType).Select(a => a.IdProfileType)).Distinct().ToList());
                Dictionary<Int32, String> pNames = GetPersonsName(items.Where(s => s.Assignments.Any() && s.Assignments.Where(a => a.Type == DashboardAssignmentType.User).Any()).SelectMany(i => i.Assignments.Where(a => a.Type == DashboardAssignmentType.User).Select(a => a.IdPerson)).Distinct().ToList());
                if (cNames != null){
                    foreach (dtoDashboardSettings item in items.Where(i=>i.IdCommunity>0))
                    {
                        item.CommunityName = (cNames.ContainsKey(item.IdCommunity) ? cNames[item.IdCommunity] : "");
                    }
                }

                if (rNames != null || tNames != null || pNames != null)
                {
                    var query = items.Where(i => 
                                        (rNames != null && i.Assignments.Any() && i.Assignments.Where(a => a.Type == DashboardAssignmentType.RoleType).Any())
                                        ||
                                        (tNames != null && i.Assignments.Any() && i.Assignments.Where(a => a.Type == DashboardAssignmentType.ProfileType).Any())
                                        ||
                                        (pNames != null && i.Assignments.Any() && i.Assignments.Where(a => a.Type == DashboardAssignmentType.User).Any())
                                        ).SelectMany(i=> i.Assignments);
                    foreach (dtoDashboardAssignment item in query)
                    {
                        switch(item.Type){
                            case DashboardAssignmentType.User:
                                item.DisplayName = (pNames!=null && pNames.ContainsKey(item.IdPerson) ? pNames[item.IdPerson] : "");
                                break;
                            case DashboardAssignmentType.RoleType:
                                item.DisplayName = (rNames != null && cNames.ContainsKey(item.IdRole) ? rNames[item.IdRole] : "");
                                break;
                            case DashboardAssignmentType.ProfileType:
                                item.DisplayName = (tNames != null && tNames.ContainsKey(item.IdProfileType) ? tNames[item.IdProfileType] : "");
                                break;
                        }
                    }
                }
                pNames = GetPersonsName(items.Select(i => i.IdModifiedBy).Distinct().ToList());

                foreach (dtoDashboardSettings item in items.Where(i => pNames.ContainsKey(i.IdModifiedBy) && !String.IsNullOrEmpty(pNames[i.IdModifiedBy])))
                {
                    item.ModifiedBy = pNames[item.IdModifiedBy];
                }
                foreach (var item in items.Where(i => String.IsNullOrEmpty(i.ModifiedBy)))
                {
                    item.ModifiedBy = unknownUser + item.IdModifiedBy.ToString();
                }

            }
            private Dictionary<Int32, String> GetCommunitiesName(List<Int32> idCommunities)
            {
                if (idCommunities.Count > 0)
                {
                    if (idCommunities.Count <= maxItemsForQuery)
                        return (from c in Manager.GetIQ<liteCommunity>() where idCommunities.Contains(c.Id) select c).ToDictionary(c => c.Id, c => c.Name);
                    else
                        return (from c in Manager.GetIQ<liteCommunity>() select c).ToList().Where(c => idCommunities.Contains(c.Id)).ToDictionary(c => c.Id, c => c.Name);
                }
                return null;
            }
            private Dictionary<Int32, String> GetRolesName(List<Int32> idRoles)
            {
                if (idRoles.Count>0)
                    return (from c in Manager.GetTranslatedItem<dtoTranslatedRoleType>(UC.Language.Id) where idRoles.Contains(c.Id) select c).ToDictionary(c => c.Id, c => c.Name);
                return null;
            }
            private Dictionary<Int32, String> GetProfileTypeName(List<Int32> idTypes)
            {
                if (idTypes.Count > 0)
                    return (from c in Manager.GetTranslatedItem<dtoTranslatedProfileType>(UC.Language.Id) where idTypes.Contains(c.Id) select c).ToDictionary(c => c.Id, c => c.Name);
                return null;
            }
            private Dictionary<Int32, String> GetPersonsName(List<Int32> idPersons)
            {
                if (idPersons.Count > 0)
                {
                    if (idPersons.Count <= maxItemsForQuery)
                        return (from c in Manager.GetIQ<litePerson>() where idPersons.Contains(c.Id) select c).ToDictionary(c => c.Id, c => c.SurnameAndName);
                    else
                        return (from c in Manager.GetIQ<litePerson>() select c).ToList().Where(c => idPersons.Contains(c.Id)).ToDictionary(c => c.Id, c => c.SurnameAndName);
                }
                return null;
            }


            public DashboardSettings DashboardSettingsSetStatus(long idDashboard, lm.Comol.Core.Dashboard.Domain.AvailableStatus status)
            {
                return DashboardSettingsSetStatus(Manager.Get<DashboardSettings>(idDashboard), status);
            }
            public DashboardSettings DashboardSettingsSetStatus(DashboardSettings settings, lm.Comol.Core.Dashboard.Domain.AvailableStatus status)
            {
                DashboardSettings item = null;
                try
                {
                    DashboardErrorType mType = SettingsEditable(settings, status, settings.Deleted);
                    if (mType != DashboardErrorType.None)
                        throw new DashboardException(mType);

                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (settings != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        Manager.BeginTransaction();
                        settings.Status = status;
                        settings.Active = (status== AvailableStatus.Available);
                        settings.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        UpdateOtherActiveSettings(settings, person);
                        Manager.Commit();
                        item = settings;
                        Int32 idCommunity = -2;
                        switch(settings.Type){
                            case DashboardType.Portal:
                                idCommunity=0;
                                break;
                            case DashboardType.AllCommunities:
                                idCommunity = -1;
                                break;
                            case DashboardType.Community:
                                idCommunity = (settings.Community==null) ? idCommunity : settings.Community.Id;
                                break;
                        }
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.DashboardTiles(settings.Id));
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.CurrentDashboardSettings(idCommunity));
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.Dashboard(idCommunity));
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllDashboard);
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard);
                    }
                }
                catch (DashboardException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return item;
            }
            public DashboardSettings DashboardSettingsVirtualDelete(long idDashboard, Boolean delete)
            {
                return DashboardSettingsVirtualDelete(Manager.Get<DashboardSettings>(idDashboard), (delete) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None);
            }
            public DashboardSettings DashboardSettingsVirtualDelete(long idDashboard, BaseStatusDeleted deleted)
            {
                return DashboardSettingsVirtualDelete(Manager.Get<DashboardSettings>(idDashboard), deleted);
            }
            public DashboardSettings DashboardSettingsVirtualDelete(DashboardSettings settings, BaseStatusDeleted deleted)
            {
                Boolean isInTransaction = Manager.IsInTransaction();
                DashboardSettings result = null;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (settings != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        DashboardErrorType mType = SettingsEditable(settings, settings.Status, deleted);
                        switch (mType)
                        {
                            case DashboardErrorType.NoAssignmentsForUndelete:
                            case DashboardErrorType.None:
                                break;
                            default:
                                throw new DashboardException(mType);
                        }
                        if (!isInTransaction)
                            Manager.BeginTransaction();
                        if (settings.Deleted != deleted && deleted == BaseStatusDeleted.None)
                        {
                            settings.Active = false;
                            if (settings.Status == AvailableStatus.Available)
                                settings.Status = AvailableStatus.Unavailable;
                        }
                        settings.Deleted = deleted;
                        settings.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        if (settings.Active && settings.ForAll)
                            UpdateOtherActiveSettings(settings, person);
                        Manager.SaveOrUpdate(settings);

                        foreach (DashboardAssignment assignment in settings.Assignments)
                        {
                            assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            assignment.Deleted = (deleted == BaseStatusDeleted.Manual) ? (assignment.Deleted | BaseStatusDeleted.Cascade) : ((BaseStatusDeleted)((int)assignment.Deleted - (int)BaseStatusDeleted.Cascade));
                        }
                        foreach (PageSettings page in settings.Pages)
                        {
                            page.Deleted = (deleted == BaseStatusDeleted.Manual) ? (page.Deleted | BaseStatusDeleted.Cascade) : ((BaseStatusDeleted)((int)page.Deleted - (int)BaseStatusDeleted.Cascade));
                        }

                        if (!isInTransaction)
                            Manager.Commit();
                        result = settings;

                        if (deleted != BaseStatusDeleted.None)
                        {
                            Int32 idCommunity = -2;
                            switch (settings.Type)
                            {
                                case DashboardType.Portal:
                                    idCommunity = 0;
                                    break;
                                case DashboardType.AllCommunities:
                                    idCommunity = -1;
                                    break;
                                case DashboardType.Community:
                                    idCommunity = (settings.Community == null) ? idCommunity : settings.Community.Id;
                                    break;
                            }
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.DashboardTiles(settings.Id));
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.CurrentDashboardSettings(idCommunity));
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllDashboard);
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard);
                        }
                        if (mType == DashboardErrorType.NoAssignmentsForUndelete)
                            throw new DashboardException(mType);
                    }
                }
                catch (DashboardException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    result = null;
                    if (!isInTransaction)
                        Manager.RollBack();
                    throw new DashboardException(DashboardErrorType.GenericError, ex);
                }
                return result;
            }

            private void UpdateOtherActiveSettings(DashboardSettings settings, litePerson person)
            {
                if (settings.Status == AvailableStatus.Available && settings.ForAll)
                {
                    List<DashboardSettings> items = (from t in Manager.GetIQ<DashboardSettings>()
                                                     where t.Deleted == BaseStatusDeleted.None && t.Type == settings.Type && t.Id != settings.Id
                                                            && t.Active && t.ForAll
                                                        select t).ToList();
                    foreach (DashboardSettings i in items)
                    {
                        i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        i.Active = false;
                        i.Status = AvailableStatus.Unavailable;
                    }
                }
            }
            private DashboardErrorType SettingsEditable(DashboardSettings settings, AvailableStatus status, BaseStatusDeleted deleted)
            {
                DashboardErrorType result = DashboardErrorType.None;
                if (settings.ForAll && deleted != BaseStatusDeleted.None && settings.Active)
                    result = DashboardErrorType.DefaultSettingsUndeletable;
                else if (settings.ForAll)
                    result = ((settings.Status == AvailableStatus.Available && status == AvailableStatus.Unavailable) ? DashboardErrorType.DefaultSettingsUnavailable : DashboardErrorType.None);
                else if (deleted == BaseStatusDeleted.None && status == AvailableStatus.Available)
                {
                    if (settings.Assignments == null || (settings.Deleted == BaseStatusDeleted.None && !settings.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Any() || (settings.Deleted == BaseStatusDeleted.Manual && !settings.Assignments.Where(a =>  a.Deleted == BaseStatusDeleted.Cascade).Any())))
                        result = (settings.Deleted != BaseStatusDeleted.None) ?  DashboardErrorType.NoAssignmentsForUndelete : DashboardErrorType.NoAssignmentsForEnable;
                    else if (settings.Type != DashboardType.Community)
                    {
                        List<liteDashboardSettings> items = (from t in Manager.GetIQ<liteDashboardSettings>()
                                                             where t.Deleted == BaseStatusDeleted.None && t.Type == settings.Type && t.Id != settings.Id
                                                                 && t.Active && t.ForAll == settings.ForAll
                                                             select t).ToList();

                        var queryUsers = GetAssignmentsQuery(items, DashboardAssignmentType.User);
                        switch (settings.Type)
                        {
                            case DashboardType.Community:
                                var queryRoles = GetAssignmentsQuery(items, DashboardAssignmentType.RoleType);
                                if (settings.GetAssignments(DashboardAssignmentType.RoleType).Where(a => queryRoles.Where(r => r.IdRole == a.IdRole).Any()).Any())
                                    result = DashboardErrorType.MultipleAssignmentForRole;
                                else if (settings.GetAssignments(DashboardAssignmentType.User).Where(a => queryUsers.Where(r => r.IdPerson == a.IdPerson).Any()).Any())
                                    result = DashboardErrorType.MultipleAssignmentForPerson;
                                break;
                            case DashboardType.Portal:
                                var queryType = GetAssignmentsQuery(items, DashboardAssignmentType.ProfileType);
                                if (settings.GetAssignments(DashboardAssignmentType.ProfileType).Where(a => queryType.Where(r => r.IdProfileType == a.IdProfileType).Any()).Any())
                                    result = DashboardErrorType.MultipleAssignmentForRole;
                                else if (settings.GetAssignments(DashboardAssignmentType.User).Where(a => queryUsers.Where(r => r.IdPerson == a.IdPerson).Any()).Any())
                                    result = DashboardErrorType.MultipleAssignmentForPerson;
                                break;
                        }
                    }

                }
                return result;
            }
            private DashboardErrorType SettingsEditable(dtoBaseDashboardSettings dto,DashboardSettings settings)
            {
                var querySettings = (from t in Manager.GetIQ<DashboardSettings>()
                                                         where t.Deleted == BaseStatusDeleted.None && t.Type == settings.Type && t.Id != settings.Id
                                                             && t.Active && t.ForAll == dto.ForAll
                                                         select t);


                DashboardErrorType result = DashboardErrorType.None;
                if (dto.ForAll && settings.Deleted != BaseStatusDeleted.None && settings.Active)
                    result = DashboardErrorType.DefaultSettingsUndeletable;
                else if (dto.ForAll){
                    if (!settings.Pages.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type != DashboardViewType.Search && p.Type != DashboardViewType.Subscribe).Any())
                        result = DashboardErrorType.NotActivableSettings;
                    else
                    {
                        if (dto.IdCommunity>0)
                            querySettings = querySettings.Where(s=> s.Community != null && s.Community.Id== dto.IdCommunity);
                        List<DashboardSettings> items = querySettings.ToList();
                        if (items.Any())
                            result = DashboardErrorType.AlreadyActiveSettings;
                        else
                            result = DashboardErrorType.None;
                    }
                }
                else if (settings.Deleted == BaseStatusDeleted.None && settings.Status == AvailableStatus.Available)
                {
                    if (!dto.Assignments.Any())
                        result = DashboardErrorType.NoAssignmentsForEnable;
                    else if (!settings.Pages.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type != DashboardViewType.Search && p.Type != DashboardViewType.Subscribe).Any())
                        result = DashboardErrorType.NotActivableSettings;
                    else if (settings.Type != DashboardType.Community)
                    {
                        List<DashboardSettings> items = (from t in Manager.GetIQ<DashboardSettings>()
                                                             where t.Deleted == BaseStatusDeleted.None && t.Type == settings.Type && t.Id != settings.Id
                                                                 && t.Active && t.ForAll == dto.ForAll
                                                             select t).ToList();
                      
                        var queryUsers = GetAssignmentsQuery(items, DashboardAssignmentType.User);
                        switch (settings.Type)
                        {
                            case DashboardType.Community:
                                var queryRoles = GetAssignmentsQuery(items, DashboardAssignmentType.RoleType);
                                if (dto.GetAssignments(DashboardAssignmentType.RoleType).Where(a => queryRoles.Where(r => r.IdRole == a.IdRole).Any()).Any())
                                    result = DashboardErrorType.MultipleAssignmentForRole;
                                else if (dto.GetAssignments(DashboardAssignmentType.User).Where(a => queryUsers.Where(r => r.IdPerson == a.IdPerson).Any()).Any())
                                    result = DashboardErrorType.MultipleAssignmentForPerson;
                                break;
                            case DashboardType.Portal:
                                var queryType = GetAssignmentsQuery(items, DashboardAssignmentType.ProfileType);
                                if (dto.GetAssignments(DashboardAssignmentType.ProfileType).Where(a => queryType.Where(r => r.IdProfileType == a.IdProfileType).Any()).Any())
                                    result = DashboardErrorType.MultipleAssignmentForProfileType;
                                else if (dto.GetAssignments(DashboardAssignmentType.User).Where(a => queryUsers.Where(r => r.IdPerson == a.IdPerson).Any()).Any())
                                    result = DashboardErrorType.MultipleAssignmentForPerson;
                                break;
                        }
                    }

                }
                return result;
            }
            private IEnumerable<liteDashboardAssignment> GetAssignmentsQuery(List<liteDashboardSettings> items, DashboardAssignmentType type)
            {
                return items.SelectMany(i => i.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == type)); 
            }
            private IEnumerable<DashboardAssignment> GetAssignmentsQuery(List<DashboardSettings> items, DashboardAssignmentType type)
            {
                return items.SelectMany(i => i.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == type));
            }
            public DashboardSettings DashboardSettingsClone(long idDashboard, String cloneOf)
            {
                DashboardSettings clone = null;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    DashboardSettings settings = Manager.Get<DashboardSettings>(idDashboard);
                    if (settings != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        Manager.BeginTransaction();
                        clone = settings.Copy(person, UC.IpAddress, UC.ProxyIpAddress);
                        clone.Active = false;
                        clone.Deleted =  BaseStatusDeleted.None;
                        clone.Name = cloneOf + settings.Name;
                        Manager.SaveOrUpdate(clone);
                        Manager.Commit();
                        List<DashboardTileAssignment> assignments = (from a in Manager.GetIQ<DashboardTileAssignment>() where a.Dashboard == settings select a).ToList().Select(a=> a.Copy(clone,person, UC.IpAddress, UC.ProxyIpAddress)).ToList();
                        Manager.BeginTransaction();
                        Manager.SaveOrUpdateList(assignments);
                        Manager.Commit();
                    }
                }
                catch (DashboardException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    clone = null;
                    throw new DashboardException(DashboardErrorType.GenericError, ex);
                }
                return clone;
            }

            #region "Save Base Settings"
                public DashboardSettings SaveBaseSettings(dtoBaseDashboardSettings dto)
                {
                    DashboardSettings settings = null;
                    litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                    if (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser)
                    {
                        Boolean toUpdate = false;
                        Boolean toAdd = (dto.Id == 0);
                        if (toAdd)
                        {
                            settings = new DashboardSettings();
                            settings.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            settings.Status = AvailableStatus.Draft;
                            settings.Type = dto.Type;
                            settings.Community = (dto.Type == DashboardType.Community && dto.IdCommunity > 0) ? Manager.Get<liteCommunity>(dto.IdCommunity) : null;
                            settings.FullWidth = true;

                            settings.Container = new ContainerSettings(dto.Type);
                        }
                        else
                        {
                            settings = Manager.Get<DashboardSettings>(dto.Id);
                            if (settings == null)
                                return null;
                        }
                        DashboardErrorType error = DashboardErrorType.None;
                        if (!toAdd)
                            error = SettingsEditable(dto,settings);
                        if (error == DashboardErrorType.None)
                        {
                            try
                            {
                                Manager.BeginTransaction();
                                settings.Name = dto.Name;
                                settings.Description = dto.Description;
                                if (!toAdd)
                                    settings.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                toUpdate =  (settings.ForAll != dto.ForAll);
                                settings.ForAll = dto.ForAll;
                                Manager.SaveOrUpdate(settings);
                                if (dto.ForAll)
                                {
                                    foreach (var item in settings.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None))
                                    {
                                        item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                        item.Deleted = BaseStatusDeleted.Manual;
                                    }
                                }
                                else if (dto.Assignments.Any())
                                    SaveAssignments(settings, dto.Assignments,p);
                                if (toAdd)
                                {
                                    List<DashboardTileAssignment> assignments = (from a in Manager.GetIQ<DashboardTileAssignment>()
                                                                                 where a.Deleted == BaseStatusDeleted.None && a.Dashboard.Active && a.Dashboard.ForAll
                                                                                 select a).ToList();
                                    Dictionary<TileType, long> orderByItems = new Dictionary<TileType, long>();
                                    orderByItems[TileType.Module] = 1;
                                    orderByItems[TileType.CommunityType] = 1;
                                    orderByItems[TileType.CombinedTags] = 1;

                                    foreach (DashboardTileAssignment assignment in assignments.Where(a => a.Tile != null).OrderBy(a => a.Tile.Type).ThenBy(a => a.DisplayOrder)) { 
                                        DashboardTileAssignment nAssignment = assignment.Copy(settings, p, UC.IpAddress, UC.ProxyIpAddress);
                                        switch (assignment.Tile.Type)
                                        {
                                            case TileType.CommunityType:
                                                nAssignment.DisplayOrder = orderByItems[TileType.CommunityType];
                                                orderByItems[TileType.CommunityType]++;
                                                break;
                                            case TileType.Module:
                                            case TileType.UserDefined:
                                                nAssignment.DisplayOrder = orderByItems[TileType.Module];
                                                orderByItems[TileType.Module]++;
                                                break;
                                            case TileType.CombinedTags:
                                            case TileType.CommunityTag:
                                            case TileType.DashboardUserDefined:
                                                nAssignment.DisplayOrder = orderByItems[TileType.CombinedTags];
                                                orderByItems[TileType.CombinedTags]++;
                                                break;
                                        }
                                        
                                        Manager.SaveOrUpdate(nAssignment);
                                    }
                                }
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                throw new DashboardException(DashboardErrorType.GenericError);
                            }
                            if (toUpdate || toAdd)
                            {
                                CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.DashboardTiles(settings.Id));
                                CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.Dashboard(dto.IdCommunity));
                                CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.CurrentDashboardSettings(dto.IdCommunity));
                                CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard);                            
                            }
                        }
                        else
                            throw new DashboardException(error);
                    }
                    return settings;
                }
                private void SaveAssignments(DashboardSettings settings, List<dtoDashboardAssignment> items, litePerson  p)
                {
                    switch (settings.Type)
                    {
                        case DashboardType.Community:
                            //SaveUserAssignments(settings, items.Where(i => i.Type == DashboardAssignmentType.User).ToList(), p);
                            SaveRoleAssignments(settings, items.Where(i => i.Type == DashboardAssignmentType.RoleType).ToList(), p);
                            break;
                        case DashboardType.Portal:
                            //SaveUserAssignments(settings, items.Where(i => i.Type == DashboardAssignmentType.User).ToList(), p);
                            SaveProfileTypeAssignments(settings, items.Where(i => i.Type == DashboardAssignmentType.ProfileType).ToList(), p);
                            break;
                    }
                }
                private void SaveUserAssignments(DashboardSettings settings, List<dtoDashboardAssignment> items, litePerson p)
                {
                    List<Int32> idPersons = items.Select(a => a.IdPerson).Distinct().ToList();
                    List<DashboardAssignment> assignments = settings.Assignments.Where(a => a.Type == DashboardAssignmentType.User).ToList();
                    if (idPersons.Any() || assignments.Any()){
                        foreach (var item in assignments.Where(a => a.Deleted == BaseStatusDeleted.None && !idPersons.Contains(a.IdPerson)))
                        {
                            item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            item.Deleted = BaseStatusDeleted.Manual;
                        }
                        foreach (var item in assignments.Where(a => a.Deleted != BaseStatusDeleted.None && idPersons.Contains(a.IdPerson)))
                        {
                            item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            item.Deleted = BaseStatusDeleted.None;
                        }
                        foreach (Int32 idPerson in idPersons.Where(i=> !assignments.Where(a=> a.IdPerson==i).Any()))
                        {
                            DashboardAssignment assignment = new DashboardAssignment();
                            assignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            assignment.IdPerson= idPerson;
                            assignment.Type= DashboardAssignmentType.User;
                            assignment.Dashboard = settings;
                            Manager.SaveOrUpdate(assignment);
                            settings.Assignments.Add(assignment);
                        }
                    }
                }
                private void SaveRoleAssignments(DashboardSettings settings, List<dtoDashboardAssignment> items, litePerson p)
                {
                    List<Int32> idRoles = items.Select(a => a.IdRole).Distinct().ToList();
                    List<DashboardAssignment> assignments = settings.Assignments.Where(a => a.Type == DashboardAssignmentType.RoleType).ToList();
                    if (idRoles.Any() || assignments.Any()){
                        foreach (var item in assignments.Where(a => a.Deleted == BaseStatusDeleted.None && !idRoles.Contains(a.IdRole)))
                        {
                            item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            item.Deleted = BaseStatusDeleted.Manual;
                        }
                        foreach (var item in assignments.Where(a => a.Deleted != BaseStatusDeleted.None && idRoles.Contains(a.IdRole)))
                        {
                            item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            item.Deleted = BaseStatusDeleted.None;
                        }
                        foreach (Int32 idRole in idRoles.Where(i=> !assignments.Where(a=> a.IdRole==i).Any()))
                        {
                            DashboardAssignment assignment = new DashboardAssignment();
                            assignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            assignment.IdRole= idRole;
                            assignment.Type= DashboardAssignmentType.RoleType;
                            assignment.Dashboard = settings;
                            Manager.SaveOrUpdate(assignment);
                            settings.Assignments.Add(assignment);
                        }
                    }
                }
                private void SaveProfileTypeAssignments(DashboardSettings settings, List<dtoDashboardAssignment> items, litePerson p)
                {
                    List<Int32> idTypes = items.Select(a => a.IdProfileType).Distinct().ToList();
                    List<DashboardAssignment> assignments = settings.Assignments.Where(a => a.Type == DashboardAssignmentType.ProfileType).ToList();
                    if (idTypes.Any() || assignments.Any()){
                        foreach (var item in assignments.Where(a => a.Deleted == BaseStatusDeleted.None && !idTypes.Contains(a.IdProfileType)))
                        {
                            item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            item.Deleted = BaseStatusDeleted.Manual;
                        }
                        foreach (var item in assignments.Where(a => a.Deleted != BaseStatusDeleted.None && idTypes.Contains(a.IdProfileType)))
                        {
                            item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            item.Deleted = BaseStatusDeleted.None;
                        }
                        if (assignments.Any())
                            Manager.SaveOrUpdateList(assignments);

                        List<DashboardAssignment> toAdd = new List<DashboardAssignment>();
                        foreach (Int32 idType in idTypes.Where(i=> !assignments.Where(a=> a.IdProfileType==i).Any()))
                        {
                            DashboardAssignment assignment = new DashboardAssignment();
                            assignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            assignment.IdProfileType = idType;
                            assignment.Type= DashboardAssignmentType.ProfileType;
                            assignment.Dashboard = settings;
                            toAdd.Add(assignment);
                        }
                        if (toAdd.Any())
                        {
                            Manager.SaveOrUpdateList(toAdd);
                            toAdd.ForEach(a=> settings.Assignments.Add(a));
                        }
                    }
                }
            #endregion

            #region "Save View Settings"
                public DashboardSettings SaveViewSettings(DashboardSettings settings, dtoViewSettings dto)
                {
                    DashboardSettings result = null;
                    litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                    if (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser && settings!=null )
                    {
                        Boolean toUpdate = false;
                        try{
                            Manager.BeginTransaction();
                            settings.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                            if (settings.Container==null)
                                settings.Container = new ContainerSettings( settings.Type);

                            settings.Container.Default.GroupBy =  dto.Container.Default.GroupBy;
                            settings.Container.Default.DefaultNoticeboard = dto.Container.Default.DefaultNoticeboard;
                            settings.Container.Default.OrderBy = dto.Container.Default.OrderBy;
                            settings.Container.Default.Search = dto.Container.Default.Search;
                            settings.Container.Default.AfterUserLogon = dto.Container.Default.AfterUserLogon;
                            settings.Container.Default.View = dto.Container.Default.View;
                            foreach (DashboardViewType vType in Enum.GetValues(typeof(DashboardViewType)).OfType<DashboardViewType>()){
                                if (settings.Container.AvailableViews.Contains(vType) && !dto.Container.AvailableViews.Contains(vType))
                                    settings.Container.AvailableViews = settings.Container.AvailableViews.Where(t=> t!= vType).ToList();
                                else if (!settings.Container.AvailableViews.Contains(vType) && dto.Container.AvailableViews.Contains(vType))
                                    settings.Container.AvailableViews.Add(vType);
                            }
                            List<GroupItemsBy> gToRemove =  settings.Container.AvailableGroupBy.Where(g=> !dto.Container.AvailableGroupBy.Contains(g)).ToList();
                            List<OrderItemsBy> oToRemove =  settings.Container.AvailableOrderBy.Where(g=> !dto.Container.AvailableOrderBy.Contains(g)).ToList();
                            foreach (GroupItemsBy gItem in gToRemove){
                                settings.Container.AvailableGroupBy.Remove(gItem);
                            }
                            foreach (GroupItemsBy gItem in dto.Container.AvailableGroupBy.Where(g=> !settings.Container.AvailableGroupBy.Contains(g))){
                                settings.Container.AvailableGroupBy.Add(gItem);
                            }
                            foreach (OrderItemsBy oItem in oToRemove)
                            {
                                settings.Container.AvailableOrderBy.Remove(oItem);
                            }
                            foreach (OrderItemsBy oItem in dto.Container.AvailableOrderBy.Where(g=> !settings.Container.AvailableOrderBy.Contains(g))){
                                settings.Container.AvailableOrderBy.Add(oItem);
                            }
                            settings.FullWidth = dto.FullWidth;
                          
                            foreach (PageSettings page in settings.Pages){
                                if (page.Deleted== BaseStatusDeleted.None && ! dto.Container.AvailableViews.Contains(page.Type))
                                    page.Deleted = BaseStatusDeleted.Manual;
                                else if (page.Deleted!= BaseStatusDeleted.None &&  dto.Container.AvailableViews.Contains(page.Type))
                                    page.Deleted = BaseStatusDeleted.None;
                            }
                            Boolean addPage = false;
                            foreach (DashboardViewType vType in dto.Container.AvailableViews)
                            {
                                PageSettings page = settings.Pages.Where(t => t.Type == vType).FirstOrDefault();
                                if (page == null)
                                {
                                    page = new PageSettings();
                                    page.Type = vType;
                                    addPage = true;
                                }
                                UpdatePage(settings, page, dto.Pages.Where(pt=> pt.Type== vType).FirstOrDefault());
                                Manager.SaveOrUpdate(page);
                                if (addPage)
                                {
                                    settings.Pages.Add(page);
                                    addPage = false;
                                }
                            }

                            foreach (PageSettings page in settings.Pages.Where(pt => pt.Deleted != BaseStatusDeleted.None && dto.Pages.Where(pp => pp.Type == pt.Type).Any()))
                            {
                                UpdatePage(settings, page, dto.Pages.Where(pt => pt.Type == page.Type).FirstOrDefault());
                                Manager.SaveOrUpdate(page);
                            }
                            if (settings.Container.Default.DefaultNoticeboard == DisplayNoticeboard.DefinedOnAllPages)
                            {
                                settings.Container.Default.ListNoticeboard = GetDefaultNoticeboardSettings(settings.Pages, DashboardViewType.List);
                                settings.Container.Default.CombinedNoticeboard = GetDefaultNoticeboardSettings(settings.Pages, DashboardViewType.Combined);
                                settings.Container.Default.TileNoticeboard = GetDefaultNoticeboardSettings(settings.Pages, DashboardViewType.Tile);
                            }
                            else
                            {
                                settings.Container.Default.ListNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                                settings.Container.Default.CombinedNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                                settings.Container.Default.TileNoticeboard = DisplayNoticeboard.InheritsFromDefault;
                            }

                            Manager.Commit();
                            result = settings;
                            if (settings.Active){
                                Int32 idCommunity = 0;
                                switch (settings.Type)
                                {
                                    case DashboardType.AllCommunities:
                                        idCommunity = -1;
                                        break;
                                    case DashboardType.Community:
                                        idCommunity = (settings.Community != null) ? settings.Community.Id : -2;
                                        break;
                                }
                                CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.DashboardTiles(settings.Id));
                                CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.Dashboard(idCommunity));
                                CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.CurrentDashboardSettings(idCommunity));
                                CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard); 
                            }
                        }
                        catch(Exception ex){
                            Manager.RollBack();
                            return null;
                        }
                    }
                    return result;
                }
                private DisplayNoticeboard GetDefaultNoticeboardSettings(IList<PageSettings> pages, DashboardViewType type)
                {
                    return
                        (pages.Any() && pages.Where(pg => pg.Type == type && pg.Noticeboard != DisplayNoticeboard.InheritsFromDefault).Any() 
                        ? pages.Where(pg=> pg.Type== type && pg.Noticeboard != DisplayNoticeboard.InheritsFromDefault).Select(pg=> pg.Noticeboard).FirstOrDefault() 
                        : DisplayNoticeboard.OnRight);
                }
                private void UpdatePage(DashboardSettings dashboard,PageSettings page, dtoPageSettings dto ){
                    if (dto !=null){
                        switch(page.Type){
                            case DashboardViewType.List:
                                page.AutoUpdateLayout = dto.AutoUpdateLayout;
                                if (page.Dashboard == null)
                                    page.Dashboard = dashboard;
                                page.DisplayAsTile = false;
                                page.ExpandOrganizationList = dto.ExpandOrganizationList;
                                page.MaxItems = dto.MaxItems;
                                page.MaxMoreItems = dto.MaxMoreItems;
                                page.MiniTileDisplayItems = 0;
                                page.More = dto.More;
                                page.Noticeboard = dto.Noticeboard;
                                page.PlainLayout = dto.PlainLayout;
                                
                                if (dto.Range.IsValid(dto.MaxItems))
                                    page.Range = dto.Range;
                                else
                                    page.Range = new RangeSettings() { DisplayItems = dto.MaxItems, HigherLimit = dto.MaxItems, LowerLimit = 0 };
                                break;
                            case DashboardViewType.Combined:
                                page.TileLayout = dto.TileLayout;
                                page.More = dto.More;
                                page.AutoUpdateLayout = dto.AutoUpdateLayout;
                                page.Noticeboard = dto.Noticeboard;
                                page.MaxItems = dto.MaxItems;
                                page.MaxMoreItems = dto.MaxMoreItems;
                                if (page.Dashboard==null)
                                    page.Dashboard = dashboard;
                                page.DisplayAsTile = false;
                                page.TileRedirectOn = DashboardViewType.Combined;
                                page.PlainLayout = dto.PlainLayout;
                                page.MiniTileDisplayItems = dto.MiniTileDisplayItems;
                                break;
                        case DashboardViewType.Tile:
                            if (page.Dashboard == null)
                                page.Dashboard = dashboard;
                            page.AutoUpdateLayout = dto.AutoUpdateLayout;
                            page.DisplayAsTile = false;
                            page.MaxItems = dto.MaxItems;
                            //page.MaxMoreItems = dto.MaxMoreItems;
                            //page.MiniTileDisplayItems = 0;
                            page.More = dto.More;
                            page.Noticeboard = dto.Noticeboard;
                            page.TileLayout = dto.TileLayout;
                            page.TileRedirectOn = dto.TileRedirectOn;
                            page.PlainLayout = PlainLayout.full;
                            break;
                        case DashboardViewType.Search:
                            page.AutoUpdateLayout = dto.AutoUpdateLayout;
                            if (page.Dashboard == null)
                                page.Dashboard = dashboard;
                            page.DisplayAsTile = false;
                            page.ExpandOrganizationList = false;
                            page.MaxItems = dto.MaxItems;
                            page.MaxMoreItems = dto.MaxMoreItems;
                            page.MiniTileDisplayItems = 0;
                            page.More = dto.More;
                            page.Noticeboard = DisplayNoticeboard.Hide;
                            page.PlainLayout =  PlainLayout.full;
                            if (dto.Range.IsValid(dto.MaxItems))
                                page.Range = dto.Range;
                            else
                                page.Range = new RangeSettings() { DisplayItems = dto.MaxItems, HigherLimit = dto.MaxItems, LowerLimit = 0 };
                            break;
                        case DashboardViewType.Subscribe:
                            page.AutoUpdateLayout = dto.AutoUpdateLayout;
                            if (page.Dashboard == null)
                                page.Dashboard = dashboard;
                            page.DisplayAsTile = false;
                            page.ExpandOrganizationList = false;
                            page.MaxItems = dto.MaxItems;
                            page.MaxMoreItems = dto.MaxMoreItems;
                            page.MiniTileDisplayItems = 0;
                            page.More = dto.More;
                            page.Noticeboard = DisplayNoticeboard.Hide;
                            page.PlainLayout = PlainLayout.full;
                            if (dto.Range.IsValid(dto.MaxItems))
                                page.Range = dto.Range;
                            else
                                page.Range = new RangeSettings() { DisplayItems = dto.MaxItems, HigherLimit = dto.MaxItems, LowerLimit = 0 };
                            break;
                        }
                    }
                }
            #endregion

            #region "Save Tile Order"
                public Boolean DashboardExist(long idDashboard)
                {
                    Boolean found = false;
                    try
                    {
                        found = (from d in Manager.GetIQ<liteDashboardSettings>() where d.Id == idDashboard select d.Id).Any();
                    }
                    catch (Exception ex)
                    {

                    }
                    return found;
                }
                public List<dtoTileForReorder> DashboardGetTilesForReorder(long idDashboard, WizardDashboardStep step) {
                    List<dtoTileForReorder> items = new List<dtoTileForReorder>();
                    try
                    {
                        Int32 idDefaultLanguage = Manager.GetDefaultLanguage().Id;
                        var tQyery = GetTileQuery(step);
                        List<dtoTileForReorder> fItems = tQyery.ToList().Select(t=> new dtoTileForReorder(t, UC.Language.Id,idDefaultLanguage)).ToList();

                        var query = (from a in Manager.GetIQ<smallDashboardTileAssignment>() where a.IdDashboard == idDashboard && a.Deleted != BaseStatusDeleted.Automatic && a.Deleted != BaseStatusDeleted.Cascade select a);
                        switch (step)
                        {
                            case WizardDashboardStep.CommunityTypes:
                                query = query.Where(a => a.Tile != null && a.Tile.Type == TileType.CommunityType);
                                break;
                            case WizardDashboardStep.Modules:
                                query = query.Where(a => a.Tile != null && (a.Tile.Type == TileType.Module || a.Tile.Type == TileType.UserDefined));
                                break;
                            case WizardDashboardStep.Tiles:
                                query = query.Where(a => a.Tile != null && (a.Tile.Type == TileType.CombinedTags || a.Tile.Type == TileType.CommunityTag || a.Tile.Type == TileType.DashboardUserDefined));
                                break;
                        }
                        List<smallDashboardTileAssignment> assignments = query.Where(t => t.Tile != null).ToList();
                        foreach (dtoTileForReorder item in fItems)
                        {
                            smallDashboardTileAssignment assignment = assignments.Where(a => item.IdTile == a.Tile.Id).FirstOrDefault();
                            if (assignment != null)
                            {
                                item.IdAssignment = assignment.Id;
                                item.AssignmentStatus = assignment.Status;
                                item.DisplayOrder = assignment.DisplayOrder;
                                item.Deleted = assignment.Deleted;
                            }
                            items.Add(item);
                        }
                        //return items.OrderByDescending(i => i.IsAssigned).ThenBy(i => i.DisplayOrder).ThenBy(i => i.Name).ToList();
                        items = items.OrderBy(i=>i.IsNotAssigned).ThenBy(i => i.DisplayOrder).ThenBy(i => i.Name).ToList();
                    }
                    catch (Exception ex)
                    {
                        items = null;
                    }
                    return items;
                }

                public Boolean DashboardTilesSaveOrder(long idDashboard, List<dtoTileForReorder> tiles){
                    Boolean result = false;
                    try{
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        DashboardSettings dashboard = Manager.Get<DashboardSettings>(idDashboard);
                        if (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser){
                            List<DashboardTileAssignment> assignments = (from a in Manager.GetIQ<DashboardTileAssignment>() where a.Dashboard != null && a.Dashboard.Id == idDashboard select a).ToList();
                            Manager.BeginTransaction();
                            DateTime d = DateTime.Now;
                            foreach (dtoTileForReorder t in tiles)
                            {
                                DashboardTileAssignment assignment = assignments.Where(a => a.Tile != null && a.Tile.Id == t.IdTile).FirstOrDefault();
                                if (assignment != null)
                                {
                                    assignment.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress, d);
                                    assignment.DisplayOrder = t.DisplayOrder;
                                    if (t.AssignmentStatus == AvailableStatus.Available)
                                    {
                                        assignment.Deleted = BaseStatusDeleted.None;
                                        assignment.Status = AvailableStatus.Available;
                                    }
                                    else if (assignment.Status == AvailableStatus.Available)
                                        assignment.Status = AvailableStatus.Unavailable;
                                    Manager.SaveOrUpdate(assignment);
                                }
                                else {
                                    Tile tile = Manager.Get<Tile>(t.IdTile);
                                    if (tile != null && tile.Status == AvailableStatus.Available)
                                    {
                                        assignment = new DashboardTileAssignment();
                                        assignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress, d);
                                        assignment.Status = AvailableStatus.Available;
                                        assignment.Tile = tile;
                                        assignment.Dashboard = dashboard;
                                        assignment.DisplayOrder = t.DisplayOrder;
                                        Manager.SaveOrUpdate(assignment);
                                    }
                                }
                            }
                            Manager.Commit();
                            result = true;
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.DashboardTiles(idDashboard));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return result;
                }
            #endregion

                private IEnumerable<liteSearchTile> GetTileQuery(WizardDashboardStep step)
                {
                    var tQyery = (from t in Manager.GetIQ<liteSearchTile>()
                                  where t.Deleted == BaseStatusDeleted.None
                                  select t);
                    switch (step)
                    {
                        case WizardDashboardStep.CommunityTypes:
                            tQyery = tQyery.Where(t => t.Type == TileType.CommunityType);
                            break;
                        case WizardDashboardStep.Modules:
                            tQyery = tQyery.Where(t => t.Type == TileType.Module || t.Type == TileType.UserDefined);
                            break;
                        case WizardDashboardStep.Tiles:
                            tQyery = tQyery.Where(t => t.Type == TileType.CombinedTags || t.Type == TileType.CommunityTag || t.Type == TileType.DashboardUserDefined);
                            break;
                    }
                    return tQyery;
                }
        #endregion

        #region Wizard
            public List<lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>> GetAvailableSteps(WizardDashboardStep current, long idDashboard, DashboardType type= DashboardType.Portal, Int32 idCommunity=0)
            {
                liteDashboardSettings settings = (idDashboard > 0) ? Manager.Get<liteDashboardSettings>(idDashboard) : null;
                if (settings != null)
                {
                    type = settings.Type;
                    idCommunity = settings.IdCommunity;
                }
                return GetAvailableSteps(current, (idDashboard > 0) ? Manager.Get<liteDashboardSettings>(idDashboard) : null, idDashboard, type, idCommunity);
            }
            public List<lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>> GetAvailableSteps(WizardDashboardStep current, liteDashboardSettings settings, long idDashboard, DashboardType type, Int32 idCommunity)
            {
                List<lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>> items = new List<lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>>();
                WizardDashboardStep startStep = WizardDashboardStep.Settings;
                if (current == WizardDashboardStep.None)
                    current = startStep;

                dtoSettingsStep sStep = new dtoSettingsStep(WizardDashboardStep.Settings, settings);
                items.Add(new lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>()
                {
                    DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.first,
                    Id = sStep,
                    AutoPostBack = false,
                    Status = sStep.Status,
                    Active = (idDashboard == 0) || (current == WizardDashboardStep.Settings),
                    Url = (idDashboard == 0) ? "" : RootObject.DashboardEdit(idDashboard, type, idCommunity,false)
                });

                dtoHomeSettingsStep rStep = new dtoHomeSettingsStep(WizardDashboardStep.HomepageSettings, settings);
                items.Add(new lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>()
                {
                    Id = rStep,
                    AutoPostBack = false,
                    Status =rStep.Status,
                    Active = (current == WizardDashboardStep.HomepageSettings),
                    Url = (idDashboard == 0) ? "" : RootObject.DashboardEditViews(idDashboard, type, idCommunity)
                });

                IEnumerable<smallDashboardTileAssignment> query = (from t in Manager.GetIQ<smallDashboardTileAssignment>()
                                                                  where t.Deleted == BaseStatusDeleted.None && t.IdDashboard == idDashboard && t.Tile != null
                                                                    select t);
                switch (type)
                {
                    case DashboardType.Portal:
                        dtoTileStep dStep = GetTileStepInfo(settings, query, WizardDashboardStep.Tiles);
                        items.Add(new lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>()
                        {
                            Id = dStep,
                            AutoPostBack = false,
                            Status = dStep.Status,
                            Active = (current == WizardDashboardStep.Tiles),
                            DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.none,
                            Url = (idDashboard == 0) ? "" : RootObject.DashboardTileReorder(WizardDashboardStep.Tiles, idDashboard, type, idCommunity)
                        });
                        dStep = GetTileStepInfo(settings, query, WizardDashboardStep.CommunityTypes);
                        items.Add(new lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>()
                        {
                            Id = dStep,
                            AutoPostBack = false,
                            Status = dStep.Status,
                            Active = (current == WizardDashboardStep.CommunityTypes),
                            DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last,
                            Url = (idDashboard == 0) ? "" : RootObject.DashboardTileReorder(WizardDashboardStep.CommunityTypes, idDashboard, type, idCommunity)
                        });
                        break;
                    default:
                          dStep = GetTileStepInfo(settings, query, WizardDashboardStep.Modules);
                        items.Add(new lm.Comol.Core.Wizard.dtoNavigableWizardItem<dtoDashboardStep>()
                        {
                            Id = dStep,
                            AutoPostBack = false,
                            Status = dStep.Status,
                            Active = (current == WizardDashboardStep.Modules),
                            DisplayOrderDetail = Core.Wizard.DisplayOrderEnum.last,
                            Url = (idDashboard == 0) ? "" : RootObject.DashboardTileReorder(WizardDashboardStep.Modules, idDashboard, type, idCommunity)
                        });
                        break;
                }
                return items;
            }

            private dtoTileStep GetTileStepInfo(liteDashboardSettings settings, IEnumerable<smallDashboardTileAssignment> query, WizardDashboardStep step)
            {
                dtoTileStep item = new dtoTileStep(step);
                if (settings != null)
                {
                    switch (step)
                    {
                        case WizardDashboardStep.Tiles:
                            query = query.Where(t => t.Tile.Type != TileType.CommunityType);
                            item.Status = ((settings.Container.AvailableViews.Contains(DashboardViewType.Combined)
                                           || settings.Container.AvailableViews.Contains(DashboardViewType.Tile))
                                           && (settings.Container.AvailableGroupBy.Contains(GroupItemsBy.Tag)
                                           || settings.Container.AvailableGroupBy.Contains(GroupItemsBy.Tile))) ? Wizard.WizardItemStatus.none : Wizard.WizardItemStatus.disabled;
                             break;
                        case WizardDashboardStep.CommunityTypes:
                            query = query.Where(t => t.Tile.Type == TileType.CommunityType);
                                                        item.Status = ((settings.Container.AvailableViews.Contains(DashboardViewType.Combined)
                                           || settings.Container.AvailableViews.Contains(DashboardViewType.Tile))
                                           && (settings.Container.AvailableGroupBy.Contains(GroupItemsBy.CommunityType))) ? Wizard.WizardItemStatus.none : Wizard.WizardItemStatus.disabled;
                            break;
                        case WizardDashboardStep.Modules:
                                                        item.Status = ((settings.Container.AvailableViews.Contains(DashboardViewType.Combined)
                                           || settings.Container.AvailableViews.Contains(DashboardViewType.Tile))
                                           && (settings.Container.AvailableGroupBy.Contains(GroupItemsBy.Service))) ? Wizard.WizardItemStatus.none : Wizard.WizardItemStatus.disabled;
                            break;
                    }
                    item.Tiles = GetTileQuery(step).Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => t.Id).Count();
                    item.AvailableTiles = GetTileQuery(step).Where(t => t.Deleted == BaseStatusDeleted.None && t.Status == AvailableStatus.Available).Select(t => t.Id).Count();
                    item.InUseTiles = query.Where(t => t.Deleted == BaseStatusDeleted.None && t.Status == AvailableStatus.Available && t.Tile != null && t.Tile.Deleted== BaseStatusDeleted.None  && t.Tile.Status == AvailableStatus.Available).Select(i => i.Id).Count();
                    item.UserTile = query.Where(t => t.Deleted == BaseStatusDeleted.None && t.Status == AvailableStatus.Available && t.Tile != null && t.Status == AvailableStatus.Available && (t.Tile.Type == TileType.UserDefined || t.Tile.Type == TileType.DashboardUserDefined)).Select(i => i.Id).Count();
                    if (item.Status != Wizard.WizardItemStatus.disabled)
                    {
                        if (item.AvailableTiles > 0 && item.InUseTiles == 0)
                        {
                            item.Errors.Add(EditingErrors.NoTilesSelected);
                            item.Status = Wizard.WizardItemStatus.warning;
                        }
                        else if (item.AvailableTiles == 0)
                        {
                            item.Errors.Add(EditingErrors.NoTiles);
                            item.Status = Wizard.WizardItemStatus.error;
                        }
                        else
                            item.Status = Wizard.WizardItemStatus.valid;
                    }
                }
                else
                    item.Status = (settings == null) ? Core.Wizard.WizardItemStatus.disabled : Core.Wizard.WizardItemStatus.none;
                return item;
            }
        #endregion

        public List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> GetDashboardAvailableRoles(liteDashboardSettings settings)
        {
            List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> roles = Manager.GetTranslatedItem<dtoTranslatedRoleType>(UC.Language.Id);
            List<Int32> idRoles = Manager.GetAvailableRoles(settings.IdCommunity);
            if (settings.Active)
            {
                List<liteDashboardSettings> sItems = (from s in Manager.GetIQ<liteDashboardSettings>()
                                                      where s.Active && s.Id != settings.Id && s.Type == settings.Type
                                                      select s).ToList();
                List<Int32> inUse = sItems.SelectMany(s => s.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == DashboardAssignmentType.RoleType).Select(a => a.IdRole)).Distinct().ToList();

                roles = roles.Where(t => !inUse.Contains(t.Id)).ToList();
            }


            return roles.Where(r => idRoles.Contains(r.Id)).OrderBy(r => r.Name).ToList();
        }
        public List<lm.Comol.Core.DomainModel.dtoTranslatedProfileType> GetDashboardAvailableProfileTypes(liteDashboardSettings settings)
        {
            List<lm.Comol.Core.DomainModel.dtoTranslatedProfileType> types = Manager.GetTranslatedItem<dtoTranslatedProfileType>(UC.Language.Id);
            if (settings.Active)
            {
                List<liteDashboardSettings> sItems = (from s in Manager.GetIQ<liteDashboardSettings>()
                                                      where s.Active && s.Id != settings.Id && s.Type == settings.Type
                                                      select s).ToList();
                List<Int32> inUse = sItems.SelectMany(s => s.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == DashboardAssignmentType.ProfileType).Select(a => a.IdProfileType)).Distinct().ToList();

                types = types.Where(t => !inUse.Contains(t.Id )).ToList();
            }
            return types.OrderBy(r => r.Name).ToList();
        }
    }
}