Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewWorkBookItem
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter
        ReadOnly Property PreloadedWorkBookID() As System.Guid
        ReadOnly Property PreloadedItemID() As System.Guid
        ReadOnly Property PreloadedIsInsertMode() As Boolean
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        ReadOnly Property AllowPublish() As Boolean

        WriteOnly Property AllowEditingChanging() As Boolean
        Sub SetEditing(ByVal oAvailableEditing As List(Of TranslatedItem(Of Integer)), ByVal ItemEditing As EditingPermission)

        ReadOnly Property CurrentItem() As WorkBookItem
        WriteOnly Property AllowEdit() As Boolean
        WriteOnly Property AllowFileManagement() As Boolean
        Property AllowStatusChange() As Boolean
        WriteOnly Property ShowDraftMode() As Boolean
        WriteOnly Property SetStatus() As MetaApprovationStatus
        Sub LoadItem(ByVal oItem As WorkBookItem, ByVal oAvailableStatus As List(Of TranslatedItem(Of Integer)))
        Sub SetBackToWorkbookURL(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter)
        Sub SendToItemsList(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter, ByVal GoToItemId As System.Guid)
        Sub SendToFileManagement(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter)

        Sub LoadFilesToManage(ByVal ItemID As System.Guid, ByVal CommunityID As Integer, ByVal AllowManagement As Boolean, ByVal oPermission As WorkBookItemPermission, ByVal oModule As ModuleCommunityRepository, ByVal AllowPublish As Boolean)
        Sub NoPermission(ByVal CommunityID As Integer)
        Sub NoItemWithThisID(ByVal CommunityID As Integer)
        Sub NotifyAdd(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyEdit(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub SendAddAction(ByVal ItemID As System.Guid)
        Sub SendEditAction(ByVal ItemID As System.Guid)
        ReadOnly Property GetEditingTranslation(ByVal Permissions As Integer) As String
        ReadOnly Property GetEditingTranslationOwner(ByVal isOwner As Boolean, ByVal Permissions As Integer) As String
    End Interface
End Namespace