using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Languages
{
    [Serializable]
    public class TitleDescriptionObjectTranslation: ICloneable 
    {
        public virtual String Title { get; set; }
        public virtual String ShortTitle { get; set; }
        public virtual String Description { get; set; }

        public virtual Boolean IsEmpty { get { return String.IsNullOrEmpty(Title) && String.IsNullOrEmpty(Description) && String.IsNullOrEmpty(ShortTitle); } }
        public virtual Boolean IsContentEmpty { get { return String.IsNullOrEmpty(Description); } }

        public TitleDescriptionObjectTranslation()
        {

        }

        public TitleDescriptionObjectTranslation Copy()
        {
            return new TitleDescriptionObjectTranslation() { Title = this.Title, ShortTitle = this.ShortTitle, Description = this.Description};
        }
        public Boolean IsValid(Boolean alsoTitle = true, Boolean alsoDescription = false, Boolean alsoShortTitle = false)
        {
            return (!alsoTitle || (alsoTitle && !String.IsNullOrEmpty(Title)))
                && (!alsoDescription || (alsoDescription && !String.IsNullOrEmpty(Description))) && (!alsoShortTitle || (alsoShortTitle && !String.IsNullOrEmpty(ShortTitle)));
        }

        public virtual Boolean IsTranslationEqual(String title, String shortTitle, Boolean andClause ) {
            return (andClause && IsTitleEqual(title) && IsShortTitleEqual(shortTitle)) || (!andClause && (IsTitleEqual(title) || IsShortTitleEqual(shortTitle)));
        }
        public Boolean IsTitleEqual(String title)
        {
            title = String.IsNullOrEmpty(title) ? "" : title.ToLower();
            return ((!String.IsNullOrEmpty(Title) && Title.ToLower() == title));
        }
        public virtual Boolean IsShortTitleEqual(String shortTitle)
        {
            shortTitle = String.IsNullOrEmpty(shortTitle) ? "" : shortTitle.ToLower();
            return ((!String.IsNullOrEmpty(ShortTitle) && ShortTitle.ToLower() == shortTitle));
        }
        public virtual Boolean IsEqual(TitleDescriptionObjectTranslation item)
        {
            return (Title.Equals(item.Title) && Description.Equals(item.Description) && ShortTitle.Equals(item.ShortTitle));
        }
        public object Clone()
        {
            TitleDescriptionObjectTranslation clone = new TitleDescriptionObjectTranslation();
            clone.Title= Title;
            clone.ShortTitle= ShortTitle;
            clone.Description = Description;
            return clone;
        }
    }
}
