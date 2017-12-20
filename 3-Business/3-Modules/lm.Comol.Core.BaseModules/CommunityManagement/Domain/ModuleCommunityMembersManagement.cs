using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Domain
{
    [Serializable]
    public class ModuleCommunityMembersManagement
    {
        public const string UniqueID = "SRVISCRITTI";
        
        public virtual Boolean AddUser { get; set; }
        public virtual Boolean EditSubscription { get; set; }
        public virtual Boolean DeleteSubscription { get; set; }
        public virtual Boolean Manage { get; set; }
        public virtual Boolean EditPermission { get; set; }
        public virtual Boolean Administration { get; set; }
        public virtual Boolean PrintList { get; set; }
        public virtual Boolean LoadList { get; set; }
        public virtual Boolean ViewExtendedInfo { get; set; }
        public ModuleCommunityMembersManagement() {
         
        }

        public static ModuleCommunityMembersManagement CreatePortalmodule(int UserTypeID)
        {
            ModuleCommunityMembersManagement module = new ModuleCommunityMembersManagement();
            module.AddUser = (UserTypeID == (int)UserTypeStandard.SysAdmin|| UserTypeID == (int)UserTypeStandard.Administrator|| UserTypeID == (int)UserTypeStandard.Administrative);
            module.EditSubscription = module.AddUser;
            module.DeleteSubscription = module.AddUser;
            module.Manage = module.AddUser;
            module.PrintList = module.AddUser;
            module.LoadList = module.AddUser;
            module.ViewExtendedInfo = module.AddUser;
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.EditPermission = (UserTypeID == (int)UserTypeStandard.SysAdmin);
            return module;
        }

        public ModuleCommunityMembersManagement(long permission)
        {
            AddUser = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddUser | (long)Base2Permission.Manage | (long)Base2Permission.Administration, permission);
            EditSubscription = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditSubscription | (long)Base2Permission.Manage | (long)Base2Permission.Administration, permission);
            DeleteSubscription = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteSubscription | (long)Base2Permission.Manage | (long)Base2Permission.Administration, permission);
            Manage = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Manage | (long)Base2Permission.Administration, permission);
            EditPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManagementPermission | (long)Base2Permission.Administration, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration, permission);
            PrintList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.PrintList | (long)Base2Permission.Manage | (long)Base2Permission.Administration, permission);
            LoadList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ListSubscriptions | (long)Base2Permission.Manage | (long)Base2Permission.Administration, permission);
            ViewExtendedInfo = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration | (long)Base2Permission.Manage, permission);
        }

        [Flags]
        public enum Base2Permission
        {
            ViewExtendedInfo = 1,// '0
            AddUser = 2,// '1
            EditSubscription = 4,// '2
            DeleteSubscription = 8,// '3
            Manage = 16,// '4
            ManagementPermission = 32,// '5
            Administration = 64,// '6
            PrintList = 2048,// '11
            ListSubscriptions = 1024,// '10
        }

        public enum ActionType{
            None = 0,
            NoPermission = 1,
            GenericError = 2,
            AddPerson = 79003,
            EditSubscription = 790004,
            DeleteSubscription = 79005,
            AcceptUser = 79006,
            BlockUser = 79007,
            ViewList = 79008,
            SearchUser = 79009,
            SelfSubscription = 79010,
            SelfWaitingSubscription = 79011,
            SelfUnSubscription = 79012
         }

        public enum ObjectType{
            None = 0,
            Person = 1,
            Role = 2
        }
    }
}