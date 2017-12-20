using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewBaseDashboardLoader : IViewPageBase
    {
        #region "Preload"
            ISettingsBase PreloadSettingsBase { get; }
        #endregion

        #region "Common"
            lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl GetAutoLogonCookie();
            lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode GetAutoRedirectMode();
            void RedirectToAutoLogonPage();
            void RedirectToAutoLogonPage(lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl cookie, Boolean redirect);
            UserCurrentSettings GetCurrentCookie();
            void SaveCurrentCookie(UserCurrentSettings settings);
            void SaveCurrentCookie(ISettingsBase settings);
            void GeneratePortalWebContext(Int32 idDefaultOrganization);
        #endregion
       
    }
}