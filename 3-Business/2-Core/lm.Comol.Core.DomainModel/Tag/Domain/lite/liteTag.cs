using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class liteTag : lm.Comol.Core.DomainModel.DomainBaseObject<long>, ICloneable 
    {
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsSystem { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual TagType Type { get; set; }
        public liteTag()
        {

        }

        public virtual liteTag BaseClone()
        {
            liteTag tag = new liteTag();
            tag.Type = Type;
            tag.IsDefault = false;
            tag.IsSystem = false;
            tag.Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Draft;
            return tag;
        }


        public virtual object Clone()
        {
            liteTag clone = BaseClone();
            return clone;
        }
    }
}