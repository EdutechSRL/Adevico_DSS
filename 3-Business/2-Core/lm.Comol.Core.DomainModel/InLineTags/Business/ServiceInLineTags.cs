using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using NHibernate.Linq;
using lm.Comol.Core.InLineTags.Domain;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel.Filters;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.InLineTags.Business
{
    public partial class ServiceInLineTags : CoreServices 
    {
        protected const Int32 maxItemsForQuery = 500;
        protected iApplicationContext _Context;

        #region initClass
            public ServiceInLineTags() :base() { }
            public ServiceInLineTags(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
            }
            public ServiceInLineTags(iDataContext oDC)
                : base(oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext() { DataContext = oDC };
            }
        #endregion

        #region "Permission"
            //public int ServiceModuleID()
            //{
            //    return this.Manager.GetModuleID(ModuleTags.UniqueCode);
            //}
            //public ModuleTags GetPermission(Int32 idCommunity)
            //{
            //    Person person = Manager.GetPerson(UC.CurrentUserID);
            //    if (idCommunity <= 0)
            //    {
            //        if (person == null)
            //            return ModuleTags.CreatePortalmodule((int)UserTypeStandard.Guest);
            //        else
            //            return ModuleTags.CreatePortalmodule(person.TypeID);
            //    }
            //    else {
            //        return new ModuleTags(this.Manager.GetModulePermission(UC.CurrentUserID, idCommunity, ServiceModuleID()));
            //    }
            //}

        #endregion

        #region "Manage"
            public List<InLineTag> SaveTags(litePerson person, Int32 idCommunity, Int32 idModule, String moduleCode, List<String> tags)
            {
                Boolean clearCache = false;
                Boolean isInTransaction = Manager.IsInTransaction();
                List<InLineTag> items = new List<InLineTag>();
                if (person != null)
                {
                    foreach (String t in tags.Where(t => !String.IsNullOrEmpty(t)))
                    {
                        String name = t.ToLower();
                        InLineTag tag = GetTag(name, person.Id, idCommunity, idModule, moduleCode);
                        if (tag != null && tag.Deleted == BaseStatusDeleted.None)
                            items.Add(tag);
                        else if (tag != null && tag.Deleted != BaseStatusDeleted.None)
                        {
                            if (!isInTransaction)
                                Manager.BeginTransaction();
                            tag.Name = name;
                            tag.IdPerson = person.Id;
                            tag.Deleted = BaseStatusDeleted.None;
                            tag.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            Manager.SaveOrUpdate(tag);
                            if (!isInTransaction) 
                                Manager.Commit();
                            clearCache = true;
                        }
                        else
                        {
                            tag = new InLineTag();
                            tag.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            tag.IdCommunity = idCommunity;
                            tag.IdModule = idModule;
                            tag.IdPerson =  person.Id;
                            tag.ModuleCode = moduleCode;
                            tag.Name = name;
                            if (!isInTransaction)
                                Manager.BeginTransaction();
                            Manager.SaveOrUpdate(tag);
                            if (!isInTransaction)
                                Manager.Commit();
                            clearCache = true;
                        }
                    }
                    if (clearCache){
                        if (idCommunity > 0)
                            CacheHelper.PurgeCacheItems(CacheKeys.ComunityTags(idCommunity));
                        else if (person.Id > 0)
                            CacheHelper.PurgeCacheItems(CacheKeys.UserTags(person.Id));
                        else
                            CacheHelper.PurgeCacheItems(CacheKeys.AllTags);
                    }
                }
                return items;
            }
            public InLineTag SaveTag(Int32 idPerson, Int32 idCommunity, Int32 idModule, String moduleCode,String name, long idTag = 0)
            {
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                InLineTag tag = Manager.Get<InLineTag>(idTag);
                if (person != null && (tag != null || (tag == null && idTag == 0)))
                {
                    name = String.IsNullOrEmpty(name) ? "" : name.ToLower();
                    if (tag == null)
                    {
                        tag = new InLineTag();
                        tag.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        tag.IdCommunity=idCommunity;
                        tag.IdModule=idModule;
                        tag.IdPerson=idPerson;
                        tag.ModuleCode=moduleCode;
                        idTag = 0;
                    }
                    else
                        tag.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);

                    InLineTag existTag = GetTag(name,idPerson,idCommunity,idModule, moduleCode);
                    if (existTag == null)
                    {
                        Manager.BeginTransaction();
                        tag.Name = name;
                        Manager.SaveOrUpdate(tag);
                        Manager.Commit();
                        if (idCommunity > 0)
                            CacheHelper.PurgeCacheItems(CacheKeys.ComunityTags(idCommunity));
                        else if (idPerson > 0)
                            CacheHelper.PurgeCacheItems(CacheKeys.UserTags(idPerson));
                        else
                            CacheHelper.PurgeCacheItems(CacheKeys.AllTags);
                    }
                    else if (existTag.Deleted != BaseStatusDeleted.None && idTag==0)
                    {
                        Manager.BeginTransaction();
                        tag.Name = name;
                        tag.IdPerson = idPerson;
                        tag.Deleted = BaseStatusDeleted.None;
                        Manager.SaveOrUpdate(tag);
                        Manager.Commit();
                        if (idCommunity > 0)
                            CacheHelper.PurgeCacheItems(CacheKeys.ComunityTags(idCommunity));
                        else if (idPerson > 0)
                            CacheHelper.PurgeCacheItems(CacheKeys.UserTags(idPerson));
                        else
                            CacheHelper.PurgeCacheItems(CacheKeys.AllTags);
                    }
                    else if (existTag.Id != tag.Id)
                        return existTag;
                    else
                        return tag;
                }
                return tag;
            }
            public InLineTag GetTag(String name, Int32 idPerson, Int32 idCommunity, Int32 idModule, String moduleCode)
            {
                name = String.IsNullOrEmpty(name) ? "" : name.ToLower();
                if (idPerson > 0 && idCommunity==0)
                    return (from t in Manager.GetIQ<InLineTag>()
                            where t.Name == name && t.IdCommunity == idCommunity && t.IdModule == idModule && t.ModuleCode == moduleCode
                            && t.IdPerson == idPerson
                            select t).Skip(0).Take(1).ToList().FirstOrDefault();
                else
                    return (from t in Manager.GetIQ<InLineTag>()
                            where t.Name == name && t.IdCommunity == idCommunity && t.IdModule == idModule && t.ModuleCode == moduleCode
                            select t).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public InLineTag VirtualDelete(long idTag, Boolean delete)
            {
                InLineTag item = null;
                try {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    item = Manager.Get<InLineTag>(idTag);
                    if (item != null && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        Manager.BeginTransaction();
                        item.Deleted = (delete) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                        item.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    
                        Manager.SaveOrUpdate(item);
                        Manager.Commit();
                        if (item.IdCommunity>0)
                            CacheHelper.PurgeCacheItems(CacheKeys.ComunityTags(item.IdCommunity));
                        else if (item.IdPerson > 0)
                            CacheHelper.PurgeCacheItems(CacheKeys.UserTags(item.IdPerson));
                        else
                            CacheHelper.PurgeCacheItems(CacheKeys.AllTags);
                    }
                }
                catch (Exception ex)
                {
                    item = null;
                    Manager.RollBack();
                }
                return item;
            }


            public List<String> GetAvailableTags(Int32 idPerson, Int32 idCommunity, Int32 idModule)
            {
                return GetAvailableTags(idPerson, idCommunity, idModule, Manager.GetModuleCode(idModule));
            }
            public List<String> GetAvailableTags(Int32 idPerson, Int32 idCommunity, String moduleCode)
            {
                return GetAvailableTags(idPerson, idCommunity, Manager.GetModuleID(moduleCode), moduleCode);
            }
            public List<String> GetAvailableTags(Int32 idPerson, Int32 idCommunity, Int32 idModule, String moduleCode)
            {
                List<String> items = null;
                try
                {
                    items = (from t in Manager.GetIQ<InLineTag>()
                             where t.Deleted == BaseStatusDeleted.None && ((idCommunity == 0 && t.IdPerson == idPerson) || idCommunity>0) && t.IdCommunity == idCommunity && t.IdModule == idModule && t.ModuleCode == moduleCode
                             select t.Name).ToList().Distinct().OrderBy(t=>t).ToList();
                }
                catch (Exception ex)
                {

                }
                return items;
            }

            public Boolean HasDuplicate(String name, Int32 idCommunity, Int32 idModule,long idTag=0)
            {
                return HasDuplicate(name, idCommunity, idModule, Manager.GetModuleCode(idModule), idTag);
            }
            public Boolean HasDuplicate(String name, Int32 idCommunity, String moduleCode, long idTag = 0)
            {
                return HasDuplicate(name, idCommunity, Manager.GetModuleID(moduleCode), moduleCode, idTag);
            }
            public Boolean HasDuplicate(String name, Int32 idCommunity, Int32 idModule, String moduleCode, long idTag = 0)
            {
                Boolean result = false;
                try
                {
                    name = String.IsNullOrEmpty(name) ? "" : name.ToLower();
                    result = (from t in Manager.GetIQ<liteInLineTag>()
                              where t.Id != idTag && t.Deleted == BaseStatusDeleted.None && t.IdModule == idModule && t.ModuleCode == moduleCode && t.IdCommunity == idCommunity
                              && t.Name == name
                              select t.Id).Any();
                }
                catch (Exception ex) { 
                
                
                }
                return result;
            }

        #endregion
    }
}