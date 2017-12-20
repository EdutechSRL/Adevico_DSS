using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    public class liteSettingsPermission
    {
        public virtual Int64 Id { get; set; }
        public virtual liteUser User { get; set; }
        
        public virtual Int32? PersonTypeId { get; set; }

        public virtual Enums.PermissionType PermissionType { get; set; }

        public virtual Int64 PermissionValue { get; set; }

        public virtual lm.Comol.Core.DomainModel.BaseStatusDeleted Deleted { get; set; }
        //DELETED!!!
    }
}