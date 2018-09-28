using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [CLSCompliant(true)]
    public class PermissionMenuDTO
    {
        public virtual Int16 Position { get; set; }
        public virtual String Name { get; set; }
    }
}
