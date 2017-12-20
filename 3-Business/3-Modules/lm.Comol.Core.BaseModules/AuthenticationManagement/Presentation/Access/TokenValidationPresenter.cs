using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public class TokenValidationPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private InternalAuthenticationService _InternalService;
            private UrlMacAuthenticationService _UrlService;
            private lm.Comol.Core.BaseModules.AuthenticationManagement.Business.MacProviderHelper _Helper;
            private ProfileManagement.Business.ProfileManagementService _ProfileService;
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement _ServiceCommunity;
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService _PolicyService;

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
            protected virtual IViewTokenValidation View
            {
                get { return (IViewTokenValidation)base.View; }
            }
            private lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService PolicyService
            {
                get
                {
                    if (_PolicyService == null)
                        _PolicyService = new lm.Comol.Core.BaseModules.PolicyManagement.Business.PolicyService(AppContext);
                    return _PolicyService;
                }
            }
            private ProfileManagement.Business.ProfileManagementService ProfileService
            {
                get
                {
                    if (_ProfileService == null)
                        _ProfileService = new ProfileManagement.Business.ProfileManagementService(AppContext);
                    return _ProfileService;
                }
            }
            private lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement ServiceCommunity
            {
                get
                {
                    if (_ServiceCommunity == null)
                        _ServiceCommunity = new lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(AppContext);
                    return _ServiceCommunity;
                }
            }
            private UrlMacAuthenticationService UrlService
            {
                get
                {
                    if (_UrlService == null)
                        _UrlService = new UrlMacAuthenticationService(AppContext);
                    return _UrlService;
                }
            }
            private InternalAuthenticationService InternalService
            {
                get
                {
                    if (_InternalService == null)
                        _InternalService = new InternalAuthenticationService(AppContext);
                    return _InternalService;
                }
            }
            private lm.Comol.Core.BaseModules.AuthenticationManagement.Business.MacProviderHelper Helper
            {
                get
                {
                    if (_Helper == null)
                        _Helper = new lm.Comol.Core.BaseModules.AuthenticationManagement.Business.MacProviderHelper(CurrentManager,UrlService,ProfileService);
                    return _Helper;
                }
            }
        
            public TokenValidationPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TokenValidationPresenter(iApplicationContext oContext, IViewTokenValidation view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean accessAvailable = !(View.isSystemOutOfOrder && !View.AllowAdminAccess);
            View.AllowSubscription = !View.isSystemOutOfOrder && View.SubscriptionActive;
            if (!accessAvailable)
                View.DisplaySystemOutOfOrder();
            else
            {
                if (View.HasUrlValues)
                {
                    List<dtoMacUrlProviderIdentifier> items = UrlService.GetActiveApplicationIdentifiers();
                    if (items == null || items.Count == 0)
                        View.DisplayProviderNotFound();
                    else
                    {
                        dtoMacUrlProviderIdentifier identifier = View.GetUrlIdentifier(items);
                        if (identifier == null)
                            View.DisplayProviderNotFound();//DisplayInvalidMessage(UrlProviderResult.UnknowToken);
                        else if (!identifier.isEnabled)
                            View.DisplayUrlAuthenticationUnavailable();
                        else
                        {
                            View.FromLogonUrl = View.PreloadFromUrl;
                            UrlProviderLogon(identifier,  View.PreloadFromUrl);
                        }
                    }
                }
                else
                    View.DisplayNoToken();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiCode">user language code from browser</param>
        public void InitializeLanguageView(String uiCode) {
            String lCode = uiCode;
            List<dtoMacUrlProviderIdentifier> items = UrlService.GetActiveApplicationIdentifiers();
            Language language = CurrentManager.GetLanguage(uiCode);
            if (items != null && items.Any())
            {
                dtoMacUrlProviderIdentifier identifier = View.GetUrlIdentifier(items);
                BaseUrlMacAttribute lAttribute = UrlService.GetProviderLanguageAttribute((identifier == null) ? 0 : identifier.IdProvider);
                if (lAttribute != null)
                    lCode = View.GetUrlAttributeValue(lAttribute.QueryStringName);
            }
            if (lCode != "" && uiCode.Contains(lCode))
                lCode = uiCode;
            if (language == null || (lCode != uiCode && !String.IsNullOrEmpty(lCode)))
                language = CurrentManager.GetLanguageByCodeOrDefault(lCode);
            View.LoadLanguage(language);
        }
        private void InitializeLanguage(Person person)
        {
            Language language = null;
            if (person != null)
                language = CurrentManager.GetLanguage(person.LanguageID);
            if (language == null)
                language = CurrentManager.GetDefaultLanguage();

            View.LoadLanguage(language);
        }

        private void UrlProviderLogon(dtoMacUrlProviderIdentifier identifier,String fromUrl)
        {
            MacUrlAuthenticationProvider provider = UrlService.GetProvider(identifier.IdProvider);
            if (provider != null)
            {
                List<dtoMacUrlUserAttribute> attributes = View.GetTokenAttributes(provider.GetUserAttributes());

                dtoMacUrlToken vToken = provider.ValidateToken(attributes, fromUrl,UserContext.IpAddress, UserContext.ProxyIpAddress);

                //if (!String.IsNullOrEmpty(provider.RemoteLoginUrl))
                //    View.SetExternalWebLogonUrl(provider.RemoteLoginUrl);
                //else if (!String.IsNullOrEmpty(provider.SenderUrl))
                //    View.SetExternalWebLogonUrl(provider.SenderUrl);
                List<ExternalLoginInfo> users = UrlService.FindUserByIdentifier(vToken.UniqueIdentifyer, provider);
                List<ExternalLoginInfo> userIdentifiers = null;
                Person logonUser = null;
                if (View.PreloadForDebug)
                    View.DisplayDebugInfo(vToken);
                else {
                    // NEL CASO IN CUI UN UTENTE ACCEDA CoN DUE IDENTIFICATIVI DISTINTI MA IL SISTEMA NON CONSENTA DUE TAXCODE identici !
                    if (!String.IsNullOrEmpty(vToken.UniqueIdentifyer) && users.Count == 0 && provider.AllowMultipleInsert && !provider.AllowTaxCodeDuplication) {
                        String taxCode = provider.GetAttributeValue(ProfileAttributeType.taxCode, attributes);
                        List<Person> pItems = ProfileService.GetUserByTaxCode(taxCode);
                        if (pItems != null && pItems.Count == 1)
                        {
                            logonUser = pItems[0];
                            userIdentifiers = UrlService.GetUserIdentifiers(pItems[0], provider);
                        }
                    }
                    switch (vToken.Evaluation.Result)
                    {
                        case UrlProviderResult.ValidToken:
                            if (users.Count == 1)
                            {
                                ExternalLoginInfo loginInfo = users[0];
                                if (loginInfo.Person != null)
                                    UpdateProfileByToken(loginInfo.Person, provider, attributes);
                                ExternalLogonManage(vToken, loginInfo, provider, attributes);
                            }
                            else if (logonUser != null && !logonUser.isDisabled )
                            {
                                UpdateProfileByToken(logonUser, provider, attributes);
                                ExternalLogonManage(vToken, UrlService.AddUserInfo(logonUser, provider, vToken.UniqueIdentifyer), provider, attributes);
                            }
                            else if (!String.IsNullOrEmpty(vToken.UniqueIdentifyer) && users.Count == 0)
                            {
                                if (provider.AutoEnroll)
                                {
                                    UrlProviderResult result = UrlProviderResult.ValidToken;
                                    Int32 idOrganization = 0;
                                    Int32 idProfileType = 0;
                                    Int32 idProfile = 0;
                                    List<OrganizationAttributeItem> items = provider.GetOrganizationsInfo(attributes);
                                    if (items == null || items.Count != 1)
                                        result = UrlProviderResult.InvalidToken;
                                    else
                                    {
                                        List<UserProfileAttribute> pAttributes = provider.GetProfileAttributes();

                                        idOrganization = items[0].Organization.Id;
                                        idProfileType = items[0].IdDefaultProfile;

                                        String taxCode = provider.GetAttributeValue(ProfileAttributeType.taxCode, pAttributes, attributes);
                                        if (!provider.AllowTaxCodeDuplication && !UrlService.isUniqueTaxCode(taxCode))
                                        {
                                            result = UrlProviderResult.InvalidToken;
                                            View.DisplayTaxCodeAlreadyPresent();
                                        }
                                        else
                                            idProfile = View.CreateUserProfile(Helper.GetProfileData(provider, pAttributes, attributes, idOrganization, idProfileType), idProfileType, idOrganization, provider, UrlService.GetCredentials(provider, attributes));
                                    }
                                    if (result != UrlProviderResult.ValidToken)
                                        View.DisplayInvalidMessage(UrlProviderResult.InvalidToken);
                                    else if (idProfile == 0)
                                        View.DisplayAutoEnrollmentFailed();
                                    else
                                    {
                                        if (provider.HasCatalogues())
                                            UrlService.UpdateCatalogueAssocation(idProfile, provider, attributes);
                                        ExternalLogonManage(vToken, UrlService.GetUserInfo(provider.Id, idProfile, vToken.UniqueIdentifyer), provider, attributes);
                                    }
                                }
                                else
                                    View.GoToProfile(vToken, lm.Comol.Core.BaseModules.ProfileManagement.RootObject.MacUrlProfileWizard(provider.Id, attributes));
                            }
                            break;
                        default:
                            int idPerson = (users.Count == 1 && users[0].Person != null) ? users[0].Person.Id : 0;

                            if (users.Count == 1 && users[0].Person != null)
                                View.DisplayInvalidMessage(users[0].Person.SurnameAndName, vToken.Evaluation.Result);
                            else
                                View.DisplayInvalidMessage(vToken.Evaluation.Result);

                            if (!String.IsNullOrEmpty(provider.RemoteLoginUrl))
                                View.SetAutoLogonUrl(provider.RemoteLoginUrl);
                            else if (!String.IsNullOrEmpty(provider.SenderUrl))
                                View.SetAutoLogonUrl(provider.SenderUrl);
                            break;
                    }
                }
            }
            else
                View.DisplayUrlAuthenticationUnavailable();
        }
        private void ExternalLogonManage(dtoMacUrlToken vToken, ExternalLoginInfo userInfo, MacUrlAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
        {
            String wizardUrl = lm.Comol.Core.BaseModules.ProfileManagement.RootObject.MacUrlProfileWizard(provider.Id, attributes);
            String defaultUrl = provider.RemoteLoginUrl;

            if (userInfo.Person == null)
                View.GoToProfile(vToken, wizardUrl);
            else if (!userInfo.isEnabled || userInfo.Person.isDisabled)
                View.DisplayAccountDisabled(lm.Comol.Core.BaseModules.ProfileManagement.RootObject.DisabledProfile(provider.Id, userInfo.Person.Id));
            else
            {
                UrlService.UpdateUserAccessTime(userInfo.Person);
                if (userInfo.Person.AcceptPolicy || !PolicyService.UserHasPolicyToAccept(userInfo.Person))
                    View.LogonUser(userInfo.Person, provider.Id, defaultUrl, false,CurrentManager.GetUserDefaultIdOrganization(userInfo.Person.Id));
                else
                    View.DisplayPrivacyPolicy(userInfo.Person.Id, provider.Id, defaultUrl, false);
            }
        }

        #region "Update Profile"
            private void UpdateProfileByToken(Person person, MacUrlAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
            {
                List<UserProfileAttribute> pAttributes = provider.Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(UserProfileAttribute)).Select(p => (UserProfileAttribute)p).ToList();
                Int32 idDefaultType = 0;
                List<OrganizationAttributeItem> items = provider.GetOrganizationsInfo(attributes);
                UpdateOrganizationAssociations(person, provider, attributes);
                Dictionary<ProfileAttributeType, string> agencyAttributes = Helper.GetUserAttributesForAgency(provider, attributes);
                if (items.Count == 1){
                    idDefaultType = items[0].IdDefaultProfile;
                    if (idDefaultType != person.TypeID) {
                        CurrentManager.Detach(person);
                        EditProfileType(person.Id, idDefaultType, items[0].Organization.Id, provider, pAttributes, attributes);
                    }
                    else if (agencyAttributes.Count > 0 && person.TypeID == (int)UserTypeStandard.Employee ) { 
                        Employee emp = (Employee)person;
                        Agency empAgency = Helper.GetAgencyByAttributes(person.Id, items[0].Organization.Id, provider, attributes);
                        if (emp.CurrentAffiliation == null || (empAgency != null && emp.CurrentAffiliation.Agency.Id != empAgency.Id))
                            UpdateAgencyAssocation(person.Id, empAgency);
                    }
                }
                else if (items.Count > 0 && agencyAttributes.Count > 0 && person.TypeID != (int)UserTypeStandard.Employee)
                {
                    idDefaultType = (int)UserTypeStandard.Employee;
                    CurrentManager.Detach(person);
                    EditProfileType(person.Id, idDefaultType, items[0].Organization.Id, provider, pAttributes, attributes);
                }
                else if (items.Count > 0 && agencyAttributes.Count > 0 && person.TypeID == (int)UserTypeStandard.Employee)
                {
                    Employee emp = (Employee)person;
                    Agency empAgency = Helper.GetAgencyByAttributes(emp.Id, items[0].Organization.Id, provider, attributes);
                    if (emp.CurrentAffiliation == null || (empAgency != null && emp.CurrentAffiliation.Agency.Id != empAgency.Id))
                        UpdateAgencyAssocation(person.Id, empAgency);
                }
                if (provider.HasCatalogues())
                    UrlService.UpdateCatalogueAssocation(person.Id, provider, attributes);
            }
            private Boolean UpdateOrganizationAssociations(Person person, MacUrlAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
            {
                Boolean updated = false;
                List<OrganizationAttributeItem> items = provider.GetOrganizationsInfo(attributes);
                List<lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileOrganization> associations = ProfileService.GetProfileOrganizations(person);

                foreach (var item in items.Where(i => !associations.Select(a => a.IdOrganization).ToList().Contains(i.Organization.Id)).ToList())
                {
                    Organization organization = CurrentManager.GetOrganization(item.Organization.Id);
                    if (organization != null)
                    {
                        LazySubscription sub = ServiceCommunity.AddProfileToOrganization(item.Organization.Id, person, false);
                        if (!updated && (sub !=null))
                            updated = true; 
                    }
                }
                if (updated)
                    View.UpdateLogonXml(person.Id);
                // COSaA FARE SE NON SONO PI§ DI UNA ORGANIZZAZIONE ?? CHIEDERE PRIMA DI ESEGUIRE CODICE
                //foreach (var a in associations.Where(a => !items.Select(i => i.IdOrganization).ToList().Contains(a.IdOrganization)).ToList())
                //{
                
                //}
                return updated;
            }
            private Boolean UpdateAgencyAssocation(Int32 idUser, Int32 idOrganization, MacUrlAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
            {
                return UpdateAgencyAssocation(idUser, Helper.GetAgencyByAttributes(idUser, idOrganization, provider, attributes));
            }
            private Boolean UpdateAgencyAssocation(Int32 idUser,Agency agency)
            {
                Boolean saved = false;
                try
                {
                    ProfileService.AddEmployeeAffiliation(agency.Id, idUser);
                }
                catch (Exception ex)
                {

                }
                return saved;
            }
        
            private Boolean EditProfileType(Int32 idProfile,  Int32 idNewType, Int32 idOrganization, MacUrlAuthenticationProvider provider, List<UserProfileAttribute> pAttributes, List<dtoMacUrlUserAttribute> attributes)
            {
                Boolean result = false;
                ProfileTypeChanger person = CurrentManager.Get<ProfileTypeChanger>(idProfile);
                Int32 idOldType = person.TypeID;

                dtoBaseProfile profile = GetCurrentProfileData(idProfile, idOldType, provider.ProviderType);
                if (idProfile > 0 && person != null)
                {
                    Person people = CurrentManager.GetPerson(idProfile);
                    if (people != null)
                        CurrentManager.Detach(people);
                    if (person.TypeID == (int)UserTypeStandard.Company && idNewType != (int)UserTypeStandard.Company)
                        person = ProfileService.EditProfileType(person, idNewType);
                    else if (idNewType == (int)UserTypeStandard.Company)
                        person = ProfileService.EditProfileType(person, idNewType);
                    else if (person.TypeID == (int)UserTypeStandard.Employee && idNewType != (int)UserTypeStandard.Employee)
                        person = ProfileService.EditProfileType(person, idNewType);
                    else if (idNewType == (int)UserTypeStandard.Employee)
                        person = ProfileService.EditProfileType(person, idNewType);
                    if (idOldType != idNewType && person !=null )
                    {
                        if (idNewType == (int)UserTypeStandard.Company)
                        {
                            dtoCompany company = (dtoCompany)Helper.GetProfileData(profile,provider, pAttributes, attributes, idOrganization, idNewType);
                            if (idOldType == (int)UserTypeStandard.Employee || View.DeletePreviousProfileType(idProfile, idOldType, idNewType))
                                result = (ProfileService.SaveCompanyUser(company, null) != null);
                        }
                        else if (idNewType == (int)UserTypeStandard.Employee)
                        {
                            dtoEmployee employee = (dtoEmployee)Helper.GetProfileData(profile, provider, pAttributes, attributes, idOrganization, idNewType);
                            if (idOldType == (int)UserTypeStandard.Company || View.DeletePreviousProfileType(idProfile, idOldType, idNewType))
                            {
                                Employee savedEmployee = ProfileService.SaveEmployee(employee, null);
                                if (savedEmployee != null)
                                {
                                    //long idAgency = employee.CurrentAgency.Key;
                                    //if (idAgency < 1)
                                    //    idAgency = ProfileService.GetEmptyAgency(0).Key;
                                    //SaveAgencyAffiliation(employee.CurrentAgency.Key, IdProfile);
                                    UpdateAgencyAssocation(idProfile, idOrganization, provider, attributes);
                                }
                                result = (savedEmployee != null);
                            }
                        }
                        else
                            result = View.EditProfileType(idProfile, Helper.GetProfileData(profile, provider, pAttributes, attributes, idOrganization, idNewType), idOldType, idNewType);
                        if (result && idOldType == (int)UserTypeStandard.Employee)
                            ProfileService.CloseEmployeeAffiliations(idProfile);
                    }
                }
                return result;
            }
        #endregion

        #region "Add profile"
            private dtoBaseProfile GetCurrentProfileData(Int32 idProfile, Int32 idProfileType, AuthenticationProviderType type)
            {
                dtoBaseProfile oldProfile = null;
                Person person = CurrentManager.GetPerson(idProfile);
                if (person != null && idProfileType != (int)UserTypeStandard.Company && idProfileType != (int)UserTypeStandard.Employee)
                    oldProfile = View.GetOldTypeProfileData(idProfile, idProfileType);
                return Helper.GetCurrentProfileData(idProfile, idProfile, type, oldProfile);
            }
            public Employee AddEmployee(Employee profile)
            {
                return ProfileService.AddEmployee(profile);
            }
            public Employee AddEmployee(Employee profile, long idProvider, dtoExternalCredentials credentials)
            {
                return ProfileService.AddEmployee(profile, idProvider, credentials);
            }

            public CompanyUser AddCompanyUser(CompanyUser profile)
            {
                return ProfileService.AddCompanyUser(profile);
            }
            public CompanyUser AddCompanyUser(CompanyUser profile, long idProvider, dtoExternalCredentials credentials)
            {
                return ProfileService.AddCompanyUser(profile, idProvider, credentials);
            }
            public ExternalLoginInfo AddExternalProfile(Int32 IdPerson, long idProvider, dtoExternalCredentials credentials)
            {
                return ProfileService.AddExternalProfile(IdPerson, idProvider, credentials);
            }
        #endregion
    }
}