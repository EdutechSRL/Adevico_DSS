using lm.Comol.Core.DomainModel;
using System;
namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable(), CLSCompliant(true)]
    public class ModuleRepository
    {
        public const String UniqueCode = "SRVMATER";
        #region "Properties"
            public Boolean UploadFile { get; set; }
            public Boolean EditMyFiles { get; set; }    
            public Boolean DeleteMyFiles { get; set; }
            public Boolean ViewMyStatistics { get; set; }
            public Boolean ManageModulePermission { get; set; }
            public Boolean Administration { get; set; }
            public Boolean ManageItems { get; set; }
            public Boolean ViewItemsList { get; set; }
            public Boolean ViewRepository { get { return ViewItemsList | DownloadOrPlay | Administration | ManageItems | UploadFile | EditOthersFiles; } }
            public Boolean EditOthersFiles { get; set; }    
            public Boolean DownloadOrPlay { get; set; }
            public Boolean ViewStatistics { get; set; }
        #endregion
       
        public ModuleRepository(){}
        public static ModuleRepository CreatePortalmodule(int idProfileType)
        {
            Boolean admin = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator);
            Boolean baseAdmin = (admin || idProfileType == (int)UserTypeStandard.Administrative);
            Boolean isGenericUser = (idProfileType == (int)UserTypeStandard.Guest || idProfileType == (int)UserTypeStandard.PublicUser);
            ModuleRepository module = new ModuleRepository();
            module.UploadFile = baseAdmin;
            module.EditMyFiles = baseAdmin;
            module.DeleteMyFiles = baseAdmin;
            module.Administration = admin;
            module.ViewMyStatistics = !isGenericUser;
            module.ViewItemsList = !isGenericUser;
            module.DownloadOrPlay = !isGenericUser;;
            module.EditOthersFiles = admin;
            module.ViewStatistics = admin;
            module.ManageModulePermission = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator);
            module.ManageItems = admin;
            return module;
        }
        public static ModuleRepository CreateForSelf()
        {
            ModuleRepository module = new ModuleRepository();
            module.UploadFile = true;
            module.EditMyFiles = true;
            module.DeleteMyFiles = true;
            module.Administration = true;
            module.ViewMyStatistics = true;
            module.ViewItemsList = true;
            module.DownloadOrPlay = true;
            module.EditOthersFiles = true;
            module.ViewStatistics = true;
            module.ManageModulePermission = true;
            module.ManageItems = true;
            return module;
        }
        public ModuleRepository(long permission)
        {
            UploadFile = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Upload, permission);
            EditMyFiles = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditMyFiles, permission);
            DeleteMyFiles = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteMyFiles, permission);
            ViewMyStatistics = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewMyStatistics, permission);
            ManageModulePermission = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.ManageModulePermission), permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration, permission);
            ManageItems = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.Manage), permission);
            ViewItemsList = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.Manage | (long)Base2Permission.ViewList | (long)Base2Permission.DownloadOrPlay), permission);
            EditOthersFiles = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.EditOthersFiles | (long)Base2Permission.Manage), permission);
            DownloadOrPlay = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.DownloadOrPlay), permission);
            ViewStatistics = PermissionHelper.CheckPermissionSoft(((long)Base2Permission.Administration | (long)Base2Permission.ViewStatistics), permission);
        }
        public long GetPermissions()
        {
            long permission = 0;
            if (DownloadOrPlay)
                permission = permission | (long)Base2Permission.DownloadOrPlay;
            if (UploadFile)
                permission = permission | (long)Base2Permission.Upload;
            if (EditMyFiles)
                permission = permission | (long)Base2Permission.EditMyFiles;
            if (DeleteMyFiles)
                permission = permission | (long)Base2Permission.DeleteMyFiles;
            if (ManageItems)
                permission = permission | (long)Base2Permission.Manage;
            if (Administration)
                permission = permission | (long)Base2Permission.Administration;
            if (EditOthersFiles)
                permission = permission | (long)Base2Permission.EditOthersFiles;
            if (ViewStatistics)
                permission = permission | (long)Base2Permission.ViewStatistics;
            if (ViewMyStatistics)
                permission = permission | (long)Base2Permission.ViewMyStatistics;
            if (ViewItemsList)
                permission = permission | (long)Base2Permission.ViewList;
            if (ManageModulePermission)
                permission = permission | (long)Base2Permission.ManageModulePermission;
            return permission;
        }

        public static ObjectType GetObjectType(ItemType type)
        {
            switch (type)
            {
                case ItemType.File:
                    return ObjectType.File;
                case ItemType.Folder:
                    return ObjectType.Folder;
                case ItemType.Link:
                    return ObjectType.Url;
                case ItemType.Multimedia:
                    return ObjectType.Multimedia;
                case ItemType.ScormPackage:
                    return ObjectType.ScormPackage;
                case ItemType.SharedDocument:
                    return ObjectType.SharedDocument;
                case ItemType.VideoStreaming:
                    return ObjectType.VideoStreaming;
                default:
                    return ObjectType.File;
            }
        }

        [Flags(), Serializable()]
        public enum Base2Permission : long
        {
            DownloadOrPlay = 1,
            //0
            Upload = 2,
            //1
            EditMyFiles = 4,
            //2
            DeleteMyFiles = 8,
            //3
            Manage = 16,
            //4
            ManageModulePermission = 32,
            //5
            Administration = 64,
            //6
            //PrintList = 2048,
            //11
            ViewStatistics = 4096,
            ViewList = 1024,
            ViewMyStatistics = 256,
            EditOthersFiles = 8192
        }
        [Serializable()]
        public enum ActionType
        {
            None = 70000,
            NoPermission = 70001,
            GenericError = 70002,
            AddFolder = 70006,
            AddFile = 70007,
            DownloadFile = 70008,
            PlayFile = 70009,
            DeleteFolder = 70018,
            DeleteFile = 70020,
            ShowItem = 70021,
            HideItem = 70022,
            ShowFolder = 70023,
            HideFolder = 70024,
            LoadRepository = 70100,
            UnableToLoadRepository = 70101,
            UnknownItemFound = 70102,
            UnavailableItem = 70103,
            UnableToHide = 70104,
            UnableToShow = 70105,
            VirtualDeleteItem = 70106,
            UndeleteItem = 70107,
            PhisicalDeleteItem = 70108,
            UnableToVirtualDeleteItem = 70109,
            UnableToUndeleteItem = 70110,
            UnableToPhisicalDeleteItem = 70111,
            TryToMoveItem = 70112,
            UnableToTryToMoveItem = 70113,
            UnknownItemsFound = 70114,
            UnableToFindItems = 70115,
            NotMovedItem = 70116,
            NotMovedItems = 70117,
            MovedItem = 70118,
            MovedItems = 70119,
            UnavailableSpaceOnFolder = 70120,
            UnableToAddFolder = 70121,
            AddLink = 70122,
            UnableToAddLink = 70123,
            UnableToAddFile = 70124,
            UnableToAddSomeFile = 70125,
            UnavailableItems = 70126,
            ShowItems = 70127,
            HideItems = 70128,
            VirtualDeleteItems = 70129,
            UndeleteItems = 70130,
            PhisicalDeleteItems = 70131,
            UnableToVirtualDeleteItems = 70132,
            UnableToUndeleteItems = 70133,
            UnableToPhisicalDeleteItems = 70134,
            VersionUnableToAdd = 70135,
            VersionAddedToFile = 70136,
            VersionAddingToFile = 70137,
            InLineItemDetailsLoaded = 70138,
            InLineItemDetailsTryView = 70139,
            EditDetailsTryToLoad = 70140,
            EditDetailsLoaded = 70142,
            EditDetailsNoPermissions = 70144,
            ViewDetailsTryToLoad = 70141,
            ViewDetailsLoaded = 70143,
            ViewDetailsNoPermissions = 70145,
            ItemSavedDetails = 70146,
            ItemSavedSomeDetails = 70147,
            ItemTryToSaveDetails = 70148,
            ThumbnailAdded = 70149,
            ThumbnailRemoved = 70150,
            ThumbnailUnableToAdd = 70151,
            ThumbnailUnableToRemove = 70152,
            ThumbnailNoPermissionToAdd = 70153,
            ThumbnailNoPermissionToRemove = 70154,
            VersionUnableToVirtualDelete = 70155,
            VersionVirtualDeleted = 70156,
            VersionUnableToVirtualUndelete = 70157,
            VersionVirtualUndeleted = 70158,
            VersionUnableToPhisicalDelete = 70159,
            VersionPhisicalDeleted = 70160,
            VersionSetAsActive = 70161,
            VersionUnableToSetAsActive = 70162,
            PermissionsNothingToSave = 70163,
            PermissionsSaved = 70164,
            PermissionsNotSaved = 70165,
            MultimedaSettingsTryToLoad = 70166,
            MultimedaSettingsLoaded = 70167,
            MultimedaSettingsNoPermissions = 70168,
            MultimedaSettingsInvalidType = 70169,
            MultimedaSettingsStatusError = 70170,
            MultimedaSettingsUnableToSetDefaultDocument = 70171,
            MultimedaSettingsSetDefaultDocument = 70172,
            ScormSettingsTryToLoad = 70173,
            ScormSettingsLoaded = 70174,
            ScormSettingsNoPermissions = 70175,
            ScormSettingsInvalidType = 70176,
            ScormSettingsStatusError = 70177,
            ScormSettingsWaiting = 70178,
            ScormSettingsUnableToSave = 70179,
            ScormSettingsSaved = 70180,
            DeleteScormPackage = 70181,
            DeleteMultimedia = 70182,
            DeleteLink = 70183,
            DeleteSharedDocument = 70184,
            DeleteVideoStreaming = 70185,
            ShowScormPackage = 70186,
            ShowMultimedia = 70187,
            ShowLink = 70188,
            ShowSharedDocument = 70189,
            ShowVideoStreaming = 70190,
            HideScormPackage = 70191,
            HideMultimedia = 70192,
            HideLink = 70193,
            HideSharedDocument = 70194,
            HideVideoStreaming = 70195,
            VirtualDeleteFile = 70196,
            VirtualDeleteFolder = 70197,
            VirtualDeleteMultimedia = 70198,
            VirtualDeleteScormPackage = 70199,
            VirtualDeleteLink = 70200,
            VirtualDeleteSharedDocument = 70201,
            VirtualDeleteVideoStreaming = 70202,
            VirtualUndeleteFile = 70203,
            VirtualUndeleteFolder = 70204,
            VirtualUndeleteMultimedia = 70205,
            VirtualUndeleteScormPackage = 70206,
            VirtualUndeleteLink = 70207,
            VirtualUndeleteSharedDocument = 70208,
            VirtualUndeleteVideoStreaming = 70209,
            ScormSettingsView = 70210,
            AddInternalFile = 70211,
        }

        [Serializable()]
        public enum OldActionType
        {
            ListForDownload = 70003,
            ListForAdmin = 70004,
            Statistics = 70005,
            CreateFolder = 70006,
            UploadFile = 70007,
         
            Details = 70010,
            MoveFile = 70011,
            MoveFolder = 70012,
            ChangeInfo = 70013,
            StartOtherUpload = 70014,
            EndOtherUpload = 70015,
            StartUpload = 70016,
            EndUpload = 70017,
            DeleteFolder = 70018,
            AjaxUpdate = 70019,
            DeleteFile = 70020,
            ShowFile = 70021,
            HideFile = 70022,
            ShowFolder = 70023,
            HideFolder = 70024,
            ImportFiles = 70025,
            DeleteMultiple = 70026,
            ImportFolders = 70027,
            ViewDeleteMultiplePage = 70028,
            ViewImportFoldersPage = 70029,
            ImportItemsCompleted = 70030,
            FolderPermissionToSome = 70031,
            FolderPermissionToCommunity = 70032,
            FolderPermissionModifyed = 70033,
            FilePermissionToCommunity = 70034,
            FilePermissionToSome = 70035,
            FilePermissionModifyed = 70036,
            FolderEditing = 70037,
            FolderEdited = 70038,
            FileEditing = 70039,
            FileEdited = 70040,
            FolderEditingPermission = 70041,
            FileEditingPermission = 70042,
            ScormEditMetadata = 70043,
            ScormEditMetadataError = 70044,
            NoPermissionToViewItemPersonalStatistics = 70045,
            NoPermissionToViewItemAdvancedStatistics = 70046,
            ViewFolder = 70047,
            EditFileSettings = 70048
        }
        [Serializable()]
        public enum ObjectType
        {
            None = 0,
            File = 1,
            ScormPackage = 2,
            Folder = 3,
            VideoStreaming = 4,
            Multimedia = 5,
            Url= 6,
            SharedDocument = 7,
            VersionItem = 8
        }
    }
}