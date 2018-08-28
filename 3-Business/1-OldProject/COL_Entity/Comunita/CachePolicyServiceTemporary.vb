Public Class CachePolicyServiceTemporary

#Region "Const"
    Private Const _PlainServiceSmall As String = "PlainServiceSmall"
    Private Const _SystemTranslated As String = "SystemTranslated{0}"
    Private Const _ListTranslated As String = "PlainServiceListTranslated{0}{1}"
    Private Const _ListTranslatedRole As String = "PlainServiceListTranslatedRole{0}{1}{2}"
#End Region

#Region "Subscription"
    Public Shared Function PlainServiceSmall() As String
        Return String.Format(_PlainServiceSmall)
    End Function
    Public Shared Function SystemTranslated() As String
        Return String.Format(_SystemTranslated, "")
    End Function
    Public Shared Function SystemTranslated(ByVal LanguageID As Integer) As String
        Return String.Format(_SystemTranslated, "_" & LanguageID)
    End Function
    Public Shared Function ListTranslated() As String
        Return String.Format(_ListTranslated, "", "")
    End Function
    Public Shared Function ListTranslated(ByVal CommunityID As Integer) As String
        Return String.Format(_ListTranslated, "_" & CommunityID, "_")
    End Function
    Public Shared Function ListCommunityTranslated(ByVal CommunityID As Integer, ByVal LanguageID As Integer) As String
        Return String.Format(_ListTranslated, "_" & CommunityID, "_" & LanguageID)
    End Function
    Public Shared Function ListTranslatedRole() As String
        Return String.Format(_ListTranslatedRole, "_", "", "")
    End Function
    Public Shared Function ListTranslatedRole(ByVal CommunityID As Integer) As String
        Return String.Format(_ListTranslatedRole, "_" & CommunityID, "", "")
    End Function
    Public Shared Function ListTranslatedRole(ByVal CommunityID As Integer, ByVal RoleID As Integer) As String
        Return String.Format(_ListTranslatedRole, "_" & CommunityID, "_" & RoleID, "_")
    End Function
    Public Shared Function ListTranslatedRole(ByVal CommunityID As Integer, ByVal RoleID As Integer, ByVal LanguageID As Integer) As String
        Return String.Format(_ListTranslatedRole, "_" & CommunityID, "_" & RoleID, "_" & LanguageID)
    End Function
#End Region
End Class
