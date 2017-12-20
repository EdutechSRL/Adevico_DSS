using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Languages
{
       [Serializable]
    public class ItemBaseTranslation : DomainBaseObjectLiteMetaInfo<long>, IDisposable, ICloneable 
        {
            public virtual Int32 IdLanguage { get; set; }
            public virtual String LanguageCode { get; set; }
            public virtual String LanguageName { get; set; }
            public virtual Boolean IsEmpty { get { return Translation.IsEmpty; } }
            public virtual Boolean IsValid { get { return Translation.IsValid(); } }
            public virtual TitleDescriptionObjectTranslation Translation { get; set; }

            public ItemBaseTranslation()
            {
                Translation = new TitleDescriptionObjectTranslation();
            }

            public virtual ItemBaseTranslation Copy(litePerson person, String ipAddress, String ipProxyAddress, String titlePrefix = "")
            {
                ItemBaseTranslation t = new ItemBaseTranslation();
                t.CreateMetaInfo(person, ipAddress, ipProxyAddress);

                t.IdLanguage = IdLanguage;
                t.LanguageCode = LanguageCode;
                t.LanguageName = LanguageName;
                t.Translation = Translation.Copy();
                if (!String.IsNullOrEmpty(titlePrefix))
                    t.Translation.Title = titlePrefix + t.Translation.Title;
                return t;
            }

            public void Dispose()
            {
            }

            public virtual object Clone()
            {
                ItemBaseTranslation t = new ItemBaseTranslation();

                t.IdLanguage = IdLanguage;
                t.LanguageCode = LanguageCode;
                t.LanguageName = LanguageName;
                t.Translation = Translation.Copy();
                return t;
            }
        }
    }