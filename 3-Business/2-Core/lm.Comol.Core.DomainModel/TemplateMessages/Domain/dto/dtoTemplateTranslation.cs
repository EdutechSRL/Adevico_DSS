using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{

    [Serializable]
    public class dtoTemplateTranslation : lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation
    {
        public virtual long IdVersion { get; set; }
        public dtoTemplateTranslation()
        {
            Translation = new DomainModel.Languages.ItemObjectTranslation ();
        }
        public dtoTemplateTranslation(TemplateTranslation t)
        {
            if (t != null)
            {
                Id = t.Id;
                Deleted = t.Deleted;
                IdVersion = t.Version.Id;
                IdLanguage = t.IdLanguage;
                Translation = t.Translation;
                LanguageCode = t.LanguageCode;
                LanguageName = t.LanguageName;
            }
        }

        public Boolean IsCompliant(lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation item) {
            if (item == null)
                return false;
            else
                return ((item.Subject == Translation.Subject)
                        &&
                        (
                            (String.IsNullOrEmpty(item.Body) && String.IsNullOrEmpty(Translation.Body))
                            ||
                            (!String.IsNullOrEmpty(item.Body) && !String.IsNullOrEmpty(Translation.Body) && item.Body.Replace("&nbsp;","") == Translation.Body.Replace("&nbsp;",""))
                            )
                        &&
                        (
                            (String.IsNullOrEmpty(item.Signature) && String.IsNullOrEmpty(Translation.Signature))
                            ||
                            (!String.IsNullOrEmpty(item.Signature) && !String.IsNullOrEmpty(Translation.Signature) && item.Signature.Replace("&nbsp;", "") == Translation.Signature.Replace("&nbsp;", ""))
                            )
                       );
        }
    }
}