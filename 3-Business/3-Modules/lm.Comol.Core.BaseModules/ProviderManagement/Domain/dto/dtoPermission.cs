using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class dtoPermission
    {
        public virtual Boolean Enable { get; set; }
        public virtual Boolean Disable { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Info { get; set; }
        public virtual Boolean Delete { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean VirtualUndelete { get; set; }

        public dtoPermission() { }


        public dtoPermission(ModuleProviderManagement module, BaseStatusDeleted deleted, long usedBy, Boolean isEnabled) {
            Edit = (deleted == BaseStatusDeleted.None && module.EditProvider);
            Enable = !isEnabled && (deleted == BaseStatusDeleted.None && module.EditProvider);
            Disable = isEnabled && (deleted == BaseStatusDeleted.None && module.EditProvider);
            Info = (module.ViewProviders || module.Administration);
            if (module.DeleteProvider || module.Administration)
            {
                //if (usedBy > 0 && deleted != BaseStatusDeleted.None) {
                if (usedBy > 0 )
                {
                    if (deleted != BaseStatusDeleted.None)
                        VirtualUndelete = true;
                }
                
                else if (usedBy == 0)
                {
                    if (deleted == BaseStatusDeleted.None)
                        VirtualDelete = true;
                    else
                    {
                        VirtualUndelete = true;
                        Delete = true;
                    }
                }
            }
           
        }
    }
}