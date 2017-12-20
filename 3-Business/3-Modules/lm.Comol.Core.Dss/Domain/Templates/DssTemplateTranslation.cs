using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dss.Domain.Templates
{
    [Serializable]
    public class DssTemplateTranslation : DomainBaseObject<long>, IDisposable 
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual Boolean IsEmpty { get { return Translation.IsEmpty(); } }
        public virtual Boolean IsValid { get { return Translation.IsValid(); } }
        public virtual ItemObjectTranslation Translation { get; set; }
        public virtual TranslationType Type { get; set; }
        public virtual long IdMethod { get; set; }
        public virtual long IdRatingSet { get; set; }
        public virtual long IdRatingValue { get; set; }

        public DssTemplateTranslation()
        {
            Translation = new ItemObjectTranslation();
        }

        //public virtual TemplateTranslation Copy(String namePrefix = "")
        //{
        //    TemplateTranslation t = new TemplateTranslation();
        //    t.IdLanguage = IdLanguage;
        //    t.LanguageCode = LanguageCode;
        //    t.Translation = Translation.Copy();
        //    if (!String.IsNullOrEmpty(namePrefix))
        //        t.Translation.Name = namePrefix + t.Translation.Name;
        //    return t;
        //}

        public void Dispose()
        {
            
        }
    }
}