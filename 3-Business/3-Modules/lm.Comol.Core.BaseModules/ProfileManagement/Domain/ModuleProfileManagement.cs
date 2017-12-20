using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class ModuleProfileManagement
    {
        public const string UniqueID = "SRVPRFMNG";
        public virtual Boolean ViewProfiles {get;set;}
        public virtual Boolean AddProfile { get; set; }
        public virtual Boolean EditProfile {get;set;}
        public virtual Boolean RenewPassword { get; set; }
        public virtual Boolean DeleteProfile { get; set; }
        public virtual Boolean Administration {get;set;}
        public virtual Boolean AddAuthenticationProviderToProfile { get; set; }
        public virtual Boolean ViewProfileDetails {get;set;}
        public ModuleProfileManagement() {
         
        }

        public static ModuleProfileManagement CreatePortalmodule(int UserTypeID)
        {
            ModuleProfileManagement module = new ModuleProfileManagement();
            module.ViewProfiles = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.AddProfile = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.EditProfile = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.RenewPassword = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.DeleteProfile = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.RenewPassword = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.AddAuthenticationProviderToProfile = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.ViewProfileDetails = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            return module;
        }
        public lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages ToTemplateModule()
        {
            lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages m = new lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(UniqueID);
            m.Add = Administration;
            m.Administration = Administration;
            m.Clone = Administration;
            m.DeleteMyTemplates = Administration;
            m.DeleteOtherTemplates = Administration;
            m.Edit = Administration;
            m.List = Administration;
            m.SendMessage = Administration;
            m.ManageModulePermission = Administration;
            return m;
        }
        public static List<ActionType> GetNotificationActions() {
            List<ActionType> items = new List<ActionType>();
            items.Add(ActionType.SetPassword);
            items.Add(ActionType.RetrievePassword);
            items.Add(ActionType.ExpiredPassword);
            items.Add(ActionType.ExpiringPassword);
            items.Add(ActionType.MyProfileCreated);
            items.Add(ActionType.CreateNewPassword);

            return items;
        }

        //public ModuleProfileManagement(long permission)
        //{
        //    ViewDiaryItems = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewLessons | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
        //    Edit = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
        //    PrintList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewLessons | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
        //    ManagementPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.GrantPermission , permission);
        //    UploadFile = PermissionHelper.CheckPermissionSoft((long)Base2Permission.UploadFile | (long)Base2Permission.AddLesson | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
        //    AddItem = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddLesson | (long)Base2Permission.AdminService, permission);
        //    Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AdminService, permission);
        //    DeleteItem  = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteLesson | (long) Base2Permission.AdminService , permission); 
        //}

        [Flags]
        public enum Base2Permission
        {
            ViewProfile = 1,
            EditProfile = 2,
            RenewPassword = 4,
            DeleteProfile = 8,
            GrantPermission = 32,
            AdminService = 64,
            AddProfile = 2048,
            AddProfileAuthenticationProvider = 8192
        }

        public enum ActionType{
            NoPermissions = 90000,
            ViewProfiles = 90001,
            EditProfile = 90002,
            VirtualDeleteProfile = 90003,
            VirtualUnDeleteProfile = 90004,
            PhisicalDeleteProfile = 90005,
            EditAtuhenticationInfo = 90006,
            EditLogin = 90007,
            SetPassword = 90008,
            RetrievePassword = 90009,
            CreateNewPassword = 90010,
            AddInternalProvider = 90011,
            ExpiredPassword = 90012,
            ExpiringPassword = 90013,
            DisabledProfileAutenticationProvider = 90014,
            EnabledProfileAutenticationProvider = 90015,
            ImportProfiles = 90016,
            OtherProfileCreated = 90017,
            MyProfileCreated = 90018,
         }

        public enum ObjectType{
            None = 0,
            Profile = 1,
            AuthenticationProvider = 2,
            ProfileLoginInfo = 3
        }
    }
}