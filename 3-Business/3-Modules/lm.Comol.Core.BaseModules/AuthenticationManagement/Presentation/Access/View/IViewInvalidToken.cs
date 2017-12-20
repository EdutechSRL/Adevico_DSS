using System;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewInvalidToken : IViewBaseAuthenticationPages
    {
        int PreloadedIdProvider { get; }
        int PreloadedIdPerson { get; }
        UrlProviderResult PreloadedToken { get; }

        void DisplayMessage(UrlProviderResult message);
        void DisplayMessage(string username, UrlProviderResult message);
        void SetAutoLogonUrl(String url);
        void GoToDefaultPage();
        void LoadLanguage(Language language);
    }
}