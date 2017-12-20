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
    public class NoticeboardDashboardPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceNoticeBoard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewNoticeboardDashboard View
            {
                get { return (IViewNoticeboardDashboard)base.View; }
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
            public NoticeboardDashboardPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public NoticeboardDashboardPresenter(iApplicationContext oContext, IViewNoticeboardDashboard view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity,Boolean isForPortal, Boolean setBackUrl, long idMessage =0)
        {
            if (IsSessionTimeout(true))
                return;
           
            if (!isForPortal && idCommunity == -1)
                idCommunity = UserContext.CurrentCommunityID;
            if (idCommunity == 0)
                isForPortal = true;
            NoticeBoard.Domain.ModuleNoticeboard permissions = Service.GetPermissions(idCommunity);
            View.CurrentPermissions = permissions;
            View.IdCurrentCommunity = idCommunity;
            View.IsPortalPage = isForPortal;
            if (permissions.Administration || permissions.ViewCurrentMessage || permissions.ViewOldMessage)
                InitializeView(permissions, idCommunity, isForPortal, setBackUrl, idMessage);
            else
                View.DisplayNoPermission(idCommunity, CurrentIdModule);

        }
        private void InitializeView(NoticeBoard.Domain.ModuleNoticeboard permissions,Int32 idCommunity, Boolean isForPortal, Boolean setBackUrl, long idMessage)
        {
            View.SetHeaderTitle(isForPortal,(isForPortal) ? View.PortalName : CurrentManager.GetCommunityName(idCommunity));
            String url = View.GetBackUrl();
            if (setBackUrl && !String.IsNullOrEmpty(url))
                View.SetBackUrl(url);
            else if (setBackUrl && isForPortal)
                View.SetBackUrl(View.GetDefaultHomePage());

            LoadMessage(permissions, idCommunity, isForPortal, idMessage);
        }

        private void LoadMessage(NoticeBoard.Domain.ModuleNoticeboard permissions, Int32 idCommunity, Boolean isForPortal, long idMessage, Boolean initializeDisplayRender = true )
        {
            liteHistoryItem message = null;
            if (idMessage != 0)
                message = Service.GetHistoryItem(idMessage);
            else
            {
                message = Service.GetLastHistoryItem(idCommunity);
                if (message != null)
                    idMessage = message.Id;
                else
                    idMessage = 0;
            }
            View.IdCurrentMessage = idMessage;
            SetDashboardCommands(permissions, idCommunity, isForPortal, message);
            if (initializeDisplayRender)
                View.InitializeControl(idCommunity, permissions, idMessage);
        }
        private void SetDashboardCommands(NoticeBoard.Domain.ModuleNoticeboard permissions,Int32 idCommunity, Boolean isForPortal,liteHistoryItem  message)
        {
            if (permissions.Administration)
                View.SetNewMessageUrls(RootObject.AddMessageWithAdvancedEditor(idCommunity, isForPortal, false), RootObject.AddMessageWithSimpleEditor(idCommunity, isForPortal, false), true);
            if (message != null)
            {
                View.AllowVirtualUndelete(message.isDeleted && (permissions.Administration || permissions.RetrieveOldMessage));
                View.AllowVirtualDelete(!message.isDeleted && (permissions.Administration || permissions.DeleteMessage));
                if (!message.isDeleted && permissions.Administration)
                    View.SetEditingUrls(RootObject.EditMessageWithAdvancedEditor(message.Id, idCommunity, isForPortal, false), RootObject.EditMessageWithSimpleEditor(message.Id, idCommunity, isForPortal, false));
                else
                    View.SetEditingUrls("", "");
                View.AllowVirtualUndeleteAndSetActive(message.isDeleted && (permissions.Administration || permissions.RetrieveOldMessage));
                View.AllowSetActive(!message.isDeleted && message.Status != Status.Active);
            }
            else
            {
                View.AllowVirtualDelete(false);
                View.AllowVirtualUndelete(false);
                View.HideEditingCommands();
            }
        }

        public void SelectedMessage(Int32 idCommunity, Boolean isForPortal, long idMessage)
        {
            if (IsSessionTimeout(true))
                return;
            NoticeBoard.Domain.ModuleNoticeboard permissions = Service.GetPermissions(idCommunity);
            LoadMessage(permissions, idCommunity, isForPortal, idMessage, false);
        }

        public void SetActive(long idMessage, Int32 idCommunity, Boolean isForPortal, String renderfolderpath, String defaultHttpUrl, System.Guid applicationWorkingId, String baseUrlHttp)
        {
            if (IsSessionTimeout(true))
                return;
            NoticeBoard.Domain.ModuleNoticeboard permissions = Service.GetPermissions(idCommunity);
            if (permissions.Administration)
            {
                String url = defaultHttpUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.DisplayMessageWithoutSession(idMessage, idCommunity, applicationWorkingId);
                NoticeboardMessage message = Service.SetActiveMessage(idMessage, renderfolderpath, url, baseUrlHttp, defaultHttpUrl);
                View.DisplayMessage((message != null), ModuleNoticeboard.ActionType.SetDefault);
                if (message != null)
                {
                    View.SendUserAction(idCommunity, CurrentIdModule, message, RootObject.ViewMessage(idMessage, idCommunity), ModuleNoticeboard.ActionType.GenericError);
                    LoadMessage(Service.GetPermissions(idCommunity), idCommunity, isForPortal, message.Id, true);
                }
                else
                    View.SendUserAction(idCommunity, CurrentIdModule, ModuleNoticeboard.ActionType.GenericError);
            }
            else
                SetDashboardCommands(permissions, idCommunity, isForPortal, Service.GetHistoryItem(idMessage));
        }
        // String url,
        public void ClearNoticeBoard(Int32 idCommunity, Boolean isForPortal, String renderfolderpath,String defaultHttpUrl, System.Guid applicationWorkingId, String baseUrlHttp)
        {
            if (IsSessionTimeout(true))
                return;
             NoticeBoard.Domain.ModuleNoticeboard permissions = Service.GetPermissions(idCommunity);
             if (permissions.Administration)
             {
                 String url = defaultHttpUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.DisplayMessageWithoutSession("{0}", idCommunity, applicationWorkingId);
                 NoticeboardMessage message = Service.AddEmptyMessage(idCommunity, isForPortal, renderfolderpath, url, baseUrlHttp, defaultHttpUrl);

                 View.DisplayMessage((message != null), ModuleNoticeboard.ActionType.Clean);
                 if (message != null)
                 {
                     View.SendUserAction(idCommunity, CurrentIdModule, message, RootObject.ViewMessage(message.Id, idCommunity), ModuleNoticeboard.ActionType.Clean);
                     LoadMessage(Service.GetPermissions(idCommunity), idCommunity, isForPortal, message.Id, true);
                 }
                 else
                     View.SendUserAction(idCommunity, CurrentIdModule, ModuleNoticeboard.ActionType.GenericError);
             }
             else
                 SetDashboardCommands(permissions, idCommunity, isForPortal, Service.GetHistoryItem(View.IdCurrentMessage));
        }
        public void VirtualDeleteMessage(long idMessage,Int32 idCommunity, Boolean isForPortal,System.Guid applicationWorkingId)
        {
            if (IsSessionTimeout(true))
                return;
             NoticeBoard.Domain.ModuleNoticeboard permissions = Service.GetPermissions(idCommunity);
             if (permissions.Administration || permissions.DeleteMessage)
             {
                NoticeboardMessage message = Service.VirtualDeleteMessage(idMessage, true, false, applicationWorkingId);
                View.DisplayMessage((message != null), ModuleNoticeboard.ActionType.VirtualDelete);
                if (message != null)
                {
                    View.SendUserAction(idCommunity, CurrentIdModule, message, RootObject.ViewMessage(idMessage, idCommunity), ModuleNoticeboard.ActionType.VirtualDelete);
                    LoadMessage(Service.GetPermissions(idCommunity), idCommunity, isForPortal, message.Id, true);
                }
                else
                    View.SendUserAction(idCommunity, CurrentIdModule, ModuleNoticeboard.ActionType.GenericError);
             }
             else
                 SetDashboardCommands(permissions, idCommunity, isForPortal, Service.GetHistoryItem(idMessage));
        }
        public void DeleteMessage(long idMessage, Int32 idCommunity, Boolean isForPortal)
        {
            if (IsSessionTimeout(true))
                return;
            NoticeboardMessage message = Service.DeleteMessage(idMessage);
            View.DisplayMessage((message == null),ModuleNoticeboard.ActionType.Delete);
            if (message != null)
            {
                View.SendUserAction(idCommunity, CurrentIdModule, message, "",  ModuleNoticeboard.ActionType.Delete);
                LoadMessage(Service.GetPermissions(idCommunity), idCommunity, isForPortal, message.Id, true);
            }
            else
                View.SendUserAction(idCommunity, CurrentIdModule, ModuleNoticeboard.ActionType.GenericError);
        }
        public void VirtualUnDeleteMessage(long idMessage, Boolean activate, Int32 idCommunity, Boolean isForPortal, System.Guid applicationWorkingId, String renderfolderpath = "", String defaultHttpUrl = "", String baseUrlHttp = "")
        {
            if (IsSessionTimeout(true))
                return;
            NoticeBoard.Domain.ModuleNoticeboard permissions = Service.GetPermissions(idCommunity);
            if (permissions.Administration || permissions.RetrieveOldMessage)
            {
                String url = defaultHttpUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.DisplayMessageWithoutSession(idMessage, idCommunity, applicationWorkingId);
                NoticeboardMessage message = Service.VirtualDeleteMessage(idMessage, false, activate, applicationWorkingId, renderfolderpath, url, baseUrlHttp, defaultHttpUrl);
                View.DisplayMessage((message != null), (activate) ? ModuleNoticeboard.ActionType.UndeleteAndActivate : ModuleNoticeboard.ActionType.Undelete);
                if (message != null)
                {
                    View.SendUserAction(idCommunity, CurrentIdModule, message, RootObject.ViewMessage(idMessage, idCommunity), (activate) ? ModuleNoticeboard.ActionType.UndeleteAndActivate : ModuleNoticeboard.ActionType.Undelete);
                    LoadMessage(Service.GetPermissions(idCommunity), idCommunity, isForPortal, message.Id, true);
                }
                else
                    View.SendUserAction(idCommunity, CurrentIdModule, ModuleNoticeboard.ActionType.GenericError);
            }
            else
                SetDashboardCommands(permissions, idCommunity, isForPortal, Service.GetHistoryItem(idMessage));
        }



        private Boolean IsSessionTimeout(Boolean onLoad = false) {
            if (UserContext.isAnonymous)
            {
                if (onLoad)
                    View.DisplaySessionTimeout();
                else
                    View.DisplaySessionTimeout(RootObject.NoticeboardDashboard(View.IdCurrentMessage, View.IdCurrentCommunity, View.IsPortalPage, false), View.IdCurrentCommunity);
                return true;
            }
            return false;
        }
    }
}