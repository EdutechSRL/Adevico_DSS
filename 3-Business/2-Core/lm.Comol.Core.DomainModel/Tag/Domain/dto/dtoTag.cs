using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Filters;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class dtoTag 
    {
        public long Id { get; set; }
        public Boolean IsDefault { get; set; }
        public Boolean IsSystem { get; set; }
        public lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public Boolean IsReadonly { get; set; }
        public dtoTranslation SelectedTranslation { get; set; }
        public List<dtoTranslation> Translations { get; set; }
        public List<FilterListItem> Organizations { get; set; }
        public List<FilterListItem> CommunityTypes { get; set; }
        public List<FilterListItem> SelectedCommunityTypes { get; set; }

        public dtoTag()
        {
            Translations = new List<dtoTranslation>();
            Organizations = new List<FilterListItem>();
            CommunityTypes = new List<FilterListItem>();
            SelectedCommunityTypes = new List<FilterListItem>();
        }
    }
    [Serializable]
    public class dtoTranslation
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageCode { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual String Title { get; set; }
        public virtual String Description { get; set; }

        public dtoTranslation()
        { }

        public dtoTranslation(liteTagTranslation t)
        {
            IdLanguage = t.IdLanguage;
            LanguageCode = t.LanguageCode;
            LanguageName = t.LanguageName;
            Title = t.Translation.Title;
            Description = t.Translation.Description;
        }
    }

    [Serializable]
    public class dtoTagApply
    {
        public dtoTranslation Name { get; set; }
        public long IdTag { get; set; }
        public Boolean AllCommunityTypes { get; set; }
        public Boolean OnlyCommunityWithoutTag { get; set; }
        public Boolean OnlyCommunityWithoutThisTag { get; set; }
        public Boolean IsReadonly { get; set; }
        public List<FilterListItem> CommunityTypes { get; set; }

        public dtoTagApply()
        {
            CommunityTypes = new List<FilterListItem>();
        }
      
    }
}