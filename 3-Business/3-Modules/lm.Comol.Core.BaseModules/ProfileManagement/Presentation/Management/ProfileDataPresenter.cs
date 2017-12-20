using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class ProfileDataPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewProfileData View
            {
                get { return (IViewProfileData)base.View; }
            }
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            public ProfileDataPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ProfileDataPresenter(iApplicationContext oContext, IViewProfileData view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idProfile)
        {
            Person person = CurrentManager.GetPerson(idProfile);
            if (person == null)
                View.DisplayProfileUnknown();
            else
            {
                View.idDefaultProvider = person.IdDefaultProvider;
                View.IdProfile = person.Id;
                View.IdProfileType = person.TypeID;

                switch (person.TypeID) { 
                    case (int)UserTypeStandard.Company:
                        View.LoadProfileData(new dtoCompany(((CompanyUser)person)), ((CompanyUser)person).PersonInfo);
                        break;
                    case (int)UserTypeStandard.Employee:
                        View.LoadProfileData(new dtoEmployee(((Employee)person), AgencyVisibility.NotDeleted), ((Employee)person).PersonInfo);
                        break;
                    default:
                        View.LoadProfileData(person.Id, person.TypeID);
                        break;
                }
            }
        }

        public Boolean SaveData() {
            Boolean result = false;

            if (View.IdProfileType == (int)UserTypeStandard.Company)
            {
                dtoCompany company = (dtoCompany)View.CurrentProfile;
                PersonInfo userInfo = View.ProfilePersonalData;
                result = (Service.SaveCompanyUser(company, userInfo) != null);
            }
            else if (View.IdProfileType == (int)UserTypeStandard.Employee)
            {
                dtoEmployee employee = (dtoEmployee)View.CurrentProfile;
                PersonInfo userInfo = View.ProfilePersonalData;
                Employee savedEmployee = Service.SaveEmployee(employee, userInfo);
                if (savedEmployee != null && employee.CurrentAgency.Key > 0)
                    SaveAgencyAffiliation(employee.CurrentAgency.Key, employee.Id);
                result = (savedEmployee != null);
            }
            else
            {
                result = View.SaveProfile(View.CurrentProfile, View.ProfilePersonalData);
                if (result == true)
                    Service.UpdateFirstLetter(View.IdProfile);

            }
            return result;
        }

        public Boolean ValidateInput(Boolean verifyTaxCode) { 
            List<ProfilerError> errors = Service.VerifyStandardInfoDuplicate(View.CurrentProfile,verifyTaxCode);
            if (errors.Count>0)
                View.LoadErrors(errors);
            return (errors.Count == 0);
        }
         

        public Boolean EditProfileType(Int32 IdNewType)
        {
            Boolean result = false;
            Int32 IdProfile = View.IdProfile;
            ProfileTypeChanger person = CurrentManager.Get<ProfileTypeChanger>(IdProfile);
            Int32 oldIdType = person.TypeID;

            if (IdProfile > 0 && person != null) {
                if (person.TypeID == (int)UserTypeStandard.Company && IdNewType !=(int)UserTypeStandard.Company)
                   person = Service.EditProfileType(person, IdNewType);
                else if (IdNewType ==(int)UserTypeStandard.Company)
                   person = Service.EditProfileType(person, IdNewType);
                else if (person.TypeID == (int)UserTypeStandard.Employee && IdNewType != (int)UserTypeStandard.Employee)
                    person = Service.EditProfileType(person, IdNewType);
                else if (IdNewType == (int)UserTypeStandard.Employee)
                    person = Service.EditProfileType(person, IdNewType);
                if (oldIdType != IdNewType)
                {
                    if (IdNewType == (int)UserTypeStandard.Company) {
                        dtoCompany company = (dtoCompany)View.CurrentProfile;
                        if (oldIdType == (int)UserTypeStandard.Employee ||  View.DeletePreviousProfileType(IdProfile, oldIdType, IdNewType))
                            result = (Service.SaveCompanyUser(company, null) != null);
                    }
                    else if (IdNewType == (int)UserTypeStandard.Employee) {
                        dtoEmployee employee = (dtoEmployee)View.CurrentProfile;
                        if (oldIdType == (int)UserTypeStandard.Company || View.DeletePreviousProfileType(IdProfile, oldIdType, IdNewType)) {
                            Employee savedEmployee = Service.SaveEmployee(employee, null);
                            if (savedEmployee != null)
                            {
                                long idAgency = employee.CurrentAgency.Key;
                                if (idAgency < 1)
                                    idAgency = Service.GetEmptyAgency(0).Key;
                                SaveAgencyAffiliation(employee.CurrentAgency.Key, IdProfile);
                            }
                            result = (savedEmployee != null);
                        }
                    }
                    else
                    {
                        result = View.EditProfileType(View.CurrentProfile,oldIdType, IdNewType);
                        if (result == true)
                            Service.UpdateFirstLetter(IdProfile);
                    }

                    if (result && oldIdType == (int)UserTypeStandard.Employee)
                        Service.CloseEmployeeAffiliations(IdProfile);
                }
            }
            return result;
        }

        public Boolean SaveAgencyAffiliation(long idAgency, int idProfile)
        {
            Boolean result = (Service.AddEmployeeAffiliation(idAgency, idProfile) != null);
            if (result)
                View.LoadAffiliations(Service.GetUserAffiliations(idProfile, AgencyVisibility.NotDeleted));
            return result;
        }

        public void GetPersonAffiliations(int idProfile)
        {
            View.LoadAffiliations(Service.GetUserAffiliations(idProfile, AgencyVisibility.NotDeleted));
        }
    }
}