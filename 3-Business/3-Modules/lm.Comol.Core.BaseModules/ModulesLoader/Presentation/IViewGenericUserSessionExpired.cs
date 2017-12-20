using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ModulesLoader.Presentation
{
    public interface IViewGenericUserSessionExpired : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void WriteLogonCookies(dtoExpiredAccessUrl item);
        void GoToDefaultPage();
        void LoadInternalLoginPage();
        void LoadExternalProviderPage(String url);
    }
}
