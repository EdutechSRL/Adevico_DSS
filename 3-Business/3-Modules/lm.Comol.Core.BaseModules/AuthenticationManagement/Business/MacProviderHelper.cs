using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Business
{
    public class MacProviderHelper
    {
        private UrlMacAuthenticationService UrlService;
        private BaseModuleManager CurrentManager;
        private ProfileManagement.Business.ProfileManagementService ProfileService;

        public MacProviderHelper(BaseModuleManager manager,UrlMacAuthenticationService uService, ProfileManagement.Business.ProfileManagementService pService)
        {
            UrlService = uService;
            ProfileService = pService;
            CurrentManager = manager;
        }
        public dtoBaseProfile GetCurrentProfileData(Int32 idProfile, Int32 idProfileType, AuthenticationProviderType type, dtoBaseProfile oldProfile )
        {
            dtoBaseProfile profile = new dtoBaseProfile();
            Person person = CurrentManager.GetPerson(idProfile);
            if (person != null)
            {
                Language language = CurrentManager.GetLanguage(person.LanguageID);
                switch (idProfileType)
                {
                    case (int)UserTypeStandard.Company:
                        profile = new dtoCompany(CurrentManager.Get<CompanyUser>(idProfile));
                        break;
                    case (int)UserTypeStandard.Employee:
                        profile = new dtoEmployee(CurrentManager.Get<Employee>(idProfile));
                        break;
                    default:
                        profile = oldProfile;
                        break;
                }
                profile.IdProfileType = idProfileType;
                profile.AuthenticationProvider = type;
                profile.IdLanguage = language.Id;
                profile.LanguageName = language.Name;
            }
            return profile;
        }
        public dtoBaseProfile GetProfileData(dtoBaseProfile previous, MacUrlAuthenticationProvider provider, List<UserProfileAttribute> pAttributes, List<dtoMacUrlUserAttribute> attributes, Int32 idOrganization, Int32 idProfileType)
        {
            dtoBaseProfile result = GetProfileData(provider, pAttributes, attributes, idOrganization, idProfileType);
            if (previous != null)
            {
                result.Id = previous.Id;
                result.Name = previous.Name;
                result.Surname = previous.Surname;
                if (String.IsNullOrEmpty(provider.GetAttributeValue(ProfileAttributeType.taxCode, pAttributes, attributes)))
                    result.TaxCode = previous.TaxCode;

                if (result.Mail == result.Login + "@invalid.invalid.it" && !previous.Mail.Contains("@invalid.invalid.it"))
                    result.Mail = previous.Mail;
            }
            return result;
        }
        public dtoBaseProfile GetProfileData(MacUrlAuthenticationProvider provider, List<UserProfileAttribute> pAttributes, List<dtoMacUrlUserAttribute> attributes, Int32 idOrganization, Int32 idProfileType)
        {
            dtoBaseProfile profile = new dtoBaseProfile();
            String pwd = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, true, true, false);
            Language language = GetUserLanguage(provider.GetAttributeValue(ProfileAttributeType.language, pAttributes, attributes));

            switch (idProfileType)
            {
                case (int)UserTypeStandard.ExternalUser:
                    profile = new dtoExternal();
                    break;
                case (int)UserTypeStandard.Company:
                    profile = new dtoCompany();
                    break;
                case (int)UserTypeStandard.Employee:
                    profile = new dtoEmployee();
                    break;
                default:
                    profile = new dtoBaseProfile();
                    break;
            }
            profile.Login = provider.GetAttributeValue(ProfileAttributeType.login, pAttributes, attributes);
            if (String.IsNullOrEmpty(profile.Login))
                profile.Login = provider.GetAttributeValue(ProfileAttributeType.externalId, attributes);

            profile.Name = provider.GetAttributeValue(ProfileAttributeType.name, pAttributes, attributes);
            profile.Surname = provider.GetAttributeValue(ProfileAttributeType.surname, pAttributes, attributes);
            profile.TaxCode = provider.GetAttributeValue(ProfileAttributeType.taxCode, pAttributes, attributes);
            if (String.IsNullOrEmpty(profile.TaxCode))
                profile.TaxCode = UrlService.GenerateRandomTaxCode();
            profile.Mail = provider.GetAttributeValue(ProfileAttributeType.mail, pAttributes, attributes);
            if (String.IsNullOrEmpty(profile.Mail))
                profile.Mail = profile.Login + "@invalid.invalid.it";
            //if (!String.IsNullOrEmpty(profile.Mail))
            //    profile.Mail = profile.Mail.ToLower();
            profile.Password = pwd;
            profile.ShowMail = false;
            if (!String.IsNullOrEmpty(profile.Surname))
                profile.FirstLetter = profile.Surname[0].ToString().ToLower();
            profile.IdProfileType = idProfileType;
            profile.AuthenticationProvider = provider.ProviderType;
            profile.IdLanguage = language.Id;
            profile.LanguageName = language.Name;


            switch (idProfileType)
            {
                case (int)UserTypeStandard.Company:
                    dtoCompany dCompany = (dtoCompany)profile;
                    dCompany.Info.Address = provider.GetAttributeValue(ProfileAttributeType.companyAddress, pAttributes, attributes);
                    dCompany.Info.City = provider.GetAttributeValue(ProfileAttributeType.companyCity, pAttributes, attributes);
                    dCompany.Info.Name = provider.GetAttributeValue(ProfileAttributeType.companyName, pAttributes, attributes);
                    dCompany.Info.Region = provider.GetAttributeValue(ProfileAttributeType.companyRegion, pAttributes, attributes);
                    dCompany.Info.TaxCode = provider.GetAttributeValue(ProfileAttributeType.companyTaxCode, pAttributes, attributes);
                    dCompany.Info.ReaNumber = provider.GetAttributeValue(ProfileAttributeType.companyReaNumber, pAttributes, attributes);
                    dCompany.Info.AssociationCategories = provider.GetAttributeValue(ProfileAttributeType.companyAssociations, pAttributes, attributes);
                    return dCompany;

                case (int)UserTypeStandard.Employee:
                    dtoEmployee dEmployee = (dtoEmployee)profile;
                    Person anonymous = ProfileService.GetAnonymousUser();
                    Agency agency = null;
                    if (anonymous == null)
                    {
                        Dictionary<ProfileAttributeType, string> agencyAttributes = GetUserAttributesForAgency(provider, attributes);
                        agency = ProfileService.GetAgency(agencyAttributes);
                        if (agency == null)
                            agency = ProfileService.GetDefaultAgency(idOrganization);
                    }
                    else
                        agency = GetAgencyByAttributes(anonymous.Id, idOrganization, provider, attributes);

                    if (agency != null)
                        dEmployee.CurrentAgency = new KeyValuePair<long, string>(agency.Id, agency.Name);
                    else
                        dEmployee.CurrentAgency = ProfileService.GetEmptyAgency(idOrganization);

                    return dEmployee;

             
                case (int)UserTypeStandard.ExternalUser:
                    dtoExternal dExternal = (dtoExternal)profile;
                    dExternal.ExternalUserInfo = provider.GetAttributeValue(ProfileAttributeType.externalUserInfo, pAttributes, attributes);
                    return dExternal;
               
                default:
                    return profile;
            }
        }
        public Language GetUserLanguage(String lCode)
        {
            Language result = null;
            if (!String.IsNullOrEmpty(lCode))
            {
                List<Language> languages = (from l in CurrentManager.GetIQ<Language>() where l.Code == lCode || l.Code.StartsWith(lCode + "-") select l).ToList();
                if (languages.Count == 1 || languages.Count > 1)
                    result = languages.FirstOrDefault();
            }
            return (result == null) ? CurrentManager.GetDefaultLanguage() : result;
        }
        public Agency GetAgencyByAttributes(Int32 idProfile, Int32 idOrganization, MacUrlAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
        {
            List<UserProfileAttribute> pAttributes = provider.Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(UserProfileAttribute)).Select(p => (UserProfileAttribute)p).ToList();

            Dictionary<ProfileAttributeType, string> items = GetUserAttributesForAgency(provider, attributes);
            Agency agency = ProfileService.GetAgency(items);
            if (agency == null && provider.AutoAddAgency && items.Values.Where(v=>String.IsNullOrEmpty(v)).Any())
                agency = ProfileService.SaveAgency(idProfile, items);
            if (agency == null)
                agency = ProfileService.GetDefaultAgency(idOrganization);

            if (agency == null)
                agency = ProfileService.GetEmptyAgencyForOrganization(idOrganization);
            return agency;
        }
        public Dictionary<ProfileAttributeType, string> GetUserAttributesForAgency(MacUrlAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
        {
            List<UserProfileAttribute> pAttributes = provider.Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(UserProfileAttribute)).Select(p => (UserProfileAttribute)p).ToList();

            Dictionary<ProfileAttributeType, string> items = new Dictionary<ProfileAttributeType, string>();
            items.Add(ProfileAttributeType.agencyExternalCode, provider.GetAttributeValue(ProfileAttributeType.agencyExternalCode, pAttributes, attributes));
            items.Add(ProfileAttributeType.agencyTaxCode, provider.GetAttributeValue(ProfileAttributeType.agencyTaxCode, pAttributes, attributes));
            items.Add(ProfileAttributeType.agencyNationalCode, provider.GetAttributeValue(ProfileAttributeType.agencyNationalCode, pAttributes, attributes));
            items.Add(ProfileAttributeType.agencyInternalCode, provider.GetAttributeValue(ProfileAttributeType.agencyInternalCode, pAttributes, attributes));

            return items.Where(i => !String.IsNullOrEmpty(i.Value)).ToDictionary(k => k.Key, k => k.Value);
        }


    }
}