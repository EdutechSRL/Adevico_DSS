using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
     [Serializable]
    public class dtoReorder 
    {
        public virtual long IdActivity { get; set; }
        public virtual long IdFather { get; set; }
        public virtual long DisplayOrder { get; set; }
        public dtoReorder()
        {
        }
    }
}