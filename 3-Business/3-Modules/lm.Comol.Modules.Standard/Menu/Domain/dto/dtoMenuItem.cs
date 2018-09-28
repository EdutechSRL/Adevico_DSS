using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoMenuItem : dtoBaseMenuItem
    {
        public virtual long IdItemOwner { get; set; }
        public virtual long IdColumnOwner { get; set; }
        public virtual long ParentsNumber { get; set; }
    }
}