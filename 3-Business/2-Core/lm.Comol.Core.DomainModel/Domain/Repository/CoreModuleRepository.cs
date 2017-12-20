using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CoreModuleRepository
	{
		public const string UniqueID = "SRVMATER";
		public bool UploadFile {get;set;}
		public bool DeleteMyFile {get;set;}
		public bool ManagementPermission{get;set;}
		public bool Administration {get;set;}
		public bool ListFiles{get;set;}
		public bool Edit {get;set;}
        public bool DownLoad { get; set; }


		public CoreModuleRepository()
		{
		}
		public CoreModuleRepository(string permissionValue)
		{
			if ((string.IsNullOrEmpty(permissionValue))) {
				CreateFromModuleLongValue(Convert.ToInt64(0));
			} else {
                Char [] permissionArray = permissionValue.ToCharArray();
                Array.Reverse(permissionArray);
                CreateFromModuleLongValue(Convert.ToInt64(new string(permissionArray), 2));
			}
		}
		public CoreModuleRepository(long permission)
		{
			CreateFromModuleLongValue(permission);
		}
		private void CreateFromModuleLongValue(long permission)
		{
            this.UploadFile = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AdminService, permission) | PermissionHelper.CheckPermissionSoft((long)Base2Permission.Moderate, permission) | PermissionHelper.CheckPermissionSoft((long)Base2Permission.UploadFile, permission);
            this.UploadFile = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AdminService | (long)Base2Permission.Moderate | (long)Base2Permission.UploadFile, permission);
            this.Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AdminService, permission);
            this.DeleteMyFile = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteFile, permission);
            this.DownLoad = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AdminService | (long)Base2Permission.Moderate | (long)Base2Permission.DownloadFile, permission);
            this.Edit = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditFile, permission);
            this.ManagementPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.GrantPermission, permission);
            this.ListFiles = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DownloadFile, permission);
		}

		[Flags(), Serializable()]
		public enum Base2Permission : long
		{
			DownloadFile = 1,
			//0
			UploadFile = 2,
			//1
			EditFile = 4,
			//2
			DeleteFile = 8,
			//3
			Moderate = 16,
			//4
			GrantPermission = 32,
			//5
			AdminService = 64,
			//6
			PrintList = 2048,
			//11
			ChangeFileOwner = 4096
			//11
		}
		[Serializable()]
		public enum ActionType
		{
			None = 0,
			NoPermission = 1,
			GenericError = 2,
			ListForDownload = 70003,
			ListForAdmin = 70004,
			Statistics = 70005,
			CreateFolder = 70006,
			UploadFile = 70007,
			DownloadFile = 70008,
			PlayFile = 70009,
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
            Multimedia = 5
		}
		public static CoreModuleRepository CreatePortalmodule(int UserTypeID)
		{
			CoreModuleRepository oService = new CoreModuleRepository();
			{
				oService.Administration = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative || UserTypeID == (int)UserTypeStandard.SysAdmin);
				oService.DeleteMyFile = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative || UserTypeID == (int)UserTypeStandard.SysAdmin);
				oService.DownLoad = (UserTypeID != (int)UserTypeStandard.Guest);
				oService.Edit = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative || UserTypeID == (int)UserTypeStandard.SysAdmin);
				oService.ListFiles = (UserTypeID != (int)UserTypeStandard.Guest);
				oService.ManagementPermission = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.SysAdmin);
				oService.UploadFile = (UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative || UserTypeID == (int)UserTypeStandard.SysAdmin);
			}
			return oService;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik, @toddanglin
//Facebook: facebook.com/telerik
//=======================================================
