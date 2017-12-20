using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewAgencyInfo : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long PreloadedIdAgency { get; }

        long IdAgency { get; set; }

        void LoadAgencyInfo(dtoAgency agency);

        void DisplayAgencyUnknown();
        void DisplaySessionTimeout();
        void DisplayNoPermission();
    }
}