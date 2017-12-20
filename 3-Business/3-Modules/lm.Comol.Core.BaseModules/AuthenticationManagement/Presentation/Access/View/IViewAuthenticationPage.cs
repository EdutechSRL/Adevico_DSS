using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewAuthenticationPage : IViewBaseAuthenticationPages
    {
        Boolean AllowInternalAuthentication { set; }
        Boolean isSystemOutOfOrder {get;}
        Boolean IsFromLogoutPage { get; }
        Boolean AllowAdminAccess { get; }
        Boolean HasUrlValues { get; }

        dtoUrlToken GetUrlToken(List<String> identifiers);

        void GoToProfile(long idProvider,dtoUrlToken urlToken, string wizardUrl);
        void GoToProfile(string wizardUrl);
      

        void GotoRemoteUrl(String url);


        void DisplayMustEditPassword(int userId, long idProvider);
        void DisplayPrivacyPolicy(int userId, long idProvider, String defaultUrl, Boolean internalPage);
        void DisplayAccountDisabled(String url);
        void DisplaySystemOutOfOrder();
        void DisplayInvalidToken(String url, int idPerson, dtoUrlToken urlToken,UrlProviderResult status);
        void LogonUser(Person user, long idProvider,String defaultUrl, Boolean internalPage, Int32 idUserDefaultIdOrganization);
    } 
}