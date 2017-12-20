using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoProfileItem <T>
    {
        public virtual long Id { get; set; }
        public virtual T Profile {get; set;}
        public virtual dtoProfilePermission Permission { get; set; }
        public virtual long ProvidersCount { get; set; }
        public virtual String TypeName { get; set; }
        public virtual StatusProfile Status { get; set; }
        public virtual String AuthenticationTypeName { get; set; }
        public virtual Authentication.AuthenticationProviderType AuthenticationType{ get; set; } 

        public dtoProfileItem() { 

        }
        public dtoProfileItem(T item)
        {
            Profile = item;
            Permission = new dtoProfilePermission();
        }
        public dtoProfileItem(T item,dtoProfilePermission permission)
        {
            Profile = item;
            Permission = permission;
        }
    }
}
