using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.Authentication.Business
{
    public class UrlMacAuthenticationService : ExternalAuthenticationService
    {

        #region initClass
            public UrlMacAuthenticationService() :base() { }
            public UrlMacAuthenticationService(iApplicationContext oContext) :base(oContext) {}
            public UrlMacAuthenticationService(iDataContext oDC) : base(oDC) { }
        #endregion

        //public override Boolean isUniqueTaxCode(string taxCode, long idProvider)
        //{

        //    List<Int32> users = (from p in Manager.GetIQ<Person>() where (p.TaxCode == taxCode && !string.IsNullOrEmpty(taxCode)) select p.Id).ToList();
        //    if (users == null || users.Count == 0)
        //        return true;
        //    else { 
        //        return (from u in Manager.GetIQ<ExternalLoginInfo>()
        //                 where u.Deleted== BaseStatusDeleted.None && users.Contains(u.Person.Id))
        //    }
        //}
            public virtual List<ProfilerError> VerifyProfileInfo(dtoBaseProfile profile, long idProvider)
            {
                List<ProfilerError> result = new List<ProfilerError>();
                MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                if (!isUniqueMail(profile.Mail))
                    result.Add(ProfilerError.mailDuplicate);
                if (!String.IsNullOrEmpty(profile.TaxCode) && (provider != null && provider.AllowTaxCodeDuplication) && !isUniqueTaxCode(profile.TaxCode))
                    result.Add(ProfilerError.taxCodeDuplicate);

                return result;
            }
            public override List<ProfilerError> VerifyProfileInfo(dtoBaseProfile profile, long idProvider, dtoExternalCredentials credentials)
            {
                List<ProfilerError> result = new List<ProfilerError>();
                MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                if (!isUniqueMail(profile.Mail))
                    result.Add(ProfilerError.mailDuplicate);
                if (!String.IsNullOrEmpty(profile.TaxCode) && (provider!=null && provider.AllowTaxCodeDuplication)  && !isUniqueTaxCode(profile.TaxCode))
                    result.Add(ProfilerError.taxCodeDuplicate);

                result.AddRange(VerifyProfileInfo(profile, Manager.Get<AuthenticationProvider>(idProvider), credentials));
                return result;
            }
        public ExternalLoginInfo AddFromInternalAccount(InternalLoginInfo internalAccount, MacUrlAuthenticationProvider provider, String externalString)
        {
            return AddUserInfo(internalAccount.Person, internalAccount.Person, provider, externalString);
        }
        public ExternalLoginInfo AddUserInfo(Person person, MacUrlAuthenticationProvider provider, String externalString)
        {
            Person currentUser = Manager.GetPerson(UC.CurrentUserID);
            return AddUserInfo(person, (currentUser == null) ? person : currentUser, provider, externalString);
        }
        private ExternalLoginInfo AddUserInfo(Person person, Person currentUser, MacUrlAuthenticationProvider provider, String externalString)
        {
            ExternalLoginInfo account = null;
            try
            {
                Manager.BeginTransaction();
                //List<String> userIdentifiers = externalString.Split(' ').ToList();
                var query = (from si in Manager.GetIQ<ExternalLoginInfo>() where si.Provider == provider select si);

                List<ExternalLoginInfo> accounts = new List<ExternalLoginInfo>();
                //foreach (String identifier in userIdentifiers.Where(i => !string.IsNullOrEmpty(i)).ToList())
                //{
                //    accounts.AddRange(query.Where(si => si.Provider == provider && si.IdExternalString == identifier).ToList());
                //}
                accounts.AddRange(query.Where(si => si.Provider == provider && si.IdExternalString == externalString).ToList());


                //foreach (String identifier in userIdentifiers.Where(i => !string.IsNullOrEmpty(i)).ToList())
                //{
                if (accounts.Where(a => a.IdExternalString.ToLower() == externalString.ToLower()).Any())
                {
                    account = accounts.Where(a => a.IdExternalString.ToLower() == externalString.ToLower()).FirstOrDefault();
                    if (account != null)
                        UpdateAccount(account, person, currentUser, provider, externalString);
                    else
                        account = CreateAccount(person, currentUser, provider, externalString);
                }
                else
                    account = CreateAccount(person, currentUser, provider, externalString);
                Manager.SaveOrUpdate(account);
                AddToHistory(account);
                //}
                if (person.IdDefaultProvider == 0 || String.IsNullOrEmpty(person.FirstLetter))
                {
                    //// TEMPORANEO
                    //creator.Login = login;
                    //creator.Password=userInfo.Password;
                    //// TEMPORANEO
                    if (person.IdDefaultProvider == 0 && provider != null)
                        person.IdDefaultProvider = provider.Id;
                    if (String.IsNullOrEmpty(person.FirstLetter))
                        person.FirstLetter = person.Surname[0].ToString().ToLower();
                    Manager.SaveOrUpdate(person);
                }
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
            return account;
        }
        public override List<ExternalLoginInfo> FindUserByIdentifier(String identifier, AuthenticationProvider provider)
        {
            List<ExternalLoginInfo> list = null;
            if (provider.MultipleItemsForRecord && !string.IsNullOrEmpty(provider.MultipleItemsSeparator))
            {
                List<String> multipleIdentifiers = null;
                multipleIdentifiers = identifier.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                if (multipleIdentifiers == null || multipleIdentifiers.Count == 0)
                    list = (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString == identifier && i.Provider == provider && i.Person != null select i).ToList();
                else
                {
                    list = new List<ExternalLoginInfo>();
                    List<ExternalLoginInfo> items = new List<ExternalLoginInfo>();
                    foreach (String idn in multipleIdentifiers)
                    {
                        items.AddRange((from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString.Contains(idn) && i.Provider == provider && i.Person != null select i).ToList());
                    }
                    multipleIdentifiers.ForEach(
                        id => 
                        list.AddRange(
                            items.Where(
                                i => i.IdExternalString.Split(
                                    provider.MultipleItemsSeparator.ToArray(), 
                                    StringSplitOptions.RemoveEmptyEntries)
                                .Contains(id)
                            )
                            .ToList()
                            )
                        );

                }
            }
            else
                list = (
                    from i in this.Manager.GetIQ<ExternalLoginInfo>()
                    where i.IdExternalString == identifier 
                        && i.Provider == provider 
                        && i.Person != null
                        && i.Deleted == BaseStatusDeleted.None
                    select i).ToList();
            return list;
        }

        public List<ExternalLoginInfo> GetUserIdentifiers(Person person, AuthenticationProvider provider)
        {
            return (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.Provider == provider && i.Person != null && i.Person.Id== person.Id select i).ToList();
        }

        public List<MacUrlAuthenticationProvider> GetActivePoviders()
        {
            return (from p in this.Manager.GetIQ<MacUrlAuthenticationProvider>() where p.Deleted == BaseStatusDeleted.None && p.IsEnabled select p).ToList();
        }
        //public List<UrlMacAuthenticationProvider> GetActivePoviders(String urlIdentifier)
        //{
        //    return (from p in this.Manager.GetIQ<UrlAuthenticationProvider>() where p.Deleted == BaseStatusDeleted.None && p.UrlIdentifier == urlIdentifier select p).ToList();
        //}
        //public UrlAuthenticationProvider GetActivePovider(ApplicationAttribute apIdentifier, FunctionAttribute fnIdentifier)
        //{
        //    return (from p in this.Manager.GetIQ<UrlAuthenticationProvider>()
        //            where p.Deleted == BaseStatusDeleted.None && p.UrlIdentifier == urlIdentifier
        //            select p).Skip(0).Take(1).ToList().FirstOrDefault();
        //}
        public MacUrlAuthenticationProvider GetProvider(long idProvider)
        {
            try
            {
                return Manager.Get<MacUrlAuthenticationProvider>(idProvider);
            }
            catch (Exception ex) {
                return null;
            }
        }
        public UserProfileAttribute GetProviderLanguageAttribute(long idProvider)
        {
            try
            {
                return (from a in Manager.GetIQ<UserProfileAttribute>() where a.Deleted == BaseStatusDeleted.None && a.Provider.Id == idProvider && a.Attribute == ProfileAttributeType.language select a).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<dtoMacUrlProviderIdentifier> GetActiveApplicationIdentifiers()
        {
            List<dtoMacUrlProviderIdentifier> identifiers = new List<dtoMacUrlProviderIdentifier>();
            List<BaseUrlMacAttribute> items = (from p in this.Manager.GetIQ<BaseUrlMacAttribute>()
                    where p.Deleted == BaseStatusDeleted.None && (p.Type== UrlMacAttributeType.applicationId || p.Type == UrlMacAttributeType.functionId)
                    select p).ToList();
            foreach (var item in items.GroupBy(i=> i.Provider).ToList()){
                identifiers.Add(new dtoMacUrlProviderIdentifier()
                {
                    IdProvider = item.Key.Id,
                    isEnabled = item.Key.IsEnabled,
                    Application = (ApplicationAttribute)item.Where(i => i.GetType() == typeof(ApplicationAttribute)).FirstOrDefault()
                     ,
                    Function = (FunctionAttribute)item.Where(i => i.GetType() == typeof(FunctionAttribute)).FirstOrDefault()
                });
            }
            return identifiers;
        }

        public Boolean UpdateCatalogueAssocation(Int32 idUser, MacUrlAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
        {
            Boolean saved = false;
            try
            {
                Manager.BeginTransaction();
                Person person = Manager.GetPerson(idUser);
                if (person != null) {
                    List<String> userCodes = new List<String>();
                    List<long> idCatalogues = new List<long>();

                    if (provider.Attributes.Where(a => a.Type == UrlMacAttributeType.coursecatalogue && a.Deleted == BaseStatusDeleted.None).Any()) {
                        List<CatalogueAttribute> cAttributes = provider.Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.Type == UrlMacAttributeType.coursecatalogue && p.GetType() == typeof(CatalogueAttribute)).Select(p => (CatalogueAttribute)p).ToList();
                        foreach (dtoMacUrlUserAttribute uAtt in attributes.Where(a => a.Type == UrlMacAttributeType.coursecatalogue && !String.IsNullOrEmpty(a.QueryValue)).ToList())
                        {
                            CatalogueAttribute pAtt = cAttributes.Where(a => a.Id == uAtt.Id).FirstOrDefault();
                            if (pAtt != null)
                            {
                                if (pAtt.AllowMultipleValue)
                                    userCodes.AddRange(uAtt.QueryValue.Split(pAtt.MultipleValueSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList());
                                else
                                    userCodes.Add(uAtt.QueryValue);
                            }
                            idCatalogues.AddRange(pAtt.Items.Where(i => i.Deleted == BaseStatusDeleted.None && userCodes.Contains(i.RemoteCode)).Select(i => i.Catalogue.Id).ToList());
                        }
                        idCatalogues = idCatalogues.Distinct().ToList();
                    }

                    List<lm.Comol.Core.Catalogues.CataloguePersonAssignment> assignments = (from a in Manager.GetIQ<lm.Comol.Core.Catalogues.CataloguePersonAssignment>()
                                                                                            where a.AssignedTo.Id == idUser && a.Catalogue !=null
                                                                                            select a).ToList();
                    foreach (lm.Comol.Core.Catalogues.CataloguePersonAssignment assignment in assignments)
                    {
                        if (idCatalogues.Contains(assignment.Catalogue.Id) && !assignment.Allowed)
                        {
                            assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            if (assignment.Deleted != BaseStatusDeleted.None)
                            {
                                assignment.FromProvider = true;
                                assignment.Deleted = BaseStatusDeleted.None;
                            }
                            assignment.Allowed = true;
                            Manager.SaveOrUpdate(assignment);
                        }
                        else if (!idCatalogues.Contains(assignment.Catalogue.Id) && assignment.Deleted == BaseStatusDeleted.None && assignment.Allowed && assignment.FromProvider)
                        {
                            assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            assignment.Allowed = false;
                            Manager.SaveOrUpdate(assignment);
                        }
                    }
                    foreach (long idCatalogue in idCatalogues.Where(i => !assignments.Select(a => a.Catalogue.Id).ToList().Contains(i)).ToList())
                    {
                        Catalogues.Catalogue catalogue = Manager.Get<Catalogues.Catalogue>(idCatalogue);
                        if (catalogue != null)
                        {
                            lm.Comol.Core.Catalogues.CataloguePersonAssignment pAssignment = new Catalogues.CataloguePersonAssignment();
                            pAssignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            pAssignment.FromProvider = true;
                            pAssignment.AssignedTo = person;
                            pAssignment.Allowed = true;
                            pAssignment.Catalogue = catalogue;
                            Manager.SaveOrUpdate(pAssignment);
                        }
                    }
                    saved = true;
                }
                Manager.Commit();
            }

            catch (Exception ex) {
                saved = false;
                Manager.RollBack();
            }
           
            return saved;
        }
  
        //public Boolean UpdateOrganizationsAssocation(Int32 idUser, UrlMacAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
        //{
        //    Boolean saved = false;
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        Person person = Manager.GetPerson(idUser);
        //        if (person != null)
        //        {
        //            List<OrganizationAttributeItem> items = provider.GetOrganizationsInfo(attributes);

        //            List<OrganizationAttribute> oAttributes = provider.Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(OrganizationAttribute)).Select(p => (OrganizationAttribute)p).ToList();
        //            List<String> userCodes = new List<String>();
        //            List<Int32> idOrganizations = new List<Int32>();
        //            foreach (dtoMacUrlUserAttribute uAtt in attributes.Where(a => a.Type == UrlMacAttributeType.organization && !String.IsNullOrEmpty(a.QueryValue)).ToList())
        //            {
        //                OrganizationAttribute oAtt = oAttributes.Where(a => a.Id == uAtt.Id).FirstOrDefault();
        //                if (oAtt != null)
        //                {
        //                    if (oAtt.AllowMultipleValue)
        //                        userCodes.AddRange(uAtt.QueryValue.Split(oAtt.MultipleValueSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList());
        //                    else
        //                        userCodes.Add(uAtt.QueryValue);
        //                }
        //                idOrganizations.AddRange(oAtt.Items.Where(i => i.Deleted == BaseStatusDeleted.None && userCodes.Contains(i.RemoteCode)).Select(i => i.IdOrganization).ToList());
        //            }

        //            List<OrganizationProfiles> associations = (from a in Manager.GetIQ<OrganizationProfiles>()
        //                                                        where a.Profile.Id== idUser
        //                                                         select a).ToList();
        //            //foreach (lm.Comol.Core.Catalogues.CataloguePersonAssignment assignment in assignments)
        //            //{
        //            //    if (idCatalogues.Contains(assignment.Id) && !assignment.Allowed)
        //            //    {
        //            //        assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //            //        if (assignment.Deleted != BaseStatusDeleted.None)
        //            //        {
        //            //            assignment.FromProvider = true;
        //            //            assignment.Deleted = BaseStatusDeleted.None;
        //            //        }
        //            //        assignment.Allowed = true;
        //            //        Manager.SaveOrUpdate(assignment);
        //            //    }
        //            //    else if (!idCatalogues.Contains(assignment.Id) && assignment.Deleted == BaseStatusDeleted.None && assignment.Allowed && assignment.FromProvider)
        //            //    {
        //            //        assignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //            //        assignment.Allowed = false;
        //            //        Manager.SaveOrUpdate(assignment);
        //            //    }
        //            //}
        //            //foreach (long idCatalogue in idCatalogues.Where(i => !assignments.Select(a => a.Id).ToList().Contains(i)).ToList())
        //            //{
        //            //    lm.Comol.Core.Catalogues.CataloguePersonAssignment pAssignment = new Catalogues.CataloguePersonAssignment();
        //            //    pAssignment.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //            //    pAssignment.FromProvider = true;
        //            //    pAssignment.AssignedTo = person;
        //            //    pAssignment.Allowed = true;
        //            //    Manager.SaveOrUpdate(pAssignment);
        //            //}
        //            saved = true;
        //        }
        //        Manager.Commit();
        //    }

        //    catch (Exception ex)
        //    {
        //        saved = false;
        //        Manager.RollBack();
        //    }

        //    return saved;
        //}

        public dtoExternalCredentials GetCredentials(MacUrlAuthenticationProvider provider, List<dtoMacUrlUserAttribute> attributes)
        {
            dtoExternalCredentials credentials = new dtoExternalCredentials();
            if (attributes.Where(a=> a.isIdentifier).Any())
                 credentials.IdentifierString =attributes.Where(a=> a.isIdentifier).FirstOrDefault().QueryValue;
            else{
                UserProfileAttribute pAttribute = provider.Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(UserProfileAttribute)).Where(p => ((UserProfileAttribute)p).Attribute == ProfileAttributeType.externalId).Select(p => (UserProfileAttribute)p).FirstOrDefault();
                if (pAttribute != null)
                    credentials.IdentifierString = attributes.Where(i => i.Type == UrlMacAttributeType.profile && i.Id == pAttribute.Id).Select(i => i.QueryValue).FirstOrDefault();
                else {
                    CompositeProfileAttribute cmpAttribute = provider.Attributes.Where(p => p.Deleted == BaseStatusDeleted.None && p.GetType() == typeof(CompositeProfileAttribute)).Where(p => ((CompositeProfileAttribute)p).Attribute == ProfileAttributeType.externalId).Select(p => (CompositeProfileAttribute)p).FirstOrDefault();
                    if (!cmpAttribute.Items.Where(i => i.Deleted == BaseStatusDeleted.None).Any())
                        credentials.IdentifierString = "";
                    else
                        credentials.IdentifierString = (attributes.Where(i => i.Id == cmpAttribute.Id).Any()) ? attributes.Where(i => i.Id == cmpAttribute.Id).Select(i => i.QueryValue).FirstOrDefault() : "";
                }
            }            
            return credentials;
        }

        public ExternalLoginInfo GetUserInfo(long idProvider, Int32 idUser, String identifier) {
            ExternalLoginInfo user = null;
            try
            {
                user = (from ui in Manager.GetIQ<ExternalLoginInfo>() where ui.Deleted == BaseStatusDeleted.None && ui.Person.Id == idUser && ui.Provider.Id == idProvider && ui.IdExternalString == identifier select ui).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex) { }

            return user;
        }
    }
}