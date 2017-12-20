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
using lm.Comol.Core.BaseModules.Tiles.Domain;

namespace lm.Comol.Core.BaseModules.Tiles.Business
{
    public partial class ServiceTiles : lm.Comol.Core.Dashboard.Business.ServiceDashboard 
    {
        private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement _serviceCommunityManagement;
        protected lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement ServiceCommunityManagement { get { return _serviceCommunityManagement ?? new lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(_Context); } }

        #region initClass
            public ServiceTiles() :base() { }
            public ServiceTiles(iApplicationContext oContext)
                : base(oContext)
            {

            }
            public ServiceTiles(iDataContext oDC)
                : base(oDC)
            {

            }
        #endregion

        #region "Manage List"
            public Int32 GetCommunityTypesWithoutTilesCount()
            {
                Int32 count = 0;
                try
                {
                    List<Int32> idTileCommunityType = (from t in Manager.GetIQ<liteTile>()
                                                       where t.Deleted == BaseStatusDeleted.None && t.Type == TileType.CommunityType select t).ToList().SelectMany(t=> t.CommunityTypes).ToList();

                    count = (from t in Manager.GetIQ<CommunityType>() select t.Id).Count() -  idTileCommunityType.Distinct().Count();
                }
                catch (Exception ex)
                {

                }
                return count;
            }

