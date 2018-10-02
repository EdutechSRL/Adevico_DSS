Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common


Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IViewProfileManagement
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedPageIndex() As Integer
        ReadOnly Property PreLoadedOrder() As ProfileOrder
        ReadOnly Property DefaultPageSize() As Integer
        Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase
        Property CurrentPageSize() As Integer
        Property CurrentOrder() As ProfileOrder
        Property CurrentOrganizationID() As Integer
        Property CurrentProfileTypeID() As Integer
        Property CurrentStatus() As ProfileStatus
        Property CurrentAuthenticationTypeID() As Integer
        Property CurrentSearchFor() As ProfileSearchBy
        Property CurrentValue() As String
        Property CurrentStartWith() As String
        Property SavedFilters() As dtoProfileFilters


        ReadOnly Property GetStatusTranslation(ByVal status As ProfileStatus) As String
        ReadOnly Property GetSearchByTranslation(ByVal status As ProfileSearchBy) As String



        Sub ShowErrorView()
        Sub ShowListView()

        'Sub LoadWorkBooks(ByVal oList As List(Of dtoWorkbooks))
        'Sub LoadAvailableView(ByVal oList As List(Of dtoWorkBookListView))
        'Sub LoadAvailableFilters(ByVal oList As List(Of WorkBookCommunityFilter))
        'Sub LoadAvailableOrderBy(ByVal oList As List(Of WorkBookOrder))

        Sub NoPermissionToAccessPage()
        Sub NavigationUrl(ByVal oContext As WorkBookContext)
    End Interface
End Namespace