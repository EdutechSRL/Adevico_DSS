using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using NHibernate.Linq;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Filters;
using lm.Comol.Core.Tag.Domain;
using lm.Comol.Core.BaseModules.Tags.Presentation;

namespace lm.Comol.Core.BaseModules.Tags.Business
{
    public partial class ServiceTags : lm.Comol.Core.Tag.Business.ServiceTags
    {
        private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement _serviceCommunityManagement;
        protected lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement ServiceCommunityManagement { get { return (_serviceCommunityManagement == null) ? new lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(_Context) : _serviceCommunityManagement; } }

        private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities _ServiceDashboard;
        protected lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceDashboard { get { return (_ServiceDashboard == null) ? new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(_Context) : _ServiceDashboard; } }

        #region initClass
            public ServiceTags() :base() { }
            public ServiceTags(iApplicationContext oContext)
                : base(oContext)
            {

            }
            public ServiceTags(iDataContext oDC)
                : base(oDC)
            {

            }
        #endregion

        #region "Manage Tag"
            public lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation GetTagTranslation(long idTag, Int32 idLanguage)
            {
                lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation result = null;
                try
                {
                    liteTagItem tag = Manager.Get<liteTagItem>(idTag);
                    if (tag != null)
                        result = tag.GetTranslation(idLanguage, Manager.GetDefaultLanguage().Id);
                }
                catch (Exception ex)
                {

                }
                if (result == null)
                    result = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
                return result;
            }
            public dtoTag GetDtoTag(long idTag, Boolean forEdit, String dLanguageName, String dLanguageCode, Int32 idOrganization=-1, String dTitle ="")
            {
                dtoTag result = null;
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                liteTagItem tag = Manager.Get<liteTagItem>(idTag);
                if (((idTag >0 && tag != null) || idTag==0) && person!=null)
                {
                    Dictionary<Int32, ModuleTags> permissions = new Dictionary<Int32, ModuleTags>();
                    permissions.Add(-3, ModuleTags.CreatePortalmodule(person.TypeID));
                    if (tag==null){
                        tag = new liteTagItem();
                        tag.Id = idTag;
                        tag.DefaultTranslation = new DomainModel.Languages.TitleDescriptionObjectTranslation() { Title = dTitle };
                        tag.Type = TagType.Community;
                        if (idOrganization > -1)
                        {
                            tag.IsSystem = false;
                            tag.Organizations.Add(new liteOrganizationAvailability() { Id = 0, IdOrganization = idOrganization, IsDefault = true, IdTag = 0 });
                        }
                        else
                            tag.IsSystem = true;
                        tag.IsDefault = false;
                    }
                   

                    result = new dtoTag() { Id= tag.Id, IsDefault= tag.IsDefault, IsReadonly= ! forEdit , IsSystem= tag.IsSystem, Status= tag.Status};
                    if (!tag.IsSystem){
                        Int32 dOrganization = tag.Organizations.Where(o=> o.Deleted== BaseStatusDeleted.None && o.IsDefault).Select(o=> o.IdOrganization).FirstOrDefault();
                        permissions = GetPermissions(person, true);
                        
                        result.IsReadonly= !forEdit && (permissions.ContainsKey(dOrganization) && (permissions[dOrganization].Administration  || permissions[dOrganization].Edit));


                        List<Int32> idAssignedOrganizations = tag.Organizations.Where(o=> o.Deleted== BaseStatusDeleted.None).Select(o=> o.IdOrganization).ToList();
                        List<Int32> idOrganizations = new List<int>();
                        idOrganizations.AddRange(idAssignedOrganizations);
                        idOrganizations.AddRange(permissions.Keys.Where(k=> k>0).ToList());
                        idOrganizations = idOrganizations.Distinct().ToList();

                        List<Organization> organizations = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select o).ToList();
                        result.Organizations= organizations.Select(o=> new FilterListItem() { Id=o.Id, Name= o.Name, Checked= idAssignedOrganizations.Contains(o.Id),
                                                                    Disabled= result.IsReadonly || o.Id ==dOrganization ||
                                                                    (permissions.ContainsKey(o.Id)
                                                                    &&
                                                                    !(permissions[o.Id].Administration || permissions[o.Id].Edit)
                                                                    )
                                                                    }).OrderBy(o=>o.Name).ToList();
                    }
                    else
                        result.IsReadonly= !forEdit && (permissions[-3].Administration  || permissions[-3].Edit);
                    List<Language> languages = Manager.GetAllLanguages().ToList();


                    result.Translations = new List<dtoTranslation>();
                    result.Translations.Add(new dtoTranslation() { IdLanguage=0, LanguageName = dLanguageName, LanguageCode= dLanguageCode, Title= tag.DefaultTranslation.Title, Description= tag.DefaultTranslation.Description});

                    result.Translations.AddRange(languages.OrderByDescending(l=> l.isDefault).ThenBy(l=> l.Name).Select(
                        l=> new dtoTranslation() { IdLanguage=l.Id, LanguageName = l.Name, LanguageCode= l.ShortCode, Title= tag.GetTranslation(l.Id).Title  , Description= tag.GetTranslation(l.Id).Description}).ToList());


                    List<dtoTranslatedCommunityType> tCommunities = Manager.GetTranslatedItem<dtoTranslatedCommunityType>(UC.Language.Id);
                    result.CommunityTypes = tCommunities.Select(t => new FilterListItem() { Id = t.Id, Name = t.Name, Disabled = result.IsReadonly, Checked = (tag.CommunityTypes != null && tag.CommunityTypes.Contains(t.Id))}).ToList();
                    result.SelectedCommunityTypes = result.CommunityTypes.Where(s => s.Checked).ToList();
                }
                return result;
            }
            public Int32 GetIdOrganization(Int32 idCommunity )
            {
                return Manager.GetIdOrganizationFromCommunity(idCommunity);
            }
            public dtoTag SaveTag(dtoTag dto, TagType type, String dLanguageName, String dLanguageCode,Int32 idOrganization = -1)
            {
                dtoTag result = null;
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                TagItem tag = Manager.Get<TagItem>(dto.Id);
                if (person != null && (tag != null || (tag == null && dto.Id == 0)))
                {
                    Dictionary<Int32, ModuleTags> permissions = new Dictionary<Int32, ModuleTags>();
                    permissions.Add(-3, ModuleTags.CreatePortalmodule(person.TypeID));

                    Int32 dOrganization = (dto.Id == 0 && idOrganization > 0) ? idOrganization : (((tag == null && idOrganization == -1) || (tag != null && tag.IsSystem)) ? -3 : tag.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None && o.IsDefault).Select(o => o.IdOrganization).FirstOrDefault());
                    if (tag == null)
                    {
                        tag = new TagItem();
                        tag.Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Draft;
                        tag.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    else
                        tag.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    if (dOrganization > 0)
                        permissions = GetPermissions(person, true);

                    if (permissions.ContainsKey(dOrganization) && (((permissions[dOrganization].Add || permissions[dOrganization].Administration) && dto.Id == 0) || (permissions[dOrganization].Administration || permissions[dOrganization].Edit)))
                    {
                        if (dto.Id == 0)
                        {
                            tag.Type = type;
                            tag.IsDefault = dto.IsDefault;
                            tag.IsSystem = (dOrganization == -3);
                        }
                        if (!dto.Translations.Where(t => t.IdLanguage == 0 && !String.IsNullOrEmpty(t.Title) && !String.IsNullOrEmpty(t.Title.Trim())).Any())
                            throw new TagException(ErrorMessageType.DefaultTranslationMissing, dto);
                        else
                        {
                            dtoTranslation dTranslation = dto.Translations.Where(t => t.IdLanguage == 0).FirstOrDefault();
                            if (HasDefaultDuplicate(tag.Id, dTranslation.Title, type, dOrganization))
                                throw new TagException(ErrorMessageType.DefaultTranslationDuplicate, dto);
                            else{
                                tag.DefaultTranslation = new DomainModel.Languages.TitleDescriptionObjectTranslation() { Title = dTranslation.Title, Description = dTranslation.Description };
                                if (String.IsNullOrEmpty(tag.DefaultTranslation.Description))
                                    tag.DefaultTranslation.Description = "";

                            }
                        }
                     
                        try
                        {
                            Manager.BeginTransaction();
                            Manager.SaveOrUpdate(tag);
                            if (tag.IsDefault)
                            {
                                List<TagItem> tags = null;
                                List<OrganizationAvailability> organizations = new List<OrganizationAvailability>();
                                if (tag.IsSystem)
                                    tags = (from t in Manager.GetIQ<TagItem>()
                                            where t.Deleted == BaseStatusDeleted.None && t.Id != tag.Id && tag.IsDefault
                                            select t).ToList();
                                else
                                {
                                    Int32 idOrg = tag.Organizations.Where(o=> o.IsDefault && o.Deleted== BaseStatusDeleted.None).Select(o=> o.IdOrganization).FirstOrDefault();
                                    List<long> idItems = (from o in Manager.GetIQ<liteOrganizationAvailability>()
                                                         where o.Deleted == BaseStatusDeleted.None && o.IsDefault && o.IdTag != tag.Id
                                                                && ((idOrg > 0 && o.IdOrganization == idOrg) || (idOrg == 0 && idOrganization > 0 && o.IdOrganization == idOrganization))
                                                         select o.Id).ToList();
                                    organizations = (from o in Manager.GetIQ<OrganizationAvailability>() where o.Deleted == BaseStatusDeleted.None && o.IsDefault && idItems.Contains(o.Id) select o).ToList();
                                }

                                foreach (OrganizationAvailability o in organizations)
                                {
                                    o.IsDefault = false;
                                    o.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                                foreach (TagItem t in tags)
                                {
                                    t.IsDefault = false;
                                    t.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                }
                                if (tags.Any())
                                    Manager.SaveOrUpdateList(tags);
                            }
                            Manager.Commit();
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllCommunityTags);
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllUserCommunitiesTags);
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            throw new TagException(ErrorMessageType.SavingTag, dto);
                        }
                        Boolean isNew = (dto.Id == 0);
                        dto.Id = tag.Id;
                        SaveTagTranslation(person, tag,dto, dto.Translations);
                        if (!tag.IsSystem && ( (dto.Organizations.Where(o=> o.Checked).Any()) || (tag.Organizations !=null && tag.Organizations.Any())))
                            SaveOrganizationAssociations(person, tag,dto,  dto.Organizations, dOrganization);
                        if ((dto.SelectedCommunityTypes.Any()) || (tag.CommunityTypes != null && tag.CommunityTypes.Any()))
                            SaveCommunityTypesAssociations(person, tag, dto, dto.SelectedCommunityTypes);
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllCommunityTags);
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllUserCommunitiesTags);
                    }
                    else if (dto.Id > 0)
                        throw new TagException(ErrorMessageType.NoPermissionToEdit, dto);
                    else if (dOrganization == -3)
                        throw new TagException(ErrorMessageType.NoPermissionToAddToSystem, dto);
                    else
                        throw new TagException(ErrorMessageType.NoPermissionToAddToOrganization, dto);


                    result = new dtoTag() { Id = tag.Id, IsDefault = tag.IsDefault, IsReadonly = false, IsSystem = tag.IsSystem, Status = tag.Status };
                    if (!tag.IsSystem)
                    {
                        result.IsReadonly = (permissions.ContainsKey(dOrganization) && (permissions[dOrganization].Administration || permissions[dOrganization].Edit));


                        List<Int32> idAssignedOrganizations = tag.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None).Select(o => o.IdOrganization).ToList();
                        List<Int32> idOrganizations = new List<int>();
                        idOrganizations.AddRange(idAssignedOrganizations);
                        idOrganizations.AddRange(permissions.Keys.Where(k => k > 0).ToList());
                        idOrganizations = idOrganizations.Distinct().ToList();

                        List<Organization> organizations = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select o).ToList();
                        result.Organizations = organizations.Select(o => new FilterListItem()
                        {
                            Id = o.Id,
                            Name = o.Name,
                            Checked = idAssignedOrganizations.Contains(o.Id),
                            Disabled = result.IsReadonly || o.Id == dOrganization ||
                            (permissions.ContainsKey(o.Id)
                            &&
                            !(permissions[o.Id].Administration || permissions[o.Id].Edit)
                            )
                        }).OrderBy(o => o.Name).ToList();
                    }
                    else
                        result.IsReadonly = (permissions[-3].Administration || permissions[-3].Edit);
                    List<Language> languages = Manager.GetAllLanguages().ToList();

                    result.Translations = new List<dtoTranslation>();
                    result.Translations.Add(new dtoTranslation() { IdLanguage = 0, LanguageName = dLanguageName, LanguageCode = dLanguageCode, Title = tag.DefaultTranslation.Title, Description = tag.DefaultTranslation.Description });
                    result.Translations.AddRange(languages.OrderByDescending(l => l.isDefault).ThenBy(l => l.Name).Select(
                        l => new dtoTranslation() { IdLanguage = l.Id, LanguageName = l.Name, LanguageCode = l.ShortCode, Title = tag.GetTranslation(l.Id).Title, Description = tag.GetTranslation(l.Id).Description }).ToList());
                }
                return result;
            }
            private void SaveTagTranslation(litePerson person, TagItem tag, dtoTag dto, List<dtoTranslation> dtoTranslations)
            {
                List<TagTranslation> translations = null;
                if (tag.Translations == null || !tag.Translations.Any())
                {
                    translations = dtoTranslations.Where(t => t.IdLanguage > 0).Select(t => new TagTranslation()
                                {
                                        Tag= tag,
                                        IdLanguage= t.IdLanguage,
                                        LanguageCode= t.LanguageCode,
                                        LanguageName = t.LanguageName,
                                        Translation = new DomainModel.Languages.TitleDescriptionObjectTranslation(){
                                            Title=String.IsNullOrEmpty(t.Title) ? "" : t.Title,
                                            Description= String.IsNullOrEmpty(t.Description) ? "" : t.Description
                                        }
                                }).ToList();
                    translations.ForEach(t=> t.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                }
                else
                {
                    foreach (TagTranslation t in tag.Translations)
                    {
                        dtoTranslation translation = dtoTranslations.Where(tr => tr.IdLanguage == t.IdLanguage).FirstOrDefault();
                        if (translation!=null){
                            t.Translation.Title = String.IsNullOrEmpty(translation.Title) ? "" : translation.Title;
                            t.Translation.Description = String.IsNullOrEmpty(translation.Description) ? "" : translation.Description;
                            t.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            t.Deleted=  BaseStatusDeleted.None;
                        }
                        else
                            t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    translations = new List<TagTranslation>();
                    foreach (dtoTranslation t in dtoTranslations.Where(tr => tr.IdLanguage > 0 && !tag.Translations.Where(tt => tt.IdLanguage == tr.IdLanguage).Any()))
                    {
                        TagTranslation translation = new TagTranslation();
                        translation.Translation.Title = String.IsNullOrEmpty(t.Title) ? "" : t.Title;
                        translation.Translation.Description = String.IsNullOrEmpty(t.Description) ? "" : t.Description;
                        translation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        translation.Tag = tag;
                        translation.IdLanguage = t.IdLanguage;
                        translation.LanguageCode = t.LanguageCode;
                        translation.LanguageName = t.LanguageName;
                        translations.Add(translation);
                    }
                }
                if (translations != null)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Manager.SaveOrUpdateList(translations);
                        //if (isNew)
                        //    tag.Translations = translations;
                        //else
                        translations.ForEach(t => tag.Translations.Add(t));
                        Manager.SaveOrUpdate(tag);
                        Manager.Commit();
                        SaveTile(person, tag, dto);
                    }
                    catch (TagException tEx)
                    {
                        throw tEx;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        throw new TagException(ErrorMessageType.SavingTranslations, dto);
                    }
                }
             }
            private void SaveTile(litePerson person, TagItem tag, dtoTag dto)
            {
                try
                {
                    Tile nTile = tag.MyTile;
                   
                    List<TileTranslation> tileTranslations = new List<TileTranslation>();
                    if (nTile == null || nTile.Id==0)
                    {
                        nTile = new lm.Comol.Core.Dashboard.Domain.Tile();
                        nTile.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        nTile.AutoNavigateUrl = true;
                        nTile.Status= tag.Status;
                        switch (tag.Type) { 
                            case TagType.Community:
                                nTile.Type = TileType.CommunityTag;
                                break;
                            case TagType.Module:
                                nTile.Type = TileType.Module;
                                break;
                        }
                        nTile.DefaultTranslation = tag.DefaultTranslation;
                        Manager.BeginTransaction();
                        Manager.SaveOrUpdate(nTile);
                        tag.MyTile = nTile;
                        TileTagAssociation tAssociation = new TileTagAssociation();
                        tAssociation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        tAssociation.Tag = tag;
                        tAssociation.Tile = nTile;
                        Manager.SaveOrUpdate(tAssociation);
                        nTile.Tags.Add(tAssociation);
                        Manager.SaveOrUpdate(nTile);
                        Manager.Commit();
                        List<DashboardSettings> dSettings = (from s in Manager.GetIQ<DashboardSettings>()
                                                            where s.Deleted == BaseStatusDeleted.None
                                                            && s.Type== DashboardType.Portal 
                                                            select s).ToList();
                        List<DashboardTileAssignment> assignments = new List<DashboardTileAssignment>();
                        foreach (DashboardSettings settings in dSettings)
                        {
                            DashboardTileAssignment assignment = new DashboardTileAssignment();
                            assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            assignment.Dashboard = settings;
                            assignment.Tile = nTile;
                            assignment.DisplayOrder = ServiceDashboard.GetTileDisplayOrder(0, settings, nTile.Type) +1;
                            assignment.Status = nTile.Status;
                            assignments.Add(assignment);
                        }

                        if (assignments.Any())
                        {
                            Manager.BeginTransaction();
                            Manager.SaveOrUpdateList(assignments);
                            Manager.Commit();
                        }
                    }
                    else
                    {
                        nTile.DefaultTranslation = tag.DefaultTranslation;
                        foreach (TileTranslation t in nTile.Translations)
                        {
                            TagTranslation tTranslation = tag.Translations.Where(tg => tg.IdLanguage == t.IdLanguage && tg.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                            if (tTranslation != null)
                            {
                                t.Translation.Title = tTranslation.Translation.Title;
                                t.Translation.Description = tTranslation.Translation.Description;
                                t.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                t.Deleted = BaseStatusDeleted.None;
                            }
                            else
                                t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        Manager.BeginTransaction();
                        if (nTile.Translations.Any())
                            Manager.SaveOrUpdateList(nTile.Translations);
                        Manager.Commit();
                    }

                    foreach (TagTranslation t in tag.Translations.Where(tr => tr.Deleted == BaseStatusDeleted.None && tr.IdLanguage > 0 && !tag.MyTile.Translations.Where(tt => tt.IdLanguage == tr.IdLanguage).Any()))
                    {
                        TileTranslation translation = new TileTranslation();
                        translation.Translation.Title = t.Translation.Title;
                        translation.Translation.Description = t.Translation.Description;
                        translation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        translation.Tile = nTile;
                        translation.IdLanguage = t.IdLanguage;
                        translation.LanguageCode = t.LanguageCode;
                        translation.LanguageName = t.LanguageName;
                        tileTranslations.Add(translation);
                    }
                    Manager.BeginTransaction();
                    if (tileTranslations.Any())
                    {
                        Manager.SaveOrUpdateList(tileTranslations);
                        tileTranslations.ForEach(t => nTile.Translations.Add(t));
                    }
                    Manager.SaveOrUpdate(nTile);
                    Manager.SaveOrUpdate(tag);
                    Manager.Commit();
                }
                catch (TagException tEx)
                {
                    throw tEx;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    throw new TagException(ErrorMessageType.SavingTileTranslations, dto);
                }
            }
            private void SaveOrganizationAssociations(litePerson person, TagItem tag, dtoTag dto, List<FilterListItem> dtoOrganizations, Int32 dOrganization)
            {
                List<OrganizationAvailability> organizations = null;
                if (tag.Organizations == null || !tag.Organizations.Any())
                {
                    organizations = dtoOrganizations.Where(o=> o.Checked).Select(t => new OrganizationAvailability()
                                {
                                    Tag = tag,
                                    IdOrganization= (Int32)t.Id,
                                    IsDefault = t.Disabled && (t.Id==dOrganization)
                                }).ToList();
                    organizations.ForEach(t => t.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                }
                else
                {
                    foreach (OrganizationAvailability o in tag.Organizations)
                    {
                        FilterListItem oItem = dtoOrganizations.Where(tr => (Int32)tr.Id == o.IdOrganization).FirstOrDefault();
                        if (oItem != null)
                        {
                            if (o.Deleted != BaseStatusDeleted.None && oItem.Checked && !oItem.Disabled)
                            {
                                o.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                o.Deleted = BaseStatusDeleted.None;
                            }
                            else if (o.Deleted == BaseStatusDeleted.None && !oItem.Checked && !oItem.Disabled)
                                o.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        else if (!o.IsDefault)
                            o.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    organizations = new List<OrganizationAvailability>();
                    foreach (FilterListItem t in dtoOrganizations.Where(o => o.Checked && !o.Disabled && !tag.Organizations.Where(oo => oo.IdOrganization == (Int32)o.Id).Any()))
                    {
                        OrganizationAvailability oAvailability = new OrganizationAvailability();
                        oAvailability.Tag = tag;
                        oAvailability.IdOrganization = (Int32)t.Id;
                        organizations.Add(oAvailability);
                    }
                }
                if (organizations != null)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Manager.SaveOrUpdateList(organizations);
                        //if (isNew)
                        //    tag.Organizations = organizations;
                        //else
                            organizations.ForEach(t => tag.Organizations.Add(t));
                        Manager.SaveOrUpdate(tag);
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        throw new TagException(ErrorMessageType.SavingTranslations, dto);
                    }
                }
             }
            private void SaveCommunityTypesAssociations(litePerson person, TagItem tag, dtoTag dto, List<FilterListItem> dtoCommunityTypes)
            {
                List<Int32> idToRemove = null;
                if (tag.CommunityTypes == null || !tag.CommunityTypes.Any())
                    idToRemove = new List<int>();
                else
                    idToRemove = tag.CommunityTypes.Where(t => !dtoCommunityTypes.Where(dt => (Int32)dt.Id == t).Any()).ToList();
                    
                foreach (Int32 id in idToRemove)
                {
                    tag.CommunityTypes.Remove(id);
                }

                foreach (FilterListItem item in dtoCommunityTypes.Where(t => !tag.CommunityTypes.Contains((Int32)t.Id)))
                {
                    tag.CommunityTypes.Add((Int32)item.Id);
                }

                try
                {
                    Manager.BeginTransaction();
                    tag.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    Manager.SaveOrUpdate(tag);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    throw new TagException(ErrorMessageType.SavingCommunityTypes, dto);
                }
            }

            public dtoTagApply GetDtoTagApply(long idTag, Int32 idLanguage, String dLanguageName, String dLanguageCode)
            {
                dtoTagApply result = null;
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                liteTagItem tag = Manager.Get<liteTagItem>(idTag);
                if (tag != null && person != null)
                {
                    Dictionary<Int32, ModuleTags> permissions = GetPermissions(person, true);
                    result = new dtoTagApply();
                    result.IdTag = idTag;
                    List<dtoTranslatedCommunityType> tCommunities = Manager.GetTranslatedItem < dtoTranslatedCommunityType>(UC.Language.Id);

                    result.CommunityTypes = tCommunities.Select(t => new FilterListItem() { Id = t.Id, Name = t.Name, Disabled = result.IsReadonly, Checked = (tag.CommunityTypes != null && tag.CommunityTypes.Contains(t.Id)) }).ToList();
                    //result.SelectedCommunityTypes = result.CommunityTypes.Where(s => s.Checked).ToList();
                    result.AllCommunityTypes = !result.CommunityTypes.Where(c => c.Checked).Any();
                    result.OnlyCommunityWithoutTag = true;
                    result.Name = new dtoTranslation(tag.GetTranslation(idLanguage, Manager.GetDefaultLanguage().Id, dLanguageName, dLanguageCode));

                    Int32 dOrganization = ((tag.IsSystem) ? -3 : tag.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None && o.IsDefault).Select(o => o.IdOrganization).FirstOrDefault());

                    result.IsReadonly = !(permissions[dOrganization].List || permissions[dOrganization].Administration || permissions[dOrganization].Edit);
                }
                return result;
            }
            public dtoTagApply ApplyTagTo(dtoTagApply options)
            {
                dtoTagApply result = null;
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                TagItem tag = Manager.Get<TagItem>(options.IdTag);
                if (tag != null && person != null)
                {
                    if (tag.Status != AvailableStatus.Available)
                    {
                        tag.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(tag);
                    }
                    List<Organization> organizations = ServiceCommunityManagement.GetAvailableOrganizations(person.Id, SearchCommunityFor.CommunityManagement);
                    if (!tag.IsSystem)
                        organizations = organizations.Where(o=> tag.Organizations.Where(to=> to.Deleted== BaseStatusDeleted.None && to.IdOrganization == o.Id).Any()).ToList();

                    dtoTreeCommunityNode rootNode = ServiceCommunityManagement.GetAllCommunitiesTree(person);
                    List<Int32> idCommunities = new List<Int32>();
                    foreach(dtoTreeCommunityNode node in rootNode.Nodes.Where(n=> organizations.Where(o=> o.Id==n.IdOrganization).Any())){
                        if (options.AllCommunityTypes)
                        {
                            idCommunities.Add(node.Id);
                            idCommunities.AddRange(node.GetAllNodes().Select(n => n.Id).Distinct().ToList());
                        }
                        else
                        {
                            if (options.CommunityTypes.Where(t => t.Checked && t.Id == node.IdCommunityType).Any())
                                idCommunities.Add(node.Id);
                            idCommunities.AddRange(node.GetAllNodes().Where(n => options.CommunityTypes.Where(t => t.Checked && t.Id == n.IdCommunityType).Any()).Select(n => n.Id).Distinct().ToList());
                        }
                    }
                    //if (options.OnlyCommunityWithoutThisTag)
                    //{
                        List<Int32> idTagCommunities = tag.CommunityAssignments.Select(c => c.IdCommunity).ToList();
                        idCommunities = idCommunities.Except(idTagCommunities).Distinct().ToList();
                    //}
                    if (options.OnlyCommunityWithoutTag)
                    {
                        List<Int32> idTagsCommunities = (from t in Manager.GetIQ<liteCommunityTag>() where t.Deleted == BaseStatusDeleted.None select t.IdCommunity).Distinct().ToList();
                        idCommunities = idCommunities.Except(idTagsCommunities).Distinct().ToList();
                    }
                    List<CommunityTag> links = null;
                    if (idCommunities.Count <= maxItemsForQuery)
                    {
                        links = (from t in Manager.GetIQ<CommunityTag>()
                                 where t.Deleted == BaseStatusDeleted.Manual && t.Tag != null && t.Tag.Id == options.IdTag && idCommunities.Contains(t.IdCommunity)
                                 select t).ToList();

                    }
                    else{
                         links = (from t in Manager.GetIQ<CommunityTag>()
                                  where t.Deleted == BaseStatusDeleted.Manual && t.Tag != null && t.Tag.Id == options.IdTag 
                                       select t).ToList().Where(t=> idCommunities.Contains(t.IdCommunity)).ToList();
                                      
                    }
                    if (links!=null && links.Any()){
                        foreach(CommunityTag link in links){
                            link.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        Manager.SaveOrUpdateList(links);
                        idCommunities = idCommunities.Except(links.Select(l => l.IdCommunity).ToList()).ToList();
                    }
                    if (idCommunities.Any())
                    {
                        links = new List<CommunityTag>();
                        foreach (Int32 idCommunity in idCommunities)
                        {
                            CommunityTag link = new CommunityTag();
                            link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            link.IdCommunity = idCommunity;
                            link.Tag = tag;
                            links.Add(link);
                        }
                        Manager.SaveOrUpdateList(links);
                        links.ForEach(l => tag.CommunityAssignments.Add(l));
                        Manager.SaveOrUpdate(tag);
                    }
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllCommunityTags);
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllUserCommunitiesTags);
                    result = options;
                }
                return result;
            }

            public List<TagItem> BulkInsert(TagType type,Boolean isSystem, Int32 idCommunity, List<Dictionary<int, string>> itemsToInsert, List<Dictionary<int, string>> notInserted)
            {
                List<TagItem> tags = new  List<TagItem>();
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (person != null && (person.TypeID != (Int32)UserTypeStandard.Guest || person.TypeID != (Int32)UserTypeStandard.PublicUser))
                    {
                        Int32 idOrganization = (isSystem) ? 0 : Manager.GetIdOrganizationFromCommunity(idCommunity);
                        List<Language> languages = Manager.GetAllLanguages().ToList();
                        foreach (Dictionary<int, string> item in itemsToInsert)
                        {
                            TagItem tag = new TagItem();
                            tag.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            tag.Type = type;
                            tag.Status = AvailableStatus.Available;
                            tag.DefaultTranslation.Title = item[0];
                            tag.IsSystem = isSystem;
                            if (HasDefaultDuplicate(0, item[0], type, idOrganization))
                                notInserted.Add(item);
                            else
                            {
                                Manager.BeginTransaction();

                                if (isSystem)
                                    Manager.SaveOrUpdate(tag);
                                else if (idOrganization > 0)
                                {
                                    Manager.SaveOrUpdate(tag);
                                    OrganizationAvailability oAvailability = new OrganizationAvailability();
                                    oAvailability.Tag = tag;
                                    oAvailability.IdOrganization = idOrganization;
                                    oAvailability.IsDefault = true;
                                    Manager.SaveOrUpdate(oAvailability);
                                    tag.Organizations.Add(oAvailability);
                                }
                                Manager.Commit();
                                if (isSystem || idOrganization > 0)
                                {
                                    tags.Add(tag);
                                    List<TagTranslation> translations = item.Where(i => i.Key != 0).Select(i => new TagTranslation()
                                    {
                                        IdLanguage = i.Key,
                                        Translation = new DomainModel.Languages.TitleDescriptionObjectTranslation() { Title = i.Value, Description = "", ShortTitle = "" },
                                        Tag = tag
                                    }).ToList();
                                    foreach (TagTranslation t in translations)
                                    {
                                       t.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                       t.LanguageCode = languages.Where(l => l.Id == t.IdLanguage).Select(l => l.Code).FirstOrDefault();
                                       t.LanguageName = languages.Where(l => l.Id == t.IdLanguage).Select(l => l.Name).FirstOrDefault();
                                    }
                                    Manager.BeginTransaction();
                                    Manager.SaveOrUpdateList(translations);
                                    translations.ForEach(t => tag.Translations.Add(t));
                                    Manager.SaveOrUpdate(tag);
                                    Manager.Commit();
                                }
                            }
                        }
                        if (tags.Any())
                        {
                            foreach (TagItem tag in tags)
                            {
                                SaveTile(person, tag, null);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
               // notInserted = toInsert;
                return tags;
            }

        #endregion
        #region "Manage List"
            public List<dtoTagItem> GetTags(Int32 idPerson, TagType type, dtoFilters filters, Int32 idCommunity, String unknownUser)
            {
                List<dtoTagItem> items = null;
                try
                {
                    litePerson person = Manager.GetLitePerson(idPerson);
                    if (person != null)
                    {
                        List<Language> languages = Manager.GetAllLanguages().ToList();

                        Dictionary<Int32, ModuleTags> permissions = GetPermissions(person, filters.ForOrganization);
                        IEnumerable<liteTagItem> tags = GetAvailableTags(type, filters.ForOrganization, filters.FromRecycleBin, permissions).Where(t => (filters.IdCreatedBy == -1 || filters.IdCreatedBy == t.IdCreatedBy)
                                        && (filters.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Any || filters.Status == t.Status)
                                        && ! t.IsEmptyForLanguage(filters.IdSelectedLanguage)
                                        && ((filters.IdOrganization==-3 && t.IsSystem) || (filters.IdOrganization==-1 && !t.IsSystem)
                                                ||(filters.IdOrganization>0 && t.Organizations.Where(o=> o.IdOrganization==filters.IdOrganization && o.Deleted== BaseStatusDeleted.None).Any())));

                        items = tags.Select(t => new dtoTagItem(t, permissions,filters.IdSelectedLanguage,languages, idPerson)).Where(t=> t.Translation !=null && !String.IsNullOrEmpty(t.Translation.Title)).ToList();
                        List<Int32> idOwners = items.Select(t => t.IdCreatedBy).Distinct().ToList();
                        idOwners.AddRange(items.Select(t => t.IdModifiedBy).Distinct().ToList());
                        List<FilterListItem> owners = GetTagOwners(idOwners.Distinct().ToList());

                        foreach (var item in items.Where(i=> owners.Where(o=> o.Id== i.IdCreatedBy && !String.IsNullOrEmpty(o.Name)).Any()))
                        {
                            item.CreatedBy = owners.Where(o=> o.Id== item.IdCreatedBy).Select(o=> o.Name).FirstOrDefault();
                        }
                        foreach (var item in items.Where(i=> String.IsNullOrEmpty(i.CreatedBy)))
                        {
                            item.CreatedBy = unknownUser+item.IdCreatedBy.ToString();
                        }
                        foreach (var item in items.Where(i=> owners.Where(o=> o.Id== i.IdModifiedBy && !String.IsNullOrEmpty(o.Name)).Any()))
                        {
                            item.ModifiedBy = owners.Where(o=> o.Id== item.IdModifiedBy).Select(o=> o.Name).FirstOrDefault();
                        }
                        foreach (var item in items.Where(i=> String.IsNullOrEmpty(i.ModifiedBy)))
                        {
                            item.ModifiedBy = unknownUser+item.IdModifiedBy.ToString();
                        }
                       
                        var query = (from t in items select t);
                        if (!String.IsNullOrEmpty(filters.Name) && !String.IsNullOrEmpty(filters.Name.Trim()))
                            query = query.Where(i => i.Translation.Title.ToLower().Contains(filters.Name.ToLower()));
                        if (filters.StartWith != "#" && filters.StartWith != "")
                            query = query.Where(n => !String.IsNullOrEmpty(n.GetFirstLetter()) && n.GetFirstLetter() == filters.StartWith.ToLower());
                        else if (filters.StartWith == "#")
                            query = query.Where(n => DefaultOtherChars().Contains(n.GetFirstLetter()));

                        
                        switch (filters.OrderBy)
                        {
                            case OrderTagsBy.Name:
                                query = (filters.Ascending) ? query.OrderBy(s => s.Translation.Title) : query.OrderByDescending(s => s.Translation.Title);
                                break;
                            case OrderTagsBy.UsedBy:
                                query = (filters.Ascending) ? query.OrderBy(s => s.CommunityAssignments) : query.OrderByDescending(s => s.CommunityAssignments);
                                break;
                            case OrderTagsBy.CreatedOn:
                                query = (filters.Ascending) ? query.OrderBy(s => s.CreatedOn) : query.OrderByDescending(s => s.CreatedOn);
                                break;
                            case OrderTagsBy.ModifiedOn:
                                query = (filters.Ascending) ? query.OrderBy(s => s.ModifiedOn) : query.OrderByDescending(s => s.ModifiedOn);
                                break;
                        }
                        return query.ToList();
                    }
                }
                catch (Exception ex)
                {

                }
                return items;
            }
            public List<dtoTagSelectItem> GetTags(TagType type, Int32 idCommunity, List<Int32> idOrganizations, Int32 idCommunityType = -1, List<long> selectedTags = null)
            {
                List<dtoTagSelectItem> items = null;
                try
                {
                    List<long> idTags = new List<long>();
                    
                    if (selectedTags != null)
                        idTags.AddRange(selectedTags);
                    else
                    {
                        switch (type)
                        {
                            case TagType.Community:
                                if (idCommunity > 0)
                                    idTags = (from t in Manager.GetIQ<liteCommunityTagForEdit>()
                                              where t.Deleted == BaseStatusDeleted.None && t.IdCommunity == idCommunity
                                              select t.IdTag).ToList();
                                else if (idCommunityType > -1)
                                    idTags = (from t in Manager.GetIQ<liteTagItem>()
                                              where t.Deleted == BaseStatusDeleted.None && t.Type == type && t.Status == AvailableStatus.Available
                                              && (t.IsDefault || t.CommunityTypes != null)
                                              select t).ToList().Where(t => t.IsDefault || (t.CommunityTypes != null && t.CommunityTypes.Any() && t.CommunityTypes.Contains(idCommunityType))).Select(t => t.Id).ToList();
                                break;
                            default:
                                idTags = new List<long>();
                                break;
                        }
                    }
                    items = GetAvailableTags(type, idOrganizations).Select(t => new dtoTagSelectItem(t, UC.Language.Id, Manager.GetDefaultIdLanguage(), idTags)).ToList();
                  
                    return items.Where(t=> t.IsSelected || t.Status== AvailableStatus.Available).OrderBy(i=>i.Name).ToList();
                }
                catch (Exception ex)
                {

                }
                return items;
            }

            private List<liteTagItem> GetAvailableTags(TagType type, List<Int32> idOrganizations)
            {
                List<liteTagItem> tags = (from t in Manager.GetIQ<liteTagItem>()
                                          where t.Deleted == BaseStatusDeleted.None && t.Type == type && t.Status != AvailableStatus.Draft
                                          select t).ToList();
                return tags.Where(t => 
                                        t.IsSystem || (!t.IsSystem && t.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None && idOrganizations.Contains(o.IdOrganization)).Any())).ToList();

            }

            public List<liteCommunityTagForEdit> ApplyTagsToCommunities(List<Int32> idCommunities, List<long> selectedTags)
            {
                List<liteCommunityTagForEdit> items = null;
                try
                { 
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (person != null && (person.TypeID != (Int32)UserTypeStandard.Guest || person.TypeID != (Int32)UserTypeStandard.PublicUser))
                    {
                        Boolean toUpdate = false;
                        DateTime now = DateTime.Now;
                        var query = (from c in Manager.GetIQ<liteCommunityTagForEdit>() where c.Deleted == BaseStatusDeleted.None select c);
                        List<liteCommunityTagForEdit> cItems = (idCommunities.Count > maxItemsForQuery) ? query.ToList().Where(c => idCommunities.Contains(c.IdCommunity)).ToList() : query.Where(c => idCommunities.Contains(c.IdCommunity)).ToList();
                        foreach (liteCommunityTagForEdit a in cItems.Where(c =>  c.Deleted == BaseStatusDeleted.None && !selectedTags.Contains(c.IdTag)))
                        {
                            a.SetDeleteMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, now);
                            toUpdate = true;
                        }
                        foreach (liteCommunityTagForEdit a in cItems.Where(c => c.Deleted !=  BaseStatusDeleted.None && selectedTags.Contains(c.IdTag)))
                        {
                            a.RecoverMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, now);
                            toUpdate = true;
                        }
                        items = new List<liteCommunityTagForEdit>();
                        foreach (Int32 idCommunity in idCommunities.Where(i => selectedTags.Where(t => !cItems.Where(c => c.IdCommunity == i && c.IdTag == t).Any()).Any()))
                        {
                            foreach (long idTag in selectedTags.Where(t => !cItems.Where(c => c.IdCommunity == idCommunity && c.IdTag == t).Any()))
                            {
                                liteCommunityTagForEdit cTag = new liteCommunityTagForEdit();
                                cTag.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, now);
                                cTag.IdCommunity = idCommunity;
                                cTag.IdTag = idTag;
                                items.Add(cTag);
                            }
                        }
                        if (items.Any()) {
                            Manager.BeginTransaction();
                            Manager.SaveOrUpdateList(items);
                            Manager.Commit();
                            toUpdate = true;
                        }
                        if (toUpdate)
                        {
                            if (cItems.Any())
                            {
                                Manager.BeginTransaction();
                                Manager.SaveOrUpdateList(cItems);
                                Manager.Commit();
                            }
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllCommunityTags);
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllUserCommunitiesTags);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return items;
            }
            public Boolean ApplyTagsToCommunities(List<dtoBulkCommunityForTags> items)
            {
                Boolean result = false;
                try
                { 
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (person != null && (person.TypeID != (Int32)UserTypeStandard.Guest || person.TypeID != (Int32)UserTypeStandard.PublicUser))
                    {
                        Boolean toUpdate = false;
                        DateTime now = DateTime.Now;
                        var query = (from c in Manager.GetIQ<liteCommunityTagForEdit>() where c.Deleted == BaseStatusDeleted.None select c);
                        List<Int32> idCommunities = items.Select(i => i.IdCommunity).Distinct().ToList();
                        List<liteCommunityTagForEdit> tagAssignments = (idCommunities.Count > maxItemsForQuery) ? query.ToList().Where(c => idCommunities.Contains(c.IdCommunity)).ToList() : query.Where(c => idCommunities.Contains(c.IdCommunity)).ToList();
                        foreach (liteCommunityTagForEdit a in tagAssignments.Where(c => c.Deleted == BaseStatusDeleted.None && items.Where(i=> i.IdCommunity== c.IdCommunity && !i.IdSelectedTags.Contains(c.IdTag)).Any()))
                        {
                            a.SetDeleteMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, now);
                            toUpdate = true;
                        }
                        foreach (liteCommunityTagForEdit a in tagAssignments.Where(c => c.Deleted != BaseStatusDeleted.None && items.Where(i => i.IdCommunity == c.IdCommunity && i.IdSelectedTags.Contains(c.IdTag)).Any()))
                        {
                            a.RecoverMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, now);
                            toUpdate = true;
                        }
                        List<liteCommunityTagForEdit> toAdd = new List<liteCommunityTagForEdit>();
                        foreach (Int32 idCommunity in idCommunities.Where(i => items.Where(t => t.IdCommunity==i && t.IdSelectedTags.Where(s=> !tagAssignments.Where(c => c.IdCommunity == i && c.IdTag ==s).Any()).Any()).Any()))
                        {
                            List<long> selectedTags = items.Where(i => i.IdCommunity == idCommunity).Select(i => i.IdSelectedTags).FirstOrDefault();
                            foreach (long idTag in selectedTags.Where(t => !tagAssignments.Where(c => c.IdCommunity == idCommunity && c.IdTag == t).Any()))
                            {
                                liteCommunityTagForEdit cTag = new liteCommunityTagForEdit();
                                cTag.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, now);
                                cTag.IdCommunity = idCommunity;
                                cTag.IdTag = idTag;
                                toAdd.Add(cTag);
                            }
                        }
                        if (items.Any()) {
                            Manager.BeginTransaction();
                            Manager.SaveOrUpdateList(toAdd);
                            if (toUpdate)
                                Manager.SaveOrUpdateList(tagAssignments);
                            Manager.Commit();
                            toUpdate = true;
                        }
                        else if (toUpdate)
                        {
                            Manager.BeginTransaction();
                            Manager.SaveOrUpdateList(tagAssignments);
                            Manager.Commit();
                        }
                        result = true;
                        if (toUpdate)
                        {
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllCommunityTags);
                            CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllUserCommunitiesTags);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
        #endregion

        #region "Manage Filters"
            public List<Filter> GetDefaultFilters(TagType type, Int32 idPerson, Int32 idCommunity, Int32 idLanguage, Boolean forOrganization, Boolean fromRecycleBin, String searchBy = "", Dictionary<searchFilterType, long> defaultValues = null)
            {
                litePerson p = Manager.GetLitePerson(idPerson);
                if (p != null)
                {
                    ModuleTags permissions = null;
                    if (forOrganization)
                        permissions = GetPermission(idCommunity);
                    else
                        permissions = ModuleTags.CreatePortalmodule(p.TypeID);


                    if (defaultValues == null)
                    {
                        defaultValues = (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none select n).ToDictionary(t => t, t => (long)-1);
                        defaultValues[searchFilterType.organization] = -2;
                        defaultValues[searchFilterType.name] = (!String.IsNullOrEmpty(searchBy) ? 0 : -1);
                    }

                    if (permissions.Administration || permissions.Edit || permissions.List)
                        return GetFilters(type, p, idCommunity, idLanguage, forOrganization, fromRecycleBin, searchBy, defaultValues);
                }
                return new List<Filter>();
            }
            public List<Filter> ChangeFilters(TagType type, Int32 idPerson, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter filter, Int32 idCommunity, Int32 idLanguage, Boolean forOrganization, Boolean fromRecycleBin)
            {
                litePerson p = Manager.GetLitePerson(idPerson);
                if (p != null)
                    return ChangeFilters(type,p, filters, filter, idCommunity, idLanguage, forOrganization, fromRecycleBin);
                else
                    return filters;
            }
            public List<Filter> ChangeFilters(TagType tagType, litePerson person, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter filter, Int32 idCommunity, Int32 idLanguage, Boolean forOrganization, Boolean fromRecycleBin)
            {
                if (filter != null)
                {
                    Dictionary<searchFilterType, long> dValues = GetDefaultValues(filters);
                    searchFilterType type = lm.Comol.Core.DomainModel.Helpers.EnumParser<searchFilterType>.GetByString(filter.Name, searchFilterType.none);
                    switch (type)
                    {
                        case searchFilterType.organization:
                            return GetFilters(tagType, person, idCommunity, idLanguage, forOrganization, fromRecycleBin, filters.Where(f => f.Name == searchFilterType.name.ToString()).Select(f => f.Value).FirstOrDefault(), dValues);
                        default:
                            Dictionary<Int32, ModuleTags> permissions = GetPermissions(person, forOrganization);
                            List<liteTagItem> tags = GetAvailableTags(tagType, forOrganization, fromRecycleBin, permissions);
                            var query = (from t in tags select t);
                            if (dValues[searchFilterType.organization] > 0)
                                query = query.Where(t => !t.IsSystem && t.Organizations != null && t.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None && o.IdOrganization == dValues[searchFilterType.organization]).Any());
                            else if (dValues[searchFilterType.organization] == -1)
                                query = query.Where(t => !t.IsSystem);
                            else
                                query = query.Where(t => t.IsSystem);
   
                            switch (type)
                            {
                                case searchFilterType.createdby:
                                    filters = filters.Where(f => f.Name == searchFilterType.organization.ToString() || f.Name == searchFilterType.name.ToString()).ToList();
                                    AddCreatorFilters(query, dValues, filters, filter, idLanguage);
                                    break;
                                case searchFilterType.status:

                                    query = query.Where(t => (dValues[searchFilterType.createdby] == -1 || (dValues[searchFilterType.createdby] == t.IdCreatedBy)));

                                    filters = filters.Where(f => f.Name != searchFilterType.letters.ToString()).ToList();
                                    filters.Add(GetLettersFilter(query, idLanguage, dValues));
                                    break;
            //                    case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.responsible:
            //                        filters = filters.Where(f => f.Name == searchFilterType.organization.ToString() || f.Name == searchFilterType.name.ToString() || f.Name == searchFilterType.communitytype.ToString()).ToList();

            //                        query = query.Where(n => dValues[searchFilterType.communitytype] == -1 || (dValues[searchFilterType.communitytype] == n.IdCommunityType));

            //                        AddResponsibleFilters(nodes, dValues, filters, filter, selectedTags);
            //                        if (filter != null && !(dValues[searchFilterType.communitytype] == (long)CommunityTypeStandard.Degree || dValues[searchFilterType.communitytype] == (long)CommunityTypeStandard.UniversityCourse))
            //                            filter.AutoUpdate = false;

            //                        break;
            //                    case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.year:
            //                        filters = filters.Where(f => f.Name != searchFilterType.coursetime.ToString()).ToList();
            //                        query = query.Where(n => dValues[searchFilterType.communitytype] == (long)CommunityTypeStandard.UniversityCourse);
            //                        query = query.Where(n => dValues[searchFilterType.responsible] == -1 || dValues[searchFilterType.responsible] == n.IdResponsible);
            //                        query = query.Where(n => dValues[searchFilterType.year] == -1 || dValues[searchFilterType.year] == n.Year);
            //                        query = query.Where(n => dValues[searchFilterType.status] == -1 || dValues[searchFilterType.status] == (long)n.Status);

            //                        filters.Add(GetCourseTimeFilter(query, dValues));
            //                        break;
                            }
                            break;
                    }
                }
                return filters;
            }

            private List<Filter> GetFilters(TagType type, litePerson person, Int32 idCommunity, Int32 idLanguage, Boolean forOrganization, Boolean fromRecycleBin, String searchBy = "", Dictionary<searchFilterType, long> defaultValues = null)
            {
                List<lm.Comol.Core.DomainModel.Filters.Filter> filters = new List<lm.Comol.Core.DomainModel.Filters.Filter>();

                Dictionary<Int32, ModuleTags> permissions = GetPermissions(person, forOrganization);
                List<liteTagItem> tags = GetAvailableTags(type, forOrganization, fromRecycleBin, permissions);
                lm.Comol.Core.DomainModel.Filters.Filter organization = null;

                organization = GetOrganizationFilter(person, tags, forOrganization,permissions, defaultValues, idCommunity);
                if (organization != null)
                    AddOrganizationFilters(tags, defaultValues, filters, organization, idLanguage);
                else
                {
                    var query = (from t in tags select t);
                    Filter creator = GetCreatorOwnerFilter(query, defaultValues);
                    if (creator != null && creator.Values.Count>1)
                        AddCreatorFilters(query, defaultValues, filters, creator, idLanguage);
                    else
                        AddFilterStatusAndLetters(query, defaultValues, filters, idLanguage);
                }

                filters.Add(GetSimpleSearchFilter(searchBy));
                return filters;
            }

       
            private Dictionary<Int32, ModuleTags> GetPermissions(litePerson person, Boolean forOrganization)
            {
                Dictionary<Int32, ModuleTags> permissions = new Dictionary<Int32, ModuleTags>();
                List<Organization> organizations = ServiceCommunityManagement.GetAvailableOrganizations(person.Id, SearchCommunityFor.Subscribed);
                permissions.Add(-3, ModuleTags.CreatePortalmodule(person.TypeID));

                if (organizations != null && organizations.Any())
                {
                    if (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator)
                        organizations.ForEach(o => permissions.Add(o.Id, permissions[-3]));
                    else
                    {
                        var communities = (from c in Manager.GetIQ<liteCommunityInfo>()
                                           where c.IdFather == 0
                                               && organizations.Where(o => o.Id == c.IdOrganization).Any()
                                           select new { Id = c.Id, IdOrganization = c.IdOrganization }).ToList();
                        communities.ForEach(c => permissions.Add(c.IdOrganization, GetPermission(c.Id)));
                    }
                }
                return permissions;
            }

            private List<liteTagItem> GetAvailableTags(TagType type, Boolean forOrganization, Boolean fromRecycleBin,Dictionary<Int32, ModuleTags> permissions)
            {
                BaseStatusDeleted deleted = (fromRecycleBin) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                List<liteTagItem> tags = (from t in Manager.GetIQ<liteTagItem>()
                                    where t.Deleted == deleted && t.Type== type select t).ToList();

                List<Int32> idOrganizations = tags.Where(t => !t.IsSystem).SelectMany(t => t.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None)).Select(o => o.IdOrganization).Distinct().ToList();
                idOrganizations = idOrganizations.Where(i => permissions.ContainsKey(i) && (permissions[i].Administration || permissions[i].List || permissions[i].Edit)).ToList();

                return tags.Where(t => (forOrganization 
                                        && !t.IsSystem && t.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None && idOrganizations.Contains(o.IdOrganization)).Any()
                                        )
                                        ||
                                        (
                                        t.IsSystem || (! t.IsSystem && t.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None && idOrganizations.Contains(o.IdOrganization)).Any())
                                        )
                                        
                                        ).ToList();

            }
            private Dictionary<searchFilterType, long> GetDefaultValues(List<lm.Comol.Core.DomainModel.Filters.Filter> filters)
            {
                Dictionary<searchFilterType, long> values = (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none select n).ToDictionary(t => t, t => (long)-1);
                foreach (searchFilterType key in (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none select n))
                {
                    Filter filter = filters.Where(f => f.Name == key.ToString()).FirstOrDefault();
                    if (filter == null)
                        values[key] = -1;
                    else if (filter.FilterType == FilterType.Checkbox)
                    {
                        if (filter.Values.Where(v => v.Id == filter.SelectedId).Any())
                            values[key] = filter.SelectedId;
                        else
                            values[key] = -1;
                    }
                    else
                    {
                        switch (key)
                        {
                            case searchFilterType.name:
                                values[key] = (String.IsNullOrEmpty(filter.Value) ? -1 : 0);
                                break;
                            default:
                                values[key] = (filter.Selected != null) ? filter.Selected.Id : -1;
                                break;
                        }
                    }
                }
                return values;
            }
            private void AddOrganizationFilters(List<liteTagItem> tags, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter organization, Int32 idLanguage)
            {
                var query = (from t in tags select t);
                if (organization.Selected.Id > 0)
                    query = query.Where(t => !t.IsSystem && t.Organizations != null && t.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None && o.IdOrganization == organization.Selected.Id).Any());
                else if (organization.Selected.Id ==-1)
                    query = query.Where(t => !t.IsSystem);
                else
                    query = query.Where(t => t.IsSystem);
                filters.Add(organization);

                Filter creator = GetCreatorOwnerFilter(query, defaultValues);
                if (creator != null && creator.Values.Count > 1)
                    AddCreatorFilters(query, defaultValues, filters, creator, idLanguage);
                else
                    AddFilterStatusAndLetters(query, defaultValues, filters,  idLanguage);
            }
            private void AddCreatorFilters(IEnumerable<liteTagItem> tags, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter creatorFilter, Int32 idLanguage)
            {
                var query = tags.Where(n => defaultValues[searchFilterType.createdby] == -1 || n.IdCreatedBy == defaultValues[searchFilterType.createdby]);
                filters.Add(creatorFilter);
                AddFilterStatusAndLetters(query, defaultValues, filters, idLanguage);

            }
            private void AddFilterStatusAndLetters(IEnumerable<liteTagItem> query, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Int32 idLanguage)
            {
                lm.Comol.Core.DomainModel.Filters.Filter statusFilter = GetStatusFilter(query, defaultValues);
                if (statusFilter.Values.Count > 1)
                {
                    filters.Add(GetStatusFilter(query, defaultValues));
                    query = query.Where(t => (defaultValues[searchFilterType.createdby] == -1 || (defaultValues[searchFilterType.createdby] == t.IdCreatedBy)));
                }
                filters.Add(GetLettersFilter(query, idLanguage, defaultValues));
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetOrganizationFilter(litePerson person, List<liteTagItem> tags, Boolean forOrganization, Dictionary<Int32, ModuleTags> permissions, Dictionary<searchFilterType, long> defaultValues, Int32 idCommunity)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.organization.ToString();
                filter.DisplayOrder = (int)searchFilterType.organization;
                filter.AutoUpdate = true;

                Int32 dOrganization = (idCommunity>0) ? Manager.GetIdOrganizationFromCommunity(idCommunity) : - 1;
                List<Int32> idOrganizations = tags.Where(t => !t.IsSystem).SelectMany(t => t.Organizations.Where(o => o.Deleted == BaseStatusDeleted.None)).Select(o => o.IdOrganization).Distinct().ToList();
                List<Organization> organizations = (from o in Manager.GetIQ<Organization>() select o).ToList().Where(o=>idOrganizations.Contains(o.Id)).ToList();

                filter.Values = organizations.OrderBy(o => o.Name).Select(o => new FilterListItem() { Id = o.Id, Name = o.Name }).ToList();

                if (!forOrganization && permissions.ContainsKey(-3) && (permissions[-3].Administration || permissions[-3].List || permissions[-3].Edit))
                    filter.Values.Insert(0, new FilterListItem() { Id = -3 });
                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.organization]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.organization]).Select(v => v).FirstOrDefault();
                else if (filter.Values.Where(v => v.Id == dOrganization).Any())
                {
                    defaultValues[searchFilterType.organization] = dOrganization;
                    filter.Selected = filter.Values.Where(v => v.Id == dOrganization).FirstOrDefault();
                    filter.Values.Where(v => v.Id == dOrganization).FirstOrDefault().Checked = true;
                }
                else
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.organization] = filter.Selected.Id;
                }
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetTypeFilter(IEnumerable<liteTagItem> tags, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                List<TagType> items = tags.Select(n => n.Type).Distinct().OrderBy(n => n).ToList();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.type.ToString();
                filter.DisplayOrder = (int)searchFilterType.type;
                filter.AutoUpdate = true;
                filter.Values = items.Select(p => new FilterListItem() { Id = (long)p, Name = "" }).ToList();

                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id = -1 });
                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.type]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.type]).FirstOrDefault();
                else if (items.Contains(TagType.Community))
                {
                    filter.Selected = filter.Values.Where(v => v.Id == (long)TagType.Community).FirstOrDefault();
                    defaultValues[searchFilterType.type] = (long)TagType.Community;
                }
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.type] = filter.Selected.Id;
                }
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetCreatorOwnerFilter(IEnumerable<liteTagItem> tags, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.createdby.ToString();
                filter.DisplayOrder = (int)searchFilterType.createdby;
                filter.AutoUpdate = true;
                filter.Values = GetTagOwners(tags.Select(t => t.IdCreatedBy).Distinct().ToList());
                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id = -1 });

                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.createdby]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.createdby]).FirstOrDefault();
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.createdby] = filter.Selected.Id;
                }
                return filter;
            }
            private List<FilterListItem> GetTagOwners(List<Int32> idUsers)
            {
                List<FilterListItem> results = new List<FilterListItem>();
                try
                {
                    List<litePerson> users = null;
                    if (idUsers.Count > maxItemsForQuery)
                        users = (from p in Manager.GetIQ<litePerson>() select p).ToList().Where(p => idUsers.Contains(p.Id)).ToList();
                    else
                        users = (from p in Manager.GetIQ<litePerson>() where idUsers.Contains(p.Id) select p).ToList();
                    results.AddRange(users.Select(p => new FilterListItem() { Id = p.Id, Name = p.SurnameAndName }));
                    if (users == null)
                        results.AddRange(idUsers.Select(p => new FilterListItem() { Id = p, Name = "" }));
                    else if (results.Count != idUsers.Count)
                    {
                        idUsers = idUsers.Where(i => !results.Where(r => r.Id == i).Any()).ToList();
                        results.AddRange(idUsers.Select(p => new FilterListItem() { Id = p, Name = "" }));
                    }
                }
                catch (Exception ex)
                {

                }
                return results.OrderBy(n => n.Name).ToList();
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetStatusFilter(IEnumerable<liteTagItem> tags, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                List<lm.Comol.Core.Dashboard.Domain.AvailableStatus> items = tags.Select(n => n.Status).Distinct().OrderBy(n => n).ToList();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.status.ToString();
                filter.DisplayOrder = (int)searchFilterType.status;
                filter.AutoUpdate = false;
                filter.Values = items.Select(p => new FilterListItem() { Id = (long)p, Name = "" }).ToList();

                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id = -1 });
                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.status]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.status]).FirstOrDefault();
                else if (items.Contains(lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available))
                {
                    filter.Selected = filter.Values.Where(v => v.Id == (long)lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available).FirstOrDefault();
                    defaultValues[searchFilterType.status] = (long)lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available;
                }
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.status] = filter.Selected.Id;
                }
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetSimpleSearchFilter(String searchBy)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Text;
                filter.Name = searchFilterType.name.ToString();
                filter.DisplayOrder = (int)searchFilterType.name;
                filter.AutoUpdate = false;
                filter.Value = searchBy;
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetLettersFilter(IEnumerable<liteTagItem> tags, Int32 idLanguage, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.MaskedRadio;
                filter.Name = searchFilterType.letters.ToString();
                filter.DisplayOrder = (int)searchFilterType.letters;
                filter.AutoUpdate = false;
                filter.Values = GenerateAlphabetItems(tags.Select(n => n.GetFirstLetter(idLanguage)).Where(n => !String.IsNullOrEmpty(n)).OrderBy(n => n).Distinct().ToList(), defaultValues[searchFilterType.letters]);
                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.letters]).Any())
                    filter.SelectedId = defaultValues[searchFilterType.letters];
                else
                {
                    filter.SelectedId = -1;
                    defaultValues[searchFilterType.letters] = -1;
                }
                return filter;
            }
            private List<FilterListItem> GenerateAlphabetItems(List<String> availableWords, long selected)
            {
                Boolean hasOtherChars = false;
                List<AlphabetItem> items = new List<AlphabetItem>();
                List<AlphabetItem> otherChars = new List<AlphabetItem>();

                //if (displayMode.IsFlagSet(AlphabetDisplayMode.commonletters) || displayMode.IsFlagSet(AlphabetDisplayMode.extendedletters))
                items = (from n in Enumerable.Range(97, 26) select new AlphabetItem() { isEnabled = false, Value = Char.ConvertFromUtf32(n), DisplayName = Char.ConvertFromUtf32(n).ToString().ToUpper(), isSelected = false }).ToList();
                //if (displayMode.IsFlagSet(AlphabetDisplayMode.extendedletters))
                //    items.AddRange(GetOtherAlphabetItems(false));
                //if (displayMode.IsFlagSet(AlphabetDisplayMode.addUnmatchLetters))
                //otherChars = GetOtherAlphabetItems(true);
                otherChars = GetOtherAlphabetItems(true);
                foreach (String l in availableWords)
                {
                    if (items.Where(i => i.Value == l).Any())
                        items.Where(i => i.Value == l).FirstOrDefault().isEnabled = true;
                    else if (System.Text.RegularExpressions.Regex.IsMatch(l, @"[^\w\.@-]", System.Text.RegularExpressions.RegexOptions.None))
                    {
                        String upper = "";
                        try
                        {
                            upper = l.ToUpper();
                        }
                        catch (Exception ex)
                        {
                            upper = l;
                        }
                        items.Add(new AlphabetItem() { isEnabled = true, Value = l, DisplayName = upper });
                    }
                    else if (otherChars.Where(i => i.Value == l).Any())
                        items.AddRange(otherChars.Where(i => i.Value == l).ToList());
                    else
                        hasOtherChars = true;
                }

                items = items.OrderBy(i => i.Value).ToList();

                items.Insert(0, new AlphabetItem() { Type = AlphabetItemType.otherChars, isEnabled = hasOtherChars, Value = "-9", DisplayName = "ALL" });
                items.Insert(0, new AlphabetItem() { DisplayAs = AlphabetItem.AlphabetItemDisplayAs.first, isEnabled = true, Type = AlphabetItemType.all, Value = "-1", DisplayName = "#" });
                items.LastOrDefault().DisplayAs = AlphabetItem.AlphabetItemDisplayAs.last;

                switch (selected)
                {
                    case -1:
                        items.Where(i => i.Type == DomainModel.Helpers.AlphabetItemType.all).FirstOrDefault().isSelected = true;
                        break;
                    case -9:
                        items.Where(i => i.Type == DomainModel.Helpers.AlphabetItemType.otherChars).FirstOrDefault().isSelected = true;
                        break;
                    default:
                        var query = items.Where(i => i.Type != DomainModel.Helpers.AlphabetItemType.otherChars && i.Type != DomainModel.Helpers.AlphabetItemType.all
                                && !String.IsNullOrEmpty(i.Value) && (long)i.Value[0] == selected);
                        if (query.Where(i => i.isEnabled).Any())
                            query.Where(i => i.isEnabled).FirstOrDefault().isSelected = true;
                        else if (query.Where(i => !i.isEnabled).Any())
                            items.Where(i => i.Type == DomainModel.Helpers.AlphabetItemType.all).FirstOrDefault().isSelected = true;
                        break;
                }
                return items.Select(i => new FilterListItem() { Disabled = !i.isEnabled, Checked = i.isSelected, Name = i.DisplayName, Id = (i.Type == AlphabetItemType.all || i.Type == AlphabetItemType.otherChars) ? long.Parse(i.Value) : (long)i.Value[0] }).ToList();
            }
            private static List<AlphabetItem> GetOtherAlphabetItems(Boolean defaultEnable)
            {
                return (from n in Enumerable.Range(222, 34) select new AlphabetItem() { isEnabled = defaultEnable, Value = Char.ConvertFromUtf32(n), DisplayName = Char.ConvertFromUtf32(n).ToString().ToUpper(), isSelected = false }).ToList();
            }
            #endregion
    }
}