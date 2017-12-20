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
    public class DefaultDashboardPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private ServiceDashboard service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewDefaultDashboardLoader View
            {
                get { return (IViewDefaultDashboardLoader)base.View; }
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
            public DefaultDashboardPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DefaultDashboardPresenter(iApplicationContext oContext, IViewDefaultDashboardLoader view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Boolean forPortal)
        {
            Int32 idCommunity = View.PreloadIdCommunity;
            if (forPortal)
                idCommunity = 0;
            else if (idCommunity == -1)
            {
                idCommunity = UserContext.CurrentCommunityID;
                forPortal = (idCommunity==0) ? true : false;
            }

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl cookie = View.GetAutoLogonCookie();
                lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode rMode = (forPortal && cookie != null && !String.IsNullOrWhiteSpace(cookie.DestinationUrl)) ? cookie.Display : DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None;
                if (forPortal)
                    View.GeneratePortalWebContext(CurrentManager.GetUserDefaultIdOrganization(UserContext.CurrentUserID));
                switch (rMode) { 
                    case DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow:
                        View.RedirectToAutoLogonPage();
                        break;
                    case DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow:
                        Boolean redirect = (!String.IsNullOrWhiteSpace(cookie.PreviousUrl));
                        View.RedirectToAutoLogonPage(cookie, redirect);
                        //if (!redirect)
                        //    InternalInitView(forPortal, idCommunity);
                        break;
                    default:
                        InternalInitView(forPortal, idCommunity);
                        break;
                }
            }
        }
        public void InternalInitView(Boolean forPortal, Int32 idCommunity)
        {
            DashboardType type = (forPortal) ? DashboardType.Portal : DashboardType.Community;
            liteDashboardSettings settings = Service.DashboardSettingsGet(type, idCommunity, true);
            if (settings != null)
            {
                if (forPortal)
                    LoadPortalDashboard(settings);
                else
                    LoadCommunityDashboard(settings, idCommunity);
            }
        }
        private void LoadPortalDashboard(liteDashboardSettings settings)
        {
            String url = "";
            switch(settings.Container.Default.AfterUserLogon){
                case OnLoadSettings.AlwaysDefault:
                    url = RootObject.LoadPortalView(UserContext.CurrentUserID, settings.Container.Default);
                    View.SaveCurrentCookie(settings.Container.Default);
                    break;
                case OnLoadSettings.UserDefault:
                    liteUserDashboardSettings userSettings = Service.UserPortalDashboardSettingsGet(UserContext.CurrentUserID, true, settings);
                    if (userSettings == null || userSettings.AfterUserLogon == OnLoadSettings.AlwaysDefault)
                    {
                        url = RootObject.LoadPortalView(UserContext.CurrentUserID, settings.Container.Default);
                        View.SaveCurrentCookie(settings.Container.Default);
                    }
                    else if (userSettings.AfterUserLogon == OnLoadSettings.UserLastClick && View.GetCurrentCookie() != null)
                        url = RootObject.LoadPortalView(UserContext.CurrentUserID, View.GetCurrentCookie());
                    else
                    {
                        url = RootObject.LoadPortalView(UserContext.CurrentUserID, userSettings);
                        View.SaveCurrentCookie(userSettings);
                    }
                    break;
                case OnLoadSettings.UserLastClick:
                    if (View.GetCurrentCookie() != null)
                        url = RootObject.LoadPortalView(UserContext.CurrentUserID, View.GetCurrentCookie());
                    else
                    {
                        url = RootObject.LoadPortalView(UserContext.CurrentUserID, settings.Container.Default);
                        View.SaveCurrentCookie(settings.Container.Default);
                    }
                    break;
            }
             View.LoadDashboard(url);
        }
        private void LoadCommunityDashboard(liteDashboardSettings settings, Int32 idCommunity)
        {
        //    String url = "";
        //     switch(settings.Container.Default.AfterUserLogon){
        //                case OnLoadSettings.AlwaysDefault:
        //                    url = RootObject.

        //                    break;
        //                case OnLoadSettings.UserDefault:
        //                    liteUserDashboardSettings userSettings = (forPortal) ? Service.UserPortalDashboardSettingsGet(UserContext.CurrentUserID, true, settings) : Service.UserCommunityDashboardSettingsGet(UserContext.CurrentUserID, idCommunity, true, settings);
        //                    if (userSettings==null)
        //                        url =
        //                    break;
        //                case OnLoadSettings.UserLastClick:
        //                    break;
        //            }
        //    View.LoadDashboard(url);
        }
    }
}