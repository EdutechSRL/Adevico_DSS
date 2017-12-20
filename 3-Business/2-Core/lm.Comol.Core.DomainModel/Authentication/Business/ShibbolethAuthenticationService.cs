using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.Authentication.Business
{
    public class ShibbolethAuthenticationService : ExternalAuthenticationService
    {

        #region initClass
            public ShibbolethAuthenticationService() :base() { }
            public ShibbolethAuthenticationService(iApplicationContext oContext) :base(oContext) {}
            public ShibbolethAuthenticationService(iDataContext oDC) : base(oDC) { }
        #endregion

            public ExternalLoginInfo AddFromInternalAccount(InternalLoginInfo internalAccount, ShibbolethAuthenticationProvider provider, String externalString) {
                return AddUserInfo(internalAccount.Person, internalAccount.Person, provider, externalString);
            }

            public ExternalLoginInfo AddUserInfo(Person person, ShibbolethAuthenticationProvider provider, String externalString) {
                Person currentUser = Manager.GetPerson(UC.CurrentUserID);
                return AddUserInfo(person, (currentUser == null) ? person : currentUser, provider, externalString);
            }

            private ExternalLoginInfo AddUserInfo(Person person,  Person currentUser, ShibbolethAuthenticationProvider provider, String externalString) {
                ExternalLoginInfo account = null;
                try
                {
                    Manager.BeginTransaction();

                    // find accounts for user
                    List<ExternalLoginInfo> accounts = (from si in Manager.GetIQ<ExternalLoginInfo>() where si.Person == person && si.Provider == provider select si).ToList();
                    // verify if externalString must be splitted
                    List<String> multipleIdentifiers = null;
                    if (provider.MultipleItemsForRecord && !string.IsNullOrEmpty(provider.MultipleItemsSeparator))
                        multipleIdentifiers = externalString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (multipleIdentifiers == null || multipleIdentifiers.Count == 0)
                    {
                        account = accounts.Where(a => a.IdExternalString.ToLower() == externalString.ToLower()).FirstOrDefault();
                        if (account == null)
                            account = CreateAccount(person, currentUser, provider, externalString);
                        else
                            UpdateAccount(account, person, currentUser, provider, externalString);
                        Manager.SaveOrUpdate(account);
                        AddToHistory(account);
                    }
                    else {
                        foreach (ExternalLoginInfo item in accounts) {
                            List<String> itemIdentifiers = item.IdExternalString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (itemIdentifiers.Where(i => multipleIdentifiers.Contains(i)).Any())
                            {
                                account = item;
                                multipleIdentifiers.Where(mi => !itemIdentifiers.Contains(mi)).ToList().ForEach(i => account.IdExternalString += provider.MultipleItemsSeparator + i);
                                break;
                            }
                        }
                        if (account == null)
                            account = CreateAccount(person, currentUser, provider, externalString);
                        else
                            UpdateAccount(account, person, currentUser, provider, account.IdExternalString);
                        AddToHistory(account);
                        Manager.SaveOrUpdate(account);
                    }
                    if (person.IdDefaultProvider == 0 || String.IsNullOrEmpty(person.FirstLetter))
                    {
                        //// TEMPORANEO
                        //creator.Login = login;
                        //creator.Password=userInfo.Password;
                        //// TEMPORANEO
                        if (person.IdDefaultProvider == 0 && provider!=null)
                            person.IdDefaultProvider = provider.Id;
                        if (String.IsNullOrEmpty(person.FirstLetter))
                            person.FirstLetter = person.Surname[0].ToString().ToLower();
                        Manager.SaveOrUpdate(person);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {

                }
                return account;
            }

        //public ExternalLoginInfo AddFromInternalAccount(InternalLoginInfo internalAccount,ShibbolethAuthenticationProvider provider, String externalString )
        //{
        //    ExternalLoginInfo account = null;
        //    try {
        //        Manager.BeginTransaction();
        //        Person currentUser = Manager.GetPerson(UC.CurrentUserID);

        //        List <ExternalLoginInfo> accounts = (from si in Manager.GetIQ<ExternalLoginInfo>() where si.Person == internalAccount.Person && si.Provider == provider select si).ToList();
        //        List<String> userIdentifiers = externalString.Split(' ').ToList();

        //        foreach (String identifier in userIdentifiers.Where(i=> !string.IsNullOrEmpty(i)).ToList())
        //        {
        //            if (accounts.Where(a => a.IdExternalString.ToLower() == externalString.ToLower()).Any())
        //            {
        //                account = accounts.Where(a => a.IdExternalString.ToLower() == externalString.ToLower()).FirstOrDefault();
        //                if (account != null)
        //                {
        //                    account.UpdateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
        //                    account.isEnabled = true;
        //                    account.Deleted = BaseStatusDeleted.None;

        //                    Manager.SaveOrUpdate(account);
        //                }
        //                else {
        //                    account = new ExternalLoginInfo();
        //                    account.CreateMetaInfo((currentUser == null) ? internalAccount.Person : currentUser, UC.IpAddress, UC.ProxyIpAddress);
        //                    account.Person = internalAccount.Person;
        //                    account.Provider = provider;
        //                    account.ExternalIdentifier = provider.UniqueCode;
        //                    account.IdExternalString = externalString;
        //                    account.isEnabled = true;
        //                    account.Deleted = BaseStatusDeleted.None;

        //                    Manager.SaveOrUpdate(account);
        //                }
        //            }
        //        }
        //        Manager.Commit();
        //    }
        //    catch (Exception ex){
            
        //    }
        //    return account;
        //}

        //public ExternalLoginInfo AddUserInfo(Person person, ShibbolethAuthenticationProvider provider, String externalString)
        //{
        //    ExternalLoginInfo account = null;
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        Person currentUser = Manager.GetPerson(UC.CurrentUserID);
        //        List<String> userIdentifiers = externalString.Split(' ').ToList();
        //        var query = (from si in Manager.GetIQ<ExternalLoginInfo>() where si.Provider == provider select si);

        //        List<ExternalLoginInfo> accounts = new List<ExternalLoginInfo>();
        //        foreach (String identifier in userIdentifiers.Where(i=> !string.IsNullOrEmpty(i)).ToList())
        //        {
        //            accounts.AddRange(query.Where(si => si.Provider == provider && si.IdExternalString == identifier).ToList());
        //        }

        //        foreach (String identifier in userIdentifiers.Where(i => !string.IsNullOrEmpty(i)).ToList())
        //        {
        //            if (accounts.Where(a => a.IdExternalString.ToLower() == externalString.ToLower()).Any())
        //            {
        //                account = accounts.Where(a => a.IdExternalString.ToLower() == externalString.ToLower()).FirstOrDefault();
        //                if (account != null)
        //                {
        //                    account.UpdateMetaInfo((currentUser == null) ? person : currentUser, UC.IpAddress, UC.ProxyIpAddress);
        //                    account.isEnabled = !person.isDisabled;
        //                    account.Deleted = BaseStatusDeleted.None;

        //                    Manager.SaveOrUpdate(account);
        //                }
        //                else
        //                {
        //                    account = new ExternalLoginInfo();
        //                    account.CreateMetaInfo((currentUser == null) ? person : currentUser, UC.IpAddress, UC.ProxyIpAddress);
        //                    account.Person = person;
        //                    account.Provider = provider;
        //                    account.ExternalIdentifier = provider.UniqueCode;
        //                    account.IdExternalString = externalString;
        //                    account.isEnabled = !person.isDisabled;
        //                    account.Deleted = BaseStatusDeleted.None;

        //                    Manager.SaveOrUpdate(account);
        //                }
        //            }
        //        }
        //        Manager.Commit();
        //        //account = accounts.Distinct().FirstOrDefault();
        //        //if (account == null)
        //        //{
        //        //    account = new ExternalLoginInfo();
        //        //    account.CreateMetaInfo((currentUser == null) ? person : currentUser, UC.IpAddress, UC.ProxyIpAddress);
        //        //    account.Person = person;
        //        //    account.Provider = provider;
        //        //    account.ExternalIdentifier = provider.UniqueCode;
        //        //    account.IdExternalString = externalString;
        //        //}
        //        //else
        //        //{
        //        //    foreach (String identifier in userIdentifiers)
        //        //    {
        //        //        if (!account.IdExternalString.Contains(identifier))
        //        //            account.IdExternalString += " " + identifier;
        //        //    }
        //        //    account.UpdateMetaInfo((currentUser == null) ? person : currentUser, UC.IpAddress, UC.ProxyIpAddress);
                   
        //        //}
        //        //account.isEnabled = !person.isDisabled;
        //        //account.Deleted = BaseStatusDeleted.None;
               
        //        //Manager.SaveOrUpdate(account);
        //        //Manager.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //    }
        //    return account;
        //}

        public override List<ExternalLoginInfo> FindUserByIdentifier(String identifier, AuthenticationProvider provider)
        {
            //List<ExternalLoginInfo> list = (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString==identifier && i.Provider == provider && i.Person != null select i).ToList();
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
                    multipleIdentifiers.ForEach(id => list.AddRange(items.Where(i => i.IdExternalString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).Contains(id)).ToList()));
                }
            }
            else
                list = (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString == identifier && i.Provider == provider && i.Person != null select i).ToList();
            return list;
        }
        public List<ShibbolethAuthenticationProvider> GetActivePoviders()
        {
            return (from p in this.Manager.GetIQ<ShibbolethAuthenticationProvider>() where p.Deleted == BaseStatusDeleted.None select p).ToList();
        }
    }
}