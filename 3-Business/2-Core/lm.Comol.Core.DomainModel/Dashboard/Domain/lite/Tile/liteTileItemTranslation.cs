using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class liteTileItemTranslation :DomainBaseObject<long>, ICloneable , IDisposable 
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual String ToolTip { get; set; }

        public liteTileItemTranslation()
        {

        }

        public virtual object Clone()
        {
            liteTileItemTranslation clone = new liteTileItemTranslation();
            clone.Id = Id;
            clone.Deleted = Deleted;
            clone.IdLanguage = IdLanguage;
            clone.LanguageCode = LanguageCode;
            clone.ToolTip = ToolTip;

            return clone;
        }
        public void Dispose()
        {
        }

       
    }
}