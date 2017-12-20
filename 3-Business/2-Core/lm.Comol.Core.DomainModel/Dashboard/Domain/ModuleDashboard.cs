using System;
using System.Linq;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Dashboard.Domain
{
    [Serializable]
    public class ModuleDashboard
    {
        public const String UniqueCode = "SRVDSHBOARD";
        public virtual Boolean List { get; set; }
        public virtual Boolean Add { get; set; }
        public virtual Boolean Edit { get; set; }
        public virtual Boolean Clone { get; set; }
        public virtual Boolean DeleteOther { get; set; }
        public virtual Boolean DeleteMy { get; set; }
        public virtual Boolean Administration { get; set; }
        public virtual Boolean ManageTiles { get; set; }
        public virtual Boolean ManageModulePermission { get; set; }
        public ModuleDashboard()
        {
        }
        public static ModuleDashboard CreatePortalmodule(int idProfileType)
        {
            Boolean admin = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator);
            Boolean baseAdmin = (admin || idProfileType == (int)UserTypeStandard.Administrative);
            ModuleDashboard module = new ModuleDashboard();
            module.List = baseAdmin;
            module.Add = admin;
            module.Administration = admin;
            module.Edit = baseAdmin;
            module.Clone = baseAdmin;
            module.DeleteMy = baseAdmin;
            module.DeleteOther = admin;
            module.ManageTiles = admin;
            module.ManageModulePermission = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator);
            return module;
        }

        public ModuleDashboard(long permission)
        {
            List = PermissionHelper.CheckPermissionSoft((long)Base2Permission.List, permission);
            Add = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add, permission);
            Edit = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Edit, permission);
            Clone = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Clone, permission);
            DeleteOther = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.DeleteOther), permission);
            DeleteMy = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add, permission);
            Administration = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration), permission);
            ManageTiles = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.ManageTiles), permission);
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
            if (ManageTiles)
                permission = permission | (long)Base2Permission.ManageTiles;
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
            ManageTiles = 16,
            GrantPermission = 32,
            Administration = 64,
            Clone = 2
        }

        [Serializable]
        public enum ActionType 
        {
            None = 93000,
            NoPermission = 93001,
            GenericError = 93002,
            SessionTimeout= 93003,
            MalformedUrl = 93004,
            ListDashboardLoad = 93005,
            CombinedDashboardLoad = 93006,
            TileDashboardLoad = 93007,
            SearchDashboardLoad = 93008, 
            ListDashboardLoadCommunities = 93009,
            ListDashboardMoreComminities = 93010,
            ListDashboardLessComminities = 93011,
            ListDashboardLoadSubscribedCommunities = 93012,
            CombinedDashboardMoreTiles = 93013,
            CombinedDashboardLessTiles = 93014,
            CombinedDashboardMoreCommunities = 93015,
            CombinedDashboardLessCommunities = 93016,
            TileDashboardMoreTiles = 93017,
            TileDashboardLessTiles = 93018,
            SearchSimpleDashboardLoad = 93019,
            SearchAdvancedDashboardLoad = 93020,
            SearchDashboardApplyFilters = 93021,
            SearchDashboardLoadcommunities = 93022,
            SearchDashboardChangePageIndex = 93023,
            UnsubscribeFromCommunity = 93024,
            UnsubscribeFromCommunities = 93025,
            UnableToUnsubscribeFromCommunity = 93026,
            UnableToUnsubscribeFromCommunities = 93027,
            UnableToUnsubscribe = 93028,
            UnsubscribeNotallowed = 93029,
            RequireUnsubscribeConfirm = 93030,
            RequireUnsubscribeConfirmFromSubCommunities = 93031,
            ListDashboard = 93055,
            UnknownDashboard = 93056,
            StartAddNewDashboard = 93057,

            TilesPortalDashboardList = 93058,
            TilesAllCommunitiesList = 93059,
            TilesCommunityList = 93060,
            TileUnknown = 93061,
            TilePortalStartAdding = 93062,
            TileAllCommunitiesStartAdding = 93063,
            TileCommunityStartAdding = 93064,
            TileEnable = 93065,
            TileDisable = 93066,
            TileUnableToEnable = 93067,
            TileUnableToDisable = 93068,
            TileVirtualDelete = 93069,
            TileVirtualUndelete = 93070,
            TileUnableToVirtualDelete = 93071,
            TileUnableToUndelete = 93072,
            TileAutoGenerateForCommunityTypes = 93073,
            TileUnableAutoGenerateForCommunityTypes = 93074,
            TileAlreadyGeneratedForCommunityTypes = 93075,
            TileStartAdding = 93076,
            TileStartEditing = 93077,
            TileImageInvalid = 93078,
            TileImageNotUploaded = 93079,
            TileImageUploaded = 93080,
            TileImageAssigned = 93081,
            TileAdded = 93082,
            TileSaved = 93083,
            TileUnableToAdd = 93084,
            TileUnableToSave = 93085,
            TileView = 93086,

            DashboardSettingsList = 93087,
            DashboardSettingsPortalList = 93088,
            DashboardSettingsAllCommunitiesList = 93089,
            DashboardSettingsCommunityList = 93090,
            DashboardSettingsPortalStartAdding = 93091,
            DashboardSettingsAllCommunitiesStartAdding = 93092,
            DashboardSettingsCommunityStartAdding = 93093,
            DashboardSettingsEnable = 93094,
            DashboardSettingsDisable = 93095,
            DashboardSettingsUnableToEnable = 93096,
            DashboardSettingsUnableToDisable = 93097,
            DashboardSettingsVirtualDelete = 93098,
            DashboardSettingsVirtualUndelete = 93099,
            DashboardSettingsUnableToVirtualDelete = 93100,
            DashboardSettingsUnableToUndelete = 93101,
            DashboardSettingsStartAdding = 93102,
            DashboardSettingsStartEditing = 93103,
            DashboardSettingsAdded = 93104,
            DashboardSettingsSaved = 93105,
            DashboardSettingsUnableToAdd = 93106,
            DashboardSettingsUnableToSave = 93107,
            DashboardSettingsView = 93108,
            DashboardSettingsClone= 93109,
            DashboardSettingsUnableToClone = 93110,
            DashboardSettingsViewsStartEditing = 93111,
            DashboardSettingsViewsSaved = 93112,
            DashboardSettingsViewsUnableToSave = 93113,
            DashboardSettingsTilesStartReorder = 93114,
            DashboardSettingsTilesOrderSaved = 93115,
            DashboardSettingsTilesOrderUnableToSave = 93116,
            EnrollPageLoad = 93117,
            EnrollPageLoadWithCommunities = 93118,
            EnrollPageApplyFilters = 93119,
            EnrollPageLoadcommunities = 93120,
            EnrollPageChangePageIndex = 93121,
            EnrollPageLoadcommunitiesAfterEnrolling = 93122,
            EnrollToCommunity = 93123,
            EnrollToCommunities = 93124,
            UnableToEnrollToCommunity = 93125,
            UnableToEnrollToCommunitiesCommunities = 93126,
            UnableToEnroll = 93127,
            EnrollNotAllowed = 93128,
            RequireEnrollConfirm = 93129,
            EnrollToCommunityWaitingConfirm = 93130,
            EnrollToCommunitiesWaitingConfirm = 93131,
            NoSelectedCommunitiesToEnroll = 93132,
            LoadingCommunityDetails = 93133,
            LoadingUnknownCommunityDetails = 93134,
            TreePageLoading = 93135,
            TreePageLoadingFromCommunity = 93136,
            TreePageLoadingFromUnknownCommunity = 93137,
            TreeUnableToLoad = 93138,
            TreeNoChildrenToLoad = 93139,
            TreeLoad = 93140,
            TreeLoadChildren = 93141,
            //SearchDashboard = 93121,
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
            Dashboard = 1,
            PageSettings = 2,
            ContainerSettings = 3,
            DefaultSettings = 4,
            DashboardAssignment = 5,
            Tile = 6,
            TileTranslation = 7,
            TileItem = 8,
            TileAssignment = 9,
            TileTagAssignment = 10,
            Community = 11
        }
    }
}