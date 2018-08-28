Public Class CachePolicySearch
    Private Const _SearchRole As String = "RoleSearch{0}{1}"
    Private Const _SearchRoleAvailable As String = "RoleSearchAvailable{0}{1}"
    Private Const _TemporaryUserIdList As String = "TemporaryUserIdList_{0}"
    Private Const _UserByCommunities As String = "UserSearchByCom_{0}"

    Public Shared Function SearchRole() As String
        Return String.Format(_SearchRole, "", "")
    End Function
    Public Shared Function SearchRole(ByRef LinguaId) As String
        Return String.Format(_SearchRole, LinguaId, "")
    End Function
    Public Shared Function SearchRole(ByVal LinguaID As Integer, ByVal ListID As List(Of Integer)) As String
        Return String.Format(_SearchRole, LinguaID, CompositeCommunityID(ListID))
    End Function
    Public Shared Function SearchRoleAvailable(ByVal LinguaID As Integer, ByVal ListID As List(Of Integer)) As String
        Return String.Format(_SearchRoleAvailable, LinguaID, CompositeCommunityID(ListID))
    End Function
    Public Shared Function TemporaryUserIdList(ByRef CurrentUserId) As String
        Return String.Format(_TemporaryUserIdList, CurrentUserId)
    End Function
    Public Shared Function SearchUserByCommunitiesId(ByRef oCommunitiesIdList As List(Of Integer))
        Return String.Format(_UserByCommunities, CompositeCommunityID(oCommunitiesIdList))
    End Function

    Private Shared Function CompositeCommunityID(ByVal ListCommunityID As List(Of Integer)) As String
        Dim composite As String = String.Empty
        If Not ListCommunityID Is Nothing Then
            For Each id As Integer In ListCommunityID
                composite &= id.ToString & "_"
            Next
        End If
        Return composite
    End Function

End Class


