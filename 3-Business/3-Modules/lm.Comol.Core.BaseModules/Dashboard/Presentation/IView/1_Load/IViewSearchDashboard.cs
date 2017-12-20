using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewSearchDashboard : IViewBaseSearch
    {
        Boolean PreloadMyCommunities { get; }
        long PreloadIdTile { get; }
        List<long> TagsToLoad { get; set; }
        void InitalizeTopBar(liteDashboardSettings settings, UserCurrentSettings userSettings, String searchBy);
        void ApplyFilters(litePageSettings pageSettings, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, liteTile tile, Int32 idLanguage, Int32 idDefaultLanguage);
        void InitializeCommunitiesList(litePageSettings pageSettings, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, List<dtoItemFilter<OrderItemsBy>> items, liteTile tile, Int32 idLanguage, Int32 idDefaultLanguage);
        void InitializeCommunitiesList(litePageSettings pageSettings, List<dtoItemFilter<OrderItemsBy>> items, liteTile tile, Int32 idLanguage, Int32 idDefaultLanguage);
        void LoadDefaultFilters(List<lm.Comol.Core.DomainModel.Filters.Filter> filters);
    }
}