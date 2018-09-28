using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [Serializable]
    public class dtoPermission
    {
        public virtual bool VirtualUndelete { get; set; }
        public virtual bool Delete { get; set; }
        public virtual bool VirtualDelete { get; set; }
        public virtual bool Edit { get; set; }
        public virtual bool AddItem { get; set; }
    }
}