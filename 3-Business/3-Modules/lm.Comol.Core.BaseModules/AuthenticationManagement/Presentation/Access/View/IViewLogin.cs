using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewLogin : IViewBaseAuthenticationPages
    {
        int LoggedUserId { get; set; }
        Boolean AllowAuthentication { set; }

        Boolean AllowAdminAccess { get; }
        Boolean isSystemOutOfOrder { get; }

        void DisplaySystemOutOfOrder();
        void DisplayAuthenticationOutOfOrder();
        void DisplayPrivacyPolicy(int userId, long idProvider, String defaultUrl, Boolean internalPage);
        void DisplayMustEditPassword(int userId, long idProvider);
        void DisplayAccountDisabled();
        void DisplayInvalidToken(String url, int idPerson, dtoUrlToken urlToken,UrlProviderResult status);
        void LogonUser(Person user, long idProvider,String defaultUrl, Boolean internalPage,Int32  idUserDefaultIdOrganization);
        void GoToProfile(string wizardUrl);
        void GotoAuthenticationSelctorPage();
    }
}