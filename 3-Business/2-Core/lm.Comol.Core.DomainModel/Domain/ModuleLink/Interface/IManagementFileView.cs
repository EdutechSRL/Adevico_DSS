using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel.Common
{
	[CLSCompliant(true)]
	public interface IManagementFileView<T> : iDomainView
	{

		T PreloadedItemID { get; }
		bool AllowPublish { get; set; }
		bool AllowCommunityUpload { set; }
		bool AllowCommunityLink { set; }
		bool AllowUpload { set; }
		CoreModuleRepository RepositoryPermission(int CommunityID);
		//   WriteOnly Person BackToItem() As T
		T ItemID { get; set; }
		int ItemCommunityId { get; set; }
		void SetBackToItemUrl(int CommunityID, T ItemID);
		void SetMultipleUploadUrl(T ItemID);
		void ReturnToItemsList(int CommunityID, T ItemID);

		void NoPermissionToManagementFiles(int CommunityID, int ModuleID);
		void InitializeCommunityUploader(long FolderID, int CommunityID, CoreModuleRepository oPermission);
		void InitializeModuleUploader(int CommunityID);
		void AddManagementFileAction(int CommunityID, int ModuleID);
		void AddModuleFileAction(int CommunityID, int ModuleID);

		void AddCommunityFileAction(int CommunityID, int ModuleID);
		ModuleActionLink GetUploadedModuleFile(object item, int itemTypeId, string moduleCode, int moduleOwnerActionID, int moduleId);
		void ReturnToFileManagement(int CommunityID, T ItemID);

		void ReturnToFileManagementWithErrors(List<dtoModuleUploadedFile> NUinternalFile, List<dtoUploadedFile> NUcommunityFile);
		void LoadFilesToManage(T ItemID, CoreItemPermission oPermission, IList<iCoreItemFileLink<T>> files, string urlToPublish);
		void LoadEditingPermission(long fileId, int communityId, long folderId, T itemID);
	}
}