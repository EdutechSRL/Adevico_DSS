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
    public class NoticeboardBlockPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            private lm.Comol.Core.BaseModules.NoticeBoard.Business.ServiceNoticeBoard servicenoticeboard;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewNoticeboardBlock View
            {
                get { return (IViewNoticeboardBlock)base.View; }
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
            public NoticeboardBlockPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public NoticeboardBlockPresenter(iApplicationContext oContext, IViewNoticeboardBlock view)
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
                NoticeBoard.Domain.ModuleNoticeboard permissions = ServiceNoticeboard.GetPermissions((idCommunity > 0) ? idCommunity : 0);
                if (permissions.Administration || permissions.ViewCurrentMessage)
                    View.LoadMessage(ServiceNoticeboard.GetLastMessageId((idCommunity > 0) ? idCommunity : 0), idCommunity);
                else
                    View.LoadMessage(0, idCommunity);
                if (permissions.Administration || permissions.DeleteMessage || permissions.EditMessage)
                    View.DisplayEdit(NoticeBoard.Domain.RootObject.NoticeboardDashboard(0, idCommunity, (idCommunity < 1), true));
                View.AllowPrint = permissions.Administration || permissions.ViewCurrentMessage || permissions.PrintMessage;
            }
        }
    }
}