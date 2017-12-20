using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoAgencyPermission
    {
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Info { get; set; }
        public virtual Boolean Delete { get; set; }
        public virtual Boolean VirtualDelete { get; set; }
        public virtual Boolean VirtualUndelete { get; set; }

        public dtoAgencyPermission() { }

        public dtoAgencyPermission(Int32 loaderType, dtoAgency agency, Int32 userCount)
        {
            Edit = (agency.Deleted == BaseStatusDeleted.None && (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative));
            Info = (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative);
            Delete = (!agency.IsDefault && !agency.IsEmpty) &&( loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative) ;
            if (Delete)
            {
                //if (usedBy > 0 && deleted != BaseStatusDeleted.None) {
                if (userCount > 0)
                {
                    VirtualUndelete = (agency.Deleted != BaseStatusDeleted.None);
                    Delete = false;
                }
                else if (userCount == 0)
                {
                    if (agency.Deleted == BaseStatusDeleted.None)
                    {
                        VirtualDelete = true;
                        Delete = false;
                    }
                    else
                    {
                        VirtualUndelete = true;
                    }
                }
            }
        }
    }
}
