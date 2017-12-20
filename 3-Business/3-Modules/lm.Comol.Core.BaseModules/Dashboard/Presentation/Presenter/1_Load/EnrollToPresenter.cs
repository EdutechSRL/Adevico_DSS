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
    public class EnrollToPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEnrollToDashboard View
            {
                get { return (IViewEnrollToDashboard)base.View; }
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
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceCommunities
            {
                get
                {
                    if (serviceCommunities == null)
                        serviceCommunities = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(AppContext);
                    return serviceCommunities;
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
            public EnrollToPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EnrollToPresenter(iApplicationContext oContext, IViewEnrollToDashboard view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 preloadIdCommunityType, String searchText, Boolean preloadList)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ////if (UserContext.CurrentCommunityID > 0)
                ////{
                ////    View.GeneratePortalWebContext();
                ////    UserContext.CurrentCommunityID = 0;
                ////    UserContext.CurrentCommunityOrganizationID = 0;
                ////    UserContext.WorkingCommunityID = 0;
                ////}

                liteDashboardSettings settings = Service.DashboardSettingsGet(DashboardType.Portal, 0);
                litePageSettings pSettings = (settings == null) ? null : settings.Pages.Where(p => p.Type == DashboardViewType.Subscribe).FirstOrDefault();
                Int32 itemsForPage = (pSettings==null) ? 25 : pSettings.MaxItems;
                RangeSettings range = (pSettings == null) ? null : pSettings.Range;
                View.EnableFullWidth((settings == null) ? false : settings.FullWidth);
                View.InitializeSubscriptionControl(itemsForPage,range, preloadIdCommunityType, searchText, preloadList);
                //View.SendUserAction(UserContext.CurrentCommunityID, CurrentIdModule, ModuleDashboard.ActionType.e);
            }
        }
    }
}