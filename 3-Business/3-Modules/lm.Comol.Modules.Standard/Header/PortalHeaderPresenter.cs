using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Modules.Standard.Menu.Business;



namespace lm.Comol.Modules.Standard.Header.Presentation
{
    public class PortalHeaderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initalize"
            private ServiceMenubar _Service;
            private ServiceMenubar Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceMenubar(AppContext);
                    return _Service;
                }
            }
            public virtual BaseModuleManager CurrentManager { get; set; }

            protected virtual IViewPortalHeader View
            {
                get { return (IViewPortalHeader)base.View; }
            }

            public PortalHeaderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PortalHeaderPresenter(iApplicationContext oContext, IViewPortalHeader view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idCommunity, String baseUrl, String defaultModuleToolTip, String defaultModuleUrl, String defaultModuleText)
        {
            InitView(idCommunity,View.MenubarType, baseUrl, defaultModuleToolTip, defaultModuleUrl, defaultModuleText);
        }

        public void InitView(Int32 idCommunity,MenuBarType menubarType, String baseUrl, String defaultModuleToolTip, String defaultModuleUrl, String defaultModuleText)
        {
            if (idCommunity==-1)
                idCommunity = UserContext.CurrentCommunityID;

            Int32 idOrganization = 0;
            if (idCommunity>0){
                liteCommunity community = CurrentManager.Get<liteCommunity>(idCommunity);
                if (community==null)
                    idCommunity=0;
                else
                    idOrganization= community.IdOrganization;
            }
            if (idOrganization==0)
                idOrganization = CurrentManager.GetUserDefaultIdOrganization(UserContext.CurrentUserID);
      
            if (idCommunity == 0 && (menubarType != MenuBarType.Portal && menubarType != MenuBarType.PortalAdministration)){
                menubarType = MenuBarType.Portal;
                idCommunity = 0;
            }
            else if (idCommunity > 0 && menubarType == MenuBarType.None)
                menubarType = MenuBarType.GenericCommunity;
            if (idCommunity == 0)
                View.DisplayName((menubarType == MenuBarType.Portal) ? View.GetPortalName : View.GetAdministrationName);
            else
                View.DisplayName(CurrentManager.GetCommunityName(idCommunity));

            //Language language = CurrentManager.GetLanguageByIdOrDefault(UserContext.cu)
            View.BindLogo(idCommunity, idOrganization, UserContext.Language.Code);
            View.LoadMenuBar(Service.RenderCachedMenubar(idCommunity,menubarType, baseUrl, defaultModuleToolTip, defaultModuleUrl, defaultModuleText));
            View.MenubarType = menubarType;
        }

    }
}
