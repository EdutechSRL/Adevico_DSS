using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.PersonalInfo;
using lm.Comol.Core.Authentication.Business;
using NHibernate.Linq;
using lm.Comol.Core.BaseModules.ProviderManagement;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Business
{
    public partial class ProfileManagementService : CoreServices 
    {
        protected const int maxItemsForQuery = 1000;
        protected iApplicationContext _Context;

        #region initClass
            private UrlAuthenticationService _UrlService;
            private InternalAuthenticationService _InternalService;
            private UrlAuthenticationService UrlService
            {
                get
                {
                    if (_UrlService == null)
                        _UrlService = new UrlAuthenticationService(_Context);
                    return _UrlService;
                }
            }
            private InternalAuthenticationService InternalService
            {
                get
                {
                    if (_InternalService == null)
                        _InternalService = new InternalAuthenticationService(_Context);
                    return _InternalService;
                }
            }
            public ProfileManagementService() :base() { }
            public ProfileManagementService(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.UC = oContext.UserContext;
            }
            public ProfileManagementService(iDataContext oDC) : base(oDC) {
                _Context = new ApplicationContext() { DataContext = oDC };
            }
        #endregion

            public List<ProfileWizardStep> GetStandardProfileWizardStep(WizardType type)
            {
                List<ProfileWizardStep> steps = new List<ProfileWizardStep>();
                switch (type) { 
                    case  WizardType.Internal:
                        steps.Add(ProfileWizardStep.StandardDisclaimer);
                        steps.Add(ProfileWizardStep.OrganizationSelector);
                        steps.Add(ProfileWizardStep.ProfileTypeSelector);
                        steps.Add(ProfileWizardStep.ProfileUserData);
                        if (ExistPolicyToAccept())
                            steps.Add(ProfileWizardStep.Privacy);
                        steps.Add(ProfileWizardStep.Summary);
                        break;
                    case WizardType.UrlProvider:
                        steps.Add(ProfileWizardStep.UnknownProfileDisclaimer);
                        steps.Add(ProfileWizardStep.InternalCredentials);
                        steps.Add(ProfileWizardStep.OrganizationSelector);
                        steps.Add(ProfileWizardStep.ProfileTypeSelector);
                        steps.Add(ProfileWizardStep.ProfileUserData);
                        if (ExistPolicyToAccept())
                            steps.Add(ProfileWizardStep.Privacy);
                        steps.Add(ProfileWizardStep.Summary);
                        steps.Add(ProfileWizardStep.WaitingLogon);
                        break;
                    case WizardType.MacUrl:
                        steps.Add(ProfileWizardStep.UnknownProfileDisclaimer);
                        steps.Add(ProfileWizardStep.InternalCredentials);
                        steps.Add(ProfileWizardStep.OrganizationSelector);
                        steps.Add(ProfileWizardStep.ProfileTypeSelector);
                        steps.Add(ProfileWizardStep.ProfileUserData);
                        if (ExistPolicyToAccept())
                            steps.Add(ProfileWizardStep.Privacy);
                        steps.Add(ProfileWizardStep.Summary);
                        steps.Add(ProfileWizardStep.WaitingLogon);
                        break;
                    case WizardType.Ldap:
                        steps.Add(ProfileWizardStep.StandardDisclaimer);
                        steps.Add(ProfileWizardStep.OrganizationSelector);
                        steps.Add(ProfileWizardStep.ProfileTypeSelector);
                        steps.Add(ProfileWizardStep.LdapCredentials);
                        steps.Add(ProfileWizardStep.ProfileUserData);
                        if (ExistPolicyToAccept())
                            steps.Add(ProfileWizardStep.Privacy);
                        steps.Add(ProfileWizardStep.Summary);
                        break;
                    case WizardType.Shibboleth:
                        steps.Add(ProfileWizardStep.UnknownProfileDisclaimer);
                        steps.Add(ProfileWizardStep.InternalCredentials);
                        steps.Add(ProfileWizardStep.OrganizationSelector);
                        steps.Add(ProfileWizardStep.ProfileTypeSelector);
                        steps.Add(ProfileWizardStep.ProfileUserData);
                        if (ExistPolicyToAccept())
                            steps.Add(ProfileWizardStep.Privacy);
                        steps.Add(ProfileWizardStep.Summary);
                        steps.Add(ProfileWizardStep.WaitingLogon);
                        
                        break;
                    case WizardType.Administration:
                        steps.Add(ProfileWizardStep.ProfileTypeSelector);
                        steps.Add(ProfileWizardStep.OrganizationSelector);
                        steps.Add(ProfileWizardStep.AuthenticationTypeSelector);
                        steps.Add(ProfileWizardStep.ProfileUserData);
                        steps.Add(ProfileWizardStep.Summary);
                        break;
                    default:
                        break;
                }
                return steps;
            }
         
            private Boolean ExistPolicyToAccept()
            {
                return (from dp in Manager.GetIQ<DataPolicy>() where dp.Deleted == BaseStatusDeleted.None && dp.isActive && dp.Mandatory select dp.Id).Any();
            }

            #region "Add LoginInfo"
           
                public ProfilerError AddInternalLogin(Int32 IdPerson, String login, ref String newPassword, ref InternalLoginInfo loginInfo)
                {
                    ProfilerError message = ProfilerError.internalError;
                    Person person = Manager.GetPerson(IdPerson);
                    if (person!=null){
                        loginInfo = (from li in Manager.GetIQ<InternalLoginInfo>() where li.Person!=null && li.Person== person select li).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (loginInfo != null)
                            message = UpdateInternalProfile(loginInfo, person, login);
                        else {
                            message = InternalService.VerifyProfileInfo(person, login);
                            if (message == ProfilerError.none) {
                                newPassword = InternalService.GeneratePassword();
                                loginInfo = AddInternalProfile(login, newPassword, person.Id);
                                if (loginInfo != null)
                                    message = ProfilerError.none;
                                else
                                    message = ProfilerError.itemNotCreated;
                            }
                        }
                    } 
                    return message;
                }
                public ProfilerError UpdateInternalProfile(long idLoginInfo,Int32 IdPerson, String login)
                {
                    return UpdateInternalProfile(Manager.Get<InternalLoginInfo>(idLoginInfo),  Manager.GetPerson(IdPerson), login);
                }
                public ProfilerError UpdateInternalProfile(InternalLoginInfo loginInfo, Person person, String login)
                {
                    return InternalService.UpdateInternalProfile(loginInfo, person, login);;
                }
                public ProfilerError AddExternalLogin(Int32 IdPerson, long idProvider, dtoExternalCredentials credentials)
                {
                    ExternalLoginInfo externalLogin = UrlService.AddExternalProfile(Manager.GetPerson(IdPerson), Manager.Get<AuthenticationProvider>(idProvider), credentials);
                    if (externalLogin == null)
                        return ProfilerError.internalError;
                    else
                        return ProfilerError.none;
                }
                public ProfilerError UpdateExternalLogin(long idLoginInfo, dtoExternalCredentials credentials)
                {
                    ProfilerError message = ProfilerError.internalError;
                    try
                    {
                        Manager.BeginTransaction();
                        ExternalLoginInfo externalLogin = Manager.Get<ExternalLoginInfo>(idLoginInfo);
                        if (externalLogin != null)
                        {
                            externalLogin.UpdateMetaInfo(Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                            externalLogin.Deleted = BaseStatusDeleted.None;
                            externalLogin.IdExternalLong = credentials.IdentifierLong;
                            externalLogin.IdExternalString = credentials.IdentifierString;
                            Manager.SaveOrUpdate(externalLogin);
                            UrlService.AddToHistory(externalLogin);
                            message = ProfilerError.none;
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        message = ProfilerError.internalError;
                    }
       
                    return message;
                }
                public ProfilerError VerifyExternalInfoDuplicate(Int32 IdPerson,long idProvider, dtoExternalCredentials credentials) {
                    return UrlService.VerifyDuplicateExternalLoginInfo(Manager.GetPerson(IdPerson), Manager.Get<AuthenticationProvider>(idProvider), credentials);
                }
                public InternalLoginInfo AddInternalProfile(String login, String password, Int32 IdPerson)
                {
                    return InternalService.AddUserInfo(login, password, IdPerson);
                }
                public InternalLoginInfo AddInternalProfile(dtoBaseProfile profile, Int32 IdPerson) {
                    return InternalService.AddUserInfo(profile.Login, profile.Password, IdPerson);
                }
                            
                public ExternalLoginInfo AddExternalProfile(Int32 IdPerson, long idProvider, dtoExternalCredentials credentials)
                {
                    try
                    {
                        AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                        Person person = Manager.GetPerson(IdPerson);
                        if (provider != null && person != null)
                            return UrlService.AddExternalProfile(person, provider, credentials);
                    }
                    catch (Exception ex)
                    {

                    }
                    return null;
                }
                public WaitingActivationProfile AddWaitingActivationProfile(Int32 IdPerson)
                {
                    WaitingActivationProfile waiting = null;
                    try
                    {
                        Person user = Manager.GetPerson(IdPerson);
                        if (user != null)
                        {
                            Manager.BeginTransaction();
                            waiting = (from p in Manager.GetIQ<WaitingActivationProfile>() where p.Person == user select p).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (waiting == null)
                            {
                                waiting = new WaitingActivationProfile();
                                waiting.CreatedOn = DateTime.Now;
                                waiting.Deleted = BaseStatusDeleted.None;
                                waiting.Person = user;
                            }
                            waiting.UrlIdentifier = System.Guid.NewGuid();
                            Manager.SaveOrUpdate(waiting);
                            Manager.Commit();
                        }
                    }
                    catch (Exception ex) {
                        waiting = null;
                    }                
                    return waiting;
                }

                public InternalLoginInfo RenewPassword(int IdUser, ref String password) {
                    return InternalService.RenewPassword(Manager.GetPerson(IdUser), EditType.reset, ref password);
                }

                public Boolean DeleteLoginInfo(long idLoginInfo)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        BaseLoginInfo loginInfo = Manager.Get<BaseLoginInfo>(idLoginInfo);
                        if (loginInfo != null)
                        {
                            loginInfo.UpdateMetaInfo(Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                            loginInfo.Deleted = BaseStatusDeleted.Manual;
                            Manager.SaveOrUpdate(loginInfo);
                            if (typeof(InternalLoginInfo) == loginInfo.GetType())
                                InternalService.AddToHistory((InternalLoginInfo)loginInfo);
                            else
                                UrlService.AddToHistory((ExternalLoginInfo)loginInfo);
                            result = true;
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                public Boolean DeletePhisicalLoginInfo(long idLoginInfo)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        BaseLoginInfo loginInfo = Manager.Get<BaseLoginInfo>(idLoginInfo);
                        if (loginInfo != null)
                            Manager.DeletePhysical(loginInfo);
                        Manager.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }

                public List<ProfilerError> VerifyStandardInfoDuplicate(dtoBaseProfile profile, Boolean verifyTaxCode)
                {
                    return InternalService.VerifyExistingProfile(profile, verifyTaxCode);
                }
            #endregion

            #region "Add Profiles"

            public CompanyUser ImportCompanyUser(CompanyUser profile, AuthenticationProvider provider)
            {
                try
                {
                    if (provider == null && provider.ProviderType == AuthenticationProviderType.Internal)
                        return AddInternalCompanyUser(profile);
                    else
                        return AddCompanyUser(profile, provider);
                }
                catch (Exception ex) {
                    return null;
                }
            }
            public CompanyUser AddCompanyUser(CompanyUser profile)
            {
                try
                {
                        return AddInternalCompanyUser(profile);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        private CompanyUser AddInternalCompanyUser(CompanyUser profile)
            {
                AuthenticationProvider provider = (from p in Manager.GetIQ<InternalAuthenticationProvider>() where p.Deleted == BaseStatusDeleted.None select p).Skip(0).Take(1).ToList().FirstOrDefault();
                CompanyUser result = AddCompanyUser(profile,  provider);

                if (result != null)
                {
                    InternalLoginInfo userInfo = InternalService.AddUserInfo(profile.Login, profile.Password, result.Id);
                    
                        try
                        {
                            Manager.BeginTransaction();
                            if (userInfo == null)
                                Manager.DeletePhysical(result);
                            else
                                result.IdDefaultProvider = provider.Id;
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            result = null;
                        }
                }
                return result;
            }
            public CompanyUser SaveCompanyUser(dtoCompany company,PersonInfo userInfo)
            {
                CompanyUser result = null;
                try
                {
                    Manager.BeginTransaction();
                    CompanyUser user = Manager.Get<CompanyUser>(company.Id);
                    if (user != null) {
                        user.Name = company.Name;
                        user.CompanyInfo = company.Info;
                        user.FirstLetter = company.Surname[0].ToString().ToLower();
                        user.Mail = company.Mail;

                        user.Job = company.Job;
                        user.Sector = company.Sector;

                        if (!String.IsNullOrEmpty(user.Mail))
                            user.Mail = user.Mail.ToLower();
                        user.Surname = company.Surname;
                        user.TaxCode = company.TaxCode;
                        if (userInfo != null)
                        {
                            user.PersonInfo.Address = userInfo.Address;
                            user.PersonInfo.BirthDate = userInfo.BirthDate;
                            user.PersonInfo.BirthPlace = userInfo.BirthPlace;
                            user.PersonInfo.City = userInfo.City;
                            user.PersonInfo.DefaultShowMailAddress = userInfo.DefaultShowMailAddress;
                            user.PersonInfo.Fax = userInfo.Fax;
                            user.PersonInfo.Homepage = userInfo.Homepage;
                            user.PersonInfo.HomePhone = userInfo.HomePhone;
                            user.PersonInfo.IdNation = userInfo.IdNation;
                            user.PersonInfo.IdProvince = userInfo.IdProvince;
                            user.PersonInfo.IsMale = userInfo.IsMale;
                            user.PersonInfo.Mobile = userInfo.Mobile;
                            user.PersonInfo.Note = userInfo.Note;
                            user.PersonInfo.OfficePhone = userInfo.OfficePhone;
                            user.PersonInfo.PostCode = userInfo.PostCode;
                        }
                        Manager.SaveOrUpdate(user);
                        result = user;
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    result = null;
                }
                return result;
            }
            public CompanyUser AddCompanyUser(CompanyUser profile, long idProvider, dtoExternalCredentials credentials)
            {
                try
                {
                    AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                    if (provider != null)
                        return AddCompanyUser(profile, provider, credentials);
                }
                catch (Exception ex)
                {

                }
                return null;
            }
            public CompanyUser AddCompanyUser(CompanyUser profile, AuthenticationProvider provider, dtoExternalCredentials credentials)
            {
                CompanyUser result = AddCompanyUser(profile, provider);

                if (result != null) {
                    if (provider.ProviderType == AuthenticationProviderType.Internal)
                    {
                        InternalLoginInfo userInfo = InternalService.AddUserInfo(profile.Login, profile.Password, result.Id);
                        if (userInfo == null)
                        {
                            try
                            {
                                Manager.BeginTransaction();
                                Manager.DeletePhysical(result);
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                result = null;
                            }
                        }
                    }
                    else {
                        ExternalLoginInfo externalLogin = UrlService.AddExternalProfile(result, provider, credentials);
                        if (externalLogin == null)
                        {
                            try
                            {
                                Manager.BeginTransaction();
                                Manager.DeletePhysical(result);
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                result = null;
                            }
                        }
                    }
                }
                return result;
            }

            private CompanyUser AddCompanyUser(CompanyUser profile, AuthenticationProvider provider)
            {
                CompanyUser result = null;
                try
                {
                    Manager.BeginTransaction();
                    result = Manager.Get<CompanyUser>(profile.Id);
                    if (result == null)
                    {
                        result = new CompanyUser();
                        result.CreatedOn = DateTime.Now;
                    }
                    result.AuthenticationTypeID = profile.AuthenticationTypeID;
                    result.PersonInfo = profile.PersonInfo;
                    result.CompanyInfo = profile.CompanyInfo;
                    result.isDisabled = profile.isDisabled;
                    result.LanguageID = profile.LanguageID;
                    result.Login = profile.Login;
                    result.Mail = profile.Mail;

                    result.Job = profile.Job;
                    result.Sector = profile.Sector;

                    if (!String.IsNullOrEmpty(result.Mail))
                        result.Mail = result.Mail.ToLower();
                    result.Name = profile.Name;
                    lm.Comol.Core.Authentication.Helpers.InternalEncryptor helper = new lm.Comol.Core.Authentication.Helpers.InternalEncryptor();
                    //if (authenticationType == AuthenticationProviderType.Internal)
                        result.Password = helper.Encrypt(profile.Password);
                    //else
                    //    result.Password = helper.ge
                    result.Surname = profile.Surname;
                    if (string.IsNullOrEmpty(profile.FirstLetter ))
                        result.FirstLetter = profile.Surname[0].ToString().ToLower();
                    else
                        result.FirstLetter = profile.FirstLetter;

                    result.TaxCode = profile.TaxCode;
                    result.TypeID = profile.TypeID;
                    result.IdDefaultProvider = provider.Id;
                    result.LastAccessOn = result.CreatedOn;
                    Manager.SaveOrUpdate(result);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    result = null;
                }
                return result;
            }

            #region "Agency / Employee"
                public Employee ImportEmployee(Employee profile, AuthenticationProvider provider)
                {
                    try
                    {
                        if (provider == null && provider.ProviderType == AuthenticationProviderType.Internal)
                            return AddInternalEmployee(profile);
                        else
                            return AddEmployee(profile, provider);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                public Employee AddEmployee(Employee profile)
                {
                    try
                    {
                        return AddInternalEmployee(profile);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                private Employee AddInternalEmployee(Employee profile)
                {
                    AuthenticationProvider provider = (from p in Manager.GetIQ<InternalAuthenticationProvider>() where p.Deleted == BaseStatusDeleted.None select p).Skip(0).Take(1).ToList().FirstOrDefault();
                    Employee result = AddEmployee(profile, provider);

                    if (result != null && provider !=null)
                    {
                        InternalLoginInfo userInfo = InternalService.AddUserInfo(profile.Login, profile.Password, result.Id);                     
                            try
                            {
                                Manager.BeginTransaction();
                                if (userInfo == null)
                                    Manager.DeletePhysical(result);
                                else
                                    result.IdDefaultProvider = provider.Id;
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                result = null;
                            }
                    }
                    else
                        result = null;
                    return result;
                }
                public Employee AddEmployee(Employee profile, long idProvider, dtoExternalCredentials credentials)
                {
                    try
                    {
                        AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                        if (provider != null)
                            return AddEmployee(profile, provider, credentials);
                    }
                    catch (Exception ex)
                    {

                    }
                    return null;
                }
                public Employee AddEmployee(Employee profile, AuthenticationProvider provider, dtoExternalCredentials credentials)
                {
                    Employee result = AddEmployee(profile, provider);

                    if (result != null)
                    {
                        if (provider.ProviderType == AuthenticationProviderType.Internal)
                        {
                            InternalLoginInfo userInfo = InternalService.AddUserInfo(profile.Login, profile.Password, result.Id);
                            if (userInfo == null)
                            {
                                try
                                {
                                    Manager.BeginTransaction();
                                    Manager.DeletePhysical(result);
                                    Manager.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                    result = null;
                                }
                            }
                        }
                        else
                        {
                            ExternalLoginInfo externalLogin = UrlService.AddExternalProfile(result, provider, credentials);
                            if (externalLogin == null)
                            {
                                try
                                {
                                    Manager.BeginTransaction();
                                    Manager.DeletePhysical(result);
                                    Manager.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                    result = null;
                                }
                            }
                        }
                    }
                    return result;
                }
                private Employee AddEmployee(Employee profile, AuthenticationProvider provider)
                {
                    Employee result = null;
                    try
                    {
                        Manager.BeginTransaction();
                        result = Manager.Get<Employee>(profile.Id);
                        if (result == null)
                        {
                            result = new Employee();
                            result.CreatedOn = DateTime.Now;
                        }

                        result.Name = profile.Name;
                        result.Surname = profile.Surname;
                        result.LanguageID = profile.LanguageID;
                        result.Mail = profile.Mail;
                        if (!String.IsNullOrEmpty(result.Mail))
                            result.Mail = result.Mail.ToLower();

                        result.Login = profile.Login;
                        lm.Comol.Core.Authentication.Helpers.InternalEncryptor helper = new lm.Comol.Core.Authentication.Helpers.InternalEncryptor();
                        result.Password = helper.Encrypt(profile.Password);

                        result.TaxCode = profile.TaxCode;
                        result.TypeID = profile.TypeID;
                        result.Job = profile.Job;
                        result.Sector = profile.Sector;
                        
                        result.PersonInfo = profile.PersonInfo;
                        //result.PersonInfo.Note
                        //result.PersonInfo.IdProvince
                        //result.PersonInfo.IdNation
                        //result.PersonInfo.IdIstitution
                        //result.PersonInfo.BirthPlace
                        //result.PersonInfo.BirthDate
                        //result.PersonInfo.Address
                        //result.PersonInfo.PostCode
                        //result.PersonInfo.City
                        //result.PersonInfo.HomePhone
                        //result.PersonInfo.OfficePhone
                        //result.PersonInfo.Mobile
                        //result.PersonInfo.Fax
                        //result.PersonInfo.Homepage

                        result.AuthenticationTypeID = profile.AuthenticationTypeID;
                        result.isDisabled = profile.isDisabled;
                        
                        if (string.IsNullOrEmpty(profile.FirstLetter))
                            result.FirstLetter = profile.Surname[0].ToString().ToLower();
                        else
                            result.FirstLetter = profile.FirstLetter;

                        result.IdDefaultProvider = provider.Id;
                        result.LastAccessOn = result.CreatedOn;

                        Manager.SaveOrUpdate(result);
                        if (profile.Affiliations.Count>0){
                            foreach (AgencyAffiliation dto in profile.Affiliations){
                                Agency agency = Manager.Get<Agency>(dto.Agency.Id);
                                Person currentUser = Manager.GetPerson(UC.CurrentUserID);
                                if (agency == null)
                                    agency= GetEmptyAgency();
                                if (agency != null && (result!= null || currentUser!=null ))
                                {
                                    var query = (from aff in Manager.GetIQ<AgencyAffiliation>()
                                                                     where aff.Agency == agency && aff.Deleted == BaseStatusDeleted.None && aff.Employee == result && aff.IsEnabled
                                                                     select aff).Skip(0).Take(1).ToList();
                                    if (!query.Any()) {
                                        AgencyAffiliation affiliation = new AgencyAffiliation();

                                        if (currentUser != null && UC.UserTypeID != (int)UserTypeStandard.Guest)
                                            affiliation.CreateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
                                        else
                                            affiliation.CreateMetaInfo(result, UC.IpAddress, UC.ProxyIpAddress);
                                        affiliation.Agency = agency;
                                        affiliation.FromDate = DateTime.Now;
                                        affiliation.Employee = result;
                                        affiliation.IsEnabled = true;
                                        Manager.SaveOrUpdate(affiliation);
                                    }
                                }
                            }
                        }
                      
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = null;
                    }
                    return result;
                }
                public Employee SaveEmployee(dtoEmployee employee, PersonInfo userInfo)
                {
                    Employee result = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Employee user = Manager.Get<Employee>(employee.Id);
                        if (user != null)
                        {
                            user.Name = employee.Name;
                            user.FirstLetter = employee.Surname[0].ToString().ToLower();
                            if (!String.IsNullOrEmpty(employee.Mail))
                                user.Mail = employee.Mail.ToLower();
                            user.Surname = employee.Surname;
                            user.TaxCode = employee.TaxCode;

                            user.Job = employee.Job;
                            user.Sector = employee.Sector;

                            if (userInfo != null)
                            {
                                user.PersonInfo.Address = userInfo.Address;
                                user.PersonInfo.BirthDate = userInfo.BirthDate;
                                user.PersonInfo.BirthPlace = userInfo.BirthPlace;
                                user.PersonInfo.City = userInfo.City;
                                user.PersonInfo.DefaultShowMailAddress = userInfo.DefaultShowMailAddress;
                                user.PersonInfo.Fax = userInfo.Fax;
                                user.PersonInfo.Homepage = userInfo.Homepage;
                                user.PersonInfo.HomePhone = userInfo.HomePhone;
                                user.PersonInfo.IdNation = userInfo.IdNation;
                                user.PersonInfo.IdProvince = userInfo.IdProvince;
                                user.PersonInfo.IsMale = userInfo.IsMale;
                                user.PersonInfo.Mobile = userInfo.Mobile;
                                user.PersonInfo.Note = userInfo.Note;
                                user.PersonInfo.OfficePhone = userInfo.OfficePhone;
                                user.PersonInfo.PostCode = userInfo.PostCode;
                            }
                            Manager.SaveOrUpdate(user);
                            result = user;
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = null;
                    }
                    return result;
                }
                public AgencyAffiliation AddEmployeeAffiliation(long idAgency, int idEmployee)
                {
                    AgencyAffiliation affiliation = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Person employee = Manager.Get<Person>(idEmployee);
                        Agency agency = Manager.Get<Agency>(idAgency);
                        Person currentUser = Manager.GetPerson(UC.CurrentUserID);
                        if (employee != null && currentUser !=null)
                        {
                            Agency emptyAgency = GetEmptyAgency();
                            List<AgencyAffiliation> activeAffiliations = (from a in Manager.GetIQ<AgencyAffiliation>() 
                                                                       where a.Employee == employee && a.IsEnabled && a.Deleted == BaseStatusDeleted.None select a).ToList();
                            if (agency == null)
                                agency = emptyAgency;
                            if (agency != null)
                            {
                                foreach (AgencyAffiliation aff in activeAffiliations.Where(a => a.Agency != agency).ToList())
                                {
                                    aff.UpdateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
                                    aff.IsEnabled = false;
                                    aff.ToDate = DateTime.Now;
                                    Manager.SaveOrUpdate(aff);
                                }
                                if (activeAffiliations.Count == 0) {
                                    AgencyAffiliation lastAffiliations = (from a in Manager.GetIQ<AgencyAffiliation>()
                                                                          where a.Employee == employee && !a.IsEnabled && a.Deleted == BaseStatusDeleted.None
                                                                          orderby a.ToDate descending
                                                                          select a).Skip(0).Take(1).ToList().FirstOrDefault();
                                    if (lastAffiliations != null && lastAffiliations.Agency == emptyAgency) {
                                        affiliation = lastAffiliations;
                                        affiliation.UpdateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
                                        affiliation.ToDate = null;
                                        affiliation.IsEnabled = true;
                                        Manager.SaveOrUpdate(affiliation);
                                    }
                                }
                                else
                                    affiliation = activeAffiliations.Where(a => a.Agency == agency).FirstOrDefault();
                                if (affiliation == null)
                                {
                                    affiliation = new AgencyAffiliation();

                                    affiliation.CreateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
                                    affiliation.Agency = agency;
                                    affiliation.FromDate = DateTime.Now;
                                    affiliation.Employee = employee;
                                    affiliation.IsEnabled = true;
                                    Manager.SaveOrUpdate(affiliation);
                                }
                            }
                            //else {
                            //    agency = emptyAgency;
                            //    if (agency != null && !activeAffiliations.Where(a => a.Agency == agency).Any()) {
                            //        affiliation = new AgencyAffiliation();

                            //        affiliation.CreateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
                            //        affiliation.Agency = agency;
                            //        affiliation.FromDate = DateTime.Now;
                            //        affiliation.Employee = employee;
                            //        affiliation.IsEnabled = true;
                            //        Manager.SaveOrUpdate(affiliation);
                            //    }
                            //}
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        affiliation = null;
                    }
                    return affiliation;
                }
                public KeyValuePair<long,String> GetEmptyAgency(int idOrganization)
                {
                    KeyValuePair<long, String> result;
                    try
                    {
                        result = (from oa in Manager.GetIQ<OrganizationAffiliation>()
                                  where oa.IdOrganization == idOrganization && oa.Deleted == BaseStatusDeleted.None && oa.Agency != null && oa.Agency.Deleted == BaseStatusDeleted.None
                                  && oa.Agency.IsEmpty
                                  select new KeyValuePair<long, String>(oa.Agency.Id,oa.Agency.Name)).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (result.Key==0)
                            result = (from a in Manager.GetIQ<Agency>()
                                      where a.Deleted == BaseStatusDeleted.None && a.IsEmpty
                                      select new KeyValuePair<long, String>(a.Id, a.Name)).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        result = new KeyValuePair<long, String>((long)0,"");
                    }
                    return result;
                }
                public Agency GetEmptyAgencyForOrganization(int idOrganization)
                {
                    Agency result;
                    try
                    {
                        result = (from oa in Manager.GetIQ<OrganizationAffiliation>()
                                  where oa.IdOrganization == idOrganization && oa.Deleted == BaseStatusDeleted.None && oa.Agency != null && oa.Agency.Deleted == BaseStatusDeleted.None
                                  && oa.IsEmpty
                                  select oa.Agency).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (result == null) {
                            result = (from a in Manager.GetIQ<Agency>()
                                      where a.Deleted == BaseStatusDeleted.None && a.IsEmpty
                                      select a).Skip(0).Take(1).ToList().FirstOrDefault();
                        }
                    }
                    catch (Exception ex)
                    {
                        result = null;
                    }
                    return result;
                }
                public Agency GetEmptyAgency()
                {
                    Agency result;
                    try
                    {
                        result = (from a in Manager.GetIQ<Agency>()
                                  where a.Deleted == BaseStatusDeleted.None && a.Deleted == BaseStatusDeleted.None
                                  && a.IsEmpty
                                  select a).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        result = null;
                    }
                    return result;
                }
                public Agency GetDefaultAgency(int idOrganization)
                {
                    Agency result;
                    try
                    {
                        result = (from oa in Manager.GetIQ<OrganizationAffiliation>()
                                  where oa.IdOrganization == idOrganization && oa.Deleted == BaseStatusDeleted.None && oa.Agency != null && oa.Agency.Deleted == BaseStatusDeleted.None
                                  && oa.IsDefault
                                  select oa.Agency).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (result==null)
                            result = (from a in Manager.GetIQ<Agency>()
                                      where a.Deleted == BaseStatusDeleted.None && a.IsDefault
                                      select a).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex) {
                        result = null;
                    }
                    return result;
                }
                public Boolean ExistAgency(long idAgency)
                {
                    Boolean result = false;
                    try
                    {
                        result = (from a in Manager.GetIQ<Agency>()
                                  where a.Id == idAgency
                                  select a).Any();
                    }
                    catch (Exception ex)
                    {

                    }
                    return result;
                }
                public List<lm.Comol.Core.DomainModel.Helpers.StringItem<String>> GetAgenciesByName(int idOrganization, String value)
                {
                    List<lm.Comol.Core.DomainModel.Helpers.StringItem<String>> results = new List<lm.Comol.Core.DomainModel.Helpers.StringItem<String>>();
                    try
                    {
                        results = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None && (a.IsDefault || a.AlwaysAvailable)
                                   select a).ToList().Where(a=> a.Name.ToLower().Contains(value.ToLower())).Select(a=>
                                   new lm.Comol.Core.DomainModel.Helpers.StringItem<String>() { Id = a.Id.ToString(), Name = a.Name }).ToList();

                        var query = (from oa in Manager.GetIQ<OrganizationAffiliation>()
                         where oa.IdOrganization == idOrganization && oa.Deleted == BaseStatusDeleted.None && oa.Agency != null && oa.Agency.Deleted == BaseStatusDeleted.None
                         && oa.Agency.Name.Contains(value)
                                     select new { Id = oa.Agency.Id, Name = oa.Agency.Name });
                        results.AddRange(query.ToList().Select(a => new lm.Comol.Core.DomainModel.Helpers.StringItem<String>() { Id = a.Id.ToString(), Name = a.Name }).ToList());
                    }
                    catch (Exception ex) { 
                    
                    }
                    return results.OrderBy(a => a.Name).Distinct().ToList();
                }
                public List<lm.Comol.Core.DomainModel.Helpers.StringItem<String>> GetAgenciesByUser(int idPerson, String value)
                {
                    List<lm.Comol.Core.DomainModel.Helpers.StringItem<String>> results = new List<lm.Comol.Core.DomainModel.Helpers.StringItem<String>>();
                    try
                    {
                        Person person = Manager.GetPerson(idPerson);
                        List<long> idAffiliations = (from a in Manager.GetIQ<AgencyAffiliation>() where a.Deleted == BaseStatusDeleted.None && a.Employee== person && a.IsEnabled && a.Agency != null select a.Agency.Id).ToList();
                        List<int> idOrganizations = (from po in Manager.GetIQ<OrganizationProfiles>() where po.Profile == person select po.OrganizationID).ToList();
                        results = (from a in Manager.GetIQ<Agency>()
                                   where a.Deleted == BaseStatusDeleted.None && !idAffiliations.Contains(a.Id) && (a.IsDefault || a.AlwaysAvailable)
                                   select a).ToList().Where(a => a.Name.ToLower().Contains(value.ToLower())).Select(a =>
                                   new lm.Comol.Core.DomainModel.Helpers.StringItem<String>() { Id = a.Id.ToString(), Name = a.Name }).ToList();

                        var query = (from oa in Manager.GetIQ<OrganizationAffiliation>()
                                     where idOrganizations.Contains(oa.IdOrganization) && oa.Deleted == BaseStatusDeleted.None && oa.Agency != null && oa.Agency.Deleted == BaseStatusDeleted.None
                                     && !idAffiliations.Contains(oa.Agency.Id) && oa.Agency.Name.Contains(value)
                                     select new { Id = oa.Agency.Id, Name = oa.Agency.Name });
                        results.AddRange(query.ToList().Select(a => new lm.Comol.Core.DomainModel.Helpers.StringItem<String>() { Id = a.Id.ToString(), Name = a.Name }).ToList());
                    }
                    catch (Exception ex)
                    {

                    }
                    return results.OrderBy(a => a.Name).Distinct().ToList();
                }

                public List<dtoAgencyAffiliation> GetUserAffiliations(int idEmployee, AgencyVisibility visibility)
                {
                    List<dtoAgencyAffiliation> affiliations = new List<dtoAgencyAffiliation>();
                    try
                    {
                        Person person = Manager.GetPerson(idEmployee);
                        affiliations = (from a in Manager.GetIQ<AgencyAffiliation>() where a.Employee==person &&
                                        (
                                        (visibility == AgencyVisibility.Deleted && a.Deleted != BaseStatusDeleted.None)
                                        ||
                                            (
                                                (a.Deleted == BaseStatusDeleted.None)
                                                &&
                                                (
                                                visibility == AgencyVisibility.NotDeleted
                                                ||
                                                (visibility == AgencyVisibility.Active && a.IsEnabled)
                                                )
                                            )) select 
                                    new dtoAgencyAffiliation() { FromDate = a.FromDate, ToDate = a.ToDate, Id = a.Id, IsEnabled = a.IsEnabled, Agency = new KeyValuePair<long, string>(a.Agency.Id, a.Agency.Name) }).ToList();

                    }
                    catch (Exception ex)
                    {

                    }
                    return affiliations.OrderByDescending(a => a.IsEnabled).ThenByDescending(a => a.ToDate).ToList();
                }
                public Boolean CloseEmployeeAffiliations(int idEmployee)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        Person employee = Manager.Get<Person>(idEmployee);
                        Person currentUser = Manager.GetPerson(UC.CurrentUserID);
                        if (employee != null && currentUser != null)
                        {
                            List<AgencyAffiliation> activeAffiliations = (from a in Manager.GetIQ<AgencyAffiliation>()
                                                                          where a.Employee == employee && a.IsEnabled && a.Deleted == BaseStatusDeleted.None
                                                                          select a).ToList();
                            foreach (AgencyAffiliation aff in activeAffiliations)
                            {
                                aff.UpdateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
                                aff.IsEnabled = false;
                                aff.ToDate = DateTime.Now;
                                Manager.SaveOrUpdate(aff);
                            }
                        }
                        Manager.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                
                /// <summary>
                ///  Ritorna la lista delle agenzie correnti per gli utenti indicati
                /// </summary>
                /// <param name="idUsers"></param>
                /// <returns></returns>
                public Dictionary<long, List<Int32>> GetUsersWithAgencies(List<Int32> idUsers)
                {
                    Dictionary<long, List<Int32>> list = new Dictionary<long, List<Int32>>();
                    try
                    {
                        List<AgencyAffiliation> affiliations = new List<AgencyAffiliation>();
                        if (idUsers.Count() <= maxItemsForQuery)
                            affiliations = (from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && idUsers.Contains(a.Employee.Id) select a).ToList();
                        else
                        {
                            Int32 index = 0;
                            List<Int32> tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (tUsers.Any())
                            {
                                affiliations.AddRange((from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && tUsers.Contains(a.Employee.Id) select a).ToList());
                                index++;
                                tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                        }
                        if (affiliations.Any())
                            list = affiliations.GroupBy(a => a.Agency.Id).ToDictionary(a => a.Key, a => a.Select(af => af.Employee.Id).ToList());
                    }
                    catch (Exception ex)
                    {

                    }
                    return list;
                }
                public Dictionary<long, String> GetAgenciesName(List<long> idAgencies)
                {
                    Dictionary<long, String> list = new Dictionary<long, String>();
                    try
                    {
                        if (idAgencies.Count() <= maxItemsForQuery)
                            list = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None && idAgencies.Contains(a.Id) select a).ToDictionary(a => a.Id, a => a.Name);
                        else
                        {
                            Int32 index = 0;
                            List<long> tAgencies = idAgencies.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (tAgencies.Any())
                            {
                                (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None && tAgencies.Contains(a.Id) select a).ToList().ForEach(a => list.Add(a.Id, a.Name));
                                index++;
                                tAgencies = idAgencies.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return list;
                }
        
                public long AgenciesCount(dtoAgencyFilters filters)
                {
                    long count = (long)0;
                    try
                    {
                        var query = AgenciesQuery(filters);

                        count = query.Count();
                    }
                    catch (Exception ex)
                    {

                    }
                    return count;
                }
                private IEnumerable<Agency> AgenciesQuery(dtoAgencyFilters filters)
                {
                    IEnumerable<Agency> query = GetBaseAgencyQuery(filters);
                    if ((filters.SearchBy == SearchAgencyBy.Name || filters.SearchBy == SearchAgencyBy.All || filters.SearchBy == SearchAgencyBy.Contains || string.IsNullOrEmpty(filters.Value)) && !string.IsNullOrEmpty(filters.StartWith))
                    {
                        if (filters.StartWith != "#")
                            query = query.Where<Agency>(p => p.Name[0].ToString().ToLower() == filters.StartWith.ToLower());
                        else
                            query = query.Where<Agency>(p => DefaultOtherChars().Contains(p.Name[0].ToString()));
                    }
                    return query;
                }
                private IEnumerable<Agency> GetBaseAgencyQuery(dtoAgencyFilters filters)
                {
                    IEnumerable<Agency> query = (from p in Manager.GetIQ<Agency>() select p);
                    switch (filters.Availability)
                    {
                        case AgencyAvailability.Default:
                            List<long> dItems = (from a in Manager.GetIQ<OrganizationAffiliation>()
                                                   where a.Deleted== BaseStatusDeleted.None && a.IsDefault && a.Agency !=null  select a.Agency.Id).ToList().Distinct().ToList();
                            query = query.Where<Agency>(p => p.IsDefault || dItems.Contains(p.Id));
                            break;
                        case AgencyAvailability.Empty:
                            List<long> eItems = (from a in Manager.GetIQ<OrganizationAffiliation>()
                                                where a.Deleted == BaseStatusDeleted.None && a.IsDefault && a.Agency != null
                                                 select a.Agency.Id).ToList().Distinct().ToList();
                            query = query.Where<Agency>(p => p.IsEmpty || eItems.Contains(p.Id));
                            break;
                    }
                    if (!string.IsNullOrEmpty(filters.Value) && string.IsNullOrEmpty(filters.Value.Trim()) == false)
                    {
                        switch (filters.SearchBy)
                        {
                            case SearchAgencyBy.Contains:
                                query = query.Where<Agency>(p => p.Name.ToLower().Contains(filters.Value.ToLower()));
                                break;
                            case SearchAgencyBy.Name:
                                query = query.Where<Agency>(p => p.Name.ToLower().Contains(filters.Value.ToLower()));
                                break;
                            case SearchAgencyBy.TaxCode:
                                query = query.Where<Agency>(p => p.TaxCode != null && p.TaxCode != "" && p.TaxCode.ToLower().Contains(filters.Value.ToLower()));
                                break;
                            case SearchAgencyBy.ExternalCode:
                                query = query.Where<Agency>(p => p.ExternalCode != null && p.ExternalCode != "" && p.ExternalCode.ToLower().Contains(filters.Value.ToLower()));
                                break;
                            case SearchAgencyBy.NationalCode:
                                query = query.Where<Agency>(p => p.NationalCode != null && p.NationalCode != "" && p.NationalCode.ToLower().Contains(filters.Value.ToLower()));
                                break;
                        }
                    }
                    return query;
                }

                public List<String> GetAvailableStartLetter(dtoAgencyFilters filters)
                {
                    List<String> letters = new List<String>();
                    try
                    {
                        Manager.BeginTransaction();
                        var query = GetBaseAgencyQuery(filters).Where(p => p.Name.Length>0);
                        letters = (from p in query select p.Name[0].ToString().ToLower()).Distinct().ToList();
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return letters;
                }
                private IEnumerable<Agency> GetSearchAgencyQuery(dtoAgencyFilters filters)
                {
                    IEnumerable<Agency> query = AgenciesQuery(filters);
                    if (filters.Ascending){
                            switch(filters.OrderBy){
                                case OrderAgencyBy.Name:
                                    query = query.OrderBy(p => p.Name);
                                    break;
                                case OrderAgencyBy.ExternalCode:
                                    query = query.OrderBy(p => p.ExternalCode);
                                    break;
                                case OrderAgencyBy.NationalCode:
                                    query = query.OrderBy(p => p.NationalCode);
                                    break;
                                case OrderAgencyBy.TaxCode:
                                    query = query.OrderBy(p => p.TaxCode);
                                    break;
                            }
                        }
                    else
                    {
                        switch (filters.OrderBy)
                        {
                            case OrderAgencyBy.Name:
                                query = query.OrderByDescending(p => p.Name);
                                break;
                            case OrderAgencyBy.ExternalCode:
                                query = query.OrderByDescending(p => p.ExternalCode);
                                break;
                            case OrderAgencyBy.NationalCode:
                                query = query.OrderByDescending(p => p.NationalCode);
                                break;
                            case OrderAgencyBy.TaxCode:
                                query = query.OrderByDescending(p => p.TaxCode);
                                break;
                        }
                    }
                    return query;
                }
                public List<dtoAgencyItem> GetAgencies(dtoAgencyFilters filters, Int32 pageIndex, Int32 pageSize)
                {
                    List<dtoAgencyItem> items = new List<dtoAgencyItem>();
                    try
                    {
                        var query = GetSearchAgencyQuery(filters);
                        Int32 loaderType = UC.UserTypeID;
                        List<dtoAgency> agencies = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                 select new dtoAgency()
                                 {
                                     Id = p.Id,
                                     TaxCode = p.TaxCode,
                                     AlwaysAvailable = p.AlwaysAvailable,
                                     Name = p.Name,
                                     ExternalCode= p.ExternalCode,
                                     IsDefault= p.IsDefault,
                                     IsEditable= p.IsEditable,
                                     IsEmpty= p.IsEmpty,
                                     NationalCode= p.NationalCode,
                                     Deleted= p.Deleted 
                                 }).ToList();

                        IList<Organization> organizations = Manager.GetAll<Organization>();
                        foreach (dtoAgency agency in agencies.Where(a => a.AlwaysAvailable == false)) { 
                            List<OrganizationAffiliation> oAffiliations = (from i in Manager.GetIQ<OrganizationAffiliation>() where i.Deleted== BaseStatusDeleted.None && i.Agency != null && i.Agency.Id== agency.Id select i).ToList();
                            List<dtoOrganizationAffiliation> aAffiliations = new List<dtoOrganizationAffiliation>();
                            foreach(OrganizationAffiliation a in oAffiliations){
                                if (organizations.Where(o => o.Id == a.IdOrganization).Any()) {
                                    aAffiliations.Add(new dtoOrganizationAffiliation()
                                    {
                                        Id= a.IdOrganization,
                                        Name = organizations.Where(o => o.Id == a.IdOrganization).Select(o=>o.Name).FirstOrDefault(),
                                        IsDefault= a.IsDefault,
                                        IsEmpty = a.IsEmpty 
                                    });
                                }
                            }
                            agency.Organizations = aAffiliations.OrderBy(a => a.Name).ToList();
                        }

                        agencies.ForEach(a=> items.Add(new dtoAgencyItem(a,loaderType, a.Deleted, (from af in Manager.GetIQ<AgencyAffiliation>() where af.Deleted== BaseStatusDeleted.None && af.IsEnabled && af.Agency != null && af.Agency.Id== a.Id select af.Id).Count())));
                    }
                    catch (Exception ex)
                    {

                    }
                    return items;
                }
                public dtoAgency GetDtoAgency(long idAgency)
                {
                    dtoAgency result = null;
                    try
                    {
                        result = (from a in Manager.GetIQ<Agency>()
                                  where a.Id == idAgency
                                                    select new dtoAgency()
                                                    {
                                                        Id = a.Id,
                                                        TaxCode = a.TaxCode,
                                                        AlwaysAvailable = a.AlwaysAvailable,
                                                        Name = a.Name,
                                                        ExternalCode = a.ExternalCode,
                                                        IsDefault = a.IsDefault,
                                                        IsEditable = a.IsEditable,
                                                        NationalCode = a.NationalCode,
                                                        IsEmpty = a.IsEmpty,
                                                        Deleted = a.Deleted
                                                    }).ToList().FirstOrDefault();

                        if (result != null) {
                            IList<Organization> organizations = Manager.GetAll<Organization>();
                            //List<Int32> idOrganizations = (from i in Manager.GetIQ<OrganizationAffiliation>() where i.Deleted == BaseStatusDeleted.None && i.Agency != null && i.Agency.Id == idAgency select i.IdOrganization).ToList();
                            //result.Organizations = organizations.Where(o => idOrganizations.Contains(o.Id)).Select(o => new dtoOrganizationAffiliation() { Id = o.Id, Name = o.Name }).ToList();

                            List<OrganizationAffiliation> oAffiliations = (from i in Manager.GetIQ<OrganizationAffiliation>() where i.Deleted == BaseStatusDeleted.None && i.Agency != null && i.Agency.Id == idAgency select i).ToList();
                            List<dtoOrganizationAffiliation> aAffiliations = new List<dtoOrganizationAffiliation>();
                            foreach (OrganizationAffiliation a in oAffiliations)
                            {
                                if (organizations.Where(o => o.Id == a.IdOrganization).Any())
                                {
                                    aAffiliations.Add(new dtoOrganizationAffiliation()
                                    {
                                        Id = a.IdOrganization,
                                        Name = organizations.Where(o => o.Id == a.IdOrganization).Select(o => o.Name).FirstOrDefault(),
                                        IsDefault = a.IsDefault,
                                        IsEmpty = a.IsEmpty
                                    });
                                }
                            }
                            result.Organizations = aAffiliations.OrderBy(a => a.Name).ToList();

                            result.EmployeeNumber = (from af in Manager.GetIQ<AgencyAffiliation>() where af.Deleted == BaseStatusDeleted.None && af.IsEnabled && af.Agency != null && af.Agency.Id == idAgency select af.Id).Count();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        result = null;
                    }
                    return result;
                }
                public Agency SaveAgency(dtoAgency dto) {
                    return SaveAgency(UC.CurrentUserID,dto );
                }
                public Agency SaveAgency(Int32 idUser, dtoAgency dto)
                {
                    Agency agency = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Person user = Manager.GetPerson(idUser);
                        if (user!=null){
                            if (dto.Id == 0)
                            {
                                agency = new Agency();
                                agency.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                            }
                            else {
                                agency = Manager.Get<Agency>(dto.Id);
                                if (agency!=null)
                                    agency.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                            }
                            if (agency != null) {
                                Boolean wasDefault = agency.IsDefault;
                                agency.AlwaysAvailable = dto.AlwaysAvailable;
                                agency.Description = dto.Description;
                                agency.ExternalCode = dto.ExternalCode;
                                if (dto.AlwaysAvailable)
                                {
                                    agency.IsDefault = dto.IsDefault;
                                    agency.IsEditable = dto.IsEditable;
                                }
                                else {
                                    agency.IsDefault = false;
                                    agency.IsEditable = false;
                                }
                                agency.IsEmpty = dto.IsEmpty;
                                agency.Name = dto.Name;
                                agency.NationalCode = dto.NationalCode;
                                agency.TaxCode = dto.TaxCode;

                                Manager.SaveOrUpdate(agency);
                                List<OrganizationAffiliation> affiliations = null;

                                if (dto.AlwaysAvailable)
                                {
                                    #region "Empty settings"
                                    if (dto.IsEmpty)
                                    {
                                        List<Agency> emptyAgencies = (from a in Manager.GetIQ<Agency>()
                                                                      where a.Deleted == BaseStatusDeleted.None && a.IsEmpty && a.Id != dto.Id
                                                                      select a).ToList();
                                        if (emptyAgencies.Count > 0)
                                        {
                                            foreach (Agency a in emptyAgencies)
                                            {
                                                a.IsEmpty = false;
                                                a.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                            Manager.SaveOrUpdateList(emptyAgencies);
                                        }
                                    }
                                    #endregion
                                    #region "Default settings"
                                    if (dto.IsDefault)
                                    {
                                        List<Agency> defaultAgencies = (from a in Manager.GetIQ<Agency>()
                                                                        where a.Deleted == BaseStatusDeleted.None && a.AlwaysAvailable && a.IsDefault && a.Id != dto.Id
                                                                        select a).ToList();
                                        if (defaultAgencies.Any())
                                        {
                                            foreach (Agency a in defaultAgencies)
                                            {
                                                a.IsDefault = false;
                                                a.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                            Manager.SaveOrUpdateList(defaultAgencies);
                                        }
                                    }
                                    #endregion
                                    #region "Remove affiliations"
                                    affiliations = agency.ActiveAffiliations();
                                    if (affiliations.Any())
                                    {
                                        foreach (OrganizationAffiliation a in affiliations)
                                        {
                                            a.Deleted = BaseStatusDeleted.Manual;
                                            a.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                        Manager.SaveOrUpdateList(affiliations);
                                    }
                                    #endregion
                                }

                                else{
                                    List<Int32> idOrganizations = dto.Organizations.Select(o => o.Id).ToList();

                                    if (idOrganizations.Any() && (dto.IsDefault || dto.IsEmpty)) {
                                        List<OrganizationAffiliation> eAffiliations = (from a in Manager.GetIQ<OrganizationAffiliation>()
                                                                                       where a.Deleted == BaseStatusDeleted.None && a.Agency != null && a.Agency.Id != dto.Id
                                                                                       && ((dto.IsEmpty && a.IsEmpty) || (dto.IsDefault && a.IsDefault))
                                                                                       && idOrganizations.Contains(a.IdOrganization)
                                                                                       select a).ToList();

                                        foreach (OrganizationAffiliation a in eAffiliations)
                                        {
                                            a.IsEmpty = (dto.IsEmpty) ? !dto.IsEmpty : a.IsEmpty;
                                            a.IsDefault = (dto.IsDefault) ? !dto.IsDefault : a.IsDefault;
                                            a.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                                        }
                                    }

                                    #region "Set affiliations"

                                    foreach (OrganizationAffiliation affiliation in agency.OrganizationAffiliations) {
                                        if (idOrganizations.Contains(affiliation.IdOrganization))
                                        {
                                            if (affiliation.Deleted != BaseStatusDeleted.None || affiliation.IsDefault != dto.IsDefault || affiliation.IsEmpty != dto.IsEmpty)
                                            {
                                                affiliation.Deleted = BaseStatusDeleted.None;
                                                affiliation.IsDefault = dto.IsDefault;
                                                affiliation.IsEmpty = dto.IsEmpty;
                                                affiliation.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                                            }
                                            idOrganizations.Remove(affiliation.IdOrganization);
                                        }
                                        else 
                                            affiliation.SetDeleteMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                                    }
                                    foreach (Int32 idOrganization in idOrganizations)
                                    {
                                        OrganizationAffiliation affiliation = new OrganizationAffiliation();
                                        affiliation.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                                        affiliation.Agency = agency;
                                        affiliation.IdOrganization = idOrganization;
                                        affiliation.IsDefault = dto.IsDefault;
                                        affiliation.IsEmpty = dto.IsEmpty;
                                        Manager.SaveOrUpdate(affiliation);
                                        agency.OrganizationAffiliations.Add(affiliation);
                                        
                                    }
                                    Manager.SaveOrUpdate(agency);
                                    #endregion
                                }
                                Manager.SaveOrUpdate(agency);
                            }
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        agency = null;
                    }
                    return agency;
                }
                public virtual List<AgencyError> VerifyExistingAgency(dtoAgency dto)
                {
                    List<AgencyError> result = new List<AgencyError>();
                    if ((from p in Manager.GetIQ<Agency>() where p.Name == dto.Name && p.Id != dto.Id select p.Id).Any())
                        result.Add(AgencyError.nameDuplicate);
                    if (!String.IsNullOrEmpty(dto.TaxCode) && (from p in Manager.GetIQ<Agency>() where p.TaxCode == dto.TaxCode && p.Id != dto.Id select p.Id).Any())
                        result.Add(AgencyError.taxCodeDuplicate);
                    if (!String.IsNullOrEmpty(dto.NationalCode) && (from p in Manager.GetIQ<Agency>() where p.NationalCode == dto.NationalCode && p.Id != dto.Id select p.Id).Any())
                        result.Add(AgencyError.nationalCodeDuplicate);
                    if (!String.IsNullOrEmpty(dto.ExternalCode) && (from p in Manager.GetIQ<Agency>() where p.ExternalCode == dto.ExternalCode && p.Id != dto.Id select p.Id).Any())
                        result.Add(AgencyError.externalCodeDuplicate);
                    return result;
                }
                public Boolean VirtualDeleteAgency(long idAgency,Boolean delete)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Agency agency = Manager.Get<Agency>(idAgency);
                        Person user = Manager.GetPerson(UC.CurrentUserID);
                        if (agency != null && user != null && !agency.IsEmpty && !agency.IsDefault)
                        {
                            List<AgencyAffiliation> employees = (from p in Manager.GetIQ<AgencyAffiliation>() where p.Agency == agency select p).ToList();
                            foreach (AgencyAffiliation emp in employees) {
                                emp.Deleted = delete ? (emp.Deleted | BaseStatusDeleted.Cascade) : (emp.Deleted = (BaseStatusDeleted)((int)emp.Deleted - (int)BaseStatusDeleted.Cascade));
                                emp.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(emp);
                            }
                            agency.Deleted = (delete) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                            agency.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                            Manager.SaveOrUpdate(agency);
                           
                        }
                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }

                public Boolean PhisicalDeleteAgency(long idAgency)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Agency agency = Manager.Get<Agency>(idAgency);
                        List<AgencyAffiliation> employees = null;
                        List<OrganizationAffiliation> affiliations = null;
                        if (agency != null && !agency.IsEmpty && !agency.IsDefault)
                        {
                            employees = (from p in Manager.GetIQ<AgencyAffiliation>() where p.Agency == agency select p).ToList();
                            affiliations = (from p in Manager.GetIQ<OrganizationAffiliation>() where p.Agency == agency select p).ToList();
                            Manager.DeletePhysicalList(employees);
                            Manager.DeletePhysicalList(affiliations);
                            Manager.DeletePhysical(agency);
                        }
                        //else
                        //{
                        //    employees = (from p in Manager.GetIQ<BaseLoginInfo>() where p.Person == null && p.Deleted != BaseStatusDeleted.None select p).ToList();
                        //    affiliations = (from p in Manager.GetIQ<WaitingActivationProfile>() where p.Person == null && p.Deleted != BaseStatusDeleted.None select p).ToList();
                        //}
                       
                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }
                //public Dictionary<Int32, String> GetAvailableOrganizations()
                //{
                //    Dictionary<Int32, String> items = null;
                //    try
                //    {
                //        items = (from o in Manager.GetIQ<Organization>() select o).ToDictionary(o => o.Id, o => o.Name);
                //    }
                //    catch (Exception ex)
                //    {
                //        items = new Dictionary<Int32, String>();
                //    }
                //    return items;
                //}


                public List<ExternalCell> ValidateAgencyDataToImport(DestinationItem<Int32> item, List<ExternalCell> cells)
                {
                    List<ExternalCell> errors = new List<ExternalCell>();
                    try{
                        foreach(ExternalCell cell in cells.Where(c=>!c.isEmpty).ToList()){
                            var query = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None select a);
                             switch (item.Id)
                            {
                                case (int)ImportedAgencyColumn.name:
                                    query = query.Where(a => a.Name == cell.Value);
                                    break;
                                case (int)ImportedAgencyColumn.externalCode:
                                    query = query.Where(a => a.ExternalCode == cell.Value);
                                    break;
                                case (int)ImportedAgencyColumn.nationalCode:
                                    query = query.Where(a => a.NationalCode == cell.Value);
                                    break;
                                case (int)ImportedAgencyColumn.taxCode:
                                    query = query.Where(a => a.TaxCode == cell.Value);
                                    break;
                            }
                            if (query.Any())
                                errors.Add(cell);
                        }
                    }
                    catch(Exception ex){
                    
                    }
                    return errors;
                }

                public List<ExternalCell> ValidateAgencyDataToImport(List<DestinationItemCells<Int32>> items)
                {
                    List<ExternalCell> errors = new List<ExternalCell>();
                    try{

                        foreach (DestinationItemCells<Int32> item in items) {
                        //    if (item.HasAlternative)
                        //    {
                        //        foreach (ExternalCell cell in item.Cells.Where(c => !c.isEmpty).ToList())
                        //        {
                        //            List<ExternalCell> evaluateCells = 

                        //            var query = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None select a);
                        //            switch (item.Id)
                        //            {
                        //                case (int)ImportedAgencyColumn.name:
                        //                    query = query.Where(a => a.Name == cell.Value);
                        //                    break;
                        //                case (int)ImportedAgencyColumn.externalCode:
                        //                    query = query.Where(a => a.ExternalCode == cell.Value);
                        //                    break;
                        //                case (int)ImportedAgencyColumn.nationalCode:
                        //                    query = query.Where(a => a.NationalCode == cell.Value);
                        //                    break;
                        //                case (int)ImportedAgencyColumn.taxCode:
                        //                    query = query.Where(a => a.TaxCode == cell.Value);
                        //                    break;
                        //            }
                        //            if (query.Any())
                        //                errors.Add(cell);
                        //        }
                        //    }
                        //    else
                                errors.AddRange(ValidateAgencyDataToImport(item, item.Cells));
                        }
                    }
                    catch(Exception ex){
                    
                    }
                    return errors;
                }


                public Agency GetAgency(Dictionary<ProfileAttributeType, string> items)
                {
                    Agency agency = null;
                    try
                    {
                        var query = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None select a);
                        foreach (var item in items.Where(i=> !String.IsNullOrEmpty(i.Value)).ToList()) {
                            switch (item.Key)
                            {
                                case ProfileAttributeType.agencyTaxCode:
                                    query = query.Where(a => a.TaxCode == item.Value);
                                    break;
                                case ProfileAttributeType.agencyName:
                                    query = query.Where(a => a.Name == item.Value);
                                    break;
                                case ProfileAttributeType.agencyExternalCode:
                                    query = query.Where(a => a.ExternalCode == item.Value);
                                    break;
                                case ProfileAttributeType.agencyNationalCode:
                                    query = query.Where(a => a.NationalCode == item.Value);
                                    break;
                                case ProfileAttributeType.agencyInternalCode:
                                    query = query.Where(a => a.Id == Int32.Parse(item.Value));
                                    break;
                            }
                        }
                        if (items.Where(i => !String.IsNullOrEmpty(i.Value) && i.Key != ProfileAttributeType.agencyName).Any()) {
                            agency = query.ToList().FirstOrDefault();
                        }
 
                    }
                    catch (Exception ex) { 
                    
                    }
                    return agency;
                }
                public Agency SaveAgency(Int32 idProfile,Dictionary<ProfileAttributeType, string> items)
                {
                    Agency agency = null;
                    try
                    {
                        dtoAgency dto = new dtoAgency();
                        dto.IsDefault = false;
                        dto.IsEditable = true;
                        dto.IsEmpty = false;
                        dto.Name= (items.ContainsKey(ProfileAttributeType.agencyName) ? items[ProfileAttributeType.agencyName] :
                            "");
                        if (items.ContainsKey(ProfileAttributeType.agencyExternalCode))
                            dto.ExternalCode = items[ProfileAttributeType.agencyExternalCode];
                        if (items.ContainsKey(ProfileAttributeType.agencyNationalCode))
                            dto.NationalCode = items[ProfileAttributeType.agencyNationalCode];
                        if (items.ContainsKey(ProfileAttributeType.agencyTaxCode))
                            dto.TaxCode = items[ProfileAttributeType.agencyTaxCode];
                        if (String.IsNullOrEmpty(dto.Name) && !String.IsNullOrEmpty(dto.ExternalCode))
                            dto.Name = dto.ExternalCode;
                        else if (String.IsNullOrEmpty(dto.Name) && !String.IsNullOrEmpty(dto.NationalCode))
                            dto.Name = dto.NationalCode;
                        else if (String.IsNullOrEmpty(dto.Name) && !String.IsNullOrEmpty(dto.TaxCode))
                            dto.Name = dto.TaxCode;
                        dto.AlwaysAvailable = true;
                        agency = SaveAgency(idProfile,dto);
                    }
                    catch (Exception ex)
                    {

                    }
                    return agency;
                }

                public List<dtoAvailableAffiliation> GetAgencyAvailableOrganizations(long idAgency=0)
                {
                    List<dtoAvailableAffiliation> items = new List<dtoAvailableAffiliation>();
                    try
                    {
                        Person person = Manager.GetPerson(UC.CurrentUserID);
                        if (person!=null){
                            items = (from o in GetDisplayOrganizations(person) select new dtoAvailableAffiliation() { Organization= o }).ToList();
                            List<OrganizationAffiliation> affiliations = (from a in Manager.GetIQ<OrganizationAffiliation>() where a.Deleted == BaseStatusDeleted.None && a.Agency != null && a.Agency.Id == idAgency select a).ToList();
                            foreach (OrganizationAffiliation a in affiliations) {
                                dtoAvailableAffiliation item = items.Where(i => i.Organization.Id == a.IdOrganization).FirstOrDefault();
                                if (item !=null)
                                {
                                    item.IsDefault = a.IsDefault;
                                    item.IsEmpty = a.IsEmpty;
                                    item.Id = a.Id;
                                }
                                else {
                                    item = new dtoAvailableAffiliation();
                                    item.IsEditable = false;
                                    item.IsDefault = a.IsDefault;
                                    item.IsEmpty = a.IsEmpty;
                                    item.Id = a.Id;
                                    item.Organization = Manager.Get<Organization>(a.IdOrganization);
                                    if (item.Organization != null)
                                        items.Add(item);
                                }
                            }
                            items = items.OrderBy(o => o.Organization.Name).ToList();
                        }
                    }
                    catch (Exception ex) { 
                      
                    }
                    return items;
                }

                public Dictionary<Int32,String> GetDisplayOrganizations() {
                    return GetDisplayOrganizations(Manager.GetPerson(UC.CurrentUserID)).ToDictionary(o => o.Id, o => o.Name);
                }
                public List<Organization> GetDisplayOrganizations(Person person)
                {
                    List<Organization> organizations = null;
                    if (person != null)
                    {
                        List<Int32> idOrganizations = null;
                        if (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator)
                        {
                            organizations = (from o in Manager.GetIQ<Organization>() orderby o.Name select o).ToList();
                        }
                        else
                        {
                            idOrganizations = (from o in Manager.GetIQ<OrganizationProfiles>() where o.Profile == person select o.OrganizationID).ToList();
                            organizations = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) orderby o.Name select o).ToList();
                        }
                    }
                    else
                        organizations = new List<Organization>();
                    return organizations;
                }
            #endregion
         #endregion
                #region "Edit Profiles"
                public Boolean SaveProfileMailPolicy(Int32 idProfile, List<Int32> idSelected, List<Int32> previousCommunitiesPolicy)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        List<LazySubscription> subscriptions = (from s in Manager.GetIQ<LazySubscription>()
                                                                where s.IdPerson == idProfile && idSelected.Contains(s.IdCommunity) && s.DisplayMail == false
                                                                select s).ToList();
                        if (subscriptions.Count > 0) {
                            foreach (LazySubscription sub in subscriptions) {
                                sub.DisplayMail = true;
                            }
                            Manager.SaveOrUpdateList(subscriptions);
                        }
                        previousCommunitiesPolicy = previousCommunitiesPolicy.Except(idSelected).ToList();
                        List<LazySubscription> unSubscriptions = (from s in Manager.GetIQ<LazySubscription>()
                                                                  where s.IdPerson == idProfile && previousCommunitiesPolicy.Contains(s.IdCommunity) && s.DisplayMail
                                                                select s).ToList();
                        if (unSubscriptions.Count > 0)
                        {
                            foreach (LazySubscription sub in unSubscriptions)
                            {
                                sub.DisplayMail = false;
                            }
                            Manager.SaveOrUpdateList(unSubscriptions);
                        }
                        Manager.Commit();
                        result = true;
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                    }
                    return result;
                }
                public Boolean isPendingMailUnique(Int32 idUser, String mail)
                {
                    Boolean found = false;
                    Person person = Manager.GetPerson(idUser);
                    found = (from p in Manager.GetIQ<Person>()
                              where p.Id != idUser && p.Mail == mail
                              select p.Id).Any();

                    found = found || (from p in Manager.GetIQ<MailEditingPending>()
                                        where p.Person != null && p.Person != person && p.Deleted == BaseStatusDeleted.None && p.Mail == mail
                                        select p).Any();
                    return !found;
                }
                public Boolean HasMailEditingPendings(Person person)
                {
                    return (from p in Manager.GetIQ<MailEditingPending>() where p.Person==person && p.Deleted==  BaseStatusDeleted.None select p).Any();
                }
                public MailEditingPending LastEditingPending(Person person)
                {
                    return (from p in Manager.GetIQ<MailEditingPending>() where p.Person == person && p.Deleted == BaseStatusDeleted.None orderby p.CreatedOn descending  select p).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                public MailEditingPending GetEditingPending(Person person, String activationCode)
                {
                    return (from p in Manager.GetIQ<MailEditingPending>() where p.Person == person && p.ActivationCode == activationCode orderby p.CreatedOn descending select p).Skip(0).Take(1).ToList().FirstOrDefault();
                }   
                public MailEditingPending GetEditingPending(Person person, System.Guid urlIdentifier)
                {
                    return (from p in Manager.GetIQ<MailEditingPending>() where p.Person == person && p.UrlIdentifier == urlIdentifier orderby p.CreatedOn descending select p).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                public List<MailEditingPending> GetEditingPendings( System.Guid urlIdentifier)
                {
                    return (from p in Manager.GetIQ<MailEditingPending>() where  p.UrlIdentifier == urlIdentifier orderby p.CreatedOn descending select p).ToList();
                }
                public MailEditingPending SavePendingChanges(Int32 idUser, String mail)
                {
                    MailEditingPending pending = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(idUser);
                        if (person != null && !String.IsNullOrEmpty(mail)) { 
                            pending = new MailEditingPending();
                            pending.CreatedOn = DateTime.Now;
                            pending.Mail = mail;
                            if (!String.IsNullOrEmpty(pending.Mail))
                                pending.Mail = pending.Mail.ToLower();
                            pending.Person = person;
                            pending.UrlIdentifier = Guid.NewGuid();
                            pending.Deleted = BaseStatusDeleted.None;
                            pending.ActivationCode = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10,true, true, false);
                            Manager.SaveOrUpdate(pending);
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                        pending = null;
                    }
                    return pending;
                }
                public MailEditingPending ActivateMailPendingChange(Int32 idUser, long idPending, String code)
                {
                    MailEditingPending pending = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(idUser);
                        pending = Manager.Get<MailEditingPending>(idPending);
                        if (person != null && pending != null && pending.ActivationCode == code)
                        {
                            pending.Deleted = BaseStatusDeleted.Manual;
                            person.Mail = pending.Mail;
                            if (!String.IsNullOrEmpty(person.Mail))
                                person.Mail = person.Mail.ToLower();
                            Manager.SaveOrUpdate(pending);
                            Manager.SaveOrUpdate(person);

                            List<MailEditingPending> pendings = (from p in Manager.GetIQ<MailEditingPending>() where p.Person == person && p.Deleted == BaseStatusDeleted.None select p).ToList();
                            foreach (MailEditingPending p in pendings){
                                p.Deleted= BaseStatusDeleted.Manual;
                            }
                            Manager.SaveOrUpdateList(pendings);
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        pending = null;
                    }
                    return pending;
                }
                public Boolean ActivateMailPendingChange(MailEditingPending pending)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        Manager.Refresh(pending);
                        if (pending != null && pending.Person !=null )
                        {
                            pending.Deleted = BaseStatusDeleted.Manual;
                            pending.Person.Mail = pending.Mail;
                            if (!String.IsNullOrEmpty(pending.Mail))
                                pending.Mail = pending.Mail.ToLower();
                            Manager.SaveOrUpdate(pending.Person);
                            Manager.SaveOrUpdate(pending);

                            List<MailEditingPending> pendings = (from p in Manager.GetIQ<MailEditingPending>() where p.Person == pending.Person && p.Deleted == BaseStatusDeleted.None select p).ToList();
                            foreach (MailEditingPending p in pendings)
                            {
                                p.Deleted = BaseStatusDeleted.Manual;
                            }
                            Manager.SaveOrUpdateList(pendings);
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                public MailEditingPending RenewPendingChangesCode(Int32 idUser, long idPending)
                {
                    MailEditingPending pending = null;
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(idUser);
                        pending = Manager.Get<MailEditingPending>(idPending);
                        if (person != null && pending != null )
                        {
                            pending.ActivationCode = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, true, true, false);
                            Manager.SaveOrUpdate(pending);
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        pending = null;
                    }
                    return pending;
                }

                public Boolean UpdateAvatar(Int32 idUser,String avatar)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(idUser);
                        if (person != null)
                        {
                            person.FotoPath = avatar;
                            Manager.SaveOrUpdate(person);
                        }

                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }
                public Boolean UpdateFirstLetter(Int32 idUser)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(idUser);
                        if (person != null)
                        {
                            person.FirstLetter= person.Surname[0].ToString().ToLower();
                            Manager.SaveOrUpdate(person);
                        }

                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }
                public Boolean DisableProfile(int IdUser) {
                    try{
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(IdUser);
                        if (person!=null){
                            person.isDisabled=true;
                            Manager.SaveOrUpdate(person);
                        }
                       
                        Manager.Commit();
                        return true;
                    }
                    catch(Exception ex){
                        Manager.RollBack();
                        return false;
                    }
                }
                public Boolean EnableProfile(int IdUser) {
                    try{
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(IdUser);
                        if (person!=null){
                            person.isDisabled=false;
                            List<ExternalLoginInfo> logins = (from p in Manager.GetIQ<ExternalLoginInfo>() where p.Person == person && p.isEnabled== false && p.CreatedBy== p.Person  select p).ToList();
                            foreach (ExternalLoginInfo login in logins)
                            {
                                login.isEnabled = true;
                                Manager.SaveOrUpdate(login);
                            }
                            List<WaitingActivationProfile> waitings = (from p in Manager.GetIQ<WaitingActivationProfile>() where p.Person == person select p).ToList();
                            if (waitings.Count>0)
                                Manager.DeletePhysical(waitings);
                            Manager.SaveOrUpdate(person);
                        }
                        Manager.Commit();
                        return true;
                    }
                    catch(Exception ex){
                        Manager.RollBack();
                        return false;
                    }
                }
                public Boolean ActivateProfile(int IdUser)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(IdUser);
                        if (person != null)
                        {
                            person.isDisabled = false;
                            List<ExternalLoginInfo> logins = (from p in Manager.GetIQ<ExternalLoginInfo>() where p.Person == person && p.isEnabled == false && p.CreatedBy == p.Person select p).ToList();
                            foreach (ExternalLoginInfo login in logins)
                            {
                                login.isEnabled = true;
                                Manager.SaveOrUpdate(login);
                                UrlService.AddToHistory(login);
                            }
                            List<WaitingActivationProfile> items = (from p in Manager.GetIQ<WaitingActivationProfile>() where p.Person == person select p).ToList();
                            if (items.Count > 0)
                                Manager.DeletePhysicalList(items);
                            Manager.SaveOrUpdate(person);
                        }
                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }

                public Boolean VirtualDeleteProfileInfo(int IdUser)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(IdUser);
                        if (person != null)
                        {
                            List<BaseLoginInfo> logins = (from p in Manager.GetIQ<BaseLoginInfo>() where p.Person == person select p).ToList();
                            foreach (BaseLoginInfo login in logins)
                            {
                                login.Deleted = BaseStatusDeleted.Manual;
                                Manager.SaveOrUpdate(login);
                                if (typeof(InternalLoginInfo) == login.GetType())
                                    InternalService.AddToHistory((InternalLoginInfo)login);
                                else
                                    UrlService.AddToHistory((ExternalLoginInfo)login);
                            }

                            List<WaitingActivationProfile> items = (from p in Manager.GetIQ<WaitingActivationProfile>() where p.Person == person select p).ToList();
                            foreach (WaitingActivationProfile waiting in items)
                            {
                                waiting.Deleted = BaseStatusDeleted.Manual;
                                Manager.SaveOrUpdate(waiting);
                            }
                        }
                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }
          
                public Boolean PhisicalDeleteProfileInfo(int IdUser)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(IdUser);
                        List<BaseLoginInfo> logins = null;
                        List<WaitingActivationProfile> items = null;
                        if (person != null)
                        {
                            logins = (from p in Manager.GetIQ<BaseLoginInfo>() where p.Person == person select p).ToList();
                            items = (from p in Manager.GetIQ<WaitingActivationProfile>() where p.Person == person select p).ToList();
                        }
                        else {
                            logins = (from p in Manager.GetIQ<BaseLoginInfo>() where p.Person == null && p.Deleted!= BaseStatusDeleted.None  select p).ToList();
                            items = (from p in Manager.GetIQ<WaitingActivationProfile>() where p.Person == null && p.Deleted != BaseStatusDeleted.None select p).ToList();
                        }
                        foreach (BaseLoginInfo l in logins) {
                            if (typeof(InternalLoginInfo) == l.GetType())
                                InternalService.AddToHistory((InternalLoginInfo)l);
                            else
                                UrlService.AddToHistory((ExternalLoginInfo)l);
                        }
                        Manager.DeletePhysicalList(logins);
                        Manager.DeletePhysicalList(items);
                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        return false;
                    }
                }
            #endregion
            #region "Profile Management"
                #region "Subscriptions"
                    public List<dtoProfileOrganization> GetProfileOrganizations(Person person)
                    {
                        List<dtoProfileOrganization> items = null;
                        try
                        {
                            items = (from op in Manager.GetIQ<OrganizationProfiles>()
                                     where op.Profile == person
                                     select new dtoProfileOrganization() { isDefault = op.isDefault, IdOrganization = op.OrganizationID }).ToList();
                            List<Int32> idOrganizations = items.Select(i => i.IdOrganization).ToList();

                            var query = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select o).ToList();

                            items.ForEach(d => d.Name = (from q in query where q.Id == d.IdOrganization select q.Name).FirstOrDefault());
                        }
                        catch (Exception ex)
                        {
                            items = new List<dtoProfileOrganization>();
                        }
                        return items.Where(i => string.IsNullOrEmpty(i.Name) == false).ToList();
                    }
                    public long GetProfileSubscriptionsCount(lm.Comol.Core.Subscriptions.dtoSubscriptionFilters filters,Person person)
                    {
                        long count = (long)0;
                        try
                        {
                            Person otherUser = null;
                            if (filters.IdOwner>0 ||(filters.SearchBy == Subscriptions.SearchSubscriptionsBy.Owner || filters.SearchBy == Subscriptions.SearchSubscriptionsBy.Responsible)) {
                                otherUser = Manager.GetPerson(filters.IdOwner);
                            }
                            var query = GetSubscriptionQuery(filters, person, otherUser);

                            count = query.Count();
                        }
                        catch (Exception ex)
                        {

                        }
                        return count;
                    }
                    public List<SubscriptionStatus> GetAvailableStatus(lm.Comol.Core.Subscriptions.dtoSubscriptionFilters filters, Person person)
                    {
                        List<SubscriptionStatus> list = new List<SubscriptionStatus>();

                        Person otherUser = null;
                        if (filters.IdOwner > 0 || (filters.SearchBy == Subscriptions.SearchSubscriptionsBy.Owner || filters.SearchBy == Subscriptions.SearchSubscriptionsBy.Responsible))
                        {
                            otherUser = Manager.GetPerson(filters.IdOwner);
                        }

                        
                        SubscriptionStatus previousStatus = filters.Status;
                        filters.Status = SubscriptionStatus.all;

                        if (GetSubscriptionQuery(filters, person, otherUser).Any())
                            list.Add(SubscriptionStatus.activemember);
                        filters.Status = SubscriptionStatus.blocked;
                        if (GetSubscriptionQuery(filters, person, otherUser).Any())
                            list.Add(SubscriptionStatus.blocked);
                        filters.Status = SubscriptionStatus.waiting;
                        if (GetSubscriptionQuery(filters, person, otherUser).Any())
                            list.Add(SubscriptionStatus.waiting);
                        filters.Status = SubscriptionStatus.responsible;
                        if (GetSubscriptionQuery(filters, person, otherUser).Any())
                            list.Add(SubscriptionStatus.responsible);
                        filters.Status = previousStatus;
                        return list;
                    }
                    public List<lm.Comol.Core.Subscriptions.dtoBaseSubscription> GetProfileSubscriptions(lm.Comol.Core.Subscriptions.dtoSubscriptionFilters filters, Person person,Int32 pageIndex, Int32 pageSize)
                    {
                        List<lm.Comol.Core.Subscriptions.dtoBaseSubscription> items = null;
                        try
                        {
                            Person otherUser = null;
                            if (filters.IdOwner > 0 || (filters.SearchBy == Subscriptions.SearchSubscriptionsBy.Owner || filters.SearchBy == Subscriptions.SearchSubscriptionsBy.Responsible))
                            {
                                otherUser = Manager.GetPerson(filters.IdOwner);
                            }

                            var query = GetSearchSubscriptionQuery(filters, person, otherUser);

                            query = query.Where<Subscription>(s => s.Role != null && s.Role.Id > 0);

                            items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                     select new lm.Comol.Core.Subscriptions.dtoBaseSubscription()
                                     {
                                         CommunityName= p.Community.Name,
                                         Id= p.Id,
                                         IdCommunity= p.Community.Id,
                                         isEnabled= p.Enabled,
                                         isResponsabile= p.isResponsabile,
                                         LastAccessOn= p.LastAccessOn,
                                         Role= (Role)p.Role,
                                         SubscriptedOn= p.SubscriptedOn,
                                         Status = p.Status

                                         //Id = p.Profile.Id,
                                         //ProvidersCount = GetProvidersCount(p.Profile),
                                         //Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                         //TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                         //Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,
                                         //AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                         //AuthenticationType = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                     }).ToList();
                        }
                        catch (Exception ex)
                        {
                            items = new List<lm.Comol.Core.Subscriptions.dtoBaseSubscription>();
                        }
                        return items;
                    }

                    private IEnumerable<Subscription> GetSearchSubscriptionQuery(lm.Comol.Core.Subscriptions.dtoSubscriptionFilters filters, Person user, Person otherUser)
                    {
                        IEnumerable<Subscription> query = GetSubscriptionQuery(filters, user, otherUser);
                        if (filters.Ascending){
                            switch (filters.OrderBy){
                                case Subscriptions.OrderSubscriptionsBy.SubscriptionDate:
                                    query = query.OrderBy(s => s.SubscriptedOn).ThenBy(s => s.Community.Name);
                                    break;
                                case Subscriptions.OrderSubscriptionsBy.LastVisit:
                                    query = query.OrderBy(s => s.LastAccessOn).ThenBy(s=>s.Community.Name);
                                    break;
                                case Subscriptions.OrderSubscriptionsBy.Name:
                                    query = query.OrderBy(s => s.Community.Name);
                                    break;
                            }
                        }
                        else
                        {
                            switch (filters.OrderBy)
                            {
                                case Subscriptions.OrderSubscriptionsBy.SubscriptionDate:
                                    query = query.OrderByDescending(s => s.SubscriptedOn).ThenBy(s => s.Community.Name);
                                    break;
                                case Subscriptions.OrderSubscriptionsBy.LastVisit:
                                    query = query.OrderByDescending(s => s.LastAccessOn).ThenBy(s => s.Community.Name);
                                    break;
                                case Subscriptions.OrderSubscriptionsBy.Name:
                                    query = query.OrderByDescending(s => s.Community.Name);
                                    break;
                            }
                        }
                        return query;
                    }
                    private IEnumerable<Subscription> GetSubscriptionQuery(lm.Comol.Core.Subscriptions.dtoSubscriptionFilters filters, Person user, Person otherUser)
                    {
                        IEnumerable<Subscription> query = GetBaseSubscriptionQuery(filters, user, otherUser);
                        if ((filters.SearchBy !=  Subscriptions.SearchSubscriptionsBy.StartAs || string.IsNullOrEmpty(filters.Value)) && !string.IsNullOrEmpty(filters.StartWith))
                        {
                            if (filters.StartWith != "#")
                                query = query.Where<Subscription>(p => string.Compare(p.Community.Name[0].ToString().ToLower(), filters.StartWith.ToLower(), true) == 0);
                            else
                                query = query.Where<Subscription>(p => DefaultOtherChars().Contains(p.Community.Name[0].ToString().ToLower()));
                        }
                        return query;
                    }
                    private IEnumerable<Subscription> GetBaseSubscriptionQuery(lm.Comol.Core.Subscriptions.dtoSubscriptionFilters filters, Person user, Person otherUser) {
                        IEnumerable<Subscription> query = GetBaseSubscriptionQuery(filters, user);
                        if (filters.SearchBy == Subscriptions.SearchSubscriptionsBy.Responsible || otherUser !=null )
                        {
                            SubscriptionStatus previousStatus = filters.Status;
                            filters.Status = SubscriptionStatus.responsible;

                            var queryResponsible = (from s in GetBaseSubscriptionQuery(filters, otherUser) select s.Community.Id).ToList();
                            query = query.Where<Subscription>(s => queryResponsible.Contains(s.Community.Id));
                            filters.Status = previousStatus;
                        }
                        else if (filters.SearchBy == Subscriptions.SearchSubscriptionsBy.Owner)
                        {
                            var queryOwner = (from c in Manager.GetIQ<Community>() where c.Creator == otherUser select c.Id).ToList();
                            query = query.Where<Subscription>(s => queryOwner.Contains(s.Community.Id));
                        }

                        return query;
                    }
                    private IEnumerable<Subscription> GetBaseSubscriptionQuery(lm.Comol.Core.Subscriptions.dtoSubscriptionFilters filters, Person user)
                    {
                        IEnumerable<Subscription> query = (from s in Manager.GetIQ<Subscription>() where s.Person==user  select s);

                        if (filters.IdOrganization > 0)
                            query = query.Where<Subscription>(p => p.Community.IdOrganization == filters.IdOrganization);
                        if (filters.IdcommunityType > 0)
                            query = query.Where<Subscription>(s => s.Community.TypeOfCommunity.Id == filters.IdcommunityType);

                        switch (filters.Status)
                        {
                            case SubscriptionStatus.activemember:
                                query = query.Where<Subscription>(s=> s.Accepted && s.Enabled);
                                break;
                            case  SubscriptionStatus.blocked:
                                query = query.Where<Subscription>(s => s.Accepted && !s.Enabled);
                                break;
                            case  SubscriptionStatus.waiting:
                                query = query.Where<Subscription>(s => !s.Accepted && !s.Enabled);
                                break;
                            case SubscriptionStatus.responsible:
                                query = query.Where<Subscription>(s => s.isResponsabile);
                                break;
                        }
                        if (!string.IsNullOrEmpty(filters.Value) && string.IsNullOrEmpty(filters.Value.Trim()) == false)
                        {
                            switch (filters.SearchBy)
                            {
                                case Subscriptions.SearchSubscriptionsBy.Contains:
                                    query = query.Where<Subscription>(s => s.Community.Name.ToLower().Contains(filters.Value.ToLower()));
                                    break;
                                case Subscriptions.SearchSubscriptionsBy.StartAs:
                                    query = query.Where<Subscription>(s => s.Community.Name.ToLower().StartsWith(filters.Value.ToLower()));
                                    break;
                            }
                        }
                        return query;
                    }
                #endregion

                #region "User subscriptions"
                    public List<Int32> GetAvailableSubscriptionsIdRoles(Int32 idCommunity, List<Int32> removeIdUsers, Boolean alsoHidden = false)
                    {
                        List<Int32> roles = new List<Int32>();
                        try
                        {
                            if (removeIdUsers.Count <= maxItemsForQuery)
                            {
                                roles = (from t in Manager.GetIQ<LazySubscription>()
                                         where t.IdCommunity == idCommunity && ((alsoHidden && t.IdRole < 0) || (!alsoHidden && t.IdRole > 0)) && !removeIdUsers.Contains(t.IdPerson)
                                         select t.IdRole).Distinct().ToList();
                            }
                            else {
                                roles = (from t in Manager.GetIQ<LazySubscription>()
                                         where t.IdCommunity == idCommunity && ((alsoHidden && t.IdRole < 0) || (!alsoHidden && t.IdRole > 0))
                                         select new {IdRole = t.IdRole, IdPerson = t.IdPerson}).ToList().Where(t=>!removeIdUsers.Contains(t.IdPerson)).Select(t=>t.IdRole).Distinct().ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                            roles = new List<Int32>();
                        }

                        return roles;
                    }
                    public List<SubscriptionStatus> GetAvailableSubscriptionsStatus(Int32 idCommunity, List<Int32> idUsersToRemove, Boolean alsoHidden = false)
                    {
                        List<SubscriptionStatus> list = new List<SubscriptionStatus>();
                        var query = GetBaseSubscriptionsQuery(idCommunity,-1, alsoHidden);

                        if (idUsersToRemove.Count <= maxItemsForQuery)
                        {
                            if (query.Where(s => s.Accepted && s.Enabled).Any())
                                list.Add(SubscriptionStatus.activemember);

                            if (query.Where(s => s.Accepted && !s.Enabled).Any())
                                list.Add(SubscriptionStatus.communityblocked);

                            if ((from s in Manager.GetIQ<LazySubscription>() where s.IdCommunity == idCommunity && ((alsoHidden && s.IdRole < 0) || (!alsoHidden && s.IdRole > 0)) && !s.Accepted select s.Id).Any())
                                list.Add(SubscriptionStatus.waiting);
                        }
                        else {
                            if (query.Where(s => s.Accepted && s.Enabled).ToList().Where(s=> s.Person != null && !idUsersToRemove.Contains(s.Person.Id)).Any())
                                list.Add(SubscriptionStatus.activemember);

                            if (query.Where(s => s.Accepted && !s.Enabled).ToList().Where(s => s.Person != null && !idUsersToRemove.Contains(s.Person.Id)).Any())
                                list.Add(SubscriptionStatus.communityblocked);

                            if ((from s in Manager.GetIQ<LazySubscription>() where s.IdCommunity == idCommunity && ((alsoHidden && s.IdRole < 0) || (!alsoHidden && s.IdRole > 0)) && !s.Accepted select s.IdPerson).ToList().Where(s => !idUsersToRemove.Contains(s)).Any())
                                list.Add(SubscriptionStatus.waiting);
                        }
                        if (list.Count > 1)
                            list.Insert(0, SubscriptionStatus.all);

                        return list;
                    }
                    public List<Int32> GetAvailableProfileTypes(Int32 idCommunity, Int32 idRole, SubscriptionStatus status, Boolean alsoHidden = false)
                    {
                        List<Int32> result = null;
                        try
                        {
                            var query = GetBaseSubscriptionsQuery(idCommunity, idRole, status, alsoHidden);
                            result = query.Select(s => s.Person.TypeID).Distinct().ToList();
                        }
                        catch (Exception ex)
                        {
                            result = new List<Int32>();
                        }
                        return result;
                    }
                    public Dictionary<long, String> GetAvailableAgencies(Int32 idCommunity, Int32 idRole, SubscriptionStatus status, Boolean alsoHidden = false)
                    {
                        Dictionary<long, String> list = new Dictionary<long, String>();
                        try
                        {
                            var query = GetBaseSubscriptionsQuery(idCommunity, idRole, status, alsoHidden);
                            List<Agency> agencies = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None select a).ToList();
                            List<Int32> idUsers = query.Select(s => s.Person.Id).ToList();
                            List<long> idUserAgencies = new List<long>();

                            if (idUsers.Count() <= maxItemsForQuery)
                            {
                                idUserAgencies = (from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && idUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList();
                            }
                            else {
                                Int32 index = 0;
                                List<Int32> tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                while (tUsers.Any())
                                {
                                    idUserAgencies.AddRange((from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && tUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList());
                                    index++;
                                    tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                }
                                idUserAgencies = idUserAgencies.Distinct().ToList();
                            }
                            list = agencies.Where(a => idUserAgencies.Contains(a.Id)).OrderBy(a => a.Name).ToDictionary(a => a.Id, a => a.Name);
                        }
                        catch (Exception ex)
                        {

                        }
                        return list;
                    }
                    public List<String> GetAvailableSubsriptionStartLetter(dtoUserFilters filters, List<Int32> idUsersToRemove)
                    {
                        List<String> letters = new List<String>();
                        try
                        {
                            Manager.BeginTransaction();
                            var query = GetBaseSubscriptionsQuery(filters);

                            if (idUsersToRemove.Count <= maxItemsForQuery)
                                letters = (from p in query where !idUsersToRemove.Contains(p.Person.Id) && !String.IsNullOrEmpty(p.Person.FirstLetter) select p.Person.FirstLetter).Distinct().ToList();
                            else
                                letters = (from p in query where !String.IsNullOrEmpty(p.Person.FirstLetter) select new { IdProfile = p.Person.Id, Letter = p.Person.FirstLetter }).ToList().Where(p => !idUsersToRemove.Contains(p.IdProfile)).Select(p => p.Letter).Distinct().ToList();
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return letters;
                    }
                    public long CommunitySubscriptionsCount(dtoUserFilters filters, List<Int32> removeIdUsers = null, Boolean alsoHidden = false)
                    {
                        long count = (long)0;
                        try
                        {
                            var query = GetSubscriptionsQuery(filters, alsoHidden);
                            if (removeIdUsers == null || !removeIdUsers.Any())
                                count = query.Count();
                            else if (removeIdUsers.Count <= maxItemsForQuery)
                                count = (from p in query where !removeIdUsers.Contains(p.Person.Id) select p).Count();
                            else
                                count = query.Select(q => q.Person.Id).ToList().Where(p => !removeIdUsers.Contains(p)).Count();
                        }
                        catch (Exception ex)
                        {

                        }
                        return count;
                    }

                    public List<dtoSubscriptionProfileItem<dtoBaseProfile>> GetProfiles(dtoUserFilters filters, Int32 pageIndex, Int32 pageSize, List<TranslatedItem<Int32>> profileTypes, List<TranslatedItem<Int32>> roles, List<Int32> removeIdUsers = null)
                    {
                        List<dtoSubscriptionProfileItem<dtoBaseProfile>> items = null;
                        try
                        {
                            var query = GetSearchSubscriptionsQuery(filters);
                            if (removeIdUsers == null || !removeIdUsers.Any())
                                #region "Standard"
                                items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoBaseProfile>(new dtoBaseProfile()
                                         {
                                             Id = p.Person.Id,
                                             TaxCode = p.Person.TaxCode,
                                             Mail = p.Person.Mail,
                                             Surname = p.Person.Surname,
                                             Name = p.Person.Name,
                                             IdLanguage = p.Person.LanguageID,
                                             IdProfileType = p.Person.TypeID
                                         })
                                         {
                                             Id = p.Person.Id,
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status =p.Status,
                                             RoleName= (from r in roles where r.Id == p.Role.Id  select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion
                            else if (removeIdUsers.Count <= maxItemsForQuery)
                                #region "In query remove"
                                items = (from p in query.Where(p => !removeIdUsers.Contains(p.Person.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoBaseProfile>(new dtoBaseProfile()
                                         {
                                             Id = p.Person.Id,
                                             TaxCode = p.Person.TaxCode,
                                             Mail = p.Person.Mail,
                                             Surname = p.Person.Surname,
                                             Name = p.Person.Name,
                                             IdLanguage = p.Person.LanguageID,
                                             IdProfileType = p.Person.TypeID
                                         })
                                         {
                                             Id = p.Person.Id,
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status = p.Status,
                                             RoleName = (from r in roles where r.Id == p.Role.Id select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion
                            else
                                #region "After query remove users"
                                items = (from p in query.ToList().Where(p => !removeIdUsers.Contains(p.Person.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoBaseProfile>(new dtoBaseProfile()
                                         {
                                             Id = p.Person.Id,
                                             TaxCode = p.Person.TaxCode,
                                             Mail = p.Person.Mail,
                                             Surname = p.Person.Surname,
                                             Name = p.Person.Name,
                                             IdLanguage = p.Person.LanguageID,
                                             IdProfileType = p.Person.TypeID
                                         })
                                         {
                                             Id = p.Person.Id,
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status = p.Status,
                                             RoleName = (from r in roles where r.Id == p.Role.Id select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion

                        }
                        catch (Exception ex)
                        {
                            items = new List<dtoSubscriptionProfileItem<dtoBaseProfile>>();
                        }
                        return items;
                    }
                    public List<dtoSubscriptionProfileItem<dtoCompany>> GetCompanyUserProfiles(dtoUserFilters filters, Int32 pageIndex, Int32 pageSize, List<TranslatedItem<Int32>> profileTypes, List<TranslatedItem<Int32>> roles, List<Int32> removeIdUsers = null)
                    {
                        List<dtoSubscriptionProfileItem<dtoCompany>> items = null;
                        try
                        {
                            var query = GetSearchSubscriptionsQuery(filters);
                            if (removeIdUsers == null || !removeIdUsers.Any())
                                #region "Standard"
                                items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoCompany>(new dtoCompany()
                                         {
                                             Id = p.Person.Id,
                                             TaxCode = p.Person.TaxCode,
                                             Mail = p.Person.Mail,
                                             Surname = p.Person.Surname,
                                             Name = p.Person.Name,
                                             IdLanguage = p.Person.LanguageID,
                                             IdProfileType = p.Person.TypeID,
                                             Info = (typeof(CompanyUser) == p.Person.GetType()) ? ((CompanyUser)p.Person).CompanyInfo : new CompanyInfo()
                                         })
                                         {
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status =p.Status,
                                             RoleName= (from r in roles where r.Id == p.Role.Id  select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion
                            else if (removeIdUsers.Count <= maxItemsForQuery)
                                #region "In query remove"
                                items = (from p in query.Where(p => !removeIdUsers.Contains(p.Person.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoCompany>(new dtoCompany()
                                         {
                                             Id = p.Person.Id,
                                             TaxCode = p.Person.TaxCode,
                                             Mail = p.Person.Mail,
                                             Surname = p.Person.Surname,
                                             Name = p.Person.Name,
                                             IdLanguage = p.Person.LanguageID,
                                             IdProfileType = p.Person.TypeID,
                                             Info = (typeof(CompanyUser) == p.Person.GetType()) ? ((CompanyUser)p.Person).CompanyInfo : new CompanyInfo()
                                         })
                                         {
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status =p.Status,
                                             RoleName= (from r in roles where r.Id == p.Role.Id  select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion
                            else
                                #region "After query remove users"
                                items = (from p in query.ToList().Where(p => !removeIdUsers.Contains(p.Person.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoCompany>(new dtoCompany()
                                         {
                                             Id = p.Person.Id,
                                             TaxCode = p.Person.TaxCode,
                                             Mail = p.Person.Mail,
                                             Surname = p.Person.Surname,
                                             Name = p.Person.Name,
                                             IdLanguage = p.Person.LanguageID,
                                             IdProfileType = p.Person.TypeID,
                                             Info = (typeof(CompanyUser) == p.Person.GetType()) ? ((CompanyUser)p.Person).CompanyInfo : new CompanyInfo()
                                         })
                                         {
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status =p.Status,
                                             RoleName= (from r in roles where r.Id == p.Role.Id  select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion
                        }
                        catch (Exception ex)
                        {
                            items = new List<dtoSubscriptionProfileItem<dtoCompany>>();
                        }
                        return items;
                    }
                    public List<dtoSubscriptionProfileItem<dtoEmployee>> GetEmployeeProfiles(dtoUserFilters filters, Int32 pageIndex, Int32 pageSize, List<TranslatedItem<Int32>> profileTypes, List<TranslatedItem<Int32>> roles, List<Int32> removeIdUsers = null)
                    {
                        List<dtoSubscriptionProfileItem<dtoEmployee>> items = null;
                        try
                        {
                            var query = GetSearchSubscriptionsQuery(filters);
                            if (removeIdUsers == null || !removeIdUsers.Any())
                                #region "Standard"
                                items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoEmployee>(new dtoEmployee(((Employee)p.Person)))
                                         {
                                             Id = p.Person.Id,
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status = p.Status,
                                             RoleName = (from r in roles where r.Id == p.Role.Id select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion
                            else if (removeIdUsers.Count <= maxItemsForQuery)
                                #region "In query remove"
                                items = (from p in query.Where(p => !removeIdUsers.Contains(p.Person.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoEmployee>(new dtoEmployee(((Employee)p.Person)))
                                         {
                                             Id = p.Person.Id,
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status = p.Status,
                                             RoleName = (from r in roles where r.Id == p.Role.Id select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion
                            else
                                #region "After query remove users"
                                items = (from p in query.ToList().Where(p => !removeIdUsers.Contains(p.Person.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                         select new dtoSubscriptionProfileItem<dtoEmployee>(new dtoEmployee(((Employee)p.Person)))
                                         {
                                             Id = p.Person.Id,
                                             TypeName = (from t in profileTypes where t.Id == p.Person.TypeID select t.Translation).FirstOrDefault(),
                                             Status = p.Status,
                                             RoleName = (from r in roles where r.Id == p.Role.Id select r.Translation).FirstOrDefault()
                                         }).ToList();
                                #endregion
                        }
                        catch (Exception ex)
                        {
                            items = new List<dtoSubscriptionProfileItem<dtoEmployee>>();
                        }
                        return items;
                    }


                    public List<Int32> GetSubscriptionsIdUsers(List<Int32> idUsersToRemove, Boolean allSubscriptions, dtoUserFilters filter, List<Int32> selectedIdUsers)
                    {
                        List<Int32> items = null;
                        try
                        {
                            if (allSubscriptions)
                            {
                                var query = GetSubscriptionsQuery(filter);
                                items = query.Select(s => s.Person.Id).ToList();
                            }
                            else
                                items = selectedIdUsers;
                            if (items.Any())
                                items = items.Where(i => !idUsersToRemove.Contains(i)).ToList();
                        }
                        catch (Exception ex)
                        {
                            items = new List<Int32>();
                        }
                        return items;
                    }
                    public IEnumerable<Subscription> GetSearchSubscriptionsQuery(dtoUserFilters filters)
                    {
                        IEnumerable<Subscription> query = GetSubscriptionsQuery(filters);
                        if (filters.Ascending)
                            query = query.OrderBy(s => s.Person.Surname).ThenBy(p => p.Person.Name);
                        else
                            query = query.OrderByDescending(p => p.Person.Surname).ThenBy(p => p.Person.Name);
                        return query;
                    }
                    private IEnumerable<Subscription> GetSubscriptionsQuery(dtoUserFilters filters, Boolean alsoHidden = false)
                    {
                        IEnumerable<Subscription> query = GetBaseSubscriptionsQuery(filters, alsoHidden);
                        if ((filters.SearchBy == SearchProfilesBy.Name || filters.SearchBy == SearchProfilesBy.All || filters.SearchBy == SearchProfilesBy.Contains || string.IsNullOrEmpty(filters.Value)) && !string.IsNullOrEmpty(filters.StartWith))
                        {
                            if (filters.StartWith != "#")
                                query = query.Where<Subscription>(p => p.Person.FirstLetter == filters.StartWith.ToLower());
                            else
                                query = query.Where<Subscription>(p => DefaultOtherChars().Contains(p.Person.FirstLetter));
                        }
                        return query;
                    }
                    private IEnumerable<Subscription> GetBaseSubscriptionsQuery(Int32 idCommunity, Int32 idRole, Boolean alsoHidden = false)
                    {
                        var query = (from s in Manager.GetIQ<Subscription>() where s.Community.Id == idCommunity select s);
                        if (idRole != -1 && idRole != 0)
                            query = query.Where(s => s.Role.Id == idRole);
                        else
                            query = query.Where(s => ((alsoHidden && s.Role.Id < 0) || (!alsoHidden && s.Role.Id > 0)));
                        return query;
                    }
                    private IEnumerable<Subscription> GetBaseSubscriptionsQuery(Int32 idCommunity, Int32 idRole, SubscriptionStatus status, Boolean alsoHidden = false)
                    {
                        var query = GetBaseSubscriptionsQuery(idCommunity, idRole, alsoHidden);
                        switch (status)
                        {
                            case SubscriptionStatus.communityblocked:
                                query = query.Where(s => s.Accepted && !s.Enabled);
                                break;
                            case SubscriptionStatus.activemember:
                                query = query.Where(s => s.Accepted && s.Enabled);
                                break;
                            case SubscriptionStatus.waiting:
                                query = query.Where(s => !s.Accepted && s.Enabled);
                                break;
                        }
                        return query;
                    }
                    private IEnumerable<Subscription> GetBaseSubscriptionsQuery(dtoUserFilters filters, Boolean alsoHidden = false)
                    {
                        IEnumerable<Subscription> query = (from s in Manager.GetIQ<Subscription>() where filters.IdCommunities.Contains(s.Community.Id) select s);

                        if (filters.IdRole > 0)
                            query = query.Where<Subscription>(p => p.Role.Id == filters.IdRole);
                        else
                            query = query.Where(s => ((alsoHidden && s.Role.Id < 0) || (!alsoHidden && s.Role.Id > 0)));

                        if (filters.IdProfileType > 0)
                            query = query.Where<Subscription>(p => p.Person.TypeID == filters.IdProfileType);
                        if (filters.IdAgency == -2 && filters.IdProfileType <1)
                            query = query.Where<Subscription>(p => p.Person.TypeID == (int)UserTypeStandard.Employee);
                        if (filters.IdAgency == -3 && filters.IdProfileType < 1)
                            query = query.Where<Subscription>(p => p.Person.TypeID != (int)UserTypeStandard.Employee);
                        switch (filters.Status)
                        {
                            case SubscriptionStatus.activemember:
                                query = query.Where<Subscription>(s => s.Accepted && s.Enabled );
                                break;
                            case SubscriptionStatus.communityblocked:
                                query = query.Where<Subscription>(s => s.Accepted && !s.Enabled );
                                break;
                            case SubscriptionStatus.waiting:
                                query = query.Where<Subscription>(s => !s.Accepted);
                                break;
                        }
                        if (filters.IdAgency > 0)
                        {
                            List<Int32> userAgencies = (from uf in Manager.GetIQ<AgencyAffiliation>()
                                                        where uf.Deleted == BaseStatusDeleted.None && uf.IsEnabled && uf.Agency != null && uf.Agency.Id == filters.IdAgency && uf.Employee != null
                                                        select uf.Employee.Id).ToList();
                            List<Int32> idUsers = query.Select(s => s.Person.Id).ToList();
                            userAgencies = userAgencies.Where(i=> idUsers.Contains(i)).ToList();

                            query = query.Where<Subscription>(p => userAgencies.Contains(p.Person.Id));
                        }
                        if (!string.IsNullOrEmpty(filters.Value) && string.IsNullOrEmpty(filters.Value.Trim()) == false)
                        {
                            switch (filters.SearchBy)
                            {
                                case SearchProfilesBy.Contains:
                                    List<String> items = filters.Value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                                    if (items.Any() && items.Count == 1)
                                        query = query.Where<Subscription>(p => p.Person.Name.ToLower().Contains(filters.Value.ToLower()) || p.Person.Surname.ToLower().Contains(filters.Value.ToLower()));
                                    else if (items.Any() && items.Count > 1)
                                        query = query.Where<Subscription>(p => items.Any(p.Person.Name.ToLower().Contains) && items.Any(p.Person.Surname.ToLower().Contains));
                                    break;
                                case SearchProfilesBy.Mail:
                                    query = query.Where<Subscription>(p => p.Person.Mail.ToLower().Contains(filters.Value.ToLower()));
                                    break;
                                case SearchProfilesBy.Name:
                                    query = query.Where<Subscription>(p => p.Person.Name.ToLower().StartsWith(filters.Value.ToLower()));
                                    break;
                                case SearchProfilesBy.Surname:
                                    query = query.Where<Subscription>(p => p.Person.Surname.ToLower().StartsWith(filters.Value.ToLower()));
                                    break;
                                case SearchProfilesBy.TaxCode:
                                    query = query.Where<Subscription>(p => p.Person.TaxCode.ToLower().Contains(filters.Value.ToLower()));
                                    break;
                            }
                        }
                        return query;
                    }

                #endregion
                    public ProfileTypeChanger EditProfileType(ProfileTypeChanger profile, Int32 idType) {
                    try
                    {
                        Manager.BeginTransaction();
                        if (idType == (int)UserTypeStandard.Company)
                            profile.Discriminator = idType;
                        else if (idType == (int)UserTypeStandard.Employee)
                            profile.Discriminator = idType;
                        else
                            profile.Discriminator = 0;
                        profile.TypeID = idType;
                        Manager.SaveOrUpdate(profile);
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                    }
                    return profile;
                }

                #region "Display"
                    public long ProfilesCount(dtoFilters filters, List<Int32> removeIdUsers=null) {
                        long count = (long)0;
                        try
                        {
                            var query = GetProfileQuery(filters);
                            if (removeIdUsers == null || !removeIdUsers.Any())
                                count = query.Count();
                            else if (removeIdUsers.Count <= maxItemsForQuery)
                                count = (from p in query where !removeIdUsers.Contains(p.Profile.Id) select p).Count();
                            else
                                count = query.Select(q => q.Profile.Id).ToList().Where(p => !removeIdUsers.Contains(p)).Count();
                        }
                        catch (Exception ex) { 
                
                        }
                        return count;
                    }
                    public long GetProvidersCount(Person person)
                    {
                        return (from li in Manager.GetIQ<BaseLoginInfo>() where li.Person == person select li.Provider.Id).Distinct().Count();
                    }

                    public Dictionary<Int32, String> GetProfilesLogin(List<Int32> idItems)
                    {
                        Dictionary<Int32, String> logins = new Dictionary<Int32, String>();
                        try
                        {
                            logins = (from l in Manager.GetIQ<InternalLoginInfo>()
                                      where l.Deleted == BaseStatusDeleted.None && l.Person != null && idItems.Contains(l.Person.Id)
                                      select l).ToDictionary(l => l.Person.Id, l => l.Login);
                        }
                        catch (Exception ex)
                        {

                        }
                        return logins;
                    }
                public List<dtoProfileItem<dtoBaseProfile>> GetProfiles(dtoFilters filters, Int32 pageIndex, Int32 pageSize, List<TranslatedItem<Int32>> profileTypes, List<dtoBaseProvider> providers, List<Int32> removeIdUsers = null)
                {
                    List<dtoProfileItem<dtoBaseProfile>> items = null;
                    try
                    {
                        var queryWaiting = (from wp in Manager.GetIQ<WaitingActivationProfile>() select wp.Person.Id).ToList();
                        var query = GetSearchProfileQuery(filters);
                        if (filters.Ascending)
                            query = query.OrderBy(p => p.Profile.Surname).ThenBy(p=>p.Profile.Name) ;
                        else
                            query = query.OrderByDescending(p => p.Profile.Surname).ThenBy(p => p.Profile.Name);

                        Int32 loaderType = UC.UserTypeID;
                        if (removeIdUsers == null || !removeIdUsers.Any())
                            #region "Standard"
                            items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                     select new dtoProfileItem<dtoBaseProfile>(new dtoBaseProfile()
                                     {
                                         Id = p.Profile.Id,
                                         IdDefaultProvider = p.Profile.IdDefaultProvider,
                                         TaxCode = p.Profile.TaxCode,
                                         Mail = p.Profile.Mail,
                                         Surname = p.Profile.Surname,
                                         Name = p.Profile.Name,
                                         IdLanguage = p.Profile.LanguageID,
                                         IdProfileType = p.Profile.TypeID,
                                         Login = p.Profile.Login
                                     })
                                     {
                                         Id = p.Profile.Id,
                                         ProvidersCount = GetProvidersCount(p.Profile),
                                         Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                         TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                         Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,
                                         AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                         AuthenticationType = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                     }).ToList();
                            #endregion
                        else if (removeIdUsers.Count <= maxItemsForQuery)
                            #region "In query remove"
                            items = (from p in query.Where(p => !removeIdUsers.Contains(p.Profile.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                 select new dtoProfileItem<dtoBaseProfile>(new dtoBaseProfile()
                                 {
                                     Id = p.Profile.Id,
                                     IdDefaultProvider = p.Profile.IdDefaultProvider,
                                     TaxCode = p.Profile.TaxCode,
                                     Mail = p.Profile.Mail,
                                     Surname = p.Profile.Surname,
                                     Name = p.Profile.Name,
                                     IdLanguage= p.Profile.LanguageID,
                                     IdProfileType = p.Profile.TypeID,
                                     Login = p.Profile.Login
                                 }) {
                                     Id = p.Profile.Id,
                                     ProvidersCount = GetProvidersCount(p.Profile) ,
                                     Permission= new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                     TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                     Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting ,
                                     AuthenticationTypeName = (from pr in providers where pr.IdProvider== p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                     AuthenticationType= (from pr in providers where pr.IdProvider== p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                 }).ToList();
                            #endregion
                        else
                            #region "After query remove users"
                            items = (from p in query.ToList().Where(p => !removeIdUsers.Contains(p.Profile.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                 select new dtoProfileItem<dtoBaseProfile>(new dtoBaseProfile()
                                 {
                                     Id = p.Profile.Id,
                                     IdDefaultProvider = p.Profile.IdDefaultProvider,
                                     TaxCode = p.Profile.TaxCode,
                                     Mail = p.Profile.Mail,
                                     Surname = p.Profile.Surname,
                                     Name = p.Profile.Name,
                                     IdLanguage = p.Profile.LanguageID,
                                     IdProfileType = p.Profile.TypeID,
                                     Login = p.Profile.Login
                                 })
                                 {
                                     Id = p.Profile.Id,
                                     ProvidersCount = GetProvidersCount(p.Profile),
                                     Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                     TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                     Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,
                                     AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                     AuthenticationType = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                 }).ToList();
                            #endregion

                    }
                    catch (Exception ex)
                    {
                        items = new List<dtoProfileItem<dtoBaseProfile>>();
                    }
                    return items;
                }
                public List<Person> GetPersonsByType(Int32 idType, Boolean onlyActive)
                {
                    List<Person> items = null;
                    try
                    {
                        List<Int32> waitingUsers = (onlyActive) ? (from wp in Manager.GetIQ<WaitingActivationProfile>() select wp.Person.Id).ToList() : new List<Int32>();

                        items = (from p in Manager.GetIQ<Person>() where p.TypeID == idType && (!onlyActive || (onlyActive && !p.isDisabled)) select p).ToList().Where(p => !waitingUsers.Contains(p.Id)).ToList();
                        
                    }
                    catch (Exception ex)
                    {
                        items = new List<Person>();
                    }
                    return items;
                }
                public List<litePerson> GetLitePersonsByType(Int32 idType, Boolean onlyActive)
                {
                    List<litePerson> items = null;
                    try
                    {
                        List<Int32> waitingUsers = (onlyActive) ? (from wp in Manager.GetIQ<WaitingActivationProfile>() select wp.Person.Id).ToList() : new List<Int32>();

                        items = (from p in Manager.GetIQ<litePerson>() where p.TypeID == idType && (!onlyActive || (onlyActive && !p.isDisabled)) select p).ToList().Where(p => !waitingUsers.Contains(p.Id)).ToList();

                    }
                    catch (Exception ex)
                    {
                        items = new List<litePerson>();
                    }
                    return items;
                }
                public List<dtoProfileItem<dtoCompany>> GetCompanyUserProfiles(dtoFilters filters, Int32 pageIndex, Int32 pageSize, List<TranslatedItem<Int32>> profileTypes, List<dtoBaseProvider> providers, List<Int32> removeIdUsers = null)
                 {
                     List<dtoProfileItem<dtoCompany>> items = null;
                     try
                     {
                         var queryWaiting = (from wp in Manager.GetIQ<WaitingActivationProfile>() select wp.Person.Id).ToList();
                         var query = GetSearchProfileQuery(filters);
                        

                         Int32 loaderType = UC.UserTypeID;

                         if (removeIdUsers == null || !removeIdUsers.Any())
                             #region "Standard"
                             items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                      select new dtoProfileItem<dtoCompany>(new dtoCompany()
                                      {
                                          Id = p.Profile.Id,
                                          IdDefaultProvider = p.Profile.IdDefaultProvider,
                                          TaxCode = p.Profile.TaxCode,
                                          Mail = p.Profile.Mail,
                                          Surname = p.Profile.Surname,
                                          Name = p.Profile.Name,
                                          IdProfileType = p.Profile.TypeID,
                                          Login = p.Profile.Login
                                          ,
                                          Info = (typeof(CompanyUser) == p.Profile.GetType()) ? ((CompanyUser)p.Profile).CompanyInfo : new CompanyInfo()
                                      })
                                      {
                                          Id = p.Profile.Id,
                                          ProvidersCount = GetProvidersCount(p.Profile),
                                          Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                          TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                          Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,

                                          AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                          AuthenticationType = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                      }).ToList();
                             #endregion
                         else if (removeIdUsers.Count <= maxItemsForQuery)
                             #region "In query remove"
                              items = (from p in query.Where(p => !removeIdUsers.Contains(p.Profile.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                  select new dtoProfileItem<dtoCompany>(new dtoCompany()
                                  {
                                      Id = p.Profile.Id,
                                      IdDefaultProvider = p.Profile.IdDefaultProvider,
                                      TaxCode = p.Profile.TaxCode,
                                      Mail = p.Profile.Mail,
                                      Surname = p.Profile.Surname,
                                      Name = p.Profile.Name,
                                      IdProfileType = p.Profile.TypeID,
                                      Login = p.Profile.Login
                                      ,
                                      Info = (typeof(CompanyUser) == p.Profile.GetType()) ? ((CompanyUser)p.Profile).CompanyInfo : new CompanyInfo()
                                  })
                                  {
                                      Id = p.Profile.Id,
                                      ProvidersCount = GetProvidersCount(p.Profile),
                                      Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                      TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                      Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,

                                      AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                      AuthenticationType= (from pr in providers where pr.IdProvider== p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                  }).ToList();
                             #endregion
                         else
                             #region "After query remove users"
                             items = (from p in query.ToList().Where(p => !removeIdUsers.Contains(p.Profile.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                      select new dtoProfileItem<dtoCompany>(new dtoCompany()
                                      {
                                          Id = p.Profile.Id,
                                          IdDefaultProvider = p.Profile.IdDefaultProvider,
                                          TaxCode = p.Profile.TaxCode,
                                          Mail = p.Profile.Mail,
                                          Surname = p.Profile.Surname,
                                          Name = p.Profile.Name,
                                          IdProfileType = p.Profile.TypeID,
                                          Login = p.Profile.Login
                                          ,
                                          Info = (typeof(CompanyUser) == p.Profile.GetType()) ? ((CompanyUser)p.Profile).CompanyInfo : new CompanyInfo()
                                      })
                                      {
                                          Id = p.Profile.Id,
                                          ProvidersCount = GetProvidersCount(p.Profile),
                                          Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                          TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                          Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,

                                          AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                          AuthenticationType = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                      }).ToList();
                             #endregion
                     }
                     catch (Exception ex)
                     {
                         items = new List<dtoProfileItem<dtoCompany>>();
                     }
                     return items;
                 }
                public List<dtoProfileItem<dtoEmployee>> GetEmployeeProfiles(dtoFilters filters, Int32 pageIndex, Int32 pageSize, List<TranslatedItem<Int32>> profileTypes, List<dtoBaseProvider> providers, List<Int32> removeIdUsers = null)
                {
                    List<dtoProfileItem<dtoEmployee>> items = null;
                    try
                    {
                        var queryWaiting = (from wp in Manager.GetIQ<WaitingActivationProfile>() select wp.Person.Id).ToList();
                        var query = GetSearchProfileQuery(filters);


                        Int32 loaderType = UC.UserTypeID;

                        if (removeIdUsers == null || !removeIdUsers.Any())
                            #region "Standard"
                            items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                     select new dtoProfileItem<dtoEmployee>(new dtoEmployee(((Employee)p.Profile)))
                                     {
                                         Id = p.Profile.Id,
                                         ProvidersCount = GetProvidersCount(p.Profile),
                                         Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                         TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                         Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,

                                         AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                         AuthenticationType = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                     }).ToList();
                            #endregion
                        else if (removeIdUsers.Count <= maxItemsForQuery)
                            #region "In query remove"
                              items = (from p in query.Where(p => !removeIdUsers.Contains(p.Profile.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                 select new dtoProfileItem<dtoEmployee>(new dtoEmployee(((Employee)p.Profile)))
                                 {
                                     Id = p.Profile.Id,
                                     ProvidersCount = GetProvidersCount(p.Profile),
                                     Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                     TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                     Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,

                                     AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                     AuthenticationType = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                 }).ToList();
                            #endregion
                        else
                            #region "After query remove users"
                            items = (from p in query.ToList().Where(p => !removeIdUsers.Contains(p.Profile.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                     select new dtoProfileItem<dtoEmployee>(new dtoEmployee(((Employee)p.Profile)))
                                     {
                                         Id = p.Profile.Id,
                                         ProvidersCount = GetProvidersCount(p.Profile),
                                         Permission = new dtoProfilePermission(loaderType, p.Profile.TypeID),
                                         TypeName = (from t in profileTypes where t.Id == p.Profile.TypeID select t.Translation).FirstOrDefault(),
                                         Status = (!p.Profile.isDisabled) ? StatusProfile.Active : (p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id)) ? StatusProfile.Disabled : StatusProfile.Waiting,

                                         AuthenticationTypeName = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Translation.Name).FirstOrDefault(),
                                         AuthenticationType = (from pr in providers where pr.IdProvider == p.Profile.IdDefaultProvider select pr.Type).FirstOrDefault()
                                     }).ToList();
                            #endregion
                    }
                    catch (Exception ex)
                    {
                        items = new List<dtoProfileItem<dtoEmployee>>();
                    }
                    return items;
                }

                public List<Int32> GetIdUsers(List<Int32> idUsersToRemove, Boolean allSubscriptions, dtoFilters filter, List<Int32> selectedIdUsers)
                {
                    List<Int32> items = null;
                    try
                    {
                        if (allSubscriptions)
                        {
                            var query = GetSearchProfileQuery(filter);
                            items = query.Select(s => s.Profile.Id).ToList();
                        }
                        else
                            items = selectedIdUsers;
                        if (items.Any())
                            items = items.Where(i => !idUsersToRemove.Contains(i)).ToList();
                    }
                    catch (Exception ex)
                    {
                        items = new List<Int32>();
                    }
                    return items;
                }
                private List<dtoBaseProfile> GetBaseProfiles(dtoFilters filters, Int32 pageIndex, Int32 pageSize)
                {
                    List<dtoBaseProfile> items = null;
                    try
                    {
                        var queryWaiting = (from wp in Manager.GetIQ<WaitingActivationProfile>() select wp.Person.Id).ToList();
                        var query = GetSearchProfileQuery(filters);
                        if (filters.idProvider== (int) UserTypeStandard.Company)
                            items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                     select (dtoBaseProfile) new dtoCompany()
                                  {
                                      Id = p.Profile.Id,
                                      IdDefaultProvider = p.Profile.IdDefaultProvider,
                                      TaxCode = p.Profile.TaxCode,
                                      Mail = p.Profile.Mail,
                                      Surname = p.Profile.Surname,
                                      Name = p.Profile.Name,
                                      IdProfileType = p.Profile.TypeID,
                                      Login = p.Profile.Login
                                      ,
                                      Info = (typeof(CompanyUser) == p.Profile.GetType()) ? ((CompanyUser)p.Profile).CompanyInfo : new CompanyInfo()
                                  }).ToList();
                        else
                             items = (from p in query.Skip(pageIndex * pageSize).Take(pageSize).ToList()
                                 select new dtoBaseProfile()
                                 {
                                     Id = p.Profile.Id,
                                     IdDefaultProvider = p.Profile.IdDefaultProvider,
                                     TaxCode = p.Profile.TaxCode,
                                     Mail = p.Profile.Mail,
                                     Surname = p.Profile.Surname,
                                     Name = p.Profile.Name
                                     ,
                                     IdProfileType = p.Profile.TypeID,
                                     Login = p.Profile.Login
                                 }).ToList();
                    }
                    catch (Exception ex)
                    {
                        items = new List<dtoBaseProfile>();
                    }
                    return items;
                }
            
                /// <summary>
                ///     Get profiles by user Id
                /// </summary>
                /// <param name="idUsers"></param>
                /// <param name="value"></param>
                /// <param name="startWith"></param>
                /// <param name="maxItemsToLoad"></param>
                /// <returns></returns>
                public List<dtoBaseProfile> GetBaseProfiles(List<Int32> idUsers, String value = "", String startWith = "", Int32 maxItemsToLoad = 0)
                {
                    return GetBaseProfiles(idUsers,  new dtoFilters() { Ascending = true, OrderBy = OrderProfilesBy.SurnameAndName, Value = value, SearchBy = SearchProfilesBy.Contains, StartWith = startWith }, maxItemsToLoad);
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="idUsers"></param>
                /// <param name="filter"></param>
                /// <param name="maxItemsToLoad"></param>
                /// <returns></returns>
                public List<dtoBaseProfile> GetBaseProfiles(List<Int32> idUsers,  dtoFilters filter, Int32 maxItemsToLoad = 0)
                {
                    List<dtoBaseProfile> items = null;
                    try
                    {
                        var query = GetSearchProfileQuery(filter);
                        if (filter.Ascending)
                            query = query.OrderBy(p => p.Profile.Surname).ThenBy(p=>p.Profile.Name) ;
                        else
                            query = query.OrderByDescending(p => p.Profile.Surname).ThenBy(p => p.Profile.Name);
                        if (idUsers.Count > maxItemsForQuery || (idUsers.Count > maxItemsForQuery && maxItemsToLoad > maxItemsForQuery)) {
                            items = query.Select(p => new dtoBaseProfile()
                            {
                                Id = p.Profile.Id,
                                TaxCode = p.Profile.TaxCode,
                                Mail = p.Profile.Mail,
                                Surname = p.Profile.Surname,
                                Name = p.Profile.Name,
                            }).ToList().Where(i => idUsers.Contains(i.Id)).ToList();

                            if (maxItemsToLoad > 0)
                                items = items.Skip(0).Take(maxItemsToLoad).ToList();
                        }
                        else{
                            items = query.Where(p=> idUsers.Contains(p.Profile.Id)).Select(p=> new dtoBaseProfile()
                                    {
                                        Id = p.Profile.Id,
                                        TaxCode = p.Profile.TaxCode,
                                        Mail = p.Profile.Mail,
                                        Surname = p.Profile.Surname,
                                        Name = p.Profile.Name,
                                    }).ToList();
                            if (maxItemsToLoad > 0)
                                items = items.Skip(0).Take(maxItemsToLoad).ToList();

                        }
                    }
                    catch (Exception ex)
                    {
                        items = new List<dtoBaseProfile>();
                    }
                    return items;
                }

                public Int32 GetBaseProfilesCount(List<Int32> idUsers, String value = "", String startWith = "")
                {
                    return GetBaseProfilesCount(idUsers, new dtoFilters() { Ascending = true, OrderBy = OrderProfilesBy.SurnameAndName, Value = value, SearchBy = SearchProfilesBy.Contains, StartWith = startWith });
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="idUsers"></param>
                /// <param name="filter"></param>
                /// <param name="maxItemsToLoad"></param>
                /// <returns></returns>
                public Int32 GetBaseProfilesCount(List<Int32> idUsers, dtoFilters filter)
                {
                    Int32 count = 0;
                    try
                    {
                        var query = GetSearchProfileQuery(filter);
                        if (filter.Ascending)
                            query = query.OrderBy(p => p.Profile.Surname).ThenBy(p => p.Profile.Name);
                        else
                            query = query.OrderByDescending(p => p.Profile.Surname).ThenBy(p => p.Profile.Name);
                        if (idUsers.Count > maxItemsForQuery)
                            count = query.Select(p => p.Profile.Id).ToList().Where(i => idUsers.Contains(i)).Count();
                        else
                            count = query.Where(p => idUsers.Contains(p.Profile.Id)).Count();
                    }
                    catch (Exception ex)
                    {
                    }
                    return count;
                }
                public List<String> GetAvailableStartLetter(List<Int32> idUsers, String value = "", String startWith = "")
                {
                    return GetAvailableStartLetter(idUsers, new dtoFilters() { Ascending = true, OrderBy = OrderProfilesBy.SurnameAndName, Value = value, SearchBy = SearchProfilesBy.Contains, StartWith = startWith });
                }
                public List<String> GetAvailableStartLetter(List<Int32> idUsers, dtoFilters filter)
                {
                    List<String> letters = new List<String>();
                    try
                    {
                        Manager.BeginTransaction();
                        var query = GetBaseProfileQuery(filter);

                        if (idUsers.Count > maxItemsForQuery)
                        {
                            var pQuery = query.Select(p => new { IdProfile = p.Profile.Id, Letter = p.Profile.FirstLetter }).ToList();
                            letters = pQuery.Where(p => idUsers.Contains(p.IdProfile)).Select(p => p.Letter).Distinct().ToList();
                        }
                        else
                            letters = query.Where(p => idUsers.Contains(p.Profile.Id)).Select(p => p.Profile.FirstLetter).Distinct().ToList();
                       
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return letters;
                }

                public List<String> GetAvailableStartLetter(dtoFilters filters)
                {
                    List<String> letters = new List<String>();
                    try
                    {
                        Manager.BeginTransaction();
                        var query = GetBaseProfileQuery(filters);
                        if (query.Where(p => String.IsNullOrEmpty(p.Profile.FirstLetter)).Any()) {
                            query.Where(p => String.IsNullOrEmpty(p.Profile.FirstLetter)).ToList().ForEach(p => p.Profile.FirstLetter = p.Profile.Surname[0].ToString().ToLower());
                            Manager.SaveOrUpdateList(query.Where(p => String.IsNullOrEmpty(p.Profile.FirstLetter)).ToList());
                            
                        }
                        letters = (from p in query select p.Profile.FirstLetter).Distinct().ToList();
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return letters;
                }
                public List<String> GetAvailableStartLetter(dtoFilters filters, List<Int32> idUsersToRemove)
                {
                    List<String> letters = new List<String>();
                    try
                    {
                        Manager.BeginTransaction();
                        var query = GetBaseProfileQuery(filters);
                        
                        if (idUsersToRemove.Count <= maxItemsForQuery)
                            letters = (from p in query where !idUsersToRemove.Contains(p.Profile.Id) && !String.IsNullOrEmpty(p.Profile.FirstLetter) select p.Profile.FirstLetter).Distinct().ToList();
                        else
                            letters = (from p in query where !String.IsNullOrEmpty(p.Profile.FirstLetter) select new { IdProfile = p.Profile.Id, Letter = p.Profile.FirstLetter }).ToList().Where(p => !idUsersToRemove.Contains(p.IdProfile)).Select(p => p.Letter).Distinct().ToList();
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return letters;
                }
                //public List<AlphabetItem> GetAvailableStartLetter(dtoFilters filters,List<Int32> idUsersToRemove)
                //{
                //    List<AlphabetItem> items = (from n in Enumerable.Range(97, 26) select new AlphabetItem() { isEnabled = false, Value = Char.ConvertFromUtf32(n), DisplayName = Char.ConvertFromUtf32(n).ToString().ToUpper(), isSelected = false }).ToList();
                //    Boolean hasOtherChars = false;
                //    try
                //    {
                //        Manager.BeginTransaction();
                //        var query = GetBaseProfileQuery(filters);
                //        List<String> letters = new List<String>();
                //        if (idUsersToRemove.Count <= maxItemsForQuery)
                //            letters = (from p in query where !idUsersToRemove.Contains(p.Profile.Id) && !String.IsNullOrEmpty(p.Profile.FirstLetter) select p.Profile.FirstLetter).Distinct().ToList();
                //        else
                //            letters = (from p in query where !String.IsNullOrEmpty(p.Profile.FirstLetter) select new { IdProfile = p.Profile.Id, Letter = p.Profile.FirstLetter }).ToList().Where(p=> !idUsersToRemove.Contains(p.IdProfile)).Select(p=> p.Letter).Distinct().ToList();

                //        if (letters.Any()) {
                //            List<AlphabetItem> otherChars = GetOtherAlphabetItems();
                //            foreach (String l in letters) {
                //                if (items.Where(i => i.Value == l).Any())
                //                    items.Where(i => i.Value == l).FirstOrDefault().isEnabled = true;
                //                else if (System.Text.RegularExpressions.Regex.IsMatch(l, @"[^\w\.@-]", System.Text.RegularExpressions.RegexOptions.None))
                //                {
                //                    String upper = "";
                //                    try
                //                    {
                //                        upper = l.ToUpper();
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        upper = l;
                //                    }
                //                    items.Add(new AlphabetItem() { isEnabled = true, Value = l, DisplayName = upper });
                //                }
                //                else if (otherChars.Where(i => i.Value == l).Any())
                //                    items.AddRange(otherChars.Where(i => i.Value == l).ToList());
                //                else
                //                    hasOtherChars = true;
                //            }
                //        }
                //        Manager.Commit();
                //    }
                //    catch (Exception ex)
                //    {
                //        Manager.RollBack();
                //    }
                //    items = items.OrderBy(i => i.Value).ToList();
                //    items.Insert(0, new AlphabetItem() { DisplayAs = AlphabetItem.AlphabetItemDisplayAs.first, isEnabled=true , Type = AlphabetItemType.all});
                //    items.Insert(1, new AlphabetItem() { Type= AlphabetItemType.otherChars, isEnabled= hasOtherChars, Value="#", DisplayName=""});
                //    items.LastOrDefault().DisplayAs = AlphabetItem.AlphabetItemDisplayAs.last;
                //    return items;
                //}
                public IEnumerable<OrganizationProfiles> GetSearchProfileQuery(dtoFilters filters)
                {
                    IEnumerable<OrganizationProfiles> query = GetProfileQuery(filters);
                    if (filters.Ascending)
                        query = query.OrderBy(p => p.Profile.Surname).ThenBy(p => p.Profile.Name);
                    else
                        query = query.OrderByDescending(p => p.Profile.Surname).ThenBy(p => p.Profile.Name);
                    return query;
                }
                private IEnumerable<OrganizationProfiles> GetProfileQuery(dtoFilters filters)
                {
                    IEnumerable<OrganizationProfiles> query = GetBaseProfileQuery(filters);
                    if ((filters.SearchBy == SearchProfilesBy.Name || filters.SearchBy == SearchProfilesBy.All || filters.SearchBy== SearchProfilesBy.Contains || string.IsNullOrEmpty(filters.Value)) && !string.IsNullOrEmpty(filters.StartWith))
                    {
                        if (filters.StartWith != "#")
                            query = query.Where<OrganizationProfiles>(p => p.Profile.FirstLetter== filters.StartWith.ToLower());
                        else
                            query = query.Where<OrganizationProfiles>(p => DefaultOtherChars().Contains(p.Profile.FirstLetter));
                    }
                    return query;
                }
                private IEnumerable<OrganizationProfiles> GetBaseProfileQuery(dtoFilters filters)
                    {
                        IEnumerable<OrganizationProfiles> query = (from p in Manager.GetIQ<OrganizationProfiles>() select p);
                        var queryWaiting = (from wp in Manager.GetIQ<WaitingActivationProfile>() select wp.Person.Id).ToList();

                        if (filters.IdOrganization > 0)
                            query = query.Where<OrganizationProfiles>(p => p.OrganizationID == filters.IdOrganization);
                        else if (UC.UserTypeID == (int)UserTypeStandard.Administrator || UC.UserTypeID == (int)UserTypeStandard.SysAdmin)
                        {
                            query = query.Where<OrganizationProfiles>(p => p.isDefault);
                        }
                        else { 
                            query = query.Where<OrganizationProfiles>(p => p.isDefault && filters.IdAvailableOrganization.Contains(p.OrganizationID));
                        }
                        if (filters.IdProfileType > 0)
                            query = query.Where<OrganizationProfiles>(p => p.Profile.TypeID == filters.IdProfileType);
                        if (filters.idProvider > 0)
                            query = query.Where<OrganizationProfiles>(p => p.Profile.IdDefaultProvider == filters.idProvider);
                        switch (filters.Status)
                        {
                            case StatusProfile.Active:
                                query = query.Where<OrganizationProfiles>(p => p.Profile.isDisabled == false);
                                break;
                            case StatusProfile.Disabled:
                                query = query.Where<OrganizationProfiles>(p => p.Profile.isDisabled && !queryWaiting.Contains(p.Profile.Id));
                                break;
                            case StatusProfile.Waiting:
                                query = query.Where<OrganizationProfiles>(p => p.Profile.isDisabled && queryWaiting.Contains(p.Profile.Id));
                                break;
                        }
                        if (filters.IdAgency > 0) {

                            List<Int32> userAgencies = (from uf in Manager.GetIQ<AgencyAffiliation>()
                                                        where uf.Deleted==  BaseStatusDeleted.None && uf.IsEnabled && uf.Agency!=null && uf.Agency.Id==filters.IdAgency && uf.Employee !=null select uf.Employee.Id).ToList();
                            query = query.Where<OrganizationProfiles>(p => userAgencies.Contains(p.Profile.Id));
                        }
                        if (!string.IsNullOrEmpty(filters.Value) && string.IsNullOrEmpty(filters.Value.Trim()) == false)
                        {
                            switch (filters.SearchBy)
                            {
                                case SearchProfilesBy.Contains:
                                    List<String> items = filters.Value.Split(' ').ToList().Where(f=> !String.IsNullOrEmpty(f)).Select(f=>f.ToLower()).ToList();
                                    if (items.Any() && items.Count==1)
                                        query = query.Where<OrganizationProfiles>(p => p.Profile.Name.ToLower().Contains(filters.Value.ToLower()) || p.Profile.Surname.ToLower().Contains(filters.Value.ToLower()));
                                    else if (items.Any() && items.Count > 1)
                                        query = query.Where<OrganizationProfiles>(p => items.Any(p.Profile.Name.ToLower().Contains) && items.Any(p.Profile.Surname.ToLower().Contains));
                                    break;
                                case SearchProfilesBy.Login:
                                    List<Int32> logins = (from m in Manager.GetIQ<InternalLoginInfo>() where m.Deleted == BaseStatusDeleted.None && m.Login.Contains(filters.Value) select m.Person.Id).ToList();
                                    query = query.Where<OrganizationProfiles>(p => logins.Contains(p.Profile.Id));
                                    break;
                                case SearchProfilesBy.Mail:
                                    query = query.Where<OrganizationProfiles>(p => p.Profile.Mail.ToLower().Contains(filters.Value.ToLower()));
                                    break;
                                case SearchProfilesBy.Name:
                                    query = query.Where<OrganizationProfiles>(p => p.Profile.Name.ToLower().StartsWith(filters.Value.ToLower()));
                                    break;
                                case SearchProfilesBy.Surname:
                                    query = query.Where<OrganizationProfiles>(p => p.Profile.Surname.ToLower().StartsWith(filters.Value.ToLower()));
                                    break;
                                case SearchProfilesBy.TaxCode:
                                    query = query.Where<OrganizationProfiles>(p => p.Profile.TaxCode.ToLower().Contains(filters.Value.ToLower()));
                                    break;
                            }
                        }
                        return query;
                    }

                public List<string> DefaultChars(Boolean all =false)
                {
                    List<string> chars = new List<string>();
                   
                    // maiuscole
                    for (int i = 65; i <= 90; i++)
                    {
                        chars.Add(Char.ConvertFromUtf32(i));
                    }
                    // minuscole
                    for (int i = 97; i <= 122; i++)
                    {
                        chars.Add(Char.ConvertFromUtf32(i));
                    }
                    if (all)
                        chars.AddRange((from n in Enumerable.Range(222, 34) select Char.ConvertFromUtf32(n)).ToList());
                    return chars;
                }
                public List<string> DefaultOtherChars()
                {
                    List<string> chars = new List<string>();
                    for (int i = 48; i <= 57; i++)
                    {
                        chars.Add(Char.ConvertFromUtf32(i));
                    }
                    for (int i = 32; i <= 47; i++)
                    {
                        chars.Add(Char.ConvertFromUtf32(i));
                    }
                    for (int i = 58; i <= 64; i++)
                    {
                        chars.Add(Char.ConvertFromUtf32(i));
                    }
                    for (int i = 91; i <= 96; i++)
                    {
                        chars.Add(Char.ConvertFromUtf32(i));
                    }
                    for (int i = 123; i <= 126; i++)
                    {
                        chars.Add(Char.ConvertFromUtf32(i));
                    }
                    return chars;
                }
                public List<String> SplitStringValue(String value, Char word='\0', Boolean allowCharAsWord = false)
                {
                    List<String> items = new List<String>();
                    if (String.IsNullOrEmpty(value))
                        return items;
                    else if (word == '\0')
                        items = value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                    else
                    {
                        Int32 count = CharOccurencies(value, word);
                        if (count <= 1)
                            items = value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                        else
                        {
                            Int32 startIndex= -1;
                            Int32 occurency = 0;
                            for (Int32 n = 0; n < value.Length; n++)
                            {
                                if (value[n] == word)
                                {
                                    occurency++;
                                    if (startIndex == -1)
                                    {
                                        items.AddRange(value.Substring(0, n - 1).Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList());
                                        startIndex = n;
                                    }
                                    else if (startIndex + 1 == n) {
                                        startIndex = n;
                                        if (allowCharAsWord)
                                            items.Add(word.ToString() + word.ToString());
                                    }
                                    else if (occurency % 2 == 0)
                                    {
                                        items.Add(value.Substring(startIndex + 1, n - startIndex - 1));
                                        startIndex = n;
                                    }
                                    else {
                                        items.AddRange(value.Substring(startIndex + ((allowCharAsWord) ? 0 : 1), n - startIndex - ((allowCharAsWord) ? 0 : 1)).Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList());
                                        startIndex = n;
                                    }
                                }
                            }
                            if (startIndex+1!=value.Length)
                                items.AddRange(value.Substring(startIndex + ((allowCharAsWord) ? 0 : 1), value.Length - startIndex - ((allowCharAsWord) ? 0 : 1)).Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList());
                        }
                    }
                    return items;
                }
                public Int32 CharOccurencies(String source,Char c)
                {
                    Int32 count = 0;
                    for (Int32 n = 0; n < source.Length; n++) if (source[n] == c) count++;

                    return count;
                }

                public List<Organization> GetAvailableOrganizations(Int32 idUser, SearchCommunityFor type)
                {
                    List<Organization> organizations = null;
                    try
                    {
                        Person user = Manager.GetPerson(idUser);
                        if (user != null) {
                            if (user.TypeID == (int)UserTypeStandard.SysAdmin || user.TypeID == (int)UserTypeStandard.Administrator)
                                organizations = (from o in Manager.GetIQ<Organization>() select o).ToList();
                            else
                            {
                                organizations = new List<Organization>();
                                List<int> idOrganizations = (from po in Manager.GetIQ<OrganizationProfiles>() where po.Profile == user select po.OrganizationID).ToList();
                                List<Organization> sOrganizations = (from o in Manager.GetIQ<Organization>() where idOrganizations.Contains(o.Id) select o).ToList();
                                List<Community> communities = (from c in Manager.GetIQ<Community>() where c.IdFather == 0 && idOrganizations.Contains(c.IdOrganization) select c).ToList();
                                List<int> idCommunities = communities.Select(c=>c.Id).ToList();

                                Boolean isAdministrative = (user.TypeID == (int)UserTypeStandard.Administrative);
                                
                                List<LazySubscription> subscriptions = (from s in Manager.GetIQ<LazySubscription>()
                                                                                where s.Accepted && s.Enabled && s.IdPerson == idUser && idCommunities.Contains(s.IdCommunity)
                                                                                select s).ToList();
                                switch (type){
                                    case SearchCommunityFor.CommunityManagement:
                                    case SearchCommunityFor.SystemManagement:
                                        Int32 idModule = Manager.GetModuleID(lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID);
                                        foreach(LazySubscription sub in subscriptions){
                                            lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement module = new lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement(Manager.GetModulePermission(idUser, sub.IdCommunity, idModule));
                                            if (module.Administration || module.Manage)
                                                organizations.Add(sOrganizations.Where(o => o.Id == communities.Where(c => c.Id == sub.IdCommunity).Select(c => c.IdOrganization).FirstOrDefault()).FirstOrDefault());
                                        }
                                        
                                        break;
                                    case SearchCommunityFor.Subscribe:
                                        organizations= sOrganizations.Where(o=> !communities.Where(c=> subscriptions.Select(s=>s.IdCommunity).ToList().Contains(c.Id)).Select(c=>c.IdOrganization).ToList().Contains(o.Id)).ToList();
                                        break;
                                    case SearchCommunityFor.Subscribed:
                                        if (subscriptions.Any())
                                            organizations= sOrganizations.Where(o=> communities.Where(c=> subscriptions.Select(s=>s.IdCommunity).ToList().Contains(c.Id)).Select(c=>c.IdOrganization).ToList().Contains(o.Id)).ToList();
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        organizations = new List<Organization>();
                    }
                    return organizations;
                }
                public List<Int32> GetAvailableProfileTypes(Int32 IdOrganization)
                {
                    List<Int32> result= null;
                    try{
                        result = (from p in Manager.GetIQ<OrganizationProfiles>() 
                                  where  IdOrganization <0 || p.OrganizationID==IdOrganization 
                                  select p.Profile.TypeID).Distinct().ToList();
                    }
                    catch (Exception ex){
                        result = new List<Int32>();
                    }
                    return result;
                }
                public List<StatusProfile> GetAvailableStatus(Int32 IdOrganization, Int32 IdProfileType)
                {
                    List<StatusProfile> list = new List<StatusProfile>();
                    if ((from p in Manager.GetIQ<OrganizationProfiles>() where (IdOrganization < 1 || p.OrganizationID == IdOrganization) && (IdProfileType == -1 || p.Profile.TypeID == IdProfileType) && !p.Profile.isDisabled select p.Id).Any())
                        list.Add( StatusProfile.Active);

                    if ((from p in Manager.GetIQ<OrganizationProfiles>() where (IdOrganization<1 || p.OrganizationID==IdOrganization) && (IdProfileType == -1 || p.Profile.TypeID== IdProfileType ) && p.Profile.isDisabled select p.Id).Any())
                        list.Add(StatusProfile.Disabled);

                    List<Int32> waitingList = (from p in Manager.GetIQ<WaitingActivationProfile>()
                                               where p.Person.isDisabled && (IdProfileType == -1 || p.Person.TypeID == IdProfileType)
                                               select p.Person.Id).ToList();

                    if (waitingList.Any())
                    {
                        if ((from p in Manager.GetIQ<OrganizationProfiles>() where (IdOrganization < 1 || p.OrganizationID == IdOrganization) && waitingList.Contains(p.Profile.Id) select p.Id).Any())
                            list.Add(StatusProfile.Waiting);
                    }
                    
                    if (list.Count > 1)
                        list.Insert(0, StatusProfile.AllStatus); 

                    return list;
                }
                    public Dictionary<long,String> GetAvailableAgencies(Int32 idOrganization)
                    {
                        Dictionary<long, String> list = new Dictionary<long, String>();
                        try
                        {
                            List<Agency> agencies = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None && (a.AlwaysAvailable || (a.OrganizationAffiliations.Where(o => o.IdOrganization == idOrganization).Any())) select a).ToList();
                            List<long> idUserAgencies = (from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None select a.Agency.Id ).Distinct().ToList();
                            list = agencies.Where(a => idUserAgencies.Contains(a.Id)).OrderBy(a=>a.Name).ToDictionary(a => a.Id, a => a.Name);
                        }
                        catch (Exception ex) { 
                        
                        }
                        return list;
                    }
                #endregion
                public Boolean MustEditPassword(Int32 IdUser, String oldPassword, String newPassword)
                {
                    return EditPassword(Manager.GetPerson(IdUser), oldPassword, newPassword, true);
                }
                public Boolean EditPassword(Int32 IdUser, String oldPassword, String newPassword)
                {
                    return EditPassword(Manager.GetPerson(IdUser), oldPassword, newPassword);
                }
                public Boolean EditPassword(Person user, String oldPassword, String newPassword)
                {
                    return EditPassword(user,oldPassword,newPassword, false );
                }
                public Boolean EditPassword(Person user, String oldPassword, String newPassword, Boolean mustEdit)
                {
                    Boolean edited = false;
                    try
                    {
                        edited = InternalService.EditPassword(user, oldPassword, newPassword, mustEdit);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return edited;
                }
                public InternalLoginInfo SetPassword(Person user, String newPassword, Boolean isOneTimePassword)
                {
                    InternalLoginInfo loginInfo = null;
                    try
                    {
                        loginInfo = InternalService.SetPassword(user, newPassword, isOneTimePassword);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return loginInfo;
                }
                public Boolean SetDefaultProvider(long idProvider, Int32 IdProfile)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        Person person = Manager.GetPerson(IdProfile);
                        if (person != null)
                        {
                            person.IdDefaultProvider = idProvider;
                            Manager.SaveOrUpdate(person);
                        }
                        Manager.Commit();
                        result = (person!=null);
                    }
                    catch (Exception ex) { 
                    
                    }
                    return result;
                }
                public Boolean ModifyAuthenticationActivation(long idLoginInfo, Boolean enable)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        BaseLoginInfo loginInfo = Manager.Get<BaseLoginInfo>(idLoginInfo);
                        if (loginInfo != null)
                        {
                            loginInfo.UpdateMetaInfo(Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                            loginInfo.isEnabled = enable;
                            Manager.SaveOrUpdate(loginInfo);
                        }
                        Manager.Commit();
                        result = (loginInfo != null);
                    }
                    catch (Exception ex)
                    {

                    }
                    return result;
                }
                public List<Person> GetUserByTaxCode(String taxCode){
                    List<Person> items = null;
                    try
                    {
                        items = (from p in Manager.GetIQ<Person>() where p.TaxCode == taxCode select p).ToList();
                    }
                    catch (Exception ex) {

                    }
                    return items;
                }

               
            #endregion

            #region "Import"
                public void AnalyzeItems(lm.Comol.Core.DomainModel.Helpers.ProfileExternalResource resource, dtoImportSettings settings)
                {
                    List<lm.Comol.Core.DomainModel.Helpers.ProfileAttributeColumn> cols = resource.VerifyDuplicationValues();
                    foreach (lm.Comol.Core.DomainModel.Helpers.ProfileAttributeColumn col in cols) {
                        List<lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell> cells = (from c in resource.GetColumnCells(col.Attribute) where c.isValid && c.Row.AllowImport && !c.Row.HasDBDuplicatedValues select c).ToList();
                        switch (col.Attribute) { 
                            case Authentication.ProfileAttributeType.mail:
                                AnalyzeCellsForMailDuplication(cells);
                                break;
                            case Authentication.ProfileAttributeType.login:
                                AnalyzeCellsForLoginDuplication(cells);
                                break;
                            case Authentication.ProfileAttributeType.taxCode:
                                AnalyzeCellsForTaxCodeDuplication(cells);
                                break;
                            case Authentication.ProfileAttributeType.externalId:
                                AnalyzeCellsForExternalIdDuplication(cells, settings.IdProvider);
                                break;
                        }
                    }
                    if (settings.AutoGenerateLogin) {
                        DomainModel.Helpers.ProfileAttributeColumn column = new DomainModel.Helpers.ProfileAttributeColumn() { Attribute = ProfileAttributeType.autoGeneratedLogin, Number = -1 };
                        foreach (DomainModel.Helpers.ProfileAttributesRow row in resource.Rows)
                        {
                            DomainModel.Helpers.ProfileAttributeCell cell = new DomainModel.Helpers.ProfileAttributeCell();
                            cell.Column= column;
                            cell.Row=row;
                            try
                            {
                                cell.Value = InternalService.GenerateInternalLogin(row.Cells.Where(c => c.Column.Attribute == ProfileAttributeType.name).Select(c => c.Value).FirstOrDefault(), row.Cells.Where(c => c.Column.Attribute == ProfileAttributeType.surname).Select(c => c.Value).FirstOrDefault());
                            }
                            catch (Exception ex) {

                            }
                            
                            row.Cells.Add(cell);
                        }

                        resource.ColumHeader.Add(column);
                    }
                    if (settings.IdProfileType == (int)UserTypeStandard.Employee) {
                        AnalyzeCellsForAgency(resource, settings);
                    }
                }
                public void AnalyzeCellsForMailDuplication(List<lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell> cells)
                {
                    foreach (lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell c in cells){
                        try{
                            c.isDBduplicate= (from p in Manager.GetIQ<Person>() where p.Mail == c.Value select p.Id).Any();
                        }
                        catch(Exception ex){
                    
                        }   
                    }
                }
                public void AnalyzeCellsForLoginDuplication(List<lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell> cells)
                {
                    foreach (lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell c in cells.Where(ci => !string.IsNullOrEmpty(ci.Value)).ToList())
                    {
                        try
                        {
                            c.isDBduplicate = (from s in Manager.GetIQ<InternalLoginInfo>()
                                               where s.Login== c.Value && s.Deleted== BaseStatusDeleted.None
                                               select s.Id).Any();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                public void AnalyzeCellsForTaxCodeDuplication(List<lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell> cells)
                {
                    foreach (lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell c in cells.Where(ci => !string.IsNullOrEmpty(ci.Value)).ToList())
                    {
                        try
                        {
                            c.isDBduplicate = (from s in Manager.GetIQ<Person>()
                                               where s.TaxCode == c.Value 
                                               select s.Id).Any();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                public void AnalyzeCellsForExternalIdDuplication(List<lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell> cells, long idProvider)
                {
                    AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                    if (provider != null){
                        foreach (lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell c in cells.Where(ci => !string.IsNullOrEmpty(ci.Value)).ToList())
                        {
                            var query = (from s in Manager.GetIQ<ExternalLoginInfo>()
                                                   where s.Deleted == BaseStatusDeleted.None && s.Provider == provider
                                                   select s);
                            try
                            {
                                if (provider.IdentifierFields== IdentifierField.longField){
                                    long id = 0;
                                    long.TryParse(c.Value,out id);
                                    c.isDBduplicate = query.Where(s=> s.IdExternalLong==id).Select(s=>s.Id).Any();
                                }
                                else{
                                    c.isDBduplicate = query.Where(s => s.IdExternalString == c.Value).Select(s => s.Id).Any();
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                public void AnalyzeCellsForAgency(ProfileExternalResource resource, dtoImportSettings settings)
                {
                    List<ProfileAttributeType> attributes = new List<ProfileAttributeType>() { ProfileAttributeType.agencyInternalCode, ProfileAttributeType.agencyExternalCode, ProfileAttributeType.agencyName, ProfileAttributeType.agencyNationalCode, ProfileAttributeType.agencyTaxCode };
                    List<ProfileAttributeCell> cells = resource.GetColumnCells(attributes);
                    List<int> rows = cells.Select(c => c.Row.Number).Distinct().ToList();

                    ProfileComputedColumn column = new ProfileComputedColumn(attributes) { Attribute = ProfileAttributeType.agencyInternalCode, Number = resource.ColumHeader.Last().Number + 1 };
                    resource.ColumHeader.Add(column);

                    Agency emptyAgency = Manager.Get<Agency>(settings.DefaultAgency.Key);
                    if (emptyAgency==null)
                        emptyAgency = GetEmptyAgency();
                    foreach (int row in rows) {
                        ProfileAttributeComputedCell calcCell = new ProfileAttributeComputedCell(column) {  Row = resource.Rows[row], Value = "", InternallLongValue = 0, isDBduplicate = false, DisplayValue = "0" };
                        try
                        {
   
                            long idAgency = 0;
                            String name = "";
                            var query = (from ag in Manager.GetIQ<Agency>() where ag.Deleted == BaseStatusDeleted.None select ag);

                            ProfileAttributeCell cInternal = GetCell(cells, row, ProfileAttributeType.agencyInternalCode);
                            if (cInternal != null && cInternal.isValid)
                            {
                                long.TryParse(cInternal.Value, out idAgency);
                                query = query.Where(a => a.Id == idAgency);
                            }
                            else {
                                ProfileAttributeCell cExternal = GetCell(cells, row, ProfileAttributeType.agencyExternalCode);
                                if (cExternal != null && !string.IsNullOrEmpty(cExternal.Value))
                                {
                                    query = query.Where(a => a.ExternalCode == cExternal.Value);
                                }
                                ProfileAttributeCell cNational = GetCell(cells, row, ProfileAttributeType.agencyNationalCode);
                                if (cNational != null && !string.IsNullOrEmpty(cNational.Value))
                                {
                                    query = query.Where(a => a.NationalCode == cNational.Value);
                                }
                                ProfileAttributeCell cTaxCode = GetCell(cells, row, ProfileAttributeType.agencyTaxCode);
                                if (cTaxCode != null && !string.IsNullOrEmpty(cTaxCode.Value))
                                {
                                    query = query.Where(a => a.TaxCode == cTaxCode.Value);
                                }
                            }
                            List<Agency> items = query.ToList();
                            if (items != null && items.Count>0)
                            {
                                Agency found = items.FirstOrDefault();
                                idAgency = found.Id;
                                name = found.Name;
                            }
                            else if (emptyAgency != null)
                            {
                                idAgency = emptyAgency.Id;
                                name = emptyAgency.Name;
                            }
                            else
                                idAgency = 0;
                            calcCell.Value = idAgency.ToString();
                            calcCell.DisplayValue = name;
                            calcCell.InternallLongValue = idAgency;
                        }
                        catch (Exception ex) {

                        }
                        resource.Rows[row].Cells.Add(calcCell);
                    }
                }
                private ProfileAttributeCell GetCell(List<lm.Comol.Core.DomainModel.Helpers.ProfileAttributeCell> cells, int number,ProfileAttributeType attribute)
                {
                    return cells.Where(c => c.Row.Number == number && c.Column.Attribute == attribute).FirstOrDefault();
                }

        /// <summary>
        /// Recupera gli attributi in base al tipo
        /// </summary>
        /// <param name="idType"></param>
        /// <param name="idProvider"></param>
        /// <param name="alsoTaxCode"></param>
        /// <param name="alsoPassword"></param>
        /// <param name="autoGenerateLogin"></param>
        /// <returns></returns>
                public List<dtoProfileAttributeType> GetAvailableAttributeTypes(Int32 idType, long idProvider, Boolean alsoTaxCode, Boolean alsoPassword, Boolean autoGenerateLogin)
                {
                    List<dtoProfileAttributeType> items = GetProfileTypeBaseAttributes(idType, idProvider, alsoTaxCode, alsoPassword, autoGenerateLogin);
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.skip,false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.telephoneNumber,false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.fax,false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.mobile,false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.city,false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.address,false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.zipCode, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.language, false));

                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.sector, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.job, false));

                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.birthDate, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.birthPlace, false));
                    
                    switch (idType)
                    {
                        case (Int32)UserTypeStandard.Company:
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyRegion, false, UserTypeStandard.Company ));
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyTaxCode, false, UserTypeStandard.Company ));
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyAddress, false, UserTypeStandard.Company ));
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyCity, false, UserTypeStandard.Company));
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyReaNumber, false, UserTypeStandard.Company));
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyAssociations, false, UserTypeStandard.Company));
                            break;
                        case (Int32)UserTypeStandard.Employee:
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyName, false, UserTypeStandard.Employee));
                            break;
                    }

                    return items;
                }
                public List<dtoProfileAttributeType> GetProfileTypeBaseAttributes(Int32 idType, long idProvider, Boolean alsoTaxCode, Boolean alsoPassword, Boolean autoGenerateLogin)
                {
                    List<dtoProfileAttributeType> items = new List<dtoProfileAttributeType>();

                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.name));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.surname));
                    if (alsoTaxCode)
                        items.Add(new dtoProfileAttributeType(ProfileAttributeType.taxCode));
                    else
                        items.Add(new dtoProfileAttributeType(ProfileAttributeType.taxCode,false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.mail));
                    try{
                        Manager.BeginTransaction();
                        AuthenticationProviderType providerType = (from ap in Manager.GetIQ<AuthenticationProvider>()
                                                                   where ap.Id == idProvider
                                                                   select ap.ProviderType).Skip(0).Take(1).ToList().FirstOrDefault();
                        Manager.Commit();

                        switch (providerType) { 
                            case AuthenticationProviderType.Internal:
                                if (!autoGenerateLogin)
                                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.login));
                                if (alsoPassword)
                                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.password));
                                break;
                            default:
                                items.Add(new dtoProfileAttributeType(ProfileAttributeType.externalId));
                                break;
                        }
                    }

                    catch(Exception ex){
                        Manager.RollBack();
                        items.Add(new dtoProfileAttributeType(ProfileAttributeType.login));
                        if (alsoPassword)
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.password));
                    }
                    
                    switch (idType)
                    {
                        case (Int32)UserTypeStandard.Employee:
                            List<ProfileAttributeType> alternatives = new List<ProfileAttributeType>();
                            alternatives.Add(ProfileAttributeType.agencyNationalCode);
                            alternatives.Add(ProfileAttributeType.agencyExternalCode);
                            alternatives.Add(ProfileAttributeType.agencyTaxCode);
                            alternatives.Add(ProfileAttributeType.agencyInternalCode);

                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyNationalCode, true, UserTypeStandard.Employee, alternatives.Where(a=>a !=ProfileAttributeType.agencyNationalCode).ToList()));
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyExternalCode, true, UserTypeStandard.Employee, alternatives.Where(a => a != ProfileAttributeType.agencyExternalCode).ToList()));
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyTaxCode, true, UserTypeStandard.Employee, alternatives.Where(a => a != ProfileAttributeType.agencyTaxCode).ToList()));
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyInternalCode, true, UserTypeStandard.Employee, alternatives.Where(a => a != ProfileAttributeType.agencyInternalCode).ToList()));
                            break;
                        case (Int32)UserTypeStandard.Company:
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyName, UserTypeStandard.Company));
                            break;
                        case (Int32)UserTypeStandard.ExternalUser:
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.externalUserInfo,UserTypeStandard.ExternalUser));                            
                            break;
                    }

                    return items;
                }
                public List<ProfileAttributeType> GetProfileMandatoryAttributes(Int32 idType, long idProvider, Boolean alsoTaxCode, Boolean alsoPassword, Boolean autoGenerateLogin)
                {
                    return GetProfileTypeBaseAttributes(idType, idProvider, alsoTaxCode, alsoPassword, autoGenerateLogin).Select(a=>a.Attribute).ToList();
                }
                public List<ProfileAttributeType> GetProfileTypeMailTemplateAttributes(dtoImportSettings settings)
                {
                    List<ProfileAttributeType> items = new List<ProfileAttributeType>();

                    items.Add(ProfileAttributeType.name);
                    items.Add(ProfileAttributeType.surname);
                    if (settings.AddTaxCode)
                        items.Add(ProfileAttributeType.taxCode);
                    items.Add(ProfileAttributeType.mail);
                    try
                    {
                        Manager.BeginTransaction();
                        AuthenticationProviderType providerType = (from ap in Manager.GetIQ<AuthenticationProvider>()
                                                                   where ap.Id == settings.IdProvider
                                                                   select ap.ProviderType).Skip(0).Take(1).ToList().FirstOrDefault();
                        Manager.Commit();

                        switch (providerType)
                        {
                            case AuthenticationProviderType.Internal:
                                if (settings.AutoGenerateLogin)
                                    items.Add(ProfileAttributeType.autoGeneratedLogin);
                                else
                                    items.Add(ProfileAttributeType.login);
                                items.Add(ProfileAttributeType.password);
                                break;
                            default:
                                items.Add(ProfileAttributeType.externalId);
                                break;
                        }
                    }

                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        items.Add(ProfileAttributeType.login);
                        if (settings.AddPassword)
                            items.Add(ProfileAttributeType.password);
                    }

                    switch (settings.IdProfileType)
                    {
                        case (Int32)UserTypeStandard.Employee:
                             items.Add(ProfileAttributeType.agencyName);
                            break;
                        case (Int32)UserTypeStandard.Company:
                            items.Add(ProfileAttributeType.companyName);
                            items.Add(ProfileAttributeType.companyAddress);
                            items.Add(ProfileAttributeType.companyCity);
                            break;
                        case (Int32)UserTypeStandard.ExternalUser:
                            items.Add(ProfileAttributeType.externalUserInfo);
                            break;
                    }

                    return items;
                }

                public List<dtoProfileAttributeType> GetAllProfileAttributes(Boolean alsoTaxCode)
                {
                    List<dtoProfileAttributeType> items = new List<dtoProfileAttributeType>();

                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.name));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.surname));
                    if (alsoTaxCode)
                        items.Add(new dtoProfileAttributeType(ProfileAttributeType.taxCode));
                    else
                        items.Add(new dtoProfileAttributeType(ProfileAttributeType.taxCode, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.mail));
                  
                    try
                    {
                        Manager.BeginTransaction();
                        List<AuthenticationProvider> providers = (from ap in Manager.GetIQ<AuthenticationProvider>()
                                                                         where ap.Deleted== BaseStatusDeleted.None 
                                                                         select ap).ToList();
                        Manager.Commit();

                        if (providers.Where(p=>p.ProviderType== AuthenticationProviderType.Internal).Any())
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.login, !providers.Where(p => p.ProviderType != AuthenticationProviderType.Internal).Any()));
                        if (providers.Where(p => p.ProviderType != AuthenticationProviderType.Internal).Any())
                            items.Add(new dtoProfileAttributeType(ProfileAttributeType.externalId));
                    }

                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }

                    //Employee
                    List<ProfileAttributeType> alternatives = new List<ProfileAttributeType>();
                    alternatives.Add(ProfileAttributeType.agencyNationalCode);
                    alternatives.Add(ProfileAttributeType.agencyExternalCode);
                    alternatives.Add(ProfileAttributeType.agencyTaxCode);
                    alternatives.Add(ProfileAttributeType.agencyInternalCode);

                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyNationalCode, true, UserTypeStandard.Employee, alternatives.Where(a => a != ProfileAttributeType.agencyNationalCode).ToList()));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyExternalCode, true, UserTypeStandard.Employee, alternatives.Where(a => a != ProfileAttributeType.agencyExternalCode).ToList()));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyTaxCode, true, UserTypeStandard.Employee, alternatives.Where(a => a != ProfileAttributeType.agencyTaxCode).ToList()));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyInternalCode, true, UserTypeStandard.Employee, alternatives.Where(a => a != ProfileAttributeType.agencyInternalCode).ToList()));

                    //Company
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyName, UserTypeStandard.Company));
                 
                    //ExternalUser
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.externalUserInfo, UserTypeStandard.ExternalUser));

                    // NOT mandatory

                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.telephoneNumber, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.fax, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.mobile, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.city, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.address, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.zipCode, false));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.language, false));

                    //Company:
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyRegion, false, UserTypeStandard.Company ));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyTaxCode, false, UserTypeStandard.Company ));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyAddress, false, UserTypeStandard.Company ));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyCity, false, UserTypeStandard.Company));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyReaNumber, false, UserTypeStandard.Company));
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.companyAssociations, false, UserTypeStandard.Company));
                    //UserTypeStandard.Employee:
                    items.Add(new dtoProfileAttributeType(ProfileAttributeType.agencyName, false, UserTypeStandard.Employee));

                    return items;
                }

            #endregion
                #region Community List"



                //public Boolean ProfileHasCommunityToUnsubscribe(Int32 idProfile, List<Int32> subscribedCommunitiesId)
                //{
                //    Person person = Manager.GetPerson(idProfile);
                //    Int32 IdDefaultOrganization = (from so in Manager.GetIQ<OrganizationProfiles>() where so.Profile == person && so.isDefault == true select so.OrganizationID).Skip(0).Take(1).ToList().FirstOrDefault();
                //    Int32 IdCommunity = (from c in Manager.GetIQ<Community>() where c.IdFather == 0 && c.IdOrganization == IdDefaultOrganization && c.IdTypeOfCommunity == 0 select c.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                //    subscribedCommunitiesId.Add(IdCommunity);
                //    return (from s in Manager.GetIQ<LazySubscription>()
                //            where s.IdRole > 0 && s.IdPerson == idProfile && !subscribedCommunitiesId.Contains(s.IdCommunity)
                //            select s.Id).Any();

                //}
        #endregion

                #region "Utilities"
                public String GetUserMail(Int32 idPerson)
                {
                    String mail = "";
                    try
                    {
                        mail = (from p in Manager.GetIQ<Person>() where p.Id == idPerson select p.Mail).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    return mail;
                }
                public List<AuthenticationProvider> GetUserAuthenticationProviders(Person person)
                {
                    List<AuthenticationProvider> providers = null;
                    try
                    {
                        providers = (from p in Manager.GetAll<BaseLoginInfo>(p => p.Person == person && p.isEnabled && p.Deleted == BaseStatusDeleted.None) 
                                     select p.Provider).ToList().Where(p=>p.IsEnabled).ToList();
                    }
                    catch (Exception ex)
                    {
                        providers = new List<AuthenticationProvider>();
                    }
                    return providers;
                }
                public List<dtoBaseProvider> GetAvailableAuthenticationProviders(Int32 IdLanguage, Int32 IdProfile)
                {
                    List<dtoBaseProvider> providers = new List<dtoBaseProvider>();
                    Language language = Manager.GetLanguage(UC.Language.Id);
                    try
                    {
                        Language dLanguage = Manager.GetDefaultLanguage();
                        Person person = Manager.GetPerson(IdProfile);

                        List<long> exceptIdProviders = (from p in Manager.GetAll<ExternalLoginInfo>(p => p.Person == person && p.Deleted == BaseStatusDeleted.None && p.Provider.AllowMultipleInsert==false ) select p.Provider.Id).ToList();
                        exceptIdProviders.AddRange((from p in Manager.GetAll<InternalLoginInfo>(p => p.Person == person && p.Deleted == BaseStatusDeleted.None) select p.Provider.Id).ToList());
                        providers = (from p in Manager.GetAll<AuthenticationProvider>(p => p.Deleted == BaseStatusDeleted.None && ! exceptIdProviders.Contains(p.Id))
                                     select new dtoBaseProvider()
                                     {
                                         IdProvider = p.Id,
                                         DisplayToUser = p.DisplayToUser,
                                         AllowAdminProfileInsert = p.AllowAdminProfileInsert,
                                         Type = p.ProviderType,
                                         IdentifierFields = p.IdentifierFields,
                                         Translation = TranslateProviderInfo(language, dLanguage, p),
                                     }).ToList();
                    }
                    catch (Exception ex)
                    {
                        providers = new List<dtoBaseProvider>();
                    }
                    return providers.OrderBy(p => p.Translation.Name).ToList();
                }
                public List<dtoBaseProvider> GetAuthenticationProviders(Int32 IdLanguage, Boolean forAdmin)
                {
                    Language language = Manager.GetLanguage(UC.Language.Id);
                    return GetAuthenticationProviders(language, forAdmin);
                }
                public List<dtoBaseProvider> GetAuthenticationProviders(Int32 IdOrganization, Int32 IdProfileType, Int32 IdLanguage, Boolean forAdmin)
                {
                    Language language = Manager.GetLanguage(UC.Language.Id);
                    List<dtoBaseProvider> providers = GetAuthenticationProviders(language, forAdmin);

                    List<long> IdDefaultProviders = (from p in Manager.GetIQ<OrganizationProfiles>()
                                              where (IdOrganization < 1 || p.OrganizationID == IdOrganization) && (IdProfileType == -1 || p.Profile.TypeID == IdProfileType)
                                              select p.Profile.IdDefaultProvider).Distinct().ToList();
                    return (from p in providers where IdDefaultProviders.Contains(p.IdProvider) orderby p.Translation.Name select p).ToList();
                }
                private List<dtoBaseProvider> GetAuthenticationProviders(Language language,Boolean forAdmin)
                {
                    List<dtoBaseProvider> providers = new List<dtoBaseProvider>();
                    try
                    {
                        Language dLanguage = Manager.GetDefaultLanguage();
                        providers = (from p in Manager.GetAll<AuthenticationProvider>(p=> p.Deleted == BaseStatusDeleted.None && (forAdmin || p.DisplayToUser == true))
                                     select new dtoBaseProvider()
                                     {
                                         IdProvider = p.Id,
                                         DisplayToUser = p.DisplayToUser,
                                         AllowAdminProfileInsert = p.AllowAdminProfileInsert,
                                         Type = p.ProviderType,
                                         LogoutMode= p.LogoutMode,
                                         IdentifierFields= p.IdentifierFields,
                                         Translation = TranslateProviderInfo(language, dLanguage, p),
                                         isEnabled = p.IsEnabled 
                                     }).ToList();
                    }
                    catch (Exception ex)
                    {
                        providers = new List<dtoBaseProvider>();
                    }
                    return providers.OrderBy(p=> p.Translation.Name).ToList();
                }
                public AuthenticationProvider GetAuthenticationProvider(long idProvider)
                {
                    AuthenticationProvider provider = null;
                    try
                    {
                        provider =  Manager.Get<AuthenticationProvider>(idProvider);
                    }
                    catch (Exception ex)
                    {

                    }
                    return provider;
                }
                public dtoBaseProvider GetAuthenticationProvider(Int32 IdLanguage, long idProvider){
                    Language language = Manager.GetLanguage(UC.Language.Id);
                    return GetAuthenticationProvider(language, idProvider);
                }
                public dtoBaseProvider GetAuthenticationProvider(Language language, long idProvider)
                {
                    dtoBaseProvider provider = null;
                    try
                    {
                        Language dLanguage = Manager.GetDefaultLanguage();
                        provider = (from p in Manager.GetAll<AuthenticationProvider>(p => p.Deleted == BaseStatusDeleted.None && p.Id == idProvider)
                                     select new dtoBaseProvider()
                                     {
                                         IdProvider = p.Id,
                                         DisplayToUser = p.DisplayToUser,
                                         AllowAdminProfileInsert = p.AllowAdminProfileInsert,
                                         Type = p.ProviderType,
                                         IdentifierFields = p.IdentifierFields,
                                         Translation = TranslateProviderInfo(language, dLanguage, p),
                                     }).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    return provider;
                }

                public List<dtoUserProvider> GetProfileProviders(Int32 IdPerson, Boolean forAdmin)
                {
                    Person person = Manager.GetPerson(IdPerson);
                    Language language = Manager.GetLanguage(UC.Language.Id);
                    return GetProfileProviders(person, forAdmin, language);
                }
                public List<dtoUserProvider> GetProfileProviders(Person person, Boolean forAdmin, Language language)
                {
                    List<dtoUserProvider> providers = new List<dtoUserProvider>();
                    try
                    {
                        Language dLanguage = Manager.GetDefaultLanguage();
                        providers = (from p in
                                         (from li in Manager.GetAll<InternalLoginInfo>(li => li.Deleted == BaseStatusDeleted.None && li.Person != null && li.Person == person)
                                          select new dtoInternalUserProvider()
                                          {
                                              Id = li.Id,
                                              isEnabled = li.isEnabled,
                                              IdProvider = li.Provider.Id,
                                              DisplayToUser = li.Provider.DisplayToUser,
                                              Type = li.Provider.ProviderType,
                                              ModifiedOn = li.ModifiedOn,
                                              ModifiedBy = li.ModifiedBy,
                                              PasswordExpiresOn = li.PasswordExpiresOn,
                                              Translation = TranslateProviderInfo(language, dLanguage, li.Provider),
                                              ResetType = li.ResetType,
                                              Login = li.Login 
                                          }).ToList()
                                     select (dtoUserProvider)p).ToList();

                        providers.AddRange((from li in Manager.GetAll<ExternalLoginInfo>(li => li.Deleted == BaseStatusDeleted.None && li.Person != null && li.Person == person)
                                            select (dtoUserProvider)new dtoExternalUserProvider()
                                            {
                                                Id = li.Id,
                                                isEnabled = li.isEnabled,
                                                IdProvider = li.Provider.Id,
                                                DisplayToUser = li.Provider.DisplayToUser,
                                                Type = li.Provider.ProviderType,
                                                ModifiedOn = li.ModifiedOn,
                                                ModifiedBy = li.ModifiedBy,
                                                Translation = TranslateProviderInfo(language, dLanguage, li.Provider),
                                                RemoteUrl = (li.Provider.ProviderType == AuthenticationProviderType.Url) ? ((UrlAuthenticationProvider)li.Provider).RemoteLoginUrl : "",
                                                IdExternalLong= li.IdExternalLong, 
                                                IdExternalString= li.IdExternalString,
                                                IdentifierFields= li.Provider.IdentifierFields 
                                            }).ToList());

                       // providers = providers.Where(p => p.DisplayToUser == true || forAdmin).ToList();

                    }
                    catch (Exception ex)
                    {
                        providers = new List<dtoUserProvider>();
                    }
                    return providers.Where(p => p.DisplayToUser == true || forAdmin).ToList();
                }
                public dtoUserProvider GetProfileProvider(Int32 IdPerson, long idUserProvider, Int32 IdLanguage) {
                    Language language = Manager.GetLanguage(UC.Language.Id);
                    return GetProfileProvider(Manager.GetPerson(IdPerson),idUserProvider,language);
                }
                public dtoUserProvider GetProfileProvider(Person person, long idUserProvider, Language language)
                {
                    dtoUserProvider provider = null;
                    try
                    {
                        Language dLanguage = Manager.GetDefaultLanguage();
                        BaseLoginInfo loginInfo=  (from li in Manager.GetAll<BaseLoginInfo>(li => li.Deleted == BaseStatusDeleted.None && li.Person != null && li.Person == person && li.Provider.Id== idUserProvider) select li).FirstOrDefault();
                        if (typeof(InternalLoginInfo) == loginInfo.GetType())
                            provider = new dtoInternalUserProvider()
                                          {
                                              Id = loginInfo.Id,
                                              isEnabled = loginInfo.isEnabled,
                                              IdProvider = loginInfo.Provider.Id,
                                              DisplayToUser = loginInfo.Provider.DisplayToUser,
                                              Type = loginInfo.Provider.ProviderType,
                                              ModifiedOn = loginInfo.ModifiedOn,
                                              ModifiedBy = loginInfo.ModifiedBy,
                                              PasswordExpiresOn = ((InternalLoginInfo)loginInfo).PasswordExpiresOn,
                                              Translation = TranslateProviderInfo(language, dLanguage, loginInfo.Provider),
                                              ResetType = ((InternalLoginInfo)loginInfo).ResetType
                                          };

                        else
                            provider = (dtoUserProvider)new dtoExternalUserProvider()
                                            {
                                                Id = loginInfo.Id,
                                                isEnabled = loginInfo.isEnabled,
                                                IdProvider = ((ExternalLoginInfo)loginInfo).Provider.Id,
                                                DisplayToUser = ((ExternalLoginInfo)loginInfo).Provider.DisplayToUser,
                                                Type = ((ExternalLoginInfo)loginInfo).Provider.ProviderType,
                                                ModifiedOn = loginInfo.ModifiedOn,
                                                ModifiedBy = loginInfo.ModifiedBy,
                                                Translation = TranslateProviderInfo(language, dLanguage, ((ExternalLoginInfo)loginInfo).Provider),
                                                IdentifierFields = ((ExternalLoginInfo)loginInfo).Provider.IdentifierFields,
                                                RemoteUrl = (((ExternalLoginInfo)loginInfo).Provider.ProviderType == AuthenticationProviderType.Url) ? ((UrlAuthenticationProvider)((ExternalLoginInfo)loginInfo).Provider).RemoteLoginUrl : ""
                                            };

                    }
                    catch (Exception ex)
                    {
                    }
                    return provider;
                }
                private dtoProviderTranslation TranslateProviderInfo(Language ulanguage,Language planguage, AuthenticationProvider provider)
                {
                    dtoProviderTranslation translation = null;
                    try
                    {
                        List <dtoProviderTranslation> list = (from li in Manager.GetIQ<AuthenticationProviderTranslation>()
                                                              where li.Deleted == BaseStatusDeleted.None && (li.Language == ulanguage || li.Language == planguage) && li.Provider == provider
                                                              select new dtoProviderTranslation()
                                                              {
                                                                  Id = li.Id,
                                                                  Description = li.Description ,
                                                                   Name= li.Name,
                                                                     ForSubscribeDescription= li.ForSubscribeDescription,
                                                                     ForSubscribeName= li.ForSubscribeName ,
                                                                    idLanguage = li.Language.Id,
                                                                    FieldLong= li.FieldLong,
                                                                    FieldString = li.FieldString
                                                              }
                                                ).ToList();

                        if (list.Where(l => l.idLanguage == ulanguage.Id).Any())
                            translation = list.Where(l => l.idLanguage == ulanguage.Id).FirstOrDefault();
                        else
                            translation = list.Where(l => l.idLanguage == planguage.Id).FirstOrDefault();
                        if (translation == null)
                            translation = new dtoProviderTranslation() { Name = provider.Name };
                    }
                    catch (Exception ex)
                    {
                        translation = new dtoProviderTranslation();
                    }
                    return translation;
                }

                public Person GetAnonymousUser()
                {
                    return (from p in Manager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                }
        #endregion

            private static List<AlphabetItem> GetOtherAlphabetItems()
            {
                return (from n in Enumerable.Range(222, 34) select new AlphabetItem() { isEnabled = true, Value = Char.ConvertFromUtf32(n), DisplayName = Char.ConvertFromUtf32(n).ToString().ToUpper(), isSelected = false }).ToList();
            }
    }
}