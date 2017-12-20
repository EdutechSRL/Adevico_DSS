using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoTemplateItem
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
        public virtual TemplateLevel Level { get; set; }


        public virtual List<dtoVersionItem> Versions { get; set; }

        public virtual BaseStatusDeleted Deleted { get; set; }

        public dtoTemplateItem(){
            Versions = new List<dtoVersionItem>();
        }
    }
}