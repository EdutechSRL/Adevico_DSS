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
    public class CommunityDetailsPagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities serviceCommunities;
            private lm.Comol.Core.Tag.Business.ServiceTags servicetag;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewCommunityDetailsPage View
            {
                get { return (IViewCommunityDetailsPage)base.View; }
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
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleDashboard.UniqueCode);
                    return currentIdModule;
                }
            }
            public CommunityDetailsPagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CommunityDetailsPagePresenter(iApplicationContext oContext, IViewCommunityDetailsPage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity, Boolean fromPage, String url="", Boolean fromSession=false)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (fromSession && idCommunity == 0)
                    idCommunity = UserContext.CurrentCommunityID;
                View.DashboardIdCommunity = idCommunity;
                liteCommunityInfo community = CurrentManager.GetLiteCommunityInfo(idCommunity);
                if (community == null){
                    View.DisplayUnknownCommunity();
                    View.SendUserAction(UserContext.CurrentCommunityID, CurrentIdModule, idCommunity, ModuleDashboard.ActionType.LoadingUnknownCommunityDetails);
                }
                else
                {
                    View.SetTitle(community.Name);
                    View.SendUserAction(UserContext.CurrentCommunityID, CurrentIdModule, idCommunity, ModuleDashboard.ActionType.LoadingCommunityDetails);
                    View.InitializeDetails(community);
                }
            }
        }
    }
}