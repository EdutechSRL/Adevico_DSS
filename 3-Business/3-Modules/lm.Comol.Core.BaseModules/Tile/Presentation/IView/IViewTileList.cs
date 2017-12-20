using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation 
{
    public interface IViewTileList : IViewBase
    {
        #region "Context"
            Int32 CurrentPageSize { get; set; }
            Int32 IdSelectedTileLanguage { get; set; }
            Int32 IdTilesCommunity { get; set; }
            lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
            dtoFilters CurrentFilters { get; set; }
            Boolean FirstLoad { get; set; }
            Dictionary<Int32, Boolean> FirstLoadForLanguages { get; set; }
            dtoFilters GetSubmittedFilters();
        #endregion
        String GetDefaultLanguageName();
        String GetDefaultLanguageCode();
        String GetUnknownUserName();
        Dictionary<lm.Comol.Core.Dashboard.Domain.TileType, String> GetTranslatedTileTypes();
        void InitializeControl(ModuleDashboard permissions, DashboardType type, Int32 idCommunity, Boolean loadFromRecycleBin, long idTile, TileType preloadType);
        void LoadLanguages(List<lm.Comol.Core.Dashboard.Domain.dtoItemFilter<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem>> items);
        void LoadTilesInfo(Int32 unstranslatedTiles);
        void LoadTilesInfo(Int32 communityTypesWithoutTiles,Int32 unstranslatedTiles);
        void LoadTiles(List<dtoTileItem> items, Int32 idLanguage);
        void DisplayErrorLoadingFromDB();
        void DisplayMessage(ModuleDashboard.ActionType action);
        void AllowApplyFilters(Boolean allow);
        void GenerateCommunityTypesTile();
        void HideCommunityTypesTileAutoGenerate(Boolean hideAutoGenerate);
        void LoadDefaultFilters(List<lm.Comol.Core.DomainModel.Filters.Filter> filters);
    }
}