using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Editor
{
    [Serializable()]
    public class AdvancedLinkItem
    {
        public virtual String Tag { get; set; }
        public virtual List<ItemTranslation> Translations { get; set; }
        public virtual List<ItemAvailability> Availability { get; set; }
        public virtual Boolean AsPopup { get; set; }

        public String GetName(Int32 idLanguage, Int32 idDefault) {
            ItemTranslation t = GetTranslation(idLanguage, idDefault);
            return (t == null) ? "" : t.Name;
        }
        public String GetDescription(Int32 idLanguage, Int32 idDefault)
        {
            ItemTranslation t = GetTranslation(idLanguage, idDefault);
            return (t == null) ? "" : t.Description;
        }
        public String GetTooltip(Int32 idLanguage, Int32 idDefault)
        {
            ItemTranslation t = GetTranslation(idLanguage, idDefault);
            return (t==null) ? "" : t.Tooltip;
        }
        private ItemTranslation GetTranslation(Int32 idLanguage, Int32 idDefault)
        {
            ItemTranslation translation = GetTranslation(idLanguage);
            if (translation == null)
            {
                translation = GetTranslation(idDefault);
                if (translation == null && Translations.Any())
                    translation = Translations.FirstOrDefault();
            }
            return translation;
        }
        private ItemTranslation GetTranslation(Int32 idLanguage)
        {
            return (Translations == null || Translations.Count == 0) ? null : Translations.Where(t => t.IdLanguage == idLanguage).FirstOrDefault();
        }
        public AdvancedLinkItem() {
            Availability = new List<ItemAvailability>();
            Translations = new List<ItemTranslation>();
        }

        [Serializable()]
        public enum TranslationType
        {
            name = 0,
            description = 1,
            tooltip = 2,
        }
    }
    [Serializable()]
    public class ItemAvailability {
        public virtual EditorType AvailableFor { get; set; }
        public virtual String Url { get; set; }
        public virtual String PopupUrl { get; set; }
        public virtual String Script { get; set; }
        public virtual String DialogWidth { get; set; }
        public virtual String DialogHeight { get; set; }
    }



    [Serializable()]
    public class ItemTranslation
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String Tooltip { get; set; }
        public ItemTranslation()
        {
        }
    }
}