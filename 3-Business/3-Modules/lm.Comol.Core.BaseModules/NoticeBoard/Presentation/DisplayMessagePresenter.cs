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
    public class DisplayMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewDisplayMessage View
            {
                get { return (IViewDisplayMessage)base.View; }
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

        public DisplayMessagePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public DisplayMessagePresenter(iApplicationContext oContext, IViewDisplayMessage view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView(String editorConfigurationPath, System.Guid currentWorkingApplicationId){
            NoticeboardMessage message = null;

            EditorConfiguration config = ServiceEditor.GetConfiguration(editorConfigurationPath);
            if (config != null) {
                ModuleEditorSettings mSettings = (config.ModuleSettings == null) ? null : config.ModuleSettings.Where(m => m.ModuleCode == ModuleNoticeboard.UniqueID).FirstOrDefault();
                if (mSettings == null && config.CssFiles.Any())
                    View.PreloadCssFiles(config.DefaultCssFilesPath,config.CssFiles);
                else if (mSettings != null && mSettings.CssFiles != null && mSettings.CssFiles.Any() && mSettings.OvverideCssFileSettings) {
                    View.PreloadCssFiles(config.DefaultCssFilesPath, mSettings.CssFiles);
                }
                else if (mSettings != null && mSettings.CssFiles != null && !mSettings.OvverideCssFileSettings)
                {
                    View.PreloadCssFiles(config.DefaultCssFilesPath, config.CssFiles);
                }
            }

            long idMessage = View.PreloadedIdMessage;
            int IdCommunity = View.PreloadedIdCommunity;
            if (idMessage != 0)
                message = Service.GetMessage(idMessage);
            else
            {
                message = Service.GetLastMessage(IdCommunity);
                if (message != null)
                    idMessage = message.Id;
            }
            if (message != null && message.Community != null)
                IdCommunity = message.Community.Id;
            else if (message != null && message.isForPortal)
                IdCommunity = 0;
            else
                IdCommunity = UserContext.WorkingCommunityID;

            Community community = null;
            if (IdCommunity > 0)
                community = CurrentManager.GetCommunity(IdCommunity);
            if (community == null && IdCommunity > 0)
                View.ContainerName = "";
            else if (community != null)
                View.ContainerName = community.Name;
            else
                View.ContainerName = View.PortalName;

            Boolean anonymousViewAllowed = ( View.PreloadWorkingApplicationId != Guid.Empty && View.PreloadWorkingApplicationId== currentWorkingApplicationId );
            if (message == null && idMessage > 0)
            {
                View.DisplayUnknownMessage();
                View.SendUserAction(IdCommunity, ModuleID, idMessage, ModuleNoticeboard.ActionType.ViewUnknownMessage);
            }
            else if (message == null && idMessage == 0){
                View.DisplayEmptyMessage();
                View.SendUserAction(IdCommunity, ModuleID, idMessage, ModuleNoticeboard.ActionType.ViewEmptyMessage);
            }
            else if (UserContext.isAnonymous && !anonymousViewAllowed  )
                View.DisplaySessionTimeout();
            else
            {
                View.IdCurrentMessage = idMessage;

                ModuleNoticeboard module = null;
                if (IdCommunity == 0 && message.isForPortal)
                    module = ModuleNoticeboard.CreatePortalmodule((UserContext.isAnonymous) ? (int)UserTypeStandard.Guest : UserContext.UserTypeID);
                else if (IdCommunity > 0)
                    module = new ModuleNoticeboard(CurrentManager.GetModulePermission(UserContext.CurrentUserID, IdCommunity, ModuleID));
                else
                    module = new ModuleNoticeboard();
                if (module.Administration || module.ViewCurrentMessage || module.ViewOldMessage || anonymousViewAllowed)
                {
                    View.DisplayMessage(message);
                    View.SendUserAction(IdCommunity, ModuleID, idMessage, ModuleNoticeboard.ActionType.ViewMessage);
                }
                else
                {
                    View.DisplayNoPermission();
                    View.SendUserAction(IdCommunity, ModuleID, idMessage, ModuleNoticeboard.ActionType.NoPermission);
                }
            }
        }
    }
}