            public List<Tile> GenerateCommunityTypesTile(){
                List<Tile> tiles = new List<Tile>();
                try{
                    List<Int32> idTileCommunityType = (from t in Manager.GetIQ<liteTile>()
                                                       where t.Deleted == BaseStatusDeleted.None && t.Type == TileType.CommunityType select t).ToList().SelectMany(t=> t.CommunityTypes).ToList();
                    List<Int32> idTypes = (from t in Manager.GetIQ<CommunityType>() select t.Id).ToList().Except(idTileCommunityType).ToList();
                    if (idTypes.Any()){
                        litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                        if (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser)
                        {
                            List<Language> languages = Manager.GetAllLanguages().ToList();
                            Dictionary<Int32, List<dtoTranslatedCommunityType>> types = languages.ToDictionary(l=> l.Id, l=>  Manager.GetTranslatedItem<dtoTranslatedCommunityType>(l.Id));
                            Language dLanguage = languages.Where(l=> l.isDefault).FirstOrDefault(); 
                            foreach (Int32 idType in idTypes){
                                try{
                                    Tile tile = new Tile();
                                    tile.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                    tile.Status = AvailableStatus.Available;
                                    tile.Type =  TileType.CommunityType;
                                    tile.IdCommunity = 0;
                                    tile.AutoNavigateUrl=true;
                                    tile.DefaultTranslation.Title= types[dLanguage.Id].Where(t=> t.Id== idType).FirstOrDefault().Name;
                                    tile.DefaultTranslation.ShortTitle= tile.DefaultTranslation.Title;
                                    Manager.BeginTransaction();
                                    Manager.SaveOrUpdate(tile);
                                    foreach(Language l in languages){
                                        TileTranslation tTranslation = new TileTranslation();
                                        tTranslation.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                        tTranslation.IdLanguage = l.Id;
                                        tTranslation.LanguageCode = l.Code;
                                        tTranslation.LanguageName = l.Name;
                                        tTranslation.Tile = tile;
                                        tTranslation.Translation.Title= types[l.Id].Where(t=> t.Id== idType).FirstOrDefault().Name;
                                        tTranslation.Translation.ShortTitle= tTranslation.Translation.Title;
                                        Manager.SaveOrUpdate(tTranslation);
                                        tile.Translations.Add(tTranslation);
                                    }
                                    Manager.SaveOrUpdate(tile);
                                    tile.CommunityTypes.Add(idType);
                                    Manager.SaveOrUpdate(tile);
                                    Manager.Commit();
                                    tiles.Add(tile);
                                    SetTileDashboardAssignments(p, tile);
                                }
                                catch(Exception ex){

                                }
                            }
                        }
                    }
                }
                catch (Exception ex){
                    
                }
                if (tiles.Any()){
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard);
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllDashboardTiles);
                }
                return tiles;


    
        //lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllDashboard)
        //lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllCurrentSettings)
        //lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllCommunityTags)
        //lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllTags)
            }
            public Int32 GetUntranslatedTilesCount(dtoFilters filters)
            {
                Int32 count = 0;
                try
                {
                    if (filters.IdSelectedLanguage > 0)
                    {
                        IQueryable<liteTile> query = GetAvailableTiles(filters);
                        count = query.ToList().Where(t => t.Translations == null
                                                     || !t.Translations.Where(tr => tr.Deleted == BaseStatusDeleted.None && tr.IdLanguage == filters.IdSelectedLanguage).Any()
                                                     || t.Translations.Where(tr => tr.Deleted == BaseStatusDeleted.None && tr.IdLanguage == filters.IdSelectedLanguage && String.IsNullOrEmpty(tr.Translation.Title)).Any()
                                                     ).Count();


                    }
                }
                catch (Exception ex)
                {

                }
                return count;
            }
            public List<dtoTileItem> GetTiles(Int32 idPerson,ModuleDashboard permissions, dtoFilters filters, String unknownUser, Dictionary<lm.Comol.Core.Dashboard.Domain.TileType, String> translatedTileType)
            {
                List<dtoTileItem> items = null;
                try
                {
                    Person person = Manager.GetPerson(idPerson);
                    if (person != null)
                    {
                        List<Language> languages = Manager.GetAllLanguages().ToList();
                        IEnumerable<liteTile> tQuery = GetAvailableTiles(filters).AsEnumerable();
                        List<Int32> idCommunityTypes = (from t in Manager.GetIQ<CommunityType>() select t.Id).ToList();
                        items = tQuery.Select(t => new dtoTileItem(t, permissions, filters.IdSelectedLanguage, languages, idPerson, idCommunityTypes)).ToList().Where(t => t.Translation != null && !String.IsNullOrEmpty(t.Translation.Title)).ToList();
                        List<FilterListItem> modifiers = GetTileModifiers(items.Select(t => t.IdModifiedBy).Distinct().ToList());

                        foreach (var item in items.Where(i => modifiers.Where(o => o.Id == i.IdModifiedBy && !String.IsNullOrEmpty(o.Name)).Any()))
                        {
                            item.ModifiedBy = modifiers.Where(o => o.Id == item.IdModifiedBy).Select(o => o.Name).FirstOrDefault();
                        }
                        foreach (var item in items.Where(i => String.IsNullOrEmpty(i.ModifiedBy)))
                        {
                            item.ModifiedBy = unknownUser + item.IdModifiedBy.ToString();
                        }

                        foreach (var item in items)
                        {
                            item.TranslatedType = translatedTileType[item.Type];
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
                            case OrderTilesBy.Name:
                                query = (filters.Ascending) ? query.OrderBy(s => s.Translation.Title) : query.OrderByDescending(s => s.Translation.Title);
                                break;
                            case OrderTilesBy.Type:
                                query = (filters.Ascending) ? query.OrderBy(s => s.TranslatedType).ThenBy(s => s.Translation.Title) : query.OrderByDescending(s => s.TranslatedType).ThenBy(s => s.Translation.Title);
                                break;
                            case OrderTilesBy.CreatedOn:
                                query = (filters.Ascending) ? query.OrderBy(s => s.CreatedOn) : query.OrderByDescending(s => s.CreatedOn);
                                break;
                            case OrderTilesBy.ModifiedOn:
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

            public List<TileType> GetTileTypesAvailable(DashboardType dashboardType)
            {
                List<TileType> items = new List<TileType>();
                try
                {
                    switch (dashboardType)
                    {
                        case DashboardType.Portal:
                            items.Add(TileType.CombinedTags);
                            items.Add(TileType.DashboardUserDefined);
                            break;
                        default:
                            items.Add(TileType.Module);
                            items.Add(TileType.UserDefined);
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return items;
            }

            public dtoEditTile GetEditTile(long idTile)
            {
                dtoEditTile tile = null;
                try
                {
                    liteTile t = Manager.Get<liteTile>(idTile);
                    if (t!=null)
                        tile = new dtoEditTile(t, Manager.GetAllLanguages().ToList());
                }
                catch (Exception ex)
                {

                }
                return tile;
            }
            public String GetCommunityTypeName(Int32 idType, Int32 idLanguage)
            {
                return Manager.GetCommunityTypeName(idType, idLanguage);
            }
            public Tile SaveTile(dtoEditTile dto)
            {
                Tile tile = null;
                litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                if (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser)
                {
                    List<Language> languages = Manager.GetAllLanguages().ToList();
                    Boolean toAdd = (dto.Id == 0);
                    if (toAdd)
                    {
                        tile = new Tile();
                        tile.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        tile.Status = AvailableStatus.Draft;
                        tile.Type = dto.Type;
                        tile.IdCommunity = dto.IdCommunity;
                    }
                    else
                    {
                        tile = Manager.Get<Tile>(dto.Id);
                        if (tile == null)
                            return null;
                        tile.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    tile.AutoNavigateUrl = (tile.Type== TileType.CombinedTags || tile.Type== TileType.CommunityTag || tile.Type == TileType.CommunityType);
                

                    if (!dto.Translations.Where(t => t.IdLanguage == 0 && !String.IsNullOrEmpty(t.Translation.Title) && !String.IsNullOrEmpty(t.Translation.Title.Trim())).Any())
                        throw new TileException(ErrorMessageType.DefaultTranslationMissing);
                    else
                    {
                        lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation item = dto.Translations.Where(t => t.IdLanguage == 0).Select(t=> t.Translation).FirstOrDefault();
                        if (HasDefaultDuplicate(dto.Id, item.Title, dto.Type, dto.IdCommunity))
                            throw new TileException(ErrorMessageType.DefaultTranslationDuplicate);
                        else{
                            tile.DefaultTranslation = new DomainModel.Languages.TitleDescriptionObjectTranslation() { Title = item.Title, Description = item.Description };
                            if (String.IsNullOrEmpty(tile.DefaultTranslation.Description))
                                tile.DefaultTranslation.Description = "";
                        }
                    }
                    tile.ImageCssClass = dto.ImageCssClass;
                    tile.ImageUrl = dto.ImageUrl;
                    tile.NavigateUrl = dto.NavigateUrl;
                    try
                    {
                        Manager.BeginTransaction();
                        Manager.SaveOrUpdate(tile);
                        Manager.Commit();
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard);
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllDashboardTiles);
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        throw new TileException(ErrorMessageType.SavingTile);
                    }
                    foreach (var item in dto.Translations.Where(i=> i.IdLanguage>0))
                    {
                        Language l = languages.Where(ln=> ln.Id== item.IdLanguage).FirstOrDefault();
                        if (l!=null)
                            item.LanguageCode= l.Code;
                    }
                    SaveTileTranslation(p, tile, dto.Translations);
                    if (tile.Type== TileType.CombinedTags)
                        SaveTileTagLinks(p, tile, dto.IdTags);
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllUserDashboard);
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Dashboard.Domain.CacheKeys.AllDashboardTiles);
                    if (toAdd)
                        SetTileDashboardAssignments(p,tile);
                }
                return tile;
            }
            private void SaveTileTranslation(litePerson person, Tile tile, List<lm.Comol.Core.DomainModel.Languages.dtoBaseObjectTranslation> dtoTranslations)
            {
                List<TileTranslation> tileTranslations = new List<TileTranslation>();
                if (tile.Translations == null || !tile.Translations.Any())
                {
                    tileTranslations = dtoTranslations.Where(t => t.IdLanguage > 0).Select(t => new TileTranslation()
                    {
                        Tile = tile,
                        IdLanguage = t.IdLanguage,
                        LanguageCode = t.LanguageCode,
                        LanguageName = t.LanguageName,
                        Translation = new DomainModel.Languages.TitleDescriptionObjectTranslation()
                        {
                            Title = String.IsNullOrEmpty(t.Translation.Title) ? "" : t.Translation.Title,
                            Description = String.IsNullOrEmpty(t.Translation.Description) ? "" : t.Translation.Description
                        }
                    }).ToList();
                    tileTranslations.ForEach(t => t.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                }
                else
                {
                    foreach (TileTranslation t in tile.Translations)
                    {
                        lm.Comol.Core.DomainModel.Languages.dtoBaseObjectTranslation item = dtoTranslations.Where(tr => tr.IdLanguage == t.IdLanguage).FirstOrDefault();
                        if (item != null)
                        {
                            t.Translation.Title = String.IsNullOrEmpty(item.Translation.Title) ? "" : item.Translation.Title;
                            t.Translation.Description = String.IsNullOrEmpty(item.Translation.Description) ? "" : item.Translation.Description;
                            t.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            t.Deleted = BaseStatusDeleted.None;
                        }
                        else
                            t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    }
                    foreach (lm.Comol.Core.DomainModel.Languages.dtoBaseObjectTranslation t in dtoTranslations.Where(tr => tr.IdLanguage > 0 && !tile.Translations.Where(tt => tt.IdLanguage == tr.IdLanguage).Any()))
                    {
                        TileTranslation translation = new TileTranslation();
                        translation.Translation.Title = String.IsNullOrEmpty(t.Translation.Title) ? "" : t.Translation.Title;
                        translation.Translation.Description = String.IsNullOrEmpty(t.Translation.Description) ? "" : t.Translation.Description;
                        translation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        translation.Tile = tile;
                        translation.IdLanguage = t.IdLanguage;
                        translation.LanguageCode = t.LanguageCode;
                        translation.LanguageName = t.LanguageName;
                        tileTranslations.Add(translation);
                    }
                }
                if (tileTranslations != null)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Manager.SaveOrUpdateList(tileTranslations);
                        //if (isNew)
                        //    tag.Translations = translations;
                        //else
                        tileTranslations.ForEach(t => tile.Translations.Add(t));
                        Manager.SaveOrUpdate(tile);
                        Manager.Commit();
                        lm.Comol.Core.Tag.Domain.TagItem tag = tile.GetDefaultTag();
                        if (tag!=null)
                            SaveTagTranslations(person, tile, tag);
                    }
                    catch (TileException tEx)
                    {
                        throw tEx;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        throw new TileException(ErrorMessageType.SavingTranslations);
                    }
                }
            }
            private void SaveTagTranslations(litePerson person, Tile tile, lm.Comol.Core.Tag.Domain.TagItem tag)
            {
                List<lm.Comol.Core.Tag.Domain.TagTranslation> tagTranslations = new List<Tag.Domain.TagTranslation>();
                tag.DefaultTranslation = tile.DefaultTranslation;
                foreach (lm.Comol.Core.Tag.Domain.TagTranslation t in tag.Translations)
                {
                    TileTranslation tTranslation = tile.Translations.Where(tg => tg.IdLanguage == t.IdLanguage && tg.Deleted == BaseStatusDeleted.None).FirstOrDefault();
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
              
                foreach (TileTranslation t in tile.Translations.Where(tr => tr.Deleted == BaseStatusDeleted.None && tr.IdLanguage > 0 && !tag.Translations.Where(tt => tt.IdLanguage == tr.IdLanguage).Any()))
                {
                    lm.Comol.Core.Tag.Domain.TagTranslation translation = new lm.Comol.Core.Tag.Domain.TagTranslation();
                    translation.Translation.Title = t.Translation.Title;
                    translation.Translation.Description = t.Translation.Description;
                    translation.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    translation.Tag = tag;
                    translation.IdLanguage = t.IdLanguage;
                    translation.LanguageCode = t.LanguageCode;
                    translation.LanguageName = t.LanguageName;
                    tagTranslations.Add(translation);
                }
                try
                {
                    Manager.BeginTransaction();
                    if (tag.Translations.Any())
                        Manager.SaveOrUpdateList(tag.Translations);
                    if (tagTranslations.Any())
                    {
                        Manager.SaveOrUpdateList(tagTranslations);
                        tagTranslations.ForEach(t => tag.Translations.Add(t));
                    }
                    Manager.SaveOrUpdate(tag);
                    Manager.Commit();
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllCommunityTags);
                    CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllUserCommunitiesTags);
                }
                catch(Exception ex){
                    Manager.RollBack();
                    throw new TileException(ErrorMessageType.SavingTagTranslations, ex);
                }
            }
            private void SaveTileTagLinks(litePerson person, Tile tile, List<long> idTags)
            {
                try
                {
                    Boolean toSave = false;
                    Manager.BeginTransaction();
                    List<TileTagAssociation> newLinks = new List<TileTagAssociation>();
                    List<TileTagAssociation> links = (from l in Manager.GetIQ<TileTagAssociation>() where l.Tile.Id== tile.Id select l).ToList();
                    foreach (TileTagAssociation t in links)
                    {
                        if (t.Tag != null && idTags.Contains(t.Tag.Id))
                        {
                            if ( t.Deleted != BaseStatusDeleted.None)
                                toSave = true;
                            t.Deleted = BaseStatusDeleted.None;
                        }
                        else if (t.Deleted == BaseStatusDeleted.None)
                        {
                            t.SetDeleteMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            toSave = true;
                        }
                    }

                    foreach (long idTag in idTags.Where(i=> !links.Where(l=> l.Deleted== BaseStatusDeleted.None && l.Tag != null && l.Tag.Id ==i).Any()))
                    {
                        lm.Comol.Core.Tag.Domain.TagItem tag = Manager.Get<lm.Comol.Core.Tag.Domain.TagItem>(idTag);
                        if (tag.Status == AvailableStatus.Available && tag.Deleted == BaseStatusDeleted.None)
                        {
                            TileTagAssociation link = new TileTagAssociation();
                            link.Tag = tag;
                            link.Tile = tile;
                            link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            newLinks.Add(link);

                        }
                    }

                    if (newLinks.Any())
                    {
                        toSave = true;
                        Manager.SaveOrUpdateList(newLinks);
                    }
                    Manager.SaveOrUpdate(tile);
                    Manager.Commit();
                    if (toSave)
                    {
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllCommunityTags);
                        CacheHelper.PurgeCacheItems(lm.Comol.Core.Tag.Domain.CacheKeys.AllUserCommunitiesTags);
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    throw new TileException(ErrorMessageType.SavingTagLinks, ex);
                }
            }
            public void SetTileDashboardAssignments(litePerson person, Tile tile)
            {
                try
                {
                    List<DashboardSettings> dSettings = (from s in Manager.GetIQ<DashboardSettings>()
                                                         where s.Deleted == BaseStatusDeleted.None
                                                         && s.Type == DashboardType.Portal
                                                         select s).ToList();
                    List<DashboardTileAssignment> assignments = new List<DashboardTileAssignment>();
                    foreach (DashboardSettings settings in dSettings)
                    {
                        DashboardTileAssignment assignment = new DashboardTileAssignment();
                        assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        assignment.Dashboard = settings;
                        assignment.Tile = tile;
                        assignment.DisplayOrder = GetTileDisplayOrder(0, settings, tile.Type) + 1;
                        assignment.Status = tile.Status;
                        assignments.Add(assignment);
                    }

                    if (assignments.Any())
                    {
                        Manager.BeginTransaction();
                        Manager.SaveOrUpdateList(assignments);
                        Manager.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    throw new TileException(ErrorMessageType.SavingDashboardAssignments);
                }
               
            }
            public Boolean HasDefaultDuplicate(long idTile, String title, TileType type, Int32 idCommunity)
            {
                Boolean result = false;
                try
                {
                    var query = (from t in Manager.GetIQ<liteSearchTile>()
                                 where t.Id != idTile && t.Deleted == BaseStatusDeleted.None 
                                 select t);
                    switch (type)
                    {
                        case TileType.CommunityTag:
                        case TileType.CommunityType:
                            query = query.Where(t => t.Type == type);
                            break;
                        case TileType.DashboardUserDefined:
                        case TileType.CombinedTags:
                            query = query.Where(t => t.Type == TileType.CombinedTags || t.Type == TileType.CommunityTag || t.Type == TileType.DashboardUserDefined);
                            break;
                        case TileType.Module:
                        case TileType.UserDefined:
                            query = query.Where(t => t.IdCommunity==idCommunity && (t.Type == TileType.Module || t.Type == TileType.UserDefined));
                            break;
                    }
                    result = query.ToList().Where(t => t.DefaultTranslation.IsTitleEqual(title)).Any();
                }
                catch (Exception ex)
                {


                }
                return result;
            }

        #endregion

        #region "Manage Filters"

            public List<Filter> GetDefaultFilters(DashboardType type, Int32 idPerson, Int32 idContainerCommunity, Int32 idLanguage, Int32 idTilesCommunity, Boolean loadFromRecycleBin, Dictionary<searchFilterType, long> defaultValues = null)
            {
                Person p = Manager.GetPerson(idPerson);
                if (p != null)
                {
                    ModuleDashboard permissions = null;
                    if (type!= DashboardType.Community)
                        permissions = ModuleDashboard.CreatePortalmodule(p.TypeID);
                    else
                        permissions = GetPermission(idTilesCommunity);
                        
                    if (defaultValues == null)
                        defaultValues = (from searchFilterType n in Enum.GetValues(typeof(searchFilterType)).AsQueryable() where n != searchFilterType.none select n).ToDictionary(t => t, t => (long)-1);

                    if (permissions.Administration || permissions.Edit || permissions.List)
                        return GetFilters(type, p, idTilesCommunity, idLanguage, loadFromRecycleBin,"", defaultValues);
                }
                return new List<Filter>();
            }
            public List<Filter> ChangeFilters(DashboardType type, Int32 idPerson, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter filter, Int32 idCommunity, Int32 idLanguage,  Boolean fromRecycleBin)
            {
                Person p = Manager.GetPerson(idPerson);
                if (p != null)
                    return ChangeFilters(type, p, filters, filter, idCommunity, idLanguage,  fromRecycleBin);
                else
                    return filters;
            }
            public List<Filter> ChangeFilters(DashboardType dType, Person person, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter filter, Int32 idCommunity, Int32 idLanguage, Boolean fromRecycleBin)
            {
                if (filter != null)
                {
                    Dictionary<searchFilterType, long> dValues = GetDefaultValues(filters);
                    searchFilterType type = lm.Comol.Core.DomainModel.Helpers.EnumParser<searchFilterType>.GetByString(filter.Name, searchFilterType.none);

                    IQueryable<liteTile> query = GetAvailableTiles(dType, idCommunity, fromRecycleBin);
                    switch (type)
                    {
                        case searchFilterType.type:
                            return GetFilters(dType, person, idCommunity, idLanguage, fromRecycleBin, filters.Where(f => f.Name == searchFilterType.name.ToString()).Select(f => f.Value).FirstOrDefault(), dValues);
                        default:
                            if (dValues[searchFilterType.type] > 0)
                                query = query.Where(t => (long)t.Type== dValues[searchFilterType.type]);

                            switch (type)
                            {
                                case searchFilterType.modifiedby:
                                    filters = filters.Where(f => f.Name == searchFilterType.type.ToString() || f.Name == searchFilterType.name.ToString()).ToList();
                                    AddModifierFilters(query, dValues, filters, filter, idLanguage);
                                    break;
                                case searchFilterType.status:

                                    query = query.Where(t => (dValues[searchFilterType.modifiedby] == -1 || (dValues[searchFilterType.modifiedby] == t.IdModifiedBy)));

                                    filters = filters.Where(f => f.Name != searchFilterType.letters.ToString()).ToList();
                                    filters.Add(GetLettersFilter(query, idLanguage, dValues));
                                    break;
                            }
                            break;
                    }
                }
                return filters;
            }

            private List<Filter> GetFilters(DashboardType type, Person person, Int32 idCommunity, Int32 idLanguage, Boolean fromRecycleBin, String searchBy="", Dictionary<searchFilterType, long> defaultValues = null)
            {
                List<lm.Comol.Core.DomainModel.Filters.Filter> filters = new List<lm.Comol.Core.DomainModel.Filters.Filter>();
                IQueryable<liteTile> tiles = GetAvailableTiles(type,idCommunity, fromRecycleBin);
                lm.Comol.Core.DomainModel.Filters.Filter types = GetTypeFilter(tiles, defaultValues);

                if (types != null)
                    AddTypeFilters(tiles, defaultValues, filters, types, idLanguage);
                else
                {
                    Filter modifier = GetModifierFilter(tiles, defaultValues);
                    if (modifier != null && modifier.Values.Count > 1)
                        AddModifierFilters(tiles, defaultValues, filters, modifier, idLanguage);
                    else
                        AddFilterStatusAndLetters(tiles, defaultValues, filters, idLanguage);
                }

                filters.Add(GetSimpleSearchFilter(searchBy));
                return filters;
            }

            private IQueryable<liteTile> GetAvailableTiles(dtoFilters filters)
            {
                IQueryable<liteTile> query = GetAvailableTiles(filters.DashboardType, filters.IdCommunity, filters.FromRecycleBin);
                if (filters.IdModifiedBy > 0)
                    query = query.Where(t => t.IdModifiedBy == filters.IdModifiedBy);
                if (filters.Status != AvailableStatus.Any)
                    query = query.Where(t => t.Status == filters.Status);
                if (filters.IdTileType >-1)
                    query = query.Where(t => (long)t.Type == filters.IdTileType);

                return query;
            }
            private IQueryable<liteTile> GetAvailableTiles(DashboardType type,Int32 idCommunity, Boolean fromRecycleBin)
            {
                BaseStatusDeleted deleted = (fromRecycleBin) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                var query = (from t in Manager.GetIQ<liteTile>() where t.Deleted == deleted select t);
                switch (type)
                {
                    case DashboardType.Community:
                        query = query.Where(t => (t.Type == TileType.Module || t.Type == TileType.UserDefined) && t.IdCommunity==idCommunity);
                        break;
                    case DashboardType.AllCommunities:
                        query = query.Where(t => (t.Type == TileType.Module || t.Type == TileType.UserDefined) && t.IdCommunity == -1);
                        break;
                    case DashboardType.Portal:
                        query = query.Where(t => t.Type == TileType.CombinedTags || t.Type == TileType.CommunityTag || t.Type == TileType.CommunityType || t.Type == TileType.DashboardUserDefined);
                        break;
                }
                return query;
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
            private void AddTypeFilters(IQueryable<liteTile> query, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter types, Int32 idLanguage)
            {
                if (types.Selected != null && types.Selected.Id > 0)
                    query = query.Where(t => t.Type == (TileType)types.Selected.Id);
                filters.Add(types);

                Filter modifier = GetModifierFilter(query, defaultValues);
                if (modifier != null && modifier.Values.Count > 1)
                    AddModifierFilters(query, defaultValues, filters, modifier, idLanguage);
                else
                    AddFilterStatusAndLetters(query, defaultValues, filters, idLanguage);
            }
            private void AddModifierFilters(IQueryable<liteTile> query, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Filter modifierFilter, Int32 idLanguage)
            {
                query = query.Where(n => defaultValues[searchFilterType.modifiedby] == -1 || n.IdModifiedBy == defaultValues[searchFilterType.modifiedby]);
                filters.Add(modifierFilter);
                AddFilterStatusAndLetters(query, defaultValues, filters, idLanguage);

            }
            private void AddFilterStatusAndLetters(IQueryable<liteTile> query, Dictionary<searchFilterType, long> defaultValues, List<lm.Comol.Core.DomainModel.Filters.Filter> filters, Int32 idLanguage)
            {
                lm.Comol.Core.DomainModel.Filters.Filter statusFilter = GetStatusFilter(query, defaultValues);
                if (statusFilter!= null && statusFilter.Values.Count > 1)
                {
                    filters.Add(GetStatusFilter(query, defaultValues));
                    query = query.Where(t => (defaultValues[searchFilterType.status] == -1 || (defaultValues[searchFilterType.status] == (long)t.Status )));
                }
                filters.Add(GetLettersFilter(query, idLanguage, defaultValues));
            }
      
            private lm.Comol.Core.DomainModel.Filters.Filter GetTypeFilter(IEnumerable<liteTile> tiles, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                List<TileType> items = tiles.Select(n => n.Type).Distinct().OrderBy(n => n).ToList();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.type.ToString();
                filter.DisplayOrder = (int)searchFilterType.type;
                filter.AutoUpdate = true;
                filter.Values = items.Select(p => new FilterListItem() { Id = (long)p, Name = "" }).ToList();

                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id = -1 });
                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.type]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.type]).FirstOrDefault();
                //else if (items.Contains(TagType.Community))
                //{
                //    filter.Selected = filter.Values.Where(v => v.Id == (long)TagType.Community).FirstOrDefault();
                //    defaultValues[searchFilterType.type] = (long)TagType.Community;
                //}
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.type] = filter.Selected.Id;
                }
                return filter;
            }
            private lm.Comol.Core.DomainModel.Filters.Filter GetModifierFilter(IEnumerable<liteTile> tiles, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.modifiedby.ToString();
                filter.DisplayOrder = (int)searchFilterType.modifiedby;
                filter.AutoUpdate = true;
                filter.Values = GetTileModifiers(tiles.Select(t => t.IdModifiedBy).Distinct().ToList());
                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id = -1 });

                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.modifiedby]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.modifiedby]).FirstOrDefault();
                else if (filter.Values.Any())
                {
                    filter.Selected = filter.Values.FirstOrDefault();
                    defaultValues[searchFilterType.modifiedby] = filter.Selected.Id;
                }
                return filter;
            }
            private List<FilterListItem> GetTileModifiers(List<Int32> idUsers)
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
            private lm.Comol.Core.DomainModel.Filters.Filter GetStatusFilter(IEnumerable<liteTile> tiles, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                List<AvailableStatus> items = tiles.Select(n => n.Status).Distinct().OrderBy(n => n).ToList();
                filter.FilterType = DomainModel.Filters.FilterType.Select;
                filter.Name = searchFilterType.status.ToString();
                filter.DisplayOrder = (int)searchFilterType.status;
                filter.AutoUpdate = false;
                filter.Values = items.Select(p => new FilterListItem() { Id = (long)p, Name = "" }).ToList();

                if (filter.Values.Count > 1)
                    filter.Values.Insert(0, new FilterListItem() { Id = -1 });
                if (filter.Values.Where(v => v.Id == defaultValues[searchFilterType.status]).Any())
                    filter.Selected = filter.Values.Where(v => v.Id == defaultValues[searchFilterType.status]).FirstOrDefault();
                else if (items.Contains(AvailableStatus.Available))
                {
                    filter.Selected = filter.Values.Where(v => v.Id == (long)AvailableStatus.Available).FirstOrDefault();
                    defaultValues[searchFilterType.status] = (long)AvailableStatus.Available;
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
            private lm.Comol.Core.DomainModel.Filters.Filter GetLettersFilter(IEnumerable<liteTile> tiles, Int32 idLanguage, Dictionary<searchFilterType, long> defaultValues)
            {
                lm.Comol.Core.DomainModel.Filters.Filter filter = new lm.Comol.Core.DomainModel.Filters.Filter();
                filter.FilterType = DomainModel.Filters.FilterType.MaskedRadio;
                filter.Name = searchFilterType.letters.ToString();
                filter.DisplayOrder = (int)searchFilterType.letters;
                filter.AutoUpdate = false;
                filter.Values = GenerateAlphabetItems(tiles.Select(n => n.GetFirstLetter(idLanguage)).Where(n => !String.IsNullOrEmpty(n)).OrderBy(n => n).Distinct().ToList(), defaultValues[searchFilterType.letters]);
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
                //items.AddRange(GetOtherAlphabetItems(false));
                //if (displayMode.IsFlagSet(AlphabetDisplayMode.addUnmatchLetters))
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