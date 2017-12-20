using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class MailRecipient
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual MessageTranslation Item { get; set; } 
        public virtual MailMessage Message { get; set; } 
        public virtual String MailAddress { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.RecipientType Type { get; set; }

        public virtual Int32 IdPerson { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual long IdUserModule { get; set; }
        public virtual long IdModuleObject { get; set; }
        public virtual Int32 IdModuleType { get; set; }
        public virtual Boolean IsFromModule { get { return (Ownership!=null && !String.IsNullOrEmpty(Ownership.ModuleCode)); } }
        public virtual Boolean IsInternal  { get { return IdPerson>0;} }
        public virtual Boolean IsMailSent { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual Ownership Ownership { get; set; }

        public override String ToString() {
            return String.IsNullOrEmpty(DisplayName) ? MailAddress : DisplayName + "<" + MailAddress + ">";
        }
    }
}
