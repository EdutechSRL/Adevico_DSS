using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class liteSearchTile : lm.Comol.Core.DomainModel.DomainBaseObject<long>, ICloneable 
    {

        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation DefaultTranslation { get; set; }
        public virtual IList<liteTileTranslation> Translations { get; set; }
        public virtual IList<liteTileItem> SubItems { get; set; }
        public virtual TileType Type { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual AvailableStatus Status { get; set; }
        public liteSearchTile()
        {
            DefaultTranslation = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
            Translations = new List<liteTileTranslation>();
            Status = AvailableStatus.Draft;
            SubItems = new List<liteTileItem>();
        }
      
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage)
        {
            if (idUserLanguage == -1)
                return DefaultTranslation;
            else if (Translations != null && Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any())
                return Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault();
            else
                return new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
        }
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null || !translation.IsValid())
                translation = DefaultTranslation;
            return translation;
        }
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null || !translation.IsValid())
                translation = DefaultTranslation;
            return translation;
        }
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti, Boolean useFirstOccurence)
        {
            lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation translation = null;
            if (Translations == null || Translations.Any())
            {
                translation = Translations.Where(t => t.IdLanguage == idUserLanguage).FirstOrDefault().Translation;
            }
            if (translation == null && DefaultTranslation.IsValid() && firstIsMulti)
                translation = DefaultTranslation;
            if (translation == null && Translations.Any())
                translation = Translations.Where(t => t.IdLanguage == idDefaultLanguage).FirstOrDefault().Translation;
            if (translation == null && Translations.Any() && useFirstOccurence)
                translation = Translations.FirstOrDefault().Translation;
            return (translation == null) ? new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation() { Title = Id.ToString() } : translation;
        }

        public virtual String GetTitle(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String name = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation.Title : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation.Title).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation.Title).FirstOrDefault();
            if (String.IsNullOrEmpty(name))
                name = DefaultTranslation.Title;
            return name;
        }
        public virtual String GetLowerTitle(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            String name = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation.Title : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation.Title).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation.Title).FirstOrDefault();
            if (String.IsNullOrEmpty(name))
                name = DefaultTranslation.Title;
            return (String.IsNullOrEmpty(name) ? "" : name.ToLower());
        }

        public virtual String GetFirstLetter(Int32 idLanguage)
        {
            String title = "";
            if (idLanguage <= 0)
                title = DefaultTranslation.Title;
            else
                title = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? "" : Translations.Where(t => t.IdLanguage == idLanguage).Select(t => t.Translation.Title).FirstOrDefault();

            if (String.IsNullOrEmpty(title))
                return "";
            else
                return title[0].ToString().ToLower();
        }
        public virtual Boolean IsEmptyForLanguage(Int32 idLanguage)
        {
            if (idLanguage <= 0)
                return String.IsNullOrEmpty(DefaultTranslation.Title);
            else
                return (Translations == null || !Translations.Any()
                    ||
                    (!Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idLanguage && !String.IsNullOrEmpty(t.Translation.Title)).Any()));
        }
        public virtual object Clone()
        {
            liteTile tile = new liteTile();
            tile.Type = Type;
            tile.Status = Status;
            tile.DefaultTranslation = (lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation)DefaultTranslation.Clone();
            tile.Id = Id;
            tile.Deleted = Deleted;
            if (Translations != null)
                tile.Translations = Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => (liteTileTranslation)t.Clone()).ToList();
            if (SubItems != null)
                tile.SubItems = SubItems.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => (liteTileItem)a.Clone()).ToList();

            return tile;
        }
    }
}