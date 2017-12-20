using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Business;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class PortalDashboardTopBarPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewPortalDashboardTopBar View
            {
                get { return (IViewPortalDashboardTopBar)base.View; }
            }
            private ServiceDashboard Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceDashboard(AppContext);
                    return service;
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
            public PortalDashboardTopBarPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PortalDashboardTopBarPresenter(iApplicationContext oContext, IViewPortalDashboardTopBar view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(DashboardViewType cView, liteDashboardSettings settings, UserCurrentSettings userSettings,Boolean moreTiles)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.InitializeSearch(settings.Container.Default.Search);

                LoadViews(cView, settings, userSettings, moreTiles);

                if (cView != DashboardViewType.Search)
                    LoadGroupBy(cView, settings, userSettings);
            }
        }

        private void LoadViews(DashboardViewType sView, liteDashboardSettings settings, UserCurrentSettings userSettings, Boolean moreTiles)
        {
            List<dtoItemFilter<DashboardViewType>> views = settings.Container.AvailableViews.Where(v => v != DashboardViewType.Search && v != DashboardViewType.Subscribe).Select(v => new dtoItemFilter<DashboardViewType>() { Value = v, Selected = (v == sView) }).ToList();
            if (!views.Where(v => v.Selected).Any())
                views.Add(new dtoItemFilter<DashboardViewType>() { Selected= true, Value= sView });
            if (views.Count > 1)
            {
                views.FirstOrDefault().DisplayAs = ItemDisplayOrder.first;
                views.LastOrDefault().DisplayAs = ItemDisplayOrder.last;
            }
            else if (views.Any())
                views[0].DisplayAs = ItemDisplayOrder.first | ItemDisplayOrder.last;
            foreach (dtoItemFilter<DashboardViewType> item in views)
            {
                item.Url = RootObject.LoadPortalView(UserContext.CurrentUserID,item.Value, userSettings.GroupBy, userSettings.OrderBy, userSettings.GetNoticeboard(item.Value), -1, -1, true, moreTiles);
//                item.Url = RootObject.LoadPortalView(item.Value, (item.Value == DashboardViewType.List) ? GroupItemsBy.None : userSettings.GroupBy, userSettings.OrderBy, userSettings.GetNoticeboard(item.Value), -1, -1, true, moreTiles);
            }
            View.InitializeViewSelector(views);
        }
        private void LoadGroupBy(DashboardViewType sView, liteDashboardSettings settings, UserCurrentSettings userSettings)
        {
            switch (sView)
            {
                case DashboardViewType.List:
                    View.SelectedGroupBy = userSettings.GroupBy;
                    break;
                default:
                    List<dtoItemFilter<GroupItemsBy>> items = settings.Container.AvailableGroupBy.Select(v => new dtoItemFilter<GroupItemsBy>() { Value = v, Selected = (v == userSettings.GroupBy) }).ToList();
                    if (items.Any() && !items.Where(i=> i.Selected).Any()){
                        if (items.Where(i => i.Value != GroupItemsBy.None).Any())
                            items.Where(i => i.Value != GroupItemsBy.None).FirstOrDefault().Selected = true;
                        else
                            items.FirstOrDefault().Selected = true;
                    }

                    if (items.Count > 1)
                    {
                        items.FirstOrDefault().DisplayAs = ItemDisplayOrder.first;
                        items.LastOrDefault().DisplayAs = ItemDisplayOrder.last;
                    }
                    else if (items.Any())
                        items[0].DisplayAs = ItemDisplayOrder.first | ItemDisplayOrder.last;
                    foreach (dtoItemFilter<GroupItemsBy> item in items)
                    {
                        item.Url = RootObject.LoadPortalView(UserContext.CurrentUserID, sView, (sView == DashboardViewType.List) ? GroupItemsBy.None : item.Value, userSettings.OrderBy, userSettings.GetNoticeboard(sView));
                    }
                    View.InitializeGroupBySelector(items, (items.Where(i => i.Selected).Any()? items.Where(i => i.Selected).Select(i=>i.Value).FirstOrDefault() : GroupItemsBy.None));
                    break;
            }
        }
    }
}