using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{

    [Serializable]
    public class dtoIstantMessagingSettings
    {
        public virtual IstantMessagingType Type { get; set; }
        public virtual List<dtoIstantMessaging> Items { get; set; }

        public dtoIstantMessagingSettings(){
            Items = new List<dtoIstantMessaging>();
        }
        public virtual dtoIstantMessaging GetSetting(long id){
            return Items.Where(i=> i.Id ==id).FirstOrDefault();
        }
        public virtual void RemoveSetting(long id)
        {
            Items = Items.Where(i => i.Id != id).ToList();
        }
    }

    [Serializable]
    public class dtoIstantMessaging
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdPerson { get; set; }
        public virtual String Address { get; set; }
        public virtual String Note { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual IstantMessagingType Type { get; set; }
        public virtual IstantMessagingVisibility Visibility { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public virtual Boolean IsValid { get { return !String.IsNullOrEmpty(Address) && !String.IsNullOrEmpty((Address.Trim())); } }
        public dtoIstantMessaging(){}

        public dtoIstantMessaging(liteIstantMessaging item){
            Id = item.Id;
            IdPerson = item.IdPerson;
            Address = item.Address;
            Note = item.Note;
            IsDefault = item.IsDefault;
            Type = item.Type;
            Visibility = item.Visibility;
            DisplayOrder = item.DisplayOrder;
        }
        public dtoIstantMessaging(IstantMessaging item)
        {
            Id = item.Id;
            IdPerson = (item.Person == null) ? 0 : item.Person.Id;
            Address = item.Address;
            Note = item.Note;
            IsDefault = item.IsDefault;
            Type = item.Type;
            Visibility = item.Visibility;
            DisplayOrder = item.DisplayOrder;
        }
        
    }
}