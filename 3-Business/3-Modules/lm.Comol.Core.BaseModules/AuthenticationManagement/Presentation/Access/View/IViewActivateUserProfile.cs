using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewActivateUserProfile : IViewBaseAuthenticationPages
    {
        Int32 PreloadedIdUser { get; }
        System.Guid PreloadedUrlIdentifier { get; }
        Boolean AllowAdminAccess { get; }
        Boolean isSystemOutOfOrder { get; }
        Boolean AllowExternalWebAuthentication { set; }
        Boolean AllowInternalAuthentication { set; }
        void SetExternalWebLogonUrl(String url);


        void DisplaySystemOutOfOrder();
        void DisplayActivationInfo();
        void DisplayAlreadyActivatedInfo();
        void DisplayUnknownUser();
        void ReloadLanguageSettings(int idlanguage, String code);
        void SendActivationMail(Person person);
    }
}