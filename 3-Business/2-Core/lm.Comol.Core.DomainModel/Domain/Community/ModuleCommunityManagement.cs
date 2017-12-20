using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Domain
{
    [Serializable]
    public class ModuleCommunityManagement
    {
        public const string UniqueID = "SRVADMCMNT";
        public virtual Boolean AddIstitutions { get; set; }
        public virtual Boolean AddOrganizations { get; set; }
        public virtual Boolean Add {get;set;}
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Delete {get;set;}
        public virtual Boolean Manage { get; set; }
        public virtual Boolean ManagementPermission { get; set; }
        public virtual Boolean Administration {get;set;}
        public ModuleCommunityManagement() {
         
        }


        public static ModuleCommunityManagement CreatePortalmodule(int UserTypeID)
        {
            ModuleCommunityManagement module = new ModuleCommunityManagement();
            module.AddIstitutions = (UserTypeID == (int)UserTypeStandard.SysAdmin);
            module.AddOrganizations = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.Manage = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.ManagementPermission = (UserTypeID == (int)UserTypeStandard.SysAdmin);
            return module;
        }

        public ModuleCommunityManagement(long permission)
        {
            Add = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add | (long)Base2Permission.Manage | (long)Base2Permission.AdminService, permission);
            Edit = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Edit | (long)Base2Permission.Manage | (long)Base2Permission.AdminService, permission);
            Delete = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Edit | (long)Base2Permission.Manage | (long)Base2Permission.AdminService, permission);
            ManagementPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.GrantPermission , permission);
            Manage = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Manage | (long)Base2Permission.AdminService, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AdminService, permission);
        }

        [Flags]
        public enum Base2Permission
        {
            Add = 2,
            Edit = 4,
            Delete = 8,
            Manage = 16,
            GrantPermission = 32,
            AdminService = 64,
        }

        public enum ActionType{
            None = 21000,
            NoPermission = 21001,
            GenericError = 21002,
			List = 21003,
			Admin = 21004,
			Access = 21005,
            ListToSubscribe = 21006,
            CreateCommunity = 21007,
            DeleteCommunity = 21008,
            EditCommunity = 21009,
            AdminSubCommunity = 21010
         }

        public enum ObjectType{
            None = 0,
            Community = 1
        }
    }
}