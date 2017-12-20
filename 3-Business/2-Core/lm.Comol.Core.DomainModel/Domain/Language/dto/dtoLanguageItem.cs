using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.DomainModel.Languages
{
    [Serializable]
    public class dtoLanguageItem
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual Boolean IsMultiLanguage { get; set; }
        public virtual Boolean IsDefault { get; set; }

        public virtual String ShortCode { get { return (IsMultiLanguage) ? LanguageCode : (String.IsNullOrEmpty(LanguageCode) ? "" : (LanguageCode.Contains("-") ? LanguageCode.Split('-').FirstOrDefault().ToUpper() : LanguageCode.ToUpper())); } }


        public dtoLanguageItem() { }
      
    }
}