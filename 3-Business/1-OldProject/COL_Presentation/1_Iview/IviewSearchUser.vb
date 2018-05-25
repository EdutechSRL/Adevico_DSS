Imports COL_BusinessLogic_v2.UCServices

Public Interface IviewSearchUser

    ReadOnly Property Name() As String
    ReadOnly Property Surname() As String
    ReadOnly Property MailAddress() As String
    ReadOnly Property Login() As String
    ReadOnly Property SelectedRoleId() As Integer
    ReadOnly Property PreviewUserList_isVisible() As Boolean
    ReadOnly Property CurrentUserId() As Integer

    Property OrderBy() As String
    Property showMail() As Boolean
    Property Direction() As Comol.Entity.sortDirection
    Property GridPageSize() As Integer
    Property GridCurrentPage() As Integer
    Property GridMaxPage() As Integer

    ReadOnly Property CurrentPresenter() As PresenterSearchUserByCommunities
    ReadOnly Property CurrentLanguage() As Lingua

    Property SelectionMode() As ListSelectionMode

    Sub init(ByRef oRoleList As List(Of Role))

    Function GetUsers() As List(Of BaseElement)
    Sub BindRolesByCommunities(ByRef oRoleList As List(Of Role))
    Sub BindPreview(ByRef oUserList As List(Of MemberContact))
    Sub BindSearchResult(ByRef oUserList As List(Of MemberContact))

    Property SelectedCommunitiesId() As List(Of Integer)


End Interface
