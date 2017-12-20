using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Dashboard.Domain;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewCommunitiesList : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Boolean CurrentAscending { get; set; }
        OrderItemsBy CurrentOrderBy { get; set; }
        lm.Comol.Core.DomainModel.ItemDisplayOrder CurrentDisplayOrder { get; set; }
        String TitleCssClass { get; set; }
        Boolean IsCollapsable { get; set; }
        Boolean IsCollapsed { get; set; }
        String TitleImage { get; set; }
        String DataIdForCollapse { get; set; }
        lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }

        Int32 CurrentPageSize { get; set; }
        Int32 DefaultPageSize { get; set; }

        Boolean UseDefaultStartupItems { get; set; }
        Int32 DefaultStartupItems { get; set; }
      
        Boolean IsInitialized { get; set; }
        Int32 CurrentPageIndex { get; }
        Boolean DisplayLessCommand { get; set; }
        Boolean DisplayMoreCommand { get; set; }
        String AutoDisplayTitle { get; set; }

        Int32 IdCurrentCommunityType { get; set; }
        Int32 IdCurrentRemoveCommunityType { get; set; }
        long IdCurrentTag { get; set; }
        long IdCurrentTile { get; set; }

        List<long> IdCurrentTileTags { get; set; }
        DashboardViewType PageType { get; set; }
        Boolean IsForSearch { get; set; }
        List<searchColumn> AvailableColumns { get; set; }
        lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters CurrentFilters { get; set; }


        void DisplaySessionTimeout();

        void InitalizeControlForTile(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idTile, lm.Comol.Core.DomainModel.ItemDisplayOrder display = ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last);
        void InitalizeControlForTile(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, dtoTileDisplay tile, lm.Comol.Core.DomainModel.ItemDisplayOrder display = ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last);
        void InitalizeControlForTag(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idTag, lm.Comol.Core.DomainModel.ItemDisplayOrder display = ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last);
        void InitalizeControlForCommunityType(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, Int32 idCommunityType, lm.Comol.Core.DomainModel.ItemDisplayOrder display = ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last);
        void InitalizeControl(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, lm.Comol.Core.DomainModel.ItemDisplayOrder display = ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last, Int32 idRemoveCommunityType = -1);
        void InitalizeControl(litePageSettings pageSettings, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, List<dtoItemFilter<OrderItemsBy>> orderByItems,liteTile tile, lm.Comol.Core.DomainModel.ItemDisplayOrder display = ItemDisplayOrder.first | lm.Comol.Core.DomainModel.ItemDisplayOrder.last);

        void ApplyFilters(litePageSettings pageSettings,lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters);

        void InitializeOrderBySelector(List<dtoItemFilter<OrderItemsBy>> items);
        void UpdateUserSettings(UserCurrentSettings settings);
        void LoadItems(List<dtoSubscriptionItem> items, OrderItemsBy orderBy, Boolean ascending);
        List<Int32> GetIdcommunitiesWithNews(List<Int32> idCommunities, Int32 idUser);
        void DisplayErrorFromDB();
        void SendUserAction(int idCommunity, int idModule, ModuleDashboard.ActionType action);
        void SendUserAction(int idCommunity, int idModule, int idActionCommunity, ModuleDashboard.ActionType action);
        void DisplayUnableToUnsubscribe(String community);
        void DisplayUnsubscribedFrom(String community);
        void DisplayUnsubscriptionMessage(List<String> unsubscribedFrom, List<String> unableToUnsubscribeFrom);
        //void DisplayConfirmMessage(Int32 idCommunity, String path, List<dtoUnsubscribeTreeNode> communities, List<RemoveAction> actions, RemoveAction selected);
        void DisplayConfirmMessage(Int32 idCommunity, String path, dtoUnsubscribeTreeNode community, List<RemoveAction> actions, RemoveAction selected, List<dtoUnsubscribeTreeNode> alsoFromCommunities = null);
        void DisplayUnsubscribeNotAllowed(String community);
    }
}