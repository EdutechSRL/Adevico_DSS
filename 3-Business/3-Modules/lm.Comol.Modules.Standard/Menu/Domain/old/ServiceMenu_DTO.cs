using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    [CLSCompliant(true)]
    public class ServiceMenu_DTO
    {
        public virtual Int32 ID { get; set; }
        public virtual String Name { get; set; }
        public virtual IList<PermissionMenuDTO> AvailablePermissions { get; set; }
    }
}
