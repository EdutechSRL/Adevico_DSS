using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class liteTileTranslation : lm.Comol.Core.DomainModel.Languages.dtoBaseObjectTranslation, ICloneable, IDisposable
    {
        public liteTileTranslation()
        {
            Translation = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
        }

        public virtual object Clone()
        {
            liteTileTranslation clone = new liteTileTranslation();
            clone.Id = Id;
            clone.Deleted = Deleted;
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