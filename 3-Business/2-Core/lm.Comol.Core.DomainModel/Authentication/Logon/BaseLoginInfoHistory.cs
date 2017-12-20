using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class BaseLoginInfoHistory : DomainBaseObject<long>
    {
        public virtual Int32 IdPerson { get; set; }
        public virtual String  Displayname { get; set; }
        public virtual CreatorInfo CreatorInfo { get; set; }
        public virtual long IdProvider { get; set; }
        public virtual Boolean isEnabled { get; set; }
        public BaseLoginInfoHistory()
        {
            Deleted = DomainModel.BaseStatusDeleted.None;
            CreatorInfo = new CreatorInfo();
        }

        public static BaseLoginInfoHistory NewHistoryItem(Person person, String ipAddress, String proxyIpAddress)
        {
            BaseLoginInfoHistory r = new BaseLoginInfoHistory();
            r.CreatorInfo = CreatorInfo.Create(person, ipAddress, proxyIpAddress);
            
            return r;
        }
        public static BaseLoginInfoHistory NewHistoryItem(BaseLoginInfo item, Person person, String address, String proxy)
        {
            BaseLoginInfoHistory r = new BaseLoginInfoHistory();
            r.CreatorInfo = CreatorInfo.Create(item,person, address, proxy);

            return r;
        }
    }
    [Serializable]
    public class CreatorInfo 
    {
        public virtual Int32 IdCreator { get; set; }
        public virtual String Displayname { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual String IpAddress { get; set; }
        public virtual String ProxyIpAddress { get; set; }

        public static CreatorInfo Create(Person person, String address, String proxy)
        {
            CreatorInfo info = new CreatorInfo();
            info.IdCreator = (person == null) ? 0 : person.Id;
            info.Displayname = (person == null) ? "--" : person.SurnameAndName;
            info.CreatedOn = DateTime.Now;
            info.IpAddress = address;
            info.ProxyIpAddress = proxy;
            return info;
        }
        public static CreatorInfo Create(BaseLoginInfo item, Person person, String address, String proxy)
        {
            CreatorInfo info = new CreatorInfo();
            info.IdCreator = (person != null) ? person.Id : 0;
            info.Displayname = (person != null) ? person.SurnameAndName : "--";
            info.CreatedOn =  DateTime.Now;
            info.IpAddress = address;
            info.ProxyIpAddress = proxy;
            return info;
        }
    }

    [Serializable]
    public class ExternalLoginInfoHistory : BaseLoginInfoHistory
    {
        public virtual long IdExternalLong { get; set; }
        public virtual String IdExternalString { get; set; }
        public virtual String ExternalIdentifier { get; set; }
        public virtual Boolean isEnabled { get; set; }

        public static ExternalLoginInfoHistory NewHistoryItem(ExternalLoginInfo item, Person person, String ipAddress, String proxyIpAddress)
        {
            ExternalLoginInfoHistory r = new ExternalLoginInfoHistory();
            r.CreatorInfo = CreatorInfo.Create(item,person, ipAddress, proxyIpAddress);
            r.Displayname = (item.Person != null) ? item.Person.SurnameAndName : "";
            r.IdPerson = (item.Person != null) ? item.Person.Id : 0;
            r.IdProvider = (item.Provider != null) ? item.Provider.Id : 0;
            r.IdExternalLong = item.IdExternalLong;
            r.IdExternalString = item.IdExternalString;
            r.ExternalIdentifier = item.ExternalIdentifier;
            r.isEnabled = item.isEnabled;
            r.Deleted = item.Deleted;
            return r;
        }
    }
    [Serializable]
    public class InternalLoginInfoHistory : BaseLoginInfoHistory
    {
        public virtual String Login { get; set; }
        public virtual String Password { get; set; }
        public virtual DateTime? PasswordExpiresOn { get; set; }
        public virtual EditType ResetType { get; set; }

        public static InternalLoginInfoHistory NewHistoryItem(InternalLoginInfo item, Person person, String ipAddress, String proxyIpAddress)
        {
            InternalLoginInfoHistory r = new InternalLoginInfoHistory();
            r.CreatorInfo = CreatorInfo.Create(item,person, ipAddress, proxyIpAddress);
            r.Displayname = (item.Person != null) ? item.Person.SurnameAndName : "";
            r.IdPerson = (item.Person != null) ? item.Person.Id : 0;
            r.IdProvider = (item.Provider != null) ? item.Provider.Id : 0;
            r.Login = item.Login;
            r.Password = item.Password;
            r.ResetType= item.ResetType;
            r.PasswordExpiresOn= item.PasswordExpiresOn;
            r.isEnabled = item.isEnabled;
            r.Deleted = item.Deleted;
            return r;
        }
    }
}