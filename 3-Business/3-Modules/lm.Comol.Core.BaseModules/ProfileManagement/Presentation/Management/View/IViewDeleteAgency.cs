using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewDeleteAgency : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean AllowDelete { set; }
        long PreloadedIdAgency { get; }
        long IdAgency { get; set; }

        void DisplayAgencyInfo(dtoAgency agency);
        void DisplayAgencyUnknown();
        void DisplaySessionTimeout();
        void NoPermission();
        void GotoManagementPage();
    }
}