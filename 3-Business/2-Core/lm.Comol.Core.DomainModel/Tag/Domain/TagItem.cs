using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class TagItem : lm.Comol.Core.DomainModel.DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation DefaultTranslation { get; set; }
         public virtual IList<TagTranslation> Translations { get; set; }

        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsSystem { get; set; }
        public virtual Boolean ForAllCommunityTypes { get; set; }
        public virtual IList<Int32> CommunityTypes { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.Tile MyTile { get; set; }
        public virtual IList<CommunityTag> CommunityAssignments { get; set; }
        public virtual IList<OrganizationAvailability> Organizations { get; set; }
        public virtual TagType Type { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }

        public TagItem()  {
            Type = TagType.Community;
            IsDefault = false;
            IsSystem = false;
            Organizations = new List<OrganizationAvailability>();
            Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Draft;
            CommunityAssignments = new List<CommunityTag>();
            DefaultTranslation = new TitleDescriptionObjectTranslation();
            Translations = new List<TagTranslation>();
            CommunityTypes = new List<Int32>();
        }

        public virtual TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage, Int32 idDefaultLanguage)
        {
            TitleDescriptionObjectTranslation translation = (Translations == null || (Translations.Any() && !Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Any())) ? DefaultTranslation : (Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any()) ? Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault() : Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idDefaultLanguage).Select(t => t.Translation).FirstOrDefault();
            if (translation == null || !translation.IsValid())
                translation = DefaultTranslation;
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
        public virtual TitleDescriptionObjectTranslation GetTranslation(Int32 idUserLanguage)
        {
            if (idUserLanguage == -1)
                return DefaultTranslation;
            else if (Translations != null && Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Any())
                return Translations.Where(t => t.Deleted == BaseStatusDeleted.None && t.IdLanguage == idUserLanguage).Select(t => t.Translation).FirstOrDefault();
            else
                return new TitleDescriptionObjectTranslation();
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


        public virtual TagItem BaseClone()
        {
            TagItem tag = new TagItem();
            tag.IdModule = IdModule;
            tag.ModuleCode = ModuleCode;
            tag.Type = Type;
            tag.IsDefault = false;
            tag.IsSystem = false;
            tag.Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Draft;
            tag.DefaultTranslation = (lm.Comol.Core.DomainModel.Languages.TitleDescriptionObjectTranslation)DefaultTranslation.Clone();
            return tag;
        }


        //public object Clone()
        //{
        //    TagItem tag = BaseClone();
        //    if (Translations != null)
        //        tag.Translations = Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => (lm.Comol.Core.DomainModel.Languages.)t.Clone()).ToList();
        //    if (CommunityAssignments!=null)
        //        tag.CommunityAssignments = CommunityAssignments.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => (CommunityTag)c.Clone()).ToList();
        //    if (tag.MyTile != null)
        //        tag.MyTile = (lm.Comol.Core.Dashboard.Domain.Tile)MyTile.Clone();
        //    return tag;
        //}

        public virtual TagItem Copy(litePerson person, String ipAddress, String proxyIpAddress, String titlePrefix = "")
        {
            TagItem clone = BaseClone();
            clone.CreateMetaInfo(person, ipAddress, proxyIpAddress);
            if (Translations != null)
                clone.Translations = Translations.Where(t => t.Deleted == BaseStatusDeleted.None).Select(t => t.Copy(clone, person, ipAddress, proxyIpAddress,titlePrefix, clone.CreatedOn)).ToList();
            if (CommunityAssignments != null)
                clone.CommunityAssignments = CommunityAssignments.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.Copy(clone,person, ipAddress, proxyIpAddress, clone.CreatedOn)).ToList();
            if (Organizations != null)
                clone.Organizations = Organizations.Where(c => c.Deleted == BaseStatusDeleted.None).Select(c => c.Copy(clone, person, ipAddress, proxyIpAddress, clone.CreatedOn)).ToList();
            if (MyTile != null)
                clone.MyTile = MyTile.Copy(clone, person, ipAddress, proxyIpAddress, titlePrefix, clone.CreatedOn);
            return clone;
        }
    }
}