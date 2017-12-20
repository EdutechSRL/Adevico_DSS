using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.Authentication.Business
{
    public class InternalAuthenticationService : CoreAuthenticationsService
    {

        #region initClass
            public InternalAuthenticationService() :base() { }
            public InternalAuthenticationService(iApplicationContext oContext) :base(oContext) {}
            public InternalAuthenticationService(iDataContext oDC) : base(oDC) { }
        #endregion

            public InternalAuthenticationProvider GetActiveProvider()
            {
                InternalAuthenticationProvider provider = null;
                try
                {
                    provider = (from i in this.Manager.GetIQ<InternalAuthenticationProvider>()
                              where i.Deleted == BaseStatusDeleted.None
                              select i).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {
                    provider = null;
                }
                return provider;
            }


        public Boolean Authenticate(String login, String password) {
            Boolean result = false;
            try
            {
                Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
                result = (from i in this.Manager.GetIQ<InternalLoginInfo>()
                          where i.Login == login && i.Password == helper.Encrypt(password) && i.Deleted== BaseStatusDeleted.None 
                          select i.Id).Any();
            }
            catch (Exception ex) { 
            
            }
            return result;
        }
        public InternalLoginInfo GetAuthenticatedUser(String login, String password)
        {
            InternalLoginInfo result = null;
            try
            {
                Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
                result = (from i in this.Manager.GetIQ<InternalLoginInfo>()
                          where i.Login == login && i.Password == helper.Encrypt(password) && i.Deleted == BaseStatusDeleted.None
                          select i).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

       

        //public Boolean EditPassword(String login, String oldPassword, String newPassword)
        //{
        //    Boolean result = false;
        //    try
        //    {
        //        Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
        //        InternalLoginInfo info  = (from i in this.Manager.GetIQ<InternalLoginInfo>()
        //                  where i.Login == login && i.Password == helper.Encrypt(oldPassword)
        //                  select i).Skip(0).Take(1).ToList().FirstOrDefault();
        //        if (info != null) {
        //            Manager.BeginTransaction();
        //            info.Password = helper.Encrypt(newPassword);
        //            info.UpdateMetaInfo(Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
        //            Manager.SaveOrUpdate(info);
        //            result = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //    }
        //    return result;
        //}
        public Boolean EditPassword(Person person, String oldPassword, String newPassword, Boolean mustEdit)
        {
            Boolean edited = false;
            try
            {
                InternalAuthenticationProvider provider = (from p in Manager.GetIQ<InternalAuthenticationProvider>()
                                                           where p.IsEnabled && p.Deleted== BaseStatusDeleted.None
                                                           select p).Skip(0).Take(1).ToList().FirstOrDefault();

                InternalLoginInfo info = (from i in this.Manager.GetIQ<InternalLoginInfo>()
                                          where i.Person == person && i.Deleted== BaseStatusDeleted.None 
                                          select i).Skip(0).Take(1).ToList().FirstOrDefault();

                Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
                if (info == null)
                    throw new UnknownItemException();
                else if (info.Password != helper.Encrypt(oldPassword))
                    throw new InvalidPasswordException();
                else if (mustEdit && info.Password == helper.Encrypt(newPassword))
                    throw new SamePasswordException();
                else
                {
                    Manager.BeginTransaction();
                    info.UpdateMetaInfo(Manager.GetPerson((mustEdit) ? person.Id : UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                    info.Password = helper.Encrypt(newPassword);
                    info.ResetType = (info.ModifiedBy == person || mustEdit) ? EditType.user : EditType.admin;
                    if (provider != null && provider.ChangePasswordAfterDays > 0)
                        info.PasswordExpiresOn = DateTime.Now.AddDays(provider.ChangePasswordAfterDays);
                    Manager.Commit();
                    AddToHistory(info);
                    edited = true;
                }
            }

            catch (SamePasswordException uEx)
            {
                throw uEx;
            }
            catch (UnknownItemException uEx) {
                throw uEx;
            }
            catch (InvalidPasswordException pEx)
            {
                throw pEx;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                newPassword = "";
            }
            return edited;
        }
        public InternalLoginInfo SetPassword(Person person, String newPassword, Boolean isOneTimePassword)
        {
            InternalLoginInfo userInfo = null;
            try
            {
                InternalAuthenticationProvider provider = (from p in Manager.GetIQ<InternalAuthenticationProvider>()
                                                           where p.IsEnabled && p.Deleted == BaseStatusDeleted.None
                                                           select p).Skip(0).Take(1).ToList().FirstOrDefault();

                userInfo = (from i in this.Manager.GetIQ<InternalLoginInfo>()
                                          where i.Person == person && i.Deleted == BaseStatusDeleted.None
                                          select i).Skip(0).Take(1).ToList().FirstOrDefault();
                Person setUser = Manager.GetPerson(UC.CurrentUserID);
                Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
                if (userInfo == null || setUser == null)
                    throw new UnknownItemException();
                else
                {
                    Manager.BeginTransaction();
                    userInfo.UpdateMetaInfo(setUser, UC.IpAddress, UC.ProxyIpAddress);
                    userInfo.Password = helper.Encrypt(newPassword);
                    userInfo.ResetType = (isOneTimePassword) ? EditType.oneTime : EditType.admin;
                    if (provider != null && provider.ChangePasswordAfterDays > 0)
                        userInfo.PasswordExpiresOn = DateTime.Now.AddDays(provider.ChangePasswordAfterDays);
                    AddToHistory(userInfo);
                    Manager.Commit();
                }
            }
            catch (UnknownItemException uEx)
            {
                userInfo = null;
                throw uEx;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                userInfo = null;
            }
            return userInfo;
        }
        public Boolean ExpiredPassword(Person person)
        {
            Boolean expired = false;
            try
            {
                InternalLoginInfo info = (from i in this.Manager.GetIQ<InternalLoginInfo>()
                                          where i.Person == person && i.Deleted== BaseStatusDeleted.None 
                                          select i).Skip(0).Take(1).ToList().FirstOrDefault();
                if (info != null)
                    expired = ((info.ResetType == EditType.retrieve || info.ResetType == EditType.oneTime || info.ResetType == EditType.reset) || (info.PasswordExpiresOn.HasValue && info.PasswordExpiresOn.Value < DateTime.Now));
            }
            catch (Exception ex)
            {

            }
            return expired;
        }
        public InternalLoginInfo GetLoginInfo(Person person)
        {
            InternalLoginInfo info = null;
            try
            {
                info = (from i in this.Manager.GetIQ<InternalLoginInfo>()
                                          where i.Person == person && i.Deleted == BaseStatusDeleted.None
                                          select i).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return info;
        }
        //public String RenewPassword(String login)
        //{
        //    String newPassword = "";
        //    try
        //    {
        //        InternalLoginInfo info  = (from i in this.Manager.GetIQ<InternalLoginInfo>()
        //                  where i.Login == login
        //                  select i).Skip(0).Take(1).ToList().FirstOrDefault();
        //        newPassword = RenewPassword(info);
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        newPassword = "";
        //    }
        //    return newPassword;
        //}
        public String RenewPassword(InternalLoginInfo userInfo,Person modifyBy, EditType editBy)
        {
            String newPassword = "";
            try
            {
                Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
                if (userInfo != null)
                {
                    Manager.BeginTransaction();
                    Person currentUser = Manager.GetPerson(UC.CurrentUserID);
                    if (currentUser == null && UC.CurrentUserID ==0)
                        currentUser = userInfo.Person;
                    newPassword = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, true, true, false);
                    userInfo.Password = helper.Encrypt(newPassword);
                    userInfo.UpdateMetaInfo(modifyBy, UC.IpAddress, UC.ProxyIpAddress);
                    userInfo.ResetType = editBy;
                    Manager.SaveOrUpdate(userInfo);
                    AddToHistory(userInfo);
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                newPassword = "";
            }
            return newPassword;
        }


        public InternalLoginInfo RenewPassword(Person person, EditType editBy, ref String newPassword)
        {
            InternalLoginInfo loginInfo = null;
            try
            {
                Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
                if (person != null)
                {
                    Manager.BeginTransaction();
                    Person currentUser = Manager.GetPerson(UC.CurrentUserID);
                    loginInfo = (from li in Manager.GetIQ<InternalLoginInfo>() where li.Person == person select li).Skip(0).Take(1).ToList().FirstOrDefault();

                    if (loginInfo != null)
                    {
                        newPassword = lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, true, true, false);
                        loginInfo.Password = helper.Encrypt(newPassword);
                        loginInfo.UpdateMetaInfo(Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                        loginInfo.ResetType = editBy;
                        Manager.SaveOrUpdate(loginInfo);
                        AddToHistory(loginInfo);
                    }
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                loginInfo = null;
            }
            return loginInfo;
        }
        public InternalLoginInfo AddUserInfo(String login, String password, int idPerson)
        {
            InternalAuthenticationProvider provider = (from p in Manager.GetIQ<InternalAuthenticationProvider>() where p.Deleted == BaseStatusDeleted.None select p).Skip(0).Take(1).ToList().FirstOrDefault();
            return AddUserInfo(login, password, idPerson, (provider == null || provider.ChangePasswordAfterDays==0) ? DateTime.MaxValue : DateTime.Now.AddDays(provider.ChangePasswordAfterDays), provider);
        }
        public InternalLoginInfo AddUserInfo(String login, String password, int idPerson, DateTime passwordExpiresOn, InternalAuthenticationProvider provider)
        {
            InternalLoginInfo userInfo = null;
            try
            {
                Person currentUser = Manager.GetPerson(UC.CurrentUserID);
                Person creator = Manager.GetPerson(idPerson);
                if (currentUser == null)
                    currentUser = creator;
                if (provider != null && creator!=null){
                    Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
                    Manager.BeginTransaction();
                    userInfo = new InternalLoginInfo();
                    userInfo.CreateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
                    userInfo.PasswordExpiresOn = passwordExpiresOn;
                    userInfo.Deleted = BaseStatusDeleted.None;
                    userInfo.isEnabled = true;
                    userInfo.Provider = provider;
                    userInfo.Login = login;
                    userInfo.Password = helper.Encrypt(password);
                    userInfo.Person = creator;
                    if (creator.IdDefaultProvider == 0 || String.IsNullOrEmpty(creator.FirstLetter) )
                    {
                        //// TEMPORANEO
                        //creator.Login = login;
                        //creator.Password=userInfo.Password;
                        //// TEMPORANEO
                        if (creator.IdDefaultProvider == 0)
                            creator.IdDefaultProvider = provider.Id;
                        if (String.IsNullOrEmpty(creator.FirstLetter))
                            creator.FirstLetter = creator.Surname[0].ToString().ToLower();
                        Manager.SaveOrUpdate(creator);
                    }
                    Manager.SaveOrUpdate(userInfo);
                    AddToHistory(userInfo);
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                userInfo = null;
            }
            return userInfo;
        }
        public InternalLoginInfo GenerateUserInfo(Person person,String login, String password, InternalAuthenticationProvider provider,Boolean oneTimePassword)
        {
            InternalLoginInfo userInfo = null;
            try
            {
                Person currentUser = Manager.GetPerson(UC.CurrentUserID);
                if (currentUser == null)
                    currentUser = person;
                if (provider != null && person != null)
                {
                    Helpers.InternalEncryptor helper = new Helpers.InternalEncryptor();
                    Manager.BeginTransaction();
                    if (!(from l in Manager.GetIQ<InternalLoginInfo>()
                          where l.Person == person && l.Deleted == BaseStatusDeleted.None && l.Provider == provider
                          select l.Id).Any())
                    {
                        userInfo = new InternalLoginInfo();
                        userInfo.CreateMetaInfo(currentUser, UC.IpAddress, UC.ProxyIpAddress);
                        userInfo.PasswordExpiresOn = (provider == null || provider.ChangePasswordAfterDays == 0) ? DateTime.MaxValue : DateTime.Now.AddDays(provider.ChangePasswordAfterDays);
                        userInfo.Deleted = BaseStatusDeleted.None;
                        userInfo.isEnabled = true;
                        userInfo.Provider = provider;
                        userInfo.Login = login;
                        userInfo.Password = helper.Encrypt(password);
                        userInfo.Person = person;
                        userInfo.ResetType = (oneTimePassword) ? EditType.oneTime : EditType.admin;
                        if (person.IdDefaultProvider == 0 || String.IsNullOrEmpty(person.FirstLetter))
                        {
                            if (person.IdDefaultProvider == 0)
                                person.IdDefaultProvider = provider.Id;
                            if (String.IsNullOrEmpty(person.FirstLetter))
                                person.FirstLetter = person.Surname[0].ToString().ToLower();
                            Manager.SaveOrUpdate(person);
                        }

                        Manager.SaveOrUpdate(userInfo);
                        AddToHistory(userInfo);
                    }
                    else
                        userInfo = (from l in Manager.GetIQ<InternalLoginInfo>()
                                    where l.Person == person && l.Deleted == BaseStatusDeleted.None && l.Provider == provider
                                    select l).Skip(0).Take(1).ToList().FirstOrDefault();
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                userInfo = null;
            }
            return userInfo;
        }

        public InternalLoginInfo FindUser(String login)
        {
            return (from i in this.Manager.GetIQ<InternalLoginInfo>() where i.Login == login select i).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public InternalLoginInfo FindUserByMail(String mail)
        {
            Person person = (from p in Manager.GetIQ<Person>() where p.Mail == mail select p).Skip(0).Take(1).ToList().FirstOrDefault();
            return (from i in this.Manager.GetIQ<InternalLoginInfo>() where person!= null && i.Person != null && i.Person == person select i).Skip(0).Take(1).ToList().FirstOrDefault();
        }
        public List<InternalLoginInfo> FindUsersByLogin(String login)
        {
            return (from i in this.Manager.GetIQ<InternalLoginInfo>() where i.Login == login select i).ToList();
        }
        public Boolean InternalProfileExist(String login, String mail, String taxCode){
            return (PersonExist(mail, taxCode) || (from i in this.Manager.GetIQ<InternalLoginInfo>() where i.Login == login select i.Id).Any());
        }

        public String GenerateInternalLogin(String name, String surname)
        {
            int number = 1;
            if (!String.IsNullOrEmpty(name))
                name= name.ToLower();
            if (!String.IsNullOrEmpty(surname))
                surname = surname.ToLower();
            if (surname.Contains(" "))
                surname = surname.Replace(" ", "");
            if (name.Contains(" "))
                name = name.Replace(" ", "");
            String result = name.ToLower() + "." + surname.ToLower();
            while (!isInternalUniqueLogin(result))
            {
                result = name + "." + surname + "_" + number.ToString();
                number++;
            }
            return result;
        }
        public String GeneratePassword() {
            return lm.Comol.Core.DomainModel.Helpers.RandomKeyGenerator.GenerateRandomKey(6, 10, true, true, false);
        }
        public override List<ProfilerError> VerifyProfileInfo(dtoBaseProfile profile)
        {
            List<ProfilerError> result = base.VerifyProfileInfo(profile);
            if (profile.AuthenticationProvider == AuthenticationProviderType.Internal && !isInternalUniqueLogin(profile.Login))
                result.Add(ProfilerError.loginduplicate);
            return result;
        }
        public ProfilerError UpdateInternalProfile(InternalLoginInfo loginInfo, Person person, String login)
        {
            ProfilerError message = VerifyProfileInfo(person, login);
            if (message == ProfilerError.none)
            {
                try
                {
                    Manager.BeginTransaction();
                    loginInfo.UpdateMetaInfo(Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
                    loginInfo.Deleted = BaseStatusDeleted.None;
                    loginInfo.Login = login;
                    Manager.SaveOrUpdate(loginInfo);
                    AddToHistory(loginInfo);
                    Manager.Commit();
                    message = ProfilerError.none;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    message = ProfilerError.internalError;
                }
            }
            return message;
        }
        public void AddToHistory(InternalLoginInfo item) {
            Boolean isInTransaction = Manager.IsInTransaction();
            try
            {
                if (!isInTransaction)
                    Manager.BeginTransaction();

                InternalLoginInfoHistory hItem = InternalLoginInfoHistory.NewHistoryItem(item,Manager.GetPerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);

                Manager.SaveOrUpdate(hItem);
                if (!isInTransaction)
                    Manager.Commit();
            }
            catch (Exception ex) {
                if (!isInTransaction && Manager.IsInTransaction())
                    Manager.RollBack();

            }
        }
    }
}