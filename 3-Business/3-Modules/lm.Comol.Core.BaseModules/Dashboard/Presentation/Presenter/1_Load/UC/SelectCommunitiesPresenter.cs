using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Communities;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class SelectCommunitiesPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            //private ServiceCommunityManagement _Service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSelectCommunities View
            {
                get { return (IViewSelectCommunities)base.View; }
            }
            //private ServiceCommunityManagement Service
            //{
            //    get
            //    {
            //        if (_Service == null)
            //            _Service = new ServiceCommunityManagement(AppContext);
            //        return _Service;
            //    }
            //}
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceCommunities
            {
                get
                {
                    if (serviceCommunities == null)
                        serviceCommunities = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return serviceCommunities;
                }
            }
            public SelectCommunitiesPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SelectCommunitiesPresenter(iApplicationContext oContext, IViewSelectCommunities view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(List<dtoCommunityPlainItem> items, List<Int32> unloadIdCommunities)
            {
                InitView(items,unloadIdCommunities, new List<Int32>(),null, null);
            }
            public void InitView(List<dtoCommunityPlainItem> items, List<Int32> unloadIdCommunities, List<Int32> onlyFromOrganizations)
            {
                InitView(items, unloadIdCommunities, onlyFromOrganizations, null, null);
            }
            public void InitView(List<dtoCommunityPlainItem> items, List<Int32> unloadIdCommunities, List<dtoModulePermission> andClause)
            {
                InitView(items, unloadIdCommunities, new List<Int32>(), andClause, null);
            }
            public void InitView(List<dtoCommunityPlainItem> items, List<Int32> unloadIdCommunities, List<Int32> onlyFromOrganizations, List<dtoModulePermission> andClause, List<dtoModulePermission> orClause)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    List<Int32> removeFromAdding = new List<Int32>();
                    removeFromAdding = unloadIdCommunities;
                    View.NotLoadIdCommunities = unloadIdCommunities;
                    View.OnlyFromOrganizations = onlyFromOrganizations;
                    if (items == null || items.Count == 0)
                        View.NoItems();
                    else
                    {
                        removeFromAdding.AddRange(items.Select(n => n.Community.Id).ToList());
                        View.LoadItems(items.OrderBy(c => c.Community.Name).ToList());
                    }
                    if (andClause == null && orClause == null)
                        View.InitializeAddControl(UserContext.CurrentUserID,removeFromAdding, onlyFromOrganizations);
                    else
                        View.InitializeAddControlByService(UserContext.CurrentUserID, removeFromAdding, andClause, orClause);

                    View.AllowAdd = View.HasAvailableCommunitiesToAdd;
                    View.isInitialized = true;
                }
            }

            public void AddCommunities(List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> toAdd)
            {
                List<lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem> cItems = View.SelectedCommunities;
                cItems.AddRange(toAdd.Where(c => !cItems.Any(i => i.Community.Id == c.Community.Id)).ToList());
                View.LoadItems(cItems.OrderBy(c => c.Community.Name).ToList());
            }

            public void RemoveCommunity(Int32 idCommunity)
            {
                List<dtoCommunityPlainItem> current = View.SelectedCommunities;
                current = current.Where(c => c.Community.Id != idCommunity).ToList();
                View.SelectedCommunities = current;
                if (current.Count==0)
                    View.NoItems();
                else
                    View.LoadItems(current.OrderBy(c => c.Community.Name).ToList());
            }
            public void UpdateSelectedCommunities(List<Int32> idCommunities)
            {
                List<dtoCommunityPlainItem> current = View.SelectedCommunities;
                current = current.Where(c => idCommunities.Contains(c.Community.Id)).ToList();
                View.SelectedCommunities = current;
                if (current.Count == 0)
                    View.NoItems();
                else
                    View.LoadItems(current.OrderBy(c => c.Community.Name).ToList());
            }
    }
}