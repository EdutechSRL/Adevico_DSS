using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class liteTagItem : lm.Comol.Core.DomainModel.DomainBaseObject<long>, ICloneable 
    {
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation DefaultTranslation { get; set; }
        public virtual IList<liteTagTranslation> Translations { get; set; }
        public virtual Int32 IdCreatedBy { get; set; }
        public virtual Int32 IdModifiedBy { get; set; }
        
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsSystem { get; set; }
        public virtual Boolean ForAllCommunityTypes { get; set; }
        public virtual IList<Int32> CommunityTypes { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual long IdTile { get; set; }
        public virtual IList<liteCommunityTag> CommunityAssignments { get; set; }
        public virtual IList<liteOrganizationAvailability> Organizations { get; set; }

        public virtual TagType Type { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
      
        public liteTagItem()
        {
            CommunityTypes = new List<Int32>();
        }

        public virtual TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage)
        {
            if (idUserLanguage == -1)
                return DefaultTranslation;
            else if (Translations != null && Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any())
                return Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t=> t.Translation).FirstOrDefault();
            else
                return new TitleDescriptionObjectTranslation();
        }
        public virtual TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            TitleDescriptionObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null || !translation.IsValid())
                translation = DefaultTranslation;
            return translation;
        }
        public virtual liteTagTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage, String dLanguageName, String dLanguageCode)
        {
            liteTagTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? null : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).FirstOrDefault();
            if (translation == null)
                translation = new liteTagTranslation() { IdLanguage = 0, LanguageCode = dLanguageCode, LanguageName = dLanguageName, Translation = DefaultTranslation };
            return translation;
        }

        public virtual TitleDescriptionObjectTranslation GetTranslation(String userLanguageCode, Int32 idDefaultLanguage)
        {
            TitleDescriptionObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.LanguageCode == userLanguageCode).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null || !translation.IsValid())
                translation = DefaultTranslation;
            return translation;
        }
        public virtual TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage, Boolean firstIsMulti, Boolean useFirstOccurence)
        {
            TitleDescriptionObjectTranslation translation = null;
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
            return (translation == null) ? new TitleDescriptionObjectTranslation() {  Title = Id.ToString() } : translation;
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
                title = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? "" : Translations.Where(t=> t.IdLanguage== idLanguage ).Select(t=> t.Translation.Title).FirstOrDefault();

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
        public virtual liteTagItem BaseClone()
        {
            liteTagItem tag = new liteTagItem();
            tag.IdModule = IdModule;
            tag.ModuleCode = ModuleCode;
            tag.Type = Type;
            tag.IsDefault = false;
            tag.IsSystem = false;
            tag.Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Draft;
            tag.DefaultTranslation = (lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation)DefaultTranslation.Clone();
            tag.IdCreatedBy = IdCreatedBy;
            tag.IdModifiedBy = IdModifiedBy;
            tag.ModifiedOn = ModifiedOn;
            tag.CreatedOn = CreatedOn;
            return tag;
        }


        public virtual object Clone()
        {
            liteTagItem clone = BaseClone();
            if (Translations != null)
                clone.Translations = Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => (liteTagTranslation)t.Clone()).ToList();
            if (CommunityAssignments != null)
                clone.CommunityAssignments = CommunityAssignments.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => (liteCommunityTag)c.Clone()).ToList();
            if (Organizations != null)
                clone.Organizations = Organizations.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => (liteOrganizationAvailability)c.Clone()).ToList();
            clone.IdTile = IdTile;
            return clone;
        }
    }
}