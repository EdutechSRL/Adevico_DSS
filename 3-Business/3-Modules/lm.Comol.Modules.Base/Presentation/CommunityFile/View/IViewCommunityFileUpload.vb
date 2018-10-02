Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IViewCommunityFileUpload
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        ReadOnly Property PreloadedFolderID() As Long
        ReadOnly Property PreloadedCommunityID() As Integer
        ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository
        ReadOnly Property PreloadedCreate() As ItemRepositoryToCreate
        ReadOnly Property PreLoadedPage() As RepositoryPage
        Property RepositoryCommunityID() As Integer
        WriteOnly Property AllowBackToManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal View As IViewExploreCommunityRepository.ViewRepository) As Boolean
        WriteOnly Property AllowBackToDownloads(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal View As IViewExploreCommunityRepository.ViewRepository) As Boolean

        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        Sub NoPermission(ByVal CommunityID As Integer)
        Sub NoPermissionToAdd(ByVal CommunityID As Integer)
        Sub InitializeUploader(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ForCreate As ItemRepositoryToCreate, ByVal oPermission As ModuleCommunityRepository)

        Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage)
        Sub LoadEditingPermission(ByVal ItemID As Long, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage)

        Sub ActionInitialize(ByVal CommunityID As Integer)
        Sub ActionUpload(ByVal CommunityID As Integer, ByVal FileID As Long)
        Sub ActionCreateFolder(ByVal CommunityID As Integer, ByVal FolderID As Long)
    End Interface
End Namespace