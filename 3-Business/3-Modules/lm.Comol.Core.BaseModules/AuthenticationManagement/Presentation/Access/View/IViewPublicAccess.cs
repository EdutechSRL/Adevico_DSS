using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewPublicAccess : IViewAnonymousAccess
    {
        Int32 PreloadedIdUser { get; }
        //Int32 PreloadedIdCommunity { get; }
        
        //Boolean isSystemOutOfOrder { get; }
        //void DisplayCommunityName(String name);
        //void DisplaySystemOutOfOrder();
        //void DisplayAccountDisabled();
        //void DisplayUnknownUser();
        //void DisplayUnknownCommunity();
        //void DisplayNotAllowedCommunity();
        //void LogonUser(Person user);
        //void LogonIntoCommunity(Int32 idUser, Int32 idCommunity);
    }
}