using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoProfilePermission
    {
        public virtual Boolean Edit { get; set; }
        public virtual Boolean AdvancedInfo { get; set; }
        public virtual Boolean Info { get; set; }
        public virtual Boolean Delete { get; set; }
        public virtual Boolean ManageAuthentication { get; set; }
        public virtual Boolean ChangeProfileType { get; set; }
        public virtual Boolean LogonAs { get; set; }
        public virtual Boolean AdministrativeLogonAs { get; set; }
        public virtual Boolean RenewPassword { get; set; }
        public virtual Boolean SetPassword { get; set; }
        public virtual Boolean EditStatus { get; set; }

        public dtoProfilePermission(){}

        public dtoProfilePermission(Int32 loaderType, Int32 idProfileType) {
            EditStatus = (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative) && HasTypePermission(loaderType, idProfileType);
            Edit = (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative) && HasTypePermission(loaderType, idProfileType);
            AdvancedInfo = (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative);
            Info = (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative);
            Delete = (idProfileType != (int) UserTypeStandard.Guest) && (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative) && HasTypePermission(loaderType, idProfileType);
            ManageAuthentication = (idProfileType != (int)UserTypeStandard.Guest) && (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator) && HasTypePermission(loaderType, idProfileType);
            ChangeProfileType = (idProfileType != (int)UserTypeStandard.Guest) && (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator) && HasTypePermission(loaderType, idProfileType);
            LogonAs = (idProfileType != (int)UserTypeStandard.Guest) && (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator) && HasTypePermission(loaderType, idProfileType);
            AdministrativeLogonAs = (idProfileType != (int)UserTypeStandard.Guest) && (loaderType == (int)UserTypeStandard.Administrative|| loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator) && HasTypePermission(loaderType, idProfileType);
            RenewPassword = (idProfileType != (int)UserTypeStandard.Guest) && (loaderType == (int)UserTypeStandard.SysAdmin || loaderType == (int)UserTypeStandard.Administrator || loaderType == (int)UserTypeStandard.Administrative);
            SetPassword = (idProfileType != (int)UserTypeStandard.Guest) && (
                (loaderType == (int)UserTypeStandard.SysAdmin && idProfileType != (int)UserTypeStandard.SysAdmin)
                ||
                (loaderType == (int)UserTypeStandard.Administrator && idProfileType != (int)UserTypeStandard.SysAdmin && idProfileType != (int)UserTypeStandard.Administrator)
                || 
                (loaderType == (int)UserTypeStandard.Administrative && idProfileType != (int)UserTypeStandard.SysAdmin && idProfileType != (int)UserTypeStandard.Administrator )
                );
        }

        private Boolean HasTypePermission(Int32 loaderType, Int32 idProfileType)
        {
            switch (loaderType)
            {
                case (int)UserTypeStandard.SysAdmin:
                    return true;
                case (int)UserTypeStandard.Administrator:
                    return (idProfileType != (int)UserTypeStandard.SysAdmin && idProfileType != (int)UserTypeStandard.Administrator);
                case (int)UserTypeStandard.Administrative:
                    return (idProfileType != (int)UserTypeStandard.SysAdmin && idProfileType != (int)UserTypeStandard.Administrator && idProfileType != (int)UserTypeStandard.Administrative);
                default:
                    return false;
            }
        }
    }
}
