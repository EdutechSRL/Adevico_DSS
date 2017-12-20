using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoSubscriptionProfileItem <T>
    {
        public virtual long Id { get; set; }
        public virtual T Profile {get; set;}
        public virtual String TypeName { get; set; }
        public virtual SubscriptionStatus Status { get; set; }
        public virtual String RoleName { get; set; }

        public dtoSubscriptionProfileItem() { 

        }
        public dtoSubscriptionProfileItem(T item)
        {
            Profile = item;
        }
    }
}