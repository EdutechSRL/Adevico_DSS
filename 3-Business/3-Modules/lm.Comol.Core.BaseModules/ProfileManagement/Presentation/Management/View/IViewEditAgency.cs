using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewEditAgency : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long PreloadedIdAgency { get;  }

        Boolean AllowManagement { set; }
        Boolean AllowEdit { get; set; }
        long IdAgency { get; set; }

        dtoAgency CurrentAgency { get; }
        void LoadAgencyName(string displayName);
        void LoadAgency(dtoAgency agency, List<dtoAvailableAffiliation> organizations);
        void LoadErrors(List<AgencyError> errors);
        void InitializeForAdd(List<dtoAvailableAffiliation> organizations);
        void DisplayErrorSaving();
        void DisplaySessionTimeout();
        void DisplayAgencyUnknown();
        void DisplayNoPermission();
        void GotoManagement();
    }
}