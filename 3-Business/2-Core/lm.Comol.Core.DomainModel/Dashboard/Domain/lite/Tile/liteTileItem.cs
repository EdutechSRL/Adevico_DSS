using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class liteTileItem : lm.Comol.Core.DomainModel.DomainBaseObject<long>, ICloneable
    {
        public virtual IList<liteTileItemTranslation> Translations { get; set; }
        public virtual String NavigateUrl { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual String CssClass { get; set; }
        public virtual TileItemType Type { get; set; }
       // public virtual long IdModulePage { get; set; }

        public liteTileItem()
        {
            Translations = new List<liteTileItemTranslation>();
            Type = TileItemType.UserDefinedUrl;
        }

        public virtual String GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? ToolTip : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.ToolTip).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.ToolTip).FirstOrDefault();
            if (String.IsNullOrEmpty(translation))
                translation = ToolTip;
            return translation;
        }
        public virtual String GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            String translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? ToolTip : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Select(t => t.ToolTip).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.ToolTip).FirstOrDefault();
            if (String.IsNullOrEmpty(translation))
                translation = ToolTip;
            return translation;
        }
        public virtual String GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti, Boolean useFirstOccurence)
        {
            String translation = "";
            if (Translations == null || Translations.Any())
                translation = Translations.Where(t => t.IdLanguage == idUserLanguage).FirstOrDefault().ToolTip;
            if (String.IsNullOrEmpty(translation)  && !String.IsNullOrEmpty(ToolTip) && firstIsMulti)
                translation = ToolTip;
            if (String.IsNullOrEmpty(translation) && Translations.Any())
                translation = Translations.Where(t => t.IdLanguage == idDefaultLanguage).FirstOrDefault().ToolTip;
            if (String.IsNullOrEmpty(translation) && Translations.Any() && useFirstOccurence)
                translation = Translations.FirstOrDefault().ToolTip;
            return (String.IsNullOrEmpty(translation)) ? Id.ToString() : translation;
        }

        public virtual object Clone()
        {
            liteTileItem clone = new liteTileItem();
            clone.Type = Type;
            clone.CssClass = CssClass;
            clone.ToolTip = ToolTip;
            clone.NavigateUrl = NavigateUrl;
            if (Translations.Any())
                clone.Translations = Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => (liteTileItemTranslation)t.Clone()).ToList();
            clone.Deleted = Deleted;
            clone.Id = Id;
            return clone;
        }
    }
}