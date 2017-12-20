using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class liteTagTranslation : lm.Comol.Core.DomainModel.Languages.dtoBaseObjectTranslation, ICloneable
    {
        public liteTagTranslation()
        {
            Translation = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
        }
        public virtual object Clone()
        {
            liteTagTranslation clone = new liteTagTranslation();

            clone.IdLanguage = IdLanguage;
            clone.LanguageCode = LanguageCode;
            clone.Translation = Translation.Copy();
            return clone;
        }
        public void Dispose()
        {
        }

      
    }
}
