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
    public class ViewTreePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewViewTree View
            {
                get { return (IViewViewTree)base.View; }
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
            public ViewTreePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewTreePresenter(iApplicationContext oContext, IViewViewTree view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean advanced, Int32 idCommunity = 0, Boolean fromPage=false, String url = "", Boolean fromSession = false)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = (idCommunity == 0) ? ModuleDashboard.ActionType.TreePageLoading : ModuleDashboard.ActionType.TreePageLoadingFromCommunity;
                if (fromSession && idCommunity == 0)
                    idCommunity = UserContext.CurrentCommunityID;
                View.DashboardIdCommunity = idCommunity;
                String name = (idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : "";
                if (idCommunity>0 && String.IsNullOrEmpty(name))
                {
                    View.DisplayUnknownCommunity();
                    View.SendUserAction(UserContext.CurrentCommunityID, CurrentIdModule, idCommunity, ModuleDashboard.ActionType.TreePageLoadingFromUnknownCommunity);
                }
                else
                {
                    View.SetTitle(name);
                    View.LoadTree(advanced, idCommunity);
                    View.SendUserAction(UserContext.CurrentCommunityID, CurrentIdModule, idCommunity, action);
                }
            }
        }
    }
}