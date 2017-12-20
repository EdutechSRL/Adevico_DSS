using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class CommunitiesListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities service;
        private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;
        protected virtual IViewCommunitiesList View
        {
            get { return (IViewCommunitiesList)base.View; }
        }
        private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities Service
        {
            get
            {
                if (service == null)
                    service = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                return service;
            }
        }
        private lm.Comol.Core.Tag.Business.ServiceTags ServiceTags
        {
            get
            {
                if (servicetag == null)
                    servicetag = new lm.Comol.Core.Tag.Business.ServiceTags(AppContext);
                return servicetag;
            }
        }
        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleDashboard.UniqueCode);
                return currentIdModule;
            }
        }
        public CommunitiesListPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public CommunitiesListPresenter(iApplicationContext oContext, IViewCommunitiesList view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion


        #region "Standard Initialize View"
            public void InitViewByTile(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, dtoTileDisplay tile)
            {
                InitView(pageSettings, userSettings, items, -1,  -1, -1,-1, tile);
            }
            public void InitViewByTile(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idTile)
            {
                InitView(pageSettings, userSettings, items, -1, -1, idTile);
            }
            public void InitViewByTag(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, long idTag)
            {
                InitView(pageSettings, userSettings, items, -1, -1, -1, idTag);
            }
            public void InitView(litePageSettings pageSettings, UserCurrentSettings userSettings, List<dtoItemFilter<OrderItemsBy>> items, Int32 idCommunityType = -1, Int32 idRemoveCommunityType = -1, long idTile = -1, long idTag = -1, dtoTileDisplay tile = null)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    View.IdCurrentCommunityType = idCommunityType;
                    View.IdCurrentRemoveCommunityType = idRemoveCommunityType;
                    View.IdCurrentTag = idTag;
                    View.IdCurrentTile = (tile == null) ? idTile : tile.Id;
                    View.IdCurrentTileTags = (tile == null) ? new List<long>() : tile.Tags;
                    View.PageType = pageSettings.Type;
                    View.DefaultPageSize = pageSettings.MaxMoreItems;
                    Int32 itemsCount = Service.GetSubscribedCommunitiesCount(UserContext.CurrentUserID, pageSettings.Type, idCommunityType, idRemoveCommunityType, idTile, idTag, tile);
                    Int32 pageSize = InitializePageSize(pageSettings, itemsCount);
                    View.CurrentPageSize = pageSize;

                    OrderItemsBy orderBy = items.Where(i => i.Selected).Select(i => i.Value).FirstOrDefault();
                    Boolean ascending = false;
                    if (orderBy != userSettings.OrderBy)
                    {
                        switch (orderBy)
                        {
                            //case OrderItemsBy.ActivatedOn:
                            //case OrderItemsBy.ClosedOn:
                            //case OrderItemsBy.CreatedOn:
                            //case OrderItemsBy.LastAccess:
                            case OrderItemsBy.Name:
                                ascending = true;
                                break;
                        }
                    }
                    else
                        ascending = userSettings.Ascending;

                    View.CurrentOrderBy = orderBy;
                    View.CurrentAscending = ascending;
                    View.AvailableColumns = new List<searchColumn>() { searchColumn.subscriptioninfo, searchColumn.genericdate };
                    LoadCommunities(pageSettings.Type, userSettings, 0, pageSize, orderBy, ascending, idCommunityType, idRemoveCommunityType, idTile, idTag,tile);
                }
            }
            private Int32 InitializePageSize(litePageSettings pageSettings, Int32 itemsCount)
            {
                Int32 pageSize = pageSettings.MaxItems;
                if (View.UseDefaultStartupItems && View.DefaultStartupItems > 0)
                    pageSize = View.DefaultStartupItems;

                if (pageSettings.Range != null && pageSettings.Range.IsInRange(itemsCount))
                    pageSize = pageSettings.Range.DisplayItems;
                if (itemsCount > pageSize)
                    View.DisplayMoreCommand = true;
                return pageSize;
            }
        #endregion

        #region "Search methods"
            public void InitView(litePageSettings pageSettings,lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, List<dtoItemFilter<OrderItemsBy>> items, liteTile tile= null)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    View.IsForSearch = true;
                    View.PageType = DashboardViewType.Search;
                  
                    OrderItemsBy orderBy = items.Where(i => i.Selected).Select(i => i.Value).FirstOrDefault();
                    Boolean ascending = (orderBy == OrderItemsBy.Name);
                    View.CurrentFilters = filters;
                    View.CurrentOrderBy = orderBy;
                    View.CurrentAscending = ascending;

                    Int32 idUserLanguage = ((UserContext != null && UserContext.Language != null) ? UserContext.Language.Id : -2);
                    Int32 idDefaultLanguage = CurrentManager.GetDefaultIdLanguage();

                    LoadCommunities(pageSettings, filters, orderBy, ascending, false,0,0, ModuleDashboard.ActionType.SearchDashboardLoadcommunities);
                }
            }
            private void InitializeColumns(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters){
                List<searchColumn> columns = new List<searchColumn>();

                switch (filters.Availability)
                {
                    case CommunityManagement.CommunityAvailability.Subscribed:
                        columns.Add(searchColumn.actions);
                        columns.Add(searchColumn.subscriptioninfo);
                        columns.Add(searchColumn.genericdate);
                        break;
                }
                View.AvailableColumns = columns;
            }
            private Int32 InitializeSearchPageSize(litePageSettings pageSettings, Int32 itemsCount)
            {
                Int32 pageSize = pageSettings.MaxItems;
                if (View.UseDefaultStartupItems && View.DefaultStartupItems > 0)
                    pageSize = View.DefaultStartupItems;
                else if (pageSettings.Range != null && pageSettings.Range.IsInRange(itemsCount))
                    pageSize = pageSettings.Range.DisplayItems;
                return pageSize;
            }

            public void ApplyFilters(litePageSettings pageSettings, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters,OrderItemsBy orderBy,Boolean ascending)
            {
                View.CurrentFilters = filters;
                LoadCommunities(pageSettings, filters, orderBy, ascending, false, 0, View.CurrentPageSize);
            }
            public void LoadCommunities(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsBy orderBy, Boolean ascending, Int32 pageIndex , Int32 pageSize)
            {
                LoadCommunities(null, filters, orderBy, ascending, true, pageIndex, pageSize, ModuleDashboard.ActionType.SearchDashboardChangePageIndex);
            }
            private void LoadCommunities(litePageSettings pageSettings, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsBy orderBy, Boolean ascending, Boolean useCache, Int32 pageIndex=0, Int32 pageSize=0,ModuleDashboard.ActionType action = ModuleDashboard.ActionType.SearchDashboardApplyFilters)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<dtoSubscriptionItem> items = Service.GetCommunities(UserContext.CurrentUserID, filters, useCache);
                    if (items == null)
                        View.DisplayErrorFromDB();
                    else
                    {
                        Int32 itemsCount = items.Count();

                        if (pageSettings != null)
                        {
                            View.DefaultPageSize = pageSettings.MaxItems;
                            InitializeColumns(filters);
                            pageSize = InitializeSearchPageSize(pageSettings, itemsCount);
                            View.CurrentPageSize = pageSize;
                        }
                        if (pageSize == 0)
                            pageSize = View.CurrentPageSize;
                        PagerBase pager = new PagerBase();
                        pager.PageSize = pageSize;
                        pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;
                        pager.PageIndex = pageIndex;
                        View.Pager = pager;


                        items = Service.GetCommunities(UserContext.Language.Id, items, pageIndex, pageSize, orderBy, ascending);

                        if (items != null)
                        {
                            List<Int32> withNews = View.GetIdcommunitiesWithNews(items.Where(i => i.Status != SubscriptionStatus.communityblocked && i.Status != SubscriptionStatus.blocked && i.Status != SubscriptionStatus.waiting).Select(i => i.Community.Id).ToList(), UserContext.CurrentUserID);
                            if (withNews.Any())
                                items.Where(i => withNews.Contains(i.Community.Id)).ToList().ForEach(i => i.HasNews = true);
                            Language l = CurrentManager.GetDefaultLanguage();
                            Dictionary<Int32, List<String>> tags = ServiceTags.GetCommunityAssociationToString(items.Select(i => i.Community.Id).ToList(), UserContext.Language.Id, l.Id, true);
                            if (tags.Any())
                            {
                                foreach (dtoSubscriptionItem item in items.Where(i => tags.ContainsKey(i.Community.Id)))
                                {
                                    item.Community.Tags = tags[item.Community.Id];
                                }
                            }

                            View.LoadItems(items, orderBy, ascending);
                            //View.SendUserAction(0, CurrentIdModule, action);
                        }
                        else
                            View.LoadItems(new List<dtoSubscriptionItem>(), orderBy, ascending);
                    }
                }
            }

            public void UnsubscribeFromCommunity(Int32 idCommunity, String path, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsBy orderBy, Boolean ascending, Int32 pageIndex, Int32 pageSize)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode node = Service.UnsubscribeInfo(UserContext.CurrentUserID, idCommunity, path);
                    if (node != null)
                    {
                        ModuleDashboard.ActionType dAction = ModuleDashboard.ActionType.None;
                        List<lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode> nodes = node.GetAllNodes();
                        if (!nodes.Where(n => n.AllowUnsubscribe()).Any())
                        {
                            View.DisplayUnableToUnsubscribe(CurrentManager.GetCommunityName(idCommunity));
                            dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunity;
                        }
                        else
                        {
                            List<RemoveAction> actions = new List<RemoveAction>();
                            actions.Add(RemoveAction.None);
                            actions.Add(RemoveAction.FromCommunity);
                            if (nodes.Where(n => n.AllowUnsubscribe()).Count() > 1)
                                actions.Add(RemoveAction.FromAllSubCommunities);

                            if (node == null)
                            {
                                View.DisplayUnableToUnsubscribe(CurrentManager.GetCommunityName(idCommunity));
                                dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunity;
                            }
                            else if (!node.AllowUnsubscribe())
                            {
                                View.DisplayUnsubscribeNotAllowed(node.Name);
                                dAction = ModuleDashboard.ActionType.UnsubscribeNotallowed;
                            }
                            else if (node.AllowUnsubscribe() && (!node.CommunityAllowSubscription || node.MaxUsersWithDefaultRole > 0 || (node.CommunitySubscriptionEndOn.HasValue && DateTime.Now.AddDays(30) > node.CommunitySubscriptionEndOn.Value)))
                            {
                                View.DisplayConfirmMessage(idCommunity, path, node, actions, RemoveAction.None, nodes.Where(n => n.AllowUnsubscribe() && n.Id != idCommunity).ToList());
                                dAction = ModuleDashboard.ActionType.RequireUnsubscribeConfirm;
                            }
                            else
                            {
                                if (nodes.Where(n => n.AllowUnsubscribe()).Count() > 1)
                                {
                                    View.DisplayConfirmMessage(idCommunity, path, node, actions, RemoveAction.FromCommunity, nodes.Where(n => n.AllowUnsubscribe() && n.Id != idCommunity).ToList());
                                    dAction = ModuleDashboard.ActionType.RequireUnsubscribeConfirmFromSubCommunities;
                                }
                                else
                                {
                                    List<liteSubscriptionInfo> subscriptions = Service.UnsubscribeFromCommunity(UserContext.CurrentUserID, node, RemoveAction.FromCommunity);
                                    if (subscriptions != null && subscriptions.Any() && subscriptions.Count == 1 && subscriptions[0].IdRole < 1)
                                    {
                                        View.DisplayUnsubscribedFrom(node.Name);
                                        dAction = ModuleDashboard.ActionType.UnsubscribeFromCommunity;
                                    }
                                    else {
                                        View.DisplayUnableToUnsubscribe(node.Name);
                                        dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunity;
                                    }
                                }
                            }
                        }
                        View.SendUserAction(0, CurrentIdModule, idCommunity, dAction);
                    }
                    else
                    {
                        String name = CurrentManager.GetCommunityName(idCommunity);
                        if (!String.IsNullOrEmpty(name))
                            View.DisplayUnableToUnsubscribe(CurrentManager.GetCommunityName(idCommunity));
                        View.SendUserAction(0, CurrentIdModule, idCommunity, ModuleDashboard.ActionType.UnableToUnsubscribe);
                    }
                    LoadCommunities(filters, orderBy, ascending, pageIndex, pageSize);
                }
            }
            public void UnsubscribeFromCommunity(Int32 idCommunity, String path,RemoveAction action,  lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsBy orderBy, Boolean ascending, Int32 pageIndex, Int32 pageSize)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode node = Service.UnsubscribeInfo(UserContext.CurrentUserID, idCommunity, path);
                    if (node != null)
                    {
                        switch (action)
                        {
                            case RemoveAction.None:
                                break;
                            default:
                                ModuleDashboard.ActionType dAction = ModuleDashboard.ActionType.UnableToUnsubscribe;
                                List<liteSubscriptionInfo> subscriptions = Service.UnsubscribeFromCommunity(UserContext.CurrentUserID, node, action);
                                if (subscriptions == null && !subscriptions.Any())
                                {
                                    switch (action)
                                    {
                                        case RemoveAction.FromCommunity:
                                            dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunity;
                                            View.DisplayUnableToUnsubscribe(node.Name);
                                            break;
                                        case RemoveAction.FromAllSubCommunities:
                                            dAction = ModuleDashboard.ActionType.UnableToUnsubscribeFromCommunities;
                                            View.DisplayUnsubscriptionMessage(new List<String>(), node.GetAllNodes().Where(n => n.AllowUnsubscribe()).Select(n => n.Name).ToList());
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (action)
                                    {
                                        case RemoveAction.FromCommunity:
                                            dAction = ModuleDashboard.ActionType.UnsubscribeFromCommunity;
                                            View.DisplayUnsubscribedFrom(node.Name);
                                            break;
                                        case RemoveAction.FromAllSubCommunities:
                                            dAction = ModuleDashboard.ActionType.UnsubscribeFromCommunities;
                                            View.DisplayUnsubscriptionMessage(node.GetAllNodes().Where(n => n.AllowUnsubscribe() && subscriptions.Where(s=> s.IdCommunity== n.Id && s.IdRole<1).Any()).Select(n => n.Name).ToList(),
                                                node.GetAllNodes().Where(n => n.AllowUnsubscribe() && subscriptions.Where(s => s.IdCommunity == n.Id && s.IdRole >0).Any()).Select(n => n.Name).ToList()
                                                );
                                            break;
                                    }
                                }
                                View.SendUserAction(0, CurrentIdModule, idCommunity, dAction);
                                break;
                        }
                       
                    }
                    else
                    {
                        String name = CurrentManager.GetCommunityName(idCommunity);
                        if (!String.IsNullOrEmpty(name))
                            View.DisplayUnableToUnsubscribe(name);
                        View.SendUserAction(0, CurrentIdModule, idCommunity, ModuleDashboard.ActionType.UnableToUnsubscribe);
                    }
                    LoadCommunities(filters, orderBy, ascending, pageIndex, pageSize);
                }
            }
        #endregion
      
        public void LoadCommunities(DashboardViewType view, UserCurrentSettings userSettings, Int32 pageIndex, Int32 pageSize, OrderItemsBy orderBy, Boolean ascending, Int32 idCommunityType = -1, Int32 idRemoveCommunityType = -1, long idTile = -1, long idTag = -1, dtoTileDisplay tile =null)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (View.DisplayLessCommand)
                {
                    Int32 itemsCount = Service.GetSubscribedCommunitiesCount(UserContext.CurrentUserID, view, idCommunityType, idRemoveCommunityType, idTile, idTag,tile);
                    PagerBase pager = new PagerBase();
                    pager.PageSize = pageSize;//Me.View.CurrentPageSize
                    pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;
                    pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
                    View.Pager = pager;
                }
                List<dtoSubscriptionItem> items = Service.GetSubscribedCommunities(UserContext.CurrentUserID, view, pageIndex, pageSize, orderBy, ascending, idCommunityType, idRemoveCommunityType, idTile, idTag, tile);
                if (items != null)
                {
                    List<Int32> withNews = View.GetIdcommunitiesWithNews(items.Where(i => i.Status != SubscriptionStatus.communityblocked && i.Status != SubscriptionStatus.blocked && i.Status != SubscriptionStatus.waiting).Select(i => i.Community.Id).ToList(), UserContext.CurrentUserID);
                    if (withNews.Any())
                        items.Where(i => withNews.Contains(i.Community.Id)).ToList().ForEach(i => i.HasNews = true);
                    switch (view)
                    {
                        case DashboardViewType.List:
                        case DashboardViewType.Combined:
                            Language l = CurrentManager.GetDefaultLanguage();
                            Dictionary<Int32, List<String>> tags = ServiceTags.GetCommunityAssociationToString(items.Select(i => i.Community.Id).ToList(), UserContext.Language.Id, l.Id, true);
                            if (tags.Any())
                            {
                                foreach (dtoSubscriptionItem item in items.Where(i => tags.ContainsKey(i.Community.Id)))
                                {
                                    item.Community.Tags = tags[item.Community.Id];
                                }
                            }
                            break;
                    }
                   
                    View.LoadItems(items, orderBy, ascending );
                    if (userSettings != null)
                    {
                        userSettings.Ascending = ascending;
                        userSettings.OrderBy = orderBy;
                        userSettings.View = view;
                        View.UpdateUserSettings(userSettings);
                    }
                    //View.SendUserAction(0, CurrentIdModule, ModuleDashboard.ActionType.ListDashboardLoadSubscribedCommunities);
                }
                else
                    View.DisplayErrorFromDB();
            }
        }
        public void ShowMoreCommunities(Boolean value, DashboardViewType view, OrderItemsBy orderBy, Boolean ascending, Int32 idCommunityType = -1, Int32 idRemoveCommunityType = -1, long idTile = -1, long idTag = -1, dtoTileDisplay tile = null)
        {
            View.DisplayLessCommand = value;
            View.DisplayMoreCommand = !value;
            switch (view)
            {
                case DashboardViewType.List:
                    if (value)
                        View.SendUserAction(0, CurrentIdModule, ModuleDashboard.ActionType.ListDashboardMoreComminities);
                    else
                        View.SendUserAction(0, CurrentIdModule, ModuleDashboard.ActionType.ListDashboardLessComminities);
                    break;
                case DashboardViewType.Combined:
                    if (value)
                        View.SendUserAction(0, CurrentIdModule, ModuleDashboard.ActionType.CombinedDashboardMoreCommunities);
                    else
                        View.SendUserAction(0, CurrentIdModule, ModuleDashboard.ActionType.CombinedDashboardLessCommunities);
                    break;
            }
            LoadCommunities(view, null, 0, (value) ? View.DefaultPageSize : View.CurrentPageSize, orderBy, ascending, idCommunityType, idRemoveCommunityType, idTile, idTag, tile);
        }
    }
}