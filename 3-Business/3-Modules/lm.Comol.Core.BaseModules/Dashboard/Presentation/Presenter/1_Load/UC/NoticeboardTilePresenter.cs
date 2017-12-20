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
    public class NoticeboardTilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            private lm.Comol.Core.BaseModules.NoticeBoard.Business.ServiceNoticeBoard servicenoticeboard;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewNoticeboardTile View
            {
                get { return (IViewNoticeboardTile)base.View; }
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
            private lm.Comol.Core.BaseModules.NoticeBoard.Business.ServiceNoticeBoard ServiceNoticeboard
            {
                get
                {
                    if (servicenoticeboard == null)
                        servicenoticeboard = new lm.Comol.Core.BaseModules.NoticeBoard.Business.ServiceNoticeBoard(AppContext);
                    return servicenoticeboard;
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
            public NoticeboardTilePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public NoticeboardTilePresenter(iApplicationContext oContext, IViewNoticeboardTile view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity)
        {
            litePerson p = CurrentManager.Get<litePerson>(UserContext.CurrentUserID);
            if (UserContext.isAnonymous && p != null )
                View.DisplaySessionTimeout();
            else
            {
                NoticeBoard.Domain.ModuleNoticeboard permissions = ServiceNoticeboard.GetPermissions((idCommunity > 0) ? idCommunity : 0);
                lm.Comol.Core.BaseModules.NoticeBoard.Domain.liteHistoryItem message =  ServiceNoticeboard.GetLastHistoryItem(idCommunity);
                if (permissions.Administration || permissions.ViewCurrentMessage){
                    if (message == null)
                        View.LoadNoMessage();
                    else { 
                        if (idCommunity == 0)
                            View.LoadMessage(message,p);
                        else {
                            liteSubscriptionInfo s = Service.GetSubscriptionInfo(p.Id, idCommunity);
                            if (s==null)
                                View.LoadNoPermissionsToSeeMessage();
                            else
                                View.LoadMessage(message,idCommunity, s);
                        }
                    }
                }                   
                else
                    View.LoadNoPermissionsToSeeMessage();
            }
        }
    }
}