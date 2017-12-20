using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.NoticeBoard.Business;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.Editor;
using lm.Comol.Core.BaseModules.Editor.Business;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public class ViewNoticeboardMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Init / Default"
            private int _ModuleID;
            private ServiceNoticeBoard _Service;
            private int ModuleID
            {
                get
                {
                    if (_ModuleID <= 0)
                    {
                        _ModuleID = this.Service.ServiceModuleID();
                    }
                    return _ModuleID;
                }
            }

            protected virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewViewNoticeboardMessage View
            {
                get { return (IViewViewNoticeboardMessage)base.View; }
            }
            private ServiceNoticeBoard Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceNoticeBoard(AppContext);
                    return _Service;
                }
            }
        #endregion

        public ViewNoticeboardMessagePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public ViewNoticeboardMessagePresenter(iApplicationContext oContext, IViewViewNoticeboardMessage view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView(long idMessage, Int32 idCommunity){
            View.IdLoaderCommunity = 0;
            if (IsSessionTimeout())
                return;

            View.IdLoaderCommunity = UserContext.CurrentCommunityID;
            NoticeboardMessage message = null;

            if (idMessage != 0)
                message = Service.GetMessage(idMessage);

            Boolean isForPortal = (message != null && message.isForPortal) || (message == null && idCommunity==0);
            View.SetHeaderTitle(isForPortal, (isForPortal) ? View.PortalName : CurrentManager.GetCommunityName(idCommunity));
            if (message == null && idMessage > 0)
                View.SendUserAction(idCommunity, ModuleID, idMessage, ModuleNoticeboard.ActionType.ViewUnknownMessage);
            else if (message == null && idMessage == 0)
                View.SendUserAction(idCommunity, ModuleID, idMessage, ModuleNoticeboard.ActionType.ViewEmptyMessage);
            else
            {
                ModuleNoticeboard module = null;
                if (message.isForPortal)
                    module = ModuleNoticeboard.CreatePortalmodule((UserContext.isAnonymous) ? (int)UserTypeStandard.Guest : UserContext.UserTypeID);
                else if (idCommunity > 0)
                    module = new ModuleNoticeboard(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, ModuleID));
                else
                    module = new ModuleNoticeboard();
                if (module.Administration || module.ViewCurrentMessage || module.ViewOldMessage)
                {
                    View.DisplayMessage(idMessage, idCommunity);
                    View.SendUserAction(idCommunity, ModuleID, idMessage, ModuleNoticeboard.ActionType.ViewMessage);
                }
                else
                    View.SendUserAction(idCommunity, ModuleID, idMessage, ModuleNoticeboard.ActionType.NoPermission);
                View.AllowPrint = module.Administration || module.ViewCurrentMessage || module.PrintMessage;
            }
        }

        private Boolean IsSessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return true;
            }
            return false;
        }
    }
}
