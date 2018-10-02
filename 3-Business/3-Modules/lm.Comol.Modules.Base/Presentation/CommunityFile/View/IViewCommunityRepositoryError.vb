Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IViewCommunityRepositoryError
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreloadedForUserID() As Integer
        ReadOnly Property PreloadedItemID() As Long
        ReadOnly Property PreloadedFolderID() As Long
        ReadOnly Property PreloadedCommunityID() As Integer
        ReadOnly Property PreloadedCreate() As ItemRepositoryToCreate
        ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository
        ReadOnly Property PreloadedPreviousPage() As RepositoryPage
        ReadOnly Property PreloadedPageSender() As RepositoryPage
        ReadOnly Property PreserveDownloadUrl() As Boolean
        ReadOnly Property PreloadedFirstError() As ItemRepositoryStatus
        ReadOnly Property PreloadedLanguageCode() As String
        Property CurrentForUserID() As Integer
        Property CurrentCommunityID() As Integer
        Property CurrentFolderID() As Long
        Property PreservedDownloadUrl() As String
        Property CurrentItemID As Long
        Property CurrentFirstError As ItemRepositoryStatus
        Property CurrentItemRepositoryToCreate As ItemRepositoryToCreate
        ReadOnly Property FilesToProcess() As List(Of dtoRemoteFileToRename)

        Sub SaveFilesToProcess()
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        Sub LoadRepositoryPage(ByVal FolderID As Long, ByVal CommunityID As Integer)
        Sub LoadStandardLoginPage()
        Sub LoadInternalLoginPage(ByVal idCommunity As Integer, ByVal idPerson As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String)
        Sub LoadShibbolethLoginPage(ByVal idCommunity As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String)
        Sub LoadExternalProviderPage(ByVal remoteUrl As String, ByVal idCommunity As Integer, ByVal idPerson As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String)

        Sub LoadLanguage(ByVal oLanguage As Language)
    End Interface
End Namespace