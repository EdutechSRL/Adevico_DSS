using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewBaseProfileInfo : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 IdProfile { get; set; }
        Int32 IdProfileType { get; set; }
        Boolean IsInitialized { get; set; }
        Boolean AlsoProfileTypeInfo { get; set; }
        void InitializeControl(Int32 idProfile, Boolean alsoProfileTypeInfo);
        void LoadProfileData(Int32 idProfile, Int32 idProfileType);
        void LoadProfileData(dtoCompany companyUser);
        void LoadProfileData(dtoEmployee employeeUser);
        void LoadOrganizations(List<dtoProfileOrganization> organizations);
        void DisplayEmpty();
    }
}