using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoAgencyItem
    {
        public virtual long Id { get; set; }
        public virtual lm.Comol.Core.DomainModel.dtoAgency Agency {get; set;}
        public virtual dtoAgencyPermission Permission { get; set; }
        public virtual Int32 UsedBy { get; set; }
        public dtoAgencyItem() { 

        }
        public dtoAgencyItem(lm.Comol.Core.DomainModel.dtoAgency item)
        {
            Id = item.Id;
            Agency = item;
            Permission = new dtoAgencyPermission();
        }
        public dtoAgencyItem(lm.Comol.Core.DomainModel.dtoAgency item, dtoAgencyPermission permission)
        {
            Id = item.Id;
            Agency = item;
            Permission = permission;
        }
        public dtoAgencyItem(lm.Comol.Core.DomainModel.dtoAgency item, Int32 loaderType, BaseStatusDeleted deleted, Int32 userCount  )
        {
            Id = item.Id;
            Agency = item;
            Permission = new dtoAgencyPermission(loaderType, item, userCount);
            UsedBy = userCount;
        }
    }
}
