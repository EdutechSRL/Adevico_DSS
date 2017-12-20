using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class MacAttribute : BaseUrlMacAttribute
    {
        public virtual IList<MacAttributeItem> Items { get; set; }

        public MacAttribute()
        {
            Items = new List<MacAttributeItem>();
            Type = UrlMacAttributeType.mac;
        }
    }

    [Serializable]
    public class MacAttributeItem : DomainBaseObject<long>
    {
        public virtual MacAttribute Owner { get; set; }
        public virtual BaseUrlMacAttribute Attribute { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public MacAttributeItem()
        {
             
        }
    } 
}