using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{

    [Serializable]
    public class MailTemplateContent
    {
        public virtual long Id { get; set; } 
        public virtual MailTemplate Template { get; set; }
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual Boolean IsEmpty { get { return Translation.IsEmpty(); } }
        public virtual Boolean IsValid { get { return Translation.IsValid(); } }
        public virtual lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation Translation { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public MailTemplateContent()
        {
            Translation = new lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation();
        }
    }
}