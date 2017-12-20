using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoVersionItem
    {
        public virtual long Id { get; set; }
        public virtual long IdTemplate { get; set; }
        public virtual int Number { get; set; }
        public virtual TemplateStatus Status { get; set; }
        public virtual DateTime? Lastmodify { get; set; }
        public virtual Boolean IsSelected { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual TemplateLevel Level { get; set; }
        public dtoVersionItem()
        {
            Level = TemplateLevel.Removed;
        }
        public dtoVersionItem(TemplateLevel l) {
            Level = l;
        }
    }
}
