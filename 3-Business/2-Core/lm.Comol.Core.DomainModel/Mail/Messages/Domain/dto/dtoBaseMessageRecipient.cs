using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoBaseMessageRecipient :dtoRecipient
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String CodeLanguage { get; set; }
        public virtual lm.Comol.Core.MailCommons.Domain.RecipientType Type { get; set; }

        public virtual Int32 IdPerson { get; set; }
        public virtual Int32 IdProfileType { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual long IdUserModule { get; set; }       
        public virtual Boolean IsFromModule { get { return !String.IsNullOrEmpty(ModuleCode); } }
        public virtual Boolean IsInternal  { get { return IdPerson>0;} }
        public virtual long IdModuleObject { get; set; }
        public virtual Int32 IdModuleType { get; set; }
        public virtual String FirstLetter { get { return (!String.IsNullOrEmpty(DisplayName)) ? DisplayName.Substring(0, 1).ToLower() :   (!String.IsNullOrEmpty(MailAddress)) ? MailAddress.Substring(0, 1).ToLower() : ""; } } 
       
        public dtoBaseMessageRecipient()
        {
            Type = MailCommons.Domain.RecipientType.BCC;
        }
        public override String ToString() {
            return String.IsNullOrEmpty(DisplayName) ? MailAddress : DisplayName + "<" + MailAddress + ">";
        }

        public dtoRecipient ToRecipient()
        {
            return (dtoRecipient)this;
        }
    }
}
