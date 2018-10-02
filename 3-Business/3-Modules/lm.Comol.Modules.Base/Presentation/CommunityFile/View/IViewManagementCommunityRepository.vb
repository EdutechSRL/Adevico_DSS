Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IViewManagementCommunityRepository
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        WriteOnly Property AllowAddItem(ByVal FolderID As Long, ByVal CommunityID As Integer) As Boolean
        'WriteOnly Property AllowAdvancedManagement(ByVal CommunityID As Integer, ByVal PageIndex As Integer) As Boolean
        WriteOnly Property AllowDownload(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer) As Boolean
        WriteOnly Property AllowImport(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer) As Boolean
        WriteOnly Property AllowMultipleDelete(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal PageIndex As Integer) As Boolean

        ReadOnly Property Portalname() As String
        WriteOnly Property TitleCommunity() As String
        WriteOnly Property AllowHideItems() As Boolean
        ReadOnly Property AllowFolderChange() As Boolean

        ReadOnly Property PreLoadedPageIndex() As Integer
        ReadOnly Property PreLoadedFolder() As Long
        ReadOnly Property PreLoadedCommunityID() As Integer
        ReadOnly Property DefaultPageSize() As Integer
        ReadOnly Property PreLoadedView() As IViewExploreCommunityRepository.ViewRepository
        ReadOnly Property BaseFolder() As String

        Property Ascending() As Boolean
        Property OrderBy() As CommunityFileOrder
        Property CurrentPageSize() As Integer
        Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase
        Property CurrentView() As IViewExploreCommunityRepository.ViewRepository
        Property RepositoryFolderID() As Long
        Property SelectedDestinationFolderID() As Long
        Property RepositoryCommunityID() As Integer
        Property RepositoryItemID() As Long
        Property RepositoryModuleID() As Integer
        Property ShowDescription As Boolean

        Sub LoadFolder(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)
        Sub LoadFolderContent(ByVal oList As List(Of dtoCommunityItemRepository))
        Sub UpdatePathSelector(ByVal oList As List(Of FilterElement), ByVal CommunityID As Integer)
        Sub UpdateFolderTree(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)
        Sub DefineUrlSelector(ByVal FolderID As Long, ByVal CommunityID As Integer)
        Sub NoPermission(ByVal CommunityID As Integer)


        Sub LoadFolderSelector(ByVal ExludeFolderID As Long, ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)
        Sub ShowFolderDoesntExist(ByVal FolderName As String)
        Sub ShowFileNameExist(ByVal FolderParentName As String, ByVal FileName As String)
        Sub ShowFolderExist(ByVal FolderParentName As String, ByVal FileName As String)
        Sub ShowConfirmFolder(ByVal OldFolder As String, ByVal NewFolder As String, ByVal ItemName As String)
        Sub ItemFolderChanged()


        Sub NotifyFileDeleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal FolderFatherID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal Folder As String, ByVal type As Repository.RepositoryItemType)
        Sub NotifyFolderDeleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal FolderFatherID As Long, ByVal FolderID As Long, ByVal Folder As String, ByVal ParentFolder As String)
        Sub NotifyVisibilityChange(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemName As String, ByVal ItemID As Long, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal UniqueID As System.Guid, ByVal isVisible As Boolean, ByVal type As Repository.RepositoryItemType)

        'Sub NotifyFileMoved(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String)
        Sub NotifyItemFileMoved(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal FolderID As Long, ByVal FileID As Long, ByVal UniqueID As System.Guid, ByVal FileName As String, ByVal FromFolder As String, ByVal ToFolder As String, ByVal type As Repository.RepositoryItemType)
        Sub NotifyFolderMoved(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal FolderID As Long, ByVal ItemID As Long, ByVal Folder As String, ByVal FromFolder As String, ByVal ToFolder As String)

        Sub LoadRepositoryAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
    End Interface
End Namespace