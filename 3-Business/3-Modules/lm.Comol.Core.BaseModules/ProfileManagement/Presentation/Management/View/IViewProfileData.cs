using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewProfileData : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
       
        Int32 IdProfile { get; set; }
        Int32 IdProfileType { get; set; }
        Int32 IdOldProfileType { get; set; }
        long idDefaultProvider { get; set; }

        dtoBaseProfile CurrentProfile { get; }
        PersonInfo ProfilePersonalData { get; }
        Boolean AllowAdvancedFieldsEdit { get; set; }
        Boolean IsInitialized { get; set; }
        Boolean ContainerMailEdit  { get; set; }
        Boolean ValidateContent();
        Boolean SaveData();
        Boolean SaveProfile(dtoBaseProfile profile, PersonInfo userInfo);
        void LoadProfileData(Int32 idProfile, Int32 idProfileType);
        void LoadProfileData(dtoCompany companyUser, PersonInfo userData);
        void LoadProfileData(dtoEmployee employeeUser, PersonInfo userData);
        void LoadErrors(List<ProfilerError> errors);

        void DisplayProfileUnknown();
        void InitializeControlForUserEdit(Int32 idProfile, Int32 idProfileType, Boolean allowSubscription);
        void InitializeControl(Int32 idProfile, Int32 idProfileType);
        void InitializeControlForEditingType(Int32 idProfile, Int32 idProfileType);
        void LoadAffiliations(List<dtoAgencyAffiliation> items);
        Boolean EditProfileType(Int32 newIdProfileType);
        Boolean EditProfileType(dtoBaseProfile profile, Int32 oldIdProfileType, Int32 newIdProfileType);
        Boolean DeletePreviousProfileType(Int32 idProfile, Int32 oldIdProfileType, Int32  newIdProfileType);

     //   void LoadErrorEditingType();
    }
}