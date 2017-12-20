using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewDeleteProfile : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean AllowDelete { set; }
        Int32 PreloadedIdProfile { get; }
        Int32 PreloadedIdProfileType { get; }
        Int32 IdProfile { get; set; }

        Boolean DeleteProfile(Int32 idProfile);
        void DisplayProfileInfo(String displayName);
        void DisplayProfileUnknown();
        void DisplaySessionTimeout();
        void NoPermission();
        void GotoManagementPage();
    }
}