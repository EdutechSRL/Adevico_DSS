Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IWKitemsList
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property AllowAddItem() As Boolean
        Property AllowPrint() As Boolean
        Property AllowChangeApprovation() As Boolean
        Property AllowMultipleDelete() As Boolean
        Property AllowItemsSelection() As Boolean
        Property WorkBookModuleID() As Integer
        Property WorkBookCommunityID() As Integer

        ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter
        ReadOnly Property PreloadedWorkBookID() As System.Guid
        ReadOnly Property PreloadedAscending() As Boolean
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        Property DisplayOrderAscending() As Boolean
        Property SelectedItems() As List(Of System.Guid)
        Function GetItemsStatusEditing() As List(Of dtoItemStatusEditing)

        Sub LoadItems(ByVal oList As List(Of dtoWorkBookItem))
        Sub SetAddItemUrl(ByVal WorkBookID As System.Guid)
        Sub SetPrintItemUrl(ByVal WorkBookID As System.Guid, ByVal Ascending As Boolean)
        Sub SetWorkBookManagementItemUrl(ByVal oView As WorkBookTypeFilter)
        Sub SetItemsListUrl(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter)
        Sub ReturnToWorkBookManagement(ByVal oView As WorkBookTypeFilter)
        Sub SetRedirectToItemList(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter, ByVal Ascending As Boolean)
        ReadOnly Property GetEditingTranslation(ByVal Translation As Integer) As String
        ReadOnly Property GetEditingTranslationOwner(ByVal isOwner As Boolean, ByVal Permissions As Integer) As String
        Sub NoPermissionToViewItems()
        Sub NoWorkBookWithThisID()
        Sub NoWorkBookItemWithThisID()

        Sub NotifyEdit(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyVirtualDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyVirtualUnDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyDelete(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))

        Sub SendActionList(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid)
        'Sub SendActionAdd(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid)
        'Sub SendActionEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid)
        Sub SendActionVirtualDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid)
        Sub SendActionVirtualUnDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid)
        Sub SendActionDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookItemID As System.Guid)
    End Interface
End Namespace