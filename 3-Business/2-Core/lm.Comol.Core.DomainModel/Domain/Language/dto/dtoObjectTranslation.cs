using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.DomainModel.Languages
{

    [Serializable]
    public class dtoObjectTranslation : DomainBaseObject<long>, IDisposable 
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual Boolean IsEmpty { get { return Translation.IsEmpty(); } }
        public virtual Boolean IsValid { get { return Translation.IsValid(); } }
        public virtual ItemObjectTranslation Translation { get; set; }

        public dtoObjectTranslation()
        {
            Translation = new lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation();
        }
        //public dtoTranslation(TemplateTranslation t)
        //{
        //    Id = t.Id;
        //    Deleted = t.Deleted;
        //    IdVersion = t.Version.Id;
        //    IdLanguage = t.IdLanguage;
        //    Translation = t.Translation;
        //    LanguageCode = t.LanguageCode;
        //}

        public void Dispose()
        {
        }
    }
}