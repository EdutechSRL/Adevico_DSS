using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class OrganizationAttribute : BaseUrlMacAttribute
    {
        public virtual Boolean AllowMultipleValue { get; set; }
        public virtual String MultipleValueSeparator { get; set; }
        public virtual IList<OrganizationAttributeItem> Items { get; set; }

        public OrganizationAttribute()
        {
            Items = new List<OrganizationAttributeItem>();
            Type = UrlMacAttributeType.organization;
        }

        public virtual Boolean isUniqueCode(String remoteCode) {
            return (!Items.Any() || !Items.Where(i => i.Deleted == BaseStatusDeleted.None && i.RemoteCode == remoteCode).Any());
        }
    } 
}