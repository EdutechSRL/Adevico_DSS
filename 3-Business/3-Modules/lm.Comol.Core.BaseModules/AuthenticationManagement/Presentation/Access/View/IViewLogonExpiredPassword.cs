using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewLogonExpiredPassword : IViewBaseAuthenticationPages
    {
        Int32 PreloggedUserId { get; set; }
        long PreloggedProviderId { get; set; }
        int LoggedUserId { get; set; }
        long LoggedProviderId { get; set; }

        void GotoInternalShibbolethAuthenticationPage();
        void GotoInternalAuthenticationPage();
        void GotoShibbolethAuthenticationPage();
        void GotoRemoteLogonPage(String url);

        void DisplayPrivacyPolicy(int userId, long idProvider, String defaultUrl, Boolean internalPage);
        void DisplayMustChangePassword(lm.Comol.Core.Authentication.EditType type);
        void DisplayPasswordExpiredOn(DateTime expiredOn);
        void DisplayPasswordNotChanged();
        void DisplaySamePasswordException();
        void DisplayInvalidPassword();
        void LogonUser(Person user, Int32 idCommunity, long idProvider, String defaultUrl, Boolean internalPage, Int32 idDefaultOrganization);
    }
}