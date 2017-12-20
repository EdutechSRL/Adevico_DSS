using System;
using System.Linq;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.TemplateMessages
{
    [Serializable]
    public class ModuleTemplateMessages
    {
        public const String UniqueCode = "SRVTMPMSG";
        public virtual Boolean List { get; set; }
        public virtual Boolean Add { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Clone { get; set; }
        public virtual Boolean DeleteOtherTemplates { get; set; }
        public virtual Boolean DeleteMyTemplates { get; set; }
        public virtual Boolean Administration { get; set; }
        public virtual Boolean ManageModulePermission { get; set; }
        public ModuleTemplateMessages()
        {
        }
        public static ModuleTemplateMessages CreatePortalmodule(int idProfileType,lm.Comol.Core.TemplateMessages.Domain.OwnerType type)
        {
            Boolean baseAdmin = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator || idProfileType == (int)UserTypeStandard.Administrative);
            Boolean basePermission = (idProfileType != (int)UserTypeStandard.TypingOffice || idProfileType != (int)UserTypeStandard.Guest);
            ModuleTemplateMessages module = new ModuleTemplateMessages();
            module.List = basePermission; // (basePermission && type == Domain.OwnerType.Person) || (baseAdmin);
            module.Add = (basePermission && type == Domain.OwnerType.Person) || (baseAdmin);
            module.Administration = (basePermission && type == Domain.OwnerType.Person) || (baseAdmin);
            module.Edit = (basePermission && type == Domain.OwnerType.Person) || (baseAdmin);
            module.Clone = (basePermission && type == Domain.OwnerType.Person) || (baseAdmin);
            module.DeleteMyTemplates = (basePermission && type == Domain.OwnerType.Person) || (baseAdmin);
            module.DeleteOtherTemplates = baseAdmin;
            module.ManageModulePermission = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator);
            return module;
        }

        public ModuleTemplateMessages(long permission)
        {
            List = PermissionHelper.CheckPermissionSoft((long)Base2Permission.List, permission);
            Add = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add, permission);
            Edit = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Edit, permission);
            Clone = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Clone, permission);
            DeleteOtherTemplates = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.DeleteOther), permission);
            DeleteMyTemplates = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add, permission);
            Administration = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.ManageTemplates), permission);
            ManageModulePermission = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.GrantPermission), permission);
        }
        public long GetPermissions()
        {
            long permission = 0;
            if (List)
                permission = permission | (long)Base2Permission.List;
            if (Add)
                permission = permission | (long)Base2Permission.Add;
            if (Edit)
                permission = permission | (long)Base2Permission.Edit;
            if (Clone)
                permission = permission | (long)Base2Permission.Clone;
            if (Administration)
                permission = permission | (long)Base2Permission.Administration;
            if (DeleteOtherTemplates)
                permission = permission | (long)Base2Permission.DeleteOther | (long)Base2Permission.Administration;
            /*if (DeleteMyTemplates)
                permission = permission || (long)Base2Permission.Add;*/
            if (ManageModulePermission)
                permission = permission | (long)Base2Permission.GrantPermission;
            return permission;
        }
        [Flags, Serializable]
        public enum Base2Permission
        {
            List = 1,
            Add = 8192,
            Edit = 4,
            DeleteOther = 8,
            ManageTemplates = 16,
            GrantPermission = 32,
            Administration = 64,
            Clone = 2
        }

        [Serializable]
        public enum ActionType 
        {
            None = 75000,
            NoPermission = 75001,
            GenericError = 75002,
            SessionTimeout= 75003,
            MalformedUrl = 75004,
            List = 75005,
            UnknownTemplateVersion = 75006,
            StartAddNewTemplate = 75007,
            SaveVersionSettings = 75008,
            AddNotificationSetting = 75009,
            VirtualDeleteNotificationSetting = 75010,
            SaveModuleContent = 75011,
            SaveModuleContentWithPlaceHolderReplace = 75012,
            AddNewTemplate = 75013,
            TryToAddNewTemplate = 75014,
            StartEditingSettings = 75015,
            TryToEditingSettings = 75016,
            ErrorSavingModuleContent= 75017,
            ErrorSavingNotificationSettings= 75018,
            ErrorSavingSettings= 75019,
            VirtualDeleteTranslation = 75020,
            ErrorDeletingTranslation = 75021,
            TranslationSaved = 75022,
            ErrorSavingTranslation = 75023,
            DisplayPermissions = 75024,
            StartEditingPermissions = 75025,
            TryToDisplayPermissions = 75026,
            TryToStartEditingPermissions = 75027,
            PortalPermissionsSaved = 75028,
            PortalPermissionsSavingErrors = 75029,
            CommunityPermissionsSaved = 75030,
            CommunityPermissionsSavingErrors = 75031,
            PersonPermissionsSaved = 75032,
            PersonPermissionsSavingErrors = 75033,
            AddedPersonPermissions = 75034,
            AddingPersonPermissionsErrors = 75035,
            PermissionsSaved = 75036,
            PermissionsSavingErrors = 75037,
            ListTemplates = 75038,
            VirtualDeleteTemplate = 75039,
            VirtualUnDeleteTemplate = 75040,
            PhisicalDeleteTemplate = 75041,
            VirtualDeleteTemplateVersion = 75042,
            VirtualUnDeleteTemplateVersion = 75043,
            PhisicalDeleteTemplateVersion = 75044,
            CloneTemplate = 75045,
            AddNewTemplateVersion = 75046,
            NotificationSettingsView = 75047,
            NotificationSettingsSave = 75048,
            NotificationSettingsEditing = 75049
        }
        [Serializable]
        public enum ObjectType
        {
            None = 0,
            Template = 1,
            Version = 2,
            ModuleContent = 3,
            Permission = 4,
            Translation = 5,
            NotificationSetting = 6,
        }
    }
}