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
    public class EditMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceNoticeBoard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewEditMessage View
            {
                get { return (IViewEditMessage)base.View; }
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
            public EditMessagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public EditMessagePresenter(iApplicationContext oContext, IViewEditMessage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView( long idMessage, Boolean advancedEditor, Int32 idCommunity,Boolean isForPortal)
        {
            if (IsSessionTimeout(true))
                return;
           
            if (!isForPortal && idCommunity == -1)
                idCommunity = UserContext.CurrentCommunityID;
            if (idCommunity == 0)
                isForPortal = true;
            NoticeBoard.Domain.ModuleNoticeboard permissions = Service.GetPermissions(idCommunity);

            View.IdCurrentCommunity = idCommunity;
            View.IsPortalPage = isForPortal;
            View.SetBackUrl(RootObject.NoticeboardDashboard(idMessage, idCommunity, isForPortal, false));
            if (permissions.Administration || permissions.EditMessage)
                InitializeView(idMessage,advancedEditor, idCommunity, isForPortal);
            else
            {
                View.DisplayNoPermission(idCommunity, CurrentIdModule);
                View.AllowSave(false);
            }
        }
        private void InitializeView(long idMessage,Boolean advancedEditor,Int32 idCommunity, Boolean isForPortal)
        {
            View.AllowSave(true);
            LoadMessage(idMessage, advancedEditor);
            View.SetHeaderTitle(isForPortal, (isForPortal) ? View.PortalName : CurrentManager.GetCommunityName(idCommunity));
        }

        private void LoadMessage(long idMessage, Boolean advancedEditor)
        {
            NoticeboardMessage message = null;
            if (idMessage != 0)
                message = Service.GetMessage(idMessage);
            else
            {
                message = new NoticeboardMessage();
                message.CreateByAdvancedEditor = advancedEditor;
                message.StyleSettings = new StyleSettings();
            }
            if (message == null && idMessage > 0)
            {
                View.AllowSave(false);
                View.DisplayUnknownMessage();
            }
            else { 
                View.IdCurrentMessage = idMessage;
                View.EditMessage(message, advancedEditor);
            }
        }

        public void SaveMessage(
            Boolean redirectToManagement,
            long idMessage, 
            Boolean advancedEditor, 
            Int32 idCommunity, 
            Boolean isForPortal, 
            String renderfolderpath, 
            String defaultHttpUrl, 
            System.Guid applicationWorkingId, 
            String baseUrlHttp, 
            String text,
            String plainText, 
            StyleSettings settings = null)
        {
            if (IsSessionTimeout(true))
                return;

            String url = String.Format("{0}{1}",
                defaultHttpUrl,
                lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.DisplayMessageWithoutSession("{0}", idCommunity, applicationWorkingId));

            idMessage = (Service.isNewMessage(idMessage, text, plainText) ? 0 : idMessage);

            NoticeboardMessage message = Service.SaveMessage(idMessage, advancedEditor, idCommunity, isForPortal, renderfolderpath, url, defaultHttpUrl, baseUrlHttp,text, plainText, settings);

            View.DisplayMessage((message != null), (idMessage == 0) ? ModuleNoticeboard.ActionType.Created : ModuleNoticeboard.ActionType.SavedMessage);
            if (message != null)
            {
                View.SendUserAction(
                    idCommunity, 
                    CurrentIdModule, 
                    message, 
                    RootObject.ViewMessage(message.Id, idCommunity), 
                    (idMessage == 0) ? ModuleNoticeboard.ActionType.Created : ModuleNoticeboard.ActionType.SavedMessage);

                if (redirectToManagement)
                    View.RedirectToDasboardUrl(RootObject.NoticeboardDashboard(message.Id, idCommunity, isForPortal, false));
                else
                {
                    View.IdCurrentMessage = message.Id;
                    View.EditMessage(message, advancedEditor);
                }
            }
            else
                View.SendUserAction(
                    idCommunity, 
                    CurrentIdModule, 
                    (idMessage > 0) ? ModuleNoticeboard.ActionType.UnableToSaveMessage : ModuleNoticeboard.ActionType.UnableToAddMessage);
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