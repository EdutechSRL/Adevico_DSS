using System;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewUserLogout : IViewBaseAuthenticationPages
    {
        lm.Comol.Core.DomainModel.Helpers.dtoLoginCookie UserAccessInfo { get; }
        Boolean IsShibbolethSessionActive(string key);
        void LoadInternalLoginPage();
        void LoadExternalProviderPage(String url);
        void LoadLogoutMessage( LogoutMode mode,AuthenticationProviderType type, String destinationUrl);
        //void LoadOldAuthenticationPage(int idAuthenticationType);
        void GoToDefaultPage();
        void LoadLanguage(Language language);
    }
}