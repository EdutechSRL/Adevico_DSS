Imports COL_BusinessLogic_v2.UCServices

Public Interface IviewSearchCommunity
	'Property SelectedCommunity() As CommunityType
	Property OrderBy() As String
	Property Direction() As Comol.Entity.sortDirection
	Property GridPageSize() As Integer
	Property GridCurrentPage() As Integer
	Property GridMaxPage() As Integer
	ReadOnly Property CurrentPresenter() As PresenterSearchCommunityByService
	ReadOnly Property CurrentLanguage() As Lingua
	ReadOnly Property CurrentSubscriber() As Person
	ReadOnly Property CurrentCommunity() As Community

    ReadOnly Property CurrentOrganization() As Organization
    Property CurrentCommunityType() As CommunityType
	ReadOnly Property CurrentStatus() As CommunityStatus
	ReadOnly Property CurrentSearch() As StandardCommunitySearch
	ReadOnly Property CurrentResponsibleID() As Integer
	Property SearchBy() As String

	'Property SelectedCommunity() As Community
    Property SelectedCommunitiesID() As List(Of Integer)
    Property SelectedCommunities() As Dictionary(Of Integer, String)
	Property AutoUpdateList() As Boolean
	Property SelectionMode() As ListSelectionMode

	Sub LoadOrganizations(ByVal oList As IList)
	Sub LoadCommunityTypes(ByVal oList As IList)
    Sub LoadStatus(ByVal oList As IList)
    Sub LoadCommunities(ByVal oList As IList)

	Property AllowMultipleOrganizationSelection() As Boolean
	Property AllowCommunityChangedEvent() As Boolean
	Property ServiceClauses() As GenericClause(Of ServiceClause)
    Property HasCommunity() As Boolean
    Property ExludeCommunities() As List(Of Integer)
	'Property 
End Interface