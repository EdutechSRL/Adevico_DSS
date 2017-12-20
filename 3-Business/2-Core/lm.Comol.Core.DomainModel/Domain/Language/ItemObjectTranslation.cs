using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace lm.Comol.Core.DomainModel.Languages
{
    [Serializable,DataContract]
    public class ItemObjectTranslation
    {
        [DataMember]
        public virtual String Name { get; set; }
        [DataMember]
        public virtual String Subject { get; set; }
        [DataMember]
        public virtual String Body { get; set; }
        [DataMember]
        public virtual String PlainBody { get; set; }
        [DataMember]
        public virtual String ShortText { get; set; }
        [DataMember]
        public virtual String Signature { get; set; }
        [DataMember]
        public virtual Boolean IsHtml { get; set; }
      


        public ItemObjectTranslation()
        {

        }

        public ItemObjectTranslation Copy()
        {
            return new ItemObjectTranslation() { Body = this.Body, Name = this.Name, Subject = this.Subject, ShortText = this.ShortText, IsHtml = IsHtml, Signature = Signature };
        }
        public Boolean IsValid(Boolean alsoLongMessage = true,Boolean alsoShortMessage=false, Boolean alsoName=false){
            return (!alsoLongMessage || (alsoLongMessage && !String.IsNullOrEmpty(Body)))
                && (!alsoShortMessage || (alsoShortMessage && !String.IsNullOrEmpty(ShortText))) && (!alsoName || (alsoName && !String.IsNullOrEmpty(Name)));    
        }

        public virtual void RemoveTags(Dictionary<String, List<String>> tagsToRemove) {
            if (tagsToRemove != null && tagsToRemove.Count > 0) {
                RemoveTags(tagsToRemove.Select(r => r.Value).SelectMany(t => t).ToList());
            }
        }
        public virtual void RemoveTags(List<String> tags)
        {
            if (tags != null && tags.Count > 0)
            {
                tags.ForEach(tag => RemoveTag(tag));
            }
        }
        public virtual void RemoveTag(String tag)
        {
            if (!String.IsNullOrEmpty(Subject))
                Subject = Subject.Replace(tag, "");
            if (!String.IsNullOrEmpty(Body))
                Body = Body.Replace(tag, "");
            if (!String.IsNullOrEmpty(ShortText))
                ShortText = ShortText.Replace(tag, "");
        }

        public Boolean IsEmpty() {
            return String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Subject) && String.IsNullOrEmpty(Body) && String.IsNullOrEmpty(ShortText);
        }
        public Boolean IsContentEmpty() { 
            return String.IsNullOrEmpty(Subject) && String.IsNullOrEmpty(Body) && String.IsNullOrEmpty(ShortText);
        }

    }
}