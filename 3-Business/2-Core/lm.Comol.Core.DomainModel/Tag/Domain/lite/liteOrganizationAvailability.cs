using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class liteOrganizationAvailability  : lm.Comol.Core.DomainModel.DomainBaseObject<long>, ICloneable 
    {
        public virtual long IdTag { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Int32 IdOrganization { get; set; }

        public liteOrganizationAvailability()
        {

        }

        public virtual object Clone()
        {
            liteOrganizationAvailability  clone = new liteOrganizationAvailability();
            clone.IdTag = IdTag;
            clone.IsDefault = IsDefault;
            clone.IdOrganization = IdOrganization;
            return clone;
        }
    }
}