Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IWKpublishFileToCommunity
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter
        ReadOnly Property PreloadedItemID() As System.Guid
        ReadOnly Property PreloadedFileID() As System.Guid
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        WriteOnly Property AllowCommunityPublish() As Boolean
        WriteOnly Property BackToManagement() As System.Guid
        ReadOnly Property BaseFolder() As String

        Sub NoPermissionToManagementFiles()
        Sub NoPermissionToPublishFiles()
        Sub NoPermissionToAccessPage(ByVal CommunityID As Integer, ByVal ModuleID As Integer)

        Sub LoadWorkbookFiles(ByVal o As List(Of GenericFilterItem(Of System.Guid, BaseFile)))
        Sub LoadMultipleFileName(ByVal oList As List(Of dtoFileExist(Of System.Guid)))
        Sub LoadSummary(ByVal CommunityName As String, ByVal FolderName As String, ByVal oFiles As List(Of dtoFileToPublish))
        Property SelectedFilesID() As List(Of System.Guid)
        ReadOnly Property SelectedFiles() As List(Of GenericFilterItem(Of System.Guid, String))
        Property SelectedFolder() As Long
        Property SelectedCommunityID() As Integer
        ReadOnly Property SelectedFolderName() As String
        ReadOnly Property CommunitySelectionLoaded() As Boolean

        Sub InitCommunitySelection()
        Sub InitializeFolderSelector(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowAllFolder As Boolean)
        Sub ShowFoldersList()
        Sub ShowSelectCommunity()
        Sub ShowFileList()
        Sub ShowCompleteMessage()
        Sub ShowRenamedFileList()

        Sub ReturnToFileManagement(ByVal ItemID As System.Guid)

        Function GetChangedFileName() As List(Of dtoFileExist(Of System.Guid))
        Function GetFilesToPublish() As List(Of dtoFileToPublish)

        Sub Notify(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal oItem As CommunityFile, ByVal FolderName As String)
        'Sub AddFileToCommunityAction(ByVal ModuleId As Integer, ByVal CommunityID As Integer, ByVal CommunityFileID As Long)
        'Sub NotifyAddFileToCommunity(ByVal CommunityID As Integer, ByVal CommunityFileID As Long, ByVal FileName As String, ByVal FolderName As String)
    End Interface
End Namespace