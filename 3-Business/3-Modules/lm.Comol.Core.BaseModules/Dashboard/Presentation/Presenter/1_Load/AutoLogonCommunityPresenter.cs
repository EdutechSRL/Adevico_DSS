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
    public class AutoLogonCommunityPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewAutoLogonCommunity View
            {
                get { return (IViewAutoLogonCommunity)base.View; }
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
            public AutoLogonCommunityPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AutoLogonCommunityPresenter(iApplicationContext oContext, IViewAutoLogonCommunity view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (idCommunity == 0)
                    View.RedirectToPortalHomePage();
                else
                {
                    liteCommunityInfo community = CurrentManager.GetLiteCommunityInfo(idCommunity);
                    if (community == null)
                        View.DisplayUnknownCommunity();
                    else if (community.isClosedByAdministrator)
                        View.DisplayCommunityBlock(community.Name);
                    else
                    {
                        liteSubscriptionInfo subscription = CurrentManager.GetLiteSubscriptionInfo(UserContext.CurrentUserID, idCommunity);
                        if (subscription == null || subscription.IdRole < 0)
                            View.DisplayNotEnrolledIntoCommunity(community.Name);
                        else if (subscription.Accepted && subscription.Enabled)
                            View.LogonToCommunity(idCommunity, UserContext.CurrentUserID);
                        else if (subscription.Accepted && !subscription.Enabled)
                            View.DisplayNoAccessToCommunity(community.Name);
                        else if (!subscription.Accepted && !subscription.Enabled)
                            View.DisplayWaitingActivaction(community.Name);
                    }
                }
            }
        }
    }
}