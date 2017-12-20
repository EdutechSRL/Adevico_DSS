using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
     [Serializable]
    public class OrganizationAttributeItem : DomainBaseObject<long>
    {
        public virtual OrganizationAttribute Owner { get; set; }
        public virtual String RemoteCode { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Int32 IdDefaultProfile { get; set; }
        public virtual long IdDefaultPage { get; set; }
        public OrganizationAttributeItem()
        {
            
        }
    } 
}