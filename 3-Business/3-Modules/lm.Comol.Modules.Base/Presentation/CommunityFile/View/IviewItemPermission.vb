Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewItemPermission
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository
        ReadOnly Property PreLoadedPage() As RepositoryPage
        ReadOnly Property PreloadedItemID() As Long
        ReadOnly Property PreloadedFolderID() As Long
        ReadOnly Property PreloadedCommunityID() As Long
        ReadOnly Property PreloadedPreserveUrl() As Boolean
        ReadOnly Property PreloadedAction() As PermissionAction
        ReadOnly Property ForService() As String
        ReadOnly Property BaseFolder() As String
        ReadOnly Property PreloadedRemoteItemID() As String
        Property RemoteItemID() As String
        WriteOnly Property FolderName() As String
        WriteOnly Property FileName() As String
        WriteOnly Property SessionUniqueID() As System.Guid
        WriteOnly Property AskToApplyToAllSubItems() As Boolean


        Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub NoPermissionToEdit(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean)
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository

        Property AllMembers() As Boolean
        Property SelectedRoles() As List(Of FilterElement)
        ReadOnly Property SelectedRolesID() As List(Of Integer)
        Property SelectedMembers() As List(Of dtoMember(Of Integer))
        ReadOnly Property SelectedMembersID() As List(Of Integer)
        Sub Initialize(ByVal oRoles As List(Of FilterElement), ByVal oPersons As List(Of dtoMember(Of Integer)), ByVal ForAll As Boolean)
        Sub InitializeAvailableRoles(ByVal oRoles As List(Of FilterElement), ByVal SelectedID As List(Of Integer))
        Sub InitializeMembersSelection(ByVal CommunityID As Integer)
        Sub UpdateSelectRoles(ByVal oRoles As List(Of FilterElement))
        Sub UpdateSelecteMembers(ByVal oMembers As List(Of dtoMember(Of Integer)))

        Property RepositoryItemID() As Long
        Property ItemCommunityID() As Integer
        ReadOnly Property PreloadedMultipleItemsID() As List(Of Long)
        ReadOnly Property PreloadedMultipleItemsName() As List(Of String)
        Property isSetPermissionForMultipleFile() As Boolean

        WriteOnly Property FilesName() As List(Of String)
        Sub SavePreservedUrl()


        Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage)
        Sub SetBackUrl(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage)
        Sub SetBackUrlToPrevious()
        Property PreservedUrl() As String

        WriteOnly Property AllowSave() As Boolean

        Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean)
        Sub SendActionCompleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ForFile As Boolean)
        Sub NotifyPermissionChanged(ByVal ToallCommunity As Boolean, ByVal OwnerID As Integer, ByVal IdCommunity As Integer, ByVal IdItem As Long, ByVal ItemName As String, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal uniqueId As System.Guid, ByVal isVisible As Boolean, type As Repository.RepositoryItemType)
    End Interface
End Namespace