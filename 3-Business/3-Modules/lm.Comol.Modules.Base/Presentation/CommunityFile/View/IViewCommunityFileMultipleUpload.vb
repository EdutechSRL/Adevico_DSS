Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IViewCommunityFileMultipleUpload
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        ReadOnly Property PreloadedFolderID() As Long
        ReadOnly Property PreloadedCommunityID() As Integer
        ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository
        ReadOnly Property PreLoadedPage() As RepositoryPage
        Property RepositoryCommunityID() As Integer
        WriteOnly Property AllowBackToManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal View As IViewExploreCommunityRepository.ViewRepository) As Boolean
        WriteOnly Property AllowBackToDownloads(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal View As IViewExploreCommunityRepository.ViewRepository) As Boolean

        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub NoPermissionToAdd(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub InitializeUploader(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository)

        Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage)
        Sub ActionInitialize(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub ActionUpload(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal FileID As Long)
    End Interface
End Namespace