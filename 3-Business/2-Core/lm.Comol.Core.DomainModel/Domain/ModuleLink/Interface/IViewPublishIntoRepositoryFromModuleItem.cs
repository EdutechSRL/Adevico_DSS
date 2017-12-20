using System;
namespace lm.Comol.Core.DomainModel.Common
{
	[CLSCompliant(true)]
	public interface IViewPublishIntoRepositoryFromModuleItem : lm.Comol.Core.DomainModel.Common.iDomainView
	{

		bool AllowCommunityPublish { set; }

		string BaseFolder { get; }
		void Initialize();


		//Person SelectedFolder() As Long
		//Person SelectedCommunityID() As Integer
		//ReadOnly Person SelectedFolderName() As String
		//ReadOnly Person CommunitySelectionLoaded() As Boolean


		//Sub SetBackToManagementUrl(ByVal IdCommunity As Integer, ByVal IdItem As T)
		//Sub ReturnToFileManagement(ByVal IdCommunity As Integer, ByVal IdItem As T)


		//Sub NoPermissionToManagementFiles()
		//Sub NoPermissionToPublishFiles()
		//Sub NoPermissionToAccessPage(ByVal IdCommunity As Integer, ByVal IdModule As Integer)

		//Sub LoadDiaryItemFiles(ByVal o As List(Of GenericFilterItem(Of System.Guid, BaseFile)))
		//Sub LoadMultipleFileName(ByVal oList As List(Of dtoFileExist(Of System.Guid)))
		//Person SelectedFilesID() As List(Of System.Guid)
		//ReadOnly Person SelectedFiles() As List(Of GenericFilterItem(Of System.Guid, String))


		//Sub InitCommunitySelection()
		//Sub InitializeFolderSelector(ByVal IdCommunity As Integer, ByVal selectedFolderID As Long, ByVal ShowAllFolder As Boolean)
		//Sub ShowFoldersList()
		//Sub ShowSelectCommunity()
		//Sub ShowFileList()
		//Sub ShowCompleteMessage()
		//Sub ShowRenamedFileList()

		//Function GetChangedFileName() As List(Of dtoFileExist(Of System.Guid))

		//Sub SetFolderInfo(ByVal CommunityName As String)
		//Sub SendActionInit(ByVal IdCommunity As Integer, ByVal ModuleID As Integer, ByVal IdItem As T)
		//Sub Notify(ByVal IdCommunity As Integer, ByVal ModuleID As Integer, ByVal oItem As CommunityFile, ByVal FolderName As String)
	}
}