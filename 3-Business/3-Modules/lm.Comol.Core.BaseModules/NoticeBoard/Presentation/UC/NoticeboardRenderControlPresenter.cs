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
    public class NoticeboardRenderControlPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceNoticeBoard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewNoticeboardRenderControl View
            {
                get { return (IViewNoticeboardRenderControl)base.View; }
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
            public NoticeboardRenderControlPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public NoticeboardRenderControlPresenter(iApplicationContext oContext, IViewNoticeboardRenderControl view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity,ModuleNoticeboard permissions = null, long idMessage =0)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Int32 pageIndex = Service.GetHistoryPageIndex(idCommunity, idMessage, View.HistoryPageSize, Service.GetAvailableStatus(permissions, View.IsForManagement));
                View.HistoryPageIndex = pageIndex;
                View.ContainerIdCommunity = idCommunity;
                if (permissions==null)
                    permissions = Service.GetPermissions(idCommunity);
                if (permissions.Administration || permissions.ViewCurrentMessage || permissions.ViewOldMessage)
                {
                    long idLastMessage = (idMessage == 0) ? Service.GetLastMessageId(idCommunity) : idMessage;
                    View.IdCurrentMessage = idLastMessage;
                    View.LoadMessage(idLastMessage, idCommunity);
                    if (View.DisplayHistory)
                        LoadHistoryItems(pageIndex, idCommunity, permissions, idLastMessage);
                }
                else
                {
                    View.IdCurrentMessage = 0;
                    View.LoadMessage(0, idCommunity);
                    View.HasHistory = false;
                }
                if (!View.IsForManagement)
                    View.InitalizeCommands((permissions.Administration || permissions.ViewCurrentMessage || permissions.PrintMessage));
            }
        }

        public void LoadMessage(long idMessage, Int32 idCommunity) {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.IdCurrentMessage = idMessage;
                View.LoadMessage(idMessage, idCommunity);
            }
        }
        public void LoadHistoryItems(Int32 pageIndex, Int32 idCommunity, long idDisplayedMessage)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
                LoadHistoryItems(pageIndex, idCommunity, Service.GetPermissions(idCommunity), idDisplayedMessage);
        }

        private void LoadHistoryItems(Int32 pageIndex, Int32 idCommunity, ModuleNoticeboard permissions, long idDisplayedMessage)
        {
            Int32 pageSize = View.HistoryPageSize;
            List<Status> items = Service.GetAvailableStatus(permissions, View.IsForManagement);
            Int32 count = Service.GetHistoryItemsCount(idCommunity, items);
            Int32 pageCount = (int)Math.Floor((double)(count / pageSize));//(count + pageSize - 1) / pageSize;
            View.HasHistory = (count > 0);

            if (pageIndex + 1 > pageCount)
                pageIndex = pageCount - 1;
            if (count > 0)
                View.LoadHistory(Service.GetHistoryItems(idCommunity, idDisplayedMessage, pageIndex, pageSize, View.GetTranslatedRemovedUser, items), pageIndex, pageCount);
        }
    }
}