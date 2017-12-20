using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class TagTranslation : lm.Comol.Core.DomainModel.Languages.ItemBaseTranslation, ITagBaseItem
    {
        public virtual TagItem Tag {get;set;}

        public TagTranslation()
        {
            Translation = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
        }

        public virtual TagTranslation Copy(TagItem tag, litePerson person, String ipAddress, String ipProxyAddress, String titlePrefix = "", DateTime? createdOn = null)
        {
            TagTranslation t = new TagTranslation();
            t.CreateMetaInfo(person, ipAddress, ipProxyAddress,createdOn);

            t.Tag = tag;
            t.IdLanguage = IdLanguage;
            t.LanguageCode = LanguageCode;
            t.Translation = Translation.Copy();
            if (!String.IsNullOrEmpty(titlePrefix))
                t.Translation.Title = titlePrefix + t.Translation.Title;
            return t;
        }

        public void Dispose()
        {
        }
    }
}
