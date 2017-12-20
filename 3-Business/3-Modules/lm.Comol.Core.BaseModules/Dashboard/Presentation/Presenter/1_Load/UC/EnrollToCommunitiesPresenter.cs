using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Dashboard.Domain;
using lm.Comol.Core.BaseModules.CommunityManagement;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class EnrollToCommunitiesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities service;
            private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEnrollToCommunities View
            {
                get { return (IViewEnrollToCommunities)base.View; }
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
            public EnrollToCommunitiesPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EnrollToCommunitiesPresenter(iApplicationContext oContext, IViewEnrollToCommunities view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        #region "Initalize View"
            public void InitView(Int32 itemsForPage, RangeSettings range, Int32 preloadIdCommunityType, String searchText, Boolean preloadList)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                    InitializeView(itemsForPage, range, preloadIdCommunityType, searchText, preloadList);
            }
            private void InitializeView(Int32 itemsForPage, RangeSettings range, Int32 preloadIdCommunityType, String searchText, Boolean preloadList)
            {
                View.DefaultPageSize = itemsForPage;
                View.DefaultRange = range;
                View.IsInitialized = true;
                View.FirstLoad = true;
                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                if (person != null)
                {
                    List<lm.Comol.Core.DomainModel.Filters.Filter> fToLoad = Service.GetDefaultFilters(UserContext.CurrentUserID, searchText, preloadIdCommunityType, -1,null,null, CommunityAvailability.NotSubscribed).OrderBy(f => f.DisplayOrder).ToList();
                    View.LoadDefaultFilters(fToLoad);
                    if (fToLoad != null && fToLoad.Any())
                    {
                        //lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters = Service.GetDefaultFilters(person, CommunityManagement.CommunityAvailability.NotSubscribed, false, preloadIdCommunityType);
                        lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters = new lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters(fToLoad, CommunityAvailability.NotSubscribed, preloadIdCommunityType);
                        if (preloadIdCommunityType > -1)
                            filters.IdcommunityType = preloadIdCommunityType;
                        View.CurrentFilters = filters;
                        if (preloadList)
                            InternalLoadCommunities(itemsForPage, range, filters, OrderItemsToSubscribeBy.Name, true, false, 0, ModuleDashboard.ActionType.EnrollPageLoadWithCommunities);
                        else
                        {
                            View.CurrentOrderBy = OrderItemsToSubscribeBy.Name;
                            View.CurrentAscending = true;

                            InitializeColumns(filters, new List<dtoEnrollingItem>());
                            View.LoadItems(new List<dtoEnrollingItem>(), OrderItemsToSubscribeBy.Name, true);
                        }
                        View.SendUserAction(0, CurrentIdModule, ModuleDashboard.ActionType.EnrollPageLoad);
                    }
                    SetListTitle(preloadIdCommunityType,(fToLoad != null && fToLoad.Any()));
                }
                else
                    View.DisplaySessionTimeout();
            }
            private void SetListTitle(Int32 idCommunityType, Boolean hasItems)
            {
                liteTile tile = null;
                String name = "";
                if (idCommunityType > -1)
                {
                    tile = Service.GetTileForCommunityType(idCommunityType);
                    if (tile != null)
                        name= tile.GetTitle(UserContext.Language.Id, CurrentManager.GetDefaultIdLanguage());
                }
                View.SetListTitle(name,tile);
                if (!hasItems)
                    View.DisplayNoCommunitiesToEnroll(name);
            }
            private void CalculatePageSize(Int32 itemsCount)
            {
                CalculatePageSize(View.DefaultPageSize, View.DefaultRange, itemsCount);
            }
            private Int32 CalculatePageSize(Int32 itemsForPage, RangeSettings range, Int32 itemsCount)
            {
                Int32 pageSize = itemsForPage;
                if (range != null && range.IsInRange(itemsCount))
                    pageSize = range.DisplayItems;
                return pageSize;
            }
            private List<dtoItemFilter<OrderItemsToSubscribeBy>> GetOrderByItems(Int32 idCommunityType, OrderItemsToSubscribeBy selected = OrderItemsToSubscribeBy.Name)
            {
                List<dtoItemFilter<OrderItemsToSubscribeBy>> items = (from OrderItemsToSubscribeBy e in Enum.GetValues(typeof(OrderItemsToSubscribeBy)).AsQueryable() 
                                                                      where e != OrderItemsToSubscribeBy.DegreeType && e != OrderItemsToSubscribeBy.Year && e != OrderItemsToSubscribeBy.Timespan && e != OrderItemsToSubscribeBy.DegreeType
                                                                      select new dtoItemFilter<OrderItemsToSubscribeBy>() { Value = e, Selected = (e== selected) }).ToList();
                switch(idCommunityType){
                    case (int)CommunityTypeStandard.Degree:
                        items.Add(new dtoItemFilter<OrderItemsToSubscribeBy>() { Value = OrderItemsToSubscribeBy.DegreeType, Selected = (OrderItemsToSubscribeBy.DegreeType== selected) });
                        break;
                    case (int) CommunityTypeStandard.UniversityCourse:
                        items.Add(new dtoItemFilter<OrderItemsToSubscribeBy>() { Value = OrderItemsToSubscribeBy.Year, Selected = (OrderItemsToSubscribeBy.Year== selected) });
                        items.Add(new dtoItemFilter<OrderItemsToSubscribeBy>() { Value = OrderItemsToSubscribeBy.Timespan, Selected = (OrderItemsToSubscribeBy.Timespan== selected) });
                        break;
                }
                if (!items.Where(v => v.Selected).Any() && items.Any())
                    items[0].Selected = true;
                if (items.Count > 1)
                {
                    items.FirstOrDefault().DisplayAs = ItemDisplayOrder.first;
                    items.LastOrDefault().DisplayAs = ItemDisplayOrder.last;
                }
                else if (items.Any())
                    items[0].DisplayAs = ItemDisplayOrder.first | ItemDisplayOrder.last;
                return items;
            }
            private Boolean GetDefaultAscending(OrderItemsToSubscribeBy orderBy)
            {
                switch (orderBy)
                {
                    case OrderItemsToSubscribeBy.Year:
                        return false;
                    case OrderItemsToSubscribeBy.SubscriptionClosedOn:
                        return true;
                    case OrderItemsToSubscribeBy.SubscriptionOpenOn:
                        return true;
                    case OrderItemsToSubscribeBy.MaxUsers:
                        return false;
                    case OrderItemsToSubscribeBy.Timespan:
                        return true;
                }
                return true;
            }
            private void InitializeColumns(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, List<dtoEnrollingItem> items)
            {
                List<searchColumn> columns = new List<searchColumn>();
                columns.Add(searchColumn.info);
                columns.Add(searchColumn.name);
                if (items.Where(i => i.Community.SubscriptionStartOn.HasValue).Any())
                    columns.Add(searchColumn.startsubscriptionon);
                if (items.Where(i=> i.Community.SubscriptionEndOn.HasValue).Any())
                    columns.Add(searchColumn.endsubscriptionon);
                if (items != null && items.Where(i=> i.AllowSubscribe).Any()){
                    columns.Add(searchColumn.actions);
                    if (items.Where(i=> i.AllowSubscribe).Count()>0)
                        columns.Add(searchColumn.select);
                }
                switch(filters.IdcommunityType){
                    case (int)CommunityTypeStandard.Degree:
                        if (filters.IdDegreeType < 1)
                            columns.Add(searchColumn.degreetype);
                        break;
                    case (int)CommunityTypeStandard.UniversityCourse:
                        if (filters.Year < 1)
                            columns.Add(searchColumn.year);
                        if (filters.IdCourseTime < 1)
                            columns.Add(searchColumn.coursetime);
                        break;
                }
                //if (filters.IdResponsible<1)
                    columns.Add(searchColumn.owner);
                if (items != null && items.Where(i => i.Community.MaxUsersWithDefaultRole > 0).Any())
                    columns.Add(searchColumn.maxsubscribers);
                View.AvailableColumns = columns;
            }
            private void InternalLoadCommunities(Int32 itemsForPage, RangeSettings range, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsToSubscribeBy orderBy, Boolean ascending, Boolean useCache, Int32 pageIndex, ModuleDashboard.ActionType action = ModuleDashboard.ActionType.EnrollPageApplyFilters, Boolean applyFilters = false)
            {
                View.CurrentFilters = filters;
                List<dtoItemFilter<OrderItemsToSubscribeBy>> orderItems = GetOrderByItems(filters.IdcommunityType, orderBy);
                View.InitializeOrderBySelector(orderItems);
                List<dtoEnrollingItem> items = Service.GetCommunitiesToEnroll(UserContext.CurrentUserID, filters, useCache);
                Int32 itemsCount = (items == null) ? 0 : items.Count;
                Int32 pageSize = CalculatePageSize(itemsForPage, range, itemsCount);
                View.CurrentPageSize = pageSize;
                InitializeColumns(filters, items);

                if (items == null)
                {
                    View.DisplayErrorFromDB();
                    View.CurrentOrderBy = orderBy;
                    View.CurrentAscending = ascending;
                }
                else
                {
                    if (pageSize == 0)
                        pageSize = View.DefaultPageSize;
                    PagerBase pager = new PagerBase();
                    pager.PageSize = pageSize;
                    pager.Count = (itemsCount > 0) ? itemsCount - 1 : 0;
                    pager.PageIndex = pageIndex;
                    View.Pager = pager;
                    List<dtoCommunityToEnroll> cItems = null;
                    if (applyFilters)
                        cItems = new List<dtoCommunityToEnroll>();
                    else
                    {
                        cItems = View.CurrentSelectedItems;
                        List<dtoCommunityToEnroll> sItems = View.GetSelectedItems();
                        if (sItems.Where(i => !i.Selected).Any())
                            cItems = cItems.Where(c => !sItems.Where(s => s.Id == c.Id).Any() || sItems.Where(s => s.Selected && s.Id == c.Id).Any()).ToList();
                        if (sItems.Where(i => i.Selected).Any())
                        {
                            List<Int32> idCommunities = cItems.Select(c => c.Id).ToList();
                            cItems.AddRange(sItems.Where(s => s.Selected && !idCommunities.Contains(s.Id)).ToList());
                        }
                    }
                    View.CurrentSelectedItems = cItems;
                    View.KeepOpenBulkActions = (cItems.Count>0);
                    View.InitializeBulkActions(itemsCount > pageSize, cItems);
                    items = Service.GetCommunities(UserContext.CurrentUserID, items, pageIndex, pageSize, orderBy, ascending);

                    if (items != null)
                    {
                        Language l = CurrentManager.GetDefaultLanguage();
                        Dictionary<Int32, List<String>> tags = ServiceTags.GetCommunityAssociationToString(items.Select(i => i.Community.Id).ToList(), UserContext.Language.Id, l.Id, true);
                        if (tags.Any())
                        {
                            foreach (dtoEnrollingItem item in items.Where(i => tags.ContainsKey(i.Community.Id)))
                            {
                                item.Community.Tags = tags[item.Community.Id];
                            }
                        }

                        View.LoadItems(items, orderBy, ascending);
                        //View.SendUserAction(0, CurrentIdModule, action);
                    }
                    else
                        View.LoadItems(new List<dtoEnrollingItem>(), orderBy, ascending);
                }
            }
        #endregion

        public void ApplyFilters(Int32 itemsForPage, RangeSettings range, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsToSubscribeBy orderBy)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                InternalLoadCommunities(itemsForPage, range, filters, orderBy, GetDefaultAscending(orderBy), false, 0, ModuleDashboard.ActionType.EnrollPageApplyFilters,true);

                View.CurrentFilters = filters;
                liteTile tile = null;
                if (filters.IdcommunityType > -1)
                    tile = Service.GetTileForCommunityType(filters.IdcommunityType);
            }
        }
        public void LoadCommunities(Int32 itemsForPage, RangeSettings range, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsToSubscribeBy orderBy, Boolean ascending, Int32 pageIndex)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                InternalLoadCommunities(itemsForPage, range, filters, orderBy, ascending, true, pageIndex, ModuleDashboard.ActionType.EnrollPageChangePageIndex);
        }
        public void EnrollTo(Int32 idCommunity,String name, String path, Int32 itemsForPage, RangeSettings range, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsToSubscribeBy orderBy, Boolean ascending, Int32 pageIndex)
        {
            Int32 idPerson = UserContext.CurrentUserID;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean reloadCommunities = false;
                ModuleDashboard.ActionType dAction = ModuleDashboard.ActionType.None;
                dtoCommunityInfoForEnroll item = Service.GetEnrollingItem(idPerson, idCommunity, path);
                litePerson person = CurrentManager.GetLitePerson(idPerson);
                if (item != null && item.Id > 0 && person != null && person.Id>0)
                {
                    if (!item.AllowEnroll)
                    {
                        dAction = ModuleDashboard.ActionType.EnrollNotAllowed;
                        View.DisplayEnrollMessage(item, dAction);
                    }
                    else
                    {
                        if (!item.HasConstraints && item.AllowUnsubscribe)
                        {
                            dtoEnrollment enrollment = Service.EnrollTo(idPerson, item);
                            if (enrollment == null)
                            {
                                dAction = ModuleDashboard.ActionType.UnableToEnroll;
                                View.DisplayEnrollMessage(item, dAction);
                            }
                            else
                            {
                                View.DisplayEnrollMessage(enrollment, enrollment.IdCommunity, person, Service.GetTranslatedProfileType(person),CurrentManager.GetUserDefaultOrganizationName(idPerson));
                                UpdateSelectedItems(new List<Int32>() { enrollment.IdCommunity });
                                reloadCommunities = true;
                                dAction = (enrollment.Status== EnrolledStatus.NeedConfirm) ? ModuleDashboard.ActionType.EnrollToCommunityWaitingConfirm : ModuleDashboard.ActionType.EnrollToCommunity;
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(idPerson));
                            }
                        }
                        else
                        {
                            dAction = ModuleDashboard.ActionType.RequireEnrollConfirm;
                            View.DisplayConfirmMessage(item);
                        }
                    }
                }
                else
                {
                   dAction = ModuleDashboard.ActionType.UnableToEnroll;
                   View.DisplayUnknownCommunity(name); 
                }
                View.SendUserAction(0, CurrentIdModule, idCommunity, dAction);
                if (reloadCommunities)
                    InternalLoadCommunities(itemsForPage, range, filters, orderBy, ascending,true, pageIndex, ModuleDashboard.ActionType.EnrollPageLoadcommunitiesAfterEnrolling);
            }
        }
        public void EnrollTo(List<dtoCommunityToEnroll> sCommunities , Boolean applyToAll, Int32 itemsForPage, RangeSettings range, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsToSubscribeBy orderBy, Boolean ascending, Int32 pageIndex) {
            Int32 idPerson = UserContext.CurrentUserID;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType dAction = ModuleDashboard.ActionType.None;
                Boolean reloadCommunities = false;

                List<dtoCommunityToEnroll> cCommunities = View.CurrentSelectedItems;
                if (sCommunities.Where(i => !i.Selected).Any())
                    cCommunities = cCommunities.Where(c => !sCommunities.Where(s => s.Id == c.Id).Any() || sCommunities.Where(s => s.Selected && s.Id == c.Id).Any()).ToList();
                if (sCommunities.Where(i => i.Selected).Any())
                {
                    List<Int32> idCommunities = cCommunities.Select(c => c.Id).ToList();
                    cCommunities.AddRange(sCommunities.Where(s => s.Selected && !idCommunities.Contains(s.Id)).ToList());
                }

                List<dtoCommunityInfoForEnroll> items = Service.GetEnrollingItems(idPerson, cCommunities);
                if (items != null && items.Where(i=> i.Id>0).Any())
                {
                    if (!items.Where(i=> i.AllowEnroll).Any())
                    {
                        dAction = ModuleDashboard.ActionType.EnrollNotAllowed;
                        if (items.Count == 1)
                            View.DisplayEnrollMessage(items.FirstOrDefault(), dAction);
                        else
                            View.DisplayEnrollMessage(items.Count, dAction);
                    }
                    else
                    {
                        reloadCommunities = true;
                        litePerson person = CurrentManager.GetLitePerson(idPerson);
                        String profileType = Service.GetTranslatedProfileType(person);
                        String organizationName = CurrentManager.GetUserDefaultOrganizationName(idPerson);
                        List<dtoEnrollment> enrollments = new List<dtoEnrollment>();
                        foreach (dtoCommunityInfoForEnroll cm in items.Where(i => i.AllowEnroll && !i.HasConstraints))
                        {
                            dtoEnrollment enrollment = Service.EnrollTo(idPerson, cm);
                            if (enrollment != null)
                                enrollments.Add(enrollment);
                        }
                        if (enrollments.Where(e=> e.Status== EnrolledStatus.Available || e.Status== EnrolledStatus.NeedConfirm).Any()){
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(idPerson));
                            enrollments.Where(e=> e.Status== EnrolledStatus.Available || e.Status== EnrolledStatus.NeedConfirm).ToList().ForEach(e => View.NotifyEnrollment(e, person, profileType, organizationName));
                        }
                        if ((enrollments.Any() && enrollments.Where(s=> s.Status== EnrolledStatus.NotAvailable || s.Status== EnrolledStatus.UnableToEnroll).Any())
                            || items.Where(i => i.HasConstraints || !i.AllowUnsubscribe).Any())
                        {
                            UpdateSelectedItems(enrollments.Where(e => e.Status == EnrolledStatus.Available || e.Status == EnrolledStatus.NeedConfirm || e.Status == EnrolledStatus.PreviousEnrolled || e.Status == EnrolledStatus.UnableToEnroll).Select(e => e.IdCommunity).ToList());
                            View.DisplayConfirmMessage(enrollments.Where(e=> e.Status!= EnrolledStatus.NotAvailable && e.Status != EnrolledStatus.UnableToEnroll).ToList() ,
                                enrollments.Where(e => e.Status == EnrolledStatus.NotAvailable || e.Status == EnrolledStatus.UnableToEnroll).ToList(),
                                items.Where(i => !enrollments.Where(e => e.IdCommunity == i.Id).Any()).ToList(), person, profileType, organizationName);
                             View.SendUserAction(0, CurrentIdModule, items.Where(i => !enrollments.Where(e => e.IdCommunity == i.Id).Any()).Select(c=>c.Id).ToList(), ModuleDashboard.ActionType.RequireEnrollConfirm);
                        }
                        else
                        {
                            if (enrollments.Where(e=> e.Status== EnrolledStatus.Available).Any())
                                View.SendUserAction(0, CurrentIdModule, enrollments.Where(e => e.Status == EnrolledStatus.Available).Select(c => c.IdCommunity).ToList(), ModuleDashboard.ActionType.EnrollToCommunities);
                            if (enrollments.Where(e => e.Status == EnrolledStatus.NeedConfirm).Any())
                                View.SendUserAction(0, CurrentIdModule, enrollments.Where(e => e.Status == EnrolledStatus.NeedConfirm).Select(c => c.IdCommunity).ToList(), ModuleDashboard.ActionType.EnrollToCommunitiesWaitingConfirm);
                            UpdateSelectedItems(enrollments.Where(e => e.Status == EnrolledStatus.Available || e.Status == EnrolledStatus.NeedConfirm || e.Status == EnrolledStatus.PreviousEnrolled || e.Status == EnrolledStatus.UnableToEnroll).Select(e => e.IdCommunity).ToList());
                            if (enrollments.Any() && enrollments.Count == items.Count)
                                View.DisplayEnrollMessage(enrollments);
                            else
                                View.DisplayEnrollMessage(enrollments, items.Where(i=> !enrollments.Where(e=> e.IdCommunity == i.Id).Any()).Select(c=> c.Name).ToList());
                        }
                    }
                }
                else
                    View.SendUserAction(0, CurrentIdModule, sCommunities.Select(c=>c.Id).ToList(), ModuleDashboard.ActionType.UnableToEnroll);
                if (reloadCommunities)
                    InternalLoadCommunities(itemsForPage, range, filters, orderBy, ascending, true, pageIndex, ModuleDashboard.ActionType.EnrollPageLoadcommunitiesAfterEnrolling);
            }
        }
        public void EnrollTo(List<dtoCommunityInfoForEnroll> sCommunities, Int32 itemsForPage, RangeSettings range, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters filters, OrderItemsToSubscribeBy orderBy, Boolean ascending, Int32 pageIndex)
        {
            Int32 idPerson = UserContext.CurrentUserID;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean reloadCommunities = false;
                if (sCommunities.Any())
                {
                    reloadCommunities = true;
                    litePerson person = CurrentManager.GetLitePerson(idPerson);
                    String profileType = Service.GetTranslatedProfileType(person);
                    String organizationName = CurrentManager.GetUserDefaultOrganizationName(idPerson);
                    List<dtoEnrollment> enrollments = new List<dtoEnrollment>();
                    foreach (dtoCommunityInfoForEnroll cm in sCommunities)
                    {
                        dtoEnrollment enrollment = Service.EnrollTo(idPerson, cm);
                        if (enrollment != null)
                            enrollments.Add(enrollment);
                    }
                    if (enrollments.Any())
                    {
                        if (enrollments.Where(e => e.Status == EnrolledStatus.Available || e.Status == EnrolledStatus.NeedConfirm).Any())
                        {
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(idPerson));
                            enrollments.Where(e => e.Status == EnrolledStatus.Available || e.Status == EnrolledStatus.NeedConfirm).ToList().ForEach(e => View.NotifyEnrollment(e, person, profileType, organizationName));
                            reloadCommunities = true;
                        }

                        if (enrollments.Where(e => e.Status == EnrolledStatus.Available).Any())
                            View.SendUserAction(0, CurrentIdModule, enrollments.Where(e => e.Status == EnrolledStatus.Available).Select(c => c.IdCommunity).ToList(), ModuleDashboard.ActionType.EnrollToCommunities);
                        if (enrollments.Where(e => e.Status == EnrolledStatus.NeedConfirm).Any())
                            View.SendUserAction(0, CurrentIdModule, enrollments.Where(e => e.Status == EnrolledStatus.NeedConfirm).Select(c => c.IdCommunity).ToList(), ModuleDashboard.ActionType.EnrollToCommunitiesWaitingConfirm);
                        UpdateSelectedItems(enrollments.Where(e => e.Status == EnrolledStatus.Available || e.Status == EnrolledStatus.NeedConfirm || e.Status == EnrolledStatus.PreviousEnrolled || e.Status == EnrolledStatus.UnableToEnroll).Select(e => e.IdCommunity).ToList());
                        View.DisplayEnrollMessage(enrollments);
                    }
                    if (reloadCommunities)
                        InternalLoadCommunities(itemsForPage, range, filters, orderBy, ascending, true, pageIndex, ModuleDashboard.ActionType.EnrollPageLoadcommunitiesAfterEnrolling, true);
                }
                else
                    View.SendUserAction(0, CurrentIdModule,  ModuleDashboard.ActionType.NoSelectedCommunitiesToEnroll);
            }
        }
        private void UpdateSelectedItems(List<Int32> idCommunities)
        {
            List<dtoCommunityToEnroll> cItems = View.CurrentSelectedItems;

            cItems = cItems.Where(c => !idCommunities.Contains(c.Id)).ToList();
            View.CurrentSelectedItems = cItems;
            if (!cItems.Any())
                View.KeepOpenBulkActions = false;

            View.RemoveFromSelectedItems(idCommunities);
        }
    }
}