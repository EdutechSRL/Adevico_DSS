Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IWKworkBookList
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
        ReadOnly Property PreLoadedView() As WorkBookTypeFilter
        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedPageIndex() As Integer
        ReadOnly Property PreLoadedCommunityFilter() As WorkBookCommunityFilter
        ReadOnly Property PreLoadedOrder() As WorkBookOrder


        ReadOnly Property DefaultPageSize() As Integer
        Property CurrentView() As WorkBookTypeFilter
        Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase
        Property CurrentPageSize() As Integer
        Property CurrentCommunityFilter() As WorkBookCommunityFilter
        Property CurrentOrder() As WorkBookOrder
        ReadOnly Property GetEditingTranslation(ByVal Translation As Integer) As String


        Property AllowChangeStatusEditing() As Boolean
        ReadOnly Property GetPortalName() As String
        ReadOnly Property GetAllCommunitiesName() As String

        WriteOnly Property AllowCreateWorkBook() As AllowCreation
        Sub ShowErrorView()
        Sub ShowListView()
        Sub LoadWorkBooks(ByVal oList As List(Of dtoWorkbooks))
        Sub LoadAvailableView(ByVal oList As List(Of dtoWorkBookListView))
        Sub LoadAvailableFilters(ByVal oList As List(Of WorkBookCommunityFilter))
        Sub LoadAvailableOrderBy(ByVal oList As List(Of WorkBookOrder))
        Sub NoPermissionToAccessPage()
        Sub NavigationUrl(ByVal oContext As WorkBookContext)
        Function GetItemsStatusEditing() As List(Of dtoItemStatusEditing)


        Sub NotifyPersonalVirtualDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyCommunityVirtualDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyPersonalVirtualUnDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyCommunityVirtualUnDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))

        Sub NotifyPersonalDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyCommunityDelete(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))

        Enum AllowCreation
            None = 0
            Current = 1
            ForOther = 2
        End Enum
    End Interface
End Namespace
