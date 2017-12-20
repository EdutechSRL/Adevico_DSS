
using lm.Comol.Core.DomainModel;
using System;

namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class dtoDisplayItemRepository
	{
		public RepositoryItemPermission Permission {get;set;}
        // MODIFICATO //
		public BaseCommunityFile File  {get;set;}
		public bool AvailableForAll  {get;set;}
		public bool Virtual  {get;set;}
        public string DisplayName { get; set; }

		public dtoDisplayItemRepository()
		{
			Permission = new RepositoryItemPermission();
		}

		public dtoDisplayItemRepository(BaseCommunityFile file, CoreModuleRepository oPermission, int CurrentUserID, bool isAvailableForAll)
		{
			AvailableForAll = isAvailableForAll;
            File = file;
			Permission = new RepositoryItemPermission();

			bool ItemOwner = false;
            if (file.Owner != null)
                ItemOwner = (file.Owner.Id == CurrentUserID);
            bool isMultimedia = (file.isFile && (file.RepositoryItemType != Repository.RepositoryItemType.FileStandard && file.RepositoryItemType != Repository.RepositoryItemType.None)); 
            if (file.isFile)
            {
                Permission.Play = (oPermission.Administration || oPermission.DownLoad) && isMultimedia ;
                Permission.Download = oPermission.Administration || (file.IsDownloadable && oPermission.DownLoad);
                Permission.ViewAdvancedStatistics = isMultimedia && (oPermission.Administration || oPermission.Edit || (oPermission.UploadFile && ItemOwner));
                Permission.ViewBaseStatistics = isMultimedia && !(oPermission.Administration || oPermission.Edit || (oPermission.UploadFile && ItemOwner));
                Permission.EditSettings = isMultimedia && (oPermission.Administration || oPermission.Edit || (oPermission.UploadFile && ItemOwner));
			} else {
                Permission.Download = false;
                Permission.Link = false;
                Permission.Play = false;
                Permission.ViewAdvancedStatistics = false;
                Permission.ViewBaseStatistics = false;
                Permission.EditSettings = false;
			}
            if ((file != null))
            {
                DisplayName = file.DisplayName;
			}
            Permission.ViewPermission = oPermission.Administration || oPermission.Edit || (oPermission.UploadFile && ItemOwner);
            Permission.Delete = oPermission.Administration || oPermission.Edit || (oPermission.DeleteMyFile && ItemOwner);
            Permission.UnDelete = false;
            Permission.VirtualDelete = false;
            Permission.Edit = oPermission.Administration || oPermission.Edit || (oPermission.UploadFile && ItemOwner);
            Permission.EditPermission = oPermission.Administration || oPermission.Edit || (oPermission.UploadFile && ItemOwner);
		}
	}
}