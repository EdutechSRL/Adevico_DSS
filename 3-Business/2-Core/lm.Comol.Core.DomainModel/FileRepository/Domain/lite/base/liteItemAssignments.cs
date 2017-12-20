using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    public class liteItemAssignments : liteBaseAssignments
    {
        public virtual long Permissions { get; set; }
        public virtual Boolean Denyed { get; set; }
        public virtual Boolean Inherited { get; set; }
    }
} 