using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class Tile : lm.Comol.Core.DomainModel.DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual IList<TileTagAssociation> Tags { get; set; }
        public virtual IList<Int32> CommunityTypes { get; set; }
        public virtual IList<TileAssignment> Assignments { get; set; }

        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation DefaultTranslation { get; set; }
        public virtual IList<TileTranslation> Translations { get; set; }

        public virtual IList<TileItem> SubItems { get; set; }
        public virtual TileType Type { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual AvailableStatus Status { get; set; }
        public virtual Boolean AutoNavigateUrl { get; set; }
        public virtual String NavigateUrl { get; set; }
        public virtual String ImageUrl { get; set; }
        public virtual String ImageCssClass { get; set; }
        // public virtual long IdModulePage { get; set; }
        public Tile() {
            DefaultTranslation = new lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation();
            Translations = new List<TileTranslation>();
            Tags = new List<TileTagAssociation>();
            Status = AvailableStatus.Draft;
            Assignments = new List<TileAssignment>();
            CommunityTypes = new List<Int32>();
            SubItems = new List<TileItem>();
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


        public virtual Tag.Domain.TagItem GetDefaultTag()
        {
            if (Type != TileType.CommunityTag && Tags == null || !Tags.Where(t => t.Deleted == BaseStatusDeleted.None).Any() || Tags.Where(t => t.Deleted == BaseStatusDeleted.None).Count()>1)
                return null;
            else
            {
                TileTagAssociation tAssociation= Tags.Where(t => t.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                if (tAssociation.Tag != null)
                    return tAssociation.Tag;
            }
            return null;
        }


        public virtual Tile BaseClone()
        {
            Tile tile = new Tile();
            tile.Type = Type;
            tile.Status = Status;
            tile.IdCommunity = IdCommunity;
            tile.AutoNavigateUrl = AutoNavigateUrl;
            tile.NavigateUrl = NavigateUrl;
            tile.ImageUrl = ImageUrl;
            tile.ImageCssClass = ImageCssClass;
            tile.CommunityTypes = CommunityTypes;
            tile.DefaultTranslation = (lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation)DefaultTranslation.Clone();
            return tile;
        }

        public virtual Tile Copy(lm.Comol.Core.Tag.Domain.TagItem item, litePerson person, String ipAddress, String proxyIpAddress, String titlePrefix = "", DateTime? createdOn = null)
        {
            Tile clone = Copy(person,ipAddress,proxyIpAddress,titlePrefix, createdOn);
            if (item != null)
                Tags.Add(new TileTagAssociation() { Tag = item, Tile = clone });
            return clone;
        }
        public virtual Tile Copy(litePerson person, String ipAddress, String proxyIpAddress, String titlePrefix = "", DateTime? createdOn = null)
        {
            Tile clone = BaseClone();
            clone.CreateMetaInfo(person, ipAddress, proxyIpAddress, createdOn);
            if (Translations != null)
                clone.Translations = Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => t.Copy(clone, person, ipAddress, proxyIpAddress, titlePrefix, clone.CreatedOn)).ToList();
            if (Assignments != null)
                clone.Assignments = Assignments.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.Copy(clone, person, ipAddress, proxyIpAddress, clone.CreatedOn)).ToList();
            if (Tags != null)
                clone.Tags = Tags.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.Copy(clone, person, ipAddress, proxyIpAddress, clone.CreatedOn)).ToList();
            if (SubItems != null)
                clone.SubItems = SubItems.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.Copy(clone, person, ipAddress, proxyIpAddress, clone.CreatedOn)).ToList();

            return clone;
        }
    }
}