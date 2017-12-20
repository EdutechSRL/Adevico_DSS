using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    public class liteLK_UserCategory
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64? IdUser { get; set; }

        public virtual Int64? IdCategory { get; set; }

        public virtual bool IsManager { get; set; }

    }
}
