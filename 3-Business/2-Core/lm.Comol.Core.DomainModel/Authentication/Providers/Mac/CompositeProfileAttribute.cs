using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class CompositeProfileAttribute : BaseUrlMacAttribute
    {
        public virtual String MultipleValueSeparator { get; set; }
        public virtual IList<CompositeAttributeItem> Items { get; set; }
        public virtual ProfileAttributeType Attribute {get;set;}
        public CompositeProfileAttribute()
        {
            Items = new List<CompositeAttributeItem>();
            Type = UrlMacAttributeType.compositeProfile;
        }
    }

    [Serializable]
    public class CompositeAttributeItem : DomainBaseObject<long>
    {
        public virtual BaseUrlMacAttribute Owner { get; set; }
        public virtual BaseUrlMacAttribute Attribute { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public CompositeAttributeItem()
        {
             
        }
    } 
}