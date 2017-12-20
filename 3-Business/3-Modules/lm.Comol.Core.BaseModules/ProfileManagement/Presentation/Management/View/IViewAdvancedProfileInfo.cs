using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewAdvancedProfileInfo : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }
        Int32 IdProfileType { get; set; }
        Boolean IsInitialized { get; set; }
        void InitializeControl(Int32 idProfile);
        void LoadProfileData(Int32 idProfile, Int32 idProfileType);
        void LoadProfileData(dtoCompany companyUser, PersonInfo userData);
        void LoadProfileData(dtoEmployee employeeUser, PersonInfo userData);
        void DisplayEmpty();
    }
}