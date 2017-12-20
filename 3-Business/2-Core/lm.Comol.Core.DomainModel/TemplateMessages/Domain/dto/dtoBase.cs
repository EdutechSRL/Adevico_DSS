using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoBase
    {
        public virtual long Id { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }

        public dtoBase()
        {
        }
        public dtoBase(long id)
        {
        }
    }
}
