using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Domain
{
    [Serializable]
    public class dtoTextualRecipient
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String Addresses { get; set; }
    }
}
