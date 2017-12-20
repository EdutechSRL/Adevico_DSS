using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewDashboardPreview : IViewPageBase
    {
        #region "Preload"
            long PreloadIdDashboard { get; }
            DashboardType PreloadDashboardType { get; }
            WizardDashboardStep PreloadStep { get; }
            GroupItemsBy PreloadGroupBy { get; }
            OrderItemsBy PreloadOrderBy { get; }
            DashboardViewType PreloadViewType { get; }
            //Boolean LoadViewType { get; }
            long PreloadIdTile { get; }
            long PreloadIdTag { get; }
        #endregion
        long IdDashboard { get; set; }
        List<long> TagsToLoad { get; set; }

        DashboardType DashboardType { get; set; }
        void EnableFullWidth(Boolean value);
        PlainLayout CurrentLayout { get; set; }
        GroupItemsBy SelectedGroupBy { get; set; }
        DashboardViewType CurrentViewType { get; set; }
        UserCurrentSettings CurrentSettings { get; set; }
        WizardDashboardStep CurrentStep { get; set; }
        Boolean IsInitialized { get; set; }

        Dictionary<DashboardViewType, List<dtoItemFilter<OrderItemsBy>>> CurrentOrderItems { get; set; }
        void DisplayUnknownDashboard();
        void InitializeSettingsInfo(liteDashboardSettings settings);
        void InitializeViewSelector(List<dtoItemFilter<DashboardViewType>> items);
        void InitializeGroupBySelector(List<dtoItemFilter<GroupItemsBy>> items);
        void InitializeSearch(DisplaySearchItems settings);
        void InitializeLayout(PlainLayout layout, DisplayNoticeboard display);
        void InitializeCommunitiesList(litePageSettings pSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items);
        void IntializeCombinedView(litePageSettings pSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idDashboard, long preloadIdTile = -1);
        void IntializeTileView(Int32 idCommunity, DisplayNoticeboard noticeboard, litePageSettings pSettings, UserCurrentSettings userSettings, long idDashboard);

        void InitializeSearchView(litePageSettings pageSettings, List<lm.Comol.Core.DomainModel.Filters.Filter> fToLoad, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, List<dtoItemFilter<OrderItemsBy>> items, liteTile tile, Int32 idLanguage, Int32 idDefaultLanguage);
        lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters GetSubmittedFilters();
        void ApplyFilters(litePageSettings pageSettings, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, liteTile tile, Int32 idLanguage ,Int32 idDefaultLanguage);
        void DisplayNoViewAvailable();
    }
}