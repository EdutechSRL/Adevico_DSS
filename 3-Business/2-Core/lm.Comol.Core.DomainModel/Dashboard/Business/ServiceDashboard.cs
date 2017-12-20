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
        protected const Int32 maxItemsForQuery = 500;
        protected iApplicationContext _Context;
        private Tag.Business.ServiceTags _service;
        protected Tag.Business.ServiceTags Service { get { return (_service==null) ? new Tag.Business.ServiceTags(_Context) : _service;  } }


        #region initClass
      
            public ServiceDashboard() :base() { }
            public ServiceDashboard(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
                _service = new Tag.Business.ServiceTags(oContext);
            }
            public ServiceDashboard(iDataContext oDC)
                : base(oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext() { DataContext = oDC };
                _service = new Tag.Business.ServiceTags(oDC);
            }
        #endregion

        #region "Permission"
            private int idModule;
            public int ServiceModuleID()
            {
                if (idModule<1)
                    idModule = Manager.GetModuleID(ModuleDashboard.UniqueCode);
                return idModule;
            }
            public ModuleDashboard GetPermission(Int32 idCommunity)
            {
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                if (idCommunity <= 0)
                {
                    if (person == null)
                        return ModuleDashboard.CreatePortalmodule((int)UserTypeStandard.Guest);
                    else
                        return ModuleDashboard.CreatePortalmodule(person.TypeID);
                }
                else {
                    return new ModuleDashboard(Manager.GetModulePermission(UC.CurrentUserID, idCommunity, ServiceModuleID()));
                }
            }

        #endregion

        #region "Loading"
            public liteDashboardSettings DashboardSettingsGet(DashboardType type, Int32 idCommunity = -1, Boolean useCache = true)
            {
                liteDashboardSettings settings = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<liteDashboardSettings>(CacheKeys.CurrentDashboardSettings(UC.CurrentUserID, idCommunity)) : null;
                if (settings == null) { 
                    litePerson p = Manager.Get<litePerson>(UC.CurrentUserID);
                    if (p!=null){
                        switch (type)
                        {
                            case DashboardType.Portal:
                                settings = GetPortalSettings(p, useCache);
                                if (settings !=null && useCache)
                                    CacheHelper.AddToCache<liteDashboardSettings>(CacheKeys.CurrentDashboardSettings(p.Id, 0), (liteDashboardSettings)settings.Clone(), CacheExpiration.Week);
                                break;
                            case DashboardType.Community:
                                settings = GetCommunitySettings(p,idCommunity, useCache);
                                if (settings != null && useCache)
                                    CacheHelper.AddToCache<liteDashboardSettings>(CacheKeys.CurrentDashboardSettings(p.Id, idCommunity), (liteDashboardSettings)settings.Clone(), CacheExpiration.Week);
                                break;
                            case DashboardType.AllCommunities:
                                settings = GetCommonCommunitySettings(p, useCache);
                                if (settings != null && useCache)
                                    CacheHelper.AddToCache<liteDashboardSettings>(CacheKeys.CurrentDashboardSettings(p.Id, -1), (liteDashboardSettings)settings.Clone(), CacheExpiration.Week);
                                break;
                        }
                    }
                }
                return settings;
            }

            #region "Get all available settings"
            private liteDashboardSettings GetPortalSettings(litePerson p, Boolean useCache = true)
            {
                liteDashboardSettings result = null;
                List<liteDashboardSettings> aSettings = LoadingSettingsGetAll(DashboardType.Portal,0, useCache);
                if (aSettings != null && aSettings.Any())
                {
                    liteDashboardSettings dSettings = aSettings.Where(s => s.ForAll).FirstOrDefault();
                    if (aSettings.Where(s => s.IsAvailableFor(p.Id, 0, p.TypeID)).Any())
                    {
                        result = aSettings.Where(s => s.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.IdPerson == p.Id).Any()).FirstOrDefault();
                        if (result == null)
                            result = aSettings.Where(s => s.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.IdProfileType == p.TypeID).Any()).FirstOrDefault();
                        if (result == null)
                            return dSettings;
                    }
                    else
                        return dSettings;
                }
                return result;
            }

            private liteDashboardSettings GetCommunitySettings(litePerson p, Int32 idCommunity, Boolean useCache = true){
                liteDashboardSettings result = null;
                List<liteDashboardSettings> aSettings = LoadingSettingsGetForCommunity(idCommunity, useCache);
                if (aSettings==null || !aSettings.Any())
                   return GetCommonCommunitySettings(p,useCache);
                else{
                    liteDashboardSettings dSettings = aSettings.Where(s => s.ForAll).FirstOrDefault();

                    Int32 idRole = 0;
                    if (idCommunity > 0 && aSettings.Where(s=> s.Assignments.Where(a=> a.Deleted== BaseStatusDeleted.None && a.IdRole>0).Any()).Any())
                        idRole = Manager.GetSubscriptionIdRole(UC.CurrentUserID, idCommunity);
                    if (aSettings.Where(s => s.IsAvailableFor(p.Id, idRole, p.TypeID)).Any())
                    {
                        result = aSettings.Where(s => s.Assignments.Where(a=> a.Deleted== BaseStatusDeleted.None && a.IdPerson== p.Id).Any()).FirstOrDefault();
                        if (result == null && idRole >0)
                            result = aSettings.Where(s => s.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.IdRole == idRole).Any()).FirstOrDefault();
                        if (result == null)
                            result = aSettings.Where(s => s.Assignments.Where(a => a.Deleted == BaseStatusDeleted.None && a.IdProfileType == p.TypeID).Any()).FirstOrDefault();
                        if (result == null)
                            return dSettings;
                    }
                    else
                        return dSettings;
                }
                return result;

            }
            private liteDashboardSettings GetCommonCommunitySettings(litePerson p,  Boolean useCache = true) {
                liteDashboardSettings result = null;
                List<liteDashboardSettings> aSettings = LoadingSettingsGetForAllCommunity(useCache);
                if (aSettings != null && aSettings.Any())
                  result = aSettings.Where(s => s.ForAll).FirstOrDefault();
                return result;
            }
            public List<liteDashboardSettings> LoadingSettingsGetAll(DashboardType type, Int32 idCommunity, Boolean useCache = true)
            {
                List<liteDashboardSettings> settings = null;
                String cacheKey = "";
                switch (type) {
                    case DashboardType.AllCommunities:
                    case DashboardType.Portal:
                        cacheKey = CacheKeys.Dashboard(type);
                        break;
                    case DashboardType.Community:
                        settings = LoadingSettingsGetForCommunity(idCommunity,useCache);
                        if (settings == null)
                            cacheKey = CacheKeys.Dashboard(DashboardType.AllCommunities);
                        break;

                }
                if (type!= DashboardType.Community || settings == null)
                    settings = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<liteDashboardSettings>>(cacheKey) : null;

                if (settings == null || !settings.Any())
                {
                    settings = (from t in Manager.GetIQ<liteDashboardSettings>() where t.Active && t.Deleted == BaseStatusDeleted.None select t).ToList();
                    if (settings!=null && settings.Any())
                        Manager.DetachList(settings);

                    if (settings != null && useCache)
                        CacheHelper.AddToCache<List<liteDashboardSettings>>(cacheKey, settings, CacheExpiration.Week);
                }
                return settings;
            }
            private List<liteDashboardSettings> LoadingSettingsGetForCommunity(Int32 idCommunity, Boolean useCache = true)
            {
                List<liteDashboardSettings> settings = null;
                settings = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<liteDashboardSettings>>(CacheKeys.Dashboard(idCommunity)) : null;

                if (settings == null || !settings.Any())
                {
                    settings = (from t in Manager.GetIQ<liteDashboardSettings>() where t.Deleted == BaseStatusDeleted.None && t.Active && t.IdCommunity== idCommunity && t.Type== DashboardType.Community select t).ToList();
                    if (settings != null && settings.Any())
                        Manager.DetachList(settings);
                    if (settings != null && useCache)
                        CacheHelper.AddToCache<List<liteDashboardSettings>>(CacheKeys.Dashboard(idCommunity), settings, CacheExpiration.Week);
                }
                return settings;
            }
            private List<liteDashboardSettings> LoadingSettingsGetForAllCommunity(Boolean useCache = true)
            {
                List<liteDashboardSettings> settings = null;
                settings = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<List<liteDashboardSettings>>(CacheKeys.Dashboard(DashboardType.AllCommunities)) : null;

                if (settings == null || !settings.Any())
                {
                    settings = (from t in Manager.GetIQ<liteDashboardSettings>() where t.Deleted == BaseStatusDeleted.None && t.Active && t.Type == DashboardType.AllCommunities select t).ToList();
                    if (settings != null && settings.Any())
                        Manager.DetachList(settings);
                    if (settings != null && useCache)
                        CacheHelper.AddToCache<List<liteDashboardSettings>>(CacheKeys.Dashboard(DashboardType.AllCommunities), settings, CacheExpiration.Week);
                }
                return settings;
            }
            #endregion
            public liteUserDashboardSettings UserPortalDashboardSettingsGet(Int32 idUser, Boolean useCache = true, liteDashboardSettings settings = null) {
                return UserDashboardSettingsGet(idUser, useCache, 0, settings);
            }
            public liteUserDashboardSettings UserCommunityDashboardSettingsGet(Int32 idUser,Int32 idCommunity, Boolean useCache = true,   liteDashboardSettings settings = null)
            {
                return UserDashboardSettingsGet(idUser, useCache, idCommunity, settings);
            }

            private liteUserDashboardSettings UserDashboardSettingsGet(Int32 idUser, Boolean useCache = true, Int32 idCommunity = -1, liteDashboardSettings settings = null)
            {
                liteUserDashboardSettings userSettings = null;

                userSettings = (useCache) ? lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<liteUserDashboardSettings>(CacheKeys.UserSettings(idUser, idCommunity)) : null;

                if (userSettings == null)
                {
                    userSettings = (from t in Manager.GetIQ<liteUserDashboardSettings>() where t.Deleted == BaseStatusDeleted.None && t.IdPerson == idUser && t.IdCommunity == idCommunity select t).Skip(0).Take(1).FirstOrDefault();
                    if (userSettings==null && idCommunity>0)
                        userSettings = (from t in Manager.GetIQ<liteUserDashboardSettings>() where t.Deleted == BaseStatusDeleted.None && t.IdPerson == idUser && t.IdCommunity == -1 select t).Skip(0).Take(1).FirstOrDefault();
                    if (userSettings != null)
                        Manager.Detach(userSettings);
                    if (userSettings != null && useCache)
                        CacheHelper.AddToCache<liteUserDashboardSettings>(CacheKeys.UserSettings(idUser, idCommunity), userSettings, CacheExpiration.Week);
                }
                return userSettings;
            }
        #endregion
    }
}