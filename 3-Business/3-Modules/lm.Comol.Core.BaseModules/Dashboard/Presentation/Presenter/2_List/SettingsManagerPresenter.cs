using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public class SettingsManagerPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private Dashboard.Business.ServiceDashboardCommunities service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewBaseSettingsList View
            {
                get { return (IViewBaseSettingsList)base.View; }
            }
            private Dashboard.Business.ServiceDashboardCommunities Service
            {
                get
                {
                    if (service == null)
                        service = new Dashboard.Business.ServiceDashboardCommunities(AppContext);
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
            public SettingsManagerPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SettingsManagerPresenter(iApplicationContext oContext, IViewBaseSettingsList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(DashboardType type, Boolean loadFromRecycleBin, Int32 idCommunity)
        {
            Person p = CurrentManager.GetPerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || p == null)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard permissions = ModuleDashboard.CreatePortalmodule(p.TypeID);
                if (type == DashboardType.Community)
                {
                    idCommunity = UserContext.CurrentCommunityID;
                    if (idCommunity<0)
                        idCommunity = UserContext.CurrentCommunityID;
                    if (idCommunity > 0 && (!permissions.Administration && !permissions.ManageTiles))
                        permissions = Service.GetPermission(idCommunity);
                }
                else
                    idCommunity = 0;
                View.IdContainerCommunity  = idCommunity;
                View.SetTitle(type, (idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : "");
                if (permissions.Administration || permissions.List || permissions.Edit)
                {
                    if (loadFromRecycleBin)
                        View.SetBackUrl(RootObject.DashboardList(type, false, idCommunity));
                    else {
                        View.SetRecycleUrl(RootObject.DashboardList(type, true, idCommunity));
                        View.SetAddUrl(RootObject.DashboardAdd(type, idCommunity));
                    }
                    View.InitializeListControl(permissions, type, idCommunity, loadFromRecycleBin);
                }
                else
                    View.DisplayNoPermission(idCommunity, CurrentIdModule);
            }
        }
    }
}