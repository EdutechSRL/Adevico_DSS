Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IviewImportItemsIntoRepository
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        WriteOnly Property AllowCommunityImport() As Boolean
        WriteOnly Property AllowManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean
        ReadOnly Property Portalname() As String
        ReadOnly Property BaseFolder() As String
        WriteOnly Property TitleCommunity() As String

        ReadOnly Property PreLoadedPageIndex() As Integer
        ReadOnly Property PreLoadedFolderID() As Long
        ReadOnly Property PreLoadedCommunityID() As Integer
        ReadOnly Property PreLoadedView() As IViewExploreCommunityRepository.ViewRepository
        Property DestinationCommunityID() As Integer
        WriteOnly Property DestinationCommunityName() As String
        Property VisibleToAll As Boolean
        Property SourceCommunityID() As Integer
        WriteOnly Property SourceCommunityName() As String
        WriteOnly Property FilePath() As String

        Sub NoPermissionToManagementFiles(ByVal CommunityID As Integer)
        Sub NoPermissionToImportItems(ByVal CommunityID As Integer)
        Sub NoPermissionToAccessPage(ByVal CommunityID As Integer)

        Sub InitializeSourceItemsSelector(ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)


        Sub LoadMultipleFileName(ByVal oList As List(Of dtoFileExist(Of Long)))
        Property SelectedFolder() As Long

        ReadOnly Property SelectedFolderName() As String
        ReadOnly Property CommunitySelectionLoaded() As Boolean

        Sub InitCommunitySelection(ByVal CommunityID As Integer)
        Sub InitializeFolderSelector(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowAllFolder As Boolean)
        Sub ShowFoldersList()
        Sub ShowSelectCommunity()
        Sub ShowFileList()
        Sub ShowRenamedFileList()

        Sub ReturnToFileManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository)

        Function GetChangedFileName() As List(Of dtoFileExist(Of Long))

        Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub SendActionImportCompleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub NotifyImportedItems(ByVal CommunityID As Integer, ByVal oContext As dtoImportedItem)
    End Interface
End Namespace