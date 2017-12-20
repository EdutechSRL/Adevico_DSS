using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.NoticeBoard.Business;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public class NoticeboardRenderPagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceNoticeBoard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewNoticeboardRenderPage View
            {
                get { return (IViewNoticeboardRenderPage)base.View; }
            }
            private ServiceNoticeBoard Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceNoticeBoard(AppContext);
                    return service;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleNoticeboard.UniqueID);
                    return currentIdModule;
                }
            }
            public NoticeboardRenderPagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public NoticeboardRenderPagePresenter(iApplicationContext oContext, IViewNoticeboardRenderPage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity, long idMessage =0)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                NoticeBoard.Domain.ModuleNoticeboard permissions = Service.GetPermissions(idCommunity);
                if (permissions.Administration || permissions.ViewCurrentMessage || permissions.ViewOldMessage)
                    View.InitializeControl(idCommunity, permissions, idMessage);
                else
                    View.DisplayNoPermission(idCommunity, CurrentIdModule);
            }
        }
    }
}