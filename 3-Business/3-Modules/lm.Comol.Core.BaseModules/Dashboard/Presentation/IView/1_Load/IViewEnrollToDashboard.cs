using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewEnrollToDashboard : IViewBaseSearch
    {
        Boolean PreloadCommunityList { get; }
        //void HideFilters();
        //List<Int32> GetSelectedItems();
        //void ApplyFilters(litePageSettings pageSettings, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, liteTile tile);
        //void InitializeCommunitiesList(litePageSettings pageSettings, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, List<dtoItemFilter<OrderItemsBy>> items, liteTile tile);
        void InitializeSubscriptionControl(Int32 itemsForPage, RangeSettings range,Int32 preloadIdCommunityType, String searchText, Boolean preloadList);
    }
}