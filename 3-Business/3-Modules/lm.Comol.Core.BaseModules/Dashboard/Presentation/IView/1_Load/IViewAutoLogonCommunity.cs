using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Dashboard.Domain;

namespace lm.Comol.Core.BaseModules.Dashboard.Presentation 
{
    public interface IViewAutoLogonCommunity : IViewPageBase
    {
        void LogonToCommunity(Int32 idCommunity, Int32 idUser);
        void DisplayNoAccessToCommunity(String name);
        void DisplayNotEnrolledIntoCommunity(String name);
        void DisplayWaitingActivaction(String name);
        void DisplayCommunityBlock(String name);
        void DisplayUnknownCommunity();
        void RedirectToPortalHomePage();
    }
}