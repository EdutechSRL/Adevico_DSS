Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewMultipleDelete
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        WriteOnly Property AllowManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean
        WriteOnly Property AllowMultipleDelete() As Boolean

        ReadOnly Property Portalname() As String
        WriteOnly Property TitleCommunity() As String
        ReadOnly Property PreLoadedPageIndex() As Integer
        ReadOnly Property PreLoadedFolder() As Long
        ReadOnly Property PreLoadedCommunityID() As Integer
        ReadOnly Property PreLoadedView() As IViewExploreCommunityRepository.ViewRepository
        Property RepositoryCommunityID() As Integer

        Sub NotifyDeletedItems(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal oList As List(Of dtoDeletedItem))
        Sub InitializeFileSelector(ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)
        Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage)
        Sub NoPermission(ByVal CommunityID As Integer)
        Sub NoPermissionToDelete(ByVal CommunityID As Integer)
    End Interface
End Namespace