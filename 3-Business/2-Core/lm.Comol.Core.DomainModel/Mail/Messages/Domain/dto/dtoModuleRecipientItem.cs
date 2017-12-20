using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoModuleRecipientItem
    {
        public virtual Int32 IdPerson { get; set; }
        public virtual long IdUserModule { get; set; }
        public virtual Boolean IsInternal { get { return IdPerson > 0; } }
        public virtual long IdModuleObject { get; set; }
        public virtual String MailAddress { get; set; }
        public dtoModuleRecipientItem()
        {
        }

        public Boolean IsEqualsTo(dtoBaseMessageRecipient other)
        {
            return (other != null && ((IdPerson>0) || (IdPerson<1 && other.MailAddress== MailAddress))  && other.IdPerson.Equals(IdPerson) && other.IdUserModule.Equals(IdUserModule) && other.IdModuleObject.Equals(IdModuleObject));
        }
    }
}