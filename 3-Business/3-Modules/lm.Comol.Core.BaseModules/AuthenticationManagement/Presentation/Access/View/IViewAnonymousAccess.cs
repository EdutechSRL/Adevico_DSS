using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewAnonymousAccess : IViewBaseAuthenticationPages
    {
        Int32 PreloadedIdCommunity { get; }

        Boolean isSystemOutOfOrder { get; }
        void DisplayCommunityName(String name);
        void DisplaySystemOutOfOrder();
        void DisplayAccountDisabled();
        void DisplayUnknownUser();
        void DisplayUnknownCommunity();
        void DisplayNotAllowedCommunity();
        void LogonUser(Person user, long idProvider, String defaultUrl);
        void LogonIntoCommunity(Int32 idUser, Int32 idCommunity);
    }
}