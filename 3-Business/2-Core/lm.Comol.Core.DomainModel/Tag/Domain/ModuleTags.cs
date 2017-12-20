using System;
using System.Linq;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class ModuleTags
    {
        public const String UniqueCode = "SRVTAGS";
        public virtual Boolean List { get; set; }
        public virtual Boolean Add { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean DeleteOther { get; set; }
        public virtual Boolean DeleteMy { get; set; }
        public virtual Boolean Administration { get; set; }
        public virtual Boolean ManageModulePermission { get; set; }
        public ModuleTags()
        {
        }
        public static ModuleTags CreatePortalmodule(int idProfileType)
        {
            Boolean admin = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator);
            Boolean baseAdmin = (admin || idProfileType == (int)UserTypeStandard.Administrative);
            ModuleTags module = new ModuleTags();
            module.List = baseAdmin;
            module.Add = admin;
            module.Administration = admin;
            module.Edit = baseAdmin;
            module.DeleteMy = baseAdmin;
            module.DeleteOther = admin;
            module.ManageModulePermission = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator);
            return module;
        }

        public ModuleTags(long permission)
        {
            List = PermissionHelper.CheckPermissionSoft((long)Base2Permission.List, permission);
            Add = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add, permission);
            Edit = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Edit, permission);
            DeleteOther = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.DeleteOther), permission);
            DeleteMy = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add, permission);
            Administration = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.Manage), permission);
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
            if (Administration)
                permission = permission | (long)Base2Permission.Administration;
            if (DeleteOther)
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
            Manage = 16,
            GrantPermission = 32,
            Administration = 64
        }

        [Serializable]
        public enum ActionType 
        {
            None = 93500,
            NoPermission = 93501,
            GenericError = 93502,
            SessionTimeout= 93503,
            MalformedUrl = 93504,
            PortalListTags = 93505,
            OrganizationListTags = 93506,
            UnknownTag = 93507,
            PortalStartAddNewTag = 93508,
            OrganizationStartAddNewTag = 93509,
            EnableTag = 93510,
            DisableTag = 93511,
            UnableToEnableTag = 93512,
            UnableToDisableTag = 93513,
            VirtualDelete = 93514,
            VirtualUndelete = 93515,
            UnableToDelete = 93516,
            UnableToUndelete = 93517,
            UnableToAddBulkTagsToPortal = 93518,
            UnableToAddBulkTagsToOrganization = 93519,
            AddedBulkTagsToPortal = 93520,
            AddedBulkTagsToOrganization = 93521,
            AddedBulkTagsToPortalWithDuplicates = 93522,
            AddedBulkTagsToOrganizationWithDuplicates = 93523,
            BulkTagsAssignLoad = 93524,
            NoPermissionForBulkTagsAssign = 93525,
            BulkTagsAssignToCommunities = 93526,
            BulkTagsAssignApplyFilters = 93527,
            BulkTagsAssignChangePage = 93528,
            BulkTagsAssigned = 93529,
            BulkTagsUnasigned = 93530,
            BulkTagsNoSelection = 93531,
            BulkTagsUpdateListAferTagsAssignment = 93532,
            BulkTagsNoCommunitiesFound = 93533,
            //SaveVersionSettings = 75008,
            //AddNotificationSetting = 75009,
            //VirtualDeleteNotificationSetting = 75010,
            //SaveModuleContent = 75011,
            //SaveModuleContentWithPlaceHolderReplace = 75012,
            //AddNewTemplate = 75013,
            //TryToAddNewTemplate = 75014,
            //StartEditingSettings = 75015,
            //TryToEditingSettings = 75016,
            //ErrorSavingModuleContent= 75017,
            //ErrorSavingNotificationSettings= 75018,
            //ErrorSavingSettings= 75019,
            //VirtualDeleteTranslation = 75020,
            //ErrorDeletingTranslation = 75021,
            //TranslationSaved = 75022,
            //ErrorSavingTranslation = 75023,
            //DisplayPermissions = 75024,
            //StartEditingPermissions = 75025,
            //TryToDisplayPermissions = 75026,
            //TryToStartEditingPermissions = 75027,
            //PortalPermissionsSaved = 75028,
            //PortalPermissionsSavingErrors = 75029,
            //CommunityPermissionsSaved = 75030,
            //CommunityPermissionsSavingErrors = 75031,
            //PersonPermissionsSaved = 75032,
            //PersonPermissionsSavingErrors = 75033,
            //AddedPersonPermissions = 75034,
            //AddingPersonPermissionsErrors = 75035,
            //PermissionsSaved = 75036,
            //PermissionsSavingErrors = 75037,
            //ListTemplates = 75038,
            //VirtualDeleteTemplate = 75039,
            //VirtualUnDeleteTemplate = 75040,
            //PhisicalDeleteTemplate = 75041,
            //VirtualDeleteTemplateVersion = 75042,
            //VirtualUnDeleteTemplateVersion = 75043,
            //PhisicalDeleteTemplateVersion = 75044,
            //CloneTemplate = 75045,
            //AddNewTemplateVersion = 75046,
            //NotificationSettingsView = 75047,
            //NotificationSettingsSave = 75048,
            //NotificationSettingsEditing = 75049
        }
        [Serializable]
        public enum ObjectType
        {
            None = 0,
            Tag = 1,
            TagTranslation = 2,
            CommunityTag = 3,
            MyTile = 4,
        }
    }
}