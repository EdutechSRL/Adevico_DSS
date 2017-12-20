using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.Authentication.Business
{
    public class UrlAuthenticationService : ExternalAuthenticationService
    {

        #region initClass
            public UrlAuthenticationService() :base() { }
            public UrlAuthenticationService(iApplicationContext oContext) :base(oContext) {}
            public UrlAuthenticationService(iDataContext oDC) : base(oDC) { }
        #endregion

        public ExternalLoginInfo AddFromInternalAccount(InternalLoginInfo internalAccount,UrlAuthenticationProvider provider, String externalString )
        {
            return AddUserInfo(internalAccount.Person, internalAccount.Person, provider, externalString);
        }
        public ExternalLoginInfo AddUserInfo(Person person, UrlAuthenticationProvider provider, String externalString)
        {
            Person currentUser = Manager.GetPerson(UC.CurrentUserID);
            return AddUserInfo(person, (currentUser == null) ? person : currentUser, provider, externalString);
        }
        private ExternalLoginInfo AddUserInfo(Person person,Person currentUser, UrlAuthenticationProvider provider, String externalString)
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
            if (provider.MultipleItemsForRecord && !string.IsNullOrEmpty(provider.MultipleItemsSeparator)){
                List<String> multipleIdentifiers = null;
                multipleIdentifiers = identifier.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                if (multipleIdentifiers == null || multipleIdentifiers.Count == 0)
                    list = (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString == identifier && i.Provider == provider && i.Person != null select i).ToList();
                else { 
                    list= new List<ExternalLoginInfo>(); 
                    List<ExternalLoginInfo> items = new List<ExternalLoginInfo>();
                    foreach (String idn in multipleIdentifiers){
                        items.AddRange((from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString.Contains(idn) && i.Provider == provider && i.Person != null select i).ToList());
                    }
                    multipleIdentifiers.ForEach(id => list.AddRange(items.Where(i=> i.IdExternalString.Split(provider.MultipleItemsSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries).Contains(id)).ToList()));
                }
            }
            else
                list = (from i in this.Manager.GetIQ<ExternalLoginInfo>() where i.IdExternalString == identifier && i.Provider == provider && i.Person != null select i).ToList();
            return list;
        }
        public List<UrlAuthenticationProvider> GetActivePoviders()
        {
            return (from p in this.Manager.GetIQ<UrlAuthenticationProvider>() where p.Deleted == BaseStatusDeleted.None select p).ToList();
        }
        public List<UrlAuthenticationProvider> GetActivePoviders(String urlIdentifier)
        {
            return (from p in this.Manager.GetIQ<UrlAuthenticationProvider>() where p.Deleted == BaseStatusDeleted.None && p.UrlIdentifier == urlIdentifier select p).ToList();
        }
        public UrlAuthenticationProvider GetActivePovider(String urlIdentifier)
        {
            return (from p in this.Manager.GetIQ<UrlAuthenticationProvider>()
                    where p.Deleted == BaseStatusDeleted.None && p.UrlIdentifier == urlIdentifier
                    select p).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public UrlAuthenticationProvider GetPovider(String urlIdentifier)
        {
            return (from p in this.Manager.GetIQ<UrlAuthenticationProvider>()
                    where p.UrlIdentifier == urlIdentifier
                    select p).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public List<String> GetActiveUrlIdentifiers()
        {
            return (from p in this.Manager.GetIQ<UrlAuthenticationProvider>()
                    where p.Deleted == BaseStatusDeleted.None
                    select p.UrlIdentifier).ToList();
        }
    }
}