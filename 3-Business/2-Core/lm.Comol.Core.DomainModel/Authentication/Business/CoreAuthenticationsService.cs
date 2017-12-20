using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.Authentication.Business
{
    public class CoreAuthenticationsService : CoreServices
    {
        private const string UniqueCode = "COREauth";
        private iApplicationContext _Context;

        #region initClass
            public CoreAuthenticationsService() { }
            public CoreAuthenticationsService(iApplicationContext oContext)
            {
                this.Manager = new BaseModuleManager(oContext.DataContext);
                _Context = oContext;
                this.UC = oContext.UserContext;
            }
            public CoreAuthenticationsService(iDataContext oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext();
                _Context.DataContext = oDC;
                this.UC = null;
            }
        #endregion

            public Boolean IsActivatedUserMail(Int32 iduser, Guid urlIdentifier)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    Person person = Manager.GetPerson(iduser);
                    if (person != null)
                        result = person.isDisabled==false || !(from w in Manager.GetIQ<WaitingActivationProfile>() where w.Person == person && w.UrlIdentifier == urlIdentifier select w).Any();
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }

                return result;
            }
        public Boolean ActivateUserMail(Int32 iduser,Guid urlIdentifier)
        {
            Boolean result = false;
            try
            {
                Manager.BeginTransaction();
                Person person = Manager.GetPerson(iduser);
                WaitingActivationProfile waiting = (from w in Manager.GetIQ<WaitingActivationProfile>() where w.Person == person && w.UrlIdentifier == urlIdentifier select w).Skip(0).Take(1).ToList().FirstOrDefault();
                if (waiting != null)
                {
                    Manager.DeletePhysical(waiting);
                    person.isDisabled = false;

                    List<ExternalLoginInfo> logins = (from p in Manager.GetIQ<ExternalLoginInfo>() where p.Person == person && p.isEnabled == false && p.CreatedBy == p.Person select p).ToList();
                    foreach (ExternalLoginInfo login in logins)
                    {
                        login.isEnabled = true;
                        Manager.SaveOrUpdate(login);
                    }
                }
                Manager.Commit();
                result = true;
            }
            catch (Exception ex) {
                Manager.RollBack();
            }

            return result;
        }
        public Boolean IsProfileWaitingForActivation(Person person)
        {
            Boolean result = false;
            try
            {
                Manager.BeginTransaction();
                result = (from w in Manager.GetIQ<WaitingActivationProfile>() where w.Person == person && w.Deleted== BaseStatusDeleted.None select w).Any();
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }

            return result;
        }

        public AuthenticationProvider GetActiveProvider(String externalProviderCode)
        {
            return  (from p in this.Manager.GetIQ<AuthenticationProvider>()
                     where p.Deleted == BaseStatusDeleted.None && p.UniqueCode == externalProviderCode 
                     select p).Skip(0).Take(1).ToList().FirstOrDefault();

        }
        public List<AuthenticationProvider> GetActiveProviders()
        {
            return (from p in this.Manager.GetIQ<AuthenticationProvider>() where p.IsEnabled select  p).ToList();

        }
        public List<AuthenticationProvider> GetActiveProviders(AuthenticationProviderType type)
        {
            return (from p in this.Manager.GetIQ<AuthenticationProvider>() where p.ProviderType == type && p.Deleted == BaseStatusDeleted.None && p.IsEnabled select p).ToList();
        }
        public Boolean HasActivePoviders(AuthenticationProviderType type)
        {
            return (from p in this.Manager.GetIQ<AuthenticationProvider>() where p.ProviderType == type && p.Deleted == BaseStatusDeleted.None && p.IsEnabled select p.Id).Any();
        }

        public String GenerateRandomTaxCode()
        {
            String result = "";
            int timeToFind = 100;
            while (timeToFind > 0)
            {
                result = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(16, 16, true, true, false);
                if (isUniqueTaxCode(result))
                    timeToFind = 0;
                else
                    timeToFind--;
            }
            return result;
        }
        public String GetModuleUrlByIdentifier(int IdCommunity, int IdUser, int IdLanguage, String moduleCode, String modulePage)
        {
            Language language = Manager.GetLanguage(IdLanguage);
            String languageCode = "";
            if (language != null)
                languageCode = language.Code;
            ModulePage page = (from p in Manager.GetIQ<ModulePage>()
                    where p.Deleted == BaseStatusDeleted.None && p.ModuleCode == moduleCode && p.Name == modulePage
                    select p).Skip(0).Take(1).ToList().FirstOrDefault();

            if (page == null)
                return "";
            else
                return page.GetUrlWithContext(IdCommunity, IdUser, languageCode);
        }

        public Boolean isUniqueTaxCode(string taxCode)
        {
            return !PersonExist("", taxCode);
        }
        public Boolean isUniqueMail(String mail)
        {
            return !PersonExist(mail, "");
        }
        protected Boolean PersonExist(String mail, String taxCode)
        {
            return (from p in Manager.GetIQ<litePerson>() where p.Mail == mail || (p.TaxCode == taxCode && !string.IsNullOrEmpty(taxCode)) select p.Id).Any();
        }
        public Boolean isInternalUniqueLogin(String login)
        {
            return IsInternalUniqueLogin(login, false);
        }
        public Boolean isInternalActiveUniqueLogin(String login)
        {
            return IsInternalUniqueLogin(login,true);
        }
        private Boolean IsInternalUniqueLogin(String login, Boolean active)
        {
            return !(from i in Manager.GetIQ<InternalLoginInfo>() where i.Login == login && (!active || (active && i.Deleted == BaseStatusDeleted.None)) select i.Id).Any();
        }
       
        //public String GetNewLogin(String name, string surname)
        //{
        //    int number = 1;
        //    String result = name + "." + surname;
        //    while (!isUniqueLogin(result))
        //    {
        //        result = name + "." + surname + "_" + number.ToString();
        //        number++;
        //    }
        //    return result;
        //}

        public Int32 GetDefaultLogonCommunity(Person person)
        {
            Int32 idCommunity = 0;
            try
            {
                idCommunity = (from d in Manager.GetIQ<ProfileDefaultCommunity>()
                               where d.Person == person && d.isEnabled && d.Deleted == BaseStatusDeleted.None
                               select d.IdCommunity).Skip(0).Take(1).FirstOrDefault();
            }
            catch (Exception ex) { 
            }
            return idCommunity;
        }
        public Person GetDefaultUser(UserTypeStandard type)
        {
            Person person= null;
            try
            {
                person = (from p in Manager.GetIQ<Person>()
                          where p.TypeID == (int)type
                          select p).Skip(0).Take(1).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return person;
        }
        public virtual List<ProfilerError> VerifyExistingProfile(dtoBaseProfile profile, Boolean verifyTaxCode)
        {
            List<ProfilerError> result = new List<ProfilerError>();
            if ((from p in Manager.GetIQ<Person>() where p.Mail == profile.Mail && p.Id != profile.Id select p.Id).Any())
                result.Add(ProfilerError.mailDuplicate);
            if (verifyTaxCode){
                List<MacUrlAuthenticationProvider> mProviders = (from m in Manager.GetIQ<MacUrlAuthenticationProvider>()
                                                           where m.IsEnabled && m.Deleted== BaseStatusDeleted.None && m.AllowTaxCodeDuplication 
                                                           select m).ToList();
                if (mProviders.Count == 0 && (from p in Manager.GetIQ<Person>() where p.TaxCode == profile.TaxCode && p.Id != profile.Id select p.Id).Any())
                    result.Add(ProfilerError.taxCodeDuplicate);
                else {
                    List<Int32> idUsers = (from p in Manager.GetIQ<Person>() where p.TaxCode == profile.TaxCode && p.Id != profile.Id select p.Id).ToList();
                    var query = (from e in Manager.GetIQ<ExternalLoginInfo>()
                                 where e.Deleted == BaseStatusDeleted.None && e.Person != null
                                     && idUsers.Contains(e.Person.Id)
                                 select new { IdPerson = e.Person.Id, IdProvider = e.Provider.Id }).ToList();
                    List<Int32> idProviderUsers = (from e in query
                                                   where mProviders.Select(p => p.Id).Contains(e.IdProvider)
                                                   select e.IdPerson).ToList();
                    if (idUsers.Where(u=> !idProviderUsers.Contains(u)).Any())
                        result.Add(ProfilerError.taxCodeDuplicate);
                }
            }

            return result;
        }
        public virtual List<ProfilerError> VerifyProfileInfo(dtoBaseProfile profile){
            List<ProfilerError> result = new List<ProfilerError>();
            if (!isUniqueMail(profile.Mail))
                result.Add(ProfilerError.mailDuplicate);
            if (!String.IsNullOrEmpty(profile.TaxCode) && !isUniqueTaxCode(profile.TaxCode))
                result.Add(ProfilerError.taxCodeDuplicate);
            return result;
        }
        public virtual List<ProfilerError> VerifyProfileInfo(dtoBaseProfile profile, long idProvider, dtoExternalCredentials credentials)
        {
            List<ProfilerError> result = new List<ProfilerError>();
            if (!isUniqueMail(profile.Mail))
                result.Add(ProfilerError.mailDuplicate);
            if (!String.IsNullOrEmpty(profile.TaxCode) && !isUniqueTaxCode(profile.TaxCode))
                result.Add(ProfilerError.taxCodeDuplicate);


            result.AddRange(VerifyProfileInfo(profile, Manager.Get<AuthenticationProvider>(idProvider),credentials));
            return result;
        }
        public virtual List<ProfilerError> VerifyProfileInfo(dtoBaseProfile profile, AuthenticationProvider provider, dtoExternalCredentials credentials)
        {
            List<ProfilerError> result = new List<ProfilerError>();
            if (provider != null) {
                if (provider.ProviderType== AuthenticationProviderType.Internal && (from ei in Manager.GetIQ<InternalLoginInfo>()
                                                                                    where ei.Provider == provider && ei.Login == profile.Login 
                     select ei.Id).Any())
                    result.Add(ProfilerError.loginduplicate);
                else if ((from ei in Manager.GetIQ<ExternalLoginInfo>()
                     where ei.Provider == provider && ei.IdExternalLong == credentials.IdentifierLong && (ei.IdExternalString == credentials.IdentifierString)
                     select ei.Id).Any())
                    result.Add(ProfilerError.externalUniqueIDduplicate);

            }
            return result;
        }
        public virtual ProfilerError VerifyProfileInfo(Person person, String login)
        {
            ProfilerError result = ProfilerError.none;
            if ((from ei in Manager.GetIQ<InternalLoginInfo>()
                 where ei.Deleted== BaseStatusDeleted.None && ei.Person != null && ei.Person != person && ei.Login == login
                 select ei.Id).Any())
                result = ProfilerError.loginduplicate;

            return result;
        }
        public virtual ProfilerError VerifyDuplicateExternalLoginInfo(Person person, AuthenticationProvider provider, dtoExternalCredentials credentials)
        {
            var query = (from ei in Manager.GetIQ<ExternalLoginInfo>() where ei.Deleted==  BaseStatusDeleted.None && ei.Provider == provider && ei.Person != null && ei.Person != person select ei);

            if ((provider.IdentifierFields & IdentifierField.longField) > 0 && query.Where(ei=> ei.IdExternalLong == credentials.IdentifierLong).Any())
                return ProfilerError.externalUniqueIDduplicate;
            else if ((provider.IdentifierFields & IdentifierField.stringField) > 0) {
                List<String> multipleIdentifiers = null;
                if (provider.MultipleItemsForRecord && !string.IsNullOrEmpty(provider.MultipleItemsSeparator)){
                    multipleIdentifiers = credentials.IdentifierString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<ExternalLoginInfo> items = new List<ExternalLoginInfo>();
                    foreach (String idn in multipleIdentifiers)
                    {
                        items.AddRange(query.Where(q=> q.IdExternalString.Contains(idn)).ToList());
                    }
                    if ((provider.IdentifierFields & IdentifierField.longField) > 0)
                        items = items.Where(i => i.IdExternalLong == credentials.IdentifierLong).ToList();
                    Boolean found = false;
                    foreach (ExternalLoginInfo item in items)
                    {
                        found = item.IdExternalString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList().Where(v => multipleIdentifiers.Contains(v)).Any();
                        if (found)
                            break;
                    }
                    return (found) ? ProfilerError.externalUniqueIDduplicate : ProfilerError.none;
                }
                else
                    return (query.Where(ei=> ei.IdExternalString== credentials.IdentifierString).Any()) ? ProfilerError.externalUniqueIDduplicate : ProfilerError.none;
            }
            else return ProfilerError.none;
        }

        public void UpdateUserInfo(Person person, String mail ="")
        {
            try
            {
                Manager.BeginTransaction();
                person.PreviousAccess = person.LastAccessOn;
                person.LastAccessOn = DateTime.Now;
                if (!String.IsNullOrEmpty(mail))
                    person.Mail = mail;
                Manager.SaveOrUpdate(person);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
        }
        public void UpdateUserAccessTime(Person person)
        {
            try
            {
                Manager.BeginTransaction();
                person.PreviousAccess = person.LastAccessOn;
                person.LastAccessOn = DateTime.Now;
                Manager.SaveOrUpdate(person);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
        }
        public void UpdateUserMail(Person person, String mail)
        {
            try
            {
                Manager.BeginTransaction();
                person.Mail = mail;
                Manager.SaveOrUpdate(person);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
        }
    }
}