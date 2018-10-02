Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IViewExploreCommunityRepository
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        WriteOnly Property AllowUploadFile(ByVal FolderID As Long, ByVal CommunityID As Integer) As Boolean
        WriteOnly Property AllowManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer) As Boolean
        ReadOnly Property PreLoadedPageIndex() As Integer
        ReadOnly Property PreLoadedFolder() As Long
        ReadOnly Property PreLoadedCommunityID() As Integer

        ReadOnly Property DefaultPageSize() As Integer
        ReadOnly Property PreLoadedView() As ViewRepository
        ReadOnly Property Portalname() As String
        WriteOnly Property TitleCommunity() As String

        Property RepositoryCommunityID() As Integer
        Property RepositoryModuleID() As Integer

        Property ShowDescription() As Boolean
        Property Ascending() As Boolean
        Property OrderBy() As CommunityFileOrder
        Property CurrentPageSize() As Integer
        Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase
        Property CurrentView() As ViewRepository
        Sub LoadFolder(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)
        Sub LoadFolderContent(ByVal oList As List(Of dtoCommunityItemRepository))
        Sub UpdatePathSelector(ByVal oList As List(Of FilterElement), ByVal CommunityID As Integer)
        Sub DefineUrlSelector(ByVal FolderID As Long, ByVal CommunityID As Integer)
        Sub NoPermission(ByVal CommunityID As Integer)

        Enum ViewRepository
            None = -1
            FileList = 0
            FolderList = 1
        End Enum
        Sub LoadRepositoryAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
    End Interface
End Namespace