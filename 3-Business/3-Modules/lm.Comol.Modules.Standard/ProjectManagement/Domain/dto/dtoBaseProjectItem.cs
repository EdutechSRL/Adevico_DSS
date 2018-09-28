using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoBaseProjectItem 
    {
        public virtual long Id { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual int Completeness { get; set; }
        public virtual ProjectItemStatus Status { get; set; }

        public virtual BaseStatusDeleted Deleted { get; set; }

        public dtoBaseProjectItem()
        {

        }
    }
}