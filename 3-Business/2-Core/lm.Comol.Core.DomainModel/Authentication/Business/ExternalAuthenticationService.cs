using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.Authentication.Business
{
    public class ExternalAuthenticationService : CoreAuthenticationsService
    {

        #region initClass
            public ExternalAuthenticationService() :base() { }
            public ExternalAuthenticationService(iApplicationContext oContext) :base(oContext) {}
            public ExternalAuthenticationService(iDataContext oDC) : base(oDC) { }
        #endregion



        public ExternalLoginInfo AddExternalProfile(Person person, AuthenticationProvider provider, dtoExternalCredentials credentials)
            {
                ExternalLoginInfo account = null;
                try
                {
                    Manager.BeginTransaction();
                    Person currentUser = Manager.GetPerson(UC.CurrentUserID);

                    List<ExternalLoginInfo> accounts = (from si in Manager.GetIQ<ExternalLoginInfo>() where si.Provider == provider && si.Person == person select si).ToList();

                    if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)provider.IdentifierFields, (long)IdentifierField.longField) && lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)provider.IdentifierFields, (long)IdentifierField.stringField))
                        account = accounts.Where(a => a.IdExternalLong == credentials.IdentifierLong && a.IdExternalString == credentials.IdentifierString).FirstOrDefault();
                    else if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)provider.IdentifierFields, (long)IdentifierField.longField))
                        account = accounts.Where(a => a.IdExternalLong == credentials.IdentifierLong).FirstOrDefault();
                    else{
                        List<String> multipleIdentifiers = null;
                        if (provider.MultipleItemsForRecord && !string.IsNullOrEmpty(provider.MultipleItemsSeparator))
                            multipleIdentifiers = credentials.IdentifierString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (multipleIdentifiers == null || multipleIdentifiers.Count == 0)
                            account = accounts.Where(a => a.IdExternalString.ToLower() == credentials.IdentifierString.ToLower()).FirstOrDefault();
                        else {
                            foreach (ExternalLoginInfo item in accounts) {
                                List<String> itemIdentifiers = item.IdExternalString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                                if (itemIdentifiers.Where(i => multipleIdentifiers.Contains(i)).Any())
                                {
                                    String tmp = credentials.IdentifierString;
                                    credentials.IdentifierString = item.IdExternalString;
                                    account = item;
                                    multipleIdentifiers.Where(mi => !itemIdentifiers.Contains(mi)).ToList().ForEach(i => credentials.IdentifierString += provider.MultipleItemsSeparator + i);
                                    break;
                                }
                            }
                        }
                    }
                    

                    if (account == null)
                        account = CreateAccount(person, currentUser, provider, credentials);
                    else
                        UpdateAccount(account, person, currentUser, provider, credentials);
                    Manager.SaveOrUpdate(account);
                    AddToHistory(account);
                    if (person.IdDefaultProvider == 0 || String.IsNullOrEmpty(person.FirstLetter))
                    {
                        if (person.IdDefaultProvider == 0)
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

         protected ExternalLoginInfo CreateAccount(Person person, Person currentUser, AuthenticationProvider provider, string externalIdentifier) {
             return CreateAccount(person, currentUser, provider, new dtoExternalCredentials() { IdentifierString = externalIdentifier });
         }
        protected ExternalLoginInfo CreateAccount(Person person, Person currentUser ,AuthenticationProvider provider, dtoExternalCredentials credentials)
        {
                ExternalLoginInfo account = new ExternalLoginInfo();
                account.CreateMetaInfo((currentUser == null) ? person : currentUser, UC.IpAddress, UC.ProxyIpAddress);
                account.Person = person;
                account.Provider = provider;
                account.ExternalIdentifier = provider.UniqueCode;

                if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)provider.IdentifierFields, (long)IdentifierField.longField))
                    account.IdExternalLong = credentials.IdentifierLong;
                if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)provider.IdentifierFields, (long)IdentifierField.stringField))
                    account.IdExternalString = credentials.IdentifierString;
                account.isEnabled = !person.isDisabled;
                account.Deleted = BaseStatusDeleted.None;
                return account;
        }

        protected void UpdateAccount(ExternalLoginInfo account, Person person, Person currentUser, AuthenticationProvider provider, string externalIdentifier)
        {
            UpdateAccount(account,person, currentUser, provider, new dtoExternalCredentials() { IdentifierString = externalIdentifier });
        }
        protected void UpdateAccount(ExternalLoginInfo account, Person person, Person currentUser, AuthenticationProvider provider, dtoExternalCredentials credentials)
        {
            account.UpdateMetaInfo((currentUser == null) ? person : currentUser, UC.IpAddress, UC.ProxyIpAddress);

            if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)provider.IdentifierFields, (long)IdentifierField.longField))
                account.IdExternalLong = credentials.IdentifierLong;
            if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)provider.IdentifierFields, (long)IdentifierField.stringField))
                account.IdExternalString = credentials.IdentifierString;
            account.isEnabled = !person.isDisabled;
            account.Deleted = BaseStatusDeleted.None;
        }
        //public virtual List<ExternalLoginInfo> FindUserByIdentifier(String identifier)
        //{
        //    return (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString == identifier select i).ToList();
        //}
        public virtual  List<ExternalLoginInfo> FindUserByIdentifier(String identifier, AuthenticationProvider provider)
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
                    multipleIdentifiers.ForEach(id => list.AddRange(items.Where(i => i.IdExternalString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).Contains(id)).ToList()));
                }
            }
            else
                list = (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString == identifier && i.Provider == provider && i.Person != null select i).ToList();
            return list;
          //  return (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString == identifier && i.Provider == provider select i).ToList();

        }

        public ExternalCommunityInfo FindExternalCommunityByIdentifier(String idCommunity, String externalIdentifier)
        {
            return (from ec in Manager.GetIQ<ExternalCommunityInfo>()
                    where ec.Deleted == BaseStatusDeleted.None && ec.ExternalIdentifier == externalIdentifier && ec.IdExternalString == idCommunity
                    select ec).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public ExternalCommunityInfo FindExternalCommunityByIdentifier(long idCommunity, String externalIdentifier)
        {
            return (from ec in Manager.GetIQ<ExternalCommunityInfo>()
                    where ec.Deleted == BaseStatusDeleted.None && ec.ExternalIdentifier == externalIdentifier && ec.IdExternalLong == idCommunity
                    select ec).Skip(0).Take(1).ToList().FirstOrDefault();
        }


        public Boolean isUniqueLogin(String login)
        {
            return !(from p in Manager.GetIQ<InternalLoginInfo>() where p.Login == login && p.Deleted== BaseStatusDeleted.None select p.Id).Any();
        }
        public Boolean isUniqueMail(String mail)
        {
            return !(from p in Manager.GetIQ<Person>() where p.Mail == mail select p.Id).Any();
        }
        public Boolean InternalProfileExist(String login, String mail, String taxCode)
        {
            Boolean found = false;
            if (!string.IsNullOrEmpty(login))
                found = !isUniqueLogin(login);
            if (!found){
                found = (from p in Manager.GetIQ<Person>() where (!String.IsNullOrEmpty(mail) && p.Mail == mail) || (!String.IsNullOrEmpty(taxCode) && p.TaxCode == taxCode) select p.Id).Any();
            }
            return found;
        }
        public String GetNewLogin(String name, string surname)
        {
            int number = 1;
            String result = name + "." + surname;
            while (!isUniqueLogin(result))
            {
                result = name + "." + surname + "_" + number.ToString();
                number++;
            }
            return result;
        }

        public void AddToHistory(ExternalLoginInfo item)
        {
            Boolean isInTransaction = Manager.IsInTransaction();
            try
            {
                if (!isInTransaction)
                    Manager.BeginTransaction();
                ExternalLoginInfoHistory hItem = ExternalLoginInfoHistory.NewHistoryItem(item, Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);

                Manager.SaveOrUpdate(hItem);
                if (!isInTransaction)
                    Manager.Commit();
            }
            catch (Exception ex)
            {
                if (!isInTransaction && Manager.IsInTransaction())
                    Manager.RollBack();

            }
        }
    }
}